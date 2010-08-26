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
        Form1 main = new Form1();
        public password(Form1 temp)            
        {
            InitializeComponent();
            main = temp;
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //main.
            this.Close();
        }
    }
}
