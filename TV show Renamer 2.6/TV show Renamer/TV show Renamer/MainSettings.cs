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

        int _seasonOffset = 0;
        int _episodeOffset = 0;
        int _programFormat = 0;
        int _seasonFormat = 0;
        int _titleFormat = 0;
        int _junkFormat = 0;
        int _extFormat = 0;

        int[] _backgroundColor = { 255, 153, 180, 209 };
        int[] _foregroundColor = { 255, 0, 0, 0 };
        int[] _buttonColor = { 255, 153, 180, 209 };

        string _dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\TV Show Renamer";

        LogWrite _main;

        List<string> _moveFolder = new List<string>();//TV Show folders
        List<string> _otherFolders = new List<string>();//Other folders

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

            _seasonOffset = 0;
            _episodeOffset = 0;
            _programFormat = 0;
            _seasonFormat = 0;
            _titleFormat = 0;
            _junkFormat = 0;
            _extFormat = 0;
            int[] temp1 = { 255, 153, 180, 209 };
            int[] temp2 = { 255, 0, 0, 0 };
            int[] temp3 = { 255, 153, 180, 209 };
            _backgroundColor = temp1;
            _foregroundColor = temp2;
            _buttonColor = temp3;

            _moveFolder.Clear();
            _otherFolders.Clear();
        }

        //save settings
        public bool saveStettings() 
        {
            try
            {
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

                pw.Close();//close writer stream

                //write tv folder locations
                pw = new StreamWriter(_dataFolder + "//TVFolder.seh");
                pw.WriteLine(_moveFolder.Count());
                for (int i = 0; i < _moveFolder.Count(); i++)                
                    pw.WriteLine(_moveFolder[i]);                
                pw.Close();

                //write other folder locations
                pw = new StreamWriter(_dataFolder + "//OtherFolders.seh");
                pw.WriteLine(_otherFolders.Count());
                for (int i = 0; i < _otherFolders.Count(); i++)                
                    pw.WriteLine(_otherFolders[i]);                
                pw.Close();

            }
            catch (Exception)
            {
                _main.WriteLog("Preference Write Falure");
                return false;
            }
            return true;        
        }

        //load settings file
        public bool loadStettings()
        {
            try
            {
                if (File.Exists(_dataFolder + "//newpreferences.seh"))//see if file exists
                {
                    StreamReader tr3 = new StreamReader(_dataFolder + "//newpreferences.seh");
                    _removePeriod = bool.Parse(tr3.ReadLine());
                    _removeUnderscore = bool.Parse(tr3.ReadLine());
                    _removeDash= bool.Parse(tr3.ReadLine());
                    _removeBracket= bool.Parse(tr3.ReadLine());
                    _removeCrap= bool.Parse(tr3.ReadLine());

                    _dashSeason = bool.Parse(tr3.ReadLine());
                    _dashTitle= bool.Parse(tr3.ReadLine());
                    _removeYear= bool.Parse(tr3.ReadLine());
                    _openZIPs= bool.Parse(tr3.ReadLine());
                    _autoTitle= bool.Parse(tr3.ReadLine());

                    _seasonOffset= int.Parse(tr3.ReadLine());
                    _episodeOffset = int.Parse(tr3.ReadLine());
                    _programFormat= int.Parse(tr3.ReadLine());
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
                    if (DateTime.Today.Date.ToString() != lastUpdateTime)
                        _checkForUpdates = true;
                    
                    tr3.Close();//close reader stream                                        
                }//end of if. 

                //Read TV show folders
                if (File.Exists(_dataFolder + "//TVFolder.seh"))
                {
                    StreamReader tv2 = new StreamReader(_dataFolder + "//TVFolder.seh");
                    int length = Int32.Parse(tv2.ReadLine());
                    for (int i = 0; i < length; i++)
                    {
                        if (length == 0)                        
                            break;                        
                       _moveFolder.Add(tv2.ReadLine());                                                
                    }//end of for loop  
                    tv2.Close();
                }//end of if

                //Read Other folders 
                if (File.Exists(_dataFolder + "//OtherFolders.seh"))//see if file exists
                {
                    StreamReader tv3 = new StreamReader(_dataFolder + "//OtherFolders.seh");
                    int length = Int32.Parse(tv3.ReadLine());
                    for (int i = 0; i < length; i++)
                    {
                        if (length == 0)                        
                            break;                        
                        _otherFolders.Add(tv3.ReadLine());                 
                    }//end of for loop  
                    tv3.Close();                
                }
            }
            catch (Exception e)
            {
                _main.WriteLog("Reading Preference Error \n" + e.ToString());
                return false;
            }
            return true;
        }

        //Add folder to TV folder list 
        public void moveFolderAdd(string folder) 
        {
            _moveFolder.Add(folder);
        }
       
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
            set { _checkForUpdates = value; }
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
        public List<string> MoveFolder 
        {
            get { return _moveFolder; }
            set { _moveFolder = value; }
        }
        public List<string> OtherFolders
        {
            get { return _otherFolders; }
            set { _otherFolders = value; }
        }
    }//end of class
}//end of namespace
