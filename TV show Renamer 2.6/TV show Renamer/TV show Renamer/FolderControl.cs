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
        string[] outputOptions = { "\\root", "\\root\\Show Name", "\\root\\Show Name\\Season #" };
        //List<ToolStripMenuItem> menu = new List<ToolStripMenuItem>();
        //List<string> folderList = new List<string>();

        public FolderControl(Form1 tempMain)
        {            
            Main = tempMain;
            InitializeComponent();
        }

        //add folder button
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                //Main.AddTVFolderMenu();
                Main.AddFolder(textBox1.Text, textBox2.Text,1,false);
                Main.ClearOtherFolder();

                dataGridView1.Rows.Clear();
                for (int i = 0; i < Main.menu1.Count(); i ++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = Main.menu1[i].Text.ToString();
                    string[] words = Main.menu1[i].Tag.ToString().Split('?');
                    dataGridView1.Rows[i].Cells[1].Value = words[1];
                    dataGridView1.Rows[i].Cells[3].Value = false.ToString();
                    DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[i].Cells[2]);
                    cell.Items.AddRange(outputOptions);
                    cell.Value = cell.Items[int.Parse(words[0])-1];
                }
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)            
                textBox2.Text = folderBrowserDialog1.SelectedPath;            
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
                string[] words = Main.menu1[i].Tag.ToString().Split('?');
                dataGridView1.Rows[i].Cells[1].Value = words[1];
                dataGridView1.Rows[i].Cells[3].Value = false.ToString();
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[i].Cells[2]);
                cell.Items.AddRange(outputOptions);
                cell.Value = cell.Items[int.Parse(words[0]) - 1];
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

                for (int i = 0; i < Main.menu1.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = Main.menu1[i].Text.ToString();
                    string[] words = Main.menu1[i].Tag.ToString().Split('?');
                    dataGridView1.Rows[i].Cells[1].Value = words[1];
                    dataGridView1.Rows[i].Cells[3].Value = false.ToString();
                    DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[i].Cells[2]);
                    cell.Items.AddRange(outputOptions);
                    cell.Value = cell.Items[int.Parse(words[0]) - 1];
                }
                //Main.AddTVFolderMenu();
                Main.ClearOtherFolder();
                //Main.AddBrowserMenu();

            }
        }

        //save button
        private void button4_Click(object sender, EventArgs e)
        {
            List<string> newFolderInfo = new List<string>();
            List<bool> newFolderDefaults = new List<bool>();
            for (int i = 0; i < Main.menu1.Count(); i++)
            {
                switch (dataGridView1.Rows[i].Cells[2].Value.ToString())
                {                         
                    case "\\root\\Show Name":
                        newFolderInfo.Add("2?" + dataGridView1.Rows[i].Cells[1].Value.ToString());
                        break;
                    case "\\root\\Show Name\\Season #":
                        newFolderInfo.Add("3?" + dataGridView1.Rows[i].Cells[1].Value.ToString());
                        break;                        
                    default:
                        newFolderInfo.Add("1?" + dataGridView1.Rows[i].Cells[1].Value.ToString());
                        break;
                }
                newFolderDefaults.Add(bool.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()));            
            }
            Main.SaveFolders(newFolderInfo, newFolderDefaults);
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //MessageBox.Show("CellBeginEdit");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(e.RowIndex.ToString());
        }

    }//end of class
}//end of namespace
