using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TV_show_Renamer
{
    public partial class Text_Converter : Form
    {
        List<string> textConvert = new List<string>();
        Form1 Main;

        public Text_Converter()
        {
            InitializeComponent();
        }

        public void setUp(Form1 test) {
            Main = test;
        }

        private void convert()
        {
            Main.autoConvert();
        }

        //get text list
        public List<string> getText()
        {
            return textConvert;
        }

        //"close" window
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }

        //add text to be converted
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

        //view list
        private void button3_Click(object sender, EventArgs e)
        {
            Display newbox = new Display(textConvert, 1);                
            //newbox.Show();
        }

        //clear list
        private void button4_Click(object sender, EventArgs e)
        {
            textConvert.Clear();
        }       
    }
}
