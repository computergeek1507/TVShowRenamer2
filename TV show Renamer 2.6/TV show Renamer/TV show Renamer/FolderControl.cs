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
    public partial class FolderControl : Form
    {
        Form1 Main;
        //List<ToolStripMenuItem> menu = new List<ToolStripMenuItem>();
        //List<string> folderList = new List<string>();

        public FolderControl(Form1 tempMain)
        {
            
            Main = tempMain;
            InitializeComponent();
            //this.Show();
        }

        //add folder button
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                //Main.AddTVFolderMenu();
                Main.AddFolder(textBox1.Text, textBox2.Text);
                Main.ClearOtherFolder();

                dataGridView1.Rows.Clear();
                for (int i = 0; i < Main.menu1.Count(); i ++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = Main.menu1[i].Text.ToString();
                    dataGridView1.Rows[i].Cells[1].Value = Main.menu1[i].Tag.ToString();
                }
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void FolderControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void FolderControl_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < Main.menu1.Count(); i ++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = Main.menu1[i].Text.ToString();
                dataGridView1.Rows[i].Cells[1].Value = Main.menu1[i].Tag.ToString();
            }
        }

        //remove folder button
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                for (int i = Main.menu1.Count() - 1; i >= 0; i--)
                {
                    if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected)
                    {
                        Main.menu1.RemoveAt(i);
                        Main.menu2.RemoveAt(i);
                        Main.menu3.RemoveAt(i);
                        Main.menu4.RemoveAt(i);
                    }
                }
                dataGridView1.Rows.Clear();

                for (int i = 0; i < Main.menu1.Count(); i ++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = Main.menu1[i].Text.ToString();
                    dataGridView1.Rows[i].Cells[1].Value = Main.menu1[i].Tag.ToString();
                }
                //Main.AddTVFolderMenu();
                Main.ClearOtherFolder();
                //Main.AddBrowserMenu();

            }
        }//end of method
    }//end of class
}//end of namespace
