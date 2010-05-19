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
    public partial class Addtitle : Form
    {
        
        List<String> title = new List<String>();
        List<String> names = new List<String>();
        bool open = false;
         
        public Addtitle()
        {
            InitializeComponent();
        }

        //"close" form
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        //add title button
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                if (textBox1.Text != "")
                {
                    for (int k = 0; k < names.Count(); k++)
                    {
                        if (comboBox1.SelectedItem.ToString() == names[k])
                        {
                            title[k] = textBox1.Text;
                            textBox1.Text = null;
                        }
                    }
                }
            }
        }

        //see if they entered title
        public bool checkTitle(int x) {
            //return false if dialog has never been opened
            if (!open){
                return false;
            }
                
            if (title[x] == "0"){
                return false;
            }
            else{
                return true;
            }           
        }//end of method
       
        //return title
        public string getTitle(int index)
        {
            return title[index];
        }
        
        //send video file names to add title for
        public void sendTitle( List<String> tvlist)
        {
            names = tvlist;
            comboBox1.Items.Clear();
            int x = tvlist.Count();
            int y = title.Count();            
            for (int i = 0; i < (x - y); i++)
            {
                title.Add("0");                
            }
            for (int i = 0; i < tvlist.Count(); i++)
            {
                comboBox1.Items.Add(tvlist[i]);
            }            
        }

        //clear titles                
        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < title.Count(); i++)
            {
                title[i] = "0";
            }
        }

        //show the titles that have been added
        private void button4_Click(object sender, EventArgs e)
        {
            Display box = new Display(title, names);
            box.Show();
        }
        
        //run when form loads
        private void Addtitle_Load(object sender, EventArgs e)
        {
            //set value to see if any titles were added
            open = true;
        }

        //run when files are cleared
        public void clearTitles() {
            title.Clear();
            names.Clear();
            open = false;        
        }


    }
}
