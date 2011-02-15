using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace TV_show_Renamer
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
            {
                //read junk file 
                StreamReader tr = new StreamReader(commonAppData + "//userlibrary.seh");
                userwords.Clear();//clear old list

                int size = Int32.Parse(tr.ReadLine());//read number of lines
                //if file is blank return nothing
                if (size == 0)
                {
                    return;
                }
                //read words from file
                for (int i = 0; i < size; i++)
                {
                    userwords.Add(tr.ReadLine());
                }//end of for
                tr.Close();//close reader stream
            }//end of method            
        }//end of getuserjunk method

        //add word button
        private void button1_Click(object sender, EventArgs e)
        {
            string newword = textBox1.Text;

            if (newword == "" || newword == " " || newword == "  ")
            {
                return;
            }

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
            textBox1.Text = null;//clear box

            Thread t = new Thread(new ThreadStart(convert));
            t.Start();                       
         }

        //dislpay junk words
        private void button2_Click(object sender, EventArgs e)
        {
            Display box = new Display(userwords, this);
        }

        //"close" form and save new words to file
        private void button3_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(commonAppData + "//userlibrary.seh");
            sw.WriteLine(userwords.Count());
            for (int j = 0; j < userwords.Count(); j++)
            {
                sw.WriteLine(userwords[j]);
            }//end of for
            sw.Close();//close writer stream
            this.Hide();
            

            Thread t = new Thread(new ThreadStart(convert));
            t.Start();            
        }//end of button method

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

        //clear user junk word list and rewrite file to blank
        private void button4_Click(object sender, EventArgs e)
        {
            userwords.Clear();
            StreamWriter sw = new StreamWriter(commonAppData + "//userlibrary.seh");
            sw.WriteLine("0");
            sw.Close();//close writer stream

            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }

        //remove selected 
        public void removeSelected() {
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }

        private void junk_words_Load(object sender, EventArgs e)
        {

        }
        
    }//end of partial class 
}//end of namespace
