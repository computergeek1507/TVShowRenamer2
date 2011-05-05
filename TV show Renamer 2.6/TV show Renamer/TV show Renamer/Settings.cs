using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

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
            checkBox2.Checked = Main.newMainSettings.AutoUpdates;

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
                Main.newMainSettings.BackgroundColor[0] = colorDialog1.Color.A;
                Main.newMainSettings.BackgroundColor[1] = colorDialog1.Color.R;
                Main.newMainSettings.BackgroundColor[2] = colorDialog1.Color.G;
                Main.newMainSettings.BackgroundColor[3] = colorDialog1.Color.B;
            }
        }

        //default colors
        private void button2_Click(object sender, EventArgs e)
        {
            int[] temp1 = { 255, 153, 180, 209 };
            int[] temp2 = { 255, 0, 0, 0 };
            int[] temp3 = { 255, 240, 240, 240 };
            Main.newMainSettings.BackgroundColor = temp1;
            Main.newMainSettings.ForegroundColor = temp2;
            Main.newMainSettings.ButtonColor = temp3;

            Main.BackColor = System.Drawing.Color.FromArgb(temp1[0], temp1[1], temp1[2], temp1[3]);
            Main.MainMenuStrip.BackColor = System.Drawing.Color.FromArgb(temp1[0], temp1[1], temp1[2], temp1[3]);
            Main.ForeColor = System.Drawing.Color.FromArgb(temp2[0], temp2[1], temp2[2], temp2[3]);
            Main.MainMenuStrip.ForeColor = System.Drawing.Color.FromArgb(temp2[0], temp2[1], temp2[2], temp2[3]);
            Color colorTemp1 = System.Drawing.Color.FromArgb(temp3[0], temp3[1], temp3[2], temp3[3]);
            Main.changeButtoncolor(colorTemp1);
        }

        //font color
        private void button3_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Main.ForeColor = colorDialog1.Color;
                Main.MainMenuStrip.ForeColor = colorDialog1.Color;
                Main.newMainSettings.ForegroundColor[0] = colorDialog1.Color.A;
                Main.newMainSettings.ForegroundColor[1] = colorDialog1.Color.R;
                Main.newMainSettings.ForegroundColor[2] = colorDialog1.Color.G;
                Main.newMainSettings.ForegroundColor[3] = colorDialog1.Color.B; 
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

        //button color settings
        private void button6_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Main.changeButtoncolor(colorDialog1.Color);
                Main.newMainSettings.ButtonColor[0] = colorDialog1.Color.A;
                Main.newMainSettings.ButtonColor[1] = colorDialog1.Color.R;
                Main.newMainSettings.ButtonColor[2] = colorDialog1.Color.G;
                Main.newMainSettings.ButtonColor[3] = colorDialog1.Color.B;                
            }
        }

        //change auto update setting
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Main.newMainSettings.AutoUpdates = checkBox2.Checked;
            if (Main.newMainSettings.AutoUpdates) 
            {
                Thread updateChecker = new Thread(new ThreadStart(Main.checkForUpdateSilent));
                updateChecker.Start();
            }
        }     

    }//end of class
}//end of namespace
