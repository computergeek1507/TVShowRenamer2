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

namespace TV_Show_Renamer
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
                //List<string> fileData = infoFinder(TVfile.NewFileName, TVfile.FileExtention);
                //MessageBox.Show(fileData[0]+"\n"+fileData[1]+"\n"+fileData[2]+"\n"+fileData[3]);
                string nfoFile = TVfile.NewFullFileName.Replace(TVfile.FileExtention, ".nfo");

                //old way
                //EpisodeWrite(nfoFile, fileData[3], (Convert.ToInt32(fileData[1])), Convert.ToInt32(fileData[2]), Convert.ToInt32(fileData[1]) + (int)numericUpDown1.Value, Convert.ToInt32(fileData[2]) + (int)numericUpDown2.Value);
                //MessageBox.Show(nfoFile);

                //new way
                CreateEpisodeNfoFile(nfoFile, TVfile.FileTitle, TVfile.SeasonNum, TVfile.EpisodeNum, TVfile.SeasonNum, TVfile.EpisodeNum);
            }
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
            //old way
            //int totalSize = ProcessDir(TVFolder + "\\" + folderList[comboBox1.SelectedIndex] + "\\", 0, 0);
            //MessageBox.Show(totalSize.ToString());
            //ShowWrite(TVFolder + "\\" + folderList[comboBox1.SelectedIndex] + "\\tvshow.nfo", folderList[comboBox1.SelectedIndex],totalSize);

            //new way
            CreateTVShowNfoFile(TVFolder + "\\" + folderList[comboBox1.SelectedIndex] + "\\tvshow.nfo", folderList[comboBox1.SelectedIndex]);
        }

        public void EpisodeWrite(string fileName, string title, int season, int episode, int displayseason, int displayepisode)
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
            emp.Add(new XElement("displayseason", displayseason.ToString()));
            emp.Add(new XElement("displayepisode", displayepisode.ToString()));

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

        public void CreateEpisodeNfoFile(string fileName, string title, int season, int episode, int displayseason, int displayepisode)
        {
            // create document
            XDocument infoDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement rootElem = new XElement("episodedetails");


            // populate with correct nodes from series info and episode info
            rootElem.Add(new XElement("title", title ?? string.Empty));
            rootElem.Add(new XElement("rating",  string.Empty));
            rootElem.Add(new XElement("season", season));

            rootElem.Add(new XElement("episode", episode));
            

            rootElem.Add(new XElement("plot", string.Empty));
            rootElem.Add(new XElement("thumb", string.Empty));
            rootElem.Add(new XElement("playcount", 0));
            rootElem.Add(new XElement("lastplayed", string.Empty));
            rootElem.Add(new XElement("credits", string.Empty));
            rootElem.Add(new XElement("director", string.Empty));
            rootElem.Add(new XElement("aired", string.Empty));
            //rootElem.Add(new XElement("premiered", episodeInfo.FirstAired ?? string.Empty));
            rootElem.Add(new XElement("mpaa", string.Empty));
            rootElem.Add(new XElement("premiered",  string.Empty));
            rootElem.Add(new XElement("studio", string.Empty));

            // actors from series
            
                XElement actorElem = new XElement("actor");
                actorElem.Add(new XElement("name", string.Empty));
                actorElem.Add(new XElement("role", string.Empty));
                actorElem.Add(new XElement("thumb", string.Empty));
                rootElem.Add(actorElem);         

            

            infoDoc.Add(rootElem);            
            infoDoc.Save(fileName);
            
        }
        //<episodedetails>
        //<title>My TV Episode</title>
        //<rating>10.00</rating>
        //<season>2</season>
        //<episode>1</episode>
        //<plot>he best episode in the world</plot>
        //<thumb>http://thetvdb.com/banners/episodes/164981/2528821.jpg</thumb>
        //<playcount>0</playcount>
        //<lastplayed></lastplayed>
        //<credits>Writer</credits>
        //<director>Mr. Vision</director>
        //<aired>2000-12-31</aired>
        //<premiered>2010-09-24</premiered>
        //<studio>Production studio or channel</studio>
        //<mpaa>MPAA certification</mpaa>
        //<epbookmark>200</epbookmark>  <!-- For media files containing multiple episodes,
        //                                where value is the time where the next episode begins in seconds  -->
        //<displayseason>3</displayseason>  <!-- For TV show specials, determines how the episode is sorted in the series  -->
        //<displayepisode>4096</displayepisode>
        //<actor>
        //  <name>Little Suzie</name>
        //  <role>Pole Jumper/Dancer</role>
        //</actor>

        public void CreateTVShowNfoFile(string fileName, string title)
        {
            // create document
            XDocument infoDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement rootElem = new XElement("tvshow", new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"), new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"));
            

            // populate with correct nodes from series info

            rootElem.Add(new XElement("title", title ?? string.Empty));

            rootElem.Add(new XElement("id", string.Empty));
            rootElem.Add(new XElement("rating",  string.Empty));
            rootElem.Add(new XElement("mpaa",  string.Empty));
            rootElem.Add(new XElement("premiered",  string.Empty));
            rootElem.Add(new XElement("studio",  string.Empty));
            rootElem.Add(new XElement("plot",  string.Empty));
            // actors
            
                XElement actorElem = new XElement("actor");
                actorElem.Add(new XElement("name", string.Empty));
                actorElem.Add(new XElement("role", string.Empty));
                actorElem.Add(new XElement("thumb", string.Empty));
                rootElem.Add(actorElem);
            

            infoDoc.Add(rootElem);           
            infoDoc.Save(fileName);
           
        }

    }
}
