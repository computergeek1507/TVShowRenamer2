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
        public Display(List<string> display2)
        {

            InitializeComponent();
            //int length = display2.Count();
            label1.Text="Converted \n";
            for (int i = 0; i < display2.Count();i++ )
            {
                label1.Text+=(display2[i]+"\n");
            }

        }
        public Display(List<string> display5,string start)
        {

            InitializeComponent();
            label1.Text = start+"\n";
            for (int i = 0; i < display5.Count(); i++)
            {
                label1.Text += (display5[i] + "\n");
            }

        }
        public Display(List<string> display3,int test)
        {

            InitializeComponent();
            label1.Text = "Words in Text Converter \n";
            for (int i = 0; i < display3.Count(); i += 2)
            {
                label1.Text += (display3[i] + " to "+display3[i+1]+ "\n");
            }//end of for

        }
        public Display(List<string> display2,bool test)
        {

            InitializeComponent();
            //int length = display2.Count();
            label1.Text = "Words in User Library \n";
            if (display2.Count() == 0)
            {
                label1.Text += ("None" + "\n");
            }
            for (int i = 0; i < display2.Count(); i++)
            {
                
                label1.Text += (display2[i] + "\n");
            }

        }
        public Display(string displayOutput)
        {
            InitializeComponent();
            label1.Text=displayOutput;           

        }
        public Display(List<string> display4, List<string> display5)
        {
            InitializeComponent();
            label1.Text = "Titles \n";
            for (int i = 0; i < display5.Count(); i ++)
            {
                label1.Text += (display5[i]);
                if (display4[i] == "0")
                {
                    label1.Text += ("\t" + "\n");
                }
                else
                {
                    label1.Text += ("  - "+display4[i]+ "\n");
                }
            }//end of for

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        

        
    }
}
