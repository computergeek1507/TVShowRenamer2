using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TV_Show_Renamer_Server
{
	public partial class SelectMenu : Form
	{
		int intSelected = -1;
		public int selected { get { return intSelected; } }

		public SelectMenu(List<string> select, string showName)
		{
			InitializeComponent();
			label1.Text = showName;
			dataGridView1.Rows.Clear();
			for (int z = 0; z < select.Count; z++)
			{
				dataGridView1.Rows.Add();
				dataGridView1.Rows[z].Cells[0].Value = select[z];
			}
		}
		public SelectMenu(List<string> select, string header, string showName)
		{
			InitializeComponent();
			this.Text = header;
			label1.Text = showName;
			dataGridView1.Rows.Clear();
			for (int z = 0; z < select.Count; z++)
			{
				dataGridView1.Rows.Add();
				dataGridView1.Rows[z].Cells[0].Value = select[z];
			}
		}
		public SelectMenu(List<TVShowSettings> select, string showName)
		{
			InitializeComponent();
			dataGridView1.Rows.Clear();
			label1.Text = showName;
			for (int z = 0; z < select.Count; z++)
			{
				dataGridView1.Rows.Add();
				dataGridView1.Rows[z].Cells[0].Value = select[z].SearchName;
			}
		}
		public SelectMenu(List<SearchInfo> select,string showName)
		{
			InitializeComponent();
			label1.Text = showName;
			dataGridView1.Rows.Clear();
			for (int z = 0; z < select.Count; z++)
			{
				dataGridView1.Rows.Add();
				dataGridView1.Rows[z].Cells[0].Value = select[z].Title;
			}
		}

		public SelectMenu(List<OnlineShowInfo> select,string showName,string title)
		{
			InitializeComponent();
			this.Text = title;
			label1.Text = showName;
			dataGridView1.Rows.Clear();
			Year.Visible = true;
			for (int z = 0; z < select.Count; z++)
			{
				dataGridView1.Rows.Add();
				dataGridView1.Rows[z].Cells[0].Value = select[z].ShowName;



				dataGridView1.Rows[z].Cells[1].Value = select[z].StartYear.ToString();
			}
		}



		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}


		private void button1_Click(object sender, EventArgs e)
		{			
			intSelected = dataGridView1.CurrentRow.Index;
		}

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			intSelected = dataGridView1.CurrentRow.Index;
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			intSelected = dataGridView1.CurrentRow.Index;
			if (intSelected != -1)		   
				this.DialogResult = System.Windows.Forms.DialogResult.OK;			
			else			
				this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}
	}//end of class

}//end of namespace
