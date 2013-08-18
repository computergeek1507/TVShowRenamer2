using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.Collections;
using System.Diagnostics;
<<<<<<< .mine
using Microsoft.CSharp;
using TweetSharp;
//using MongoDB.Bson;
//using MongoDB.Driver;
using XBMCRPC;
using XBMCRPC.Application.Property;
using XBMCRPC.Files;
using XBMCRPC.List;
using XBMCRPC.List.Item;
using XBMCRPC.Methods;
using XBMCRPC.Video.Fields;
=======
using Microsoft.CSharp;
using TweetSharp;
//using MongoDB.Bson;
//using MongoDB.Driver;
>>>>>>> .r102380


namespace TV_Show_Renamer_Server
{
    public partial class MainForm : Form
    {
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
		List<FileSystemWatcher> myWatcher1 = new List<FileSystemWatcher>();


        //get working directory
<<<<<<< .mine
		string commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + Path.DirectorySeparatorChar + "TV Show Renamer Server";
        //Thread t;
        //bool on = true;
=======
		string commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + Path.DirectorySeparatorChar + "TV Show Renamer Server";
        Thread t;
        bool on = true;
>>>>>>> .r102380
        static Queue TheadQueue = new Queue();
        LogWrite MainLog = new LogWrite();//log object 
        List<CategoryInfo> CategoryList = new List<CategoryInfo>();
		List<TVShowSettings> _TVShowList = new List<TVShowSettings>();
        public static ListBoxLog listBoxLog;
		NewTVDB TVDB;
		thexem theXEM;
        
        public MainForm()
        {
            InitializeComponent();
            listBoxLog = new ListBoxLog(listBox1);
			TVDB = new NewTVDB(commonAppData);
			theXEM = new thexem(commonAppData);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //check or create file folder
			if (!(System.IO.File.Exists(commonAppData)))
            {
                System.IO.Directory.CreateDirectory(commonAppData);
            }
            //MainLog.startLog(commonAppData);
            loadStettings();

            //disable standby
            SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
            
            //Start Server
            listBoxLog.Log(Level.Info, "Server Started");
            
        }        

        private void folderSelectButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                folderTextBox.Text = folderBrowserDialog1.SelectedPath;
        }        

        public void XmlRead(string fileLocation)
        {
			var doc = XDocument.Load(fileLocation + Path.DirectorySeparatorChar + "webversion.xml");

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

			doc.Save(fileLocation + Path.DirectorySeparatorChar + "webversion.xml");
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

        //save settings
        public void saveStettings()
        {
            try
            {//write newpreferences file
				StreamWriter pw = new StreamWriter(commonAppData + Path.DirectorySeparatorChar + "preferences.seh");
                if(folderTextBox.Text!= null && folderTextBox.Text!="")
                    pw.WriteLine(folderTextBox.Text);
           
                pw.Close();//close writer stream
            }
            catch (Exception)
            {
<<<<<<< .mine
				listBoxLog.Log(Level.Error, "Error Saving Preference File");
=======
				listBoxLog.Log(Level.Error, "Error Loading Preference File");
>>>>>>> .r102380
            }
            try
            {//write categoryXML file
				this.categorySave(commonAppData + Path.DirectorySeparatorChar + "CategoryList.xml", CategoryList);
            }
            catch (Exception)
            {
<<<<<<< .mine
				listBoxLog.Log(Level.Error, "Error Saving CategoryList File");
            }
=======
				listBoxLog.Log(Level.Error, "Error Loading CategoryList File");
            }
>>>>>>> .r102380

<<<<<<< .mine
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(List<TVShowSettings> ));
				TextWriter writer = new StreamWriter(commonAppData + Path.DirectorySeparatorChar + "TVShowList.xml");
				serializer.Serialize(writer, _TVShowList);
				writer.Close();
			}
			catch (Exception e )
			{

				listBoxLog.Log(Level.Error, "Error Saving TV Show File");
				MessageBox.Show(e.Message);
			}
=======
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(List<TVShowSettings> ));
				TextWriter writer = new StreamWriter(commonAppData + Path.DirectorySeparatorChar + "TVShowList.xml");
				serializer.Serialize(writer, _TVShowList);
				writer.Close();
			}
			catch (Exception e )
			{

				listBoxLog.Log(Level.Error, "Error Loading TV Show File");
				MessageBox.Show(e.Message);
			}
>>>>>>> .r102380
        }

        //load settings file
        public void loadStettings()
        {
            try
            {
<<<<<<< .mine
				if (System.IO.File.Exists(commonAppData + Path.DirectorySeparatorChar + "preferences.seh"))//see if file exists
=======
				if (File.Exists(commonAppData + Path.DirectorySeparatorChar + "preferences.seh"))//see if file exists
>>>>>>> .r102380
                {
					StreamReader tr3 = new StreamReader(commonAppData + Path.DirectorySeparatorChar + "preferences.seh");
                    var readtemp = tr3.ReadLine();
                    if (readtemp != null) folderTextBox.Text = readtemp;
                    tr3.Close();//close reader stream
                }//end of if. 
            }
            catch (Exception)
            {
				listBoxLog.Log(Level.Error, "Error Loading Preferences File");
            }

            try
            {
<<<<<<< .mine
				if (System.IO.File.Exists(commonAppData + Path.DirectorySeparatorChar + "CategoryList.xml"))//see if file exists
=======
				if (File.Exists(commonAppData + Path.DirectorySeparatorChar + "CategoryList.xml"))//see if file exists
>>>>>>> .r102380
                {
					this.categoryLoad(commonAppData + Path.DirectorySeparatorChar + "CategoryList.xml", CategoryList);
                }//end of if. 
            }
            catch (Exception e)
            {
<<<<<<< .mine
				listBoxLog.Log(Level.Error, "Error Loading Category File");
				MessageBox.Show(e.Message);
            }
			try
			{
				if (System.IO.File.Exists(commonAppData + Path.DirectorySeparatorChar + "TVShowList.xml"))//see if file exists
				{
					XmlSerializer serializer = new XmlSerializer(typeof(List<TVShowSettings>));
					FileStream reader = new FileStream(commonAppData + Path.DirectorySeparatorChar + "TVShowList.xml", FileMode.Open);
					_TVShowList = (List<TVShowSettings>)serializer.Deserialize(reader);
					reader.Close();
				}
			}
			catch (Exception e)
			{
=======
				listBoxLog.Log(Level.Error, "Error Loading Category File");
				MessageBox.Show(e.Message);
            }
			try
			{
				if (File.Exists(commonAppData + Path.DirectorySeparatorChar + "TVShowList.xml"))//see if file exists
				{
					XmlSerializer serializer = new XmlSerializer(typeof(List<TVShowSettings>));
					FileStream reader = new FileStream(commonAppData + Path.DirectorySeparatorChar + "TVShowList.xml", FileMode.Open);
					_TVShowList = (List<TVShowSettings>)serializer.Deserialize(reader);
					reader.Close();
				}
			}
			catch (Exception e)
			{
>>>>>>> .r102380

<<<<<<< .mine
				listBoxLog.Log(Level.Error, "Error Loading TV Show File");
				MessageBox.Show(e.Message);
			}

			try
			{
				if ((Directory.Exists(commonAppData + Path.DirectorySeparatorChar + "Temp")))
				{
						Directory.Delete(commonAppData + Path.DirectorySeparatorChar + "Temp", true);
				}

				Directory.CreateDirectory(commonAppData + Path.DirectorySeparatorChar + "Temp");
			}
			catch (Exception e)
			{
				log.Error("Can't Delete Temp Folder",e);
			}
=======
				listBoxLog.Log(Level.Error, "Error Loading TV Show File");
				MessageBox.Show(e.Message);
			}
>>>>>>> .r102380
            
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
            //MainLog.closeLog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
			_TVShowList.Clear();
            List<string> newitems = folderFinder(folderTextBox.Text);
<<<<<<< .mine
			foreach (string newTVShow in newitems) 
			{
				_TVShowList.Add(getTVShowSettings(newTVShow));
			}


			TVShowOptions main = new TVShowOptions(_TVShowList, folderTextBox.Text, commonAppData);
            //main.Show();
=======
			foreach (string newTVShow in newitems) 
			{
				_TVShowList.Add(new TVShowSettings(newTVShow, newTVShow));
			}


			TVShowOptions main = new TVShowOptions(_TVShowList, folderTextBox.Text);
            main.Show();
>>>>>>> .r102380
        }

        private void button2_Click(object sender, EventArgs e)
        {
<<<<<<< .mine
			TVShowOptions main = new TVShowOptions(_TVShowList, folderTextBox.Text, commonAppData);
            //main.Show();
=======
			TVShowOptions main = new TVShowOptions(_TVShowList, folderTextBox.Text);
            main.Show();
>>>>>>> .r102380
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
					if ((System.IO.File.GetAttributes(subdir) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)                    
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

			XmlSerializer serializer = new XmlSerializer(typeof(List<CategoryInfo>));
			TextWriter writer = new StreamWriter(saveLocation);
			serializer.Serialize(writer, saveItem);
			writer.Close();

			//XDocument infoDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
			//XElement MainrootElem = new XElement("categorys");
			//for (int i = 0; i < saveItem.Count(); i++)
			//{
			//    XElement rootElem = new XElement("category");

			//    rootElem.Add(new XElement("name", saveItem[i].CategoryTitle));
			//    rootElem.Add(new XElement("command", saveItem[i].CommandWords));
			//    rootElem.Add(new XElement("folder", saveItem[i].SearchFolder));

			//    rootElem.Add(new XElement("outputoption", saveItem[i].FolderOptions));

			//    MainrootElem.Add(rootElem);
			//}
			//infoDoc.Add(MainrootElem);
			//infoDoc.Save(saveLocation);
        }

        //create XML file with Category Names and such
        private void categoryLoad(string FileLocation, List<CategoryInfo> loadedItem)
        {
			XmlSerializer serializer = new XmlSerializer(typeof(List<CategoryInfo>));
			//TextWriter writer = new StreamWriter(FileLocation);
			//serializer.Serialize(writer, firstInstance);
			//writer.Close();

			FileStream reader = new FileStream(FileLocation, FileMode.Open);

			loadedItem = (List < CategoryInfo > )serializer.Deserialize(reader);

			reader.Close();

            //var doc = XDocument.Load(FileLocation);
            //var emp = doc.Descendants("category").FirstOrDefault();
            //numericUpDown1.Value = decimal.Parse(emp.Element("application").Value);
            //numericUpDown2.Value = decimal.Parse(emp.Element("library").Value);
            //numericUpDown3.Value = decimal.Parse(emp.Element("settings").Value);

			//XDocument CategoryXML = XDocument.Load(FileLocation);

			//var Categorys = from Category in CategoryXML.Descendants("category")
			//              select new
			//              {
			//                  Name = Category.Element("name").Value,
			//                  Command = Category.Element("command").Value,
			//                  Folder = Category.Element("folder").Value,
			//                  OutputOption = Category.Element("outputoption").Value
			//              };

			//foreach (var wd in Categorys)
			//{
			//   loadedItem.Add(new CategoryInfo(wd.Name,wd.Command,wd.Folder,Int32.Parse(wd.OutputOption)));
			//    //Console.WriteLine("Widget at ({0}) has a category of {1}", wd.URL, wd.Category);
			//}
        }

        //do work thread
        private void convertionThread_DoWork(object sender, DoWorkEventArgs e)
        {
            string command = (string)TheadQueue.Dequeue();
            if (command!= "")
            {
                
            }
        }

        private string infoFinder(string showName, List<string> folderlist)
        {
            int indexofTVshow = -1;
            foreach (string folderName in folderlist)// (int ifolder = 0; ifolder < folderlist.Count(); ifolder++)
            {
                int difference = Math.Abs(folderName.Length - showName.Length);
                indexofTVshow = folderName.IndexOf(showName, StringComparison.InvariantCultureIgnoreCase);
                if (indexofTVshow != -1 && difference < 3)
                    return folderName;
            }//end of for loop            
            return "";
        }//end of infofinder method

        //get list of folders
        private List<string> folderFinder(string folderwatch)
        {
            List<string> foldersIn = new List<string>();
			//List<string> revFoldersIn = new List<string>();

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folderwatch);
            try
            {
				foreach (System.IO.DirectoryInfo fi in di.GetDirectories())
				{
					if(!fi.Name.StartsWith("."))
						foldersIn.Add(fi.Name);
				}
            }
            catch (IOException)
            {
                return foldersIn;
            }
            foldersIn.Sort();
			return foldersIn;
			//for (int y = foldersIn.Count(); y > 0; y--)
			//    revFoldersIn.Add(foldersIn[y - 1]);
			//return revFoldersIn;
        }
<<<<<<< .mine

		private void button5_Click(object sender, EventArgs e)
		{
			//http://192.168.5.148:8081/api/5ae981cf1517ca6fef6e1a61256fc0cd/?cmd=shows

			WebClient client = new WebClient();
			Stream stream = client.OpenRead("http://192.168.5.148:8081/api/5ae981cf1517ca6fef6e1a61256fc0cd/?cmd=shows");
			StreamReader reader = new StreamReader(stream);

			//Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadLine());

			// instead of WriteLine, 2 or 3 lines of code here using WebClient to download the file
			//Console.WriteLine((string)jObject["albums"][0]["cover_image_url"]);

			var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());
			var alldata = results["data"];
			//var name = alldata[0];

			//dynamic parsedObject = Newtonsoft.Json.JsonConvert.DeserializeObject(alldata);
			foreach (dynamic entry in alldata)
			{
				string TVDB = entry.Name; // "test"
				dynamic value = entry.Value; // { inner: "text-value" }
				string ShowName = (string)value["show_name"];
				for (int i = 0; i < _TVShowList.Count;i++ )
				{
					if (ShowName.CompareTo(_TVShowList[i].SearchName) == 0)
					{
						_TVShowList[i].TVDBShowName = ShowName;
						_TVShowList[i].TVDBSeriesID = Int32.Parse(TVDB);
						_TVShowList[i].TVRageShowName = (string)value["tvrage_name"];
						_TVShowList[i].TVRageSeriesID = Int32.Parse((string)value["tvrage_id"]);
						if (((string)value["tvrage_id"]) == "Ended")
							_TVShowList[i].SeriesEnded = true;
						break;
					}

				}

				
			}

			stream.Close();

			
		}

		private void button6_Click(object sender, EventArgs e)
		{
			var xbmc = new XBMCRPC.Client("192.168.5.145", 80, "xbmc", "");
            var ret0 =  xbmc.JSONRPC.Introspect();
			//var ret2 = xbmc.VideoLibrary.GetTVShows(TVShow.AllFields());
			//var ret66 = xbmc.VideoLibrary.GetTVShowDetails(0);
            var ret55 =  xbmc.VideoLibrary.Scan();
			string temp = "";
            //var ret1 =  xbmc.Application.GetProperties(Client.AllValues<Name>());
            //var ret2 =  xbmc.VideoLibrary.GetTVShows(TVShow.AllFields());
            //var ret3 =  xbmc.VideoLibrary.SetMovieDetails(5801, playcount: 10);
            //var ret4 =  xbmc.VideoLibrary.GetMovies(Movie.AllFields(), new Limits() { start = 1566, end = 1570 });
            //var ret4a =  xbmc.Files.PrepareDownload(ret4.movies[0].thumbnail);
            
            //var ret5 =  xbmc.Files.GetSources();
            //var ret6 = await xbmc.Files.GetDirectory(@"C:\Users\steve_000\Music\Amazon MP3\die ärzte\auch", Media.music, Files.AllFields());
            //var ret7 =  xbmc.Playlist.GetItems(0, properties: All.AllFields());
            //var ret7a =  xbmc.Playlist.GetItems(1, properties: All.AllFields());
            //var ret8 =  xbmc.Playlist.GetPlaylists();
            //var ret9 =  xbmc.Player.GetActivePlayers();
		}

		private TVShowSettings getTVShowSettings(string showFolderName) 
		{
			TVShowSettings newShow = new TVShowSettings(showFolderName, showFolderName);

			OnlineShowInfo newTVDBID = TVDB.findTitle(showFolderName);
			if (newTVDBID.ShowID != -1)
			{
				newShow.TVDBSeriesID = newTVDBID.ShowID;
				newShow.TVDBShowName = newTVDBID.ShowName;
				
			}
			OnlineShowInfo newRageID = TVRage.findTitle(showFolderName);
			if (newRageID.ShowID != -1)
			{
				newShow.TVRageSeriesID = newRageID.ShowID;
				newShow.TVRageShowName = newRageID.ShowName;
			}

			return newShow;
		}
		
	}
=======

		private void button5_Click(object sender, EventArgs e)
		{
			//http://192.168.5.148:8081/api/5ae981cf1517ca6fef6e1a61256fc0cd/?cmd=shows

			WebClient client = new WebClient();
			Stream stream = client.OpenRead("http://192.168.5.148:8081/api/5ae981cf1517ca6fef6e1a61256fc0cd/?cmd=shows");
			StreamReader reader = new StreamReader(stream);

			//Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadLine());

			// instead of WriteLine, 2 or 3 lines of code here using WebClient to download the file
			//Console.WriteLine((string)jObject["albums"][0]["cover_image_url"]);

			var results = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());
			var alldata = results["data"];
			//var name = alldata[0];

			//dynamic parsedObject = Newtonsoft.Json.JsonConvert.DeserializeObject(alldata);
			foreach (dynamic entry in alldata)
			{
				string TVDB = entry.Name; // "test"
				dynamic value = entry.Value; // { inner: "text-value" }
				string ShowName = (string)value["show_name"];
				for (int i = 0; i < _TVShowList.Count;i++ )
				{
					if (ShowName.CompareTo(_TVShowList[i].SearchName) == 0)
					{
						_TVShowList[i].TVDBShowName = ShowName;
						_TVShowList[i].TVDBSeriesID = Int32.Parse(TVDB);
						_TVShowList[i].TVRageShowName = (string)value["tvrage_name"];
						_TVShowList[i].TVRageSeriesID = Int32.Parse((string)value["tvrage_id"]);
						if (((string)value["tvrage_id"]) == "Ended")
							_TVShowList[i].SeriesEnded = true;
						break;
					}

				}

				
			}

			stream.Close();

			
		}
        
    }
>>>>>>> .r102380
}
