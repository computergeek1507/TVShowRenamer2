using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
//using System.Threading;

namespace TV_Show_Renamer
{
    public partial class junk_words : Form
    {
        List<string> junkwords = new List<string>();
        List<string> userwords = new List<string>();
        string commonAppData = null;
        Form1 Main;

        public junk_words()
        {
            InitializeComponent();
        }

        //get info from the main form
        public void junk_adder(List<string> junkwords2, string commonAppData2,Form1 test)
        {
            Main = test;
            junkwords = junkwords2;
            commonAppData = commonAppData2;
            this.getuserjunk();
            if (userwords.Count() != 0)
            {
                dataGridView1.Rows.Clear();
                for (int i = 0; i < userwords.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = userwords[i];
                }
            }
        }
        
        //read file that has user junk in it
        private void getuserjunk()
        {
            if (!File.Exists(commonAppData + "//userlibrary.seh"))
                {
                StreamWriter sw = new StreamWriter(commonAppData + "//userlibrary.seh");
                sw.WriteLine("0");                
                sw.Close();//close writer stream
            }else
            {//read junk file 
                StreamReader tr = new StreamReader(commonAppData + "//userlibrary.seh");
                userwords.Clear();//clear old list

                int size = Int32.Parse(tr.ReadLine());//read number of lines
                //if file is blank return nothing
                if (size == 0)                
                    return;                
                //read words from file
                for (int i = 0; i < size; i++)                
                    userwords.Add(tr.ReadLine());
                tr.Close();//close reader stream
            }            
        }//end of getuserjunk method

        //add word button
        private void button1_Click(object sender, EventArgs e)
        {
            string newword = textBox1.Text;

            if (newword == "" || newword == " " || newword == "  "||newword ==null)            
                return;            

            //check to see if new word is in main library
            for (int i = 0; i < junkwords.Count; i++) {
                if (newword==junkwords[i]) {
                    MessageBox.Show("Word already in Junk Library");
                    return;                
                }
            }//end of for
            //check to see if new word has been added b4
            if (!(userwords.Count == 0))
            {
                for (int i = 0; i < userwords.Count; i++)
                {
                    if (newword == userwords[i])
                    {
                        MessageBox.Show("Word already in Junk Library");
                        return;
                    }//end of if
                }//end of for
            }//end of if
            //add word
            userwords.Add(newword);//add junkword

            dataGridView1.Rows.Clear();
            for (int i = 0; i < userwords.Count(); i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = userwords[i];
            }

            textBox1.Text = null;//clear box
            this.dataGridView1.CurrentCell = this.dataGridView1[0, dataGridView1.RowCount - 1];
            convert();                       
         }
        
        //autoconvert method 
        private void convert()
        {
            Main.autoConvert();
        }

        //return junk words
        public List<string> getjunk() 
        {
            return userwords;        
        }        
                        
        //remove button
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int y = -1;
                int x = dataGridView1.CurrentCell.ColumnIndex;
                for (int i = userwords.Count() - 1; i >= 0; i--)
                {
                    if (dataGridView1.Rows[i].Cells[0].Selected)
                    {
                        userwords.RemoveAt(i);
                        y = i - 1;
                    }                   
                }                
                dataGridView1.Rows.Clear();
                for (int i = 0; i < userwords.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = userwords[i];
                }
                if (y != -1)
                    this.dataGridView1.CurrentCell = this.dataGridView1[x, y];
                convert();
            }
        }

        //load
        private void junk_words_Load(object sender, EventArgs e)
        {
            if (userwords.Count() != 0)
            {
                dataGridView1.Rows.Clear();
                for (int i = 0; i < userwords.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = userwords[i];
                }               
            }
        }

        private void junk_words_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            StreamWriter sw = new StreamWriter(commonAppData + "//userlibrary.seh");
            sw.WriteLine(userwords.Count());
            for (int j = 0; j < userwords.Count(); j++)            
                sw.WriteLine(userwords[j]);
            sw.Close();//close writer stream
            this.Hide();
            convert(); 
        }

        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                string newword = textBox1.Text;

                if (newword == "" || newword == " " || newword == "  " || newword == null)
                    return;

                //check to see if new word is in main library
                for (int i = 0; i < junkwords.Count; i++)
                {
                    if (newword == junkwords[i])
                    {
                        MessageBox.Show("Word already in Junk Library");
                        return;
                    }
                }//end of for
                //check to see if new word has been added b4
                if (!(userwords.Count == 0))
                {
                    for (int i = 0; i < userwords.Count; i++)
                    {
                        if (newword == userwords[i])
                        {
                            MessageBox.Show("Word already in Junk Library");
                            //return;
                        }//end of if
                    }//end of for
                }//end of if
                //add word
                userwords.Add(newword);//add junkword

                dataGridView1.Rows.Clear();
                for (int i = 0; i < userwords.Count(); i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = userwords[i];
                }

                textBox1.Text = null;//clear box

                convert();
            }
        }
    }//end of partial class 
}//end of namespace
