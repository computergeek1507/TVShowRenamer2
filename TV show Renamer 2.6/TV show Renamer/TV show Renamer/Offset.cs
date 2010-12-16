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
    public partial class Offset : Form
    {
        Form1 Main;
        public Offset(Form1 temp,int seasonOffset,int episodeOffset)
        {
            Main = temp;
            InitializeComponent();
            numericUpDown1.Value = seasonOffset;
            numericUpDown2.Value = episodeOffset;
            this.Show();        
        }


        //autoconvert method 
        private void convert()
        {
            Main.autoConvert();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Main.SeasonOffset = (int)numericUpDown1.Value;
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            Main.EpisodeOffset = (int)numericUpDown2.Value;
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }
    }
}
