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
           System.Diagnostics.Process.Start("http://scottnation.com");
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("http://armorgames.com/play/4703/miami-shark");
            System.Diagnostics.Process.Start("https://www.paypal.com/us/cgi-bin/webscr?cmd=_flow&SESSION=bzy56ZFh-cc0N6PnIseq9L8NP8SwPl0JAmrAvrMYrRkAGIWg47Al10MsANO&dispatch=5885d80a13c0db1f8e263663d3faee8d5fa8ff279e37c3d9d4e38bdbee0ede69");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //System.Diagnostics.Process.Start("scott@scottnation.com");
        }
    }//end of about class
}//end of namespace
