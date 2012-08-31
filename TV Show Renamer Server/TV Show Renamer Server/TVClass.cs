using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV_Show_Renamer_Server
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
    public class LoadFileInfo
    {
        string _fileName;
        string _fileFolder;
        string _fileExt;
        public LoadFileInfo(string filename, string filefolder,string fileext)
        {
            _fileName = filename;
            _fileFolder = filefolder;
            _fileExt = fileext;
        }
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
        public string FileFolder
        {
            get { return _fileFolder; }
            set { _fileFolder = value; }
        }
        public string FileExtension
        {
            get { return _fileExt; }
            set { _fileExt = value; }
        }
        public string FullFileDirectly 
        {
            get { return _fileFolder + "//" + _fileName; }
        }

    };

    public class CategoryInfo 
    {
        string _categoryTitle;
        string _commandWords;
        string _searchFolder;
        int _folderOptions;
        //string _folderOptions;
        public CategoryInfo(string categoryTitle, string commandWords, string searchFolder, int folderOptions)
        {
            _categoryTitle = categoryTitle;
            _commandWords = commandWords;
            _folderOptions = folderOptions;
            _searchFolder = searchFolder;
        }
        public string CategoryTitle
        {
            get { return _categoryTitle; }
            set { _categoryTitle = value; }
        }
        public string CommandWords
        {
            get { return _commandWords; }
            set { _commandWords = value; }
        }
        public string SearchFolder
        {
            get { return _searchFolder; }
            set { _searchFolder = value; }
        }
        public int FolderOptions
        {
            get { return _folderOptions; }
            set { _folderOptions = value; }
        }
    };

    public class TVClass
    {
        string _fileFolder;//origonal folder
        string _fileName;//origonal file name
        string _fileExtention;//origonal file Extention
        string _fileTitle="";//Title files        
        string _newFileName;//new file name
        string _ShowName = null;
        bool _auto = true;//autoconvert
        int _tvShowID = -1;//TVDB id number
        int _seasonNum = -1;//Season Number 
        int _episodeNum = -1;//Episode Number

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

        public string ShowName
        {
            get { return _ShowName; }
            set { _ShowName = value; }
        }

        public int TVShowID
        {
            get { return _tvShowID; }
            set { _tvShowID = value; }
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
    }//end of class

    public class TVShowSettings
    {
        string _ShowFolder;
        string _RealShowName;
        string _SearchName;

        int _TVDBSeriesID = -1;//TVDB id number

        public TVShowSettings(string showFolder, string realShowName, string searchName, int tVDBSeriesID)
        {
            _ShowFolder = showFolder;
            _RealShowName = realShowName;
            _SearchName = searchName;
            _TVDBSeriesID = tVDBSeriesID;
        }

        public string ShowFolder
        {
            get { return _ShowFolder; }
            set { _ShowFolder = value; }
        }

        public string RealShowName
        {
            get { return _RealShowName; }
            set { _RealShowName = value; }
        }

        public string SearchName
        {
            get { return _SearchName; }
            set { _SearchName = value; }
        }
        public int TVDBSeriesID
        {
            get { return _TVDBSeriesID; }
            set { _TVDBSeriesID = value; }
        }
    }//end of class

    public class SearchInfo
    {
        string _title;
        string _newTitle = "";
        int _selected;

        public SearchInfo(string title, int selected)
        {
            _title = title;
            _selected = selected;
        }

        public SearchInfo(string title, string newTitle, int selected)
        {
            _title = title;
            _selected = selected;
            _newTitle = newTitle;
        }

        public SearchInfo()
        {
            _title = "";
            _selected = -1;
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string NewTitle
        {
            get { return _newTitle; }
            set { _newTitle = value; }
        }

        public int SelectedValue
        {
            get { return _selected; }
            set { _selected = value; }
        }
    }//end of class//end of class
}//end of namespace
