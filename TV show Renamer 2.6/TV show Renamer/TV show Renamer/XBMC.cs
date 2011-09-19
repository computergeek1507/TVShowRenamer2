using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TV_show_Renamer
{
    public partial class XBMC : Form
    {
        BindingList<TVClass> fileList = new BindingList<TVClass>();
        int format;
        public XBMC(BindingList<TVClass> newfileList,int newformat)
        {
            format = newformat;
            fileList = newfileList;
            InitializeComponent();
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             List<string> test =infoFinder( fileList[0].NewFileName,fileList[0].FileExtention);

             MessageBox.Show(test[0] + "\n" + test[1] + "\n" + test[2] + "\n" + test[3]);
        }        
        private List<string> infoFinder(string fileName,string ext)
        {
            List<string> stuff = new List<string>();
            //rest globals
            int season = -1;
            int episode = -1;
            string Title = null;
            string test = fileName;
            string test2 = fileName;
            string Eptitle = null;
            int you = -1;

            for (int i = 40; i >= 0; i--)
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
                        case 0:
                            you = test.IndexOf(i.ToString() + "x" + newj);
                            test2 = test2.Replace(i.ToString() + "x" + newj, "");
                            break;
                        case 1:
                            you = test.IndexOf(newi + newj);
                            test2 = test2.Replace(newi + newj, "");
                            break;
                        case 2:
                            you = test.IndexOf(i.ToString() + newj);
                            test2 = test2.Replace(i.ToString() + newj, "");
                            break;
                        case 3:
                            you = test.IndexOf("S" + newi + "E" + newj);
                            test2 = test2.Replace("S" + newi + "E" + newj, "");
                            //you = test.IndexOf("S" + newi + "e" + newj);
                            break;
                    }
                    //stop loop when name is change                    
                    if (you != -1)
                    {
                        season = i;
                        episode = j;
                        if (you == 0)
                            Title = test.Remove(you, test.Length - (you));
                        else
                            Title = test.Remove(you - 1, test.Length - (you - 1));
                        //tvdbTitle = test.Remove(you - 1, test.Length - (you - 1));
                        Eptitle=  test2.Remove(0, you+3);
                        Eptitle = Eptitle.Replace(ext, "");

                        end = true;
                        break;
                    }
                }//end of episode loop

                //stop loop when name is change
                if (end)
                    break;
            }//end of season loop
            stuff.Add(Title);
            stuff.Add(season.ToString());
            stuff.Add(episode.ToString());
            stuff.Add(Eptitle);
            return stuff;
        }
    }
}
