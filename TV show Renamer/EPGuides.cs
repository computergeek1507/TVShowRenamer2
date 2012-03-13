using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

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
            return -1;
        }

        public string getTitle(int seriesID, int season, int episode)
        { 
            return "";        
        }
    }
}
