using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;
using System.Windows.Forms;
using TvdbLib;
using TvdbLib.Data;
using System.IO;
using TvdbLib.Cache;
using System.Net;
using Newtonsoft.Json;
using Microsoft.CSharp;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TV_Show_Renamer_Server
{
	class thexem
	{
		ICacheProvider m_cacheProvider = null;
		TvdbHandler m_tvdbHandler = null;
		//List<SearchInfo> selectionList = new List<SearchInfo>();

		string folder = null;

		public thexem(string newFolder)
		{
			folder = newFolder;
			m_cacheProvider = new XmlCacheProvider(folder + Path.DirectorySeparatorChar + "Temp");
			m_tvdbHandler = new TvdbHandler(m_cacheProvider, "BC08025A4C3F3D10");
			m_tvdbHandler.InitCache();
		}

		public OnlineShowInfo findTitle(string ShowName, bool showAll = false)
		{
			if (ShowName == null)
				return new OnlineShowInfo();

			List<OnlineShowInfo> FinalList = new List<OnlineShowInfo>();

			try
			{

			List<TvdbSearchResult> list = m_tvdbHandler.SearchSeries(ShowName);
			if (list != null && list.Count > 0)
			{
				for (int i = 0; i < list.Count(); i++)
				{
					if (list[i].Id != 0)
					{
						bool ended = (getStatus(list[i].Id) == "Ended") ? true : false;
						FinalList.Add(new OnlineShowInfo(list[i].SeriesName, list[i].Id, list[i].FirstAired.ToString("yyyy"), ended));

						bool m = Regex.IsMatch(list[i].SeriesName, @"\(\d{1,4}\)", RegexOptions.IgnoreCase);
						if (m)
							showAll = true;

					}
				}
			}
			else
				return new OnlineShowInfo();

			if (FinalList != null && FinalList.Count > 0)
			{
				if (FinalList.Count() == 0) return new OnlineShowInfo();  //return if nothing found

				if (FinalList.Count() == 1)
				{
					return FinalList[0];
				}
				else
				{
					if (FinalList.Count() != 0)
					{
						int indexofTVshow = -1;
						int difference = Math.Abs(removeSymbols(FinalList[0].ShowName).Length - removeSymbols(ShowName).Length);
						indexofTVshow = removeSymbols(FinalList[0].ShowName).IndexOf(removeSymbols(ShowName), StringComparison.InvariantCultureIgnoreCase);
						if (indexofTVshow != -1 && difference < 3 && !showAll)
						{
							return FinalList[0];
							//selectedTitle = FinalList[0].ShowName;
						}
						else
						{
							SelectMenu SelectMain = new SelectMenu(FinalList, ShowName, "Select Correct TVDB Show");
							if (SelectMain.ShowDialog() == DialogResult.OK)
							{
								int selectedid = SelectMain.selected;
								if (selectedid == -1) return new OnlineShowInfo();
								//selectedShow = FinalList[selectedid];
								//selectedTitle = FinalList[selectedid].Title;
								SelectMain.Close();
								return FinalList[selectedid];
							}
						}
					}
				}
			}

			}
			catch (Exception e)
			{
				// MessageBox.Show(e.Message.ToString());
			}

			return new OnlineShowInfo();
		}
	 
		public string getTitle(int seriesID, int season, int episode)
		{
			string newTitle = "";

			try
			{
				if (season > 100)
				{
					TvdbEpisode e = m_tvdbHandler.GetEpisode(seriesID, new DateTime(episode, season / 100, season % 100), TvdbLanguage.DefaultLanguage);
					newTitle = e.EpisodeName;
				}
				else
				{
					int seasonScale = season;
					int episodeScale = episode;
					using (var client = new WebClient())
					{
						var json = client.DownloadString(String.Format("http://thexem.de/map/all?id={0}&origin=tvdb", seriesID));

						RootObject m = JsonConvert.DeserializeObject<RootObject>(json);

						foreach (Datum showData in m.data)
						{
							if (showData.scene.episode == episode && showData.scene.season == season)
							{
								seasonScale = showData.tvdb.season;
								episodeScale = showData.tvdb.episode;
								break;
							}
						}
					}
					//if (seasonScale == 0 && episodeScale == 0)
					//	return "";

					TvdbSeries s = m_tvdbHandler.GetSeries(seriesID, TvdbLanguage.DefaultLanguage, true, false, false);
					foreach (TvdbEpisode esp in s.Episodes)
					{
						if (seasonScale == esp.SeasonNumber && episodeScale == esp.EpisodeNumber)
						{
							newTitle = esp.EpisodeName;
							break;
						}
					}
				}
			}
			catch (Exception e ) 
			{
				MessageBox.Show(e.Message.ToString());
			}

			newTitle = newTitle.Replace(":", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("\"", "");
			return newTitle;
		}

		public string getStatus(int seriesID)
		{
			try
			{
				TvdbSeries s = m_tvdbHandler.GetSeries(seriesID, TvdbLanguage.DefaultLanguage, true, false, false);
				return s.Status;
			}
			catch (Exception) { }

			return "";
		}


		public SeriesData GetAllSeasonData(int seriesID)
		{
			SeriesData data = new SeriesData();
			try
			{
				TvdbSeries s = m_tvdbHandler.GetSeries(seriesID, TvdbLanguage.DefaultLanguage, true, false, false);
				foreach (TvdbEpisode esp in s.Episodes)
				{
					if (!data.ContainsSeason(esp.SeasonNumber))
					{
						SeasonData newSeason = new SeasonData(esp.SeasonNumber);
						newSeason.EpisodeList.Add(new EpisodeData(esp.EpisodeName, esp.SeasonNumber, esp.EpisodeNumber));
						data.SeasonList.Add(newSeason);
					}
					else
					{
						SeasonData newSeason = data.GetSeason(esp.SeasonNumber);
						if (newSeason.ContainsEpisode(esp.EpisodeNumber))
							continue;
						newSeason.EpisodeList.Add(new EpisodeData(esp.EpisodeName, esp.SeasonNumber, esp.EpisodeNumber));
					}
				}
			}
			catch (Exception) { }
			return data;
		}

		public SeasonData GetSeasonData(int seriesID, int season)
		{
			SeasonData data = new SeasonData();
			try
			{
				TvdbSeries s = m_tvdbHandler.GetSeries(seriesID, TvdbLanguage.DefaultLanguage, true, false, false);
				foreach (TvdbEpisode esp in s.Episodes)
				{
					if (season == esp.SeasonNumber)
					{
						if (data.ContainsEpisode(esp.EpisodeNumber))
							continue;
						data.EpisodeList.Add(new EpisodeData(esp.EpisodeName, esp.SeasonNumber, esp.EpisodeNumber));
					}
				}
			}
			catch (Exception) { }
			return data;
		}

		public EpisodeData GetEpisodeData(int seriesID, int season, int episode)
		{
			//EpisodeData data = new EpisodeData();
			try
			{
				TvdbSeries s = m_tvdbHandler.GetSeries(seriesID, TvdbLanguage.DefaultLanguage, true, false, false);
				foreach (TvdbEpisode esp in s.Episodes)
				{
					if (season == esp.SeasonNumber && episode == esp.EpisodeNumber)
					{
						return new EpisodeData(esp.EpisodeName, esp.SeasonNumber, esp.EpisodeNumber);
					}
				}
			}
			catch (Exception) { }
			return new EpisodeData();
		}

		string removeSymbols(string word)
		{
			char[] arr = word.ToCharArray();

			arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
											  || char.IsWhiteSpace(c)
											  )));
			return new string(arr);

		}
	}

	public class Scene
	{
		public int season { get; set; }
		public int episode { get; set; }
		public int absolute { get; set; }
	}

	public class Tvdb
	{
		public int season { get; set; }
		public int episode { get; set; }
		public int absolute { get; set; }
	}

	public class Tvdb2
	{
		public int season { get; set; }
		public int episode { get; set; }
		public int absolute { get; set; }
	}

	public class Rage
	{
		public int season { get; set; }
		public int episode { get; set; }
		public int absolute { get; set; }
	}

	public class Trakt
	{
		public int season { get; set; }
		public int episode { get; set; }
		public int absolute { get; set; }
	}

	public class Trakt2
	{
		public int season { get; set; }
		public int episode { get; set; }
		public int absolute { get; set; }
	}

	public class Anidb
	{
		public int season { get; set; }
		public int episode { get; set; }
		public int absolute { get; set; }
	}

	public class Datum
	{
		public Scene scene { get; set; }
		public Tvdb tvdb { get; set; }
		public Tvdb2 tvdb_2 { get; set; }
		public Rage rage { get; set; }
		public Trakt trakt { get; set; }
		public Trakt2 trakt_2 { get; set; }
		public Anidb anidb { get; set; }
	}

	public class RootObject
	{
		public string result { get; set; }
		public List<Datum> data { get; set; }
		public string message { get; set; }
	}
}
