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
                int seriesId = -1;
                for (int i = 0; i < list.Count();i++ )
                {
                    if (list[i].Overview != "")
                    {
                        //MessageBox.Show("||"+list[i].Overview+"||");
                        seriesId = list[i].Id;
                        break;
                    }
                }
                if(seriesId == -1)return;
                //int seriesId =list[0].Id;//   r.Id.ToString()
                //Display main = new Display(list);
                TvdbSeries s = m_tvdbHandler.GetSeries(seriesId, TvdbLanguage.DefaultLanguage, true, false, false);
                List<String> epList = new List<string>();
                string newTitle = null;
                foreach (TvdbEpisode esp in s.Episodes)
                {
                    if (season == esp.SeasonNumber && episode == esp.EpisodeNumber)
                    {
                        newTitle = esp.EpisodeName;
                        break;                    
                    }
                   //epList.Add("Season " + esp.SeasonNumber + " Episode " + esp.EpisodeNumber + ": " + esp.EpisodeName);
                }
                //MessageBox.Show(newTitle);
                if (newTitle == null) {
                    return;
                }

                newTitle = newTitle.Replace(":", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("\"", "");
                
                if (main.addTitle(newTitle, indexes))
                {
                    //textBox1.Text = null;
                    Thread t = new Thread(new ThreadStart(convert));
                    t.Start();
                }
                this.Close();

                //Display main = new Display(epList);
            }
            //TvdbSeries s = m_tvdbHandler.GetSeries(seriesId, TvdbLanguage.DefaultLanguage, true, true, true, false);
        }

        private void infoFinder() {
            string test = title;

            //string test = title[0];
            int you = -1;

            for (int i = 1; i < 40; i++)
            {
                //varable for break command later
                bool end = false;

                //loop for episodes
                for (int j = 1; j < 100; j++)
                {
                    string newi = i.ToString();
                    string newj = j.ToString();
                    //string output = null;
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
                    //string startnewname = fileName;
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
