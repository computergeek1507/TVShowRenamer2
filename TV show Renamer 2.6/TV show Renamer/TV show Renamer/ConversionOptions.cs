﻿using System;
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
    public partial class ConversionOptions : Form
    {

        MainSettings newMainSettings;
        Form1 Main;
        string[] seasonSettings = { "1x01", "0101", "S01E01", "101", "1-1-2011", "None" };
        string[] programSettings = { "Test Show", "Test show", "TEST SHOW", "test show"};
        string[] dashSettings = { "-"," "};
        string[] dashSettings2 = { "-", " " };
        string[] titleSettings = { "Episode Title", "Episode title", "EPISODE TITLE", "episode title", "None" };
        string[] junkSettings = { "Junk Text", "Junk text", "JUNK TEXT", "junk text", "Original" };
        string[] extSettings = { ".ext", ".Ext",".EXT" };

        public ConversionOptions(Form1 temp,MainSettings tempSettings)
        {            
            InitializeComponent();
            
            Main = temp;
            newMainSettings = tempSettings;
            comboBox1.DataSource = programSettings;
            comboBox2.DataSource = seasonSettings;
            comboBox3.DataSource = titleSettings;
            comboBox4.DataSource = dashSettings;
            comboBox5.DataSource = dashSettings2;
            comboBox6.DataSource = junkSettings;
            comboBox7.DataSource = extSettings;
            comboBox1.SelectedIndex = newMainSettings.ProgramFormat;
            comboBox2.SelectedIndex = newMainSettings.SeasonFormat;
            comboBox3.SelectedIndex = newMainSettings.TitleFormat;
            comboBox4.SelectedIndex = Convert.ToInt32(newMainSettings.DashSeason);
            comboBox5.SelectedIndex = Convert.ToInt32(newMainSettings.DashTitle);
            comboBox6.SelectedIndex = newMainSettings.JunkFormat;
            comboBox7.SelectedIndex = newMainSettings.ExtFormat;
            checkBox1.Checked = newMainSettings.RemovePeriod;
            checkBox2.Checked = newMainSettings.RemoveUnderscore;
            numericUpDown1.Value = newMainSettings.SeasonOffset;
            numericUpDown2.Value = newMainSettings.EpisodeOffset;

            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            this.comboBox5.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
            this.comboBox6.SelectedIndexChanged += new System.EventHandler(this.comboBox6_SelectedIndexChanged);
            this.comboBox7.SelectedIndexChanged += new System.EventHandler(this.comboBox7_SelectedIndexChanged);
            this.Show();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            newMainSettings.ProgramFormat = comboBox1.SelectedIndex;
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            newMainSettings.SeasonFormat = comboBox2.SelectedIndex;
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            newMainSettings.DashSeason = Convert.ToBoolean(comboBox4.SelectedIndex);
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            newMainSettings.DashTitle = Convert.ToBoolean(comboBox5.SelectedIndex);
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            newMainSettings.ExtFormat = comboBox7.SelectedIndex;
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
           newMainSettings.JunkFormat = comboBox6.SelectedIndex;
           Thread t = new Thread(new ThreadStart(convert));
           t.Start();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            newMainSettings.TitleFormat=comboBox3.SelectedIndex;
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }

        private void convert()
        {
            Main.autoConvert();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            newMainSettings.RemovePeriod = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            newMainSettings.RemoveUnderscore = checkBox2.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            newMainSettings.SeasonOffset = (int)numericUpDown1.Value;
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            newMainSettings.EpisodeOffset = (int)numericUpDown2.Value;
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }        

    }//end of class
}//end of namespace
