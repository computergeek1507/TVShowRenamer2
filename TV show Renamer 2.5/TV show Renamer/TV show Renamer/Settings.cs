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
        Form Main;
        public Settings(Form test)
        {
            InitializeComponent();
            Main = test;
            this.Show();
        }

        //set color of background
        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Main.BackColor = colorDialog1.Color;
                //Main.
                Main.MainMenuStrip.BackColor = colorDialog1.Color;
            }
        }

        //default colors
        private void button2_Click(object sender, EventArgs e)
        {
            Main.BackColor = System.Drawing.SystemColors.ActiveCaption;
            Main.MainMenuStrip.BackColor = System.Drawing.SystemColors.ActiveCaption;            
        }
    }
}
