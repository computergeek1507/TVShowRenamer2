using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TV_Show_Renamer
{
    public partial class About : Form
    {
        public About(int ver)
        {
            InitializeComponent();
            label1.Text = ("Build " + ver.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           System.Diagnostics.Process.Start("http://tvshowrenamer.codeplex.com");
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=RURURGGNBVHG6&lc=US&item_name=TV%20Show%20Renamer&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:scott@scottnation.com");
        }
    }//end of about class
}//end of namespace
