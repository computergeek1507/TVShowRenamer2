using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;
using System.Windows.Forms;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Discover;

namespace TV_Show_Renamer_Server
{
	class TMDb
	{
		TMDbClient tmdbClient = null;

		string folder = null;

		public TMDb(string newFolder)
		{
			folder = newFolder;
			tmdbClient = new TMDbClient("18923bd69f2010d75cf0939d617be0f6");
		}

		public OnlineShowInfo findTitle(string ShowName, bool showAll = false)
		{
			List<OnlineShowInfo> selectionList = new List<OnlineShowInfo>();
			
			OnlineShowInfo TVShowID = new OnlineShowInfo();
			if (ShowName == null)
				return TVShowID;
			//ShowName = ShowName.Replace("Gold Rush Alaska", "Gold Rush");
			//ShowName = ShowName.Replace("Tosh 0", "Tosh.0");
			List<OnlineShowInfo> FinalList = new List<OnlineShowInfo>();
			try
			{
				SearchContainer<TvShowBase> results = tmdbClient.SearchTvShow(ShowName);

				// Let's iterate the first few hits
				foreach (TvShowBase result in results.Results.Take(10))
				{
					FinalList.Add(new OnlineShowInfo(result.Name, result.Id, result.FirstAirDate.Value.Year.ToString()));

				}
			}
			catch (Exception e) { }

			if (FinalList != null && FinalList.Count > 0)
			{
				if (FinalList.Count() == 0) return TVShowID;  //return if nothing found
				OnlineShowInfo selectedShow = new OnlineShowInfo();
				if (FinalList.Count() == 1)
				{
					selectedShow = FinalList[0];
				}
				else
				{
					if (FinalList.Count() != 0)
					{
						int indexofTVshow = -1;
						int difference = Math.Abs(FinalList[0].ShowName.Length - ShowName.Length);
						indexofTVshow = FinalList[0].ShowName.IndexOf(ShowName, StringComparison.InvariantCultureIgnoreCase);
						if (indexofTVshow != -1 && difference < 3 && !showAll)
						{
							selectedShow = FinalList[0];
						}
						else
						{
							SelectMenu SelectMain = new SelectMenu(FinalList, ShowName,"Select Correct TMDb Show");
							if (SelectMain.ShowDialog() == DialogResult.OK)
							{
								int selectedid = SelectMain.selected;
								if (selectedid == -1) return TVShowID;
								selectedShow = FinalList[selectedid];
								SelectMain.Close();
							}
						}
					}
				}


				if (selectedShow.ShowID == -1) 
					return TVShowID;	//return if nothing is found
				TVShowID = selectedShow;
			}


			try
			{
				TvShow result = tmdbClient.GetTvShow(TVShowID.ShowID);
				TVShowID.ShowEnded = !result.InProduction;
			}
			catch (Exception e) { }


			TVShowID.ShowName = TVShowID.ShowName.Replace(":", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("\"", "");
			return TVShowID;
		}

		public string getTitle(int seriesID, int season, int episode)
		{
			string newTitle = null;

			try
			{
				TvEpisode results = tmdbClient.GetTvEpisode(seriesID, season, episode);

				newTitle = results.Name;
				
				//newTitle = ;
			}
			catch (Exception) { }

			if (newTitle == null)
				return "";
			newTitle = newTitle.Replace(":", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("\"", "");
			return newTitle;
		}
	}
}