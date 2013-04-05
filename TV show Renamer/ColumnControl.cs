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
	public partial class ColumnControl : Form
	{

		Form1 Main;   
		
		public ColumnControl (Form1 tempMain)
		{
			Main = tempMain;
			InitializeComponent();

			checkBox1.Checked = Main.dataGridView1.Columns["oldName"].Visible;
			checkBox2.Checked = Main.dataGridView1.Columns["newname"].Visible;
			checkBox3.Checked = Main.dataGridView1.Columns["filefolder"].Visible;
			checkBox4.Checked = Main.dataGridView1.Columns["fileextention"].Visible;
			checkBox5.Checked = Main.dataGridView1.Columns["TVShowID"].Visible;
			checkBox6.Checked = Main.dataGridView1.Columns["TVShowName"].Visible;
			checkBox7.Checked = Main.dataGridView1.Columns["titles"].Visible;
			checkBox8.Checked = Main.dataGridView1.Columns["SeasonNum"].Visible;
			checkBox9.Checked = Main.dataGridView1.Columns["EpisodeNum"].Visible;

			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
			this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
			this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
			this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
			this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
			this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
			this.checkBox8.CheckedChanged += new System.EventHandler(this.checkBox8_CheckedChanged);
			this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged);
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			Main.dataGridView1.Columns["oldName"].Visible = checkBox1.Checked;
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			Main.dataGridView1.Columns["newname"].Visible = checkBox2.Checked;
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			Main.dataGridView1.Columns["filefolder"].Visible = checkBox3.Checked;
		}

		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{
			Main.dataGridView1.Columns["fileextention"].Visible = checkBox4.Checked;
		}

		private void checkBox5_CheckedChanged(object sender, EventArgs e)
		{
			checkBox5.Checked = Main.dataGridView1.Columns["TVShowID"].Visible = checkBox5.Checked;
		}

		private void checkBox6_CheckedChanged(object sender, EventArgs e)
		{
			Main.dataGridView1.Columns["TVShowName"].Visible = checkBox6.Checked;
		}

		private void checkBox7_CheckedChanged(object sender, EventArgs e)
		{
			Main.dataGridView1.Columns["titles"].Visible = checkBox7.Checked;
		}

		private void checkBox8_CheckedChanged(object sender, EventArgs e)
		{
			Main.dataGridView1.Columns["SeasonNum"].Visible = checkBox8.Checked;
		}

		private void checkBox9_CheckedChanged(object sender, EventArgs e)
		{
			Main.dataGridView1.Columns["EpisodeNum"].Visible = checkBox9.Checked;
		}

		//close button
		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void ColumnControl_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}
	}
}
