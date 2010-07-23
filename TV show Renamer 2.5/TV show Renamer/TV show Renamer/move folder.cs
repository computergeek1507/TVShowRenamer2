using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TV_show_Renamer
{
    public partial class move_folder : Form
    {
        Form1 mainform = new Form1();
        List<string> tvfolderslist = new List<string>();
        
        //constructor
        public move_folder(Form1 sentForm,List<string> tempTVshows)
        {
            tvfolderslist = tempTVshows;
            mainform = sentForm;
            InitializeComponent();
            for (int i = 0; i < tempTVshows.Count(); i++)
            {
                label1.Text += (tempTVshows[i] + "\n");
            }
        }
        //close form
        private void button1_Click(object sender, EventArgs e)
        {    
            this.Close();
        }
        //send folder list to main form
        private void move_folder_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainform.tvFolderChanger(tvfolderslist);
        }
        //add folder to list
        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tvfolderslist.Add(folderBrowserDialog1.SelectedPath);
                label1.Text += folderBrowserDialog1.SelectedPath+"\n";
            }
        }
        //clear list of folder
        private void button4_Click(object sender, EventArgs e)
        {
            tvfolderslist.Clear();
            label1.Text = "";
        }
    }//end of class
}
