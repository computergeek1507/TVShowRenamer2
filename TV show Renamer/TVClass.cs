using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;

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
        string _fileFolder;//origonal folder
        string _fileName;//origonal file name
        string _fileExtention;//origonal file Extention
        string _fileTitle="";//Title files        
        string _newFileName;//new file name
        bool _auto = true;//autoconvert
        int _tvShowID = -1;//tv show Index
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
            get { return _fileFolder +Path.DirectorySeparatorChar + _fileName; }            
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

}//end of namespace
