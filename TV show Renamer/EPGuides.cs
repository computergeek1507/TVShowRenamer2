using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Globalization;

namespace TV_Show_Renamer
{
    class EPGuides
    {
        string folder = null;
        List<SearchInfo> selectionList = new List<SearchInfo>();

        public static string URL = "http://epguides.com/";

        public EPGuides(string newFolder)
        {
            folder = newFolder + "\\Temp";
        }

        public SearchInfo findTitle(string ShowName)
        {
            SearchInfo TVShowID = new SearchInfo();
            if (ShowName == null)
                return TVShowID;
           
            ShowName = ShowName.Replace("Gold Rush Alaska", "Gold Rush");
            ShowName = ShowName.Replace("Tosh 0", "Tosh.0");
            WebClient client = new WebClient();
            client.DownloadFile("http://epguides.com/common/allshows.txt", folder + "/allshows.txt");
            List<SearchInfo> TVShowList = parseCSV(folder + "/allshows.txt", ShowName);

            if (TVShowList != null && TVShowList.Count > 0)
            {
                if (TVShowList.Count() == 0) return TVShowID;  //return if nothing found
                //int selectedSeriesId = -1;
                //string selectedTitle = "";
                if (TVShowList.Count() == 1)
                {
                    return TVShowList[0];
                }
                else
                {
                    if (selectionList.Count() == 0)
                    {
                        SelectMenu SelectMain = new SelectMenu(TVShowList);
                        if (SelectMain.ShowDialog() == DialogResult.OK)
                        {
                            int selectedid = SelectMain.selected;
                            if (selectedid == -1) return TVShowID;
                            selectionList.Add(TVShowList[selectedid]);
                            SearchInfo temp = TVShowList[selectedid];                            
                            SelectMain.Close();
                            return temp;
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
                            SelectMenu SelectMain = new SelectMenu(TVShowList);
                            if (SelectMain.ShowDialog() == DialogResult.OK)
                            {
                                int selectedid = SelectMain.selected;
                                if (selectedid == -1) return TVShowID;
                                selectionList.Add(TVShowList[selectedid]);
                                SearchInfo temp = TVShowList[selectedid];
                                SelectMain.Close();
                                return temp;
                            }
                        }
                        else
                        {
                            return TVShowList[idNumber];
                        }
                    }
                }
            }
            return TVShowID;
        }

        public string getTitle(string seriesID, int season, int episode)
        {
            List<EPGuigeReturnObject> showList = new List<EPGuigeReturnObject>();
            string returnInfo="";
            try
            {
                if (File.Exists(folder + "//" + seriesID))//see if file exists
                {
                    showList = parse(folder + "\\" + seriesID);
                }
                else 
                {
                    WebClient client = new WebClient();
                    //String htmlCode = client.DownloadString(URL + showtitle);
                    client.DownloadFile(URL + seriesID, folder + "\\" + seriesID);
                    showList = parse(folder + "\\" + seriesID);
                } 
            }
            catch (Exception e)
            {                
            }
            if (!(showList.Count == 0)) 
            {
                if (season > 100)
                {
                    foreach (EPGuigeReturnObject EpisodeInfo in showList)
                    {
                        //DateTime test = new DateTime(episode, season / 100, season % 100);
                        //string testTime = ((season / 100).ToString() + "/" + (season % 100).ToString() + "/" + episode.ToString());
                        if (EpisodeInfo.EpisodeDate ==  new DateTime(episode, season / 100, season % 100).ToShortDateString())
                        {
                            returnInfo = EpisodeInfo.EpisodeTitle;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (EPGuigeReturnObject EpisodeInfo in showList)
                    {
                        if ((EpisodeInfo.EpisodeNumber2 == (season.ToString() + "-0" + episode.ToString())) || (EpisodeInfo.EpisodeNumber2 == (season.ToString() + "-" + episode.ToString())))
                        {
                            returnInfo = EpisodeInfo.EpisodeTitle;
                            break;
                        }
                    }
                }
            }
            returnInfo = returnInfo.Replace(":", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("\"", "");
            return returnInfo;        
        }

        public List<SearchInfo> parseCSV(string path,string tvshowname)
        {
            List<SearchInfo> parsedData = new List<SearchInfo>();
            StreamReader readFile= null;
            try
            {
                using (readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        row = line.Split(',');
                        //row[0].Trim('"');
                        row[0] = RemoveSpecialChars(row[0]);
                        int difference = Math.Abs(row[0].Length - tvshowname.Length);
                        int indexofTVshow = row[0].IndexOf(tvshowname, StringComparison.InvariantCultureIgnoreCase);
                        if (indexofTVshow != -1 && difference < 8)
                        {
                            parsedData.Add(new SearchInfo(row[0],row[1],Int32.Parse(row[2])));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (readFile != null)
                    readFile.Close();            
            }

            return parsedData;
        }

        public List<EPGuigeReturnObject> parse(string fileLocation)
        {
            List<EPGuigeReturnObject> AllEpisodes = new List<EPGuigeReturnObject>();
            string ShowName = "";
            //DataSet ds = new DataSet(showtitle);

            //WebClient client = new WebClient();
            //String htmlCode = client.DownloadString(URL + showtitle);
            //client.DownloadFile(URL + showtitle, "d:/dev.html.txt");

            string htmlCode = File.ReadAllText(fileLocation);

            StringReader sr = new StringReader(htmlCode);

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                // title
                if (line.Contains("<title>"))
                {
                    int start = line.IndexOf("<title>") + 7;
                    int end = line.IndexOf("(a Titles &amp");
                    ShowName=line.Substring(start, end - start);
                }

                // show and episode content
                if (line.Contains("<pre>"))
                {
                    line = sr.ReadLine();

                    // skip empty lines
                    while (line != null && line.Length == 0)
                    {
                        line = sr.ReadLine();
                    }

                    // skip episode table header
                    line = sr.ReadLine();
                    line = sr.ReadLine();
                    line = sr.ReadLine();

                    // skip empty lines
                    while (line != null && line.Length == 0)
                    {
                        line = sr.ReadLine();
                    }

                    // cycle through all season and episode lines
                    while (line != null && !line.Contains("</pre>"))
                    {
                        // season name
                        //Console.WriteLine("Season: {0}", line);

                        if (line.StartsWith("&bull; ")) // sometimes season names start with &bull; 
                        {
                            line = line.Substring(7).Trim();

                            //ds.Tables["HEAD"].Rows.Add("ContentVersion", "epguides+tvrage");
                        }
                        else
                        {
                            //ds.Tables["HEAD"].Rows.Add("ContentVersion", "epguides+tv.com");
                        }

                        line = sr.ReadLine();

                        // skip empty lines
                        while (line != null && line.Length == 0)
                        {
                            line = sr.ReadLine();
                        }

                        // episodes
                        while (line != null && !(line.Length == 0))
                        {

                            DateTime airDate = DateTime.MinValue;
                            string episodeNumber = "";
                            string episodeNumber2 = "";
                            string episode = "";
                            string episodeTitle = "";

                            Regex r = new Regex(@"\s{2,}");
                            string[] parts = r.Split(line);
                            foreach (string ppp in parts)
                            {
                                // try distance as a tool to determine if value is episode number or production number etc.
                                //Console.WriteLine("split[distance:{1}]: {0} ", ppp, line.IndexOf(ppp));

                                // get episode number
                                if (line.IndexOf(ppp) == 0)
                                {
                                    episodeNumber = ppp;
                                }

                                // get 2nd episode number
                                if (line.IndexOf(ppp) > 0 && line.IndexOf(ppp) < 10)
                                {
                                    episodeNumber2 = ppp;
                                }

                                // get episode info
                                if (line.IndexOf(ppp) > 35 && (!ppp.Contains("#trailer") && !ppp.Contains("recap")))
                                {
                                    episode = ppp;
                                }

                                // get airdate                                
                                if (airDate == DateTime.MinValue)
                                    if (!DateTime.TryParseExact(ppp, "dd/MMM/yy", CultureInfo.CreateSpecificCulture("en"), System.Globalization.DateTimeStyles.None, out airDate))
                                        DateTime.TryParseExact(ppp, "dd MMM yy", CultureInfo.CreateSpecificCulture("en"), System.Globalization.DateTimeStyles.None, out airDate);
                            }

                            // get episode title
                            List<LinkItem> list = LinkFinder.Find(episode);
                            foreach (LinkItem item in list)
                            {
                                if ((!item.Href.Contains("#trailer")) && (!item.Href.Contains("recap")))
                                {
                                    //Console.WriteLine("episode href: {0}", item.Href);
                                    //Console.WriteLine("episode title: {0}", item.Text);
                                    episodeTitle = item.Text;
                                    //episodeTitleLink = item.Href;
                                }
                            }

                            // add episode values to episode table 
                            AllEpisodes.Add(new EPGuigeReturnObject(ShowName, episodeNumber, episodeNumber2, airDate.ToShortDateString(), episodeTitle));
                            //AllEpisodes.Add(episodeNumber);
                            //AllEpisodes.Add(episodeNumber2);
                            //AllEpisodes.Add(episodeTitle);
                            //AllEpisodes.Add(airDate.ToShortDateString());
                            //dt.Rows.Add(episodeNumber2,  episodeTitle);

                            line = sr.ReadLine();
                        }

                        // skip empty lines until next season
                        while (line != null && line.Length == 0)
                        {
                            line = sr.ReadLine();
                        }
                    }
                    break; // got all episodes - we are done
                }

            }

            return AllEpisodes;
        }

        public string RemoveSpecialChars(string input)
        {
            return Regex.Replace(input, @"[^0-9a-zA-Z\ ]", string.Empty);
        }
    }

}
