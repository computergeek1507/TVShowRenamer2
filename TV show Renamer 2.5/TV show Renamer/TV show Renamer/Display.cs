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
        Addtitle mainTitle;
        bool title = false;
        //move_folder mainFolder;
        //bool folder = false;
        Text_Converter mainText;
        bool text = false;
        junk_words mainJunk;
        bool junk = false;

        //title method
        public Display(List<string> files, List<string> titles, Addtitle test)
        {
            InitializeComponent();

            dataGridView1.Columns[0].HeaderText = "Titles";
            button2.Text = "Remove Title";
            dataGridView1.ColumnHeadersVisible=true;
            for (int i = 0; i < files.Count(); i++)
            {
                dataGridView1.Rows.Add();
                if (titles[i] != "0")
                {
                    dataGridView1.Rows[i].Cells[0].Value = files[i] + " - " + titles[i];
                }
                else {
                    dataGridView1.Rows[i].Cells[0].Value = files[i];
                }
            }
            this.Show();
            columnOne = files;
            columnTwo = titles;
            mainTitle = test;
            title = true;
        }
        
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

                if (title) {
                    columnTwo[u] = "0";
                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < columnTwo.Count(); i++)
                    {
                        dataGridView1.Rows.Add();
                        if (columnTwo[i] != "0")
                        {
                            dataGridView1.Rows[i].Cells[0].Value = columnOne[i] + " - " + columnTwo[i];
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[0].Value = columnOne[i];
                        }
                    }
                    mainTitle.removeSelected(u);
                }//end of title if
                else if (text)
                {
                    columnOne.RemoveAt(u+1);
                    columnOne.RemoveAt(u);
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
