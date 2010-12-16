﻿using System;
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
        //List<String> title = new List<String>();
        BindingList<TVClass> names = new BindingList<TVClass>();
              
        Form1 Main;

        public Addtitle(BindingList<TVClass> tvlist, Form1 test)
        {
            InitializeComponent();
            Main = test;
            names = tvlist;            
            this.Show();
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
     
        private void button4_Click(object sender, EventArgs e)
        {
            Main.removeTitle();
        }

        private void Addtitle_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (names.Count() != 0)
            {
                Thread t = new Thread(new ThreadStart(convert));
                t.Start();
            }
        }
    }
}
