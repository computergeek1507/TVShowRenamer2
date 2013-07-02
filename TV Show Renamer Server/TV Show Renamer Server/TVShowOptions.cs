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

        public TVShowOptions(List<TVShowSettings> tvShowList,string rootDir)
        {
            InitializeComponent();
            _MainTVShowList = tvShowList;
			_RootDir = rootDir;
			listBox1.DataSource = _MainTVShowList;
			listBox1.DisplayMember = "SearchName";
            //foreach (TVShowSettings item in tvShowList)
            //{
            //    listBox1.Items.Add(item.SearchName);
            //}
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
        }

        public TVShowOptions()
        {
            InitializeComponent();
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
			//MessageBox.Show(listBox1.SelectedItem.ToString());
		}

		private void button2_Click(object sender, EventArgs e)
		{
			int index = listBox1.SelectedIndex;

		}

		private void folderButton_Click(object sender, EventArgs e)
		{
			folderBrowserDialog1.SelectedPath = _RootDir + Path.DirectorySeparatorChar + TVShowFolderTextBox.Text + Path.DirectorySeparatorChar;
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				string tempString = folderBrowserDialog1.SelectedPath;
				TVShowFolderTextBox.Text = tempString.Replace(_RootDir, "").Replace(Path.DirectorySeparatorChar.ToString(), "");
			}//end of if
		}
    }
}
