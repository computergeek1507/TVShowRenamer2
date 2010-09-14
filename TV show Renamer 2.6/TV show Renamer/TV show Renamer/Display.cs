using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace TV_show_Renamer
{
    public partial class Display : Form
    {
        List<String> columnOne = new List<String>();
        List<String> columnTwo = new List<String>();        
        
        //move_folder mainFolder;
        //bool folder = false;
        Text_Converter mainText;
        bool text = false;
        junk_words mainJunk;
        bool junk = false;
        bool general = false;       
        
        //text converter method
        public Display(List<string> textConvert,  Text_Converter test)
        {
            InitializeComponent();

            dataGridView1.Columns[0].HeaderText = "Text to convert";
            button2.Text = "Remove Selected";
           
            if (textConvert.Count() != 0) {
                dataGridView1.ColumnHeadersVisible = true;
                for (int i = 0; i < textConvert.Count(); i =i +2)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i/2].Cells[0].Value = textConvert[i] + " to " + textConvert[i + 1];
                }                
                mainText = test;
                text = true;
            }
            columnOne = textConvert;
            this.Show();
        }
        //general
        public Display(List<string> test)
        {
            InitializeComponent();

            //dataGridView1.Columns[0]
            button2.Hide();

            if (test.Count() != 0)
            {
                //dataGridView1.ColumnHeadersVisible = true;
                for (int i = 0; i < test.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i ].Cells[0].Value = test[i];
                }
                //mainText = test;
                text = true;
            }
            columnOne = test;
            this.Show();
        }

        //junk list method
        public Display(List<string> junklist, junk_words test)
        {
            InitializeComponent();

            dataGridView1.Columns[0].HeaderText = "Junk Words";
            button2.Text = "Remove Selected";
            if (junklist.Count() != 0)
            {
                dataGridView1.ColumnHeadersVisible = true;
                for (int i = 0; i < junklist.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = junklist[i];
                }
                mainJunk = test;
                junk = true;
            }
            columnOne = junklist;
            this.Show();
        }
        
        //close
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //remove button
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int u = dataGridView1.CurrentRow.Index;

                if (text)
                {
                    columnOne.RemoveAt((u*2)+1);
                    columnOne.RemoveAt(u * 2);
                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < columnOne.Count(); i = i + 2)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i/2].Cells[0].Value = columnOne[i] + " to " + columnOne[i + 1];
                    }
                    //mainText.removeSelected(u + 1);
                    mainText.removeSelected(u);
                }//end of text if
                else if (junk)
                {
                    columnOne.RemoveAt(u);
                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < columnOne.Count(); i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = columnOne[i] ;
                    }                    
                    mainJunk.removeSelected(u);
                }//end of text if                
            }
        }                
    }
}
