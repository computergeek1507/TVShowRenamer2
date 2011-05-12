using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TvdbLib;
using TvdbLib.Data;
using System.IO;
using TvdbLib.Cache;
using System.Threading;

namespace TV_show_Renamer
{
    public partial class TVDB : Form
    {
        ICacheProvider m_cacheProvider = null;
        TvdbHandler m_tvdbHandler = null;
        Form1 main;
        BindingList<TVClass> fileList = new BindingList<TVClass>();//TV Show list    
        List<SearchInfo> selectionList = new List<SearchInfo>();
        
        string folder = null;
        int format = -1;
        bool renameWorked = false;
        int season = -1;
        int episode = -1;
        string tvdbTitle = null;

        public TVDB(Form1 temp, BindingList<TVClass> newFileList,  string newFolder,int newFormat)
        {
            InitializeComponent();
            folder = newFolder;
            format = newFormat;
            fileList = newFileList;
            main = temp;
            findTitleAll();            
        }

        public TVDB(Form1 temp, BindingList<TVClass> newFileList,  List<int> newSelected, string newFolder, int newFormat)
        {
            InitializeComponent();
            folder = newFolder;
            format = newFormat;
            fileList = newFileList;
            main = temp;
            findTitleselected(newSelected);
        }
        
        private void findTitleAll()
        {
            for (int z = 0; z < fileList.Count(); z++)
            {
                infoFinder(fileList[z].NewFileName);

                if (tvdbTitle == null)                
                    continue;                                
                m_cacheProvider = new XmlCacheProvider(folder);
                m_tvdbHandler = new TvdbHandler(m_cacheProvider, "BC08025A4C3F3D10");
                if (fileList[z].TVShowID != -1)
                {
                    getTitle(fileList[z].TVShowID, z);
                    continue;
                }

                List<TvdbSearchResult> list = m_tvdbHandler.SearchSeries(tvdbTitle);
                if (list != null && list.Count > 0)
                {
                    List<int> seriesId = new List<int>();
                    List<string> seriesName = new List<string>();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (list[i].Overview != "")
                        {
                            seriesId.Add(list[i].Id);   
                            seriesName.Add(list[i].SeriesName);
                        }
                    }
                    if (seriesId.Count() == 0) return;  //return if nothing found
                    int selectedSeriesId = -1;
                    if (seriesId.Count() == 1)                    
                        selectedSeriesId = seriesId[0];                    
                    else
                    {
                        if (selectionList.Count() == 0)
                        {
                            SelectMenu SelectMain = new SelectMenu(seriesName);
                            if (SelectMain.ShowDialog() == DialogResult.OK)
                            {
                                int selectedid = SelectMain.selected;
                                if (selectedid == -1) continue;
                                selectionList.Add(new SearchInfo(tvdbTitle, selectedid));
                                selectedSeriesId = seriesId[selectedid];
                                SelectMain.Close();
                            }
                        }
                        else {
                            int  idNumber = -1;
                            foreach (SearchInfo testIdem in selectionList) {
                                if (testIdem.Title == tvdbTitle) 
                                {
                                    idNumber = testIdem.SelectedValue;
                                    break;
                                }                                                           
                            }
                            if (idNumber==-1) {
                                SelectMenu SelectMain2 = new SelectMenu(seriesName);
                                if (SelectMain2.ShowDialog() == DialogResult.OK)
                                {
                                    int selectedid = SelectMain2.selected;
                                    if (selectedid == -1) continue;
                                    selectionList.Add(new SearchInfo(tvdbTitle, selectedid));
                                    selectedSeriesId = seriesId[selectedid];
                                    SelectMain2.Close();
                                }
                            } else 
                                selectedSeriesId = seriesId[idNumber];                                                    
                        }
                    }

                    if (selectedSeriesId == -1) return;   //return if nothing is found
                    fileList[z].TVShowID = selectedSeriesId;

                    getTitle(selectedSeriesId, z);
                }
            }//end of for loop
            if (renameWorked)
            {
                Thread t = new Thread(new ThreadStart(convert));
                t.Start();
            }
            this.Close();
        }

        private void findTitleselected(List<int> selected)
        {
            foreach (int x in selected)
            {
                infoFinder(fileList[x].NewFileName);

                if (tvdbTitle == null)                
                    continue;                

                m_cacheProvider = new XmlCacheProvider(folder);
                m_tvdbHandler = new TvdbHandler(m_cacheProvider, "BC08025A4C3F3D10");
                if (fileList[x].TVShowID != -1) 
                {
                    getTitle(fileList[x].TVShowID, x);
                    continue;
                }

                List<TvdbSearchResult> list = m_tvdbHandler.SearchSeries(tvdbTitle);
                if (list != null && list.Count > 0)
                {
                    List<int> seriesId = new List<int>();
                    List<string> seriesName = new List<string>();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        if (list[i].Overview != "")
                        {
                            seriesId.Add(list[i].Id);   // = list[i].Id;
                            seriesName.Add(list[i].SeriesName);
                            //break;
                        }
                    }
                    if (seriesId.Count() == 0) return;  //return if nothing found
                    int selectedSeriesId = -1;
                    if (seriesId.Count() == 1)                    
                        selectedSeriesId = seriesId[0];                    
                    else
                    {
                        if (selectionList.Count() == 0)
                        {
                            SelectMenu SelectMain = new SelectMenu(seriesName);
                            if (SelectMain.ShowDialog() == DialogResult.OK)
                            {
                                int selectedid = SelectMain.selected;
                                if (selectedid == -1) continue;
                                selectionList.Add(new SearchInfo(tvdbTitle, selectedid));
                                selectedSeriesId = seriesId[selectedid];
                                SelectMain.Close();
                            }
                        }
                        else
                        {
                            int idNumber = -1;
                            foreach (SearchInfo testIdem in selectionList)
                            {
                                if (testIdem.Title == tvdbTitle)
                                {
                                    idNumber = testIdem.SelectedValue;
                                    break;
                                }
                            }
                            if (idNumber == -1)
                            {
                                SelectMenu SelectMain = new SelectMenu(seriesName);
                                if (SelectMain.ShowDialog() == DialogResult.OK)
                                {
                                    int selectedid = SelectMain.selected;
                                    if (selectedid == -1) continue;
                                    selectionList.Add(new SearchInfo(tvdbTitle, selectedid));
                                    selectedSeriesId = seriesId[selectedid];
                                    SelectMain.Close();
                                }
                            }
                            else                            
                                selectedSeriesId = seriesId[idNumber];                            
                        }
                    }

                    if (selectedSeriesId == -1) continue;   //return if nothing is found
                    fileList[x].TVShowID = selectedSeriesId;

                    getTitle(selectedSeriesId, x);
                                        
                }
            }//end of for loop
            if (renameWorked)
            {
                Thread t = new Thread(new ThreadStart(convert));
                t.Start();
            }
            this.Close();
        }

        private void infoFinder(string fileName) {
            //rest globals
            season = -1;
            episode = -1;
            tvdbTitle = null;
            string test = fileName;
            int you = -1;

            for (int i = 40; i >=0; i--)
            {
                //varable for break command later
                bool end = false;

                //loop for episodes
                for (int j = 1; j < 100; j++)
                {
                    string newi = i.ToString();
                    string newj = j.ToString();
                    //check if i is less than 10
                    if (i < 10)                    
                        newi = "0" + i.ToString();                    
                    //check if j is less than 10
                    if (j < 10)                    
                        newj = "0" + j.ToString();                    
                    //make string to compare changed name too
                    switch (format)
                    {
                        case 1:
                            you = test.IndexOf(i.ToString() + "x" + newj);
                            break;
                        case 2:
                            you = test.IndexOf(newi + newj);
                            break;
                        case 3:
                            you = test.IndexOf(i.ToString() + newj);
                            break;
                        case 4:
                            you = test.IndexOf("S" + newi + "E" + newj);
                            //you = test.IndexOf("S" + newi + "e" + newj);
                            break;
                    }
                    //stop loop when name is change                    
                    if (you != -1)
                    {
                        season = i;
                        episode = j;
                        if (you == 0)                        
                            tvdbTitle = test.Remove(you , test.Length - (you ));                        
                        else                        
                            tvdbTitle = test.Remove(you - 1, test.Length - (you - 1));                        
                        //tvdbTitle = test.Remove(you - 1, test.Length - (you - 1));
                        end = true;
                        break;
                    }
                }//end of episode loop

                //stop loop when name is change
                if (end)                
                    break;                
            }//end of season loop
        }

        private void convert()
        {
            main.autoConvert();
        }

        private void getTitle(int seriesID,int index)
        {
            TvdbSeries s = m_tvdbHandler.GetSeries(seriesID, TvdbLanguage.DefaultLanguage, true, false, false);
            List<String> epList = new List<string>();
            string newTitle = null;
            foreach (TvdbEpisode esp in s.Episodes)
            {
                if (season == esp.SeasonNumber && episode == esp.EpisodeNumber)
                {
                    newTitle = esp.EpisodeName;
                    break;
                }
            }
            if (newTitle == null)
                return;
            newTitle = newTitle.Replace(":", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("\"", "");

            if (renameWorked)
                main.addTitle(newTitle, index);
            else
                renameWorked = main.addTitle(newTitle, index);        
        
        }
    }//end of class
}//end of namespace
