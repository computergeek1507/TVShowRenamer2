﻿using System;
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

        string folder = null;

        int format = -1;

        string title = null;
        int indexes = -1;
        int season = -1;
        int episode = -1;
        string tvdbTitle = null;

        public TVDB(Form1 temp, int selected, string showMainName, string newFolder, int newFormat)
        {
            InitializeComponent();
            folder = newFolder;
            format = newFormat;
            indexes = selected;
            title = showMainName;
            main = temp;
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            infoFinder();

            if(tvdbTitle==null){
                return;
            }

            m_cacheProvider = new XmlCacheProvider(folder);
            m_tvdbHandler = new TvdbHandler(m_cacheProvider, "BC08025A4C3F3D10");
            List<TvdbSearchResult> list = m_tvdbHandler.SearchSeries(tvdbTitle);
            if (list != null && list.Count > 0)
            {
                List<int> seriesId = new List<int>();
                List<string> seriesName = new List<string>();
                for (int i = 0; i < list.Count();i++ )
                {
                    if (list[i].Overview != "")
                    {                       
                        seriesId.Add(list[i].Id);// = list[i].Id;
                        seriesName.Add(list[i].SeriesName);
                        //break;
                    }
                }
                if (seriesId.Count() == 0) return;//return if nothing found
                int selectedSeriesId = -1;
                if (seriesId.Count() == 1) 
                { 
                    selectedSeriesId = seriesId[0]; 
                } else 
                {
                    SelectMenu SelectMain = new SelectMenu(seriesName);
                    if (SelectMain.ShowDialog() == DialogResult.OK)
                    {
                        int selectedid = SelectMain.selected;
                        selectedSeriesId = seriesId[selectedid];
                        SelectMain.Close();                        
                    }                    
                }

                if (selectedSeriesId==-1) return;//return if nothing is found
                TvdbSeries s = m_tvdbHandler.GetSeries(selectedSeriesId, TvdbLanguage.DefaultLanguage, true, false, false);
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
                if (newTitle == null) {
                    return;
                }
                newTitle = newTitle.Replace(":", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("\"", "");
                
                if (main.addTitle(newTitle, indexes))
                {
                    Thread t = new Thread(new ThreadStart(convert));
                    t.Start();
                }
                this.Close();               
            }
        }

        private void infoFinder() {
            string test = title;

            //string test = title[0];
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
                    {
                        newi = "0" + i.ToString();
                    }
                    //check if j is less than 10
                    if (j < 10)
                    {
                        newj = "0" + j.ToString();
                    }
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
                            you = test.IndexOf("S" + newi + "e" + newj);
                            break;
                    }
                    //stop loop when name is change                    
                    if (you != -1)
                    {
                        season = i;
                        episode = j;
                        tvdbTitle = test.Remove(you - 1, test.Length - (you - 1));
                        end = true;
                        break;
                    }
                }//end of episode loop

                //stop loop when name is change
                if (end)
                {
                    break;
                }
            }//end of season loop
        }

        private void convert()
        {
            main.autoConvert();
        }
    }
}
