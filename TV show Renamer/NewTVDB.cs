﻿using System;
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

namespace TV_Show_Renamer
{
	class NewTVDB
	{
		ICacheProvider m_cacheProvider = null;
		TvdbHandler m_tvdbHandler = null;
		List<SearchInfo> selectionList = new List<SearchInfo>();

		//string folder = null;

		//string tvdbTitle = null;

		public NewTVDB(string Folder)
		{
			//folder = newFolder;
			m_cacheProvider = new XmlCacheProvider(Folder + Path.DirectorySeparatorChar + "Temp");
			m_tvdbHandler = new TvdbHandler(m_cacheProvider, "BC08025A4C3F3D10");
			m_tvdbHandler.InitCache();
		}

		public SearchInfo findTitle(string ShowName)
		{
			SearchInfo TVShowID = new SearchInfo();
			if (ShowName == null)
				return TVShowID;
			ShowName = ShowName.Replace("Gold Rush Alaska", "Gold Rush");
			ShowName = ShowName.Replace("Tosh 0", "Tosh.0");
			//ICacheProvider m_cacheProvider = null;
			//TvdbHandler m_tvdbHandler = null;
			List<TvdbSearchResult> list = m_tvdbHandler.SearchSeries(ShowName);
			if (list != null && list.Count > 0)
			{
				List<int> seriesId = new List<int>();
				List<string> seriesName = new List<string>();
				for (int i = 0; i < list.Count(); i++)
				{
					if (list[i].Id != 0)
					{
						seriesId.Add(list[i].Id);
						seriesName.Add(list[i].SeriesName);
					}
				}
				if (seriesId.Count() == 0) return TVShowID;  //return if nothing found
				int selectedSeriesId = -1;
				string selectedTitle="";
				if (seriesId.Count() == 1)
				{
					selectedSeriesId = seriesId[0];
					selectedTitle=seriesName[0];
				}
				else
				{
					if (selectionList.Count() == 0)
					{
						SelectMenu SelectMain = new SelectMenu(seriesName, ShowName);
						if (SelectMain.ShowDialog() == DialogResult.OK)
						{
							int selectedid = SelectMain.selected;
							if (selectedid == -1) return TVShowID;
							selectionList.Add(new SearchInfo(ShowName, selectedid));
							selectedSeriesId = seriesId[selectedid];
							selectedTitle = seriesName[selectedid];
							SelectMain.Close();
						}
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
							SelectMenu SelectMain2 = new SelectMenu(seriesName, ShowName);
							if (SelectMain2.ShowDialog() == DialogResult.OK)
							{
								int selectedid = SelectMain2.selected;
								if (selectedid == -1) return TVShowID;
								selectionList.Add(new SearchInfo(ShowName, selectedid));
								selectedSeriesId = seriesId[selectedid];
								selectedTitle = seriesName[selectedid];
								SelectMain2.Close();
							}
						}
						else
						{
							selectedSeriesId = seriesId[idNumber];
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
			string newTitle = "";
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
			
			newTitle = newTitle.Replace(":", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("\"", "");
			return newTitle;
		}
	}
}
