using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace TV_Show_Renamer
{
    class EPGuides
    {
        string folder = null;
        List<SearchInfo> selectionList = new List<SearchInfo>();

        public EPGuides(string newFolder, List<SearchInfo> newselectionList)
        {
            folder = newFolder;
            selectionList = newselectionList;
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

        public string getTitle(int seriesID,string urlLink, int season, int episode)
        { 
            return "";        
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
                        row[0].Trim('"');
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
    }

}
