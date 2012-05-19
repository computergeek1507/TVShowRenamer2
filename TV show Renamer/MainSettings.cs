using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace TV_Show_Renamer
{
    public class MainSettings
    {
        #region init
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
        bool _getTVShowName = false;

        int _seasonOffset = 0;
        int _episodeOffset = 0;
        int _programFormat = 0;
        int _seasonFormat = 0;
        int _titleFormat = 0;
        int _junkFormat = 0;
        int _extFormat = 0;
        int _tvDataBase = 0;
        int _titleSelection = 0;

        int[] _backgroundColor = { 255, 153, 180, 209 };
        int[] _foregroundColor = { 255, 0, 0, 0 };
        int[] _buttonColor = { 255, 240, 240, 240 };

        string _firstWord = "";
        string _dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\TV Show Renamer";

        Form1 _main;

        List<string> _moveFolder = new List<string>();//TV Show folders
        #endregion     
        
        //get Main Form object
        public void Start(Form1 tempMain) 
        {
            _main = tempMain;
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
            _getTVShowName = false;

            _seasonOffset = 0;
            _episodeOffset = 0;
            _programFormat = 0;
            _seasonFormat = 0;
            _titleFormat = 0;
            _junkFormat = 0;
            _extFormat = 0;
            _tvDataBase = 0;
            _titleSelection = 0;

            int[] temp1 = { 255, 153, 180, 209 };
            int[] temp2 = { 255, 0, 0, 0 };
            int[] temp3 = { 255, 240, 240, 240 };
            _backgroundColor = temp1;
            _foregroundColor = temp2;
            _buttonColor = temp3;
            _tvDataBase = 0;

            _firstWord = "";

            _main.dataGridView1.Columns["oldName"].Visible=true;
            _main.dataGridView1.Columns["newname"].Visible = true;
            _main.dataGridView1.Columns["filefolder"].Visible = false;
            _main.dataGridView1.Columns["fileextention"].Visible = false;
            _main.dataGridView1.Columns["TVShowID"].Visible = false;
            _main.dataGridView1.Columns["TVShowName"].Visible = false;
            _main.dataGridView1.Columns["titles"].Visible = false;
            _main.dataGridView1.Columns["SeasonNum"].Visible = false;
            _main.dataGridView1.Columns["EpisodeNum"].Visible = false;

            //_moveFolder.Clear();
        }

        //save settings
        public bool saveStettings()
        {
            bool returnValue = true;
            StreamWriter pw = null;
            try
            {//write newpreferences file
                pw = new StreamWriter(_dataFolder + "//newpreferences.seh");
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
                pw.WriteLine(_tvDataBase);
                pw.WriteLine(_titleSelection);
                pw.WriteLine(_getTVShowName);

                pw.WriteLine(_main.dataGridView1.Columns["oldName"].Visible);
                pw.WriteLine(_main.dataGridView1.Columns["newname"].Visible);
                pw.WriteLine(_main.dataGridView1.Columns["filefolder"].Visible);
                pw.WriteLine(_main.dataGridView1.Columns["fileextention"].Visible);
                pw.WriteLine(_main.dataGridView1.Columns["TVShowID"].Visible);
                pw.WriteLine(_main.dataGridView1.Columns["TVShowName"].Visible);
                pw.WriteLine(_main.dataGridView1.Columns["titles"].Visible);
                pw.WriteLine(_main.dataGridView1.Columns["SeasonNum"].Visible);
                pw.WriteLine(_main.dataGridView1.Columns["EpisodeNum"].Visible);
                
                pw.Close();//close writer stream
            }
            catch (Exception e)
            {
                _main.Log.WriteLog("newpreferences.seh Write Falure \n" + e.ToString());
                returnValue = false;
            }
            finally
            {
                if (pw != null)
                    pw.Close();
            }
            try
            {//write all folder locations
                pw = new StreamWriter(_dataFolder + "//Folders.seh");
                pw.WriteLine(_moveFolder.Count());
                for (int i = 0; i < _moveFolder.Count(); i++)
                    pw.WriteLine(_moveFolder[i]);
                pw.Close();
            }
            catch (Exception e)
            {
                _main.Log.WriteLog("Folders.seh Falure \n" + e.ToString());
                returnValue = false;
            }
            finally
            {
                if (pw != null)
                    pw.Close();
            }
            try
            {//write TV SHOW Infos
                TVShowListSave(_dataFolder + "//TVShowInfo.xml", _main.TVShowInfoList);
            }
            catch (Exception e)
            {
                _main.Log.WriteLog("TVShowInfo.xml Write Falure \n" + e.ToString());
                returnValue = false;
            }
            finally
            {
                if (pw != null)
                    pw.Close();
            }
            return returnValue;
        }

        //load settings file
        public bool loadStettings()
        {
            bool returnValue = true;
            bool deleteTemp = false;

            StreamReader tr3 = null;
            try
            {
                if (File.Exists(_dataFolder + "//newpreferences.seh"))//see if file exists
                {
                    tr3 = new StreamReader(_dataFolder + "//newpreferences.seh");
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
                    if (DateTime.Today.Date.ToString() != lastUpdateTime)
                    {
                        deleteTemp = true;
                        if(_autoUpdates)
                            _checkForUpdates = true;
                    }
                    _autoGetTitle = bool.Parse(tr3.ReadLine());
                    _tvDataBase = int.Parse(tr3.ReadLine());
                    _titleSelection = int.Parse(tr3.ReadLine());
                    _getTVShowName = bool.Parse(tr3.ReadLine());

                    _main.dataGridView1.Columns["oldName"].Visible = bool.Parse(tr3.ReadLine());
                    _main.dataGridView1.Columns["newname"].Visible = bool.Parse(tr3.ReadLine());
                    _main.dataGridView1.Columns["filefolder"].Visible = bool.Parse(tr3.ReadLine());
                    _main.dataGridView1.Columns["fileextention"].Visible = bool.Parse(tr3.ReadLine());
                    _main.dataGridView1.Columns["TVShowID"].Visible = bool.Parse(tr3.ReadLine());
                    _main.dataGridView1.Columns["TVShowName"].Visible = bool.Parse(tr3.ReadLine());
                    _main.dataGridView1.Columns["titles"].Visible = bool.Parse(tr3.ReadLine());
                    _main.dataGridView1.Columns["SeasonNum"].Visible = bool.Parse(tr3.ReadLine());
                    _main.dataGridView1.Columns["EpisodeNum"].Visible = bool.Parse(tr3.ReadLine());
                    
                    tr3.Close();//close reader stream                                        
                }//end of if. 
            }
            catch (Exception e)
            {
                _main.Log.WriteLog("newpreferences.seh Read Error \n" + e.ToString());
                returnValue = false;
            }
            finally
            {
                if (tr3 != null)
                    tr3.Close();
            }
            try
            {//Read TV show folders
                if (File.Exists(_dataFolder + "//TVFolder.seh"))
                {
                    tr3 = new StreamReader(_dataFolder + "//TVFolder.seh");
                    int length = Int32.Parse(tr3.ReadLine());
                    for (int i = 0; i < length; i++)
                    {
                        if (length == 0)
                            break;
                        _moveFolder.Add("TV Folder "+(i+1).ToString());
                        _moveFolder.Add(tr3.ReadLine());                        
                        _moveFolder.Add("3");                        
                    }//end of for loop  
                    tr3.Close();
                    File.Delete(_dataFolder + "//TVFolder.seh");
                }//end of if
            }
            catch (Exception e)
            {
                _main.Log.WriteLog("TVFolder.seh Read Error \n" + e.ToString());
                returnValue = false;
            }
            finally
            {
                if(tr3!=null)
                    tr3.Close();
            }

            try
            {//Read Other folders 
                if (File.Exists(_dataFolder + "//OtherFolders.seh"))//see if file exists
                {
                    tr3 = new StreamReader(_dataFolder + "//OtherFolders.seh");
                    int length = Int32.Parse(tr3.ReadLine());
                    for (int i = 0; i < length; i++)
                    {
                        if (length == 0)
                            break;
                        _moveFolder.Add(tr3.ReadLine());
                        if (i % 2 == 1)
                        {
                            _moveFolder.Add("1");                            
                        }
                    }//end of for loop  
                    tr3.Close();
                    File.Delete(_dataFolder + "//OtherFolders.seh");
                }
            }
            catch (Exception e)
            {
                _main.Log.WriteLog("OtherFolders.seh Read Error \n" + e.ToString());
                returnValue = false;
            }
            finally
            {
                if(tr3!=null)
                    tr3.Close();
            }
            try
            {//Read all folders
                if (File.Exists(_dataFolder + "//Folders.seh"))
                {
                    tr3 = new StreamReader(_dataFolder + "//Folders.seh");
                    int length = Int32.Parse(tr3.ReadLine());
                    for (int i = 0; i < length; i++)
                    {
                        if (length == 0)
                            break;
                        _moveFolder.Add(tr3.ReadLine());
                        }//end of for loop  
                    tr3.Close();
                }//end of if
            }
            catch (Exception e)
            {
                _main.Log.WriteLog("Folders.seh Read Error \n" + e.ToString());
                returnValue = false;
            }
            finally
            {
                if(tr3!=null)
                    tr3.Close();
            }
            try
            {//Read ID List folders
                if (File.Exists(_dataFolder + "//TVShowID.seh"))
                {
                    tr3 = new StreamReader(_dataFolder + "//TVShowID.seh");
                    int length = Int32.Parse(tr3.ReadLine());
                    if ((length % 2 == 0) && length != 0)
                    {
                        for (int i = 0; i < length; i = i + 2)
                        {
                            _main.TVShowInfoList.Add(new TVShowInfo(tr3.ReadLine(), "", "", int.Parse(tr3.ReadLine()), -1,"-1" ));
                        }//end of for loop  
                        tr3.Close();
                        File.Delete(_dataFolder + "//TVShowID.seh");
                    }//end of if
                }
            }
            catch (Exception e)
            {
                _main.Log.WriteLog("TVShowID.seh Read Error \n" + e.ToString());
                returnValue = false;
            }
            finally
            {
                if (tr3 != null)
                    tr3.Close();
            }

            try
            {
                if (File.Exists(_dataFolder + "//TVShowInfo.xml"))//see if file exists
                {
                    TVShowListLoad(_dataFolder + "//TVShowInfo.xml", _main.TVShowInfoList);
                }//end of if. 
            }
            catch (Exception e)
            {
                _main.Log.WriteLog("TVShowInfo.xml Read Error \n" + e.ToString());
                returnValue = false;
            }
            //delete temp folder daily
            try
            {
                if ((Directory.Exists(_dataFolder + "\\Temp")))
                {
                    if (deleteTemp)
                    {
                        Directory.Delete(_dataFolder + "\\Temp",true);
                        Directory.CreateDirectory(_dataFolder + "\\Temp");
                    }
                }else
                    Directory.CreateDirectory(_dataFolder + "\\Temp");

            }
            catch (Exception e)
            {
                _main.Log.WriteLog("Can't Delete Temp Folder\n" + e.ToString());
                returnValue = false;
            }          

            return returnValue;
        }//end of loadsettings methods

        //Write XML file with TVShowListLoad Names and such
        private void TVShowListSave(string saveLocation, List<TVShowInfo> saveItem)
        {
            if (saveItem.Count == 0) return;
            XDocument infoDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement MainrootElem = new XElement("TVShows");
            for (int i = 0; i < saveItem.Count(); i++)
            {
                XElement rootElem = new XElement("TVShow");
                rootElem.Add(new XElement("TVShowName", saveItem[i].TVShowName));
                rootElem.Add(new XElement("RealTVShowName", saveItem[i].RealTVShowName));
                rootElem.Add(new XElement("TVShowFolder", saveItem[i].TVShowFolder));
                rootElem.Add(new XElement("TVDBID", saveItem[i].TVDBID));
                rootElem.Add(new XElement("RageTVID", saveItem[i].RageTVID));
                rootElem.Add(new XElement("EpguidesID", saveItem[i].EpguidesID));

                MainrootElem.Add(rootElem);
            }
            infoDoc.Add(MainrootElem);
            infoDoc.Save(saveLocation);
        }

        //Read XML file with TVShowListLoad Names and such
        private void TVShowListLoad(string FileLocation, List<TVShowInfo> loadedItem)
        {
            XDocument TVShowListXML = XDocument.Load(FileLocation);

            var TVShowLists = from TVShowList in TVShowListXML.Descendants("TVShow")
                            select new
                            {
                                TVShowName = TVShowList.Element("TVShowName").Value,
                                RealTVShowName = TVShowList.Element("RealTVShowName").Value,
                                TVShowFolder = TVShowList.Element("TVShowFolder").Value,
                                TVDBID = TVShowList.Element("TVDBID").Value,
                                RageTVID = TVShowList.Element("RageTVID").Value,
                                EpguidesID = TVShowList.Element("EpguidesID").Value
                            };

            foreach (var wd in TVShowLists)            
                loadedItem.Add(new TVShowInfo(wd.TVShowName, wd.RealTVShowName, wd.TVShowFolder, Int32.Parse(wd.TVDBID),Int32.Parse(wd.RageTVID),wd.EpguidesID));            
        }

        #region declartions
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
        public bool GetTVShowName
        {
            get { return _getTVShowName; }
            set { _getTVShowName = value; }
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
        public int TitleSelection
        {
            get { return _titleSelection; }
            set { _titleSelection = value; }
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
  
        #endregion      
    }//end of class
}//end of namespace