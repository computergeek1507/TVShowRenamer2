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
        string password2 = "";
        string fullzipfile = null;
        string fullzipname = null;

        //get password
        public string Password { get { return password2; } }

        public password(string zipFile, string zipname)
        {
            InitializeComponent();
            fullzipfile = zipFile;
            fullzipname = zipname;
            label2.Text = zipname;
        }

        //ok button        
        private void button1_Click(object sender, EventArgs e)
        {
            password2 = textBox1.Text;
            if (password2 == "")
            {
                MessageBox.Show("No Password Entered");
                return;
            }
            this.Hide();
        }

        //close button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //update password as you type
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            password2 = textBox1.Text;
        }
    }//end of class
}//end of namespace
