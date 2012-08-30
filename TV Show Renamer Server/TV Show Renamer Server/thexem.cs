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

namespace TV_Show_Renamer_Server
{
    class thexem
    {
        ICacheProvider m_cacheProvider = null;
        TvdbHandler m_tvdbHandler = null;
        List<SearchInfo> selectionList = new List<SearchInfo>();

        string folder = null;

        public thexem(string newFolder)
        {
            folder = newFolder;
            m_cacheProvider = new XmlCacheProvider(folder + "\\Temp");
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
                string selectedTitle = "";
                if (seriesId.Count() == 1)
                {
                    selectedSeriesId = seriesId[0];
                    selectedTitle = seriesName[0];
                }
                else
                {
                    if (selectionList.Count() == 0)
                    {
                        SelectMenu SelectMain = new SelectMenu(seriesName);
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
                            SelectMenu SelectMain2 = new SelectMenu(seriesName);
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
            string newTitle = null;

            try
            {
                if (!(File.Exists(folder + "//" + seriesID.ToString()+".xml")))//see if file exists
                {
                    WebClient client = new WebClient();
                    client.DownloadFile("http://thexem.de/proxy/tvdb/scene/api/BC08025A4C3F3D10/series/" + seriesID.ToString() + "/all/en.xml", folder + "\\" + seriesID.ToString()+".xml");
                    
                }
                //WebClient client = new WebClient();
                //Stream stream = client.OpenRead("http://thexem.de/proxy/tvdb/scene/api/BC08025A4C3F3D10/series/" + seriesID.ToString() + "/all/en.xml");
                //StreamReader reader = new StreamReader(stream);
                //string content = reader.ReadToEnd();
                //File.WriteAllText("f://test.xml", content);
                //MessageBox.Show(content);

                //http://thexem.de/proxy/tvdb/scene/api/1D62F2F90030C444/series/\1/all/$INFO[language].xml
                //http://thexem.de/proxy/tvdb/scene/api/{1}/series/{2}/all/{3}.xml


                    //XDocument EpisodeList = XDocument.Load("http://thexem.de/proxy/tvdb/scene/api/BC08025A4C3F3D10/series/" + seriesID.ToString() + "/all/en.xml");
                    XDocument EpisodeList = XDocument.Load(folder + "//" + seriesID.ToString()+".xml");

                    //EpisodeList.Save("f://test.xml");
                    var Categorys = from Episode in EpisodeList.Descendants("Episode")
                                    select new
                                    {
                                        ID = Episode.Element("id").Value,
                                        CombinEpisode = Episode.Element("Combined_episodenumber").Value,
                                        CombinSeason = Episode.Element("Combined_season").Value,
                                        EpisodeName = Episode.Element("EpisodeName").Value,
                                        EpisodeNumber = Episode.Element("EpisodeNumber").Value,
                                        SeasonNumber = Episode.Element("SeasonNumber").Value,
                                        AbsoluteNumber = Episode.Element("absolute_number").Value
                                    };

                    foreach (var wd in Categorys)
                    {

                        int XMLSeason = Int32.Parse(wd.SeasonNumber);
                        int XMLEpisode = Int32.Parse(wd.EpisodeNumber);
                        if (XMLSeason == season && XMLEpisode == episode)
                        {
                            newTitle = wd.EpisodeName;
                            break;
                        }
                    }
                //newTitle = ;
            }
            catch (Exception e ) 
            {
                MessageBox.Show(e.Message.ToString());            
            }

            if (newTitle == null)
                return "";
            newTitle = newTitle.Replace(":", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("\"", "");
            return newTitle;
        }
    }
}
