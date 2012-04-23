using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV_Show_Renamer
{    
    public class TVShowInfo
    {
        //public TVShowInfo() { }
        public TVShowInfo(string tVShowName)
        {
            _TVShowName = tVShowName;
        }
        public TVShowInfo(string tVShowName, string realTVShowName,string tVShowFolder, int tvdbID, int rageTVID, int epguidesID)
        {
            _TVShowName = tVShowName;
            if(realTVShowName!="")_realTVName = realTVShowName;
            if (tVShowFolder != "") _TVShowFolder = tVShowFolder;
            if (tvdbID != -1) _tvdbID = tvdbID;
            if (rageTVID != -1) _rageTVID = rageTVID;
            if (epguidesID != -1) _epguidesID = epguidesID;
        }
        string _TVShowName="";
        string _realTVName="";
        string _TVShowFolder = "";
        int _tvdbID=-1;
        int _rageTVID=-1;
        int _epguidesID=-1;

        public string TVShowName
        {
            get { return _TVShowName; }
            set { _TVShowName = value; }
        }
        public string RealTVShowName
        {
            get { return _realTVName; }
            set { _realTVName = value; }
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
        public int RageTVID
        {
            get { return _rageTVID; }
            set { _rageTVID = value; }
        }
        public int EpguidesID
        {
            get { return _epguidesID; }
            set { _epguidesID = value; }
        }
    };    
    public class TVClass
    {
        string _fileFolder;//origonal folder
        string _fileName;//origonal file name
        string _fileExtention;//origonal file Extention
        string _fileTitle="";//Title files        
        string _newFileName;//new file name
        bool _auto = true;//autoconvert
        int _tvShowID = -1;//TVDB id number
        int _seasonNum = -1;//Season Number 
        int _episodeNum = -1;//Episode Number
        string _TVShowName = ""; //Show Name
        bool _getTitle = true;//autoGetTitle

        public TVClass(string fileFolder, string fileName, string fileExtention)
        {
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
            set { _fileFolder = value; }
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
            get { return _fileFolder +"\\"+ _fileName; }            
        }

        public string NewFullFileName
        {
            get { return _fileFolder + "\\" + _newFileName; }
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
            _TVShowName = "";
            _getTitle = true;
        }
    }//end of class

    public static class ScottsFileSystem
    {
        public static void CopyFiles(List<string> sourceFiles, List<string> destFiles)
        {
            CopyFilesDialog copyfiles = new CopyFilesDialog(sourceFiles,  destFiles,false);
            copyfiles.Show();
        }

        public static void MoveFiles(List<string> sourceFiles, List<string> destFiles)
        {
            CopyFilesDialog copyfiles = new CopyFilesDialog(sourceFiles, destFiles, true);
            copyfiles.Show();
        }
    }
}//end of namespace
