using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;
using System.Windows.Forms;

namespace TV_Show_Renamer_Server
{

	static class TVRage
	{



		public static OnlineShowInfo findTitle(string ShowName, bool showAll = false)
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
				XDocument ShowList = XDocument.Load("http://services.tvrage.com/feeds/search.php?show=" + ShowName);

				var Categorys = from Show in ShowList.Descendants("show")
								select new
								{
									ShowID = Show.Element("showid").Value,
									Name = Show.Element("name").Value,
									YearStarted = Show.Element("started").Value
								};



				foreach (var wd in Categorys)
				{
					FinalList.Add(new OnlineShowInfo(wd.Name, Int32.Parse(wd.ShowID),wd.YearStarted));
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
							SelectMenu SelectMain = new SelectMenu(FinalList, ShowName,"Select Correct TVRage Show");
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
					return TVShowID;   //return if nothing is found
				TVShowID = selectedShow;
			}
			TVShowID.ShowName = TVShowID.ShowName.Replace(":", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("\"", "");
			return TVShowID;
		}

		public static string getTitle(int seriesID, int season, int episode)
		{
			string newTitle = null;

			try
			{
				if (season < 100)
				{
					string seporater = "x0";
					if (episode > 10)
					{
						seporater = "x";
					}
					XDocument EpisodeList = XDocument.Load("http://services.tvrage.com/feeds/episodeinfo.php?sid=" + seriesID.ToString() + "&ep=" + season.ToString() + seporater + episode.ToString());

					var Categorys = from Episode in EpisodeList.Descendants("episode")
									select new
									{
										Title = Episode.Element("title").Value,
									};

					foreach (var wd in Categorys)
					{
						newTitle = wd.Title;
					}
				}
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