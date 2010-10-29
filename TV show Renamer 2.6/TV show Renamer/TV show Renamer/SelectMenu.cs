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
    public partial class SelectMenu : Form
    {
        int intSelected = 0;
        public int selected { get { if (intSelected == 0)return -1; return intSelected; } }

        public SelectMenu(List<string> select)
        {
            InitializeComponent();
            //this.Show();
            dataGridView1.Rows.Clear();
                for (int z = 0; z < select.Count; z++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[z].Cells[0].Value = select[z];
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
        
    }
}
