using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MsdnMag;
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
        public TVShowInfo(string tVShowName, string realTVShowName,string tVShowFolder, int tvdbID, int rageTVID, string epguidesID)
        {
            _TVShowName = tVShowName;
            if(realTVShowName!="")_realTVName = realTVShowName;
            if (tVShowFolder != "") _TVShowFolder = tVShowFolder;
            if (tvdbID != -1) _tvdbID = tvdbID;
            if (rageTVID != -1) _rageTVID = rageTVID;
            if (epguidesID != "-1") _epguidesID = epguidesID;
        }
        string _TVShowName="";
        string _realTVName="";
        string _TVShowFolder = "";
        int _tvdbID=-1;
        int _rageTVID=-1;
        string _epguidesID = "-1";

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
        public string EpguidesID
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

    public class MyProgressSink : FileOperationProgressSink
    {
        List<int> _selected = new List<int>();
        BindingList<TVClass> _fileList = new BindingList<TVClass>();
        //List<string> _newNames = new List<string>();
        //List<string> _newFolder = new List<string>();
        string preFileName = "";
        string preFolderName = "";

        string temp;

        int progress = 0;
        public MyProgressSink(List<int> selected,List<string> newFolder, BindingList<TVClass> fileList)
        {
            _selected = selected;
            _fileList = fileList;
            //_newFolder = newFolder;
        }
        public MyProgressSink()
        {            
        } 
        public override void PreRenameItem(uint dwFlags, IShellItem psiItem, string pszNewName)
        {
            //MessageBox.Show("PreRenameItem " + pszNewName);
        }
        public override void PostRenameItem(uint dwFlags, IShellItem psiItem, string pszNewName, uint hrRename, IShellItem psiNewlyCreated) 
        {
            //MessageBox.Show("PostRenameItem " + pszNewName);
            //_fileList[_selected[progress]].NewFileName = pszNewName;
        }
        public override void PreMoveItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, string pszNewName)
        {
            //MessageBox.Show("PreMoveItem " + pszNewName);
            preFileName = pszNewName;
            preFolderName = psiDestinationFolder.ToString();
        }        
        public override void PostMoveItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, string pszNewName, uint hrMove, IShellItem psiNewlyCreated)
        {
            //_fileList[_selected[progress]].FileFolder = psiDestinationFolder.ToString();
            //_fileList[_selected[progress]].NewFileName = pszNewName;
            //_fileList[_selected[progress]].AutoEdit = false;  
            //_newFolder.Add(psiDestinationFolder.ToString());
            //_newNames.Add(pszNewName);
            //if (premoveName == pszNewName) 
            //{
            //    _newNames.Add(pszNewName);  
            //}
            //premoveName = "";
            //MessageBox.Show("PostMoveItem " + pszNewName);

            temp = psiDestinationFolder.GetParent().ToString();
            if (!preFileName.ToUpper().Contains(pszNewName.ToUpper()) || !preFolderName.ToUpper().Contains(psiDestinationFolder.ToString().ToUpper()))
            {
                _fileList[_selected[progress]].FileFolder = psiDestinationFolder.ToString();
                _fileList[_selected[progress]].NewFileName = pszNewName;
                _fileList[_selected[progress]].AutoEdit = false; 
                progress++;
            }
        }

        public override void PreCopyItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, string pszNewName)
        {
            //preFileName = pszNewName;
            //preFolderName = psiDestinationFolder.ToString();

            //MessageBox.Show("PreCopyItem " + pszNewName);
        }

        public override void PostCopyItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, string pszNewName, uint hrCopy, IShellItem psiNewlyCreated)
        {
            //MessageBox.Show("PostCopyItem " + pszNewName);
            //progress++;
        }

        public override void FinishOperations(uint hrResult)
        {
            //if (_selected.Count == 0) return;
            //for (int i = 1; i < _newNames.Count;i=i+2 ) 
            //{
            //    _fileList[_selected[i/2]].NewFileName = _newNames[_selected[(i/2)+1]];
            //    _fileList[_selected[i/2]].FileFolder = _newFolder[_selected[i/2]];
            //    _fileList[_selected[i/2]].AutoEdit = false;            
            //}
            //MessageBox.Show(hrResult.ToString());
        }
        //public override void StartOperations()
        //{
        //    MessageBox.Show("Start Copy");
        //}
    }
    
}//end of namespace
