using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV_show_Renamer
{
    public class TVShowID
    {
        string _TVShowName;
        int _TVID;
        public TVShowID(string tVShowName, int tVID)
        {
            _TVShowName = tVShowName;
            _TVID = tVID;
        }
        public string TVShowName
        {
            get { return _TVShowName; }
            set { _TVShowName = value; }
        }
        public int TVID
        {
            get { return _TVID; }
            set { _TVID = value; }
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
    }//end of class
}//end of namespace
