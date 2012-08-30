using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.Collections;
using System.Diagnostics;

namespace TV_Show_Renamer_Server
{
    public partial class MainForm : Form
    {
        #region Standby Stuff
        //standby crap
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(
        EXECUTION_STATE flags);

        [Flags]
        public enum EXECUTION_STATE : uint
        {
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_DISPLAY_REQUIRED = 0x00000002,
            // ES_USER_PRESENT = 0x00000004,
            ES_CONTINUOUS = 0x80000000
        }

        #endregion
        const int appVersion = 000;//ALPHA
        const int HowDeepToScan = 4;

        //get working directory
        string commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\TV Show Renamer Server";
        Thread t;
        bool on = true;
        static Queue TheadQueue = new Queue();
        LogWrite MainLog = new LogWrite();//log object 
        List<CategoryInfo> CategoryList = new List<CategoryInfo>();
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //check or create file folder
            if (!(File.Exists(commonAppData)))
            {
                System.IO.Directory.CreateDirectory(commonAppData);
            }
            MainLog.startLog(commonAppData);
            loadStettings();

            //disable standby
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
            
            //Start Server
            
        }

        

        private void folderSelectButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                folderTextBox.Text = folderBrowserDialog1.SelectedPath;            
        }        

        public void XmlRead(string fileLocation)
        {
            var doc = XDocument.Load(fileLocation + "//webversion.xml");

            var emp = doc.Descendants("version").FirstOrDefault();

            //numericUpDown1.Value = decimal.Parse(emp.Element("application").Value);
            //numericUpDown2.Value = decimal.Parse(emp.Element("library").Value);
            //numericUpDown3.Value = decimal.Parse(emp.Element("settings").Value);
        }

        public void XmlWrite(string fileLocation)
        {
            var doc = new XDocument();

            var emp = new XElement("version");

            //emp.Add(new XElement("application", numericUpDown1.Value.ToString()));
            //emp.Add(new XElement("library", numericUpDown2.Value.ToString()));
            //emp.Add(new XElement("settings", numericUpDown3.Value.ToString()));

            doc.Add(emp);

            doc.Save(fileLocation + "//webversion.xml");
        }

        public void DataBaseWrite(string fileName)
        {
            // create document
            XDocument infoDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement rootElem = new XElement("episodedetails");


            // populate with correct nodes from series info and episode info
            rootElem.Add(new XElement("title",  string.Empty));
            rootElem.Add(new XElement("rating", string.Empty));
            rootElem.Add(new XElement("season", string.Empty));

            rootElem.Add(new XElement("episode", string.Empty));


            rootElem.Add(new XElement("plot", string.Empty));
            rootElem.Add(new XElement("thumb", string.Empty));
            rootElem.Add(new XElement("playcount", 0));
            rootElem.Add(new XElement("lastplayed", string.Empty));
            rootElem.Add(new XElement("credits", string.Empty));
            rootElem.Add(new XElement("director", string.Empty));
            rootElem.Add(new XElement("aired", string.Empty));
            rootElem.Add(new XElement("premiered", string.Empty));
            rootElem.Add(new XElement("mpaa", string.Empty));
            rootElem.Add(new XElement("premiered", string.Empty));
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

        //kills process        
        
        //save settings
        public void saveStettings()
        {
            try
            {//write newpreferences file
                StreamWriter pw = new StreamWriter(commonAppData + "//preferences.seh");
                if(folderTextBox.Text!= null && folderTextBox.Text!="")
                    pw.WriteLine(folderTextBox.Text);
           
                pw.Close();//close writer stream
            }
            catch (Exception)
            {
                
            }
            try
            {//write categoryXML file
                this.categorySave(commonAppData + "//CategoryList.xml", CategoryList);
            }
            catch (Exception)
            {

            }
           
        }

        //load settings file
        public void loadStettings()
        {
            try
            {
                if (File.Exists(commonAppData + "//preferences.seh"))//see if file exists
                {
                    StreamReader tr3 = new StreamReader(commonAppData + "//preferences.seh");
                    var readtemp = tr3.ReadLine();
                    if (readtemp != null) folderTextBox.Text = readtemp;
                    tr3.Close();//close reader stream                                        
                }//end of if. 
            }
            catch (Exception)
            {
               
            }

            try
            {
                if (File.Exists(commonAppData + "//CategoryList.xml"))//see if file exists
                {
                    this.categoryLoad(commonAppData + "//CategoryList.xml", CategoryList);                
                }//end of if. 
            }
            catch (Exception)
            {

            }
            
        }//end of loadsettings methods

        private void addFilesToThread(string command)
        {
            TheadQueue.Enqueue(command);
            if (!convertionThread.IsBusy)
                convertionThread.RunWorkerAsync();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            saveStettings();
            MainLog.closeLog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TVShowOptions main = new TVShowOptions();
            main.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(System.AppDomain.CurrentDomain.FriendlyName);
            MessageBox.Show(System.Reflection.Assembly.GetEntryAssembly().Location);
        }

        //write log called by download form
        public void writeLog(string error)
        {
            MainLog.WriteLog(error);
        }
        
        //new way to add files from folder
        private void ProcessDir(string sourceDir, int recursionLvl,List<LoadFileInfo> FileList,List<string> FoldersList )
        {
            if (sourceDir.Contains("Program Files") || sourceDir.Contains("Program Files (x86)") || sourceDir.Contains("$Recycle.Bin") || sourceDir.Contains("$RECYCLE.BIN"))            
                return;
            
            if (recursionLvl <= HowDeepToScan )
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
                        FileList.Add(new LoadFileInfo(origName,fi.DirectoryName,  exten));
                        if (recursionLvl > 1)                        
                            FoldersList.Add(fi.DirectoryName);                        
                    }                   
                }

                // Recurse into subdirectories of this directory.
                string[] subdirEntries = Directory.GetDirectories(sourceDir);
                foreach (string subdir in subdirEntries)
                {
                    if (subdir.Contains("Program Files") || subdir.Contains("Program Files (x86)") || subdir.Contains("$Recycle.Bin") || subdir.Contains("$RECYCLE.BIN"))                    
                        continue;                    
                    // Do not iterate through reparse points
                    if ((File.GetAttributes(subdir) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)                    
                        ProcessDir(subdir, recursionLvl + 1, FileList, FoldersList);                    
                }
            }
        }

        //CategoryEdit
        private void button4_Click(object sender, EventArgs e)
        {
            CategoryEdit mainCategoryWindow = new CategoryEdit(CategoryList);
            mainCategoryWindow.Show();
        }

        //create XML file with Category Names and such
        private void categorySave(string saveLocation, List<CategoryInfo> saveItem)
        {
            if (saveItem.Count == 0) return;
            XDocument infoDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement MainrootElem = new XElement("categorys");
            for (int i = 0; i < saveItem.Count(); i++)
            {
                XElement rootElem = new XElement("category");

                rootElem.Add(new XElement("name", saveItem[i].CategoryTitle));
                rootElem.Add(new XElement("command", saveItem[i].CommandWords));
                rootElem.Add(new XElement("folder", saveItem[i].SearchFolder));

                rootElem.Add(new XElement("outputoption", saveItem[i].FolderOptions));

                MainrootElem.Add(rootElem);
            }
            infoDoc.Add(MainrootElem);
            infoDoc.Save(saveLocation);
        }

        //create XML file with Category Names and such
        private void categoryLoad(string FileLocation, List<CategoryInfo> loadedItem)
        {
            //var doc = XDocument.Load(FileLocation);
            //var emp = doc.Descendants("category").FirstOrDefault();
            //numericUpDown1.Value = decimal.Parse(emp.Element("application").Value);
            //numericUpDown2.Value = decimal.Parse(emp.Element("library").Value);
            //numericUpDown3.Value = decimal.Parse(emp.Element("settings").Value);

            XDocument CategoryXML = XDocument.Load(FileLocation);

            var Categorys = from Category in CategoryXML.Descendants("category")
                          select new
                          {
                              Name = Category.Element("name").Value,
                              Command = Category.Element("command").Value,
                              Folder = Category.Element("folder").Value,
                              OutputOption = Category.Element("outputoption").Value
                          };

            foreach (var wd in Categorys)
            {
               loadedItem.Add(new CategoryInfo(wd.Name,wd.Command,wd.Folder,Int32.Parse(wd.OutputOption)));
                //Console.WriteLine("Widget at ({0}) has a category of {1}", wd.URL, wd.Category);
            }
        }

        //do work thread
        private void convertionThread_DoWork(object sender, DoWorkEventArgs e)
        {
            string command = (string)TheadQueue.Dequeue();
            if (command!= "")
            {
                
            }
        }

        private void trace(string str)
        {
            listBox1.Items.Add(str);
        }
    }
}
