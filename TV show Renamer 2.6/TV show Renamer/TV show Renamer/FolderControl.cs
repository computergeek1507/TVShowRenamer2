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
        int addIndex = 1;

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
                int x = dataGridView1.CurrentCell.ColumnIndex;
                Main.AddFolder(textBox1.Text, textBox2.Text, addIndex);
                Main.ClearOtherFolder();

                dataGridView1.Rows.Clear();
                for (int i = 0; i < Main.menu1.Count(); i ++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = (i+1).ToString();
                    dataGridView1.Rows[i].Cells[1].Value = Main.menu1[i].Text.ToString();
                    string[] words = Main.menu1[i].Tag.ToString().Split('?');
                    dataGridView1.Rows[i].Cells[2].Value = words[1];
                    DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[i].Cells[3]);
                    cell.Items.AddRange(outputOptions);
                    cell.Value = cell.Items[int.Parse(words[0])-1];
                }
                textBox1.Text = "";
                textBox2.Text = "";
                if (button1.Text == "Save Folder") button1.Text = "Add Folder";
                addIndex = 1;
                this.dataGridView1.CurrentCell = this.dataGridView1[x, dataGridView1.RowCount-1];
            }
        }

        //folder dialog button
        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)            
                textBox2.Text = folderBrowserDialog1.SelectedPath;            
        }       

        //remove folder button
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int y = -1;
                int x = dataGridView1.CurrentCell.ColumnIndex;
                for (int i = Main.menu1.Count() - 1; i >= 0; i--)
                {
                    if (dataGridView1.Rows[i].Cells[0].Selected || dataGridView1.Rows[i].Cells[1].Selected || dataGridView1.Rows[i].Cells[2].Selected)
                    {
                        Main.menu1.RemoveAt(i);
                        Main.menu2.RemoveAt(i);
                        Main.menu3.RemoveAt(i);
                        Main.menu4.RemoveAt(i);
                        y = i-1;
                    }
                }
                dataGridView1.Rows.Clear();

                for (int i = 0; i < Main.menu1.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
                    dataGridView1.Rows[i].Cells[1].Value = Main.menu1[i].Text.ToString();
                    string[] words = Main.menu1[i].Tag.ToString().Split('?');
                    dataGridView1.Rows[i].Cells[2].Value = words[1];
                    DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[i].Cells[3]);
                    cell.Items.AddRange(outputOptions);
                    cell.Value = cell.Items[int.Parse(words[0]) - 1];
                }
                Main.ClearOtherFolder();
                if (y != -1)
                    this.dataGridView1.CurrentCell = this.dataGridView1[x, y];
            }
        }

        //edit folder button
        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int y = dataGridView1.CurrentCell.RowIndex - 1;
                int x = dataGridView1.CurrentCell.ColumnIndex;
                int u = dataGridView1.CurrentRow.Index;

                textBox1.Text = Main.menu1[u].Text.ToString();
                string[] words2 = Main.menu1[u].Tag.ToString().Split('?');
                textBox2.Text = words2[1];
                Main.menu1.RemoveAt(u);
                Main.menu2.RemoveAt(u);
                Main.menu3.RemoveAt(u);
                Main.menu4.RemoveAt(u);
                addIndex = int.Parse(words2[0]);

                button1.Text = "Save Folder";

                dataGridView1.Rows.Clear();

                for (int i = 0; i < Main.menu1.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
                    dataGridView1.Rows[i].Cells[1].Value = Main.menu1[i].Text.ToString();
                    string[] words = Main.menu1[i].Tag.ToString().Split('?');
                    dataGridView1.Rows[i].Cells[2].Value = words[1];
                    DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[i].Cells[3]);
                    cell.Items.AddRange(outputOptions);
                    cell.Value = cell.Items[int.Parse(words[0]) - 1];
                }
                Main.ClearOtherFolder();
                if (y < 0) y = 0;
                if (dataGridView1.Rows.Count != 0)
                    this.dataGridView1.CurrentCell = this.dataGridView1[x, y];
            }
        }
        
        //combox index changed
        void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataGridViewComboBoxEditingControl temp = (DataGridViewComboBoxEditingControl)sender;
            int rowIndex = temp.EditingControlRowIndex;            
            int selectedIndex = ((ComboBox)sender).SelectedIndex;

            switch (selectedIndex)
                {                         
                    case 1:
                        Main.SaveFolder("2?" + dataGridView1.Rows[rowIndex].Cells[1].Value.ToString(), rowIndex);
                        break;
                    case 2:
                        Main.SaveFolder("3?" + dataGridView1.Rows[rowIndex].Cells[1].Value.ToString(), rowIndex);
                        break;                        
                    default:
                        Main.SaveFolder("1?" + dataGridView1.Rows[rowIndex].Cells[1].Value.ToString(), rowIndex);
                        break;
                }
        }
                
        private void FolderControl_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < Main.menu1.Count(); i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
                dataGridView1.Rows[i].Cells[1].Value = Main.menu1[i].Text.ToString();
                string[] words = Main.menu1[i].Tag.ToString().Split('?');
                dataGridView1.Rows[i].Cells[2].Value = words[1];
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[i].Cells[3]);
                cell.Items.AddRange(outputOptions);
                cell.Value = cell.Items[int.Parse(words[0]) - 1];
            }
        }

        private void FolderControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 3)
            {
                // Check box column
                ComboBox comboBox = e.Control as ComboBox;
                comboBox.SelectedIndexChanged -= new EventHandler(comboBox_SelectedIndexChanged);
                comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
            }
        }

        //up button
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Index!=0)
            {
                int u = dataGridView1.CurrentRow.Index;
                Main.MoveFolders(u, -1);
                //dataGridView1.Rows.Clear();

                for (int i = 0; i < Main.menu1.Count(); i++)
                {
                    //dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
                    dataGridView1.Rows[i].Cells[1].Value = Main.menu1[i].Text.ToString();
                    string[] words = Main.menu1[i].Tag.ToString().Split('?');
                    dataGridView1.Rows[i].Cells[2].Value = words[1];
                    DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[i].Cells[3]);
                    cell.Items.AddRange(outputOptions);
                    cell.Value = cell.Items[int.Parse(words[0]) - 1];
                }
                Main.ClearOtherFolder();
                this.dataGridView1.CurrentCell = this.dataGridView1[dataGridView1.CurrentCell.ColumnIndex, dataGridView1.CurrentCell.RowIndex - 1];

            }
        }

        //down button
        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Index != dataGridView1.RowCount-1)
            {
                int u = dataGridView1.CurrentRow.Index;
                Main.MoveFolders(u, 1);
                //dataGridView1.Rows.Clear();

                for (int i = 0; i < Main.menu1.Count(); i++)
                {
                    //dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
                    dataGridView1.Rows[i].Cells[1].Value = Main.menu1[i].Text.ToString();
                    string[] words = Main.menu1[i].Tag.ToString().Split('?');
                    dataGridView1.Rows[i].Cells[2].Value = words[1];
                    DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)(dataGridView1.Rows[i].Cells[3]);
                    cell.Items.AddRange(outputOptions);
                    cell.Value = cell.Items[int.Parse(words[0]) - 1];
                }
                Main.ClearOtherFolder();
                this.dataGridView1.CurrentCell = this.dataGridView1[dataGridView1.CurrentCell.ColumnIndex, dataGridView1.CurrentCell.RowIndex + 1];
            }
        }
        
    }//end of class
}//end of namespace
