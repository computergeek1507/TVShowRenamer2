﻿using System;
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
        public Form1()
        {
            InitializeComponent();
        }

        #region Initiate Stuff
        //initiate varibles  
        const int appVersion = 250;//2.5Beta
        const int HowDeepToScan = 4;
        bool addfile = false;
        bool shownb4 = false;
        bool openZIPs = false;
        
        List<string> fileFolder = new List<string>();//origonal folder
        List<string> fileName = new List<string>();//origonal file name
        List<string> fileExtention = new List<string>();//origonal file Extention

        List<string> newFileName = new List<string>();//new file name

        List<string> movefolder = new List<string>();
        List<string> junklist = new List<string>();
        
        //create other forms
        junk_words userJunk = new junk_words();
        Addtitle titles = new Addtitle();
        Text_Converter textConvert = new Text_Converter();
        LogWrite Log = new LogWrite();

        //get working directory
        string commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\TV Show Renamer Beta";
        
        #endregion
        
        #region Menu Buttons

        //add files button
        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            openFileDialog2.Title = "Select Media files";
            openFileDialog2.Filter = "Video Files (*.avi;*.mkv;*.mp4;*.m4v;*.mpg)|*.avi;*.mkv;*.mp4;*.m4v;*.mpg|Archive Files (*.zip;*.rar;*.r01)|*.zip;*.rar;*.r01|All Files (*.*)|*.*";
            openFileDialog2.FileName = "";
            openFileDialog2.FilterIndex = 0;
            openFileDialog2.InitialDirectory = "Documents";
            openFileDialog2.CheckFileExists = true;
            openFileDialog2.CheckPathExists = true;

            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Thread h = new Thread(delegate() { getFiles(openFileDialog2.FileNames); });                
                h.Start();
                /*      
                //loop for each file in array
                foreach (String file3 in openFileDialog2.FileNames)
                {
                    FileInfo fi3 = new FileInfo(file3);
                    if (fi3.Extension == ".avi" || fi3.Extension == ".mkv" || fi3.Extension == ".mp4" || fi3.Extension == ".m4v" || fi3.Extension == ".mpg") {
                        //add file name
                        fileName.Add(fi3.Name);
                        newFileName.Add(fi3.Name);
                        //add file folder
                        fileFolder.Add(fi3.DirectoryName);
                        //add file extension
                        fileExtention.Add(fi3.Extension);                    
                    }

                    if (fi3.Extension == ".zip" || fi3.Extension == ".rar" || fi3.Extension == ".r01")
                    {
                        List<string> info = archiveExtrector(file3);

                        if (info[0] == "0") {
                            continue;
                        }
                        //add file name
                        fileName.Add(info[0]);
                        newFileName.Add(info[0]);
                        //add file folder
                        fileFolder.Add(info[1]);
                        //add file extension
                        fileExtention.Add(info[2]);
                    }
                                        

                }//end of loop 
                dataGridView1.Rows.Clear();             
                for (int i = 0; i < fileName.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = fileName[i];
                    dataGridView1.Rows[i].Cells[1].Value = fileName[i];
                }
                if (fileName.Count() != 0)
                {
                    Thread t = new Thread(new ThreadStart(autoConvert));
                    t.Start();
                }*/

            }//end of if

        }//end of file button

        //add folder button
        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Thread u = new Thread(delegate() { getFilesInFolder(folderBrowserDialog1.SelectedPath); });
                u.Start();
                /*
                string folder = null;
                folder = folderBrowserDialog1.SelectedPath;
                ProcessDir(folder, 0);

                dataGridView1.Rows.Clear();
                for (int i = 0; i < fileName.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = fileName[i];
                    dataGridView1.Rows[i].Cells[1].Value = fileName[i];
                }

                if (fileName.Count() != 0)
                {
                    Thread t = new Thread(new ThreadStart(autoConvert));
                    t.Start();
                }
                */
                #region Old folder stuff
                /*
                bool addfile = false;
                bool shownb4 = false;
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folder);
                foreach (System.IO.FileInfo fi in di.GetFiles("*"))
                {
                    string origName = fi.Name;
                    string exten = fi.Extension;
                    //if thumb file dont convert
                    if (origName == "Thumbs.db")
                    {
                        continue;
                    }
                    //check if its a legal file type
                    if (!(exten == ".avi" || exten == ".mkv" || exten == ".mp4" || exten == ".mpg"))
                    {
                        //if dialog was shown b4 dont show again
                        if (!shownb4)
                        {                            
                            if (MessageBox.Show("You have selected a folder with files that aren't Media Files\nWould you like to add them?", "Media Options", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                //state if files should be added
                                shownb4 = true;
                                addfile = true;
                            }
                            else
                            {
                                //state if files should not be added
                                shownb4 = true;
                                addfile = false;
                                continue;
                            }
                        }
                        //dont add files if not true
                        if (!addfile) {
                            continue;
                        }                        
                    }
                    //add files to displsy
                    richTextBox1.Text += (origName + "\n");
                    //add just names to string list
                    multselct2.Add(origName);
                    //add names and folder to string list
                    multselct.Add(fi.DirectoryName + "\\" + origName);
                    multfile = fi.Name;
                    
                }//end of foreach */

                #endregion
                
            }//end of if

        }//end of folder button

        //remove selected
        private void removeSelectedMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int u = dataGridView1.CurrentRow.Index;

                fileFolder.RemoveAt(u);
                fileExtention.RemoveAt(u);
                fileName.RemoveAt(u);
                newFileName.RemoveAt(u);
                dataGridView1.Rows.Clear();
                for (int i = 0; i < fileName.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = fileName[i];
                    dataGridView1.Rows[i].Cells[1].Value = newFileName[i];
                }
            }
        }

        //Clear items from display
        private void clearListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileFolder.Clear();
            fileName.Clear();
            fileExtention.Clear();
            newFileName.Clear();
            dataGridView1.Rows.Clear();
            titles.clearTitles();
        }

        //Exit button 
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //User junk word menu
        private void addJunkWordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userJunk.Show();
        }

        //text converter menu
        private void textConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textConvert.Show();
        }

        //add title menu
        private void addTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName.Count!=0)
            {
                int r = dataGridView1.CurrentRow.Index;
                titles.sendTitle(fileName,this,r);
                titles.Show();
            }
            else
            {
                MessageBox.Show("No files selected");
            }
        }

        //default setting method 
        private void defaultSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Would You Like to Restore Default Settings?", "Restore Default Settings", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.convertToolStripMenuItem.Checked = true;
                this.convertToToolStripMenuItem.Checked = true;
                this.removeToolStripMenuItem.Checked = true;
                this.capitalizeToolStripMenuItem.Checked = true;
                this.removeExtraCrapToolStripMenuItem.Checked = true;
                this.addForTitleToolStripMenuItem.Checked = true;
                this.x01ToolStripMenuItem.Checked = true;
                this.toolStripMenuItem3.Checked = false;
                this.s01E01ToolStripMenuItem1.Checked = false;
                this.dateToolStripMenuItem.Checked = false;
                this.removeYearToolStripMenuItem.Checked = true;
                movefolder.Clear();
            }
        }//end of default setting method 
                
        //when checked change others to unchecked
        private void x01ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItem3.Checked = false;
            this.s01E01ToolStripMenuItem1.Checked = false;
            this.dateToolStripMenuItem.Checked = false;

            Thread t = new Thread(new ThreadStart(autoConvert));
            t.Start();
        }

        //when checked change others to unchecked
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.x01ToolStripMenuItem.Checked = false;
            this.s01E01ToolStripMenuItem1.Checked = false;
            this.dateToolStripMenuItem.Checked = false;

            Thread t = new Thread(new ThreadStart(autoConvert));
            t.Start();
        }

        //when checked change others to unchecked
        private void s01E01ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItem3.Checked = false;
            this.x01ToolStripMenuItem.Checked = false;
            this.dateToolStripMenuItem.Checked = false;

            Thread t = new Thread(new ThreadStart(autoConvert));
            t.Start();
        }

        //when checked change others to unchecked
        private void dateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItem3.Checked = false;
            this.s01E01ToolStripMenuItem1.Checked = false;
            this.x01ToolStripMenuItem.Checked = false;

            Thread t = new Thread(new ThreadStart(autoConvert));
            t.Start();
        }

        //convert "." to " "
        private void convertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(autoConvert));
            t.Start();
        }

        //convert "_" to " "
        private void convertToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(autoConvert));
            t.Start();
        }

        //remove "-"
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(autoConvert));
            t.Start();
        }

        //Capitalize
        private void capitalizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(autoConvert));
            t.Start();
        }

        //remove extra crap
        private void removeExtraCrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(autoConvert));
            t.Start();
        }

        //add "-" for titles
        private void addForTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(autoConvert));
            t.Start();
        }

        //remove year
        private void removeYearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(autoConvert));
            t.Start();
        }

        //Set Tv Show Folder Button
        private void setTVFolderLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            move_folder tvshow = new move_folder(this, movefolder);
            tvshow.Show();
        }

        //check for updates
        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.checkForUpdate();
            //new thread for update
            //Thread t = new Thread(new ThreadStart(checkForUpdate));
            //t.Start();
        }

        //about display
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About info = new About(appVersion);
            info.Show();
        }

        //settings menu
        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            Settings MainSettings = new Settings(this,openZIPs);
        }
                
        #endregion

        #region On the form Buttons

        //Move Button
        private void button1_Click(object sender, EventArgs e)
        {
            if (fileName.Count != 0) //if files are selected
            {
                if (movefolder.Count() != 0)
                {
                    //move crap goes here
                    List<string> folderlist = folderFinder(movefolder);
                    List<string> info = new List<string>();

                    for (int z = 0; z < fileName.Count; z++)
                    {
                        string fullFileName = fileFolder[z] + "\\" + fileName[z];
                        info = infoFinder(fullFileName, folderlist, movefolder);
                        int index = Convert.ToInt32(info[2]);
                        
                        if (info[0] != "no folder")
                        {
                            if (index == -1)
                            {
                                MessageBox.Show("Folder List is Wrong");
                                return;
                            }
                            if (info[1] != "0")
                            {
                                if (!(File.Exists(movefolder[index] + "\\" + info[0] + "\\Season " + info[1])))
                                {
                                    System.IO.Directory.CreateDirectory(movefolder[index] + "\\" + info[0] + "\\Season " + info[1]);
                                    //MessageBox.Show((movefolder + "\\" + info[0] + "\\Season " + info[1] + "\\" + multselct2[z]));
                                    try
                                    {
                                        //System.IO.File.Move(multselct[z], (movefolder[index] + "\\" + info[0] + "\\Season " + info[1] + "\\" + multselct2[z]));                                        
                                        FileSystem.MoveFile(fullFileName, (movefolder[index] + "\\" + info[0] + "\\Season " + info[1] + "\\" +fileName[z]), UIOption.AllDialogs);
                                        Log.moveWriteLog(fullFileName, (movefolder[index] + "\\" + info[0] + "\\Season " + info[1] + "\\"));
                                        //clear stuff
                                        fileFolder[z] = (movefolder[index] + "\\" + info[0] + "\\Season " + info[1] );

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
                                        MessageBox.Show("Broken\n" + t.ToString());
                                        Log.WriteLog(t.ToString());
                                        continue;
                                    }
                                }
                            }
                            else//if no season is selected 
                            {
                                try
                                {
                                    //System.IO.File.Move(multselct[z], (movefolder[index] + "\\" + info[0] + "\\" + multselct2[z]));
                                    FileSystem.MoveFile(fullFileName, (movefolder[index] + "\\" + info[0] + "\\" + fileName[z]), UIOption.AllDialogs);
                                    Log.moveWriteLog(fullFileName, (movefolder[index] + "\\" + info[0]));
                                    fileFolder[z] = (movefolder[index] + "\\" + info[0] );
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
                                catch (OperationCanceledException )
                                {
                                    continue;
                                }
                                catch (Exception t)
                                {
                                    MessageBox.Show("Broken" + t.ToString());
                                    Log.WriteLog(t.ToString());
                                    continue;
                                }
                            }//end of if-else
                        }
                        else
                        {
                            MessageBox.Show("No Such TV Show On Server");
                        }

                    }//end of for loop
                    //MessageBox.Show("Moved");
                }
                else
                {
                    MessageBox.Show("No Folder Selected For Videos to Be Moved To");
                }
            }
            else
            {//catch if nothing is selected
                MessageBox.Show("No Files Selected");
            }

        }//end of move button method

        //copy button
        private void button2_Click(object sender, EventArgs e)
        {
            if (fileName.Count != 0) //if files are selected
            {
                if (movefolder.Count() != 0)
                {
                    //move crap goes here
                    List<string> folderlist = folderFinder(movefolder);
                    List<string> info = new List<string>();

                    for (int z = 0; z < fileName.Count; z++)
                    {
                        string fullFileName = fileFolder[z] + "\\" + fileName[z];
                        info = infoFinder(fullFileName, folderlist, movefolder);
                        int index = Convert.ToInt32(info[2]);

                        if (info[0] != "no folder")
                        {
                            if (index == -1)
                            {
                                MessageBox.Show("Folder List is Wrong");
                                return;
                            }
                            if (info[1] != "0")
                            {
                                if (!(File.Exists(movefolder[index] + "\\" + info[0] + "\\Season " + info[1])))
                                {
                                    System.IO.Directory.CreateDirectory(movefolder[index] + "\\" + info[0] + "\\Season " + info[1]);
                                    //MessageBox.Show((movefolder + "\\" + info[0] + "\\Season " + info[1] + "\\" + multselct2[z]));
                                    try
                                    {
                                        //System.IO.File.Move(multselct[z], (movefolder[index] + "\\" + info[0] + "\\Season " + info[1] + "\\" + multselct2[z]));                                        
                                        FileSystem.CopyFile(fullFileName, (movefolder[index] + "\\" + info[0] + "\\Season " + info[1] + "\\" + fileName[z]), UIOption.AllDialogs);
                                        Log.moveWriteLog(fullFileName, (movefolder[index] + "\\" + info[0] + "\\Season " + info[1] + "\\"));
                                        //clear stuff
                                        fileFolder[z] = (movefolder[index] + "\\" + info[0] + "\\Season " + info[1]);

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
                                        MessageBox.Show("Broken\n" + t.ToString());
                                        Log.WriteLog(t.ToString());
                                        continue;
                                    }
                                }
                            }
                            else//if no season is selected 
                            {
                                try
                                {
                                    //System.IO.File.Move(multselct[z], (movefolder[index] + "\\" + info[0] + "\\" + multselct2[z]));
                                    FileSystem.CopyFile(fullFileName, (movefolder[index] + "\\" + info[0] + "\\" + fileName[z]), UIOption.AllDialogs);
                                    Log.moveWriteLog(fullFileName, (movefolder[index] + "\\" + info[0]));
                                    fileFolder[z] = (movefolder[index] + "\\" + info[0]);
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
                                    MessageBox.Show("Broken" + t.ToString());
                                    Log.WriteLog(t.ToString());
                                    continue;
                                }
                            }//end of if-else
                        }
                        else
                        {
                            MessageBox.Show("No Such TV Show On Server");
                        }

                    }//end of for loop
                    //MessageBox.Show("Moved");
                }
                else
                {
                    MessageBox.Show("No Folder Selected For Videos to Be Moved To");
                }
            }
            else
            {//catch if nothing is selected
                MessageBox.Show("No Files Selected");
            }
        }//end of copy button method

        //save button
        private void button5_Click(object sender, EventArgs e)
        {
            //Rename the Files
            if (fileName.Count!=0) //if files are selected
            {
            for (int y = 0; y < fileName.Count(); y++) { 
            try
            {
                System.IO.File.Move((fileFolder[y] + "\\" + fileName[y]), (fileFolder[y] + "\\" + newFileName[y]));
                fileName[y] = newFileName[y];
                dataGridView1.Rows[y].Cells[0].Value = fileName[y];
                
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File have been changed or moved \n" + (fileFolder[y] + "\\" + fileName[y]) + "\n" + (fileFolder[y] + "\\" + newFileName[y]));
                continue;
            }
            catch (IOException)
            {
                MessageBox.Show("File already exists or is in use\n" + (fileFolder[y] + "\\" + fileName[y]) + "\n" + (fileFolder[y] + "\\" + newFileName[y]));
                continue;
            }
            }//end of for loop
            }
            else
            {//catch if nothing is selected
                MessageBox.Show("No Files Selected");
            }
        }
        
        //Convert button pressed
        private void button3_Click(object sender, EventArgs e)
        {
            //run new autoConvert
            if (fileName.Count() != 0) {
                Thread t = new Thread(new ThreadStart(autoConvert));
                t.Start();            
            } else {
                MessageBox.Show("No Files Selected");
            
            }
            //bool test = autoConvert();
            //if (!test) //if files are selected
            //{//catch if nothing is selected
             //   MessageBox.Show("No Files Selected");
           // }

        }//end of Convert button pressed

        //Undo Button
        private void button4_Click(object sender, EventArgs e)
        {
            if (fileName.Count != 0) //if files are selected
            {
                for (int z = 0; z < fileName.Count; z++)
                {
                    //call fileRenamer methoid
                    newFileName[z] = fileName[z];
                    dataGridView1.Rows[z].Cells[1].Value = fileName[z];

                }//end of for loop 
                //show what has been converted
            }
            else
            {//catch if nothing is selected
                MessageBox.Show("No Files Selected");
            }

           
        }//end of undo button
        
        #endregion

        #region Update Stuff
        //check to see if internet is avilible
        bool ConnectionExists()
        {
            try
            {
                System.Net.Sockets.TcpClient clnt = new System.Net.Sockets.TcpClient("www.google.com", 80);
                clnt.Close();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }//end of ConnectionExists class

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
                    webClient.DownloadFileAsync(new Uri("http://update.scottnation.com/TV_Show_Renamer/webversion.xml"), commonAppData + "\\webversion.xml");

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
        }

        //runs when xml file is done downloading
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            //MessageBox.Show("Download completed!");
            List<string> webInfo = this.updateXmlRead();//read file
            List<string> localInfo = this.localXmlRead();
            if (Convert.ToInt32(webInfo[0]) > Convert.ToInt32(localInfo[0]))
            {
                //global update crap
                if (MessageBox.Show("There is an update available, Would you like to update?\nNOTE: This will reinstall the program", "Update available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.fullUpdate();
                }
                else
                {
                    if (Convert.ToInt32(webInfo[1]) > Convert.ToInt32(localInfo[1]))
                    {
                        //libaray update crap
                        if (MessageBox.Show("There is a libaray update available, Would you like to update?\nNOTE: This will just replace certain files", "Update available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.libarayUpdate();
                        }
                    }
                }
            }
            else if (Convert.ToInt32(webInfo[1]) > Convert.ToInt32(localInfo[1]))
            {
                //libaray update crap
                if (MessageBox.Show("There is a libaray update available, Would you like to update?\nNOTE: This will just replace certain files", "Update available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.libarayUpdate();
                }
            }
            else
            {
                //no updats available
                MessageBox.Show("No updates available");
            }
        }

        //get info off internet
        public List<String> updateXmlRead()
        {
            string document = commonAppData + "//webversion.xml";
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
                            {
                                data.Add(xmlReader.ReadString());
                            }
                            if (xmlReader.Name == "library")
                            {
                                data.Add(xmlReader.ReadString());
                            }
                            break;
                        }//end of case 
                }//end of switch
            }//end of while loop
            return data;

        }//end of WebXMLReader Method

        //read local info
        public List<String> localXmlRead()
        {
            string document = commonAppData + "//version.xml";
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
                            {
                                data.Add(xmlReader.ReadString());
                            }
                            if (xmlReader.Name == "library")
                            {
                                data.Add(xmlReader.ReadString());
                            }
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

                if (File.Exists(commonAppData + "//library.seh"))
                {
                    File.Delete(commonAppData + "//library.seh");
                }
                WebClient webClient2 = new WebClient();
                webClient2.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed2);
                webClient2.DownloadFileAsync(new Uri("http://update.scottnation.com/TV_Show_Renamer/library.seh"), commonAppData + "\\library.seh");
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
            download update = new download(commonAppData, this);
            //update.Show();

            this.Hide();
        }

        #endregion

        #region Other Methods

        //new convert method
        public void autoConvert()
        {

            if (fileName.Count != 0) //if files are selected
            {
                for (int z = 0; z < fileName.Count; z++)
                {
                    //call fileRenamer methoid
                    newFileName[z] = this.fileRenamer(fileName[z], z, fileExtention[z]);
                    //dataGridView1.Rows[z].Cells[1].Value = newFileName[z];                    
                }//end of for loop

                MethodInvoker action = delegate
                {
                    for (int z = 0; z < fileName.Count; z++)
                    {
                        dataGridView1.Rows[z].Cells[1].Value = newFileName[z];
                    }
                };
                dataGridView1.BeginInvoke(action);
            }
        }//end of convert method

        //read settings file
        public void preferenceXmlRead()
        {
            try
            {
                if (File.Exists(commonAppData + "//preferences.seh"))
                {
                    StreamReader tr3 = new StreamReader(commonAppData + "//preferences.seh");
                    this.convertToolStripMenuItem.Checked = bool.Parse(tr3.ReadLine());
                    this.convertToToolStripMenuItem.Checked = bool.Parse(tr3.ReadLine());
                    this.removeToolStripMenuItem.Checked = bool.Parse(tr3.ReadLine());
                    this.capitalizeToolStripMenuItem.Checked = bool.Parse(tr3.ReadLine());
                    this.removeExtraCrapToolStripMenuItem.Checked = bool.Parse(tr3.ReadLine());
                    this.addForTitleToolStripMenuItem.Checked = bool.Parse(tr3.ReadLine());
                    this.x01ToolStripMenuItem.Checked = bool.Parse(tr3.ReadLine());
                    this.toolStripMenuItem3.Checked = bool.Parse(tr3.ReadLine());
                    this.s01E01ToolStripMenuItem1.Checked = bool.Parse(tr3.ReadLine());
                    this.dateToolStripMenuItem.Checked = bool.Parse(tr3.ReadLine());
                    this.removeYearToolStripMenuItem.Checked = bool.Parse(tr3.ReadLine());

                    this.BackColor=System.Drawing.Color.FromArgb(int.Parse(tr3.ReadLine()), int.Parse(tr3.ReadLine()), int.Parse(tr3.ReadLine()), int.Parse(tr3.ReadLine()));

                    this.ForeColor = System.Drawing.Color.FromArgb(int.Parse(tr3.ReadLine()), int.Parse(tr3.ReadLine()), int.Parse(tr3.ReadLine()), int.Parse(tr3.ReadLine()));

                    //this.BackColor.B = int.Parse(tr3.ReadLine());
                    this.menuStrip1.BackColor = this.BackColor;
                    this.menuStrip1.ForeColor = this.ForeColor;
                    //movefolder = tr3.ReadLine();
                    openZIPs = bool.Parse(tr3.ReadLine());

                    tr3.Close();//close reader stream    
                }//end of if. 
                if (File.Exists(commonAppData + "//TVFolder.seh"))
                {
                    StreamReader tv2 = new StreamReader(commonAppData + "//TVFolder.seh");
                    int length = Int32.Parse(tv2.ReadLine());
                    for (int i = 0; i < length; i++)
                    {
                        if (length == 0)
                        {
                            break;
                        }
                        movefolder.Add(tv2.ReadLine());
                    }//end of for loop  
                    tv2.Close();
                }//end of if
            }
            catch (Exception e)
            {
                Log.WriteLog("Writing Preference Error" + e.ToString());
            }

        }//end of preferenceXMLReader Method

        //returns string list of info
        private List<string> infoFinder(string oldfile, List<string> folderlist, List<string> rootfolderlist)
        {
            string fileName = lowering(oldfile);
            List<string> stuff = new List<string>();

            string infoChanged = fileName;
            int indexof = 0;
            stuff.Add("no folder");
            stuff.Add("0");
            stuff.Add("-1");

            //figure out if tv show is listed
            for (int i = 0; i < folderlist.Count(); i++)
            {

                string newFolderEdited = lowering(folderlist[i]);

                infoChanged = fileName.Replace(newFolderEdited, "0000");
                if (infoChanged != fileName)
                {
                    stuff[0] = folderlist[i];
                    indexof = i;

                    //figure out root folder
                    string filenameraw = folderlist[indexof - 1];
                    string index = filenameraw.Replace(folderlist[indexof] + "  ", "");
                    stuff[2] = index;

                    break;
                }
            }//end of for loop              

            //loop for seasons
            for (int i = 1; i < 40; i++)
            {
                //varable for break command later
                bool end = false;

                //loop for episodes
                for (int j = 1; j < 100; j++)
                {
                    string newi = i.ToString();
                    string newj = j.ToString();
                    //string output = null;
                    //check if i is less than 10
                    if (i < 10)
                    {
                        newi = "0" + i.ToString();
                    }
                    //check if j is less than 10
                    if (j < 10)
                    {
                        newj = "0" + j.ToString();
                    }

                    //make string to compare changed name too
                    string startnewname = fileName;

                    //1x01 format 
                    if (x01ToolStripMenuItem.Checked)
                    {
                        fileName = fileName.Replace(i.ToString() + "x" + newj, "00000");//1x01 add title
                    }
                    //0101 format
                    if (toolStripMenuItem3.Checked)
                    {
                        fileName = fileName.Replace(newi + newj, "00000");//0101 add title
                    }
                    //S01E01 format
                    if (s01E01ToolStripMenuItem1.Checked)
                    {
                        fileName = fileName.Replace("S" + newi + "E" + newj, "00000");//S01E01 add title
                        fileName = fileName.Replace("S" + newi + "e" + newj, "00000");//S01E01 add title if second time
                    }

                    //stop loop when name is change                    
                    if (startnewname != fileName)
                    {
                        stuff[1] = (i.ToString());
                        end = true;
                        break;
                    }
                }//end of episode loop

                //stop loop when name is change
                if (end)
                {
                    break;
                }

            }//end of season loop
            return stuff;
        }//end of infofinder method

        //write log called by download form
        public void writeLog(string error)
        {
            Log.WriteLog(error);
        }

        //get list of folders
        private List<string> folderFinder(List<String> folderwatch)
        {
            List<String> foldersIn = new List<String>();
            List<String> revFoldersIn = new List<String>();
            for (int u = 0; u < folderwatch.Count(); u++)
            {

                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folderwatch[u]);
                foreach (System.IO.DirectoryInfo fi in di.GetDirectories())
                {
                    foldersIn.Add(fi.Name);
                    foldersIn.Add(fi.Name + "  " + u.ToString());//add name and folder number so it will be sorted correctly                    
                }
            }
            //Sort folders
            foldersIn.Sort();
            for (int y = foldersIn.Count(); y > 0; y--)
            {
                revFoldersIn.Add(foldersIn[y - 1]);
            }
            //Display box7 = new Display(revFoldersIn);
            //box7.Show();

            return revFoldersIn;
        }

        //lowercase stuff
        private string lowering(string orig)
        {
            //make every thing lowercase for crap remover to work
            StringBuilder s = new StringBuilder(orig);
            for (int l = 0; l < orig.Length; l++)
            {
                s[l] = char.ToLower(s[l]);
            }
            //reassign edited name 
            return s.ToString();
        }

        //new way to add files from folder
        public void ProcessDir(string sourceDir, int recursionLvl)
        {
            if (recursionLvl <= HowDeepToScan)
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sourceDir);
                // Process the list of files found in the directory.
                //string[] fileEntries = Directory.GetFiles(sourceDir);
                foreach (System.IO.FileInfo fi in di.GetFiles("*"))
                {
                    string origName = fi.Name;
                    string exten = fi.Extension;
                    string attrib = fi.Attributes.ToString();

                    if (attrib == "Hidden, System, Archive")
                    {
                        continue;
                    }
                    //if thumb file dont convert
                    if (origName == "Thumbs.db")
                    {
                        continue;
                    }

                    //zip fix
                    if ((exten == ".zip" || exten == ".rar" || exten == ".r01")&&openZIPs)
                    {
                        continue;
                    }

                    //check if its a legal file type
                    if (!(exten == ".avi" || exten == ".mkv" || exten == ".mp4" || exten == ".mpg" || exten == ".m4v" ))
                    {
                        //if dialog was shown b4 dont show again
                        if (!shownb4)
                        {
                            if (MessageBox.Show("You have selected a folder with files that aren't Media Files\nWould you like to add them?", "Media Options", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                //state if files should be added
                                shownb4 = true;
                                addfile = true;
                            }
                            else
                            {
                                //state if files should not be added
                                shownb4 = true;
                                addfile = false;
                                continue;
                            }
                        }
                        //dont add files if not true
                        if (!addfile)
                        {
                            continue;
                        }
                    }

                    
                    //add file name
                    fileName.Add(origName);
                    newFileName.Add(origName);
                    //add file folder
                    fileFolder.Add(fi.DirectoryName);
                    //add file extension
                    fileExtention.Add(exten);
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

        public void ProcessDirZIP(string sourceDir)
        {

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sourceDir);
            // Process the list of files found in the directory.
            //string[] fileEntries = Directory.GetFiles(sourceDir);
            foreach (System.IO.FileInfo fi in di.GetFiles("*"))
            {
                string origName = fi.Name;
                string exten = fi.Extension;
                string attrib = fi.Attributes.ToString();
                //MessageBox.Show(fi.FullName);

                
                //if thumb file dont convert
                if (origName == "Thumbs.db")
                {
                    continue;
                }

                //check if its a legal file type
                if (exten == ".zip" || exten == ".rar" || exten == ".r01")
                {
                    List<string> test = archiveExtrector(fi.FullName);

                }

            }
        }
        //rename method
        private string fileRenamer(string newfilename, int index, string extend)
        {
            //make temp function
            string temp = null;
            if (convertToolStripMenuItem.Checked)
            {
                temp = " ";
            }
            else
            {
                temp = ".";
            }//end of if-else

            //remove extention
            newfilename = newfilename.Replace(extend, temp + "&&&&");

            //Text converter
            List<string> testConverter = new List<string>();
            testConverter = textConvert.getText();

            for (int x = 0; x < testConverter.Count(); x += 2)
            {
                newfilename = newfilename.Replace(testConverter[x], testConverter[x + 1]);
            }//end of for

            //user junk list
            if (removeExtraCrapToolStripMenuItem.Checked)
            {
                //make user junk list
                List<string> userjunklist = new List<string>();
                userjunklist = userJunk.getjunk();
                if (userjunklist.Count() != 0)
                {
                    for (int x = 0; x < userjunklist.Count(); x++)
                    {
                        newfilename = newfilename.Replace(userjunklist[x], "");
                    }//end of for
                }//end of if
            }//end of removeExtraCrapToolStripMenuItem if


            //replace periods(".") with spaces 
            if (convertToolStripMenuItem.Checked)
            {
                newfilename = newfilename.Replace(".", temp);
            }

            //Replace "_" with spaces
            if (convertToToolStripMenuItem.Checked)
            {
                newfilename = newfilename.Replace("_", temp);
            }

            //Replace "-" with spaces
            if (removeToolStripMenuItem.Checked)
            {
                newfilename = newfilename.Replace("-", temp);
            }

            //make every thing lowercase for crap remover to work
            StringBuilder s = new StringBuilder(newfilename);
            for (int l = 0; l < newfilename.Length; l++)
            {
                s[l] = char.ToLower(s[l]);
            }

            //reassign edited name 
            newfilename = s.ToString();

            //remove extra crap 
            if (removeExtraCrapToolStripMenuItem.Checked)
            {
                //new way with file input
                for (int x = 0; x < junklist.Count(); x++)
                {
                    newfilename = newfilename.Replace(junklist[x], "");
                }//end of for
            }//end of removeExtraCrapToolStripMenuItem if


            //remove begining space
            StringBuilder space = new StringBuilder(newfilename);
            if (space[0] == ' ')
            {
                for (int p = 0; p < (newfilename.Length - 1); p++)
                {
                    space[p] = space[p + 1];
                }
            }

            //reassign edited name 
            newfilename = space.ToString();
            newfilename = newfilename.Replace("&&&&&", "&&&&");//fix that i hope works


            //remove year function
            if (removeYearToolStripMenuItem.Checked && (!dateToolStripMenuItem.Checked))
            {
                int curyear = System.DateTime.Now.Year;
                for (; curyear > 1980; curyear--)
                {
                    string before = newfilename;
                    newfilename = newfilename.Replace(curyear.ToString(), "");
                    //break if value found
                    if (before != newfilename)
                    {
                        break;
                    }
                }//emd of for loop
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


            //Capitalize Function
            if (capitalizeToolStripMenuItem.Checked)
            {
                StringBuilder s2 = new StringBuilder(newfilename);
                int size3 = newfilename.Length;

                //make first letter capital
                char strTemp = char.ToUpper(s2[0]);
                s2[0] = strTemp;

                //Finds Letter after spaces and capitalizes them
                for (int i = 2; i < size3 - 4; i++)
                {
                    if (s2[i] == ' ' || s2[i] == '.')
                    {
                        char strTemp2 = char.ToUpper(s2[i + 1]);
                        s2[i + 1] = strTemp2;
                    }
                }//end of for loop

                //adds changes to newfilename
                newfilename = s2.ToString();

            }//end of capilization

            //add dash if the title exists or add one 
            string tempTitle = null;
            if (convertToolStripMenuItem.Checked && addForTitleToolStripMenuItem.Checked)
            {
                bool titleAvil = titles.checkTitle(index);
                if (titleAvil)
                {
                    string newTitle = titles.getTitle(index);
                    tempTitle = " - " + newTitle;
                }
                else
                {
                    tempTitle = " -";
                }//end of nested if-else
            }
            else
            {
                tempTitle = "";
            }//end of if-else

            //loop for seasons
            for (int i = 1; i < 40; i++)
            {
                //varable for break command later
                bool end = false;

                //loop for episodes
                for (int j = 1; j < 100; j++)
                {
                    string newi = i.ToString();
                    string newj = j.ToString();
                    string output = null;
                    //check if i is less than 10
                    if (i < 10)
                    {
                        newi = "0" + i.ToString();
                    }
                    //check if j is less than 10
                    if (j < 10)
                    {
                        newj = "0" + j.ToString();
                    }

                    //make string to compare changed name too
                    string startnewname = newfilename;

                    //1x01 format 
                    if (x01ToolStripMenuItem.Checked)
                    {
                        output = i.ToString() + "x" + newj;

                        newfilename = newfilename.Replace(temp + i.ToString() + newj + temp, temp + output + temp);//101
                        newfilename = newfilename.Replace(newi + newj, output);//0101
                        newfilename = newfilename.Replace("S" + i.ToString() + "e" + j.ToString() + temp, output + temp);//S1e1
                        newfilename = newfilename.Replace("S" + i.ToString() + "e" + newj + temp, output + temp);//S1e01
                        newfilename = newfilename.Replace("S" + newi + "e" + newj, output);//S01E01
                        newfilename = newfilename.Replace("S" + newi + " E" + newj, output);//S01 E01
                        newfilename = newfilename.Replace("Season " + i.ToString() + " Episode " + newj + temp, output);//Season 1 Episode 01
                        newfilename = newfilename.Replace("Season " + i.ToString() + " Episode " + j.ToString() + temp, output);//Season 1 Episode 1
                        newfilename = newfilename.Replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);//1 01
                        newfilename = newfilename.Replace("0" + output, output);//01x01 fix might be unnessitarry
                        newfilename = newfilename.Replace(output, output + tempTitle);//1x01 add title
                    }
                    //0101 format
                    if (toolStripMenuItem3.Checked)
                    {
                        output = newi + newj;

                        newfilename = newfilename.Replace(temp + i.ToString() + newj + temp, temp + output + temp);//101 
                        newfilename = newfilename.Replace(temp + i.ToString() + "x" + newj, temp + output);//1x01
                        newfilename = newfilename.Replace("S" + newi + "e" + newj, output);//S01E01
                        newfilename = newfilename.Replace("S" + newi + " E" + newj, output);//S01 E01
                        newfilename = newfilename.Replace("S" + i.ToString() + "e" + j.ToString() + temp, output + temp);//S1e1
                        newfilename = newfilename.Replace("S" + i.ToString() + "e" + newj + temp, output + temp);//S1e01
                        newfilename = newfilename.Replace("Season " + i.ToString() + " Episode " + newj + temp, output);//Season 1 Episode 01
                        newfilename = newfilename.Replace("Season " + i.ToString() + " Episode " + j.ToString() + temp, output);//Season 1 Episode 1
                        newfilename = newfilename.Replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);// 1 01 
                        newfilename = newfilename.Replace(output, output + tempTitle);//0101 add title
                    }
                    //S01E01 format
                    if (s01E01ToolStripMenuItem1.Checked)
                    {
                        output = "S" + newi + "E" + newj;

                        newfilename = newfilename.Replace(temp + i.ToString() + newj + temp, temp + output + temp);//101
                        newfilename = newfilename.Replace(temp + i.ToString() + "x" + newj, temp + output);//1x01
                        newfilename = newfilename.Replace(newi + newj, output);//0101
                        newfilename = newfilename.Replace("S" + i.ToString() + "e" + j.ToString() + temp, output + temp);//s1e1
                        newfilename = newfilename.Replace("S" + i.ToString() + "e" + newj + temp, output + temp);//S1e01
                        newfilename = newfilename.Replace("S" + newi + " E" + newj, output);//S01 E01
                        newfilename = newfilename.Replace("Season " + i.ToString() + " Episode " + newj + temp, output);//Season 1 Episode 01
                        newfilename = newfilename.Replace("Season " + i.ToString() + " Episode " + j.ToString() + temp, output);//Season 1 Episode 1
                        newfilename = newfilename.Replace(temp + i.ToString() + temp + newj + temp, temp + output + temp);//1 01
                        newfilename = newfilename.Replace("S" + newi + "E" + newj, output + tempTitle);//S01E01 add title
                        newfilename = newfilename.Replace("S" + newi + "e" + newj, output + tempTitle);//S01E01 add title if second time
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
                {
                    break;
                }

            }//end of season loop


            //Date format
            if (dateToolStripMenuItem.Checked)
            {
                //add dash if the title exists or add one 
                string dateTitle = null;
                if (convertToolStripMenuItem.Checked && addForTitleToolStripMenuItem.Checked)
                {
                    bool titleAvil = titles.checkTitle(index);
                    if (titleAvil)
                    {
                        string newTitle = titles.getTitle(index);
                        dateTitle = " - " + newTitle;
                    }
                    else
                    {
                        dateTitle = " -";
                    }//end of nested if-else
                }
                else
                {
                    dateTitle = "";
                }//end of if-else

                for (int year = 0; year < 20; year++)
                {
                    for (int month = 1; month < 13; month++)
                    {
                        for (int day = 1; day < 32; day++)
                        {
                            string newyear = year.ToString();
                            string newmonth = month.ToString();
                            string newday = day.ToString();

                            //check if i is less than 10
                            if (year < 10)
                            {
                                newyear = "0" + year.ToString();
                            }
                            //check if j is less than 10
                            if (month < 10)
                            {
                                newmonth = "0" + month.ToString();
                            }
                            //check if k is less than 10
                            if (day < 10)
                            {
                                newday = "0" + day.ToString();
                            }
                            string kk = "20" + newyear;

                            newfilename = newfilename.Replace(kk + " " + newmonth + " " + newday, month + "-" + day + "-" + kk + dateTitle);
                            newfilename = newfilename.Replace(month + " " + day + " " + kk, month + "-" + day + "-" + kk + dateTitle);//add title

                        }//end of for loop day
                    }//end of for loop month
                }//end of for loop year

            }//end of if for date check box

            //add file extention back on 
            newfilename = newfilename.Replace(temp + "&&&&", extend);

            //Random fixes

            //newfilename = newfilename.Replace("-.", ".");
            newfilename = newfilename.Replace(" .", ".");
            newfilename = newfilename.Replace("- -", "-");
            newfilename = newfilename.Replace(".-.", ".");
            newfilename = newfilename.Replace("-.", ".");
            newfilename = newfilename.Replace(" .", ".");
            newfilename = newfilename.Replace("Vs", "vs");
            newfilename = newfilename.Replace("O C ", "O.C. ");
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
            newfilename = newfilename.Replace("Iii", "III");
            newfilename = newfilename.Replace("Ii", "II");

            // return converted file name
            return newfilename;
        }//end of file rename function

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
            startlist.Add("[vtv]");
            startlist.Add("[eztv]");
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
            startlist.Add("(the real deal)");
            startlist.Add("repack");
            startlist.Add("ac3");
            startlist.Add("mvgroup.org");
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
            startlist.Add("[cac]");
            startlist.Add("[clarkadamc]");
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
            startlist.Add("agd");
            startlist.Add("gnarly");
            startlist.Add("krs");
            startlist.Add("[goat]");
            startlist.Add("[buck]");
            startlist.Add("[bucktv]");
            startlist.Add("orpheus");
            startlist.Add("720p");
            startlist.Add("x264");
            startlist.Add("dimension");
            startlist.Add("60fps");
            startlist.Add("d734");
            startlist.Add("mspaint");
            startlist.Add("siso");
            
            startlist.Add("www.directlinkspot.com");
            startlist.Add("www directlinkspot com");
            startlist.Add("onelinkmoviez.com");
            startlist.Add("onelinkmoviez com");
            startlist.Add("tv");

            //if no file exist make a default file
            if (!File.Exists(commonAppData + "//library.seh"))
            {
                StreamWriter sw = new StreamWriter(commonAppData + "//library.seh");
                sw.WriteLine(startlist.Count());
                for (int j = 0; j < startlist.Count(); j++)
                {
                    sw.WriteLine(startlist[j]);
                }//end of for
                sw.Close();//close writer stream
            }
            else
            {//check to see if this is the newest file
                StreamReader tr2 = new StreamReader(commonAppData + "//library.seh");
                int size2 = Int32.Parse(tr2.ReadLine());//read number of lines
                tr2.Close();//close reader stream
                if (startlist.Count() > size2)
                {//replace if array is bigger than file
                    StreamWriter sw = new StreamWriter(commonAppData + "//library.seh");
                    sw.WriteLine(startlist.Count());
                    for (int j = 0; j < startlist.Count(); j++)
                    {
                        sw.WriteLine(startlist[j]);
                    }//end of for
                    sw.Close();//close writer stream
                }//end of if

            }//end of if


            //read junk file 
            StreamReader tr = new StreamReader(commonAppData + "//library.seh");

            junklist.Clear();//clear old list

            int size = Int32.Parse(tr.ReadLine());//read number of lines

            for (int i = 0; i < size; i++)
            {
                junklist.Add(tr.ReadLine());
            }//end of for

            tr.Close();//close reader stream

        }//end of junk remover

        //This XmlWrite method creates a new XML File
        private void XmlWrite()
        {
            //get size of library file
            StreamReader tr = new StreamReader(commonAppData + "//library.seh");
            int size = Int32.Parse(tr.ReadLine());//read number of lines
            tr.Close();//close reader stream

            XmlTextWriter xmlWriter = new XmlTextWriter(commonAppData + "//version.xml", null);
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
            if (!File.Exists(commonAppData + "//version.xml"))
            {
                this.XmlWrite();
            }
            else
            {
                //check to see if version.xml is the newest
                string document = commonAppData + "//version.xml";
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
                                {
                                    fileVer = Convert.ToInt32(xmlReader.ReadString());
                                }
                                if (xmlReader.Name == "library")
                                {
                                    libVer = Convert.ToInt32(xmlReader.ReadString());
                                }
                                break;
                            }//end of case 
                    }//end of switch
                }//end of while loop
                myxmlDocument.RemoveAll();
                xmlReader.Close();
                //if apps info is newer write new file                
                if (appVersion > fileVer)
                {
                    this.XmlWrite();
                }
                //if library is bigger rewrite info file
                StreamReader tr2 = new StreamReader(commonAppData + "//library.seh");
                int size2 = Int32.Parse(tr2.ReadLine());//read number of lines
                tr2.Close();//close reader stream
                if (size2 > libVer)
                {
                    this.XmlWrite();
                }
            }//end of if-else
        }

        //function to return list
        public void tvFolderChanger(List<string> sentlist)
        {
            movefolder = sentlist;
        }

        //opens zip and rar folders
        private List<string> archiveExtrector(string zipfile) {
            
            List<string> info = new List<string>();
            info.Add("0");
            info.Add("0");
            info.Add("0");
            string archiveName = null;
            int archiveIndex= -1;
            FileInfo fi8 = new FileInfo(zipfile);
            
            SevenZipExtractor mainExtrector = new SevenZipExtractor(zipfile);
            int sizeOfArchive = (int)mainExtrector.FilesCount;
            for (int j = 0; j < sizeOfArchive; j++)
            {
                archiveName = mainExtrector.ArchiveFileNames[j];
                string testArchiveName = archiveName.Replace(".avi", "0000").Replace(".mkv", "0000").Replace(".mp4", "0000").Replace(".m4v", "0000").Replace(".mpg", "0000");
                
                if (testArchiveName != archiveName)
                {
                    archiveIndex = j;
                    break;
                }
            }

            if (archiveIndex == -1) {
                return info;
            }

            mainExtrector.ExtractFile(archiveIndex, File.Create(fi8.DirectoryName + "\\" + archiveName));

            FileInfo fi9 = new FileInfo(fi8.DirectoryName + "\\" + archiveName);
            info[0] = fi9.Name;
            info[1] = fi9.DirectoryName;
            info[2] = fi9.Extension;
                   
            //return info string
            return info;
        }

        private void getFiles(string[] fileList) {
            
                //loop for each file in array
            foreach (String file3 in fileList)
                {
                    FileInfo fi3 = new FileInfo(file3);
                    if (fi3.Extension == ".avi" || fi3.Extension == ".mkv" || fi3.Extension == ".mp4" || fi3.Extension == ".m4v" || fi3.Extension == ".mpg")
                    {
                        //add file name
                        fileName.Add(fi3.Name);
                        newFileName.Add(fi3.Name);
                        //add file folder
                        fileFolder.Add(fi3.DirectoryName);
                        //add file extension
                        fileExtention.Add(fi3.Extension);
                    }

                    if (fi3.Extension == ".zip" || fi3.Extension == ".rar" || fi3.Extension == ".r01" || fi3.Extension == ".001")
                    {
                        List<string> info = archiveExtrector(file3);

                        if (info[0] == "0")
                        {
                            continue;
                        }
                        //add file name
                        fileName.Add(info[0]);
                        newFileName.Add(info[0]);
                        //add file folder
                        fileFolder.Add(info[1]);
                        //add file extension
                        fileExtention.Add(info[2]);
                    }


                }//end of loop 
                /*dataGridView1.Rows.Clear();
                for (int i = 0; i < fileName.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = fileName[i];
                    dataGridView1.Rows[i].Cells[1].Value = fileName[i];
                }*/

            MethodInvoker action2 = delegate
            {
                dataGridView1.Rows.Clear();
                for (int z = 0; z < fileName.Count; z++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[z].Cells[0].Value = fileName[z];
                    dataGridView1.Rows[z].Cells[1].Value = fileName[z];
                }
            };
            dataGridView1.BeginInvoke(action2);


                if (fileName.Count() != 0)
                {
                    Thread t = new Thread(new ThreadStart(autoConvert));
                    t.Start();
                }

            }

        private void getFilesInFolder(string folder) {

            if (openZIPs) {
                ProcessDirZIP(folder);
            }

            ProcessDir(folder, 0);

            
            /*dataGridView1.Rows.Clear();
               for (int i = 0; i < fileName.Count(); i++)
               {
                   dataGridView1.Rows.Add();
                   dataGridView1.Rows[i].Cells[0].Value = fileName[i];
                   dataGridView1.Rows[i].Cells[1].Value = fileName[i];
               }*/

            MethodInvoker action2 = delegate
            {
                dataGridView1.Rows.Clear();
                for (int z = 0; z < fileName.Count; z++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[z].Cells[0].Value = fileName[z];
                    dataGridView1.Rows[z].Cells[1].Value = fileName[z];
                }
            };
            dataGridView1.BeginInvoke(action2);

            if (fileName.Count() != 0)
            {
                Thread t = new Thread(new ThreadStart(autoConvert));
                t.Start();
            }
        
        
        }

        public void changeZIPstate(bool localZIP) {
            openZIPs = localZIP;
        }
        
        #endregion

        //loads when starts
        private void Form1_Load(object sender, EventArgs e)
        {

            if (!(File.Exists(commonAppData)))
            {
                System.IO.Directory.CreateDirectory(commonAppData);
            }
            //movefolder = "0000";
            Log.startLog(commonAppData);
            this.preferenceXmlRead();
            this.junkRemover();
            this.fileChecker();
            
            userJunk.junk_adder(junklist, commonAppData, this);
            textConvert.setUp(this);

            //MessageBox.Show(movefolder);

        }//end of load command

        //create preference file when program closes and close log
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StreamWriter pw = new StreamWriter(commonAppData + "//preferences.seh");

            pw.WriteLine(convertToolStripMenuItem.Checked);
            pw.WriteLine(convertToToolStripMenuItem.Checked);
            pw.WriteLine(removeToolStripMenuItem.Checked);
            pw.WriteLine(capitalizeToolStripMenuItem.Checked);
            pw.WriteLine(removeExtraCrapToolStripMenuItem.Checked);
            pw.WriteLine(addForTitleToolStripMenuItem.Checked);
            pw.WriteLine(x01ToolStripMenuItem.Checked);
            pw.WriteLine(toolStripMenuItem3.Checked);
            pw.WriteLine(s01E01ToolStripMenuItem1.Checked);
            pw.WriteLine(dateToolStripMenuItem.Checked);
            pw.WriteLine(removeYearToolStripMenuItem.Checked);
            //pw.WriteLine(movefolder);
            pw.WriteLine(this.BackColor.A);
            pw.WriteLine(this.BackColor.R);
            pw.WriteLine(this.BackColor.G);
            pw.WriteLine(this.BackColor.B);
            pw.WriteLine(this.ForeColor.A);
            pw.WriteLine(this.ForeColor.R);
            pw.WriteLine(this.ForeColor.G);
            pw.WriteLine(this.ForeColor.B);
            pw.WriteLine(openZIPs);
            pw.Close();//close writer stream

            //write tv folder locations
            StreamWriter tv = new StreamWriter(commonAppData + "//TVFolder.seh");
            tv.WriteLine(movefolder.Count());
            for (int i = 0; i < movefolder.Count(); i++)
            {
                tv.WriteLine(movefolder[i]);
            }
            tv.Close();
            //write log
            Log.closeLog();


        }
         

        /*public void XmlRead()
{
    string document = commonAppData + "//version.xml";
    XmlDataDocument myxmlDocument = new XmlDataDocument();
    myxmlDocument.Load(document);
    XmlTextReader xmlReader = new XmlTextReader(document);

    while (xmlReader.Read())
    {
        switch (xmlReader.NodeType)
        {
            case XmlNodeType.Element:
                {
                    string test1 = null;
                    if (xmlReader.Name == "application")
                    {
                        test1 = xmlReader.ReadString();
                    }
                    if (xmlReader.Name == "library")
                    {
                        test1 = xmlReader.ReadString();
                    }
                    if (xmlReader.Name == "settings")
                    {
                        test1 = xmlReader.ReadString();
                    }
                    if (xmlReader.Name == "DateFormat")
                    {
                        test1 = xmlReader.ReadString();
                    }
                    if (xmlReader.Name == "TimeFormat")
                    {
                        test1 = xmlReader.ReadString();
                    }
                    MessageBox.Show(test1);
                    break;
                }//end of case 
        }//end of switch
    }//end of while loop
}//end of XMLReader Method*/

/*message box example
       if (MessageBox.Show("Scott is Awesome?","Option Menu", MessageBoxButtons.YesNo)== DialogResult.Yes)
           {
               MessageBox.Show("Yeah, I know.");
           }
           else
           {
               MessageBox.Show("Well Screw You.");
           }*/

        /*
        // How much deep to scan. (of course you can also pass it to the method)
        const int HowDeepToScan = 4;

        public static void ProcessDir(string sourceDir, int recursionLvl)
        {
            if (recursionLvl <= HowDeepToScan)
            {
                // Process the list of files found in the directory.
                string[] fileEntries = Directory.GetFiles(sourceDir);
                foreach (string fileName in fileEntries)
                {
                    // do something with fileName
                    Console.WriteLine(fileName);
                }

                // Recurse into subdirectories of this directory.
                string[] subdirEntries = Directory.GetDirectories(sourceDir);
                foreach (string subdir in subdirEntries)
                    // Do not iterate through reparse points
                    if ((File.GetAttributes(subdir) &
                         FileAttributes.ReparsePoint) !=
                             FileAttributes.ReparsePoint)

                        ProcessDir(subdir, recursionLvl + 1);
            }
        }*/
                       
    }//end of form 1 partial class
}//end of namespace


