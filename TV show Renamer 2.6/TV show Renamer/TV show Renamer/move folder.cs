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
                //label1.Text += (tempTVshows[i] + "\n");
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = tempTVshows[i];
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
                //label1.Text += folderBrowserDialog1.SelectedPath+"\n";
                dataGridView1.Rows.Add();
                dataGridView1.Rows[tvfolderslist.Count()-1].Cells[0].Value = folderBrowserDialog1.SelectedPath;
            }
        }

        //clear list of folder
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int u = dataGridView1.CurrentRow.Index;
                tvfolderslist.RemoveAt(u);
                dataGridView1.Rows.Clear();
                for (int i = 0; i < tvfolderslist.Count(); i++)
                {
                    //label1.Text += (tempTVshows[i] + "\n");
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = tvfolderslist[i];
                }
                //newForm.removePlaylist(u);
            }
        }
    }//end of class
}
