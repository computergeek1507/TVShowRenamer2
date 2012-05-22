using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Net;
using System.Diagnostics;
//using Microsoft.VisualBasic.FileIO;
using System.Threading;
using SevenZip;
using System.Collections;
using MsdnMag;


namespace TV_Show_Renamer
{
    public partial class Form1 : Form
    {
        //Constructor with arguments
        public Form1(string[] args)
        {//check to see if program is already running
            bool isDupeFound = false;
            foreach (Process myProcess in Process.GetProcesses())
            {
                if (myProcess.ProcessName == Process.GetCurrentProcess().ProcessName)
                {
                    if (isDupeFound)
                        Process.GetCurrentProcess().Kill();
                    isDupeFound = true;
                }
            }
            InitializeComponent();
            dataGridView1.DataSource = fileList;
            this.loadEverything();
            if (args.GetLength(0) != 0)
            {
                ThreadAdd FilesToAdd = new ThreadAdd();
                FilesToAdd.AddType = "files";
                FilesToAdd.ObjectToAdd = args;
                addFilesToThread(FilesToAdd);
            }
        }

        //Constructor
        public Form1()
        {//check to see if program is already running
            bool isDupeFound = false;
            foreach (Process myProcess in Process.GetProcesses())
            {
                if (myProcess.ProcessName == Process.GetCurrentProcess().ProcessName)
                {
                    if (isDupeFound)
                        Process.GetCurrentProcess().Kill();
                    isDupeFound = true;
                }
            }
            InitializeComponent();
            dataGridView1.DataSource = fileList;
            this.loadEverything();
        }

        #region Initiate Stuff
        //initiate varibles  
        const int appVersion = 281;//2.8 Beta
        const int HowDeepToScan = 4;

        static public BindingList<TVClass> fileList = new BindingList<TVClass>();//TV Show list       
        static List<string> junklist = new List<string>();//junk word list
        static List<string> userjunklist = new List<string>();//user junk word list
        static List<string> textConverter = new List<string>();//textConverter word list
        public List<TVShowInfo> TVShowInfoList = new List<TVShowInfo>();//tv show info

        static Queue convertionQueue = new Queue();

        static Queue titleQueue = new Queue();

        //create other forms
        static junk_words userJunk = new junk_words();
        static Text_Converter textConvert = new Text_Converter();
        public LogWrite Log = new LogWrite();//log object
        public MainSettings newMainSettings = new MainSettings();//new settings object
        public class ThreadAdd { public string AddType; public object ObjectToAdd;};

        #endregion

        #region Menu Buttons

        //add files button
        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog2.Title = "Select Media files";
            openFileDialog2.Filter = "Video Files (*.avi;*.mkv;*.mp4;*.m4v;*.mpg;*.mov;*.mpeg;*.rm;*.rmvb;*.wmv;*.webm)|*.avi;*.mkv;*.mp4;*.m4v;*.mpg;*.mov;*.mpeg;*.rm;*.rmvb;*.wmv;*.webm|Archive Files (*.zip;*.rar;*.r01;*.7z;)|*.zip;*.rar;*.r01;*.7z;|All Files (*.*)|*.*";
            openFileDialog2.FileName = "";
            openFileDialog2.FilterIndex = 1;
            openFileDialog2.CheckFileExists = true;
            openFileDialog2.CheckPathExists = true;

            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ThreadAdd FilesToAdd = new ThreadAdd();
                FilesToAdd.AddType = "files";
                FilesToAdd.ObjectToAdd = openFileDialog2.FileNames;
                addFilesToThread(FilesToAdd);

                //AddFilesThread.RunWorkerAsync(FilesToAdd);
                //Thread h = new Thread(delegate() { getFiles(openFileDialog2.FileNames); });
                //h.Start();
            }//end of if
        }//end of file button

        //add folder button
        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                ThreadAdd FolderToAdd = new ThreadAdd();
                FolderToAdd.AddType = "folder";
                FolderToAdd.ObjectToAdd = folderBrowserDialog1.SelectedPath;
                addFilesToThread(FolderToAdd);
            }//end of if
        }//end of folder button

        //remove selected
        private void removeSelectedMenuItem_Click(object sender, EventArgs e)
        {
            AddFilesThread.CancelAsync();
            TitleThread.CancelAsync();
            deleteSelectedFiles();
        }

        //Clear items from display
        private void clearListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFilesThread.CancelAsync();
            TitleThread.CancelAsync();
            fileList.Clear();
            convertionQueue.Clear();
            dataGridView1.Refresh();
        }

        //Exit button 
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Convertion option menu 
        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            ConversionOptions MainSettings = new ConversionOptions(this, newMainSettings);
            MainSettings.Location = new Point(this.Location.X + ((this.Size.Width - MainSettings.Size.Width) / 2), this.Location.Y + ((this.Size.Height - MainSettings.Size.Height) / 2));
        }

        //User junk word menu
        private void addJunkWordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userJunk.Show();
            userJunk.Location = new Point(this.Location.X + ((this.Size.Width - userJunk.Size.Width) / 2), this.Location.Y + ((this.Size.Height - userJunk.Size.Height) / 2));
        }

        //text converter menu
        private void textConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textConvert.Show();
            textConvert.Location = new Point(this.Location.X + ((this.Size.Width - textConvert.Size.Width) / 2), this.Location.Y + ((this.Size.Height - textConvert.Size.Height) / 2));
        }

        //add title menu
        private void addTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Addtitle titles = new Addtitle(fileList, this);
            titles.Location = new Point(this.Location.X + ((this.Size.Width - titles.Size.Width) / 2), this.Location.Y + ((this.Size.Height - titles.Size.Height) / 2));
        }

        //add word to begining
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            string text = newMainSettings.FirstWord;
            if (InputBox.Show("Add Text To Begining", "Text:", ref text) == DialogResult.OK)
            {
                newMainSettings.FirstWord = text;
                autoConvert();
            }
            else
            {
                newMainSettings.FirstWord = "";
                autoConvert();
            }
            //if (text!= "")
            //{
            //    newMainSettings.FirstWord = text;
            //    autoConvert();
            //}
            //else
            //{
            //    newMainSettings.FirstWord = "";
            //    autoConvert();
            //}
        }//end of form closing

        //XBMC Tools
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (menu1.Count == 0) return;
            string[] folderSettings = menu1[0].Tag.ToString().Split('?');

            if (int.Parse(folderSettings[0]) > 1)
            {
                XBMC MainXBMC = new XBMC(fileList, newMainSettings.SeasonFormat, folderSettings[1]);
            }
        }

        //default setting method 
        private void defaultSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Would You Like to Restore Default Settings?", "Restore Default Settings", MessageBoxButtons.YesNo) == DialogResult.Yes)
                newMainSettings.defaultSettings();
        }//end of default setting method 

        //check for updates
        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {   //new thread for update
            Thread t = new Thread(new ThreadStart(checkForUpdate));
            t.Start();
        }

        //about display
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About info = new About(appVersion);
            info.Show();
            info.Location = new Point(this.Location.X + ((this.Size.Width - info.Size.Width) / 2), this.Location.Y + ((this.Size.Height - info.Size.Height) / 2));
        }

        //settings menu
        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            Settings MainSettings = new Settings(this, newMainSettings.OpenZIPs);
            MainSettings.Location = new Point(this.Location.X + ((this.Size.Width - MainSettings.Size.Width) / 2), this.Location.Y + ((this.Size.Height - MainSettings.Size.Height) / 2));
        }

        //hidden save
        private void secretSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileList.Count != 0) //if files are selected
            {
                for (int y = 0; y < fileList.Count(); y++)
                {
                    try
                    {
                        System.IO.File.Move((fileList[y].FullFileName), (fileList[y].NewFullFileName));
                        fileList[y].FileName = fileList[y].NewFileName;
                        fileList[y].FileTitle = "";
                        dataGridView1.Rows[y].Cells[0].Value = fileList[y].FileName;
                        Log.WriteLog(fileList[y].FileName, fileList[y].NewFileName);
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("File have been changed or moved \n" + (fileList[y].FullFileName) + "\n" + (fileList[y].NewFullFileName));
                        continue;
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("File already exists or is in use\n" + (fileList[y].FullFileName) + "\n" + (fileList[y].NewFullFileName));
                        continue;
                    }
                }//end of for loop
                autoConvert();
            }
        }

        //secret autoEdit reset
        private void secretResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                for (int u = 0; u < dataGridView1.Rows.Count; u++)
                {
                    //if (dataGridView1.Rows[u].Selected)
                    fileList[u].Reset();
                }
                autoConvert();
            }
        }

        //secret F1
        private void secretF1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newMainSettings.ProgramFormat = ++newMainSettings.ProgramFormat % 4;
            //MessageBox.Show(newMainSettings.ProgramFormat.ToString());
            autoConvert();
        }

        //secret F2
        private void secretF2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newMainSettings.DashSeason = !newMainSettings.DashSeason;
            //MessageBox.Show(newMainSettings.ProgramFormat.ToString());
            autoConvert();
        }

        //secret F3
        private void secretF3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newMainSettings.SeasonFormat = ++newMainSettings.SeasonFormat % 6;
            autoConvert();
        }

        //secret F4
        private void secretF4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newMainSettings.DashTitle = !newMainSettings.DashTitle;
            autoConvert();
        }

        //secret F5
        private void secretF5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newMainSettings.TitleFormat = ++newMainSettings.TitleFormat % 6;
            //MessageBox.Show(newMainSettings.TitleFormat.ToString());
            autoConvert();
        }

        //secret F6
        private void secretF6ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newMainSettings.ExtFormat = ++newMainSettings.ExtFormat % 3;
            autoConvert();
        }

        //secret F7
        private void secretF7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newMainSettings.TitleSelection = ++newMainSettings.TitleSelection % 3;
            autoConvert();
        }

        //secret F8
        private void secretF8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newMainSettings.TVDataBase = ++newMainSettings.TVDataBase % 3;
            autoConvert();
        }

        #endregion

        #region On the form Buttons

        //Move Button
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                List<int> Selected = new List<int>();
                List<string> folders = new List<string>();
                using (FileOperation fileOp = new FileOperation(new MyProgressSink(Selected, folders, fileList), this))
                {
                    if (menu1.Count() != 0)
                    {
                        string[] folderSettings = menu1[0].Tag.ToString().Split('?');

                        if (!(System.IO.Directory.Exists(folderSettings[1])))
                        {
                            MessageBox.Show("Folder Location Has Been Deleleted or Move", "Folder Unavailable");
                            return;
                        }

                        if (int.Parse(folderSettings[0]) == 1)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                Selected.Add(i);
                                folders.Add(folderSettings[1]);
                                fileOp.MoveItem(fileList[i].FullFileName, folderSettings[1], fileList[i].FileName);
                                //if (MoveFile(fileList[i].FullFileName, (folderSettings[1] + "\\" + fileList[i].FileName)))
                                //fileList[i].FileFolder = folderSettings[1];
                            }
                        }
                        else if (int.Parse(folderSettings[0]) > 1)
                        {
                            List<string> folderlist = folderFinder(folderSettings[1]);
                            string TVFolder = "";
                            for (int z = 0; z < fileList.Count; z++)
                            {
                                string fullFileName = fileList[z].FullFileName;
                                TVFolder = infoFinder(fileList[z].TVShowName, folderlist);

                                if (TVFolder == "")
                                {
                                    if (MessageBox.Show("There is No Such TV Show in the TV Show Folder, Would you like to Create One?", "Create folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        string text = fileList[z].TVShowName;
                                        if (InputBox.Show("Edit Folder Name", "Folder Name:", ref text) == DialogResult.OK)
                                        {
                                            folderlist.Add(text);
                                            TVFolder = text;
                                        }
                                        else
                                        {
                                            Selected.Add(z);
                                            folders.Add(folderSettings[1]);
                                            fileOp.MoveItem(fileList[z].FullFileName, folderSettings[1], fileList[z].FileName);
                                            //if(MoveFile(fullFileName,folderSettings[1] + "\\" + fileList[z].FileName))
                                            //fileList[z].FileFolder = (folderSettings[1]);
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        Selected.Add(z);
                                        folders.Add(folderSettings[1]);
                                        fileOp.MoveItem(fileList[z].FullFileName, folderSettings[1], fileList[z].FileName);
                                        //if(MoveFile(fullFileName,folderSettings[1] + "\\" + fileList[z].FileName))
                                        //fileList[z].FileFolder = (folderSettings[1]);
                                        continue;
                                    }
                                }

                                if (fileList[z].SeasonNum != 0 && int.Parse(folderSettings[0]) == 3)
                                {
                                    if (!(System.IO.Directory.Exists(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString())))
                                        System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString());
                                    Selected.Add(z);
                                    folders.Add(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString());
                                    fileOp.MoveItem(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString(), fileList[z].FileName);
                                    //if (MoveFile(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString() + "\\" + fileList[z].FileName))
                                    //fileList[z].FileFolder = (folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString());
                                }
                                else//if no season is selected 
                                {
                                    Selected.Add(z);
                                    folders.Add(folderSettings[1] + "\\" + TVFolder);
                                    fileOp.MoveItem(fullFileName, folderSettings[1] + "\\" + TVFolder, fileList[z].FileName);
                                    //if (MoveFile(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\" + fileList[z].FileName))
                                    //fileList[z].FileFolder = (folderSettings[1] + "\\" + TVFolder);
                                }//end of if-else                        
                            }//end of for loop 
                        }
                    }
                    else//catch if nothing is selected
                    {
                        if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                Selected.Add(i);
                                folders.Add(folderBrowserDialog2.SelectedPath);
                                fileOp.MoveItem(fileList[i].FullFileName, folderBrowserDialog2.SelectedPath, fileList[i].FileName);
                                //if (MoveFile(fileList[i].FullFileName, (folderBrowserDialog2.SelectedPath + "\\" + fileList[i].FileName)))
                                //fileList[i].FileFolder = folderBrowserDialog2.SelectedPath;
                            }
                        }
                        else
                            return;
                    }
                    fileOp.PerformOperations();
                }
            }
            else
                MessageBox.Show("No Files Selected");

        }//end of move button method

        //copy button
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                using (FileOperation fileOp = new FileOperation(new MyProgressSink(), this))
                {
                    //fileOp.CopyItem(@"C:\test\test1.txt", @"C:\test", @"copy.txt");
                    //fileOp.DeleteItem(@"C:\test\test2.txt");
                    //fileOp.MoveItem(@"C:\test\SampleDir", @"C:\test", @"NewDir");


                    if (menu1.Count() != 0)
                    {
                        string[] folderSettings = menu1[0].Tag.ToString().Split('?');
                        if (!(System.IO.Directory.Exists(folderSettings[1])))
                        {
                            MessageBox.Show("Folder Location Has Been Deleleted or Move", "Folder Unavailable");
                            return;
                        }

                        if (int.Parse(folderSettings[0]) == 1)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                fileOp.CopyItem(fileList[i].FullFileName, folderSettings[1], fileList[i].FileName);
                                //CopyFile(fileList[i].FullFileName, (folderSettings[1] + "\\" + fileList[i].FileName));
                            }
                        }
                        else if (int.Parse(folderSettings[0]) > 1)
                        {
                            List<string> folderlist = folderFinder(folderSettings[1]);
                            string TVFolder = "";
                            for (int z = 0; z < fileList.Count; z++)
                            {
                                string fullFileName = fileList[z].FullFileName;
                                TVFolder = infoFinder(fileList[z].TVShowName, folderlist);

                                if (TVFolder == "")
                                {
                                    if (MessageBox.Show("There is No Such TV Show in the TV Show Folder, Would you like to Create One?", "Create folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        string text = fileList[z].TVShowName;
                                        if (InputBox.Show("Edit Folder Name", "Folder Name:", ref text) == DialogResult.OK)
                                        {
                                            folderlist.Add(text);
                                            TVFolder = text;
                                        }
                                        else
                                        {
                                            fileOp.CopyItem(fileList[z].FullFileName, folderSettings[1], fileList[z].FileName);
                                            //CopyFile(fileList[z].FullFileName, (folderSettings[1] + "\\" + fileList[z].FileName));
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        fileOp.CopyItem(fileList[z].FullFileName, folderSettings[1], fileList[z].FileName);
                                        //CopyFile(fileList[z].FullFileName, (folderSettings[1] + "\\" + fileList[z].FileName));
                                        continue;
                                    }
                                }

                                if (fileList[z].SeasonNum != 0 && int.Parse(folderSettings[0]) == 3)
                                {
                                    if (!(System.IO.Directory.Exists(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString())))
                                        System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString());
                                    fileOp.CopyItem(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString(), fileList[z].FileName);
                                    //CopyFile(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString() + "\\" + fileList[z].FileName);
                                }
                                else
                                {//if no season is selected 
                                    fileOp.CopyItem(fullFileName, folderSettings[1] + "\\" + TVFolder, fileList[z].FileName);
                                    //CopyFile(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\" + fileList[z].FileName);
                                }
                            }//end of for loop 
                        }
                    }
                    else//catch if nothing is selected
                    {
                        if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                fileOp.CopyItem(fileList[i].FullFileName, folderBrowserDialog2.SelectedPath, fileList[i].FileName);
                                //CopyFile(fileList[i].FullFileName, (folderBrowserDialog2.SelectedPath + "\\" + fileList[i].FileName));
                            }
                        }
                        else
                            return;
                    }
                    fileOp.PerformOperations();
                }
            }
            else
                MessageBox.Show("No Files Selected");
        }//end of copy button method

        //save button
        private void button5_Click(object sender, EventArgs e)
        {//save the Files
            if (fileList.Count != 0) //if files are selected
            {
                for (int y = 0; y < fileList.Count(); y++)
                {
                    try
                    {
                        if (fileList[y].FileName == fileList[y].NewFileName) continue;
                        //FileSystem.MoveFile((fileList[y].FullFileName), (fileList[y].NewFullFileName), true);
                        System.IO.File.Move((fileList[y].FullFileName), (fileList[y].NewFullFileName));
                        Log.WriteLog(fileList[y].FileName, fileList[y].NewFileName);
                        fileList[y].FileName = fileList[y].NewFileName;
                        fileList[y].FileTitle = "";
                        dataGridView1.Rows[y].Cells[0].Value = fileList[y].FileName;

                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("File have been changed or moved \n" + (fileList[y].FullFileName) + "\n" + (fileList[y].NewFullFileName));
                        continue;
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("File already exists or is in use\n" + (fileList[y].FullFileName) + "\n" + (fileList[y].NewFullFileName));
                        continue;
                    }
                }//end of for loop

                newMainSettings.SeasonOffset = 0;
                newMainSettings.EpisodeOffset = 0;

                autoConvert();
            }
            else//catch if nothing is selected
                MessageBox.Show("No Files Selected");

        }//end of save filenames method

        //TVDB
        private void button6_Click(object sender, EventArgs e)
        {
            if (fileList.Count != 0 && ConnectionExists()) //if files are selected
            {
                List<int> selected = new List<int>();
                for (int i = 0; i < fileList.Count; i++)
                {
                    if (fileList[i].FileTitle == "")
                        selected.Add(i);
                }
                //TestTitle(selected);
                if (!TitleThread.IsBusy)
                    TitleThread.RunWorkerAsync(selected);
            }
        }

        #endregion

        #region Folder Control Stuff
        private void MenuItemClickHandler1(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            if (dataGridView1.CurrentRow != null)
            {
                //open folder browser
                if (clickedItem.Name == "browserMenu")
                {
                    if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
                        clickedItem.Tag = "1?" + folderBrowserDialog2.SelectedPath;
                    else
                        return;
                }
                string[] folderSettings = clickedItem.Tag.ToString().Split('?');
                if (!(System.IO.Directory.Exists(folderSettings[1])))
                {
                    MessageBox.Show("Folder Location Has Been Deleleted or Move", "Folder Unavailable");
                    return;
                }
                List<int> Selected = new List<int>();
                List<string> folders = new List<string>();
                using (FileOperation fileOp = new FileOperation(new MyProgressSink(Selected, folders, fileList), this))
                {
                    if (int.Parse(folderSettings[0]) == 1)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            Selected.Add(i);
                            folders.Add(folderSettings[1]);
                            fileOp.MoveItem(fileList[i].FullFileName, folderSettings[1], fileList[i].FileName);
                            //if (MoveFile(fileList[i].FullFileName, (folderSettings[1] + "\\" + fileList[i].FileName)))
                            //fileList[i].FileFolder = folderSettings[1];
                        }
                    }
                    else if (int.Parse(folderSettings[0]) > 1)
                    {
                        List<string> folderlist = folderFinder(folderSettings[1]);
                        string TVFolder = "";
                        for (int z = 0; z < fileList.Count; z++)
                        {
                            string fullFileName = fileList[z].FullFileName;
                            TVFolder = infoFinder(fileList[z].TVShowName, folderlist);

                            if (TVFolder == "")
                            {
                                if (MessageBox.Show("There is No Such TV Show in the TV Show Folder, Would you like to Create One?", "Create folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    string text = fileList[z].TVShowName;
                                    if (InputBox.Show("Edit Folder Name", "Folder Name:", ref text) == DialogResult.OK)
                                    {
                                        folderlist.Add(text);
                                        TVFolder = text;
                                    }
                                    else
                                    {
                                        Selected.Add(z);
                                        folders.Add(folderSettings[1]);
                                        fileOp.MoveItem(fullFileName, folderSettings[1], fileList[z].FileName);
                                        //if(MoveFile(fullFileName,folderSettings[1] + "\\" + fileList[z].FileName))
                                        //fileList[z].FileFolder = (folderSettings[1]);
                                        continue;
                                    }
                                }
                                else
                                {
                                    Selected.Add(z);
                                    folders.Add(folderSettings[1]);
                                    fileOp.MoveItem(fullFileName, folderSettings[1], fileList[z].FileName);
                                    //if(MoveFile(fullFileName,folderSettings[1] + "\\" + fileList[z].FileName))
                                    //fileList[z].FileFolder = (folderSettings[1]);
                                    continue;
                                }
                            }

                            if (fileList[z].SeasonNum != 0 && int.Parse(folderSettings[0]) == 3)
                            {
                                if (!(System.IO.Directory.Exists(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString())))
                                    System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString());

                                Selected.Add(z);
                                folders.Add(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString());
                                fileOp.MoveItem(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString(), fileList[z].FileName);
                                //if (MoveFile(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString() + "\\" + fileList[z].FileName))
                                //fileList[z].FileFolder = (folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString());
                            }
                            else//if no season is selected 
                            {
                                Selected.Add(z);
                                folders.Add(folderSettings[1] + "\\" + TVFolder);
                                fileOp.MoveItem(fullFileName, folderSettings[1] + "\\" + TVFolder, fileList[z].FileName);
                                //if (MoveFile(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\" + fileList[z].FileName))
                                //fileList[z].FileFolder = (folderSettings[1] + "\\" + TVFolder);
                            }//end of if-else                        
                        }//end of for loop 
                    }
                    fileOp.PerformOperations();
                }
            }
            else//catch if nothing is selected
                MessageBox.Show("No Files Selected");
        }

        private void MenuItemClickHandler2(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            if (dataGridView1.CurrentRow != null)
            {
                //open folder browser
                if (clickedItem.Name == "browserMenu")
                {
                    if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
                        clickedItem.Tag = "1?" + folderBrowserDialog2.SelectedPath;
                    else
                        return;
                }
                string[] folderSettings = clickedItem.Tag.ToString().Split('?');
                if (!(System.IO.Directory.Exists(folderSettings[1])))
                {
                    MessageBox.Show("Folder Location Has Been Deleleted or Move", "Folder Unavailable");
                    return;
                }
                using (FileOperation fileOp = new FileOperation(new MyProgressSink(), this))
                {
                    if (int.Parse(folderSettings[0]) == 1)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            fileOp.CopyItem(fileList[i].FullFileName, folderSettings[1], fileList[i].FileName);
                            //CopyFile(fileList[i].FullFileName, (folderSettings[1] + "\\" + fileList[i].FileName));
                        }
                    }
                    else if (int.Parse(folderSettings[0]) > 1)
                    {
                        List<string> folderlist = folderFinder(folderSettings[1]);
                        string TVFolder = "";
                        for (int z = 0; z < fileList.Count; z++)
                        {
                            string fullFileName = fileList[z].FullFileName;
                            TVFolder = infoFinder(fileList[z].TVShowName, folderlist);

                            if (TVFolder == "")
                            {
                                if (MessageBox.Show("There is No Such TV Show in the TV Show Folder, Would you like to Create One?", "Create folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    string text = fileList[z].TVShowName;
                                    if (InputBox.Show("Edit Folder Name", "Folder Name:", ref text) == DialogResult.OK)
                                    {
                                        folderlist.Add(text);
                                        TVFolder = text;
                                    }
                                    else
                                    {
                                        fileOp.CopyItem(fileList[z].FullFileName, folderSettings[1], fileList[z].FileName);
                                        //CopyFile(fileList[z].FullFileName, (folderSettings[1] + "\\" + fileList[z].FileName));
                                        continue;
                                    }
                                }
                                else
                                {
                                    fileOp.CopyItem(fileList[z].FullFileName, folderSettings[1], fileList[z].FileName);
                                    //CopyFile(fileList[z].FullFileName, (folderSettings[1] + "\\" + fileList[z].FileName));
                                    continue;
                                }
                            }

                            if (fileList[z].SeasonNum != 0 && int.Parse(folderSettings[0]) == 3)
                            {
                                if (!(System.IO.Directory.Exists(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString())))
                                    System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString());
                                fileOp.CopyItem(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString(), fileList[z].FileName);
                                //CopyFile(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString() + "\\" + fileList[z].FileName);
                            }
                            else
                            {//if no season is selected
                                fileOp.CopyItem(fullFileName, folderSettings[1] + "\\" + TVFolder, fileList[z].FileName);
                                //CopyFile(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\" + fileList[z].FileName);
                            }
                        }//end of for loop 
                    }
                    fileOp.PerformOperations();
                }
            }
            else//catch if nothing is selected
                MessageBox.Show("No Files Selected");
        }

        private void MenuItemClickHandler3(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            if (dataGridView1.CurrentRow != null)
            {
                //open folder browser
                if (clickedItem.Name == "browserMenu")
                {
                    if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
                        clickedItem.Tag = "1?" + folderBrowserDialog2.SelectedPath;
                    else
                        return;
                }
                string[] folderSettings = clickedItem.Tag.ToString().Split('?');
                if (!(System.IO.Directory.Exists(folderSettings[1])))
                {
                    MessageBox.Show("Folder Location Has Been Deleleted or Move", "Folder Unavailable");
                    return;
                }
                List<int> Selected = new List<int>();
                List<string> folders = new List<string>();
                using (FileOperation fileOp = new FileOperation(new MyProgressSink(Selected, folders, fileList), this))
                {
                    if (int.Parse(folderSettings[0]) == 1)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            Selected.Add(i);
                            folders.Add(folderSettings[1]);
                            fileOp.MoveItem(fileList[i].FullFileName, folderSettings[1], fileList[i].FileName);
                            //if (MoveFile(fileList[i].FullFileName, (folderSettings[1] + "\\" + fileList[i].FileName)))
                            //fileList[i].FileFolder = folderSettings[1];
                        }
                    }
                    else if (int.Parse(folderSettings[0]) > 1)
                    {
                        List<string> folderlist = folderFinder(folderSettings[1]);
                        string TVFolder = "";
                        for (int z = 0; z < fileList.Count; z++)
                        {
                            string fullFileName = fileList[z].FullFileName;
                            TVFolder = infoFinder(fileList[z].TVShowName, folderlist);

                            if (TVFolder == "")
                            {
                                if (MessageBox.Show("There is No Such TV Show in the TV Show Folder, Would you like to Create One?", "Create folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    string text = fileList[z].TVShowName;
                                    if (InputBox.Show("Edit Folder Name", "Folder Name:", ref text) == DialogResult.OK)
                                    {
                                        folderlist.Add(text);
                                        TVFolder = text;
                                    }
                                    else
                                    {
                                        Selected.Add(z);
                                        folders.Add(folderSettings[1]);
                                        fileOp.MoveItem(fullFileName, folderSettings[1], fileList[z].FileName);
                                        //if(MoveFile(fullFileName,folderSettings[1] + "\\" + fileList[z].FileName))
                                        //fileList[z].FileFolder = (folderSettings[1]);
                                        continue;
                                    }
                                }
                                else
                                {
                                    Selected.Add(z);
                                    folders.Add(folderSettings[1]);
                                    fileOp.MoveItem(fullFileName, folderSettings[1], fileList[z].FileName);
                                    //if(MoveFile(fullFileName,folderSettings[1] + "\\" + fileList[z].FileName))
                                    //fileList[z].FileFolder = (folderSettings[1]);
                                    continue;
                                }
                            }

                            if (fileList[z].SeasonNum != 0 && int.Parse(folderSettings[0]) == 3)
                            {
                                if (!(System.IO.Directory.Exists(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString())))
                                    System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString());

                                Selected.Add(z);
                                folders.Add(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString());
                                fileOp.MoveItem(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString(), fileList[z].FileName);
                                //if (MoveFile(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString() + "\\" + fileList[z].FileName))
                                //fileList[z].FileFolder = (folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString());
                            }
                            else//if no season is selected 
                            {
                                Selected.Add(z);
                                folders.Add( folderSettings[1] + "\\" + TVFolder);
                                fileOp.MoveItem(fullFileName, folderSettings[1] + "\\" + TVFolder, fileList[z].FileName);
                                //if (MoveFile(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\" + fileList[z].FileName))
                                //fileList[z].FileFolder = (folderSettings[1] + "\\" + TVFolder);
                            }//end of if-else                        
                        }//end of for loop 
                    }

                    fileOp.PerformOperations();
                }
            }
            else//catch if nothing is selected
                MessageBox.Show("No Files Selected");
        }

        private void MenuItemClickHandler4(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            if (dataGridView1.CurrentRow != null)
            {
                //open folder browser
                if (clickedItem.Name == "browserMenu")
                {
                    if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
                        clickedItem.Tag = "1?" + folderBrowserDialog2.SelectedPath;
                    else
                        return;
                }
                string[] folderSettings = clickedItem.Tag.ToString().Split('?');
                if (!(System.IO.Directory.Exists(folderSettings[1])))
                {
                    MessageBox.Show("Folder Location Has Been Deleleted or Move", "Folder Unavailable");
                    return;
                }
                using (FileOperation fileOp = new FileOperation(new MyProgressSink(), this))
                {
                    if (int.Parse(folderSettings[0]) == 1)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            fileOp.CopyItem(fileList[i].FullFileName, folderSettings[1], fileList[i].FileName);
                            //CopyFile(fileList[i].FullFileName, (folderSettings[1] + "\\" + fileList[i].FileName));
                        }
                    }
                    else if (int.Parse(folderSettings[0]) > 1)
                    {
                        List<string> folderlist = folderFinder(folderSettings[1]);
                        string TVFolder = "";
                        for (int z = 0; z < fileList.Count; z++)
                        {
                            string fullFileName = fileList[z].FullFileName;
                            TVFolder = infoFinder(fileList[z].TVShowName, folderlist);

                            if (TVFolder == "")
                            {
                                if (MessageBox.Show("There is No Such TV Show in the TV Show Folder, Would you like to Create One?", "Create folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    string text = fileList[z].TVShowName;
                                    if (InputBox.Show("Edit Folder Name", "Folder Name:", ref text) == DialogResult.OK)
                                    {
                                        folderlist.Add(text);
                                        TVFolder = text;
                                    }
                                    else
                                    {
                                        fileOp.CopyItem(fileList[z].FullFileName, folderSettings[1], fileList[z].FileName);
                                        //CopyFile(fileList[z].FullFileName, (folderSettings[1] + "\\" + fileList[z].FileName));
                                        continue;
                                    }
                                }
                                else
                                {
                                    fileOp.CopyItem(fileList[z].FullFileName, folderSettings[1], fileList[z].FileName);
                                    //CopyFile(fileList[z].FullFileName, (folderSettings[1] + "\\" + fileList[z].FileName));
                                    continue;
                                }
                            }

                            if (fileList[z].SeasonNum != 0 && int.Parse(folderSettings[0]) == 3)
                            {
                                if (!(System.IO.Directory.Exists(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString())))
                                    System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString());
                                fileOp.CopyItem(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString(), fileList[z].FileName);
                                //CopyFile(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\Season " + fileList[z].SeasonNum.ToString() + "\\" + fileList[z].FileName);
                            }
                            else
                            {//if no season is selected
                                fileOp.CopyItem(fullFileName, folderSettings[1] + "\\" + TVFolder, fileList[z].FileName);
                                //CopyFile(fullFileName, folderSettings[1] + "\\" + TVFolder + "\\" + fileList[z].FileName);
                            }
                        }//end of for loop 
                    }
                    fileOp.PerformOperations();
                }
            }
            else//catch if nothing is selected
                MessageBox.Show("No Files Selected");
        }

        public List<ToolStripMenuItem> menu1 = new List<ToolStripMenuItem>();
        public List<ToolStripMenuItem> menu2 = new List<ToolStripMenuItem>();
        public List<ToolStripMenuItem> menu3 = new List<ToolStripMenuItem>();
        public List<ToolStripMenuItem> menu4 = new List<ToolStripMenuItem>();
        ToolStripMenuItem browserMenu1 = new ToolStripMenuItem();
        ToolStripMenuItem browserMenu2 = new ToolStripMenuItem();
        ToolStripMenuItem browserMenu3 = new ToolStripMenuItem();
        ToolStripMenuItem browserMenu4 = new ToolStripMenuItem();

        public void AddFolder(string folderName, string folderDestination, int format)
        {
            moveToToolStripMenuItem1.DropDownItems.Clear();//Move all Menu Item
            copyToToolStripMenuItem.DropDownItems.Clear();//Copy all Menu Item
            moveSelectedToolStripMenuItem.DropDownItems.Clear();//Move selected Menu Item
            copySelectedToolStripMenuItem.DropDownItems.Clear();//copy selected Menu Item

            menu1.Add(new ToolStripMenuItem());
            menu1[menu1.Count - 1].Name = "move" + (menu1.Count - 1).ToString();
            menu1[menu1.Count - 1].Tag = format.ToString() + "?" + folderDestination;
            menu1[menu1.Count - 1].Text = folderName;
            menu1[menu1.Count - 1].Click += new EventHandler(MenuItemClickHandler1);

            moveToToolStripMenuItem1.DropDownItems.AddRange(menu1.ToArray());

            menu2.Add(new ToolStripMenuItem());
            menu2[menu2.Count - 1].Name = "copy" + (menu2.Count - 1).ToString();
            menu2[menu2.Count - 1].Tag = format.ToString() + "?" + folderDestination;
            menu2[menu2.Count - 1].Text = folderName;
            menu2[menu2.Count - 1].Click += new EventHandler(MenuItemClickHandler2);

            copyToToolStripMenuItem.DropDownItems.AddRange(menu2.ToArray());

            menu3.Add(new ToolStripMenuItem());
            menu3[menu3.Count - 1].Name = "moveselect" + (menu3.Count - 1).ToString();
            menu3[menu3.Count - 1].Tag = format.ToString() + "?" + folderDestination;
            menu3[menu3.Count - 1].Text = folderName;
            menu3[menu3.Count - 1].Click += new EventHandler(MenuItemClickHandler3);

            moveSelectedToolStripMenuItem.DropDownItems.AddRange(menu3.ToArray());

            menu4.Add(new ToolStripMenuItem());
            menu4[menu4.Count - 1].Name = "copyselect" + (menu4.Count - 1).ToString();
            menu4[menu4.Count - 1].Tag = format.ToString() + "?" + folderDestination;
            menu4[menu4.Count - 1].Text = folderName;
            menu4[menu4.Count - 1].Click += new EventHandler(MenuItemClickHandler4);

            copySelectedToolStripMenuItem.DropDownItems.AddRange(menu4.ToArray());

            moveToToolStripMenuItem1.DropDownItems.Add(browserMenu1);
            copyToToolStripMenuItem.DropDownItems.Add(browserMenu2);
            moveSelectedToolStripMenuItem.DropDownItems.Add(browserMenu3);
            copySelectedToolStripMenuItem.DropDownItems.Add(browserMenu4);
        }

        public void SaveFolder(string newFolderInfo, int index)
        {
            moveToToolStripMenuItem1.DropDownItems.Clear();//Move all Menu Item
            copyToToolStripMenuItem.DropDownItems.Clear();//Copy all Menu Item
            moveSelectedToolStripMenuItem.DropDownItems.Clear();//Move selected Menu Item
            copySelectedToolStripMenuItem.DropDownItems.Clear();//copy selected Menu Item

            menu1[index].Tag = newFolderInfo;
            menu2[index].Tag = newFolderInfo;
            menu3[index].Tag = newFolderInfo;
            menu4[index].Tag = newFolderInfo;

            moveToToolStripMenuItem1.DropDownItems.AddRange(menu1.ToArray());
            copyToToolStripMenuItem.DropDownItems.AddRange(menu2.ToArray());
            moveSelectedToolStripMenuItem.DropDownItems.AddRange(menu3.ToArray());
            copySelectedToolStripMenuItem.DropDownItems.AddRange(menu4.ToArray());

            moveToToolStripMenuItem1.DropDownItems.Add(browserMenu1);
            copyToToolStripMenuItem.DropDownItems.Add(browserMenu2);
            moveSelectedToolStripMenuItem.DropDownItems.Add(browserMenu3);
            copySelectedToolStripMenuItem.DropDownItems.Add(browserMenu4);
        }

        public void MoveFolders(int index, int way)
        {
            ToolStripMenuItem temp1 = menu1[index + way];
            menu1[index + way] = menu1[index];
            menu1[index] = temp1;

            ToolStripMenuItem temp2 = menu2[index + way];
            menu2[index + way] = menu2[index];
            menu2[index] = temp2;

            ToolStripMenuItem temp3 = menu3[index + way];
            menu3[index + way] = menu3[index];
            menu3[index] = temp3;

            ToolStripMenuItem temp4 = menu4[index + way];
            menu4[index + way] = menu4[index];
            menu4[index] = temp4;
            button1.Text = "Move To " + menu1[0].Text;
            button2.Text = "Copy To " + menu1[0].Text;
        }

        public void AddBrowserMenu()
        {
            browserMenu1.Name = "browserMenu";
            browserMenu1.Text = "Open Folder Browser...";
            browserMenu1.Click += new EventHandler(MenuItemClickHandler1);
            //moveToToolStripMenuItem1.DropDownItems.Add(browserMenu1);

            browserMenu2.Name = "browserMenu";
            browserMenu2.Text = "Open Folder Browser...";
            browserMenu2.Click += new EventHandler(MenuItemClickHandler2);
            //copyToToolStripMenuItem.DropDownItems.Add(browserMenu2);

            browserMenu3.Name = "browserMenu";
            browserMenu3.Text = "Open Folder Browser...";
            browserMenu3.Click += new EventHandler(MenuItemClickHandler3);
            //moveSelectedToolStripMenuItem.DropDownItems.Add(browserMenu3);

            browserMenu4.Name = "browserMenu";
            browserMenu4.Text = "Open Folder Browser...";
            browserMenu4.Click += new EventHandler(MenuItemClickHandler4);
            //copySelectedToolStripMenuItem.DropDownItems.Add(browserMenu4);

            if (menu1.Count == 0)
            {
                moveToToolStripMenuItem1.DropDownItems.Add(browserMenu1);
                copyToToolStripMenuItem.DropDownItems.Add(browserMenu2);
                moveSelectedToolStripMenuItem.DropDownItems.Add(browserMenu3);
                copySelectedToolStripMenuItem.DropDownItems.Add(browserMenu4);
            }
        }

        public void ClearOtherFolder()
        {
            moveToToolStripMenuItem1.DropDownItems.Clear();
            copyToToolStripMenuItem.DropDownItems.Clear();
            moveSelectedToolStripMenuItem.DropDownItems.Clear();
            copySelectedToolStripMenuItem.DropDownItems.Clear();

            moveToToolStripMenuItem1.DropDownItems.AddRange(menu1.ToArray());
            moveToToolStripMenuItem1.DropDownItems.Add(browserMenu1);

            copyToToolStripMenuItem.DropDownItems.AddRange(menu2.ToArray());
            copyToToolStripMenuItem.DropDownItems.Add(browserMenu2);

            moveSelectedToolStripMenuItem.DropDownItems.AddRange(menu3.ToArray());
            moveSelectedToolStripMenuItem.DropDownItems.Add(browserMenu3);

            copySelectedToolStripMenuItem.DropDownItems.AddRange(menu4.ToArray());
            copySelectedToolStripMenuItem.DropDownItems.Add(browserMenu4);
            if (menu1.Count() != 0)
            {
                button1.Text = "Move To " + menu1[0].Text;
                button2.Text = "Copy To " + menu1[0].Text;
            }
            else
            {
                button1.Text = "Move To Folder";
                button2.Text = "Copy To Folder";
            }
        }
        #endregion

        #region Update Stuff

        //check to see if internet is avilible
        bool ConnectionExists()
        {
            System.Uri Url = new System.Uri("http://www.microsoft.com");

            System.Net.WebRequest WebReq;
            System.Net.WebResponse Resp;
            WebReq = System.Net.WebRequest.Create(Url);

            try
            {
                Resp = WebReq.GetResponse();
                Resp.Close();
                WebReq = null;
                return true;
            }

            catch
            {
                WebReq = null;
                return false;
            }
        }

        //check to see if website is avilible
        bool websiteExists()
        {
            try
            {
                System.Net.Sockets.TcpClient clnt = new System.Net.Sockets.TcpClient("scottnation.com", 80);
                clnt.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }//end of ConnectionExists class

        //check for updates
        private void checkForUpdate()
        {
            if (this.ConnectionExists())
            {
                if (this.websiteExists())
                {
                    try
                    {
                        WebRequest request = WebRequest.Create(new Uri("http://update.scottnation.com/TV_Show_Renamer/webversion.xml"));
                        request.Method = "HEAD";
                        WebResponse response = request.GetResponse();
                    }
                    catch (Exception)
                    {
                        Log.WriteLog("webversion.xml file doownload failed");
                        MessageBox.Show("Problem with Server\nPlease Contact Admin");
                        return;
                    }
                    WebClient webClient = new WebClient();
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    webClient.DownloadFileAsync(new Uri("http://update.scottnation.com/TV_Show_Renamer/webversion.xml"), newMainSettings.DataFolder + "\\webversion.xml");

                }
                else
                {
                    Log.WriteLog("Server is unavalible Please try again later");
                    MessageBox.Show("Server is unavalible\nPlease try again later");
                }
            }
            else
            {
                Log.WriteLog("No Internet Connection Avalible Please check connection");
                MessageBox.Show("No Internet Connection Avalible\nPlease Check Connection");
            }
        }//check for update method

        //runs when xml file is done downloading
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {   //MessageBox.Show("Download completed!");
            List<string> webInfo = this.updateXmlRead();//read file
            List<string> localInfo = this.localXmlRead();
            if (Convert.ToInt32(webInfo[0]) > Convert.ToInt32(localInfo[0]))
            {   //global update crap
                if (MessageBox.Show("There is an update available, Would you like to update?\nNOTE: This will reinstall the program", "Update Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MethodInvoker action5 = delegate
                    {
                        this.fullUpdate();
                    };
                    this.BeginInvoke(action5);
                    //this.fullUpdate();
                }
                else
                {
                    if (Convert.ToInt32(webInfo[1]) > Convert.ToInt32(localInfo[1]))
                    {   //libaray update crap
                        if (MessageBox.Show("There is a library update available, Would you like to update?\nNOTE: This will just replace certain files", "Update Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            this.libarayUpdate();
                    }
                }
            }
            else if (Convert.ToInt32(webInfo[1]) > Convert.ToInt32(localInfo[1]))
            {   //libaray update crap
                if (MessageBox.Show("There is a libaray update available, Would you like to update?\nNOTE: This will just replace certain files", "Update Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.libarayUpdate();
            }
            else  //no updats available
                MessageBox.Show("No updates available");
        }//update complete method

        //check for updates silently
        public void checkForUpdateSilent()
        {
            if (this.ConnectionExists())
            {
                if (this.websiteExists())
                {
                    try
                    {
                        WebRequest request = WebRequest.Create(new Uri("http://update.scottnation.com/TV_Show_Renamer/webversion.xml"));
                        request.Method = "HEAD";
                        WebResponse response = request.GetResponse();
                    }
                    catch (Exception)
                    {
                        Log.WriteLog("webversion.xml file doownload failed");
                        return;
                    }
                    WebClient webClient = new WebClient();
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(CompletedSilent);
                    webClient.DownloadFileAsync(new Uri("http://update.scottnation.com/TV_Show_Renamer/webversion.xml"), newMainSettings.DataFolder + "\\webversion.xml");

                }
                else
                    Log.WriteLog("Server is unavalible Please try again later");
            }
            else
                Log.WriteLog("No Internet Connection Avalible Please check connection");
        }//end of checkForUpdateSilent method

        //runs when xml file is done downloading
        private void CompletedSilent(object sender, AsyncCompletedEventArgs e)
        {
            List<string> webInfo = this.updateXmlRead();//read file
            List<string> localInfo = this.localXmlRead();
            if (Convert.ToInt32(webInfo[0]) > Convert.ToInt32(localInfo[0]))
            {   //global update crap
                if (MessageBox.Show("There is an update available, Would you like to update?\nNOTE: This will reinstall the program", "Update Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MethodInvoker action5 = delegate
                    {
                        this.fullUpdate();
                    };
                    this.BeginInvoke(action5);
                }
                else
                {
                    if (Convert.ToInt32(webInfo[1]) > Convert.ToInt32(localInfo[1]))
                    {   //libaray update crap
                        if (MessageBox.Show("There is a library update available, Would you like to update?\nNOTE: This will just replace certain files", "Update Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            this.libarayUpdate();
                    }
                }
            }
            else if (Convert.ToInt32(webInfo[1]) > Convert.ToInt32(localInfo[1]))
            {   //libaray update crap
                if (MessageBox.Show("There is a libaray update available, Would you like to update?\nNOTE: This will just replace certain files", "Update Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.libarayUpdate();
            }
        }//update complete method silently

        //get info off internet
        public List<String> updateXmlRead()
        {
            string document = newMainSettings.DataFolder + "//webversion.xml";
            XmlDataDocument myxmlDocument = new XmlDataDocument();
            myxmlDocument.Load(document);
            XmlTextReader xmlReader = new XmlTextReader(document);
            List<string> data = new List<string>();

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            if (xmlReader.Name == "application")
                                data.Add(xmlReader.ReadString());
                            if (xmlReader.Name == "library")
                                data.Add(xmlReader.ReadString());
                            break;
                        }//end of case 
                }//end of switch
            }//end of while loop
            xmlReader.Close();
            try
            {
                System.IO.File.Delete(newMainSettings.DataFolder + "//webversion.xml");
            }
            catch (Exception)
            {
                //MessageBox.Show("not working");
            }
            return data;
        }//end of WebXMLReader Method

        //read local info
        public List<String> localXmlRead()
        {
            string document = newMainSettings.DataFolder + "//version.xml";
            XmlDataDocument myxmlDocument = new XmlDataDocument();
            myxmlDocument.Load(document);
            XmlTextReader xmlReader = new XmlTextReader(document);
            List<string> data = new List<string>();

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            if (xmlReader.Name == "application")
                                data.Add(xmlReader.ReadString());
                            if (xmlReader.Name == "library")
                                data.Add(xmlReader.ReadString());
                            break;
                        }//end of case 
                }//end of switch
            }//end of while loop
            return data;
        }//end of XMLReader Method

        //libaray Update
        private void libarayUpdate()
        {
            if (this.ConnectionExists())
            {
                /*check is not working
                  try
                {
                    WebRequest request = WebRequest.Create(new Uri("http://update.scottnation.com/TV_Show_Renamer/library.seh"));
                    request.Method = "HEAD";
                    WebResponse response = request.GetResponse();
                    Console.WriteLine("{0} {1}", response.ContentLength, response.ContentType);
                }
                catch (Exception)
                {
                    MessageBox.Show("Problem with Server\nPlease Contact Admin");
                    return;
                }*/
                if (File.Exists(newMainSettings.DataFolder + "//library.seh"))
                    File.Delete(newMainSettings.DataFolder + "//library.seh");
                WebClient webClient2 = new WebClient();
                webClient2.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed2);
                webClient2.DownloadFileAsync(new Uri("http://update.scottnation.com/TV_Show_Renamer/library.seh"), newMainSettings.DataFolder + "\\library.seh");
            }
            else
            {
                Log.WriteLog("No internet connection avalible Please check connection");
                MessageBox.Show("No internet connection avalible\nPlease check connection");
            }
        }

        //finish for library 
        private void Completed2(object sender, AsyncCompletedEventArgs e)
        {
            Log.WriteLog("Libary Update completed");
            MessageBox.Show("Update completed!");
        }

        //full Update
        private void fullUpdate()
        {
            download update = new download(newMainSettings.DataFolder, this);
            update.Show();

            //MethodInvoker action5 = delegate
            // {
            //    this.Hide();
            // };
            //this.BeginInvoke(action5);// dataGridView1.BeginInvoke(action5);

            this.Hide();
        }

        #endregion

        #region Other Methods

        #region Public Mehtods

        //change Buttons color
        public void changeButtoncolor(Color newColor)
        {
            button1.BackColor = newColor;
            button2.BackColor = newColor;
            button5.BackColor = newColor;
            button6.BackColor = newColor;
        }

        public void autoConvert()
        {
            if (fileList.Count == 0)
                return;
            ThreadAdd FolderToAdd = new ThreadAdd();
            FolderToAdd.AddType = "convert";
            addFilesToThread(FolderToAdd);
        }

        public void autoConvertNoTitle()
        {
            if (fileList.Count == 0)
                return;
            ThreadAdd FolderToAdd = new ThreadAdd();
            FolderToAdd.AddType = "convertNoTitle";
            addFilesToThread(FolderToAdd);
        }

        //write log called by download form
        public void writeLog(string error)
        {
            Log.WriteLog(error);
        }

        //change bool openZIPs
        public void changeZIPstate(bool localZIP)
        {
            newMainSettings.OpenZIPs = localZIP;
        }

        //add title
        public bool addTitle(string title)
        {
            bool worked = false;
            if (fileList.Count != 0 && dataGridView1.CurrentRow != null)
            {
                worked = true;
                int r = dataGridView1.CurrentRow.Index;
                fileList[r].FileTitle = title;
            }
            else
                MessageBox.Show("No files selected");
            return worked;
        }

        //clear titles
        public void clearTitles()
        {
            for (int i = 0; i < fileList.Count(); i++)
                fileList[i].FileTitle = "";
            autoConvert();
        }

        //remove selected titles
        public void removeTitle()
        {
            deleteSelectedTitles();
        }

        //return selected titles index
        public List<int> getSelected()
        {
            List<int> u = new List<int>();
            if (dataGridView1.CurrentRow != null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected)
                        u.Add(i);
                }
            }
            return u;
        }

        //return selected titles
        public List<string> getSelectedFileNames()
        {
            List<string> u = new List<string>();
            if (dataGridView1.CurrentRow != null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected)
                        u.Add(fileList[i].FileName);
                }
            }
            return u;
        }

        //close app for update
        public void CloseForUpdates()
        {
            AddFilesThread.CancelAsync();
            TitleThread.CancelAsync();
            newMainSettings.ClosedForUpdates = true;
            Application.Exit();
        }

        #endregion

        #region Private Methods

        private void addFilesToThread(ThreadAdd newItems)
        {
            convertionQueue.Enqueue(newItems);
            if (!AddFilesThread.IsBusy)
                AddFilesThread.RunWorkerAsync();
        }

        private void AddFilesThread_DoWork(object sender, DoWorkEventArgs e)
        {
            if (convertionQueue.Count == 0)
                return;
            ThreadAdd tempInfo = (ThreadAdd)convertionQueue.Dequeue();

            switch (tempInfo.AddType)
            {
                case "files":
                    string[] files = (string[])tempInfo.ObjectToAdd;
                    BindingList<TVClass> NewFileList = new BindingList<TVClass>();

                    if (files == null)
                        return;
                    foreach (string pendingFileName in files)
                    {
                        FileInfo fi3 = new FileInfo(pendingFileName);
                        if (fi3.Extension == ".zip" || fi3.Extension == ".rar" || fi3.Extension == ".r01" || fi3.Extension == ".001" || fi3.Extension == ".7z")
                            archiveExtrector(pendingFileName, fi3.Name, true);
                        else
                        {
                            if (fi3.Extension == "" || fi3.Extension == null)
                                break;
                            NewFileList.Add(new TVClass(fi3.DirectoryName, fi3.Name, fi3.Extension));
                            fileRenamer(NewFileList, false);
                        }
                    }

                    MethodInvoker action = delegate
                    {
                        foreach (TVClass addobject in NewFileList)
                            fileList.Add(addobject);
                        dataGridView1.Refresh();
                        dataGridView1.AutoResizeColumns();
                    };
                    dataGridView1.BeginInvoke(action);
                    break;
                case "folder":
                    string folder = (string)tempInfo.ObjectToAdd;

                    if (newMainSettings.OpenZIPs)
                        ProcessDirZIP(folder);
                    ProcessDir(folder, 0);
                    fileRenamer(fileList, false);
                    break;
                case "convert":
                    fileRenamer(fileList, false);
                    break;
                case "convertNoTitle":
                    e.Result = "NoTitle";
                    fileRenamer(fileList, true);
                    break;
                default:
                    MessageBox.Show("default");
                    break;
            }
        }

        private void AddFilesThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool getTitle = true;
            MethodInvoker action = delegate
            {
                dataGridView1.Refresh();
                dataGridView1.AutoResizeColumns();
            };
            dataGridView1.BeginInvoke(action);

            if (e.Result != null)
            {
                if (e.Result.ToString() == "NoTitle")
                    getTitle = false;
            }
            if (convertionQueue.Count != 0)
                TitleThread.RunWorkerAsync();
            else
            {
                if (newMainSettings.AutoGetTitle && fileList.Count != 0 && getTitle && newMainSettings.TitleSelection != 1)
                {
                    List<int> selected = new List<int>();
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        if (fileList[i].SeasonNum != -1 && fileList[i].EpisodeNum != -1 && fileList[i].AutoEdit && fileList[i].GetTitle)
                        {
                            selected.Add(i);
                            //fileList[i].GetTitle = false;
                        }
                    }
                    //TestTitle(selected);
                    if (selected.Count != 0)
                    {
                        if (!TitleThread.IsBusy)
                            TitleThread.RunWorkerAsync(selected);
                    }
                }
            }
        }

        private void TitleThread_DoWork(object sender, DoWorkEventArgs e)
        {
            if (TitleThread.CancellationPending) return;
            if (fileList.Count == 0) return;
            List<int> selected4 = (List<int>)e.Argument;
            if (selected4 == null)
                return;
            if (selected4.Count() == 0)
                return;
            switch (newMainSettings.TVDataBase)
            {
                case 0://TVDB
                    for (int mainindex = 0; mainindex < selected4.Count; mainindex++)
                    {
                        if (TitleThread.CancellationPending) return;
                        NewTVDB GetTitles = new NewTVDB(newMainSettings.DataFolder);
                        if (fileList[selected4[mainindex]].TVShowID == -1)
                            fileList[selected4[mainindex]].TVShowID = SearchTVShowName(fileList[selected4[mainindex]].TVShowName);

                        if (fileList[selected4[mainindex]].TVShowID != -1 && TVShowInfoList[fileList[selected4[mainindex]].TVShowID].TVDBID != -1)
                            fileList[selected4[mainindex]].FileTitle = GetTitles.getTitle(TVShowInfoList[fileList[selected4[mainindex]].TVShowID].TVDBID, fileList[selected4[mainindex]].SeasonNum, fileList[selected4[mainindex]].EpisodeNum);
                        else
                        {
                            SearchInfo newID = GetTitles.findTitle(fileList[selected4[mainindex]].TVShowName);
                            if (newID.SelectedValue != -1)
                            {
                                fileList[selected4[mainindex]].FileTitle = GetTitles.getTitle(newID.SelectedValue, fileList[selected4[mainindex]].SeasonNum, fileList[selected4[mainindex]].EpisodeNum);
                                if (fileList[selected4[mainindex]].TVShowID != -1)
                                {
                                    TVShowInfoList[fileList[selected4[mainindex]].TVShowID].TVDBID = newID.SelectedValue;
                                    TVShowInfoList[fileList[selected4[mainindex]].TVShowID].RealTVShowName = newID.Title;
                                }
                                else
                                    TVShowInfoList.Add(new TVShowInfo(fileList[selected4[mainindex]].TVShowName, newID.Title, "", newID.SelectedValue, -1, "-1"));
                            }
                        }
                    }
                    break;
                case 1:
                    for (int mainindex2 = 0; mainindex2 < selected4.Count; mainindex2++)
                    {
                        if (TitleThread.CancellationPending) return;
                        TVRage GetTitles = new TVRage();

                        if (fileList[selected4[mainindex2]].TVShowID == -1)
                            fileList[selected4[mainindex2]].TVShowID = SearchTVShowName(fileList[selected4[mainindex2]].TVShowName);

                        if (fileList[selected4[mainindex2]].TVShowID != -1 && TVShowInfoList[fileList[selected4[mainindex2]].TVShowID].RageTVID != -1)
                            fileList[selected4[mainindex2]].FileTitle = GetTitles.getTitle(TVShowInfoList[fileList[selected4[mainindex2]].TVShowID].RageTVID, fileList[selected4[mainindex2]].SeasonNum, fileList[selected4[mainindex2]].EpisodeNum);
                        else
                        {
                            SearchInfo newID = GetTitles.findTitle(fileList[selected4[mainindex2]].TVShowName);
                            if (newID.SelectedValue != -1)
                            {
                                fileList[selected4[mainindex2]].FileTitle = GetTitles.getTitle(newID.SelectedValue, fileList[selected4[mainindex2]].SeasonNum, fileList[selected4[mainindex2]].EpisodeNum);
                                if (fileList[selected4[mainindex2]].TVShowID != -1)
                                {
                                    TVShowInfoList[fileList[selected4[mainindex2]].TVShowID].RageTVID = newID.SelectedValue;
                                    TVShowInfoList[fileList[selected4[mainindex2]].TVShowID].RealTVShowName = newID.Title;
                                }
                                else
                                    TVShowInfoList.Add(new TVShowInfo(fileList[selected4[mainindex2]].TVShowName, newID.Title, "", -1, newID.SelectedValue,"-1"));
                            }
                        }
                    }
                    break;
                case 2:
                    for (int mainindex3 = 0; mainindex3 < selected4.Count; mainindex3++)
                    {
                        if (TitleThread.CancellationPending) return;
                        EPGuides GetTitles = new EPGuides(newMainSettings.DataFolder);

                        if (fileList[selected4[mainindex3]].TVShowID == -1)
                            fileList[selected4[mainindex3]].TVShowID = SearchTVShowName(fileList[selected4[mainindex3]].TVShowName);

                        if (fileList[selected4[mainindex3]].TVShowID != -1 && TVShowInfoList[fileList[selected4[mainindex3]].TVShowID].EpguidesID != "-1")
                            fileList[selected4[mainindex3]].FileTitle = GetTitles.getTitle(TVShowInfoList[fileList[selected4[mainindex3]].TVShowID].EpguidesID, fileList[selected4[mainindex3]].SeasonNum, fileList[selected4[mainindex3]].EpisodeNum);
                        else
                        {
                            SearchInfo newID = GetTitles.findTitle(fileList[selected4[mainindex3]].TVShowName);
                            if (newID.SelectedValue != -1)
                            {
                                fileList[selected4[mainindex3]].FileTitle = GetTitles.getTitle(newID.NewTitle, fileList[selected4[mainindex3]].SeasonNum, fileList[selected4[mainindex3]].EpisodeNum);
                                if (fileList[selected4[mainindex3]].TVShowID != -1)
                                {
                                    TVShowInfoList[fileList[selected4[mainindex3]].TVShowID].EpguidesID = newID.NewTitle;
                                    TVShowInfoList[fileList[selected4[mainindex3]].TVShowID].RealTVShowName = newID.Title;
                                }
                                else
                                    TVShowInfoList.Add(new TVShowInfo(fileList[selected4[mainindex3]].TVShowName, newID.Title, "", -1, -1, newID.NewTitle));
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            autoConvertNoTitle();
        }

        private int SearchTVShowName(string TVShowName)
        {
            int i = 0;
            foreach (TVShowInfo SearchInfo in TVShowInfoList)
            {
                int difference = Math.Abs(SearchInfo.TVShowName.Length - TVShowName.Length);
                if (SearchInfo.TVShowName.ToUpper().Contains(TVShowName.ToUpper()) && difference < 6)
                    return i;
                i++;
            }
            return -1;
        }

        //Search TV Show TVDB ID List
        //private int SearchTVDBID(string TVShowName)
        //{
        //    foreach (TVShowInfo SearchInfo in TVShowInfoList)
        //    {
        //        int difference = Math.Abs(SearchInfo.TVShowName.Length - TVShowName.Length);
        //        if (SearchInfo.TVShowName.ToUpper().Contains(TVShowName.ToUpper()) && difference < 8)
        //            return SearchInfo.TVDBID;                
        //    }
        //    return -1;
        //}

        ////Search TV Show TVRage ID List
        //private int SearchTVRageID(string TVShowName)
        //{
        //    foreach (TVShowInfo SearchInfo in TVShowInfoList)
        //    {
        //        if (TVShowName == SearchInfo.TVShowName)
        //            return SearchInfo.RageTVID;
        //    }
        //    return -1;
        //}

        ////Search TV Show Epguides ID List
        //private int SearchEpguidesID(string TVShowName)
        //{
        //    foreach (TVShowInfo SearchInfo in TVShowInfoList)
        //    {
        //        if (TVShowName == SearchInfo.TVShowName)
        //            return SearchInfo.EpguidesID;
        //    }
        //    return -1;
        //}

        ////Search TV Show Real Name List
        //private string SearchRealName(string TVShowName)
        //{
        //    foreach (TVShowInfo SearchInfo in TVShowInfoList)
        //    {
        //        if (TVShowName == SearchInfo.TVShowName)
        //            return SearchInfo.RealTVShowName;
        //    }
        //    return "";
        //}

        ////Search TV Show Folder List
        //private string SearchTVShowFolder(string TVShowName)
        //{
        //    foreach (TVShowInfo SearchInfo in TVShowInfoList)
        //    {
        //        if (TVShowName == SearchInfo.TVShowName)
        //            return SearchInfo.TVShowFolder;
        //    }
        //    return "";
        //}

        //returns folderName if folder exsist
        private string infoFinder(string showName, List<string> folderlist)
        {
            int indexofTVshow = -1;
            foreach (string folderName in folderlist)// (int ifolder = 0; ifolder < folderlist.Count(); ifolder++)
            {
                int difference = Math.Abs(folderName.Length - showName.Length);
                indexofTVshow = folderName.IndexOf(showName, StringComparison.InvariantCultureIgnoreCase);
                if (indexofTVshow != -1 && difference < 3)
                {
                    return folderName;
                }
            }//end of for loop            
            return "";
        }//end of infofinder method

        //get list of folders
        private List<string> folderFinder(string folderwatch)
        {
            List<string> foldersIn = new List<string>();
            List<string> revFoldersIn = new List<string>();

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folderwatch);
            try
            {
                foreach (System.IO.DirectoryInfo fi in di.GetDirectories())
                    foldersIn.Add(fi.Name);
            }
            catch (IOException)
            {
                return revFoldersIn;
            }

            //Sort folders
            foldersIn.Sort();
            for (int y = foldersIn.Count(); y > 0; y--)
                revFoldersIn.Add(foldersIn[y - 1]);
            return revFoldersIn;
        }

        //uppercase stuff with parameters
        private string UpperCaseing(string orig, int start, int end)
        {//make every thing lowercase for crap remover to work
            if (orig == "") return "";
            StringBuilder s = new StringBuilder(orig);
            for (int l = start; l < end; l++)
                s[l] = char.ToUpper(s[l]);
            return s.ToString();
        }

        //uppercase stuff after spaces with parameters
        private string UpperCaseingAfterSpace(string orig, int start, int end)
        {
            if (orig == "") return "";
            StringBuilder s2 = new StringBuilder(orig);

            //make first letter capital
            //if (first)
            s2[start] = char.ToUpper(s2[start]);

            //Finds Letter after spaces and capitalizes them
            for (int i = start; i < end; i++)
            {
                if (s2[i] == ' ' || s2[i] == '.')
                    s2[i + 1] = char.ToUpper(s2[i + 1]);
            }//end of for loop

            return s2.ToString();
        }

        //rename method
        private bool fileRenamer(BindingList<TVClass> EditFileList, bool secondTime)
        {//make temp function
            for (int index = 0; index < EditFileList.Count(); index++)
            {
                bool addSeasonFormat = true;

                if (!EditFileList[index].AutoEdit)
                    continue;
                string newfilename = EditFileList[index].FileName;
                string extend = EditFileList[index].FileExtention;
                string showTitle = EditFileList[index].FileTitle;
                string tvshowName = "";
                string finalShowName = "";
                string temp = null;
                string seasonDash = "";
                string titleDash = "";
                string formattedSeason = "";
                string formattedEpisode = "";
                int startIndex = -1;
                int endIndex = -1;

                EditFileList[index].GetTitle = true;

                if (!newMainSettings.DashSeason)
                    seasonDash = "- ";
                if (!newMainSettings.DashTitle)
                    titleDash = "- ";
                if (newMainSettings.RemovePeriod)
                    temp = " ";
                else
                    temp = ".";

                //remove extention
                newfilename = newfilename.Replace(extend, temp + "&&&&");

                //add word at begining
                if (newMainSettings.FirstWord != "")
                {
                    newfilename = newMainSettings.FirstWord + temp + newfilename;
                    //newMainSettings.FirstWord = "";
                }

                //Text converter            
                textConverter = textConvert.getText();
                for (int x = 0; x < textConverter.Count(); x += 2)
                    newfilename = newfilename.Replace(textConverter[x], textConverter[x + 1]);

                //user junk list
                if (newMainSettings.RemoveCrap)
                {
                    //make user junk list
                    userjunklist = userJunk.getjunk();
                    if (userjunklist.Count() != 0)
                    {
                        for (int x = 0; x < userjunklist.Count(); x++)
                            newfilename = newfilename.Replace(userjunklist[x], "");
                    }//end of if
                }//end of removeExtraCrapToolStripMenuItem if

                //replace periods(".") with spaces 
                if (newMainSettings.RemovePeriod)
                    newfilename = newfilename.Replace(".", temp);

                //Replace "_" with spaces
                if (newMainSettings.RemoveUnderscore)
                    newfilename = newfilename.Replace("_", temp);

                //Replace "-" with spaces
                if (newMainSettings.RemoveDash)
                    newfilename = newfilename.Replace("-", temp);

                //Replace (), {}, and [] with spaces
                if (newMainSettings.RemoveBracket)
                    newfilename = newfilename.Replace("(", temp).Replace(")", temp).Replace("{", temp).Replace("}", temp).Replace("[", temp).Replace("]", temp);

                //make every thing lowercase for crap remover to work
                newfilename = newfilename.ToLower();// s.ToString();

                //remove extra crap 
                if (newMainSettings.RemoveCrap)
                {
                    //new way with file input
                    for (int x = 0; x < junklist.Count(); x++)
                        newfilename = newfilename.Replace(junklist[x] + temp, temp);
                }//end of removeExtraCrapToolStripMenuItem if

                //remove begining space
                newfilename = newfilename.TrimStart(temp.ToCharArray());

                //remove year function
                if (newMainSettings.RemoveYear && (!(newMainSettings.SeasonFormat == 4)))
                {
                    int curyear = System.DateTime.Now.Year;
                    for (; curyear > 1900; curyear--)
                    {
                        string before = newfilename;
                        newfilename = newfilename.Replace(curyear.ToString(), "");
                        //break if value found
                        if (before != newfilename)
                            break;
                    }//end of for loop
                }//end of remove year function

                //Removes extra Spaces and periods
                string[] tempspace = new string[newfilename.Length];
                string[] tempper = new string[newfilename.Length];
                tempspace[0] = " ";
                tempper[0] = ".";

                //loop to create arrays of periods/spaces
                for (int i = 1; i < newfilename.Length; i++)
                {
                    tempspace[i] = tempspace[i - 1] + " ";
                    tempper[i] = tempper[i - 1] + ".";
                }//end of for 

                for (int k = newfilename.Length - 1; k > 0; k--)
                {
                    newfilename = newfilename.Replace(tempspace[k], " ");
                    newfilename = newfilename.Replace(tempper[k], ".");
                }//end of for                          

                #region Regular Format
                //loop for seasons
                for (int i = 1; i < 41; i++)
                {
                    //varable for break command later
                    bool end = false;

                    if (i == 40) i = 0;

                    //loop for episodes
                    for (int j = 0; j < 150; j++)
                    {
                        string newi = i.ToString();
                        string newi2 = (i + newMainSettings.SeasonOffset).ToString();
                        string newj = j.ToString();

                        string newj2 = (j + newMainSettings.EpisodeOffset).ToString();
                        string output = null;

                        //check if i is less than 10
                        if (i < 10)
                            newi = "0" + i.ToString();
                        //check if j is less than 10
                        if (j < 10)
                            newj = "0" + j.ToString();
                        if ((i + newMainSettings.SeasonOffset) < 10)
                            newi2 = "0" + (i + newMainSettings.SeasonOffset).ToString();
                        //check if j is less than 10
                        if ((j + newMainSettings.EpisodeOffset) < 10)
                            newj2 = "0" + (j + newMainSettings.EpisodeOffset).ToString();

                        //1x01 format 
                        if (newMainSettings.SeasonFormat == 0)
                        {
                            output = i.ToString() + "x" + newj;

                            newfilename = newfilename.Replace(temp + i.ToString() + newj + temp, temp + output + temp);//101
                            newfilename = newfilename.Replace(temp + newi + newj + temp, temp + output + temp);//0101                        
                            newfilename = newfilename.Replace("s" + i.ToString() + "e" + j.ToString() + temp, output + temp);//s1e1               
                            newfilename = newfilename.Replace("s" + i.ToString() + "e" + newj + temp, output + temp);//s1e01
                            newfilename = newfilename.Replace("s" + newi + "e" + newj + temp, output + temp);//s01e01
                            newfilename = newfilename.Replace("s" + newi + " e" + newj + temp, output + temp);//s01 e01
                            newfilename = newfilename.Replace("season " + i.ToString() + " episode " + newj + temp, output + temp);//season 1 episode 01
                            newfilename = newfilename.Replace("season " + i.ToString() + " episode " + j.ToString() + temp, output + temp);//season 1 episode 1
                            newfilename = newfilename.Replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);//1 01

                            startIndex = newfilename.IndexOf(temp + output + temp);//find index                        
                            if (startIndex != -1)
                            {
                                if (i > 9) { endIndex = startIndex + 6; } else { endIndex = startIndex + 5; }
                                //endIndex = startIndex + 4;
                            }
                        }

                        //0101 format
                        if (newMainSettings.SeasonFormat == 1)
                        {
                            output = newi + newj;

                            newfilename = newfilename.Replace(temp + i.ToString() + newj + temp, temp + output + temp);//101 
                            newfilename = newfilename.Replace(temp + i.ToString() + "x" + newj + temp, temp + output + temp);//1x01
                            newfilename = newfilename.Replace("s" + newi + "e" + newj + temp, output + temp);//s01e01
                            newfilename = newfilename.Replace("s" + newi + " e" + newj + temp, output + temp);//s01 e01
                            newfilename = newfilename.Replace("s" + i.ToString() + "e" + j.ToString() + temp, output + temp);//s1e1
                            newfilename = newfilename.Replace("s" + i.ToString() + "e" + newj + temp, output + temp);//s1e01
                            newfilename = newfilename.Replace("season " + i.ToString() + " episode " + newj + temp, output + temp);//season 1 episode 01
                            newfilename = newfilename.Replace("season " + i.ToString() + " episode " + j.ToString() + temp, output + temp);//season 1 episode 1
                            newfilename = newfilename.Replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);// 1 01 

                            startIndex = newfilename.IndexOf(temp + output + temp);//find index
                            if (startIndex != -1)
                                endIndex = startIndex + 5;
                        }

                        //S01E01 format
                        if (newMainSettings.SeasonFormat == 2)
                        {
                            output = "S" + newi + "E" + newj;

                            newfilename = newfilename.Replace(temp + i.ToString() + newj + temp, temp + output + temp);//101
                            newfilename = newfilename.Replace(temp + i.ToString() + "x" + newj + temp, temp + output + temp);//1x01
                            newfilename = newfilename.Replace(newi + newj + temp, output + temp);//0101
                            newfilename = newfilename.Replace("s" + newi + "e" + newj + temp, output + temp);//s01E01
                            newfilename = newfilename.Replace("s" + i.ToString() + "e" + j.ToString() + temp, output + temp);//s1e1
                            newfilename = newfilename.Replace("s" + i.ToString() + "e" + newj + temp, output + temp);//s1e01
                            newfilename = newfilename.Replace("s" + newi + " e" + newj + temp, output + temp);//s01 e01
                            newfilename = newfilename.Replace("season " + i.ToString() + " episode " + newj + temp, output + temp);//season 1 episode 01
                            newfilename = newfilename.Replace("season " + i.ToString() + " episode " + j.ToString() + temp, output + temp);//Season 1 Episode 1
                            newfilename = newfilename.Replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);//1 01

                            startIndex = newfilename.IndexOf(temp + output + temp);//find index
                            if (startIndex != -1)
                                endIndex = startIndex + 7;
                        }

                        //101 format
                        if (newMainSettings.SeasonFormat == 3)
                        {
                            output = i.ToString() + newj;

                            newfilename = newfilename.Replace(temp + newi + newj + temp, temp + output + temp);//0101
                            newfilename = newfilename.Replace(temp + i.ToString() + "x" + newj + temp, temp + output + temp);//1x01
                            newfilename = newfilename.Replace("s" + newi + "e" + newj + temp, output + temp);//s01e01
                            newfilename = newfilename.Replace("s" + newi + " e" + newj + temp, output + temp);//s01 e01
                            newfilename = newfilename.Replace("s" + i.ToString() + "e" + j.ToString() + temp, output + temp);//s1e1
                            newfilename = newfilename.Replace("s" + i.ToString() + "e" + newj + temp, output + temp);//s1e01
                            newfilename = newfilename.Replace("season " + i.ToString() + " episode " + newj + temp, output + temp);//season 1 episode 01
                            newfilename = newfilename.Replace("season " + i.ToString() + " episode " + j.ToString() + temp, output + temp);//season 1 episode 1
                            newfilename = newfilename.Replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);// 1 01 

                            startIndex = newfilename.IndexOf(temp + output + temp);//find index
                            if (startIndex != -1)
                            {
                                if (i > 9) { endIndex = startIndex + 5; } else { endIndex = startIndex + 4; }
                                //endIndex = startIndex + 3;
                            }
                        }

                        //stop loop when name is change                    
                        if (startIndex != -1)
                        {
                            EditFileList[index].SeasonNum = i + newMainSettings.SeasonOffset;
                            formattedSeason = newi2;
                            EditFileList[index].EpisodeNum = j + newMainSettings.EpisodeOffset;
                            formattedEpisode = newj2;
                            end = true;
                            break;
                        }

                    }//end of episode loop

                    //stop loop when name is change
                    if (end)
                        break;
                    if (i == 0) i = 40;
                }//end of season loop 
                #endregion

                #region DateFormat
                //Date format
                if (newMainSettings.SeasonFormat == 4)
                {
                    for (int year = 0; year < 20; year++)
                    {
                        bool end = false;
                        for (int month = 12; month > 0; month--)
                        {
                            for (int day = 31; day > 0; day--)
                            {
                                //string startnewname = newfilename;
                                string newyear = year.ToString();
                                string newmonth = month.ToString();
                                string newday = day.ToString();

                                //check if i is less than 10
                                if (year < 10)
                                    newyear = "0" + year.ToString();
                                //check if j is less than 10
                                if (month < 10)
                                    newmonth = "0" + month.ToString();
                                //check if k is less than 10
                                if (day < 10)
                                    newday = "0" + day.ToString();
                                string kk = "20" + newyear;

                                newfilename = newfilename.Replace(kk + " " + month + " " + day, month.ToString() + "-" + day.ToString() + "-" + kk);
                                newfilename = newfilename.Replace(kk + " " + newmonth + " " + newday, month.ToString() + "-" + day.ToString() + "-" + kk);

                                startIndex = newfilename.IndexOf(month + "-" + day + "-" + kk);//find index
                                if (startIndex != -1)
                                {
                                    if (day > 9 && month > 9)
                                        endIndex = startIndex + 10;
                                    else if (day > 9 || month > 9)
                                        endIndex = startIndex + 9;
                                    else
                                        endIndex = startIndex + 8;

                                    EditFileList[index].SeasonNum = (month * 100) + day;
                                    formattedSeason = month.ToString() + "-" + day.ToString();
                                    formattedEpisode = kk;
                                    EditFileList[index].EpisodeNum = Int32.Parse(kk);
                                    end = true;
                                    break;
                                }
                            }//end of for loop day
                            if (end)
                                break;
                        }//end of for loop month
                        if (end)
                            break;
                    }//end of for loop year
                }//end of if for date check box 
                #endregion

                if (startIndex == -1)
                {
                    startIndex = newfilename.Length - 5;
                    EditFileList[index].GetTitle = false;
                    addSeasonFormat = false;
                }

                tvshowName = newfilename.Substring(0, startIndex).Trim();

                EditFileList[index].TVShowID = SearchTVShowName(tvshowName);

                bool useOldTiltle = true;
                if (newMainSettings.GetTVShowName)
                {
                    if (EditFileList[index].TVShowID != -1)
                    {
                        string newTvshowName = TVShowInfoList[EditFileList[index].TVShowID].RealTVShowName;
                        if (newTvshowName != "")
                        {
                            tvshowName = newTvshowName;
                            useOldTiltle = false;
                        }
                    }
                }

                if (useOldTiltle)
                {
                    switch (newMainSettings.ProgramFormat)
                    {
                        case 0:
                            tvshowName = UpperCaseingAfterSpace(tvshowName, 0, tvshowName.Length - 1);
                            break;
                        case 1:
                            tvshowName = UpperCaseing(tvshowName, 0, 1);
                            break;
                        case 2:
                            tvshowName = UpperCaseing(tvshowName, 0, tvshowName.Length);
                            break;
                        default:
                            break;
                    }
                }

                if (newMainSettings.TitleFormat != 5 && EditFileList[index].GetTitle)
                {
                    switch (newMainSettings.TitleSelection)
                    {
                        case 0:
                            if (!secondTime)
                            {
                                showTitle = "";
                                if (endIndex != newfilename.Length - 5)
                                    showTitle = newfilename.Substring(endIndex, newfilename.Length - (endIndex + 5)).Trim();
                                if (showTitle != "")
                                    EditFileList[index].GetTitle = false;
                            }
                            break;
                        case 1:
                            EditFileList[index].GetTitle = false;
                            showTitle = "";
                            if (endIndex != newfilename.Length - 5)
                                showTitle = newfilename.Substring(endIndex, newfilename.Length - (endIndex + 5)).Trim();
                            break;
                        case 2:

                            break;
                        default:
                            break;
                    }

                    switch (newMainSettings.TitleFormat)
                    {
                        case 0:
                            showTitle = UpperCaseingAfterSpace(showTitle, 0, showTitle.Length - 1);
                            break;
                        case 1:
                            showTitle = showTitle.ToLower();// = lowering(showTitle);
                            showTitle = UpperCaseingAfterSpace(showTitle, 0, showTitle.Length - 1);
                            break;
                        case 2:
                            showTitle = showTitle.ToLower();
                            showTitle = UpperCaseing(showTitle, 0, 1);
                            break;
                        case 3:
                            showTitle = UpperCaseing(showTitle, 0, showTitle.Length);
                            break;
                        case 4:
                            showTitle = showTitle.ToLower();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    EditFileList[index].FileTitle = showTitle = "";
                    EditFileList[index].GetTitle = false;
                }

                switch (newMainSettings.ExtFormat)
                {
                    case 0:
                        extend = extend.ToLower();
                        break;
                    case 1:
                        StringBuilder ext1 = new StringBuilder(extend);
                        ext1[1] = char.ToUpper(ext1[1]);
                        extend = ext1.ToString();
                        break;
                    case 2:
                        extend = extend.ToUpper();
                        break;
                    default:
                        break;
                }

                tvshowName = tvshowName.Replace("Vs", "vs");
                tvshowName = tvshowName.Replace("O C ", "O.C. ");
                tvshowName = tvshowName.Replace("T O ", "T.O. ");
                tvshowName = tvshowName.Replace("Csi", "CSI");
                tvshowName = tvshowName.Replace("Wwii", "WWII");
                tvshowName = tvshowName.Replace("Hd", "HD");
                tvshowName = tvshowName.Replace("Tosh 0", "Tosh.0");
                tvshowName = tvshowName.Replace("O Brien", "O'Brien");
                tvshowName = tvshowName.Replace("Nbc", "NBC");
                tvshowName = tvshowName.Replace("Abc", "ABC");
                tvshowName = tvshowName.Replace("Cbs", "CBS");
                tvshowName = tvshowName.Replace("Iv" + temp, "IV" + temp);
                tvshowName = tvshowName.Replace("Ix" + temp, "IX" + temp);
                tvshowName = tvshowName.Replace("Viii", "VIII");
                tvshowName = tvshowName.Replace("Vii", "VII");
                tvshowName = tvshowName.Replace("Vi" + temp, "VI" + temp);
                tvshowName = tvshowName.Replace("Xi" + temp, "XI" + temp);
                tvshowName = tvshowName.Replace("Xii" + temp, "XII" + temp);
                tvshowName = tvshowName.Replace("Xiii" + temp, "XIII" + temp);
                tvshowName = tvshowName.Replace("Xiiii" + temp, "XIIII" + temp);
                tvshowName = tvshowName.Replace("Iii", "III");
                tvshowName = tvshowName.Replace("Ii", "II");
                tvshowName = tvshowName.Replace("X Files", "X-Files");
                tvshowName = tvshowName.Replace("La ", "LA ");
                tvshowName = tvshowName.Replace("Nba", "NBA");
                tvshowName = tvshowName.Replace("Espn", "ESPN");

                if (addSeasonFormat && newMainSettings.SeasonFormat != 5)
                {
                    //add file extention back on 
                    switch (newMainSettings.SeasonFormat)
                    {
                        case 0:
                            finalShowName = tvshowName + temp + seasonDash + EditFileList[index].SeasonNum.ToString() + "x" + formattedEpisode + temp + titleDash + showTitle + extend;
                            break;
                        case 1:
                            finalShowName = tvshowName + temp + seasonDash + formattedSeason + formattedEpisode + temp + titleDash + showTitle + extend;
                            break;
                        case 2:
                            finalShowName = tvshowName + temp + seasonDash + "S" + formattedSeason + "E" + formattedEpisode + temp + titleDash + showTitle + extend;
                            break;
                        case 3:
                            finalShowName = tvshowName + temp + seasonDash + EditFileList[index].SeasonNum.ToString() + formattedEpisode + temp + titleDash + showTitle + extend;
                            break;
                        case 4:
                            finalShowName = tvshowName + temp + seasonDash + formattedSeason + "-" + formattedEpisode + temp + titleDash + showTitle + extend;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    finalShowName = tvshowName + extend;
                }
                //newfilename = newfilename.Replace(temp + "&&&&", extend);

                //Random fixes
                finalShowName = finalShowName.Replace("..", ".");
                finalShowName = finalShowName.Replace(" .", ".");
                finalShowName = finalShowName.Replace("- -", "-");
                finalShowName = finalShowName.Replace(".-.", ".");
                finalShowName = finalShowName.Replace("-.", ".");
                finalShowName = finalShowName.Replace(" .", ".");

                //finalShowName = finalShowName.Replace("Web Dl", "WEB-DL");

                EditFileList[index].FileTitle = showTitle;
                EditFileList[index].TVShowName = tvshowName;
                EditFileList[index].NewFileName = finalShowName;
            }
            // return converted file name
            return true;
        }//end of file rename function

        //This XmlWrite method creates a new XML File
        private void XmlWrite()
        {
            //get size of library file
            StreamReader tr = new StreamReader(newMainSettings.DataFolder + "//library.seh");
            int size = Int32.Parse(tr.ReadLine());//read number of lines
            tr.Close();//close reader stream

            XmlTextWriter xmlWriter = new XmlTextWriter(newMainSettings.DataFolder + "//version.xml", null);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("version");

            xmlWriter.WriteStartElement("application", "");
            xmlWriter.WriteString(appVersion.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("library", "");
            xmlWriter.WriteString(size.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("settings", "");
            xmlWriter.WriteString("1");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();

            // close writer
            xmlWriter.Close();
        }//end of XmlWrite methoid

        //check to see if there are older files        
        private void fileChecker()
        {
            if (!File.Exists(newMainSettings.DataFolder + "//version.xml"))
                this.XmlWrite();
            else
            {
                //check to see if version.xml is the newest
                string document = newMainSettings.DataFolder + "//version.xml";
                int fileVer = 10000000;
                int libVer = 10000000;
                XmlDataDocument myxmlDocument = new XmlDataDocument();
                myxmlDocument.Load(document);
                XmlTextReader xmlReader = new XmlTextReader(document);

                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                if (xmlReader.Name == "application")
                                    fileVer = Convert.ToInt32(xmlReader.ReadString());
                                if (xmlReader.Name == "library")
                                    libVer = Convert.ToInt32(xmlReader.ReadString());
                                break;
                            }//end of case 
                    }//end of switch
                }//end of while loop
                myxmlDocument.RemoveAll();
                xmlReader.Close();
                //if apps info is newer write new file                
                if (appVersion > fileVer)
                    this.XmlWrite();
                //if library is bigger rewrite info file
                StreamReader tr2 = new StreamReader(newMainSettings.DataFolder + "//library.seh");
                int size2 = Int32.Parse(tr2.ReadLine());//read number of lines
                tr2.Close();//close reader stream
                if (size2 > libVer)
                    this.XmlWrite();
            }//end of if-else
        }

        private void extr_Extracting(object sender, ProgressEventArgs e)
        {
            int progress = e.PercentDone;
            if (progress < progressBar1.Maximum)
            {
                MethodInvoker action = delegate
                {
                    progressBar1.Value = progress;
                };
                progressBar1.BeginInvoke(action);
                //progressBar1.Value = progress;
            }
            if (progress == progressBar1.Maximum)
            {
                MethodInvoker action = delegate
                {
                    progressBar1.Hide();
                };
                progressBar1.BeginInvoke(action);
                //progressBar1.Hide();                
            }
        }

        //Move All Files
        private void moveAllFiles(string Outputfolder)
        {
            List<int> Selected = new List<int>();
            List<string> folders = new List<string>();
            using (FileOperation fileOp = new FileOperation(new MyProgressSink(Selected, folders, fileList), this))
            {
                for (int z = 0; z < fileList.Count; z++)
                {
                    string fullFileName = fileList[z].FullFileName;
                    Selected.Add(z);
                    folders.Add(Outputfolder);
                    fileOp.MoveItem(fullFileName, Outputfolder, fileList[z].FileName);
                    Log.WriteLog(fullFileName + " Moved to " + Outputfolder);
                    //clear stuff                    
                    //fileList[z].FileFolder = (Outputfolder);
                }
                try
                {
                    fileOp.PerformOperations();
                }
                catch (Exception t)
                {
                    MessageBox.Show("Error with Operation\n" + t.ToString());
                    Log.WriteLog(t.ToString());
                }
            }
        }

        //Move Selected Files
        private void moveSelectedFiles(string outputFolder)
        {
            List<int> Selected = new List<int>();
            List<string> folders = new List<string>();
            using (FileOperation fileOp = new FileOperation(new MyProgressSink(Selected, folders, fileList), this))
            {
                for (int z = 0; z < dataGridView1.Rows.Count; z++)
                {
                    if (dataGridView1.Rows[z].Selected)
                    {
                        string fullFileName = fileList[z].FullFileName;
                        Selected.Add(z);
                        folders.Add(outputFolder);
                        fileOp.MoveItem(fullFileName, outputFolder, fileList[z].FileName);
                        Log.WriteLog(fullFileName + " Moved to " + outputFolder);
                        //clear stuff                    
                        //fileList[z].FileFolder = (outputFolder);
                    }
                }
                try
                {
                    fileOp.PerformOperations();
                }
                catch (Exception t)
                {
                    MessageBox.Show("Error with Operation\n" + t.ToString());
                    Log.WriteLog(t.ToString());
                }
            }            
        }

        //Copy All Files
        private void copyAllFiles(string Outputfolder)
        {
            using (FileOperation fileOp = new FileOperation(new MyProgressSink(), this))
            {
                for (int z = 0; z < fileList.Count; z++)
                {
                    string fullFileName = fileList[z].FullFileName;
                    fileOp.CopyItem(fullFileName, Outputfolder, fileList[z].FileName);
                    Log.WriteLog(fullFileName + " Copied to " + Outputfolder);
                }
                try
                {
                    fileOp.PerformOperations();
                }
                catch (Exception t)
                {
                    MessageBox.Show("Error with Operation\n" + t.ToString());
                    Log.WriteLog(t.ToString());
                }
            }
        }

        //Copy Selected Files
        private void copySelectedFiles(string outputFolder)
        {
            using (FileOperation fileOp = new FileOperation(new MyProgressSink(), this))
            {
                for (int z = 0; z < dataGridView1.Rows.Count; z++)
                {
                    if (dataGridView1.Rows[z].Selected)
                    {
                        string fullFileName = fileList[z].FullFileName;
                        fileOp.CopyItem(fullFileName, outputFolder, fileList[z].FileName);
                        Log.WriteLog(fullFileName + " Copied to " + outputFolder);
                    }
                }
                try
                {
                    fileOp.PerformOperations();
                }
                catch (Exception t)
                {
                    MessageBox.Show("Error with Operation\n" + t.ToString());
                    Log.WriteLog(t.ToString());
                }
            }
        }

        //detete Selected Titles
        private void deleteSelectedTitles()
        {
            if (dataGridView1.CurrentRow != null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected)
                    {
                        if (fileList[i].FileTitle == "")
                            continue;
                        fileList[i].FileTitle = "";
                    }
                }
                autoConvert();
            }
        }

        //detete Selected Files
        private void deleteSelectedFiles()
        {
            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                if (dataGridView1.Rows[i].Selected)
                    fileList.RemoveAt(i);
            }
            dataGridView1.Refresh();
        }

        //new way to add files from folder
        private void ProcessDir(string sourceDir, int recursionLvl)
        {
            if (recursionLvl <= HowDeepToScan && !newMainSettings.FormClosed)
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
                    //zip fix
                    if ((exten == ".zip" || exten == ".rar" || exten == ".r01" || exten == ".7z") && newMainSettings.OpenZIPs)
                        continue;
                    //check if its a legal file type
                    if (!(exten == ".avi" || exten == ".mkv" || exten == ".mp4" || exten == ".mpg" || exten == ".m4v" || exten == ".mpeg" || exten == ".mov" || exten == ".rm" || exten == ".rmvb" || exten == ".wmv" || exten == ".webm"))
                    {
                        //if dialog was shown b4 dont show again
                        if (!newMainSettings.Shownb4)
                        {
                            if (MessageBox.Show("You have selected a folder with files that aren't Media Files\nWould you like to add them?", "Media Options", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                //state if files should be added
                                newMainSettings.Shownb4 = true;
                                newMainSettings.AddFiles = true;
                            }
                            else
                            {
                                //state if files should not be added
                                newMainSettings.Shownb4 = true;
                                newMainSettings.AddFiles = false;
                                continue;
                            }
                        }
                        //dont add files if not true
                        if (!newMainSettings.AddFiles)
                            continue;
                    }
                    MethodInvoker action = delegate
                    {
                        fileList.Add(new TVClass(fi.DirectoryName, origName, exten));
                        //autoConvert();
                    };
                    dataGridView1.BeginInvoke(action);
                }

                // Recurse into subdirectories of this directory.
                string[] subdirEntries = Directory.GetDirectories(sourceDir);
                foreach (string subdir in subdirEntries)
                {
                    if (subdir.Contains("Program Files") || subdir.Contains("Program Files (x86)") || subdir.Contains("$Recycle.Bin") || subdir.Contains("$RECYCLE.BIN"))
                    {
                        continue;
                    }
                    // Do not iterate through reparse points
                    if ((File.GetAttributes(subdir) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                    {
                        ProcessDir(subdir, recursionLvl + 1);
                    }
                }
            }
        }

        //extract zips and rar in folder
        private void ProcessDirZIP(string sourceDir)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sourceDir);
            // Process the list of files found in the directory.
            foreach (System.IO.FileInfo fi in di.GetFiles("*"))
            {
                string origName = fi.Name;
                string exten = fi.Extension;
                string attrib = fi.Attributes.ToString();

                //if thumb file dont convert
                if (origName == "Thumbs.db")
                    continue;
                //check if its a legal file type
                if (exten == ".zip" || exten == ".rar" || exten == ".r01" || exten == ".7z")
                    archiveExtrector(fi.FullName, fi.Name, false);
            }
        }

        //junk remover
        private void junkRemover()
        {
            List<string> startlist = new List<string>();

            // make array in here
            startlist.Add("dvdscr");
            startlist.Add("rerip");
            startlist.Add("2sd");
            startlist.Add("tvrip");
            startlist.Add("shotv");
            startlist.Add("thewretched");
            startlist.Add("xvid");
            startlist.Add("tvep");
            startlist.Add("hdtv");
            startlist.Add("notv");
            startlist.Add("dvdrip");
            startlist.Add("topaz");
            startlist.Add("saphire");
            startlist.Add("fqm");
            startlist.Add("vtv");
            startlist.Add("eztv");
            startlist.Add("lol");
            startlist.Add("dot");
            startlist.Add("xor");
            startlist.Add("fov");
            startlist.Add("hiqt");
            startlist.Add("filew");
            startlist.Add("arez");
            startlist.Add("pdtv");
            startlist.Add("2hd");
            startlist.Add("0tv");
            startlist.Add("sdtv");
            startlist.Add("lmao");
            startlist.Add("sys");
            startlist.Add("omicron");
            startlist.Add("miraget");
            startlist.Add("dsrip");
            startlist.Add("dsr");
            startlist.Add("dvsky");
            startlist.Add("proper");
            startlist.Add("the real deal");
            startlist.Add("repack");
            startlist.Add("ac3");
            startlist.Add("mvgroup.org");
            startlist.Add("mvgroup org");
            startlist.Add("hidef");
            startlist.Add(" ws");
            startlist.Add("saints");
            startlist.Add("uds");
            startlist.Add("uncut");
            startlist.Add("tow");
            startlist.Add("sfm");
            startlist.Add("xii");
            startlist.Add("p0w4");
            startlist.Add("crimsoni");
            startlist.Add("crimson");
            startlist.Add("cac");
            startlist.Add("clarkadamc");
            startlist.Add("bia");
            startlist.Add("dvsky");
            startlist.Add("notseen");
            startlist.Add("chgrp");
            startlist.Add("iht");
            startlist.Add("lmao");
            startlist.Add("aaf");
            startlist.Add("bajskorv");
            startlist.Add("momentum");
            startlist.Add("yestv");
            startlist.Add("qssdivx");
            startlist.Add("mmi");
            startlist.Add("rdf");
            startlist.Add("dcp");
            startlist.Add("dgas");
            startlist.Add("nogrp");
            startlist.Add("ghgrp");
            startlist.Add("aero");
            startlist.Add("latebyte");
            startlist.Add("tcm");
            startlist.Add("loki");
            startlist.Add("mrtwig");
            startlist.Add(" net");
            startlist.Add("umd");
            startlist.Add("xoxo");
            startlist.Add("medieval");
            startlist.Add("webrip");
            startlist.Add("bdrip");
            startlist.Add("agd");
            startlist.Add("gnarly");
            startlist.Add("krs");
            startlist.Add("goat");
            startlist.Add("bucktv");
            startlist.Add("buck");
            startlist.Add("orpheus");
            startlist.Add("720p");
            startlist.Add("x264");
            startlist.Add("dimension");
            startlist.Add("60fps");
            startlist.Add("d734");
            startlist.Add("mspaint");
            startlist.Add("siso");
            startlist.Add("reward");
            startlist.Add("qcf");
            startlist.Add("p2p");
            startlist.Add("h264");
            startlist.Add("vodo");
            startlist.Add("wrcr");
            startlist.Add("brrip");
            startlist.Add("fever");
            startlist.Add("twiz");
            startlist.Add("diverge");
            startlist.Add("www.directlinkspot.com");
            startlist.Add("www directlinkspot com");
            startlist.Add("onelinkmoviez.com");
            startlist.Add("onelinkmoviez com");
            startlist.Add("asap");
            startlist.Add("dd5.1");
            startlist.Add("dd5 1");
            startlist.Add("web-dl");
            startlist.Add("web dl");
            startlist.Add("web.dl");
            startlist.Add("icrap");

            //startlist.Add("tv");

            //if no file exist make a default file
            if (!File.Exists(newMainSettings.DataFolder + "//library.seh"))
            {
                StreamWriter sw = new StreamWriter(newMainSettings.DataFolder + "//library.seh");
                sw.WriteLine(startlist.Count());
                for (int j = 0; j < startlist.Count(); j++)
                    sw.WriteLine(startlist[j]);
                sw.Close();//close writer stream
            }
            else
            {//check to see if this is the newest file
                StreamReader tr2 = new StreamReader(newMainSettings.DataFolder + "//library.seh");
                int size2 = Int32.Parse(tr2.ReadLine());//read number of lines
                tr2.Close();//close reader stream
                if (startlist.Count() > size2)
                {//replace if array is bigger than file
                    StreamWriter sw = new StreamWriter(newMainSettings.DataFolder + "//library.seh");
                    sw.WriteLine(startlist.Count());
                    for (int j = 0; j < startlist.Count(); j++)
                        sw.WriteLine(startlist[j]);
                    sw.Close();//close writer stream
                }//end of if
            }//end of if

            //read junk file 
            StreamReader tr = new StreamReader(newMainSettings.DataFolder + "//library.seh");

            junklist.Clear();//clear old list

            int size = Int32.Parse(tr.ReadLine());//read number of lines

            for (int i = 0; i < size; i++)
                junklist.Add(tr.ReadLine());

            tr.Close();//close reader stream

        }//end of junk remover

        //private bool MoveFile(string oldLocation, string newLocation)
        //{
        //    if (oldLocation == newLocation) return true;
        //    try
        //    {
        //        FileSystem.MoveFile(oldLocation, newLocation, UIOption.AllDialogs);
        //        Log.WriteLog(oldLocation + " Moved to " + newLocation);
        //    }
        //    catch (FileNotFoundException r)
        //    {
        //        MessageBox.Show("File have been changed or moved \n" + oldLocation);
        //        Log.WriteLog(r.ToString());
        //        return false;
        //    }
        //    catch (IOException g)
        //    {
        //        MessageBox.Show("File already exists or is in use\n" + oldLocation);
        //        Log.WriteLog(g.ToString());
        //        return false;
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        return false;
        //    }
        //    catch (Exception t)
        //    {
        //        MessageBox.Show("Error with Operation\n" + t.ToString());
        //        Log.WriteLog(t.ToString());
        //        return false;
        //    }
        //    return true;
        //}

        //private bool CopyFile(string oldLocation, string newLocation)
        //{
        //    try
        //    {
        //        FileSystem.CopyFile(oldLocation, newLocation, UIOption.AllDialogs);
        //        Log.WriteLog(oldLocation + " Coped to " + newLocation);
        //    }
        //    catch (FileNotFoundException r)
        //    {
        //        MessageBox.Show("File have been changed or moved \n" + oldLocation);
        //        Log.WriteLog(r.ToString());
        //        return false;
        //    }
        //    catch (IOException g)
        //    {
        //        MessageBox.Show("File already exists or is in use\n" + oldLocation);
        //        Log.WriteLog(g.ToString());
        //        return false;
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        return false;
        //    }
        //    catch (Exception t)
        //    {
        //        MessageBox.Show("Error with Operation\n" + t.ToString());
        //        Log.WriteLog(t.ToString());
        //        return false;
        //    }
        //    return true;
        //}

        /// <summary>
        /// extract contents of zip/rar file
        /// </summary>
        /// <param name="zipfile">full file path</param>
        /// <param name="zipName">just file name</param>
        /// <param name="add">add file to data grid</param>
        private void archiveExtrector(string zipfile, string zipName, bool add)
        {
            List<string> info = new List<string>();
            string archiveName = null;
            int archiveIndex = -1;
            FileInfo fi8 = new FileInfo(zipfile);
            SevenZipExtractor mainExtrector;
            try
            {
                mainExtrector = new SevenZipExtractor(zipfile);
            }
            catch (SevenZipArchiveException)
            {
                password passwordYou = new password(zipfile, zipName);

                if (passwordYou.ShowDialog() == DialogResult.OK)
                {
                    this.archiveExtrector(zipfile, zipName, passwordYou.Password, add);
                    passwordYou.Close();
                }
                return;
            }
            catch (SevenZipLibraryException)
            {
                MessageBox.Show("Incorrect 7z.dll for your version of Windows");
                return;
            }
            int sizeOfArchive = 0;
            try
            {
                sizeOfArchive = (int)mainExtrector.FilesCount;
            }
            catch (SevenZipArchiveException)
            {
                //Call Passord Method
                password passwordYou = new password(zipfile, zipName);
                if (passwordYou.ShowDialog() == DialogResult.OK)
                {
                    this.archiveExtrector(zipfile, zipName, passwordYou.Password, add);
                    passwordYou.Close();
                }
                return;
            }

            MethodInvoker action2 = delegate
            {
                progressBar1.Maximum = 100;
                progressBar1.Value = 0;
                progressBar1.Show();
            };
            progressBar1.BeginInvoke(action2);
            mainExtrector.Extracting += extr_Extracting;

            for (int j = 0; j < sizeOfArchive; j++)
            {
                archiveName = mainExtrector.ArchiveFileNames[j];
                string testArchiveName = archiveName.Replace(".avi", "0000").Replace(".mkv", "0000").Replace(".mp4", "0000").Replace(".m4v", "0000").Replace(".mpg", "0000").Replace(".mpeg", "0000").Replace(".mov", "0000").Replace(".rm", "0000").Replace(".rmvb", "0000").Replace(".wmv", "0000").Replace(".webm", "0000");

                if (testArchiveName != archiveName)
                {
                    archiveIndex = j;
                    break;
                }
            }
            if (archiveIndex == -1)
            {
                MethodInvoker action3 = delegate
                {
                    progressBar1.Hide();
                };
                progressBar1.BeginInvoke(action3);
                return;
            }
            try
            {
                mainExtrector.ExtractFile(archiveIndex, File.Create(fi8.DirectoryName + "\\" + archiveName));
                if (add)
                {
                    FileInfo fi9 = new FileInfo(fi8.DirectoryName + "\\" + archiveName);
                    for (int i = 0; i < fileList.Count(); i++)
                    {
                        if (fi9.Name == fileList[i].FileName)
                        {
                            MethodInvoker action3 = delegate
                            {
                                progressBar1.Hide();
                            };
                            progressBar1.BeginInvoke(action3);
                            return;
                        }
                    }
                    //add file names
                    MethodInvoker action = delegate
                    {
                        fileList.Add(new TVClass(fi9.DirectoryName, fi9.Name, fi9.Extension));
                    };
                    dataGridView1.BeginInvoke(action);
                }
            }
            catch (SevenZipArchiveException)
            {
                //Call Passord Method
                password passwordYou = new password(zipfile, zipName);
                if (passwordYou.ShowDialog() == DialogResult.OK)
                {
                    this.archiveExtrector(zipfile, zipName, passwordYou.Password, add);
                    passwordYou.Close();
                }
                MethodInvoker action3 = delegate
                {
                    progressBar1.Hide();
                };
                progressBar1.BeginInvoke(action3);
                return;
            }
            catch (FileNotFoundException)
            {
                MethodInvoker action3 = delegate
                {
                    progressBar1.Hide();
                };
                progressBar1.BeginInvoke(action3);
                return;
            }
            catch (IOException)
            {
                MethodInvoker action3 = delegate
                {
                    progressBar1.Hide();
                };
                progressBar1.BeginInvoke(action3);
                return;
            }
            autoConvert();
            mainExtrector.Extracting -= extr_Extracting;
            mainExtrector.Dispose();
            MethodInvoker action4 = delegate
            {
                progressBar1.Hide();
            };
            progressBar1.BeginInvoke(action4);
        }

        /// <summary>
        /// extract contents of zip/rar file with Passord
        /// </summary>
        /// <param name="zipfile">full file path</param>
        /// <param name="zipName">just file name</param>
        /// <param name="password">file name password</param>
        /// <param name="add">add file to data grid</param>
        private void archiveExtrector(string zipfile, string zipName, string password, bool add)
        {
            List<string> info = new List<string>();
            string archiveName = null;
            int archiveIndex = -1;
            FileInfo fi8 = new FileInfo(zipfile);
            SevenZipExtractor mainExtrector;
            if (password == "") return;
            try
            {
                mainExtrector = new SevenZipExtractor(zipfile, password);
            }
            catch (SevenZipArchiveException)
            {
                password passwordYou = new password(zipfile, zipName);

                if (passwordYou.ShowDialog() == DialogResult.OK)
                {
                    this.archiveExtrector(zipfile, zipName, passwordYou.Password, add);
                    passwordYou.Close();
                }
                return;
            }
            catch (SevenZipLibraryException)
            {
                MessageBox.Show("Incorrect 7z.dll for your version of Windows");
                return;
            }

            int sizeOfArchive = 0;
            try
            {
                sizeOfArchive = (int)mainExtrector.FilesCount;
            }
            catch (SevenZipArchiveException)
            {   //add password
                password passwordYou = new password(zipfile, zipName);

                if (passwordYou.ShowDialog() == DialogResult.OK)
                {
                    this.archiveExtrector(zipfile, zipName, passwordYou.Password, add);
                    passwordYou.Close();
                }
                return;
            }

            MethodInvoker action2 = delegate
            {
                progressBar1.Maximum = 100;
                progressBar1.Value = 0;
                progressBar1.Show();

            };
            progressBar1.BeginInvoke(action2);
            mainExtrector.Extracting += extr_Extracting;

            for (int j = 0; j < sizeOfArchive; j++)
            {
                archiveName = mainExtrector.ArchiveFileNames[j];
                string testArchiveName = archiveName.Replace(".avi", "0000").Replace(".mkv", "0000").Replace(".mp4", "0000").Replace(".m4v", "0000").Replace(".mpg", "0000").Replace(".mpeg", "0000").Replace(".mov", "0000").Replace(".rm", "0000").Replace(".rmvb", "0000").Replace(".wmv", "0000").Replace(".webm", "0000");

                if (testArchiveName != archiveName)
                {
                    archiveIndex = j;
                    break;
                }
            }

            if (archiveIndex == -1)
            {
                MethodInvoker action3 = delegate
                {
                    progressBar1.Hide();
                };
                progressBar1.BeginInvoke(action3);
                return;
            }

            try
            {
                mainExtrector.ExtractFile(archiveIndex, File.Create(fi8.DirectoryName + "\\" + archiveName));
                if (add)
                {
                    FileInfo fi9 = new FileInfo(fi8.DirectoryName + "\\" + archiveName);
                    for (int i = 0; i < fileList.Count(); i++)
                    {
                        if (fi9.Name == fileList[i].FileName)
                        {
                            MethodInvoker action3 = delegate
                            {
                                progressBar1.Hide();
                            };
                            progressBar1.BeginInvoke(action3);
                            return;
                        }
                    }
                    //add file name
                    MethodInvoker action = delegate
                    {
                        fileList.Add(new TVClass(fi9.DirectoryName, fi9.Name, fi9.Extension));
                    };
                    dataGridView1.BeginInvoke(action);
                }
            }
            catch (SevenZipArchiveException)
            {
                //Call Passord Method
                password passwordYou = new password(zipfile, zipName);
                if (passwordYou.ShowDialog() == DialogResult.OK)
                {
                    this.archiveExtrector(zipfile, zipName, passwordYou.Password, add);
                    passwordYou.Close();
                }
                MethodInvoker action3 = delegate
                {
                    progressBar1.Hide();
                };
                progressBar1.BeginInvoke(action3);
                return;
            }
            catch (FileNotFoundException)
            {
                MethodInvoker action3 = delegate
                {
                    progressBar1.Hide();
                };
                progressBar1.BeginInvoke(action3);
                return;
            }
            catch (IOException)
            {
                MethodInvoker action3 = delegate
                {
                    progressBar1.Hide();
                };
                progressBar1.BeginInvoke(action3);
                return;
            }
            autoConvert();
            mainExtrector.Extracting -= extr_Extracting;
            mainExtrector.Dispose();
            MethodInvoker action4 = delegate
            {
                progressBar1.Hide();
            };
            progressBar1.BeginInvoke(action4);
        }

        //drag and drop
        private void dragTo_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        //drag and drop
        private void dragTo_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                ThreadAdd FilesToAdd = new ThreadAdd();
                FilesToAdd.AddType = "files";
                FilesToAdd.ObjectToAdd = files;
                addFilesToThread(FilesToAdd);
                //getFiles(files);
            }
        }

        #endregion

        #endregion

        #region Right click menus

        //save selected file
        private void saveNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                for (int u = 0; u < dataGridView1.Rows.Count; u++)
                {
                    if (dataGridView1.Rows[u].Selected)
                    {
                        try
                        {
                            //FileSystem.MoveFile((fileList[u].FullFileName), (fileList[u].NewFullFileName), true);
                            System.IO.File.Move((fileList[u].FullFileName), (fileList[u].NewFullFileName));
                            Log.WriteLog(fileList[u].FullFileName, fileList[u].NewFullFileName);
                            fileList[u].FileName = fileList[u].NewFileName;
                            fileList[u].FileTitle = "";
                            dataGridView1.Rows[u].Cells[0].Value = fileList[u].FileName;
                        }
                        catch (FileNotFoundException)
                        {
                            MessageBox.Show("File have been changed or moved \n" + (fileList[u].FullFileName) + "\n" + (fileList[u].NewFullFileName));
                        }
                        catch (IOException)
                        {
                            MessageBox.Show("File already exists or is in use\n" + (fileList[u].FullFileName) + "\n" + (fileList[u].NewFullFileName));
                        }
                    }
                    newMainSettings.SeasonOffset = 0;
                    newMainSettings.EpisodeOffset = 0;

                    autoConvert();
                }
            }
        }

        //remove selected file
        private void removeSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                deleteSelectedFiles();
            }
        }

        //view folder of selected file
        private void viewFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string selectedFiles = "";

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected)
                    {
                        selectedFiles = fileList[i].FullFileName;
                        break;
                    }
                }

                System.Diagnostics.Process.Start("explorer.exe", @"/select, " + selectedFiles);
            }
        }

        //right click to get titles off Internet
        private void getTitlesOffIMBDOfSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                if (fileList.Count != 0 && ConnectionExists()) //if files are selected
                {
                    List<int> selected = new List<int>();
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        if ((dataGridView1.Rows[i].Selected) && fileList[i].SeasonNum != -1 && fileList[i].EpisodeNum != -1 && fileList[i].AutoEdit)
                            selected.Add(i);
                    }
                    //TestTitle(selected);
                    if (!TitleThread.IsBusy)
                        TitleThread.RunWorkerAsync(selected);
                }
            }
            else//catch if nothing is selected
                MessageBox.Show("No Files Selected");
        }

        //remove selected titles
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            deleteSelectedTitles();
        }

        //edit selected titles
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected)
                    {
                        if (fileList[i].FileTitle == "")
                            continue;
                        string text = fileList[i].NewFileName;
                        if (InputBox.Show("Edit Episode Title", "Episode Title:", ref text) == DialogResult.OK)
                        {
                            fileList[i].FileTitle = text;
                            fileList[i].GetTitle = false;
                        }
                    }
                }
                autoConvert();
            }
        }

        //edit Pending File Name
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected)
                    {
                        string text = fileList[i].NewFileName;
                        if (InputBox.Show("Edit Pending File Name", "File Name:", ref text) == DialogResult.OK)
                        {
                            fileList[i].NewFileName = text;
                            fileList[i].AutoEdit = false;
                        }
                        dataGridView1.Refresh();
                        dataGridView1.AutoResizeColumns();
                    }
                }
            }
        }

        #endregion

        //load everything on startup
        private void loadEverything()
        {
            if (!(System.IO.Directory.Exists(newMainSettings.DataFolder)))
                System.IO.Directory.CreateDirectory(newMainSettings.DataFolder);
            Log.startLog(newMainSettings.DataFolder);
            newMainSettings.Start(this);

            this.junkRemover();
            this.fileChecker();
            newMainSettings.loadStettings();

            AddBrowserMenu();
            for (int i = 0; i < newMainSettings.MoveFolder.Count(); i = i + 3)
                AddFolder(newMainSettings.MoveFolder[i], newMainSettings.MoveFolder[i + 1], int.Parse(newMainSettings.MoveFolder[i + 2]));

            if (menu1.Count() != 0)
            {
                button1.Text = "Move To " + menu1[0].Text;
                button2.Text = "Copy To " + menu1[0].Text;
            }
            userJunk.junk_adder(junklist, newMainSettings.DataFolder, this);
            textConvert.setUp(this, newMainSettings.DataFolder);
            BackColor = System.Drawing.Color.FromArgb(newMainSettings.BackgroundColor[0], newMainSettings.BackgroundColor[1], newMainSettings.BackgroundColor[2], newMainSettings.BackgroundColor[3]);
            MainMenuStrip.BackColor = BackColor;
            ForeColor = System.Drawing.Color.FromArgb(newMainSettings.ForegroundColor[0], newMainSettings.ForegroundColor[1], newMainSettings.ForegroundColor[2], newMainSettings.ForegroundColor[3]);
            Color temp1 = System.Drawing.Color.FromArgb(newMainSettings.ButtonColor[0], newMainSettings.ButtonColor[1], newMainSettings.ButtonColor[2], newMainSettings.ButtonColor[3]);
            int[] temp3 = { 255, 240, 240, 240 };
            if (temp3[1] != newMainSettings.ButtonColor[1] && temp3[2] != newMainSettings.ButtonColor[2] && temp3[3] != newMainSettings.ButtonColor[3])
                changeButtoncolor(temp1);
            //this.fileChecker();
            if (newMainSettings.CheckForUpdates)
            {
                Thread updateChecker = new Thread(new ThreadStart(checkForUpdateSilent));
                updateChecker.Start();
            }
        }

        //create preference file when program closes and close log
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            newMainSettings.FormClosed = true;
            List<string> menuidemlist = new List<string>();
            foreach (ToolStripMenuItem menuItem in menu1)
            {
                menuidemlist.Add(menuItem.Text.ToString());
                string[] words = menuItem.Tag.ToString().Split('?');
                menuidemlist.Add(words[1]);
                menuidemlist.Add(words[0]);
            }
            newMainSettings.MoveFolder = menuidemlist;
            newMainSettings.saveStettings();

            if (newMainSettings.ClosedForUpdates)
            {
                Log.closeLog();
                return;
            }
            //write log
            Log.closeLog();
        }        
    }//end of form1 partial class    
}//end of namespace
