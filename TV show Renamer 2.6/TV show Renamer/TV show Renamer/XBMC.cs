using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace TV_show_Renamer
{
    public partial class XBMC : Form
    {
        const int HowDeepToScan = 4;
        BindingList<TVClass> fileList = new BindingList<TVClass>();
        BindingList<string> folderList = new BindingList<string>();
        int format=-1;
        string TVFolder = null;

        public XBMC(BindingList<TVClass> newfileList,int newformat,string newTVFolder)
        {
            format = newformat;
            fileList = newfileList;
            TVFolder = newTVFolder;
            InitializeComponent();
            folderList = folderFinder(newTVFolder);
            comboBox1.DataSource = folderList;
            this.Show();
        }

        //make TV Episode NFO
        private void button1_Click(object sender, EventArgs e)
        {
            //comboBox1.SelectedIndex
            foreach (TVClass TVfile in fileList) 
            {
                List<string> fileData = infoFinder(TVfile.NewFileName, TVfile.FileExtention);
                //MessageBox.Show(fileData[0]+"\n"+fileData[1]+"\n"+fileData[2]+"\n"+fileData[3]);
                string nfoFile = TVfile.NewFullFileName.Replace(TVfile.FileExtention, ".nfo");
                EpisodeWrite(nfoFile, fileData[3], Convert.ToInt32(fileData[1]), Convert.ToInt32(fileData[2]));
                //MessageBox.Show(nfoFile);
            }
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
                            you = test.IndexOf("S" + newi + "E" + newj);
                            test2 = test2.Replace("S" + newi + "E" + newj, "");
                            //you = test.IndexOf("S" + newi + "e" + newj);
                            break;
                        case 3:
                            you = test.IndexOf(i.ToString() + newj);
                            test2 = test2.Replace(i.ToString() + newj, "");
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

        //get list of folders
        private BindingList<string> folderFinder(string folderwatch)
        {
            BindingList<string> foldersIn = new BindingList<string>();
            

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folderwatch);
            try
            {
                foreach (System.IO.DirectoryInfo fi in di.GetDirectories())
                {
                    foldersIn.Add(fi.Name);
                }
            }
            catch (IOException)
            {
                return foldersIn;
            }
            return foldersIn;
        }

        //make logo
        private void button3_Click(object sender, EventArgs e)
        {
            if (File.Exists(TVFolder + "\\" + folderList[comboBox1.SelectedIndex] + "\\logo.png"))
            {
                MessageBox.Show("Logo Already Exists");
            }
            else
            {
                Bitmap bmp1 = CreateLogo(folderList[comboBox1.SelectedIndex]);
                bmp1.Save(TVFolder + "\\" + folderList[comboBox1.SelectedIndex] + "\\logo.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        private Bitmap CreateLogo(string text)
        {

            Bitmap objBmpImage = new Bitmap(1, 1);
            Graphics objGraphics = Graphics.FromImage(objBmpImage);
            int intWidth = 400;
            int intHeight = 155;
            int FontSize= 255;
            Font objFont = new Font("Arial", FontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            int fontWidth = (int)objGraphics.MeasureString(text, objFont).Width;
            int fontHeight = (int)objGraphics.MeasureString(text, objFont).Height;
            while (fontWidth > intWidth || fontHeight > intHeight)
            {
                objFont = new Font("Arial", FontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
                fontWidth = (int)objGraphics.MeasureString(text, objFont).Width;
                fontHeight = (int)objGraphics.MeasureString(text, objFont).Height;
                FontSize--;            
            }
            
            objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));
            objGraphics = Graphics.FromImage(objBmpImage);
            objGraphics.Clear(Color.Transparent);
            objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;
            objGraphics.DrawString(text, objFont, Brushes.White, new RectangleF(0, 0,400,155), strFormat);
            objGraphics.Flush();
            return (objBmpImage);

        }

        private Bitmap CreateBanner(string text)
        {

            Bitmap objBmpImage = new Bitmap(1, 1);
            Graphics objGraphics = Graphics.FromImage(objBmpImage);
            int intWidth = 758;
            int intHeight = 140;
            int FontSize = 255;
            Font objFont = new Font("Arial", FontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            int fontWidth = (int)objGraphics.MeasureString(text, objFont).Width;
            int fontHeight = (int)objGraphics.MeasureString(text, objFont).Height;
            while (fontWidth > intWidth || fontHeight > intHeight)
            {
                objFont = new Font("Arial", FontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
                fontWidth = (int)objGraphics.MeasureString(text, objFont).Width;
                fontHeight = (int)objGraphics.MeasureString(text, objFont).Height;
                FontSize--;
            }

            objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));
            objGraphics = Graphics.FromImage(objBmpImage);
            objGraphics.Clear(Color.Transparent);
            objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;
            objGraphics.DrawString(text, objFont, Brushes.White, new RectangleF(0, 0, 758, 140), strFormat);
            objGraphics.Flush();
            return (objBmpImage);

        }

        //make banner
        private void button4_Click(object sender, EventArgs e)
        {
            if (File.Exists(TVFolder + "\\" + folderList[comboBox1.SelectedIndex] + "\\banner.jpg"))
            {
                MessageBox.Show("Banner Already Exists");
            }
            else
            {
                Bitmap bmp1 = CreateBanner(folderList[comboBox1.SelectedIndex]);
                bmp1.Save(TVFolder + "\\" + folderList[comboBox1.SelectedIndex] + "\\banner.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        //make TV Show NFO
        private void button2_Click(object sender, EventArgs e)
        {
            int totalSize = ProcessDir(TVFolder + "\\" + folderList[comboBox1.SelectedIndex] + "\\", 0, 0);
            //MessageBox.Show(totalSize.ToString());
            ShowWrite(TVFolder + "\\" + folderList[comboBox1.SelectedIndex] + "\\tvshow.nfo", folderList[comboBox1.SelectedIndex],totalSize);
        }

        public void EpisodeWrite(string fileName,string title, int season,int episode)
        {
            var doc = new XDocument();

            var emp = new XElement("episodedetails", "");

            emp.Add(new XElement("title", title));
            emp.Add(new XElement("rating", ""));
            emp.Add(new XElement("season", season.ToString()));
            emp.Add(new XElement("episode", episode.ToString()));
            emp.Add(new XElement("plot", ""));
            emp.Add(new XElement("thumb", ""));
            emp.Add(new XElement("playcount","0"));
            emp.Add(new XElement("lastplayed", ""));
            emp.Add(new XElement("credits", ""));
            emp.Add(new XElement("director", ""));
            emp.Add(new XElement("aired", ""));
            emp.Add(new XElement("premiered", ""));
            emp.Add(new XElement("studio", ""));
            emp.Add(new XElement("displayseason", season.ToString()));
            emp.Add(new XElement("displayepisode", episode.ToString()));

            doc.Add(emp);

            doc.Save(fileName);
        }

        public void ShowWrite(string fileName, string title,int episodeCount)
        {
            var doc = new XDocument();

            var emp = new XElement("tvshow");

            emp.Add(new XElement("title", title));
            emp.Add(new XElement("rating",""));
            emp.Add(new XElement("year", "0"));
            emp.Add(new XElement("top250", "0"));
            emp.Add(new XElement("season", "-1"));
            emp.Add(new XElement("episode", episodeCount.ToString()));
            emp.Add(new XElement("displayseason", "-1"));
            emp.Add(new XElement("displayepisode", "-1"));
            emp.Add(new XElement("vote", ""));
            emp.Add(new XElement("outline", ""));

            emp.Add(new XElement("plot", ""));
            emp.Add(new XElement("tagline", ""));
            emp.Add(new XElement("runtime", ""));
            emp.Add(new XElement("playcount","0"));
            emp.Add(new XElement("episodeguide", ""));
            emp.Add(new XElement("id","tt338"));            

            doc.Add(emp);

            doc.Save(fileName);
        }

        public int ProcessDir(string sourceDir, int recursionLvl,int size)
        {
            if (recursionLvl <= HowDeepToScan)
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sourceDir);
                // Process the list of files found in the directory.
                foreach (System.IO.FileInfo fi in di.GetFiles("*"))
                {
                    string origName = fi.Name;
                    string exten = fi.Extension;
                    string attrib = fi.Attributes.ToString();

                    if (attrib == "Hidden, System, Archive")
                        continue;
                    //if thumb file dont convert
                    if (origName == "Thumbs.db")
                        continue;                    
                    //check if its a legal file type
                    if ((exten == ".avi" || exten == ".mkv" || exten == ".mp4" || exten == ".mpg" || exten == ".m4v" || exten == ".mpeg" || exten == ".mov" || exten == ".rm" || exten == ".rmvb" || exten == ".wmv" || exten == ".webm"))
                    {
                        size++;
                    }

                }

                // Recurse into subdirectories of this directory.
                string[] subdirEntries = Directory.GetDirectories(sourceDir);
                foreach (string subdir in subdirEntries)
                {
                    if (subdir == "$RECYCLE.BIN")
                    {
                        break;
                    }
                    // Do not iterate through reparse points
                    if ((File.GetAttributes(subdir) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                    {
                        size = ProcessDir(subdir, recursionLvl + 1, size);
                    }
                }
            }
            return size;
        }
    }
}
