using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;

namespace TV_Show_Renamer
{
	public class TVShowInfo
	{
		//public TVShowInfo() { }
		public TVShowInfo(string tVShowName)
		{
			_TVShowName = tVShowName;
		}
		public TVShowInfo(string tVShowName, string tVShowFolder, int tvdbID, string tVShowNametvdb, int rageTVID, string tVShowNamerage, string epguidesID,string tVShowNameepg)
		{
			_TVShowName = tVShowName;
			if (tVShowFolder != "") _TVShowFolder = tVShowFolder;
			if (tvdbID != -1) _tvdbID = tvdbID;
			if (tVShowNametvdb != "") _TVShowNametvdb = tVShowNametvdb;
			if (rageTVID != -1) _rageTVID = rageTVID;
			if (tVShowNamerage != "") _TVShowNamerage = tVShowNamerage;
			if (epguidesID != "-1") _epguidesID = epguidesID;
			if (tVShowNameepg != "") _TVShowNameepg = tVShowNameepg;
		}
		string _TVShowName="";
		string _TVShowFolder = "";
		int _tvdbID = -1;
		string _TVShowNametvdb = "";
		int _rageTVID = -1;
		string _TVShowNamerage = "";
		string _epguidesID = "-1";
		string _TVShowNameepg = "";

		public string TVShowName
		{
			get { return _TVShowName; }
			set { _TVShowName = value; }
		}
		public string TVShowFolder
		{
			get { return _TVShowFolder; }
			set { _TVShowFolder = value; }
		}
		public int TVDBID
		{
			get { return _tvdbID; }
			set { _tvdbID = value; }
		}
		public string TVShowNameTVDB
		{
			get { return _TVShowNametvdb; }
			set { _TVShowNametvdb = value; }
		}
		public int RageTVID
		{
			get { return _rageTVID; }
			set { _rageTVID = value; }
		}
		public string TVShowNameRage
		{
			get { return _TVShowNamerage; }
			set { _TVShowNamerage = value; }
		}
		public string EpguidesID
		{
			get { return _epguidesID; }
			set { _epguidesID = value; }
		}
		public string TVShowNameEPG
		{
			get { return _TVShowNameepg; }
			set { _TVShowNameepg = value; }
		}
	};
	public class TVClass
	{
		string _fileFolder;//original folder
		string _fileName;//original file name
		string _fileExtention;//original file Extension
		string _fileTitle="";//Title files		
		string _newFileName;//new file name
		bool _auto = true;//auto-convert
		int _tvShowID = -1;//tv show Index
		int _seasonNum = -1;//Season Number 
		int _episodeNum = -1;//Episode Number
        int _episodeNum2 = -1;//Episode Number 2
        string _quality = "";
		string _TVShowName = ""; //Show Name
		bool _getTitle = true;//autoGetTitle

		public TVClass(string fileFolder, string fileName, string fileExtention)
		{
			if (fileFolder.EndsWith(Path.DirectorySeparatorChar.ToString())) fileFolder = fileFolder.TrimEnd(Path.DirectorySeparatorChar);
			_fileFolder = fileFolder;
			_fileName = _newFileName = fileName;
			_fileExtention = fileExtention;
			_auto = true;
		}

		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}

		public string NewFileName
		{
			get { return _newFileName; }
			set { _newFileName = value; }
		}

		public string FileFolder
		{
			get { return _fileFolder; }
			set 
			{
				if (value.EndsWith(Path.DirectorySeparatorChar.ToString())) value = value.TrimEnd(Path.DirectorySeparatorChar);
				_fileFolder = value;
			}
		}

		public string FileExtention
		{
			get { return _fileExtention; }
			set { _fileExtention = value; }
		}

		public string FileTitle
		{
			get { return _fileTitle; }
			set { _fileTitle = value; }
		}

		public string FullFileName
		{
			get { return _fileFolder + Path.DirectorySeparatorChar + _fileName; }
		}

		public string NewFullFileName
		{
			get { return _fileFolder + Path.DirectorySeparatorChar  + _newFileName; }
		}

		public bool AutoEdit 
		{
			get { return _auto; }
			set { _auto = value; }
		}

		public int TVShowID
		{
			get { return _tvShowID; }
			set { _tvShowID = value; }
		}

		public string TVShowName
		{
			get { return _TVShowName; }
			set { _TVShowName = value; }
		}

        public string Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }

		public int SeasonNum
		{
			get { return _seasonNum; }
			set { _seasonNum = value; }
		}

		public int EpisodeNum
		{
			get { return _episodeNum; }
			set { _episodeNum = value; }
		}

        public int EpisodeNum2
        {
            get { return _episodeNum2; }
            set { _episodeNum2 = value; }
        }

		public bool GetTitle
		{
			get { return _getTitle; }
			set { _getTitle = value; }
		}

		public void Reset()
		{
			_fileTitle = "";
			_newFileName = "";
			_auto = true;
			_tvShowID = -1;
			_seasonNum = -1;
			_episodeNum = -1;
            _episodeNum2 = -1;
			_TVShowName = "";
            _quality = "";
			_getTitle = true;
		}
	}//end of class
	public class EPGuigeReturnObject
	{
		string _tvshowName;
		string _episodeNumber;
		string _episodeNumber2;
		string _episodeDate;
		string _episodeTitle;

		public EPGuigeReturnObject(string tvshowName,string episodeNumber, string episodeNumber2, string episodeDate, string episodeTitle )
		{
			_tvshowName = tvshowName;
			_episodeNumber = episodeNumber;
			_episodeNumber2 = episodeNumber2;
			_episodeDate = episodeDate;
			_episodeTitle = episodeTitle;  
		   
		}
		public string TVShowName
		{
			get { return _tvshowName; }
			set { _tvshowName = value; }
		}

		public string EpisodeNumber
		{
			get { return _episodeNumber; }
			set { _episodeNumber = value; }
		}

		public string EpisodeNumber2
		{
			get { return _episodeNumber2; }
			set { _episodeNumber2 = value; }
		}

		public string EpisodeDate
		{
			get { return _episodeDate; }
			set { _episodeDate = value; }
		}

		public string EpisodeTitle
		{
			get { return _episodeTitle; }
			set { _episodeTitle = value; }
		}		
	}//end of class

	public class FileCopyData
	{
		//public FileCopyData() { }

		public FileCopyData(string sourceFullFilePath, string destinationFileName, string destinationFolder, int dataGridIndex, bool movedNotCopied)
		{
			_SourceFullFilePath  = sourceFullFilePath;
			_DestinationFileName = destinationFileName;
			_DestinationFolder   = destinationFolder;
			_index               = dataGridIndex;
			_MovedNotCopied      = movedNotCopied;
		}
		string _SourceFullFilePath = "";
		//string _SourceFolder = "";
		string _DestinationFileName = "";
		string _DestinationFolder = "";
		int _index = -1;
		bool _Successful = true;
		bool _MovedNotCopied = false;

		public string SourceFullFilePath
		{
			get { return _SourceFullFilePath; }
			set { _SourceFullFilePath = value; }
		}
		//public string SourceFolder
		//{
		//    get { return _SourceFolder; }
		//    set { _SourceFolder = value; }
		//}
		public string DestinationFileName
		{
			get { return _DestinationFileName; }
			set { _DestinationFileName = value; }
		}

		public string DestinationFolder
		{
			get { return _DestinationFolder; }
			set { _DestinationFolder = value; }
		}

		public int DataGridIndex
		{
			get { return _index; }
			set { _index = value; }
		}

		public bool CopySuccessful
		{
			get { return _Successful; }
			set { _Successful = value; }
		}
		public bool MovedNotCopied
		{
			get { return _MovedNotCopied; }
			set { _MovedNotCopied = value; }
		}
	};
	public static class ScottsFileSystem
	{
		public static bool MoveFiles(List<FileCopyData> Files, bool copy, Form1 mainWindow)
		{
			// move some files
			ShellFileOperation fo = new ShellFileOperation();

			List<string> source = new List<string>();
			List<string> dest = new List<string>();

			foreach (FileCopyData fileData in Files)
			{
				source.Add(fileData.SourceFullFilePath);
				// _fileFolder + Path.DirectorySeparatorChar  + _newFileName;
				dest.Add(fileData.DestinationFolder + Path.DirectorySeparatorChar + fileData.DestinationFileName);
			}

			if (copy)
				fo.Operation = ShellFileOperation.FileOperations.FO_COPY;
			else
				fo.Operation = ShellFileOperation.FileOperations.FO_MOVE;
			fo.OwnerWindow = mainWindow.Handle;
			fo.SourceFiles = source;
			fo.DestFiles = dest;

			bool RetVal = fo.DoOperation();
			if (RetVal)
			{
				foreach (ShellNameMapping newName in fo.NameMappings)
				{
					for (int i = 0; i < Files.Count; i++)
					{
						if (newName.DestinationPath == (Files[i].DestinationFolder + Files[i].DestinationFileName))
						{
							FileInfo newFile = new FileInfo(newName.RenamedDestinationPath);
							Files[i].DestinationFileName = newFile.Name;
						}
					}
				}
				//MessageBox.Show("Copy Complete without errors!");
				return true;
			}
			else
			{
				MessageBox.Show("Copy Complete with errors!");
			}

			//fo.NameMappings.Length;
			
			//CopyFilesDialog copyfiles = new CopyFilesDialog(Files, copy);
			//copyfiles.Show();

			return false;
		}
	}
}//end of namespace
