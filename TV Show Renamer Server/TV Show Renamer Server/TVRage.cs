using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;
using System.Windows.Forms;

namespace TV_Show_Renamer_Server
{
	public class TVRage
	{
		List<SearchInfo> selectionList = new List<SearchInfo>();

		public SearchInfo findTitle(string ShowName)
		{
			SearchInfo TVShowID = new SearchInfo();
			if (ShowName == null)
				return TVShowID;
			ShowName = ShowName.Replace("Gold Rush Alaska", "Gold Rush");
			ShowName = ShowName.Replace("Tosh 0", "Tosh.0");
			List<SearchInfo> FinalList = new List<SearchInfo>();

			XDocument ShowList = XDocument.Load("http://services.tvrage.com/feeds/search.php?show=" + ShowName);

			var Categorys = from Show in ShowList.Descendants("show")
							select new
							{
								ShowID = Show.Element("showid").Value,
								Name = Show.Element("name").Value,
							};

			foreach (var wd in Categorys)
			{
				FinalList.Add(new SearchInfo(wd.Name, Int32.Parse(wd.ShowID)));
			}

			if (FinalList != null && FinalList.Count > 0)
			{
				if (FinalList.Count() == 0) return TVShowID;  //return if nothing found
				int selectedSeriesId = -1;
				string selectedTitle = "";
				if (FinalList.Count() == 1)
				{
					selectedSeriesId = FinalList[0].SelectedValue;
					selectedTitle = FinalList[0].Title;
				}
				else
				{
					if (FinalList.Count() != 0)
					{
						SelectMenu SelectMain = new SelectMenu(FinalList, ShowName);
						if (SelectMain.ShowDialog() == DialogResult.OK)
						{
							int selectedid = SelectMain.selected;
							if (selectedid == -1) return TVShowID;
							selectedSeriesId = FinalList[selectedid].SelectedValue;
							selectedTitle = FinalList[selectedid].Title;
							SelectMain.Close();
						}
						else
						{
							int idNumber = -1;
							foreach (SearchInfo testIdem in selectionList)
							{
								if (testIdem.Title == ShowName)
								{
									idNumber = testIdem.SelectedValue;
									break;
								}
							}
							if (idNumber == -1)
							{
								SelectMenu SelectMain2 = new SelectMenu(FinalList, ShowName);
								if (SelectMain2.ShowDialog() == DialogResult.OK)
								{
									int selectedid = SelectMain2.selected;
									if (selectedid == -1) return TVShowID;
									selectionList.Add(new SearchInfo(ShowName, selectedid));
									selectedSeriesId = FinalList[selectedid].SelectedValue;
									selectedTitle = FinalList[selectedid].Title;
									SelectMain2.Close();
								}
							}
							else
							{
								selectedSeriesId = FinalList[idNumber].SelectedValue;
							}
						}
					}
				}

				if (selectedSeriesId == -1) return TVShowID;   //return if nothing is found
				TVShowID.SelectedValue = selectedSeriesId;
				TVShowID.Title = selectedTitle;
			}
			TVShowID.Title = TVShowID.Title.Replace(":", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("\"", "");
			return TVShowID;
		}

		public string getTitle(int seriesID, int season, int episode)
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