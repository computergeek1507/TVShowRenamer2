using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;
using System.Windows.Forms;


namespace TV_Show_Renamer
{
    public class TVRage
    {

        public int findTitle(string ShowName)
        {
            int TVShowID = -1;
            if (ShowName == null)
                return TVShowID;
            ShowName = ShowName.Replace("Gold Rush Alaska", "Gold Rush");
            ShowName = ShowName.Replace("Tosh 0", "Tosh.0");
            //var doc = XDocument.Load(FileLocation);
            //var emp = doc.Descendants("category").FirstOrDefault();
            //numericUpDown1.Value = decimal.Parse(emp.Element("application").Value);
            //numericUpDown2.Value = decimal.Parse(emp.Element("library").Value);
            //numericUpDown3.Value = decimal.Parse(emp.Element("settings").Value);
            List<TVShowID> FinalList = new List<TVShowID>();

            XDocument ShowList = XDocument.Load("http://services.tvrage.com/feeds/search.php?show="+ShowName);

            var Categorys = from Show in ShowList.Descendants("show")
                            select new
                            {
                                ShowID = Show.Element("showid").Value,
                                Name = Show.Element("name").Value,                                
                            };

            foreach (var wd in Categorys)
            {
                FinalList.Add(new TVShowID(wd.Name, Int32.Parse(wd.ShowID)));
            }

            if (FinalList != null && FinalList.Count > 0)
            {
                if (FinalList.Count() == 0) return TVShowID;  //return if nothing found
                int selectedSeriesId = -1;
                if (FinalList.Count() == 1)
                    selectedSeriesId = FinalList[0].TVID;
                else
                {
                    if (FinalList.Count() != 0)
                    {
                        SelectMenu SelectMain = new SelectMenu(FinalList);
                        if (SelectMain.ShowDialog() == DialogResult.OK)
                        {
                            int selectedid = SelectMain.selected;
                            if (selectedid == -1) return TVShowID;
                            selectedSeriesId = FinalList[selectedid].TVID;
                            SelectMain.Close();
                        }
                    }            
                }

                if (selectedSeriesId == -1) return TVShowID;   //return if nothing is found
                TVShowID = selectedSeriesId;
            }
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
                    if(episode>10){
                    seporater="x";
                    }
                    XDocument EpisodeList = XDocument.Load("http://services.tvrage.com/feeds/episodeinfo.php?sid=" + seriesID.ToString() + "&ep=" + season.ToString()+seporater+episode.ToString());

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