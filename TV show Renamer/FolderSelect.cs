using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TV_Show_Renamer
{
    public partial class FolderSelect : Form
    {
        //get password
        public string OutputFolder { get { return _outputFolder; } }

        string _outputFolder = "";
        List<string> _folderList = new List<string>();
        public FolderSelect(string TVShowName/*,string folder*/,List<string> folderList)
        {
            InitializeComponent();
            label2.Text = TVShowName;
            _folderList = folderList;
            _folderList.Reverse();
            //_outputFolder = folder;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            string text = label2.Text;
            if (InputBox.Show("Create New Folder", "Folder Name:", ref text) == DialogResult.OK)
            {
                _outputFolder = text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            SelectMenu SelectMain = new SelectMenu(_folderList,"Select TV Show Folder");
            if (SelectMain.ShowDialog() == DialogResult.OK)
            {
                int selectedid = SelectMain.selected;
                if (selectedid == -1)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    return;
                }
                _outputFolder = _folderList[selectedid];
                SelectMain.Close();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
                //_outputFolder = folderBrowserDialog1.SelectedPath;
                //this.DialogResult = System.Windows.Forms.DialogResult.OK;
            
            //this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
