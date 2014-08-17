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
using System.Net.Sockets;
using System.Collections;
using System.Diagnostics;
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
//using MongoDB.Bson;
//using MongoDB.Driver;

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

        AlertForm alert;

		//get working directory

		string commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + Path.DirectorySeparatorChar + "TV Show Renamer Server";
		Thread _t;
		volatile bool _on = true;
		static Queue TheadQueue = new Queue();
		LogWrite MainLog = new LogWrite();//log object 
		List<CategoryInfo> CategoryList = new List<CategoryInfo>();
		List<TVShowSettings> _TVShowList = new List<TVShowSettings>();
		public static ListBoxLog listBoxLog;
		//NewTVDB TVDB;
		thexem theXEM;
        TMDb TMDbClient;
		
		public MainForm()
		{
			InitializeComponent();
			listBoxLog = new ListBoxLog(listBox1);
			//TVDB = new NewTVDB(commonAppData);
			theXEM = new thexem(commonAppData);
            TMDbClient = new TMDb(commonAppData);
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
			//winsock1.Listen(8089);
			//winsock1.
			_t = new Thread(new ThreadStart(serverRecieve));
			_t.Start();
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
			rootElem.Add(new XElement("title",	string.Empty));
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
				listBoxLog.Log(Level.Error, "Error Saving Preference File");
			}
			try
			{//write categoryXML file
				this.categorySave(commonAppData + Path.DirectorySeparatorChar + "CategoryList.xml", CategoryList);
			}
			catch (Exception)
			{
				listBoxLog.Log(Level.Error, "Error Saving CategoryList File");
			}

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

		}

		//load settings file
		public void loadStettings()
		{
			try
			{
				if (System.IO.File.Exists(commonAppData + Path.DirectorySeparatorChar + "preferences.seh"))//see if file exists
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
				if (System.IO.File.Exists(commonAppData + Path.DirectorySeparatorChar + "CategoryList.xml"))//see if file exists
				{
					this.categoryLoad(commonAppData + Path.DirectorySeparatorChar + "CategoryList.xml", CategoryList);
				}//end of if. 
			}
			catch (Exception e)
			{
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
			closeServer();
			//MainLog.closeLog();
		}

		private void button1_Click(object sender, EventArgs e)
		{
            if (backgroundWorker1.IsBusy != true)
            {
                // create a new instance of the alert form
                alert = new AlertForm();
                // event handler for the Cancel button in AlertForm
                alert.Canceled += new EventHandler<EventArgs>(buttonCancel_Click);
                alert.Show();
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
            }


            ////_TVShowList.Clear();
            //List<string> newitems = folderFinder(folderTextBox.Text);
            //foreach (string newTVShow in newitems) 
            //{
            // bool showFound = false;
            // for (int i = 0; i < _TVShowList.Count(); i++) 
            // {
            //    if(_TVShowList[i].ShowFolder.CompareTo(newTVShow)==0)
            //    {
            //         showFound = true;
            //         break;
            //    }				
            // }
            // if (!showFound)
            // {
            //    _TVShowList.Add(getTVShowSettings(new TVShowSettings(newTVShow, newTVShow)));
            // }
            //}


            //TVShowOptions main = new TVShowOptions(_TVShowList, folderTextBox.Text, commonAppData);
			//main.Show();

		}

		private void button2_Click(object sender, EventArgs e)
		{
			TVShowOptions main = new TVShowOptions(_TVShowList, folderTextBox.Text, commonAppData);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			openFileDialog1.Title = "Select Media files";
			openFileDialog1.Filter = "Video Files (*.avi;*.mkv;*.mp4;*.m4v;*.mpg;*.mov;*.mpeg;*.rm;*.rmvb;*.wmv;*.webm)|*.avi;*.mkv;*.mp4;*.m4v;*.mpg;*.mov;*.mpeg;*.rm;*.rmvb;*.wmv;*.webm|Archive Files (*.zip;*.rar;*.r01;*.7z;)|*.zip;*.rar;*.r01;*.7z;|All Files (*.*)|*.*";
			openFileDialog1.FileName = "";
			openFileDialog1.FilterIndex = 1;
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.CheckPathExists = true;

			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				TVClass newShow = TVRenamer.renameFile(openFileDialog1.SafeFileName);
				//openFileDialog1.FileName;
				
			}//end of if
			//MessageBox.Show(System.AppDomain.CurrentDomain.FriendlyName);
			//MessageBox.Show(System.Reflection.Assembly.GetEntryAssembly().Location);
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
						FileList.Add(new LoadFileInfo(origName,fi.DirectoryName,exten));
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
			//	 XElement rootElem = new XElement("category");

			//	 rootElem.Add(new XElement("name", saveItem[i].CategoryTitle));
			//	 rootElem.Add(new XElement("command", saveItem[i].CommandWords));
			//	 rootElem.Add(new XElement("folder", saveItem[i].SearchFolder));

			//	 rootElem.Add(new XElement("outputoption", saveItem[i].FolderOptions));

			//	 MainrootElem.Add(rootElem);
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
			//			 select new
			//			 {
			//				Name = Category.Element("name").Value,
			//				Command = Category.Element("command").Value,
			//				Folder = Category.Element("folder").Value,
			//				OutputOption = Category.Element("outputoption").Value
			//			 };

			//foreach (var wd in Categorys)
			//{
			//	loadedItem.Add(new CategoryInfo(wd.Name,wd.Command,wd.Folder,Int32.Parse(wd.OutputOption)));
			//	 //Console.WriteLine("Widget at ({0}) has a category of {1}", wd.URL, wd.Category);
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

            if (!Directory.Exists(folderwatch))
                return foldersIn;

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
			//	 revFoldersIn.Add(foldersIn[y - 1]);
			//return revFoldersIn;
		}

		private void button6_Click(object sender, EventArgs e)
		{
			try
			{
				var xbmc = new XBMCRPC.Client("192.168.5.145", 80, "xbmc", "");
				var ret0 = xbmc.JSONRPC.Introspect();
				//var ret2 = xbmc.VideoLibrary.GetTVShows(TVShow.AllFields());
				//var ret66 = xbmc.VideoLibrary.GetTVShowDetails(0);
				var ret55 = xbmc.VideoLibrary.Scan();
				string temp = "";
				//var ret1 =	xbmc.Application.GetProperties(Client.AllValues<Name>());
				//var ret2 =	xbmc.VideoLibrary.GetTVShows(TVShow.AllFields());
				//var ret3 =	xbmc.VideoLibrary.SetMovieDetails(5801, playcount: 10);
				//var ret4 =	xbmc.VideoLibrary.GetMovies(Movie.AllFields(), new Limits() { start = 1566, end = 1570 });
				//var ret4a =	xbmc.Files.PrepareDownload(ret4.movies[0].thumbnail);

				//var ret5 =	xbmc.Files.GetSources();
				//var ret6 = await xbmc.Files.GetDirectory(@"C:\Users\steve_000\Music\Amazon MP3\die ärzte\auch", Media.music, Files.AllFields());
				//var ret7 =	xbmc.Playlist.GetItems(0, properties: All.AllFields());
				//var ret7a =	xbmc.Playlist.GetItems(1, properties: All.AllFields());
				//var ret8 =	xbmc.Playlist.GetPlaylists();
				//var ret9 =	xbmc.Player.GetActivePlayers();
			}
			catch(Exception exp)
			{
				MessageBox.Show(exp.Message.ToString());
			}
		}

		private TVShowSettings getTVShowSettings(TVShowSettings newShow) 
		{
			//TVShowSettings newShow = new TVShowSettings(showFolderName, showFolderName);

            OnlineShowInfo newTVDBID = theXEM.findTitle(newShow.SearchName);

			if (newTVDBID.ShowID != -1)
			{
				newShow.TVDBSeriesID = newTVDBID.ShowID;
				newShow.TVDBShowName = newTVDBID.ShowName;
                newShow.SeriesEnded = newTVDBID.ShowEnded;
				
			}
            OnlineShowInfo newTMDbID = TMDbClient.findTitle(newShow.SearchName);
            if (newTMDbID.ShowID != -1)
			{
                newShow.TMDbSeriesID = newTMDbID.ShowID;
                newShow.TMDbShowName = newTMDbID.ShowName;

                newShow.SeriesEnded = newTVDBID.ShowEnded;
			}

			return newShow;
		}

		private void serverRecieve()
		{
			IPAddress ipAd = IPAddress.Parse("192.168.5.148");
			// use local m/c IP address, and 
			// use the same in the client

			/* Initializes the Listener */
			TcpListener myList = new TcpListener(IPAddress.Any, 8089);

			/* Start Listeneting at the specified port */
			myList.Start();
			
			while (_on)
			{
				Debug.WriteLine("Waiting for a connection.....");

				Socket s = myList.AcceptSocket();
				Debug.WriteLine("Connection accepted from " + s.RemoteEndPoint);

				byte[] b = new byte[100];
				int k = s.Receive(b);
				Debug.WriteLine("Recieved...");
				string temp = "";
				for (int i = 0; i < k; i++)
					temp = temp+ Convert.ToChar(b[i]);

				string returnValue = "";
				if (temp != "Close")
				{

				}

				ASCIIEncoding asen = new ASCIIEncoding();
				s.Send(asen.GetBytes(returnValue));
				Debug.WriteLine("\nSent Acknowledgement");
				/* clean up */
				s.Close();
			}
			myList.Stop();

		}

		private void closeServer() 
		{
			//return;
			_on = false;
			try
			{
				TcpClient tcpclnt = new TcpClient("127.0.0.1", 8089);
				string closeStr = "Close";
				Stream stm = tcpclnt.GetStream();
				ASCIIEncoding asen = new ASCIIEncoding();
				byte[] ba = asen.GetBytes(closeStr);
				stm.Write(ba, 0, ba.Length);
				byte[] bb = new byte[100];
				int k = stm.Read(bb, 0, 100);

				string temp = "";
				for (int i = 0; i < k; i++)
					temp = temp + Convert.ToChar(bb[i]);

				tcpclnt.Close();
			}
			catch(Exception ex )
			{
				Debug.WriteLine(ex.Message);
			}
		
		}

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
                // Close the AlertForm
                alert.Close();
            }
        }

        // This event handler is where the time-consuming work is done.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            //for (int i = 1; i <= 10; i++)
            //{
            //    if (worker.CancellationPending == true)
            //    {
            //        e.Cancel = true;
            //        break;
            //    }
            //    else
            //    {
            //        // Perform a time consuming operation and report progress.
            //        worker.ReportProgress(i * 10);
            //        System.Threading.Thread.Sleep(500);
            //    }
            //}

            //_TVShowList.Clear();
            List<string> newitems = folderFinder(folderTextBox.Text);
            int index = 0;
            foreach (string folderName in newitems)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

                bool HD = false;
                string newTVShow = folderName;
                if (newTVShow.EndsWith("HD"))
                {
                    newTVShow = newTVShow.Replace("HD", "").TrimEnd(' ');
                    HD = true;
                }

                bool showFound = false;
                int showIndex = -1;
                for (int i = 0; i < _TVShowList.Count(); i++)
                {
                    if (_TVShowList[i].SearchName.CompareTo(newTVShow) == 0)
                    {
                        showIndex = i;
                        break;
                    }
                }
                if (showIndex==-1)
                {
                    if(HD)
                        _TVShowList.Add(getTVShowSettings(new TVShowSettings(newTVShow, folderName,true)));
                    else
                        _TVShowList.Add(getTVShowSettings(new TVShowSettings(newTVShow, folderName)));
                }
                else 
                {
                    if (HD)
                    {
                        _TVShowList[showIndex].ShowFolderHD = folderName;
                        _TVShowList[showIndex].GetHD = true;
                    }
                    else
                    {
                        _TVShowList[showIndex].ShowFolder = folderName;
                    }
                }
                index++;
                int percentage = (int)(((double)index / (double)newitems.Count()) * 100.0);
                worker.ReportProgress(percentage);
            }


           // TVShowOptions main = new TVShowOptions(_TVShowList, folderTextBox.Text, commonAppData);
        }

        // This event handler updates the progress.
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Show the progress in main form (GUI)
           // labelResult.Text = (e.ProgressPercentage.ToString() + "%");
            // Pass the progress to AlertForm label and progressbar
            alert.Message = "In progress, please wait... " + e.ProgressPercentage.ToString() + "%";
            alert.ProgressValue = e.ProgressPercentage;
        }

        // This event handler deals with the results of the background operation.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                //labelResult.Text = "Canceled!";
                listBoxLog.Log("TV Search Canceled!");
            }
            else if (e.Error != null)
            {
               // labelResult.Text = "Error: " + e.Error.Message;
                listBoxLog.Log("TV Search Error: " + e.Error.Message);
            }
            else
            {
                TVShowOptions main = new TVShowOptions(_TVShowList, folderTextBox.Text, commonAppData);
                listBoxLog.Log("TV Search Done!");
               // labelResult.Text = "Done!";
            }
            // Close the AlertForm
            alert.Close();
        }

	}
}
