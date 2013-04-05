using System;
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

namespace TV_Show_Renamer
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
		}

		//downlaod file after checks to see if it was there
		public void downloadUpdate(string location2)
		{			
			label1.Text = location2;
						
			WebClient webClient = new WebClient();
			webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
			webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
			webClient.DownloadFileAsync(new Uri("http://update.scottnation.com/TV_Show_Renamer/TV Show Renamer Setup.exe"), location2);
			//webClient.DownloadFileAsync(new Uri("http://update.scottnation.com/TV_Show_Renamer/TV show Renamer.exe"), location2 );
		}

		//methoid for progress bar
		private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			progressBar1.Value = e.ProgressPercentage;			
		}

		//runs when download completes
		private void Completed(object sender, AsyncCompletedEventArgs e)
		{
			try
			{
				if (File.Exists(commonAppData + Path.DirectorySeparatorChar + "library.seh"))			   
					File.Delete(commonAppData + Path.DirectorySeparatorChar + "library.seh");				
			}
			catch (Exception q)
			{
				window.writeLog("Error when deleting library.seh before update" + q.ToString());
			}
			try
			{
				if (File.Exists(commonAppData + Path.DirectorySeparatorChar + "version.xml"))
					File.Delete(commonAppData + Path.DirectorySeparatorChar + "version.xml");				
			}
			catch (Exception q)
			{
				window.writeLog("Error when deleting version.xml before update" + q.ToString());
			}
			try
			{
				if (File.Exists(commonAppData + Path.DirectorySeparatorChar + "webversion.xml"))
					File.Delete(commonAppData + Path.DirectorySeparatorChar + "webversion.xml");				
			}
			catch (Exception q)
			{
				window.writeLog("Error when deleting webversion.xml before update" + q.ToString());
			}
			try
			{
				if (File.Exists(commonAppData + Path.DirectorySeparatorChar + "preferences.seh"))
					File.Delete(commonAppData + Path.DirectorySeparatorChar + "preferences.seh");			   
			}
			catch (Exception q)
			{
				window.writeLog("Error when deleting preferences.seh before update" + q.ToString());
			}			

			ProcessStartInfo startInfo2 = new ProcessStartInfo(label1.Text);
			//startInfo2.Verb = "runas";
			Process.Start(startInfo2);
			
			window.CloseForUpdates();			
		}

		//loads with form
		private void download_Load(object sender, EventArgs e)
		{			
			this.Show();
			//this.downloadUpdate(commonAppData);
			saveFileDialog1.Filter = "Installer (*.exe)|*.exe";
			saveFileDialog1.FilterIndex = 1;
			saveFileDialog1.RestoreDirectory = true;
			saveFileDialog1.FileName = "TV Show Renamer Setup.exe";
			saveFileDialog1.DefaultExt = ".exe";
			saveFileDialog1.OverwritePrompt = true;
			saveFileDialog1.Title = "Select Location to Download";

			if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				string name = saveFileDialog1.FileName;
				this.downloadUpdate(name);
			}
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
	}//end of class
}//end of namespace
