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
    public partial class CategoryEdit : Form
    {
        List<CategoryInfo> CategoryList = new List<CategoryInfo>();
        string[] OutputOptions = { "\\root", "\\root\\Show Name", "\\root\\Show Name\\Season #", "\\root\\Show Name HD", "\\root\\Show Name HD\\Season # HD", "Custom" };

        public CategoryEdit(List<CategoryInfo> tempCategoryList)
        {
            InitializeComponent();
            CategoryList = tempCategoryList;            
        }
        //edit Category
        private void button2_Click(object sender, EventArgs e)
        {
            int u = dataGridView1.CurrentRow.Index;
            CategoryAdder mainDialog = new CategoryAdder(CategoryList, u);
            //mainDialog.Location = new Point(this.Location.X + ((this.Size.Width - mainDialog.Size.Width) / 2), this.Location.Y + ((this.Size.Height - mainDialog.Size.Height) / 2));
            if (mainDialog.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
                for (int i = 0; i < CategoryList.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    //dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
                    dataGridView1.Rows[i].Cells[0].Value = CategoryList[i].CategoryTitle;
                    dataGridView1.Rows[i].Cells[1].Value = CategoryList[i].CommandWords;
                    dataGridView1.Rows[i].Cells[2].Value = CategoryList[i].SearchFolder;
                    dataGridView1.Rows[i].Cells[3].Value = OutputOptions[CategoryList[i].FolderOptions];
                }
                mainDialog.Close();
            }
        }
        //add new Category
        private void button1_Click(object sender, EventArgs e)
        {
            CategoryAdder mainDialog = new CategoryAdder(CategoryList);
            //mainDialog.Location = new Point(this.Location.X + ((this.Size.Width - mainDialog.Size.Width) / 2), this.Location.Y + ((this.Size.Height - mainDialog.Size.Height) / 2));
            if (mainDialog.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
                for (int i = 0; i < CategoryList.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    //dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
                    dataGridView1.Rows[i].Cells[0].Value = CategoryList[i].CategoryTitle;
                    dataGridView1.Rows[i].Cells[1].Value = CategoryList[i].CommandWords;
                    dataGridView1.Rows[i].Cells[2].Value = CategoryList[i].SearchFolder;
                    dataGridView1.Rows[i].Cells[3].Value = OutputOptions[CategoryList[i].FolderOptions];
                }
                mainDialog.Close();
            }
        }
        //remove Category
        private void button3_Click(object sender, EventArgs e)
        {
            CategoryList.RemoveAt(dataGridView1.CurrentRow.Index);
            dataGridView1.Rows.Clear();
            for (int i = 0; i < CategoryList.Count(); i++)
            {
                dataGridView1.Rows.Add();
                //dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
                dataGridView1.Rows[i].Cells[0].Value = CategoryList[i].CategoryTitle;
                dataGridView1.Rows[i].Cells[1].Value = CategoryList[i].CommandWords;
                dataGridView1.Rows[i].Cells[2].Value = CategoryList[i].SearchFolder;
                dataGridView1.Rows[i].Cells[3].Value = OutputOptions[CategoryList[i].FolderOptions];
            }
        }

        private void CategoryEdit_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < CategoryList.Count(); i++)
            {
                dataGridView1.Rows.Add();
                //dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
                dataGridView1.Rows[i].Cells[0].Value = CategoryList[i].CategoryTitle;
                dataGridView1.Rows[i].Cells[1].Value = CategoryList[i].CommandWords;
                dataGridView1.Rows[i].Cells[2].Value = CategoryList[i].SearchFolder;
                dataGridView1.Rows[i].Cells[3].Value = OutputOptions[CategoryList[i].FolderOptions];
            }
        }
    }
}
