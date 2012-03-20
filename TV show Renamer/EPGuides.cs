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
        
        public EPGuides(string newFolder)
        {
            folder = newFolder;            
        }
        public int findTitle(string ShowName)
        {
            WebClient client = new WebClient();
            client.DownloadFile("http://epguides.com/common/allshows.txt", folder + "/allshows.txt");
            return -1;
        }

        public string getTitle(int seriesID, int season, int episode)
        { 
            return "";        
        }

        public List<string[]> parseCSV(string path)
        {
            List<string[]> parsedData = new List<string[]>();

            try
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        row = line.Split(',');
                        parsedData.Add(row);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return parsedData;
        }
    }

}
