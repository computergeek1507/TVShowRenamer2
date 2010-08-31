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
    public partial class password : Form
    {
        //List<string> password2 = new List<string>();
        string password2 = "";
        string fullzipfile = null;
        string fullzipname = null;
        Form1 main;
        //int i = 0;

        public string Password { get { return password2; } set { password2 = value; } }

        public password(Form1 temp, string zipFile, string zipname)            
        {
            InitializeComponent();
            main = temp;
            fullzipfile = zipFile;
            fullzipname = zipname;
            label2.Text = zipname;
        }
        
        
        private void button1_Click(object sender, EventArgs e)
        {
            password2 = textBox1.Text;
            if (password2 == "") {
                MessageBox.Show("No Password Entered");
                return;
            }
            
            //main.archiveExtrector(fullzipfile, fullzipname, password2);
            //fullzipfile.RemoveAt(0);
            //fullzipname.RemoveAt(0);
            //if (fullzipfile.Count() == 0)
            //{
                //label2.Text = fullzipname[0]; 
            //}
           // else {
              //  this.Hide();
            //}
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            password2 = textBox1.Text;
        }
    }
}
