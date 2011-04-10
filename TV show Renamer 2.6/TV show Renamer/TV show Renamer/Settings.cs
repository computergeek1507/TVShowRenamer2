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
        //List<ToolStripMenuItem> menu = new List<ToolStripMenuItem>();

        public Settings(Form1 test, bool zipCheck, List<string> tvfolderLoc)
        {
            InitializeComponent();
            Main = test;
            checkBox1.Checked = zipCheck;
            tvFolder = tvfolderLoc;
            //menu = tempMenu;
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

        //TV Folders Button
        private void button5_Click(object sender, EventArgs e)
        {
            move_folder tvshow = new move_folder(Main, tvFolder);
            this.Hide();
            if (tvshow.ShowDialog() == DialogResult.OK)            
                this.Close();            
        }
                
        private void button4_Click(object sender, EventArgs e)
        {
            FolderControl folderAdd = new FolderControl(Main);
            this.Hide();
            if (folderAdd.ShowDialog() == DialogResult.OK)            
                this.Close();            
        }

    }//end of class
}//end of namespace
