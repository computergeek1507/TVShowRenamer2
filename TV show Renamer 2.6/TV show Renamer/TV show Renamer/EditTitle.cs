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
    public partial class EditTitle : Form
    {
        string title = "";
        public EditTitle(string temptitle)
        {
            InitializeComponent();
            textBox1.Text = title = temptitle;
        }

        public string getTitle(){
            return title;
        
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            title = textBox1.Text;
        }
    }
}
