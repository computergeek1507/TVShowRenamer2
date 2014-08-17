using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TvdbLib;
using TvdbLib.Data;
using System.IO;
using TvdbLib.Cache;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TV_Show_Renamer_Server
{
	class NewTVDB
	{
		ICacheProvider m_cacheProvider = null;
		TvdbHandler m_tvdbHandler = null;
		//List<SearchInfo> selectionList = new List<SearchInfo>();
		string folder = null;

		//string tvdbTitle = null;

		public NewTVDB(string newFolder)
		{
			folder = newFolder;
			m_cacheProvider = new XmlCacheProvider(folder+Path.DirectorySeparatorChar + "Temp");
			m_tvdbHandler = new TvdbHandler(m_cacheProvider, "BC08025A4C3F3D10");
			m_tvdbHandler.InitCache();
		}


		public OnlineShowInfo findTitle(string ShowName, bool showAll = false)
		{
			if (ShowName == null)
				return new OnlineShowInfo();

			List<OnlineShowInfo> FinalList = new List<OnlineShowInfo>();


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
							SelectMenu SelectMain = new SelectMenu(FinalList, ShowName,"Select Correct TVDB Show");
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

			return new OnlineShowInfo();
		}

		public string getTitle(int seriesID, int season, int episode)
		{
			string newTitle = null;
			//TvdbEpisode e;
			try
			{
				//if (season > 100) return"";
				if (season > 100)
				{
					TvdbEpisode e = m_tvdbHandler.GetEpisode(seriesID, new DateTime(episode, season / 100, season % 100), TvdbLanguage.DefaultLanguage);
					newTitle = e.EpisodeName;
				}
				else
				{
					TvdbSeries s = m_tvdbHandler.GetSeries(seriesID, TvdbLanguage.DefaultLanguage, true, false, false);
					foreach (TvdbEpisode esp in s.Episodes)
					{
						if (season == esp.SeasonNumber && episode == esp.EpisodeNumber)
						{
							newTitle = esp.EpisodeName;
							break;
						}
					}
				}
				//TvdbSeries s = m_tvdbHandler.GetSeries(seriesID, TvdbLanguage.DefaultLanguage, true, false, false);
				////List<string> epList = new List<string>();

				//foreach (TvdbEpisode esp in s.Episodes)
				//{
				//	if (season == esp.SeasonNumber && episode == esp.EpisodeNumber)
				//	{
				//		newTitle = esp.EpisodeName;
				//		break;
				//	}
				//}
				//newTitle = e.EpisodeName;
			}
			catch (Exception) { }
			
			if (newTitle == null)
				return "";
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

		string removeSymbols(string word) 
		{
			char[] arr = word.ToCharArray();

			arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
											  || char.IsWhiteSpace(c)
											  )));
			return new string(arr);
		
		}
	}

}
