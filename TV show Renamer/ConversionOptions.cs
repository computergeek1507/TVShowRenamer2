using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Threading;

namespace TV_Show_Renamer
{
	public partial class ConversionOptions : Form
	{
		MainSettings newMainSettings;
		Form1 Main;
		string[] seasonSettings = { "1x01", "0101", "S01E01", "101", "1-1-2012", "None" };
		string[] programSettings = { "Test Show", "Test show", "TEST SHOW", "test show"};
		string[] dashSettings = { "-"," "};
		string[] dashSettings2 = { "-", " " };
		string[] titleSettings = { "Original", "Episode Title", "Episode title", "EPISODE TITLE", "episode title", "None" };
		string[] junkSettings = { "Junk Text", "Junk text", "JUNK TEXT", "junk text", "Original" };
		string[] extSettings = { ".ext", ".Ext",".EXT" };
		string[] titleGetSettings = { "Use File then Online", "Use File", "Online Only" };
		string[] TVSearch = { "TVDB.com", "TVRage.com", "Epguides.com", "theXEM.de" };

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
			comboBox6.DataSource = titleGetSettings;
			comboBox7.DataSource = extSettings;
			comboBox8.DataSource = TVSearch;
			comboBox1.SelectedIndex = newMainSettings.ProgramFormat;
			comboBox2.SelectedIndex = newMainSettings.SeasonFormat;
			comboBox3.SelectedIndex = newMainSettings.TitleFormat;
			comboBox4.SelectedIndex = Convert.ToInt32(newMainSettings.DashSeason);
			comboBox5.SelectedIndex = Convert.ToInt32(newMainSettings.DashTitle);
			comboBox6.SelectedIndex = newMainSettings.TitleSelection;
			comboBox7.SelectedIndex = newMainSettings.ExtFormat;
			comboBox8.SelectedIndex = newMainSettings.TVDataBase;
			checkBox1.Checked = newMainSettings.RemovePeriod;
			checkBox2.Checked = newMainSettings.RemoveUnderscore;
			checkBox3.Checked = newMainSettings.RemoveDash;
			checkBox4.Checked = newMainSettings.RemoveBracket;
			checkBox5.Checked = newMainSettings.RemoveCrap;
			checkBox6.Checked = newMainSettings.RemoveYear;
			checkBox7.Checked = newMainSettings.AutoGetTitle;
			checkBox8.Checked = newMainSettings.GetTVShowName;
			numericUpDown1.Value = newMainSettings.SeasonOffset;
			numericUpDown2.Value = newMainSettings.EpisodeOffset;

			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
			this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
			this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
			this.comboBox5.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
			this.comboBox6.SelectedIndexChanged += new System.EventHandler(this.comboBox6_SelectedIndexChanged);
			this.comboBox7.SelectedIndexChanged += new System.EventHandler(this.comboBox7_SelectedIndexChanged);
			this.comboBox8.SelectedIndexChanged += new System.EventHandler(this.comboBox8_SelectedIndexChanged);
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
			this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
			this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
			this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
			this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
			this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
			this.checkBox8.CheckedChanged += new System.EventHandler(this.checkBox8_CheckedChanged);
			this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			this.Show();
		}
		//ProgramFormat
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			newMainSettings.ProgramFormat = comboBox1.SelectedIndex;
			convert();
		}
		//SeasonFormat
		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			newMainSettings.SeasonFormat = comboBox2.SelectedIndex;
			convert();
		}	   
		//TitleFormat
		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
			newMainSettings.TitleFormat=comboBox3.SelectedIndex;
			convert();
		}
		//TitleSelection
		private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
		{
			newMainSettings.TitleSelection = comboBox6.SelectedIndex;
			convert();
		}
		//ExtFormat
		private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
		{
			newMainSettings.ExtFormat = comboBox7.SelectedIndex;
			convert();
		}
		//DashSeason
		private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
		{
			newMainSettings.DashSeason = Convert.ToBoolean(comboBox4.SelectedIndex);
			convert();
		}
		//DashTitle
		private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
		{
			newMainSettings.DashTitle = Convert.ToBoolean(comboBox5.SelectedIndex);
			convert();
		}
		//title server
		private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
		{
			newMainSettings.TVDataBase = comboBox8.SelectedIndex;
			convert();
		}
		//remove period
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			newMainSettings.RemovePeriod = checkBox1.Checked;
			convert();
		}
		//remove underscore
		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			newMainSettings.RemoveUnderscore = checkBox2.Checked;
			convert();
		}		
		//remove dash
		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			newMainSettings.RemoveDash = checkBox3.Checked;
			convert();
		}
		//remove year
		private void checkBox6_CheckedChanged(object sender, EventArgs e)
		{
			newMainSettings.RemoveYear = checkBox6.Checked;
			convert();
		}
		//remove bracket
		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{
			newMainSettings.RemoveBracket = checkBox4.Checked;
			convert();
		}
		//remove crap
		private void checkBox5_CheckedChanged(object sender, EventArgs e)
		{
			newMainSettings.RemoveCrap = checkBox5.Checked;
			convert();
		}
		//autoGetTitle
		private void checkBox7_CheckedChanged(object sender, EventArgs e)
		{
			newMainSettings.AutoGetTitle = checkBox7.Checked;
			convert(); 
		}
		//SeasonOffset
		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			newMainSettings.SeasonOffset = (int)numericUpDown1.Value;
			if (newMainSettings.AutoGetTitle)
				Main.clearTitles();
			convert();
		}
		//EpisodeOffset
		private void numericUpDown2_ValueChanged(object sender, EventArgs e)
		{
			newMainSettings.EpisodeOffset = (int)numericUpDown2.Value;
			if (newMainSettings.AutoGetTitle)
				Main.clearTitles();
			convert();
		}

		//use online show titles
		private void checkBox8_CheckedChanged(object sender, EventArgs e)
		{
			newMainSettings.GetTVShowName = checkBox8.Checked;
			convert();
		}

		//autoconvert
		private void convert()
		{
			Main.autoConvert();
		}

		private void ConversionOptions_Activated(object sender, EventArgs e)
		{
			//this.comboBox1.SelectedIndexChanged -= new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			//this.comboBox2.SelectedIndexChanged -= new System.EventHandler(this.comboBox2_SelectedIndexChanged);
			//this.comboBox3.SelectedIndexChanged -= new System.EventHandler(this.comboBox3_SelectedIndexChanged);
			//this.comboBox4.SelectedIndexChanged -= new System.EventHandler(this.comboBox4_SelectedIndexChanged);
			//this.comboBox5.SelectedIndexChanged -= new System.EventHandler(this.comboBox5_SelectedIndexChanged);
			//this.comboBox6.SelectedIndexChanged -= new System.EventHandler(this.comboBox6_SelectedIndexChanged);
			//this.comboBox7.SelectedIndexChanged -= new System.EventHandler(this.comboBox7_SelectedIndexChanged);
			//this.comboBox8.SelectedIndexChanged -= new System.EventHandler(this.comboBox8_SelectedIndexChanged);
			comboBox1.SelectedIndex = newMainSettings.ProgramFormat;
			comboBox2.SelectedIndex = newMainSettings.SeasonFormat;
			comboBox3.SelectedIndex = newMainSettings.TitleFormat;
			comboBox4.SelectedIndex = Convert.ToInt32(newMainSettings.DashSeason);
			comboBox5.SelectedIndex = Convert.ToInt32(newMainSettings.DashTitle);
			comboBox6.SelectedIndex = newMainSettings.TitleSelection;
			comboBox7.SelectedIndex = newMainSettings.ExtFormat;
			comboBox8.SelectedIndex = newMainSettings.TVDataBase;
			//this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			//this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
			//this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
			//this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
			//this.comboBox5.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
			//this.comboBox6.SelectedIndexChanged += new System.EventHandler(this.comboBox6_SelectedIndexChanged);
			//this.comboBox7.SelectedIndexChanged += new System.EventHandler(this.comboBox7_SelectedIndexChanged);
			//this.comboBox8.SelectedIndexChanged += new System.EventHandler(this.comboBox8_SelectedIndexChanged);
		}
		
		
	}//end of class
}//end of namespace
