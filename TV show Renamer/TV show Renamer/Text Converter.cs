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
    public partial class Text_Converter : Form
    {
        List<string> textConvert = new List<string>();

        public Text_Converter()
        {
            InitializeComponent();
        }


        public List<string> getText()
        {
            return textConvert;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text != "") || (textBox1.Text != "0000"))
                {
                textConvert.Add(textBox1.Text);
                textConvert.Add(textBox2.Text);
                }
            textBox1.Text = null;
            textBox2.Text = null;
            //Display newbox = new Display(textConvert, true);                
            //newbox.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Display newbox = new Display(textConvert, 1);                
            newbox.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textConvert.Clear();
        }       
    }
}
