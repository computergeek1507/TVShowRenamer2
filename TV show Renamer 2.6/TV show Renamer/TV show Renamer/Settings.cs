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
    public partial class Settings : Form
    {
        Form1 Main;
        List<string> tvFolder;
        public Settings(Form1 test, bool zipCheck, List<string> tvfolderLoc)
        {
            InitializeComponent();
            Main = test;
            checkBox1.Checked = zipCheck;
            tvFolder = tvfolderLoc;
            this.Show();
        }

        //set color of background
        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Main.BackColor = colorDialog1.Color;
                Main.MainMenuStrip.BackColor = colorDialog1.Color;
            }
        }

        //default colors
        private void button2_Click(object sender, EventArgs e)
        {
            Main.BackColor = System.Drawing.SystemColors.ActiveCaption;
            Main.MainMenuStrip.BackColor = System.Drawing.SystemColors.ActiveCaption;
            Main.ForeColor = System.Drawing.SystemColors.ControlText;
            Main.MainMenuStrip.ForeColor = System.Drawing.SystemColors.ControlText;
        }

        //font color
        private void button3_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Main.ForeColor = colorDialog1.Color;
                Main.MainMenuStrip.ForeColor = colorDialog1.Color;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Main.changeZIPstate( checkBox1.Checked);
        }

        //movie folder
        private void button4_Click(object sender, EventArgs e)
        {
            if (Main.FolderGetter(1) != "0000"||Main.FolderGetter(1) != "")
            {
                folderBrowserDialog1.SelectedPath = Main.FolderGetter(1);
            } 
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Main.FolderChanger(1, folderBrowserDialog1.SelectedPath);
            }
        }

        //movie folder2
        private void button8_Click(object sender, EventArgs e)
        {
            if (Main.FolderGetter(2) != "0000" || Main.FolderGetter(2) != "")
            {
                folderBrowserDialog1.SelectedPath = Main.FolderGetter(2);
            } 
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Main.FolderChanger(2, folderBrowserDialog1.SelectedPath);
            }
        }

        //movie trailer folder
        private void button6_Click(object sender, EventArgs e)
        {
            if (Main.FolderGetter(3) != "0000" || Main.FolderGetter(3) != "")
            {
                folderBrowserDialog1.SelectedPath = Main.FolderGetter(3);
            } 
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Main.FolderChanger(3, folderBrowserDialog1.SelectedPath);
            }
        }

        //music video folder
        private void button7_Click(object sender, EventArgs e)
        {
            if (Main.FolderGetter(4) != "0000" || Main.FolderGetter(4) != "")
            {
                folderBrowserDialog1.SelectedPath = Main.FolderGetter(4);
            } 
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Main.FolderChanger(4, folderBrowserDialog1.SelectedPath);
            }
        }
        
        //other videos fulder
        private void button9_Click(object sender, EventArgs e)
        {
            if (Main.FolderGetter(5) != "0000" || Main.FolderGetter(5) != "")
            {
                folderBrowserDialog1.SelectedPath = Main.FolderGetter(5);
            }
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Main.FolderChanger(5, folderBrowserDialog1.SelectedPath);
            }
        }

        //TV Folders Button
        private void button5_Click(object sender, EventArgs e)
        {
            move_folder tvshow = new move_folder(Main, tvFolder);
            tvshow.Show();
        }

    }//end of class
}//end of namespace
