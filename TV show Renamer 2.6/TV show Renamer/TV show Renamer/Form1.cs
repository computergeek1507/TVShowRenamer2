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
using Microsoft.VisualBasic.FileIO;
using System.Threading;
using SevenZip;

namespace TV_show_Renamer
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
            if (args.GetLength(0) != 0)
                getFiles(args);
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
        }

        #region Initiate Stuff
        //initiate varibles  
        const int appVersion = 275;//2.7Beta
        const int HowDeepToScan = 4;
        

        BindingList<TVClass> fileList = new BindingList<TVClass>();//TV Show list       
        List<string> junklist = new List<string>();//junk word list
        List<string> userjunklist = new List<string>();//user junk word list
        List<string> textConverter = new List<string>();//textConverter word list

        //create other forms
        junk_words userJunk = new junk_words();
        Text_Converter textConvert = new Text_Converter();
        LogWrite Log = new LogWrite();//log object
        public MainSettings newMainSettings = new MainSettings();//new settings object

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
                Thread h = new Thread(delegate() { getFiles(openFileDialog2.FileNames); });
                h.Start();
            }//end of if
        }//end of file button

        //add folder button
        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Thread u = new Thread(delegate() { getFilesInFolder(folderBrowserDialog1.SelectedPath); });
                u.Start();
            }//end of if
        }//end of folder button

        //remove selected
        private void removeSelectedMenuItem_Click(object sender, EventArgs e)
        {
            deleteSelectedFiles();
        }

        //Clear items from display
        private void clearListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileList.Clear();
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
            EditTitle2 mainEdit = new EditTitle2(newMainSettings.FirstWord);
            mainEdit.Text = "Add Text To Begining";
            mainEdit.Location = new Point(this.Location.X + ((this.Size.Width - mainEdit.Size.Width) / 2), this.Location.Y + ((this.Size.Height - mainEdit.Size.Height) / 2));
            if (mainEdit.ShowDialog() == DialogResult.OK)
            {
                newMainSettings.FirstWord = mainEdit.getTitle();
                mainEdit.Close();
                Thread t = new Thread(new ThreadStart(autoConvert));
                t.Start();
            }
            else
            {
                newMainSettings.FirstWord = "";
                Thread t = new Thread(new ThreadStart(autoConvert));
                t.Start();
            }
        }//end of form closing
        
        //default setting method 
        private void defaultSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Would You Like to Restore Default Settings?", "Restore Default Settings", MessageBoxButtons.YesNo) == DialogResult.Yes)
                newMainSettings.defaultSettings();
        }//end of default setting method 

        //check for updates
        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new thread for update
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
                Thread t = new Thread(new ThreadStart(autoConvert));
                t.Start();
            }
        }

        //secret autoEdit reset
        private void secretResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                for (int u = 0; u < dataGridView1.Rows.Count; u++)
                {
                    if (dataGridView1.Rows[u].Cells[0].Selected || dataGridView1.Rows[u].Cells[1].Selected)
                        fileList[u].AutoEdit = true;
                }
                Thread t = new Thread(new ThreadStart(autoConvert));
                t.Start();
            }
        }

        #endregion

        #region On the form Buttons

        //Move Button
        private void button1_Click(object sender, EventArgs e)
        {           
                if (dataGridView1.CurrentRow != null)
                {
                    if (menu1.Count() != 0)
                    {
                        string[] folderSettings = menu1[0].Tag.ToString().Split('?');
                        if (int.Parse(folderSettings[0]) == 1)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                if (MoveFile(fileList[i].FullFileName, (folderSettings[1] + "\\" + fileList[i].FileName)))
                                    fileList[i].FileFolder = folderSettings[1];
                            }
                        }
                        else if (int.Parse(folderSettings[0]) > 1)
                        {
                            List<string> folderlist = folderFinder(folderSettings[1]);
                            List<string> info = new List<string>();
                            for (int z = 0; z < fileList.Count; z++)
                            {
                                string fullFileName = fileList[z].FullFileName;
                                info = infoFinder(fullFileName, fileList[z].FileFolder, folderlist);

                                if (info[0] == "no folder")
                                {
                                    if (MessageBox.Show("There is No Such TV Show in the TV Show Folder, Would you like to Create One?", "Create folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        EditTitle2 mainEdit = new EditTitle2(info[2]);
                                        mainEdit.Text = "Edit Folder Name";
                                        mainEdit.Location = new Point(this.Location.X + ((this.Size.Width - mainEdit.Size.Width) / 2), this.Location.Y + ((this.Size.Height - mainEdit.Size.Height) / 2));
                                        if (mainEdit.ShowDialog() == DialogResult.OK)
                                        {
                                            string methodGet = mainEdit.getTitle();
                                            System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + methodGet);
                                            folderlist.Add(methodGet);
                                            info = infoFinder(fullFileName, fileList[z].FileFolder, folderlist);
                                            info[0] = methodGet;
                                            mainEdit.Close();
                                        }
                                        else
                                        {
                                            if (MoveFile(fullFileName, folderSettings[1] + "\\" + fileList[z].FileName))
                                                fileList[z].FileFolder = (folderSettings[1]);
                                            continue; 
                                        }
                                    }
                                    else
                                    {
                                        if (MoveFile(fullFileName, folderSettings[1] + "\\" + fileList[z].FileName))
                                            fileList[z].FileFolder = (folderSettings[1]);
                                        continue; 
                                    }
                                }

                                if (info[1] != "0" && int.Parse(folderSettings[0]) == 3)
                                {
                                    if (!(System.IO.Directory.Exists(folderSettings[1] + "\\" + info[0] + "\\Season " + info[1])))
                                        System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + info[0] + "\\Season " + info[1]);

                                    if (MoveFile(fullFileName, folderSettings[1] + "\\" + info[0] + "\\Season " + info[1] + "\\" + fileList[z].FileName))
                                        fileList[z].FileFolder = (folderSettings[1] + "\\" + info[0] + "\\Season " + info[1]);
                                }
                                else//if no season is selected 
                                {
                                    if (MoveFile(fullFileName, folderSettings[1] + "\\" + info[0] + "\\" + fileList[z].FileName))
                                        fileList[z].FileFolder = (folderSettings[1] + "\\" + info[0]);
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
                                if (MoveFile(fileList[i].FullFileName, (folderBrowserDialog2.SelectedPath + "\\" + fileList[i].FileName)))
                                    fileList[i].FileFolder = folderBrowserDialog2.SelectedPath;
                            }
                        }
                        else
                            return;                    
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
                if (menu1.Count() != 0)
                {
                    string[] folderSettings = menu1[0].Tag.ToString().Split('?');
                    if (int.Parse(folderSettings[0]) == 1)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            CopyFile(fileList[i].FullFileName, (folderSettings[1] + "\\" + fileList[i].FileName));
                    }
                    else if (int.Parse(folderSettings[0]) > 1)
                    {
                        List<string> folderlist = folderFinder(folderSettings[1]);
                        List<string> info = new List<string>();
                        for (int z = 0; z < fileList.Count; z++)
                        {
                            string fullFileName = fileList[z].FullFileName;
                            info = infoFinder(fullFileName, fileList[z].FileFolder, folderlist);

                            if (info[0] == "no folder")
                            {
                                if (MessageBox.Show("There is No Such TV Show in the TV Show Folder, Would you like to Create One?", "Create folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    EditTitle2 mainEdit = new EditTitle2(info[2]);
                                    mainEdit.Text = "Edit Folder Name";
                                    mainEdit.Location = new Point(this.Location.X + ((this.Size.Width - mainEdit.Size.Width) / 2), this.Location.Y + ((this.Size.Height - mainEdit.Size.Height) / 2));
                                    if (mainEdit.ShowDialog() == DialogResult.OK)
                                    {
                                        string methodGet = mainEdit.getTitle();
                                        System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + methodGet);
                                        folderlist.Add(methodGet);
                                        info = infoFinder(fullFileName, fileList[z].FileFolder, folderlist);
                                        info[0] = methodGet;
                                        mainEdit.Close();
                                    }
                                    else
                                    {
                                        CopyFile(fileList[z].FullFileName, (folderSettings[1] + "\\" + fileList[z].FileName));
                                        continue;
                                    }
                                }
                                else
                                {
                                    CopyFile(fileList[z].FullFileName, (folderSettings[1] + "\\" + fileList[z].FileName));
                                    continue; 
                                }
                            }

                            if (info[1] != "0" && int.Parse(folderSettings[0]) == 3)
                            {
                                if (!(System.IO.Directory.Exists(folderSettings[1] + "\\" + info[0] + "\\Season " + info[1])))
                                    System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + info[0] + "\\Season " + info[1]);
                                CopyFile(fullFileName, folderSettings[1] + "\\" + info[0] + "\\Season " + info[1] + "\\" + fileList[z].FileName);
                            }
                            else//if no season is selected 
                                CopyFile(fullFileName, folderSettings[1] + "\\" + info[0] + "\\" + fileList[z].FileName);
                        }//end of for loop 
                    }
                }
                else//catch if nothing is selected
                {
                    if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            CopyFile(fileList[i].FullFileName, (folderBrowserDialog2.SelectedPath + "\\" + fileList[i].FileName));                      
                    }
                    else
                        return;                
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
                        FileSystem.MoveFile((fileList[y].FullFileName), (fileList[y].NewFullFileName), true);
                        //System.IO.File.Move((fileList[y].FullFileName), (fileList[y].NewFullFileName));
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

                Thread t = new Thread(new ThreadStart(autoConvert));
                t.Start();
            }
            else//catch if nothing is selected
                MessageBox.Show("No Files Selected");

        }//end of save filenames method

        //TVDB
        private void button6_Click(object sender, EventArgs e)
        {
            if (fileList.Count != 0 && ConnectionExists()) //if files are selected
            {
                int format = -1;
                format = newMainSettings.SeasonFormat + 1;
                Thread h = new Thread(delegate() { autoTitleTVDB(format, true); });
                h.Start();
            }
            else//catch if nothing is selected
                MessageBox.Show("No Files Selected");
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
                        clickedItem.Tag = "1?"+folderBrowserDialog2.SelectedPath;
                    else
                        return;
                }
                string[] folderSettings = clickedItem.Tag.ToString().Split('?');
                if (int.Parse(folderSettings[0]) == 1) 
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (MoveFile(fileList[i].FullFileName, (folderSettings[1] + "\\" + fileList[i].FileName)))
                            fileList[i].FileFolder = folderSettings[1];
                    }
                }
                else if (int.Parse(folderSettings[0]) > 1)
                {
                    List<string> folderlist = folderFinder(folderSettings[1]);
                    List<string> info = new List<string>();
                    for (int z = 0; z < fileList.Count; z++)
                    {
                        string fullFileName = fileList[z].FullFileName;
                        info = infoFinder(fullFileName, fileList[z].FileFolder, folderlist);

                        if (info[0] == "no folder")
                        {
                            if (MessageBox.Show("There is No Such TV Show in the TV Show Folder, Would you like to Create One?", "Create folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                EditTitle2 mainEdit = new EditTitle2(info[2]);
                                mainEdit.Text = "Edit Folder Name";
                                mainEdit.Location = new Point(this.Location.X + ((this.Size.Width - mainEdit.Size.Width) / 2), this.Location.Y + ((this.Size.Height - mainEdit.Size.Height) / 2));
                                if (mainEdit.ShowDialog() == DialogResult.OK)
                                {
                                    string methodGet = mainEdit.getTitle();
                                    System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + methodGet);
                                    folderlist.Add(methodGet);
                                    info = infoFinder(fullFileName, fileList[z].FileFolder, folderlist);
                                    info[0] = methodGet;
                                    mainEdit.Close();
                                }
                                else
                                {
                                    if(MoveFile(fullFileName,folderSettings[1] + "\\" + fileList[z].FileName))
                                        fileList[z].FileFolder = (folderSettings[1]);
                                    continue; 
                                }
                            }
                            else
                            {
                                if (MoveFile(fullFileName, folderSettings[1] + "\\" + fileList[z].FileName))
                                    fileList[z].FileFolder = (folderSettings[1]);
                                continue; 
                            }
                        }

                        if (info[1] != "0" && int.Parse(folderSettings[0]) == 3)
                        {
                            if (!(System.IO.Directory.Exists(folderSettings[1] + "\\" + info[0] + "\\Season " + info[1])))
                                System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + info[0] + "\\Season " + info[1]);

                            if(MoveFile(fullFileName, folderSettings[1] + "\\" + info[0] + "\\Season " + info[1] + "\\" + fileList[z].FileName))
                                fileList[z].FileFolder = (folderSettings[1] + "\\" + info[0] + "\\Season " + info[1]);                           
                        }
                        else//if no season is selected 
                        {
                            if (MoveFile(fullFileName, folderSettings[1] + "\\" + info[0] + "\\" + fileList[z].FileName))
                                fileList[z].FileFolder = (folderSettings[1] + "\\" + info[0]);                            
                        }//end of if-else                        
                    }//end of for loop 
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
                if (int.Parse(folderSettings[0]) == 1)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)                    
                        CopyFile(fileList[i].FullFileName, (folderSettings[1] + "\\" + fileList[i].FileName));                    
                }
                else if (int.Parse(folderSettings[0]) > 1)
                {
                    List<string> folderlist = folderFinder(folderSettings[1]);
                    List<string> info = new List<string>();
                    for (int z = 0; z < fileList.Count; z++)
                    {
                        string fullFileName = fileList[z].FullFileName;
                        info = infoFinder(fullFileName, fileList[z].FileFolder, folderlist);

                        if (info[0] == "no folder")
                        {
                            if (MessageBox.Show("There is No Such TV Show in the TV Show Folder, Would you like to Create One?", "Create folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                EditTitle2 mainEdit = new EditTitle2(info[2]);
                                mainEdit.Text = "Edit Folder Name";
                                mainEdit.Location = new Point(this.Location.X + ((this.Size.Width - mainEdit.Size.Width) / 2), this.Location.Y + ((this.Size.Height - mainEdit.Size.Height) / 2));
                                if (mainEdit.ShowDialog() == DialogResult.OK)
                                {
                                    string methodGet = mainEdit.getTitle();
                                    System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + methodGet);
                                    folderlist.Add(methodGet);
                                    info = infoFinder(fullFileName, fileList[z].FileFolder, folderlist);
                                    info[0] = methodGet;
                                    mainEdit.Close();
                                }
                                else
                                {
                                    CopyFile(fileList[z].FullFileName, (folderSettings[1] + "\\" + fileList[z].FileName));
                                    continue;
                                }
                            }
                            else
                            {
                                CopyFile(fileList[z].FullFileName, (folderSettings[1] + "\\" + fileList[z].FileName));
                                continue; 
                            }                   
                        }

                        if (info[1] != "0" && int.Parse(folderSettings[0]) == 3)
                        {
                            if (!(System.IO.Directory.Exists(folderSettings[1] + "\\" + info[0] + "\\Season " + info[1])))
                                System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + info[0] + "\\Season " + info[1]);
                            CopyFile(fullFileName, folderSettings[1] + "\\" + info[0] + "\\Season " + info[1] + "\\" + fileList[z].FileName);
                        }
                        else//if no season is selected 
                            CopyFile(fullFileName, folderSettings[1] + "\\" + info[0] + "\\" + fileList[z].FileName);                     
                    }//end of for loop 
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
                if (int.Parse(folderSettings[0]) == 1)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected)
                        {
                            if (MoveFile(fileList[i].FullFileName, (folderSettings[1] + "\\" + fileList[i].FileName)))
                                fileList[i].FileFolder = folderSettings[1];
                        }
                    }
                }
                else if (int.Parse(folderSettings[0]) > 1)
                {
                    List<string> folderlist = folderFinder(folderSettings[1]);
                    List<string> info = new List<string>();
                    for (int z = 0; z < fileList.Count; z++)
                    {
                        if (dataGridView1.Rows[z].Cells[0].Selected || dataGridView1.Rows[z].Cells[1].Selected)
                        {
                            string fullFileName = fileList[z].FullFileName;
                            info = infoFinder(fullFileName, fileList[z].FileFolder, folderlist);

                            if (info[0] == "no folder")
                            {
                                if (MessageBox.Show("There is No Such TV Show in the TV Show Folder, Would you like to Create One?", "Create folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    EditTitle2 mainEdit = new EditTitle2(info[2]);
                                    mainEdit.Text = "Edit Folder Name";
                                    mainEdit.Location = new Point(this.Location.X + ((this.Size.Width - mainEdit.Size.Width) / 2), this.Location.Y + ((this.Size.Height - mainEdit.Size.Height) / 2));
                                    if (mainEdit.ShowDialog() == DialogResult.OK)
                                    {
                                        string methodGet = mainEdit.getTitle();
                                        System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + methodGet);
                                        folderlist.Add(methodGet);
                                        info = infoFinder(fullFileName, fileList[z].FileFolder, folderlist);
                                        info[0] = methodGet;
                                        mainEdit.Close();
                                    }
                                    else
                                    {
                                        if (MoveFile(fullFileName, folderSettings[1] + "\\" + fileList[z].FileName))
                                            fileList[z].FileFolder = (folderSettings[1]);
                                        continue; 
                                    }
                                }
                                else
                                {
                                    if (MoveFile(fullFileName, folderSettings[1] + "\\" + fileList[z].FileName))
                                        fileList[z].FileFolder = (folderSettings[1]);
                                    continue; 
                                }
                            }

                            if (info[1] != "0" && int.Parse(folderSettings[0]) == 3)
                            {
                                if (!(System.IO.Directory.Exists(folderSettings[1] + "\\" + info[0] + "\\Season " + info[1])))
                                    System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + info[0] + "\\Season " + info[1]);

                                if (MoveFile(fullFileName, folderSettings[1] + "\\" + info[0] + "\\Season " + info[1] + "\\" + fileList[z].FileName))
                                    fileList[z].FileFolder = (folderSettings[1] + "\\" + info[0] + "\\Season " + info[1]);
                            }
                            else//if no season is selected 
                            {
                                if (MoveFile(fullFileName, folderSettings[1] + "\\" + info[0] + "\\" + fileList[z].FileName))
                                    fileList[z].FileFolder = (folderSettings[1] + "\\" + info[0]);
                            }//end of if-else    
                        }
                    }//end of for loop 
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
                if (int.Parse(folderSettings[0]) == 1)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected)
                        {
                            CopyFile(fileList[i].FullFileName, (folderSettings[1] + "\\" + fileList[i].FileName));
                        }
                    }
                }
                else if (int.Parse(folderSettings[0]) > 1)
                {
                    List<string> folderlist = folderFinder(folderSettings[1]);
                    List<string> info = new List<string>();
                    for (int z = 0; z < fileList.Count; z++)
                    {
                        if (dataGridView1.Rows[z].Cells[0].Selected || dataGridView1.Rows[z].Cells[1].Selected)
                        {
                            string fullFileName = fileList[z].FullFileName;
                            info = infoFinder(fullFileName, fileList[z].FileFolder, folderlist);

                            if (info[0] == "no folder")
                            {
                                if (MessageBox.Show("There is No Such TV Show in the TV Show Folder, Would you like to Create One?", "Create folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    EditTitle2 mainEdit = new EditTitle2(info[2]);
                                    mainEdit.Text = "Edit Folder Name";
                                    mainEdit.Location = new Point(this.Location.X + ((this.Size.Width - mainEdit.Size.Width) / 2), this.Location.Y + ((this.Size.Height - mainEdit.Size.Height) / 2));
                                    if (mainEdit.ShowDialog() == DialogResult.OK)
                                    {
                                        string methodGet = mainEdit.getTitle();
                                        System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + methodGet);
                                        folderlist.Add(methodGet);
                                        info = infoFinder(fullFileName, fileList[z].FileFolder, folderlist);
                                        info[0] = methodGet;
                                        mainEdit.Close();
                                    }
                                    else
                                    {
                                        CopyFile(fileList[z].FullFileName, (folderSettings[1] + "\\" + fileList[z].FileName));
                                        continue;
                                    }
                                }
                                else
                                {
                                    CopyFile(fileList[z].FullFileName, (folderSettings[1] + "\\" + fileList[z].FileName));
                                    continue; 
                                }
                            }

                            if (info[1] != "0" && int.Parse(folderSettings[0]) == 3)
                            {
                                if (!(System.IO.Directory.Exists(folderSettings[1] + "\\" + info[0] + "\\Season " + info[1])))
                                    System.IO.Directory.CreateDirectory(folderSettings[1] + "\\" + info[0] + "\\Season " + info[1]);
                                CopyFile(fullFileName, folderSettings[1] + "\\" + info[0] + "\\Season " + info[1] + "\\" + fileList[z].FileName);
                            }
                            else//if no season is selected 
                                CopyFile(fullFileName, folderSettings[1] + "\\" + info[0] + "\\" + fileList[z].FileName);

                        }
                    }//end of for loop 
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
                
        public void AddFolder(string folderName, string folderDestination,int format)
        {
            moveToToolStripMenuItem1.DropDownItems.Clear();//Move all Menu Item
            copyToToolStripMenuItem.DropDownItems.Clear();//Copy all Menu Item
            moveSelectedToolStripMenuItem.DropDownItems.Clear();//Move selected Menu Item
            copySelectedToolStripMenuItem.DropDownItems.Clear();//copy selected Menu Item

            menu1.Add(new ToolStripMenuItem());
            menu1[menu1.Count - 1].Name = "move" + (menu1.Count - 1).ToString();
            menu1[menu1.Count - 1].Tag = format.ToString()+"?"+folderDestination;
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
                
        public void SaveFolder(string newFolderInfo,int index)
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
            else {
                button1.Text = "Move To Folder";
                button2.Text = "Copy To Folder";
            }
        }
        #endregion

        #region Update Stuff
        //check to see if internet is avilible
        bool ConnectionExists2()
        {
            try
            {
                System.Net.IPHostEntry objIPHE = System.Net.Dns.GetHostByName("www.google.com");
                //System.Net.Sockets.TcpClient clnt = new System.Net.Sockets.TcpClient("www.google.com", 80);
                //clnt.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }//end of ConnectionExists class

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

        public bool MoveFile(string oldLocation, string newLocation)
        {
            if (oldLocation == newLocation) return true;
            try
            {
                FileSystem.MoveFile(oldLocation, newLocation, UIOption.AllDialogs);
                Log.WriteLog(oldLocation + " Moved to " + newLocation);
            }
            catch (FileNotFoundException r)
            {
                MessageBox.Show("File have been changed or moved \n" + oldLocation);
                Log.WriteLog(r.ToString());
                return false;
            }
            catch (IOException g)
            {
                MessageBox.Show("File already exists or is in use\n" + oldLocation);
                Log.WriteLog(g.ToString());
                return false;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (Exception t)
            {
                MessageBox.Show("Error with Operation\n" + t.ToString());
                Log.WriteLog(t.ToString());
                return false;
            }
            return true;
        }

        public bool CopyFile(string oldLocation, string newLocation)
        {
            try
            {
                FileSystem.CopyFile(oldLocation, newLocation, UIOption.AllDialogs);
                Log.WriteLog(oldLocation + " Coped to " + newLocation);
            }
            catch (FileNotFoundException r)
            {
                MessageBox.Show("File have been changed or moved \n" + oldLocation);
                Log.WriteLog(r.ToString());
                return false;
            }
            catch (IOException g)
            {
                MessageBox.Show("File already exists or is in use\n" + oldLocation);
                Log.WriteLog(g.ToString());
                return false;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (Exception t)
            {
                MessageBox.Show("Error with Operation\n" + t.ToString());
                Log.WriteLog(t.ToString());
                return false;
            }
            return true;
        }

        //change Buttons color
        public void changeButtoncolor(Color newColor)
        {
            button1.BackColor = newColor;
            button2.BackColor = newColor;
            button5.BackColor = newColor;
            button6.BackColor = newColor;
        }

        //get selected titles off IMDB
        public void getTVDBTitles()
        {
            if (fileList.Count != 0 && ConnectionExists()) //if files are selected
            {
                int format = -1;
                format = newMainSettings.SeasonFormat + 1;

                Thread h = new Thread(delegate() { autoTitleTVDB(format, false); });
                h.Start();
            }
        }

        //method for thread TVDB
        public void autoTitleTVDB(int format2, bool all)
        {
            if (all && fileList.Count != 0)
            {
                TVDB InternetTest = new TVDB(this, fileList, newMainSettings.DataFolder, format2);
            }
            else
                if (dataGridView1.CurrentRow != null)
                {
                    List<int> z = new List<int>();

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected)
                            z.Add(i);
                    }
                    if (z.Count() != 0)
                    {
                        TVDB InternetTest = new TVDB(this, fileList, z, newMainSettings.DataFolder, format2);
                    }
                }
            MethodInvoker action = delegate
            {
                dataGridView1.Refresh();
                dataGridView1.AutoResizeColumns();
            };
            dataGridView1.BeginInvoke(action);
        }

        //new convert method
        public void autoConvert()
        {
            if (fileList.Count != 0) //if files are selected
            {
                List<int> autoTileList = new List<int>();
                bool online =  ConnectionExists();
            
                for (int z = 0; z < fileList.Count; z++)
                {
                    if (!fileList[z].AutoEdit)
                        continue;
                    //call fileRenamer method
                    fileList[z].NewFileName = this.fileRenamer(fileList[z].FileName, z, fileList[z].FileExtention, autoTileList);
                }//end of for loop
                //firstWord = "";

                if (autoTileList.Count() != 0&&online)
                {
                    TVDB InternetTest = new TVDB(this, fileList, autoTileList, newMainSettings.DataFolder, newMainSettings.SeasonFormat + 1);
                }

                MethodInvoker action = delegate
                {
                    dataGridView1.Refresh();
                    dataGridView1.AutoResizeColumns();
                };
                if (!newMainSettings.FormClosed)
                    dataGridView1.BeginInvoke(action);
            }
        }//end of convert method

        //write log called by download form
        public void writeLog(string error)
        {
            Log.WriteLog(error);
        }

        //new way to add files from folder
        public void ProcessDir(string sourceDir, int recursionLvl)
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
                    };
                    dataGridView1.BeginInvoke(action);
                }

                // Recurse into subdirectories of this directory.
                string[] subdirEntries = Directory.GetDirectories(sourceDir);
                foreach (string subdir in subdirEntries)
                {
                    if (subdir == "$RECYCLE.BIN")
                    {
                        break;
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
        public void ProcessDirZIP(string sourceDir)
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
        public void junkRemover()
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
            startlist.Add("buck");
            startlist.Add("bucktv");
            startlist.Add("orpheus");
            startlist.Add("720p");
            startlist.Add("x264");
            startlist.Add("dimension");
            startlist.Add("60fps");
            startlist.Add("d734");
            startlist.Add("mspaint");
            startlist.Add("siso");
            startlist.Add("reward");
            startlist.Add("www.directlinkspot.com");
            startlist.Add("www directlinkspot com");
            startlist.Add("onelinkmoviez.com");
            startlist.Add("onelinkmoviez com");
            startlist.Add("asap");
            startlist.Add("dd5.1");
            startlist.Add("dd5 1");


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

        //add title 
        public bool addTitle(string title, int which)
        {
            bool worked = false;
            if (fileList.Count > which)
            {
                worked = true;
                fileList[which].FileTitle = title;
            }
            return worked;
        }

        //clear titles
        public void clearTitles()
        {
            for (int i = 0; i < fileList.Count(); i++)
                fileList[i].FileTitle = "";
            Thread t = new Thread(new ThreadStart(autoConvert));
            t.Start();
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
                    if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected)
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
                    if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected)
                        u.Add(fileList[i].FileName);
                }
            }
            return u;
        }

        //Move All Files
        public void moveAllFiles(string Outputfolder)
        {
            for (int z = 0; z < fileList.Count; z++)
            {
                string fullFileName = fileList[z].FullFileName;
                try
                {
                    FileSystem.MoveFile(fullFileName, (Outputfolder + "\\" + fileList[z].FileName), UIOption.AllDialogs);
                    Log.WriteLog(fullFileName + " Moved to " + Outputfolder);
                    //clear stuff                    
                    fileList[z].FileFolder = (Outputfolder);
                }
                catch (FileNotFoundException r)
                {
                    MessageBox.Show("File have been changed or moved \n" + fullFileName);
                    Log.WriteLog(r.ToString());
                    continue;
                }
                catch (IOException g)
                {
                    MessageBox.Show("File already exists or is in use\n" + fullFileName);
                    Log.WriteLog(g.ToString());
                    continue;
                }
                catch (OperationCanceledException)
                {
                    continue;
                }
                catch (Exception t)
                {
                    MessageBox.Show("Error with Operation\n" + t.ToString());
                    Log.WriteLog(t.ToString());
                    continue;
                }
            }
        }

        //Move Selected Files
        public void moveSelectedFiles(string outputFolder)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected)
                {
                    string fullFileName = fileList[i].FullFileName;
                    try
                    {
                        FileSystem.MoveFile(fullFileName, (outputFolder + "\\" + fileList[i].FileName), UIOption.AllDialogs);
                        Log.WriteLog(fullFileName + " Moved to " + outputFolder);
                        fileList[i].FileFolder = (outputFolder);
                    }
                    catch (FileNotFoundException r)
                    {
                        MessageBox.Show("File have been changed or moved \n" + fullFileName);
                        Log.WriteLog(r.ToString());
                        continue;
                    }
                    catch (IOException g)
                    {
                        MessageBox.Show("File already exists or is in use\n" + fullFileName);
                        Log.WriteLog(g.ToString());
                        continue;
                    }
                    catch (OperationCanceledException)
                    {
                        continue;
                    }
                    catch (Exception t)
                    {
                        MessageBox.Show("Error with Operation\n" + t.ToString());
                        Log.WriteLog(t.ToString());
                        continue;
                    }
                }
            }
        }

        //Copy All Files
        public void copyAllFiles(string Outputfolder)
        {
            for (int z = 0; z < fileList.Count; z++)
            {
                string fullFileName = fileList[z].FullFileName;
                try
                {
                    FileSystem.CopyFile(fullFileName, (Outputfolder + "\\" + fileList[z].FileName), UIOption.AllDialogs);
                    Log.WriteLog(fullFileName + " Copied to " + Outputfolder);
                }
                catch (FileNotFoundException r)
                {
                    MessageBox.Show("File have been changed or moved \n" + fullFileName);
                    Log.WriteLog(r.ToString());
                    continue;
                }
                catch (IOException g)
                {
                    MessageBox.Show("File already exists or is in use\n" + fullFileName);
                    Log.WriteLog(g.ToString());
                    continue;
                }
                catch (OperationCanceledException)
                {
                    continue;
                }
                catch (Exception t)
                {
                    MessageBox.Show("Error with Operation\n" + t.ToString());
                    Log.WriteLog(t.ToString());
                    continue;
                }
            }
        }

        //Copy Selected Files
        public void copySelectedFiles(string outputFolder)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected)
                {
                    string fullFileName = fileList[i].FullFileName;
                    try
                    {
                        FileSystem.CopyFile(fullFileName, (outputFolder + "\\" + fileList[i].FileName), UIOption.AllDialogs);
                        Log.WriteLog(fullFileName + " Moved to " + outputFolder);
                    }
                    catch (FileNotFoundException r)
                    {
                        MessageBox.Show("File have been changed or moved \n" + fullFileName);
                        Log.WriteLog(r.ToString());
                        continue;
                    }
                    catch (IOException g)
                    {
                        MessageBox.Show("File already exists or is in use\n" + fullFileName);
                        Log.WriteLog(g.ToString());
                        continue;
                    }
                    catch (OperationCanceledException)
                    {
                        continue;
                    }
                    catch (Exception t)
                    {
                        MessageBox.Show("Error with Operation\n" + t.ToString());
                        Log.WriteLog(t.ToString());
                        continue;
                    }
                }
            }
        }

        //detete Selected Titles
        public void deleteSelectedTitles()
        {
            if (dataGridView1.CurrentRow != null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected)
                    {
                        if (fileList[i].FileTitle == "")
                            continue;
                        fileList[i].FileTitle = "";
                    }
                }
                Thread t = new Thread(new ThreadStart(autoConvert));
                t.Start();
            }
        }

        //detete Selected Files
        public void deleteSelectedFiles()
        {
            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected)
                    fileList.RemoveAt(i);
            }
            dataGridView1.Refresh();
        }

        //close app for update
        public void CloseForUpdates()
        {
            newMainSettings.ClosedForUpdates = true;
            Application.Exit();
        }

        #endregion

        #region Private Methods

        //returns string list of info
        private List<string> infoFinder(string oldfile, string oldfileLocation, List<string> folderlist)
        {
            string fileName = lowering(oldfile);
            List<string> stuff = new List<string>();

            string infoChanged = fileName;
            stuff.Add("no folder");
            stuff.Add("0");//season 
            stuff.Add("New Folder");//show folder
            string shortTitle = null;
            string test = fileName;
            int you = -1;//index to end of shows name

            for (int i = 40; i >= 0; i--)
            {
                //varable for break command later
                bool end = false;
                //loop for episodes
                for (int j = 1; j < 100; j++)
                {
                    string newi = i.ToString();
                    string newj = j.ToString();
                    //check if i is less than 10
                    if (i < 10)
                        newi = "0" + i.ToString();
                    //check if j is less than 10
                    if (j < 10)
                        newj = "0" + j.ToString();
                    //make string to compare changed name too
                    //1x01 format 
                    if (newMainSettings.SeasonFormat == 0)
                        you = test.IndexOf(i.ToString() + "x" + newj);//1x01 add title                    
                    //0101 format
                    if (newMainSettings.SeasonFormat == 1)
                        you = test.IndexOf(newi + newj);//0101 add title                    
                    //101 format
                    if (newMainSettings.SeasonFormat == 2)
                        you = test.IndexOf(i.ToString() + newj);//0101 add title                    
                    //S01E01 format
                    if (newMainSettings.SeasonFormat == 3)
                    {
                        you = test.IndexOf("S" + newi + "E" + newj);//S01E01 add title
                        you = test.IndexOf("S" + newi + "e" + newj);//S01E01 add title if second time
                    }
                    //stop loop when name is change                    
                    if (you != -1)
                    {
                        stuff[1] = i.ToString();
                        shortTitle = test.Remove(you - 1, test.Length - (you - 1));
                        //figure out name for new folder
                        stuff[2] = oldfile.Remove(you - 1, test.Length - (you - 1)).Replace(oldfileLocation + "\\", "");

                        end = true;
                        break;
                    }
                }//end of episode loop
                //stop loop when name is change
                if (end)
                    break;
            }//end of season loop

            //date format
            if (newMainSettings.SeasonFormat == 4)
            {
                string finalValue = null;
                for (int year = 0; year < 20; year++)
                {
                    bool end = false;
                    for (int month = 12; month >= 0; month--)
                    {
                        for (int day = 31; day > 0; day--)
                        {
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
                            finalValue = month.ToString() + "-" + day.ToString() + "-" + kk;
                            you = test.IndexOf(finalValue);//date time
                            if (you != -1)
                            {
                                shortTitle = test.Remove(you - 1, test.Length - (you - 1));
                                //figure out name for new folder
                                stuff[2] = oldfile.Remove(you - 1, test.Length - (you - 1)).Replace(oldfileLocation + "\\", "");
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

            //figure out if tv show is listed
            if (shortTitle == null)
                shortTitle = fileName;
            for (int i = 0; i < folderlist.Count(); i++)
            {
                string newFolderEdited = lowering(folderlist[i]);

                infoChanged = shortTitle.Replace(newFolderEdited, "0000");
                if (infoChanged != shortTitle)
                {
                    stuff[0] = folderlist[i];
                    break;
                }
            }//end of for loop            
            return stuff;
        }//end of infofinder method

        //get list of folders
        private List<string> folderFinder(String folderwatch)
        {
            List<String> foldersIn = new List<String>();
            List<String> revFoldersIn = new List<String>();
           
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folderwatch);
                try
                {
                    foreach (System.IO.DirectoryInfo fi in di.GetDirectories())
                    {
                        foldersIn.Add(fi.Name);
                    }
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

        //lowercase stuff
        private string lowering(string orig)
        {//make every thing lowercase for crap remover to work
            StringBuilder s = new StringBuilder(orig);
            for (int l = 0; l < orig.Length; l++)
                s[l] = char.ToLower(s[l]);
            return s.ToString();
        }

        //uppercase stuff
        private string UpperCaseing(string orig)
        {//make every thing lowercase for crap remover to work
            StringBuilder s = new StringBuilder(orig);
            for (int l = 0; l < orig.Length; l++)
                s[l] = char.ToUpper(s[l]);
            return s.ToString();
        }

        //uppercase stuff with parameters
        private string UpperCaseing(string orig, int start, int end)
        {//make every thing lowercase for crap remover to work
            StringBuilder s = new StringBuilder(orig);
            for (int l = start; l < end; l++)
                s[l] = char.ToUpper(s[l]);
            return s.ToString();
        }

        //uppercase stuff after spaces with parameters
        private string UpperCaseingAfterSpace(string orig, int start, int end, bool first)
        {
            StringBuilder s2 = new StringBuilder(orig);

            //make first letter capital
            if (first)
                s2[start] = char.ToUpper(s2[start]);

            //Finds Letter after spaces and capitalizes them
            for (int i = start; i < end; i++)
            {
                if (s2[i] == ' ' || s2[i] == '.')
                    s2[i + 1] = char.ToUpper(s2[i + 1]);
            }//end of for loop

            return s2.ToString();
        }

        //uppercase first thing after space
        private string UpperCaseFirstAfterSpace(string orig, int start, int end)
        {
            StringBuilder s2 = new StringBuilder(orig);
            int size3 = orig.Length;

            //Finds Letter after spaces and capitalizes them
            for (int i = start; i < end; i++)
            {
                if ((s2[i] == ' ' || s2[i] == '.') && s2[i + 1] != '-')
                {
                    s2[i + 1] = char.ToUpper(s2[i + 1]);
                    break;
                }
            }//end of for loop

            return s2.ToString();
        }

        //rename method
        private string fileRenamer(string newfilename, int index, string extend,List<int>selected)
        {//make temp function
            string temp = null;
            string seasonDash = "";
            int startIndex = -1;
            int endIndex = -1;

            if (!newMainSettings.DashSeason)
                seasonDash = "- ";
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
                newMainSettings.FirstWord = "";
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
            StringBuilder s = new StringBuilder(newfilename);
            for (int l = 0; l < newfilename.Length; l++)
                s[l] = char.ToLower(s[l]);

            //reassign edited name 
            newfilename = s.ToString();

            //remove extra crap 
            if (newMainSettings.RemoveCrap)
            {
                //new way with file input
                for (int x = 0; x < junklist.Count(); x++)
                    newfilename = newfilename.Replace(junklist[x] + temp, temp);
            }//end of removeExtraCrapToolStripMenuItem if

            //remove begining space
            StringBuilder space = new StringBuilder(newfilename);
            if (space[0] == ' ')
            {
                for (int p = 0; p < (newfilename.Length - 1); p++)
                    space[p] = space[p + 1];
            }

            //reassign edited name 
            newfilename = space.ToString();
            newfilename = newfilename.Replace("&&&&&", "&&&&");//fix that i hope works

            //remove year function
            if (newMainSettings.RemoveYear && (!(newMainSettings.SeasonFormat == 4)))
            {
                int curyear = System.DateTime.Now.Year;
                for (; curyear > 1980; curyear--)
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

            //add dash if the title exists or add one 
            string tempTitle = null;
            if (!newMainSettings.DashTitle)
            {
                //bool titleAvil = titles.checkTitle(index);
                if (fileList[index].FileTitle != "")
                    tempTitle = " - " + fileList[index].FileTitle;
                else
                    tempTitle = " -";
            }
            else
            {
                tempTitle = temp + fileList[index].FileTitle;
                //tempTitle = "";
            }//end of if-else

            StringBuilder tempCharTitle = new StringBuilder(tempTitle);
            for (int tempCharTitleIndex = 0; tempCharTitleIndex < tempTitle.Length; tempCharTitleIndex++)
                tempCharTitle[tempCharTitleIndex] = char.ToLower(tempCharTitle[tempCharTitleIndex]);

            //reassign edited name 
            tempTitle = tempCharTitle.ToString();

            //loop for seasons
            for (int i = 0; i < 40; i++)
            {
                //varable for break command later
                bool end = false;

                //loop for episodes
                for (int j = 0; j < 100; j++)
                {
                    string newi = i.ToString();
                    string newi2 = (i + newMainSettings.SeasonOffset).ToString();
                    string newj = j.ToString();
                    string newj2 = (j + newMainSettings.EpisodeOffset).ToString();
                    string output = null;
                    string output2 = null;
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

                    //make string to compare changed name too
                    string startnewname = newfilename;

                    //1x01 format 
                    if (newMainSettings.SeasonFormat == 0)
                    {
                        output2 = (i + newMainSettings.SeasonOffset).ToString() + "x" + newj2;
                        output = i.ToString() + "x" + newj;

                        newfilename = newfilename.Replace(temp + i.ToString() + newj + temp, temp + output + temp);//101
                        newfilename = newfilename.Replace(newi + newj, output);//0101                        
                        newfilename = newfilename.Replace("s" + i.ToString() + "e" + j.ToString() + temp, output + temp);//s1e1               
                        newfilename = newfilename.Replace("s" + i.ToString() + "e" + newj + temp, output + temp);//s1e01
                        newfilename = newfilename.Replace("s" + newi + "e" + newj, output);//s01e01
                        newfilename = newfilename.Replace("s" + newi + " e" + newj, output);//s01 e01
                        newfilename = newfilename.Replace("season " + i.ToString() + " episode " + newj + temp, output + temp);//season 1 episode 01
                        newfilename = newfilename.Replace("season " + i.ToString() + " episode " + j.ToString() + temp, output + temp);//season 1 episode 1
                        newfilename = newfilename.Replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);//1 01
                        newfilename = newfilename.Replace("0" + output, output);//01x01 fix might be unnessitarry
                        newfilename = newfilename.Replace(output, seasonDash + output2 + tempTitle);//1x01 add title
                        startIndex = newfilename.IndexOf(output2);//find index                        
                        if (startIndex != -1)
                        {
                            if (i > 9) { endIndex = startIndex + 5; } else { endIndex = startIndex + 4; }
                            //endIndex = startIndex + 4;
                        }

                    }
                    //0101 format
                    if (newMainSettings.SeasonFormat == 1)
                    {
                        output2 = newi2 + newj2;
                        output = newi + newj;

                        newfilename = newfilename.Replace(temp + i.ToString() + newj + temp, temp + output + temp);//101 
                        newfilename = newfilename.Replace(temp + i.ToString() + "x" + newj, temp + output);//1x01
                        newfilename = newfilename.Replace("s" + newi + "e" + newj, output);//s01e01
                        newfilename = newfilename.Replace("s" + newi + " e" + newj, output);//s01 e01
                        newfilename = newfilename.Replace("s" + i.ToString() + "e" + j.ToString() + temp, output + temp);//s1e1
                        newfilename = newfilename.Replace("s" + i.ToString() + "e" + newj + temp, output + temp);//s1e01
                        newfilename = newfilename.Replace("season " + i.ToString() + " episode " + newj + temp, output + temp);//season 1 episode 01
                        newfilename = newfilename.Replace("season " + i.ToString() + " episode " + j.ToString() + temp, output + temp);//season 1 episode 1
                        newfilename = newfilename.Replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);// 1 01 
                        newfilename = newfilename.Replace(output, seasonDash + output2 + tempTitle);//0101 add title
                        startIndex = newfilename.IndexOf(output2);//find index
                        if (startIndex != -1)
                            endIndex = startIndex + 4;
                    }
                    //S01E01 format
                    if (newMainSettings.SeasonFormat == 2)
                    {
                        output2 = "S" + newi2 + "E" + newj2;
                        output = "S" + newi + "E" + newj;

                        newfilename = newfilename.Replace(temp + i.ToString() + newj + temp, temp + output + temp);//101
                        newfilename = newfilename.Replace(temp + i.ToString() + "x" + newj, temp + output);//1x01
                        newfilename = newfilename.Replace(newi + newj, output);//0101
                        newfilename = newfilename.Replace("s" + newi + "e" + newj, output);//s01E01
                        newfilename = newfilename.Replace("s" + i.ToString() + "e" + j.ToString() + temp, output + temp);//s1e1
                        newfilename = newfilename.Replace("s" + i.ToString() + "e" + newj + temp, output + temp);//s1e01
                        newfilename = newfilename.Replace("s" + newi + " e" + newj, output);//s01 e01
                        newfilename = newfilename.Replace("season " + i.ToString() + " episode " + newj + temp, output + temp);//season 1 episode 01
                        newfilename = newfilename.Replace("season " + i.ToString() + " episode " + j.ToString() + temp, output + temp);//Season 1 Episode 1
                        newfilename = newfilename.Replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);//1 01
                        newfilename = newfilename.Replace("S" + newi + "E" + newj, seasonDash + output2 + tempTitle);//S01E01 add title
                        newfilename = newfilename.Replace("s" + newi + "e" + newj, seasonDash + output2 + tempTitle);//S01E01 add title if second time
                        startIndex = newfilename.IndexOf(output2);//find index
                        if (startIndex != -1)
                            endIndex = startIndex + 6;
                    }
                    //101 format
                    if (newMainSettings.SeasonFormat == 3)
                    {
                        output2 = (i + newMainSettings.SeasonOffset).ToString() + newj2;
                        output = i.ToString() + newj;

                        newfilename = newfilename.Replace(newi + newj, output);//0101
                        newfilename = newfilename.Replace(temp + i.ToString() + "x" + newj, temp + output);//1x01
                        newfilename = newfilename.Replace("s" + newi + "e" + newj, output);//s01e01
                        newfilename = newfilename.Replace("s" + newi + " e" + newj, output);//s01 e01
                        newfilename = newfilename.Replace("s" + i.ToString() + "e" + j.ToString() + temp, output + temp);//s1e1
                        newfilename = newfilename.Replace("s" + i.ToString() + "e" + newj + temp, output + temp);//s1e01
                        newfilename = newfilename.Replace("season " + i.ToString() + " episode " + newj + temp, output + temp);//season 1 episode 01
                        newfilename = newfilename.Replace("season " + i.ToString() + " episode " + j.ToString() + temp, output + temp);//season 1 episode 1
                        newfilename = newfilename.Replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);// 1 01 
                        newfilename = newfilename.Replace(output, seasonDash + output2 + tempTitle);//0101 add title
                        startIndex = newfilename.IndexOf(output2);//find index
                        if (startIndex != -1)
                        {
                            if (i > 9) { endIndex = startIndex + 4; } else { endIndex = startIndex + 3; }
                            //endIndex = startIndex + 3;
                        }
                    }

                    //stop loop when name is change                    
                    if (startnewname != newfilename)
                    {
                        end = true;
                        break;
                    }

                }//end of episode loop

                //stop loop when name is change
                if (end)
                    break;

            }//end of season loop

            //Date format
            if (newMainSettings.SeasonFormat == 4)
            {
                //add dash if the title exists or add one 
                string dateTitle = null;
                if (!newMainSettings.DashTitle)
                {
                    if (fileList[index].FileTitle != "")
                        dateTitle = " - " + fileList[index].FileTitle;
                    else
                        dateTitle = " -";
                }
                else
                    dateTitle = fileList[index].FileTitle;

                for (int year = 0; year < 20; year++)
                {
                    bool end = false;
                    for (int month = 12; month > 0; month--)
                    {
                        for (int day = 31; day > 0; day--)
                        {
                            string startnewname = newfilename;
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

                            newfilename = newfilename.Replace(month + "-" + day + "-" + kk, seasonDash + month + "-" + day + "-" + kk + dateTitle);//add title
                            newfilename = newfilename.Replace(month + " " + day + " " + kk, seasonDash + month + "-" + day + "-" + kk + dateTitle);//add title
                            startIndex = newfilename.IndexOf(month + "-" + day + "-" + kk);//find index
                            if (startIndex != -1)
                            {
                                if (day > 9 && month > 9)
                                    endIndex = startIndex + 10;
                                else if (day > 9 || month > 9)
                                    endIndex = startIndex + 9;
                                else
                                    endIndex = startIndex + 8;
                            }
                            if (startnewname != newfilename)
                            {
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

            if (startIndex == -1)
                startIndex = newfilename.Length - 1;

            switch (newMainSettings.ProgramFormat)
            {
                case 0:
                    newfilename = UpperCaseingAfterSpace(newfilename, 0, startIndex, true);
                    break;
                case 1:
                    newfilename = UpperCaseing(newfilename, 0, 1);
                    break;
                case 2:
                    newfilename = UpperCaseing(newfilename, 0, startIndex);
                    break;
                default:
                    break;
            }

            if (endIndex != -1)
            {
                switch (newMainSettings.TitleFormat)
                {
                    case 0:
                        newfilename = UpperCaseingAfterSpace(newfilename, endIndex, newfilename.Length, false);
                        break;
                    case 1:
                        newfilename = UpperCaseFirstAfterSpace(newfilename, endIndex, newfilename.Length);
                        break;
                    case 2:
                        newfilename = UpperCaseing(newfilename, endIndex, newfilename.Length);
                        break;
                    case 4:
                        StringBuilder filenameOld = new StringBuilder(newfilename);
                        StringBuilder filenameNew = new StringBuilder();
                        filenameNew.Length = endIndex;
                        for (int q = 0; q < endIndex; q++)
                            filenameNew[q] = filenameOld[q];
                        newfilename = filenameNew.ToString() + temp + "&&&&";
                        break;
                    default:
                        break;
                }
                if (newMainSettings.TitleFormat != 4 && newMainSettings.AutoGetTitle)
                {
                    StringBuilder titleSting = new StringBuilder(newfilename);
                    string temp1 = (titleSting[endIndex].ToString() + titleSting[endIndex + 1].ToString() + titleSting[endIndex + 2].ToString() + titleSting[endIndex + 3].ToString() + titleSting[endIndex + 4].ToString());
                    if (temp1 == (temp + "&&&&") || temp1 == (" - &&") || temp1 == (temp + temp + "&&&"))
                    {
                        selected.Add(index);
                        //int format3 = -1;
                        //format3 = newMainSettings.SeasonFormat + 1;                        
                        //TVDB InternetTest = new TVDB(this, newfilename, index, fileList, newMainSettings.DataFolder, format3);
                    }
                }
            }

            switch (newMainSettings.ExtFormat)
            {
                case 0:
                    extend = lowering(extend);
                    break;
                case 1:
                    StringBuilder ext1 = new StringBuilder(extend);
                    ext1[1] = char.ToUpper(ext1[1]);
                    extend = ext1.ToString();
                    break;
                case 2:
                    extend = UpperCaseing(extend);
                    break;
                default:
                    break;
            }
            //add file extention back on 
            newfilename = newfilename.Replace(temp + "&&&&", extend);

            //Random fixes
            newfilename = newfilename.Replace("..", ".");
            newfilename = newfilename.Replace(" .", ".");
            newfilename = newfilename.Replace("- -", "-");
            newfilename = newfilename.Replace(".-.", ".");
            newfilename = newfilename.Replace("-.", ".");
            newfilename = newfilename.Replace(" .", ".");
            newfilename = newfilename.Replace("Vs", "vs");
            newfilename = newfilename.Replace("O C ", "O.C. ");
            newfilename = newfilename.Replace("T O ", "T.O. ");
            newfilename = newfilename.Replace("Csi", "CSI");
            newfilename = newfilename.Replace("Wwii", "WWII");
            newfilename = newfilename.Replace("Hd", "HD");
            newfilename = newfilename.Replace("Tosh 0", "Tosh.0");
            newfilename = newfilename.Replace("O Brien", "O'Brien");
            newfilename = newfilename.Replace("Nbc", "NBC");
            newfilename = newfilename.Replace("Abc", "ABC");
            newfilename = newfilename.Replace("Cbs", "CBS");
            newfilename = newfilename.Replace("Iv" + temp, "IV" + temp);
            newfilename = newfilename.Replace("Ix" + temp, "IX" + temp);
            newfilename = newfilename.Replace("Viii", "VIII");
            newfilename = newfilename.Replace("Vii", "VII");
            newfilename = newfilename.Replace("Vi" + temp, "VI" + temp);
            newfilename = newfilename.Replace("Xi" + temp, "XI" + temp);
            newfilename = newfilename.Replace("Xii" + temp, "XII" + temp);
            newfilename = newfilename.Replace("Xiii" + temp, "XIII" + temp);
            newfilename = newfilename.Replace("Xiiii" + temp, "XIIII" + temp);
            newfilename = newfilename.Replace("Iii", "III");
            newfilename = newfilename.Replace("Ii", "II");
            newfilename = newfilename.Replace("X Files", "X-Files");
            newfilename = newfilename.Replace("La ", "LA ");
            newfilename = newfilename.Replace("Nba", "NBA");

            // return converted file name
            return newfilename;
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
            Thread p = new Thread(new ThreadStart(autoConvert));
            p.Start();
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
        public void archiveExtrector(string zipfile, string zipName, string password, bool add)
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
            Thread p = new Thread(new ThreadStart(autoConvert));
            p.Start();
            mainExtrector.Extracting -= extr_Extracting;
            mainExtrector.Dispose();
            MethodInvoker action4 = delegate
            {
                progressBar1.Hide();
            };
            progressBar1.BeginInvoke(action4);
        }

        //add files 
        private void getFiles(string[] fileList2)
        {
            if (fileList2 == null)
                return;
            //loop for each file in array
            foreach (String file3 in fileList2)
            {
                bool stopall = false;
                FileInfo fi3 = new FileInfo(file3);

                //check if file is already in list
                for (int i = 0; i < fileList.Count(); i++)
                {
                    if (fi3.Name == fileList[i].FileName)
                    {
                        stopall = true;
                        break;
                    }
                }
                if (stopall)
                    continue;
                if (fi3.Extension == ".zip" || fi3.Extension == ".rar" || fi3.Extension == ".r01" || fi3.Extension == ".001" || fi3.Extension == ".7z")
                    archiveExtrector(file3, fi3.Name, true);
                else
                {
                    if (fi3.Extension == "" || fi3.Extension == null)
                        break;
                    MethodInvoker action = delegate
                    {
                        fileList.Add(new TVClass(fi3.DirectoryName, fi3.Name, fi3.Extension));
                        Thread p = new Thread(new ThreadStart(autoConvert));
                        p.Start();
                        dataGridView1.Refresh();
                    };
                    dataGridView1.BeginInvoke(action);
                }
            }//end of loop             
        }

        //add files from folder
        private void getFilesInFolder(string folder)
        {
            //unzip everything then process all of the unziped file created by the unzipping
            if (newMainSettings.OpenZIPs)
                ProcessDirZIP(folder);
            ProcessDir(folder, 0);

            Thread p = new Thread(new ThreadStart(autoConvert));
            p.Start();
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

        //drag and drop
        private void dragTo_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void dragTo_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                getFiles(files);
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
                    if (dataGridView1.Rows[u].Cells[0].Selected || dataGridView1.Rows[u].Cells[1].Selected)
                    {
                        try
                        {
                            FileSystem.MoveFile((fileList[u].FullFileName), (fileList[u].NewFullFileName), true);
                            //System.IO.File.Move((fileList[u].FullFileName), (fileList[u].NewFullFileName));
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

                    Thread t = new Thread(new ThreadStart(autoConvert));
                    t.Start();
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
                    if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected)
                    {
                        selectedFiles = fileList[i].FileFolder;
                        break;
                    }
                }
                //selectedFiles = fileList[0].FileFolder;
                Process.Start("explorer.exe", selectedFiles);
                //Display publibb = new Display(u);
            }
        }

        //right click to get titles off IMDB
        private void getTitlesOffIMBDOfSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.getTVDBTitles();
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
                    if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected)
                    {
                        if (fileList[i].FileTitle == "")                        
                            continue;                        
                        EditTitle2 mainEdit = new EditTitle2(fileList[i].FileTitle);
                        mainEdit.Location = new Point(this.Location.X + ((this.Size.Width - mainEdit.Size.Width) / 2), this.Location.Y + ((this.Size.Height - mainEdit.Size.Height) / 2));
                        if (mainEdit.ShowDialog() == DialogResult.OK)
                        {
                            fileList[i].FileTitle = mainEdit.getTitle();
                            mainEdit.Close();
                        }
                    }
                }
                Thread t = new Thread(new ThreadStart(autoConvert));
                t.Start();
            }
        }

        //edit Pending File Name
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected)
                    {
                        EditTitle2 mainEdit = new EditTitle2(fileList[i].NewFileName);
                        mainEdit.Text = "Edit Pending File Name";
                        mainEdit.Location = new Point(this.Location.X + ((this.Size.Width - mainEdit.Size.Width) / 2), this.Location.Y + ((this.Size.Height - mainEdit.Size.Height) / 2));
                        if (mainEdit.ShowDialog() == DialogResult.OK)
                        {
                            fileList[i].NewFileName = mainEdit.getTitle();
                            fileList[i].AutoEdit = false;
                            mainEdit.Close();
                        }
                        dataGridView1.Refresh();
                        dataGridView1.AutoResizeColumns();
                    }
                }
            }
        }

        #endregion

        //loads when starts
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!(System.IO.Directory.Exists(newMainSettings.DataFolder)))
                System.IO.Directory.CreateDirectory(newMainSettings.DataFolder);
            Log.startLog(newMainSettings.DataFolder);
            newMainSettings.Start(Log);

            this.junkRemover();
            this.fileChecker();
            newMainSettings.loadStettings();
            if (newMainSettings.CheckForUpdates)
            {
                Thread updateChecker = new Thread(new ThreadStart(checkForUpdateSilent));
                updateChecker.Start();
            }
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
        }//end of load command

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