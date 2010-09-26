﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;


namespace TV_show_Renamer
{
    public partial class download : Form
    {
        string commonAppData = null;
        Form1 window;
        public download(string location,Form1 window)
        {
            this.window = window;
            commonAppData = location;
            //this.Show();
            InitializeComponent();
            string path = Directory.GetCurrentDirectory();
        }

        //downlaod file after checks to see if it was there
        public void downloadUpdate(string location2){
            
            label1.Text = location2;
            //check is not working
           /* try
            {
                WebRequest request = WebRequest.Create(new Uri("http://update.scottnation.com/TV_Show_Renamer/TV Show Renamer Setup.msi"));
                request.Method = "HEAD";
                WebResponse response = request.GetResponse();
                Console.WriteLine("{0} {1}", response.ContentLength, response.ContentType);
            }
            catch (Exception)
            {
                MessageBox.Show("Problem with Server\nPlease Contact Admin");
                return;
            }*/
            
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            //webClient.DownloadFileAsync(new Uri("http://update.scottnation.com/TV_Show_Renamer/TV Show Renamer Setup.msi"), location2);
            webClient.DownloadFileAsync(new Uri("http://update.scottnation.com/TV_Show_Renamer/TV show Renamer.exe"), location2 + "//tvshowsenamer.exe");
        }

        //methoid for progress bar
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e){
            progressBar1.Value = e.ProgressPercentage;
            
        }

        //runs when download completes
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {

            try
            {
                if (File.Exists(commonAppData + "//library.seh"))
                {
                    File.Delete(commonAppData + "//library.seh");
                }
            }
            catch (Exception q)
            {
                window.writeLog("Error when deleting files before update" + q.ToString());
            }
            try
            {
                if (File.Exists(commonAppData + "//version.xml"))
                {
                    File.Delete(commonAppData + "//version.xml");
                }
            }
            catch (Exception q)
            {
                window.writeLog("Error when deleting files before update" + q.ToString());
            }
            try
            {
                if (File.Exists(commonAppData + "//webversion.xml"))
                {
                    File.Delete(commonAppData + "//webversion.xml");
                }
            }
            catch (Exception q)
            {
                window.writeLog("Error when deleting files before update" + q.ToString());
            }
            try
            {
                if (File.Exists(commonAppData + "//preferences.seh"))
                {
                    File.Delete(commonAppData + "//preferences.seh");
                }
            }
            catch (Exception q)
            {
                window.writeLog("Error when deleting files before update" + q.ToString());
            }
            //string installdir = Directory.GetCurrentDirectory();
            
            //MessageBox.Show("The uninstaller will run and when it is finished you will have to double click the installer you saved to:\n" + label1.Text);
            //ProcessStartInfo startInfo2 = new ProcessStartInfo("msiexec.exe", "/x {D0DA0E5E-92C4-4A2A-B1BF-EC7077B35217}");
            //Process.Start(startInfo2);
            //MessageBox.Show( commonAppData + "\TV show Renamer.exe " + Directory.GetCurrentDirectory() + "\TV show Renamer.exe");
            //ProcessStartInfo startInfo2 = new ProcessStartInfo(commonAppData + "//test.exe", commonAppData + "//tvshowsenamer.exe " + Directory.GetCurrentDirectory()+"//tvshowrenamer.exe");
            //Process.Start(startInfo2);

            

            //startInfo2.FileName = label1.Text;
            //startInfo.Arguments
            
            //startInfo2.
            //ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.FileName = label1.Text;
            //startInfo.Arguments
            //Process.Start(startInfo);

            //ProcessStartInfo startInfo2 = new ProcessStartInfo(commonAppData + "\\Intraller.exe", label1.Text + " " + installdir);
            //Process.Start(startInfo2);
            //System.Environment.Exit(0);

            //ProcessStartInfo startInfo2 = new ProcessStartInfo(label1.Text);
            //Process.Start(startInfo2);

            Application.Exit();
            //window.Close();
        }

        //loads with form
        private void download_Load(object sender, EventArgs e)
        {
            
            this.Show();
            //string name=null;
            saveFileDialog1.Filter = "Installer (*.msi)|*.msi";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "TV Show Renamer Setup.msi";
            saveFileDialog1.DefaultExt = ".msi";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.Title = "Select Location to Download";

            //if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
                string name = saveFileDialog1.FileName;
                this.downloadUpdate(commonAppData);
            //}         
            
        }

        //cancel button
        private void button1_Click(object sender, EventArgs e)
        {            
            window.Show();            
            this.Close();
        }       

        //run when form closes
        private void download_FormClosed(object sender, FormClosedEventArgs e)
        {
            window.Show();
        }  

    }
}
