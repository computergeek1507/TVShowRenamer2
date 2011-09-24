using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TV_show_Renamer
{
    public class MainSettings
    {
        bool _removePeriod = true;
        bool _removeUnderscore = true;
        bool _removeDash = true;
        bool _removeBracket = true;
        bool _removeCrap = true;
        bool _dashSeason = true;
        bool _dashTitle = false;
        bool _removeYear = true;
        bool _openZIPs = false;
        bool _autoTitle = true;
        bool _addFiles = false;
        bool _shownb4 = false;
        bool _formClosed = false;
        bool _closedForUpdates = false;
        bool _checkForUpdates = false;
        bool _autoUpdates = true;
        bool _autoGetTitle = true;

        int _seasonOffset = 0;
        int _episodeOffset = 0;
        int _programFormat = 0;
        int _seasonFormat = 0;
        int _titleFormat = 0;
        int _junkFormat = 0;
        int _extFormat = 0;
        int _tvDataBase = 0;

        int[] _backgroundColor = { 255, 153, 180, 209 };
        int[] _foregroundColor = { 255, 0, 0, 0 };
        int[] _buttonColor = { 255, 240, 240, 240 };

        string _firstWord = "";
        string _dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\TV Show Renamer";

        LogWrite _main;

        List<string> _moveFolder = new List<string>();//TV Show folders
       
        List<TVShowID> _TVShowIDList = new List<TVShowID>();//TV Show ID       
        
        //get log object to write too
        public void Start(LogWrite main) 
        {
            _main = main;
        }

        //change to default settings
        public void defaultSettings() {
            _removePeriod = true;
            _removeUnderscore = true;
            _removeDash = true;
            _removeBracket = true;
            _removeCrap = true;

            _dashSeason = true;
            _dashTitle = false;
            _removeYear = true;
            _openZIPs = false;
            _autoTitle = true;
            _addFiles = false;
            _shownb4 = false;
            _formClosed = false;
            _closedForUpdates = false;
            _checkForUpdates = false;
            _autoUpdates = true;
            _autoGetTitle = true;

            _seasonOffset = 0;
            _episodeOffset = 0;
            _programFormat = 0;
            _seasonFormat = 0;
            _titleFormat = 0;
            _junkFormat = 0;
            _extFormat = 0;
            int[] temp1 = { 255, 153, 180, 209 };
            int[] temp2 = { 255, 0, 0, 0 };
            int[] temp3 = { 255, 240, 240, 240 };
            _backgroundColor = temp1;
            _foregroundColor = temp2;
            _buttonColor = temp3;
            _tvDataBase = 0;

            _firstWord = "";
            _moveFolder.Clear();
        }

        //save settings
        public bool saveStettings()
        {
            bool returnValue = true;
            try
            {//write newpreferences file
                StreamWriter pw = new StreamWriter(_dataFolder + "//newpreferences.seh");
                pw.WriteLine(_removePeriod);
                pw.WriteLine(_removeUnderscore);
                pw.WriteLine(_removeDash);
                pw.WriteLine(_removeBracket);
                pw.WriteLine(_removeCrap);

                pw.WriteLine(_dashSeason);
                pw.WriteLine(_dashTitle);
                pw.WriteLine(_removeYear);
                pw.WriteLine(_openZIPs);
                pw.WriteLine(_autoTitle);

                pw.WriteLine(_seasonOffset);
                pw.WriteLine(_episodeOffset);
                pw.WriteLine(_programFormat);
                pw.WriteLine(_seasonFormat);
                pw.WriteLine(_titleFormat);
                pw.WriteLine(_junkFormat);
                pw.WriteLine(_extFormat);

                pw.WriteLine(_backgroundColor[0]);
                pw.WriteLine(_backgroundColor[1]);
                pw.WriteLine(_backgroundColor[2]);
                pw.WriteLine(_backgroundColor[3]);
                pw.WriteLine(_foregroundColor[0]);
                pw.WriteLine(_foregroundColor[1]);
                pw.WriteLine(_foregroundColor[2]);
                pw.WriteLine(_foregroundColor[3]);
                pw.WriteLine(_buttonColor[0]);
                pw.WriteLine(_buttonColor[1]);
                pw.WriteLine(_buttonColor[2]);
                pw.WriteLine(_buttonColor[3]);
                pw.WriteLine(DateTime.Today.Date.ToString());
                pw.WriteLine(_autoUpdates);
                pw.WriteLine(_autoGetTitle);

                pw.Close();//close writer stream
            }
            catch (Exception e)
            {
                _main.WriteLog("newpreferences.seh Write Falure \n" + e.ToString());
                returnValue = false;
            }
            try
            {//write all folder locations
                StreamWriter pw2 = new StreamWriter(_dataFolder + "//Folders.seh");
                pw2.WriteLine(_moveFolder.Count());
                for (int i = 0; i < _moveFolder.Count(); i++)
                    pw2.WriteLine(_moveFolder[i]);
                pw2.Close();
            }
            catch (Exception e)
            {
                _main.WriteLog("Folders.seh Falure \n" + e.ToString());
                returnValue = false;
            }
            try
            {//write TV SHOW IDs
                StreamWriter pw2 = new StreamWriter(_dataFolder + "//TVShowID.seh");
                pw2.WriteLine(_TVShowIDList.Count() * 2);
                for (int i = 0; i < _TVShowIDList.Count(); i++)
                {
                    pw2.WriteLine(_TVShowIDList[i].TVShowName);
                    pw2.WriteLine(_TVShowIDList[i].TVID);
                }                
                pw2.Close();
            }
            catch (Exception e)
            {
                _main.WriteLog("TVShowID.seh Falure \n" + e.ToString());
                returnValue = false;
            }
            return returnValue;
        }

        //load settings file
        public bool loadStettings()
        {
            bool returnValue = true;
            try
            {
                if (File.Exists(_dataFolder + "//newpreferences.seh"))//see if file exists
                {
                    StreamReader tr3 = new StreamReader(_dataFolder + "//newpreferences.seh");
                    _removePeriod = bool.Parse(tr3.ReadLine());
                    _removeUnderscore = bool.Parse(tr3.ReadLine());
                    _removeDash = bool.Parse(tr3.ReadLine());
                    _removeBracket = bool.Parse(tr3.ReadLine());
                    _removeCrap = bool.Parse(tr3.ReadLine());

                    _dashSeason = bool.Parse(tr3.ReadLine());
                    _dashTitle = bool.Parse(tr3.ReadLine());
                    _removeYear = bool.Parse(tr3.ReadLine());
                    _openZIPs = bool.Parse(tr3.ReadLine());
                    _autoTitle = bool.Parse(tr3.ReadLine());

                    _seasonOffset = int.Parse(tr3.ReadLine());
                    _episodeOffset = int.Parse(tr3.ReadLine());
                    _programFormat = int.Parse(tr3.ReadLine());
                    _seasonFormat = int.Parse(tr3.ReadLine());
                    _titleFormat = int.Parse(tr3.ReadLine());
                    _junkFormat = int.Parse(tr3.ReadLine());
                    _extFormat = int.Parse(tr3.ReadLine());

                    _backgroundColor[0] = int.Parse(tr3.ReadLine());
                    _backgroundColor[1] = int.Parse(tr3.ReadLine());
                    _backgroundColor[2] = int.Parse(tr3.ReadLine());
                    _backgroundColor[3] = int.Parse(tr3.ReadLine());
                    _foregroundColor[0] = int.Parse(tr3.ReadLine());
                    _foregroundColor[1] = int.Parse(tr3.ReadLine());
                    _foregroundColor[2] = int.Parse(tr3.ReadLine());
                    _foregroundColor[3] = int.Parse(tr3.ReadLine());
                    _buttonColor[0] = int.Parse(tr3.ReadLine());
                    _buttonColor[1] = int.Parse(tr3.ReadLine());
                    _buttonColor[2] = int.Parse(tr3.ReadLine());
                    _buttonColor[3] = int.Parse(tr3.ReadLine());
                    string lastUpdateTime = tr3.ReadLine();
                    _autoUpdates = bool.Parse(tr3.ReadLine());
                    if (DateTime.Today.Date.ToString() != lastUpdateTime && _autoUpdates)
                        _checkForUpdates = true;
                    var readtemp = tr3.ReadLine();
                    if (readtemp != null) _autoGetTitle = bool.Parse(readtemp);
                    tr3.Close();//close reader stream                                        
                }//end of if. 
            }
            catch (Exception e)
            {
                _main.WriteLog("newpreferences.seh Read Error \n" + e.ToString());
                returnValue = false;
            }


            try
            {//Read TV show folders
                if (File.Exists(_dataFolder + "//TVFolder.seh"))
                {
                    StreamReader tv2 = new StreamReader(_dataFolder + "//TVFolder.seh");
                    int length = Int32.Parse(tv2.ReadLine());
                    for (int i = 0; i < length; i++)
                    {
                        if (length == 0)
                            break;
                        _moveFolder.Add("TV Folder "+(i+1).ToString());
                        _moveFolder.Add(tv2.ReadLine());                        
                        _moveFolder.Add("3");                        
                    }//end of for loop  
                    tv2.Close();
                    File.Delete(_dataFolder + "//TVFolder.seh");
                }//end of if
            }
            catch (Exception e)
            {
                _main.WriteLog("TVFolder.seh Read Error \n" + e.ToString());
                returnValue = false;
            }
            try
            {//Read Other folders 
                if (File.Exists(_dataFolder + "//OtherFolders.seh"))//see if file exists
                {
                    StreamReader tv3 = new StreamReader(_dataFolder + "//OtherFolders.seh");
                    int length = Int32.Parse(tv3.ReadLine());
                    for (int i = 0; i < length; i++)
                    {
                        if (length == 0)
                            break;
                        _moveFolder.Add(tv3.ReadLine());
                        if (i % 2 == 1)
                        {
                            _moveFolder.Add("1");                            
                        }
                    }//end of for loop  
                    tv3.Close();
                    File.Delete(_dataFolder + "//OtherFolders.seh");
                }
            }
            catch (Exception e)
            {
                _main.WriteLog("OtherFolders.seh Read Error \n" + e.ToString());
                returnValue = false;
            }
            try
            {//Read all folders
                if (File.Exists(_dataFolder + "//Folders.seh"))
                {
                    StreamReader tv5 = new StreamReader(_dataFolder + "//Folders.seh");
                    int length = Int32.Parse(tv5.ReadLine());
                    for (int i = 0; i < length; i++)
                    {
                        if (length == 0)
                            break;
                        _moveFolder.Add(tv5.ReadLine());
                        }//end of for loop  
                    tv5.Close();
                }//end of if
            }
            catch (Exception e)
            {
                _main.WriteLog("Folders.seh Read Error \n" + e.ToString());
                returnValue = false;
            }
            try
            {//Read ID List folders
                if (File.Exists(_dataFolder + "//TVShowID.seh"))
                {
                    StreamReader tv4 = new StreamReader(_dataFolder + "//TVShowID.seh");
                    int length = Int32.Parse(tv4.ReadLine());
                    if ((length % 2 == 0) && length != 0)
                    {
                        for (int i = 0; i < length+1; i = i + 2)
                        {
                            _TVShowIDList.Add(new TVShowID(tv4.ReadLine(), int.Parse(tv4.ReadLine())));
                        }//end of for loop  
                        tv4.Close();
                    }//end of if
                }
            }
            catch (Exception e)
            {
                _main.WriteLog("TVShowID.seh Read Error \n" + e.ToString());
                returnValue = false;
            }

            return returnValue;
        }//end of loadsettings methods
                
        //public declartions
        public bool RemoveDash
        {
            get { return _removeDash; }
            set { _removeDash = value; }
        }
        public bool RemoveBracket
        {
            get { return _removeBracket; }
            set { _removeBracket = value; }
        }        
        public bool RemoveCrap
        {
            get { return _removeCrap; }
            set { _removeCrap = value; }
        }
        public bool OpenZIPs
        {
            get { return _openZIPs; }
            set { _openZIPs = value; }
        }
        public bool DashSeason
        {
            get { return _dashSeason; }
            set { _dashSeason = value; }
        }
        public bool DashTitle
        {
            get { return _dashTitle; }
            set { _dashTitle = value; }
        }
        public bool RemoveYear
        {
            get { return _removeYear; }
            set { _removeYear = value; }
        }
        public bool RemoveUnderscore
        {
            get { return _removeUnderscore; }
            set { _removeUnderscore = value; }
        }
        public bool RemovePeriod
        {
            get { return _removePeriod; }
            set { _removePeriod = value; }
        }
        public bool AutoTitle
        {
            get { return _autoTitle; }
            set { _autoTitle = value; }
        }
        public bool AddFiles
        {
            get { return _addFiles; }
            set { _addFiles = value; }
        }
        public bool Shownb4
        {
            get { return _shownb4; }
            set { _shownb4 = value; }
        }
        public bool FormClosed
        {
            get { return _formClosed; }
            set { _formClosed = value; }
        }
        public bool ClosedForUpdates
        {
            get { return _closedForUpdates; }
            set { _closedForUpdates = value; }
        }
        public bool CheckForUpdates
        {
            get { return _checkForUpdates; }
        }
        public bool AutoUpdates
        {
            get { return _autoUpdates; }
            set { _autoUpdates = value; }
        }
        public bool AutoGetTitle
        {
            get { return _autoGetTitle; }
            set { _autoGetTitle = value; }
        }
       
        public int SeasonOffset
        {
            get { return _seasonOffset; }
            set { _seasonOffset = value; }
        }
        public int EpisodeOffset
        {
            get { return _episodeOffset; }
            set { _episodeOffset = value; }
        }
        public int ProgramFormat
        {
            get { return _programFormat; }
            set { _programFormat = value; }
        }
        public int SeasonFormat
        {
            get { return _seasonFormat; }
            set { _seasonFormat = value; }
        }
        public int TitleFormat
        {
            get { return _titleFormat; }
            set { _titleFormat = value; }
        }
        public int JunkFormat
        {
            get { return _junkFormat; }
            set { _junkFormat = value; }
        }
        public int ExtFormat
        {
            get { return _extFormat; }
            set { _extFormat = value; }
        }
        public int TVDataBase
        {
            get { return _tvDataBase; }
            set { _tvDataBase = value; }
        }
        
        public int[] BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }
        public int[] ForegroundColor
        {
            get { return _foregroundColor; }
            set { _foregroundColor = value; }
        }
        public int[] ButtonColor
        {
            get { return _buttonColor; }
            set { _buttonColor = value; }
        }
       
        public string DataFolder
        {
            get { return _dataFolder; }
        }
        public string FirstWord
        {
            get { return _firstWord; }
            set { _firstWord = value; }
        }
        public List<string> MoveFolder
        {
            get { return _moveFolder; }
            set { _moveFolder = value; }
        }
        public List<TVShowID> TVShowIDList
        {
            get { return _TVShowIDList; }
            set { _TVShowIDList = value; }
        }        
    }//end of class
}//end of namespace