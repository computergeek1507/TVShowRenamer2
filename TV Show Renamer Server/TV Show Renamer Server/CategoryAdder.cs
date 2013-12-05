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
	public partial class CategoryAdder : Form
	{
		//string[] OutputOptions = { "Movie", "TV Show", "Music", };
		string[] OutputOptions = { "\\root", "\\root\\Show Name", "\\root\\Show Name\\Season #", "\\root\\Show Name HD", "\\root\\Show Name\\Season # HD", "Custom" };
		int selectedcatigory = -1;
		int selectedIndex = -1;

		List<CategoryInfo> CategoryList= new List<CategoryInfo>();

		public CategoryAdder(List<CategoryInfo> tempCategoryList)
		{
			CategoryList = tempCategoryList;
			InitializeComponent();
			comboBox1.Items.AddRange(OutputOptions);
			//this.Show();
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
		}
		public CategoryAdder(List<CategoryInfo> tempCategoryList, int tempselectedcatigory)
		{
			InitializeComponent();
			comboBox1.Items.AddRange(OutputOptions);
			selectedIndex=comboBox1.SelectedIndex = tempCategoryList[tempselectedcatigory].FolderOptions;
			button2.Text = "Save";
			this.Text = "Edit Category Settings";
			CategoryList = tempCategoryList;
			selectedcatigory = tempselectedcatigory;
			textBox1.Text = tempCategoryList[tempselectedcatigory].CategoryTitle;
			textBox3.Text = tempCategoryList[tempselectedcatigory].CommandWords;
			textBox2.Text = tempCategoryList[tempselectedcatigory].SearchFolder;
			//this.Show();
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
		}

		private void CategoryAdder_Load(object sender, EventArgs e)
		{
			
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			selectedIndex = ((ComboBox)sender).SelectedIndex;
		}

		//folder browser
		private void button1_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
				textBox2.Text = folderBrowserDialog1.SelectedPath;
		}
		//add/save button
		private void button2_Click(object sender, EventArgs e)
		{
			if (button2.Text == "Save")
			{
				CategoryInfo newInfo = new CategoryInfo(textBox1.Text, textBox3.Text, textBox2.Text, selectedIndex);
				CategoryList[selectedcatigory] = newInfo;
				button2.Text = "Add";
				this.Text = "Add New Category";
			}
			else {
				CategoryList.Add(new CategoryInfo(textBox1.Text, textBox3.Text, textBox2.Text, selectedIndex));			
			}
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Hide();
		}
		//cancel button
		private void button3_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Hide();
		}
		   
	}
}
