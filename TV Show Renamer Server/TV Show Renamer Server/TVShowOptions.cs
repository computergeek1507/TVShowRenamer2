using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace TV_Show_Renamer_Server
{
	public partial class TVShowOptions : Form
	{

		List<TVShowSettings> _MainTVShowList;
		string _RootDir;
		NewTVDB TVDB;
		//int oldIndex = -1;



		public TVShowOptions(List<TVShowSettings> tvShowList, string rootDir, string commonAppData)

		{
			InitializeComponent();

			_MainTVShowList = tvShowList;
			_RootDir = rootDir;
			TVDB = new NewTVDB(commonAppData);
			listBox1.DataSource = _MainTVShowList;
			listBox1.DisplayMember = "SearchName";
			//foreach (TVShowSettings item in tvShowList)
			//{
			//	listBox1.Items.Add(item.SearchName);
			//}
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			this.Show();

		}

		public TVShowOptions()
		{
			InitializeComponent();
			this.Show();

		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = listBox1.SelectedIndex;
			showNameTextBox.Text = _MainTVShowList[index].SearchName;
			TVShowFolderTextBox.Text = _MainTVShowList[index].ShowFolder;
			TVDBNameTextBox.Text = _MainTVShowList[index].TVDBShowName;
			TVDBIDTextBox.Text = _MainTVShowList[index].TVDBSeriesID.ToString();
			TVRageTextBox.Text = _MainTVShowList[index].TVRageShowName;
			TVRageIDTextBox.Text = _MainTVShowList[index].TVRageSeriesID.ToString();
			checkBox1.Checked = _MainTVShowList[index].UseTVDBNumbering;
			checkBox2.Checked = _MainTVShowList[index].SeriesEnded;
			//MessageBox.Show(listBox1.SelectedItem.ToString());
		}

		private void button2_Click(object sender, EventArgs e)
		{
			int index = listBox1.SelectedIndex;

			_MainTVShowList[index].SearchName	   = showNameTextBox.Text;
			_MainTVShowList[index].ShowFolder	   = TVShowFolderTextBox.Text;
			_MainTVShowList[index].TVDBShowName	 = TVDBNameTextBox.Text;
			_MainTVShowList[index].TVDBSeriesID	 = Int32.Parse(TVDBIDTextBox.Text);
			_MainTVShowList[index].TVRageShowName   = TVRageTextBox.Text;
			_MainTVShowList[index].TVRageSeriesID   = Int32.Parse(TVRageIDTextBox.Text);
			_MainTVShowList[index].UseTVDBNumbering = checkBox1.Checked;
			_MainTVShowList[index].SeriesEnded	  = checkBox2.Checked;

		}

		private void folderButton_Click(object sender, EventArgs e)
		{

			string[] subdirectoryEntries = Directory.GetDirectories(_RootDir);
			new List<string>(subdirectoryEntries);

			SelectMenu SelectMain = new SelectMenu(new List<string>(subdirectoryEntries), showNameTextBox.Text, "Select TV Show Folder");
			if (SelectMain.ShowDialog() == DialogResult.OK)
			{
				int selectedid = SelectMain.selected;
				if (selectedid == -1) return;
				TVShowFolderTextBox.Text = subdirectoryEntries[selectedid].Replace(_RootDir, "").Replace(Path.DirectorySeparatorChar.ToString(), "");
				SelectMain.Close();
			}


			//folderBrowserDialog1.SelectedPath = _RootDir + Path.DirectorySeparatorChar + TVShowFolderTextBox.Text + Path.DirectorySeparatorChar;
			//if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			//{
			//	string tempString = folderBrowserDialog1.SelectedPath;
			//	TVShowFolderTextBox.Text = tempString.Replace(_RootDir, "").Replace(Path.DirectorySeparatorChar.ToString(), "");
			//}//end of if
		}

		private void button1_Click(object sender, EventArgs e)
		{
			int index = listBox1.SelectedIndex;

			OnlineShowInfo newTVDBID = TVDB.findTitle(showNameTextBox.Text,true);
			if (newTVDBID.ShowID != -1)
			{
				TVDBNameTextBox.Text = newTVDBID.ShowName;
				TVDBIDTextBox.Text = newTVDBID.ShowID.ToString();
				checkBox2.Checked = (newTVDBID.ShowStatus == "Ended") ? true : false;
			}
			
		}

		private void button2_Click_1(object sender, EventArgs e)
		{
			OnlineShowInfo newRageID = TVRage.findTitle(showNameTextBox.Text,true);
			if (newRageID.ShowID != -1)
			{
				TVRageTextBox.Text = newRageID.ShowName;
				TVRageIDTextBox.Text = newRageID.ShowID.ToString();
				checkBox2.Checked = (newRageID.ShowStatus == "Canceled/Ended") ? true : false;
			}

		}

	}
}
