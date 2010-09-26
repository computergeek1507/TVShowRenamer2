using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TV_show_Renamer
{
    public partial class Addtitle : Form
    {        
        List<String> title = new List<String>();
        List<TVClass> names = new List<TVClass>();
        //bool open = false;
       
        Form1 Main;

        
        public Addtitle(List<TVClass> tvlist, Form1 test)
        {
            InitializeComponent();
            Main = test;
            names = tvlist;            
            this.Show();
        }

        //"close" form
        private void button1_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //Main.autoConvert();
            if (names.Count() != 0)
            {
                Thread t = new Thread(new ThreadStart(convert));
                t.Start();
            }
            this.Close();
        }

        //autoconvert method 
        private void convert() {
            Main.autoConvert();
        }

        //add title button
        private void button2_Click(object sender, EventArgs e)
        {            
                if (textBox1.Text != "" || textBox1.Text != " " || textBox1.Text != "  ")
                {
                    if (Main.addTitle(textBox1.Text))
                    {
                        textBox1.Text = null;
                        Thread t = new Thread(new ThreadStart(convert));
                        t.Start();
                    }
                }            
        }
           
        //clear titles                
        private void button3_Click(object sender, EventArgs e)
        {
            Main.clearTitles();
        }
               
        //run when form loads
        private void Addtitle_Load(object sender, EventArgs e)
        {
            //set value to see if any titles were added
            //open = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Main.removeTitle();
        }
    }
}
