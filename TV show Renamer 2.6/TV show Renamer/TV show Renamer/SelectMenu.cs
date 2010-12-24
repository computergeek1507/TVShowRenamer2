﻿using System;
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
        int intSelected = -1;
        public int selected { get { return intSelected; } }

        public SelectMenu(List<string> select)
        {
            InitializeComponent();
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            intSelected = dataGridView1.CurrentRow.Index;
        }
    }//end of class
}//end of namespace
