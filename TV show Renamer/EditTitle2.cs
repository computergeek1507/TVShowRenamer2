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
    public partial class EditTitle2 : Form
    {
        string title = "";
        public EditTitle2(string temptitle)
        {
            InitializeComponent();
            textBox1.Text = title = temptitle;
            //this.button2.Location = new System.Drawing.Point(93, 38);
            //this.button1.Location = new System.Drawing.Point(12, 38);
        }

        public string getTitle()
        {
            return title;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            title = textBox1.Text;
        }
        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                MessageBox.Show(title);
                if (title != "" || title != " "||title != null)
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                else
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }
    }//end of class
}//end of namespace
