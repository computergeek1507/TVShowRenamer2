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
		thexem TVDB;
		TMDb TMDbClient;
		//int oldIndex = -1;



		public TVShowOptions(List<TVShowSettings> tvShowList, string rootDir, string commonAppData)

		{
			InitializeComponent();

			_MainTVShowList = tvShowList;
			_RootDir = rootDir;
			TVDB = new thexem(commonAppData);
			TMDbClient = new TMDb(commonAppData);
			listBoxTVShowList.DataSource = _MainTVShowList;
			listBoxTVShowList.DisplayMember = "SearchName";
			//foreach (TVShowSettings item in tvShowList)
			//{
			//	listBox1.Items.Add(item.SearchName);
			//}
			this.listBoxTVShowList.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			this.Show();
		}

		public TVShowOptions()
		{
			InitializeComponent();
			this.Show();
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = listBoxTVShowList.SelectedIndex;
			if (index == -1)
				return;

			showNameTextBox.Text = _MainTVShowList[index].SearchName;
			TVShowFolderTextBox.Text = _MainTVShowList[index].ShowFolder;
			TVShowFolderHDTextBox.Text = _MainTVShowList[index].ShowFolderHD;

			TVDBNameTextBox.Text = _MainTVShowList[index].TVDBShowName;
			TVDBIDTextBox.Text = _MainTVShowList[index].TVDBSeriesID.ToString();
			TMDbTextBox.Text = _MainTVShowList[index].TMDbShowName;
			TMDbIDTextBox.Text = _MainTVShowList[index].TMDbSeriesID.ToString();
			checkBoxHD.Checked = _MainTVShowList[index].GetHD;
			//checkBox1.Checked = _MainTVShowList[index].UseTVDBNumbering;
			checkBox2.Checked = _MainTVShowList[index].SeriesEnded;
			//MessageBox.Show(listBox1.SelectedItem.ToString());
		}

		private void button2_Click(object sender, EventArgs e)
		{
			int index = listBoxTVShowList.SelectedIndex;

			if (index == -1)
				return;

			_MainTVShowList[index].SearchName		= showNameTextBox.Text;
			_MainTVShowList[index].ShowFolder		= TVShowFolderTextBox.Text;
			_MainTVShowList[index].ShowFolderHD	= TVShowFolderHDTextBox.Text;
			_MainTVShowList[index].TVDBShowName	 = TVDBNameTextBox.Text;
			_MainTVShowList[index].TVDBSeriesID	 = Int32.Parse(TVDBIDTextBox.Text);
			_MainTVShowList[index].TMDbShowName = TMDbTextBox.Text;
			_MainTVShowList[index].TMDbSeriesID = Int32.Parse(TMDbIDTextBox.Text);
			_MainTVShowList[index].GetHD		= checkBoxHD.Checked;
			//_MainTVShowList[index].UseTVDBNumbering = checkBox1.Checked;
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
			int index = listBoxTVShowList.SelectedIndex;

			if (index == -1)
				return;

			OnlineShowInfo newTVDBID = TVDB.findTitle(showNameTextBox.Text, true);
			if (newTVDBID.ShowID != -1)
			{
				TVDBNameTextBox.Text = newTVDBID.ShowName;
				TVDBIDTextBox.Text = newTVDBID.ShowID.ToString();
				checkBox2.Checked = newTVDBID.ShowEnded;
			}
			
		}

		private void button2_Click_1(object sender, EventArgs e)
		{
			OnlineShowInfo newTMDbID = TMDbClient.findTitle(showNameTextBox.Text, true);
			if (newTMDbID.ShowID != -1)
			{
				TMDbTextBox.Text = newTMDbID.ShowName;
				TMDbIDTextBox.Text = newTMDbID.ShowID.ToString();
				checkBox2.Checked = newTMDbID.ShowEnded;
			}

		}

		private void buttonAddShow_Click(object sender, EventArgs e)
		{

		}

		private void buttonRemoveShow_Click(object sender, EventArgs e)
		{
			int index = listBoxTVShowList.SelectedIndex;

			if (index == -1)
				return;

			if (MessageBox.Show("Remove "+_MainTVShowList[index].SearchName+" from database?", "Remove TV Show", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				 _MainTVShowList.RemoveAt(index);
				 listBoxTVShowList.ClearSelected();
				 //listBoxTVShowList.DataSource = _MainTVShowList;
				 listBoxTVShowList.DisplayMember = "";
				 listBoxTVShowList.DisplayMember = "SearchName";
				 listBoxTVShowList.Refresh();
			}
		}

		private void folderHDButton_Click(object sender, EventArgs e)
		{
			string[] subdirectoryEntries = Directory.GetDirectories(_RootDir);
			new List<string>(subdirectoryEntries);

			SelectMenu SelectMain = new SelectMenu(new List<string>(subdirectoryEntries), showNameTextBox.Text, "Select TV Show Folder");
			if (SelectMain.ShowDialog() == DialogResult.OK)
			{
				int selectedid = SelectMain.selected;
				if (selectedid == -1) return;
				TVShowFolderHDTextBox.Text = subdirectoryEntries[selectedid].Replace(_RootDir, "").Replace(Path.DirectorySeparatorChar.ToString(), "");
				SelectMain.Close();
			}
		}

		private void buttonGetEpisodes_Click(object sender, EventArgs e)
		{

		}

		private void buttonSearchFolder_Click(object sender, EventArgs e)
		{

		}

	}
}
