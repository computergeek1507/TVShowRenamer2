using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace TV_show_Renamer
{
    public partial class Text_Converter : Form
    {
        List<string> textConvert = new List<string>();
        Form1 Main;
        string commonAppData = null;

        public Text_Converter()
        {
            InitializeComponent();
        }

        //run at startup
        public void setUp(Form1 test, string commonAppData2)
        {
            Main = test;
            commonAppData = commonAppData2;
            this.getTextConvert();
        }

        //load text file
        private void getTextConvert()
        {
            if (!File.Exists(commonAppData + "//convertlibrary.seh"))
            {
                StreamWriter sw = new StreamWriter(commonAppData + "//convertlibrary.seh");
                sw.WriteLine("0");
                sw.Close();//close writer stream
            }
            else
            {   //read junk file 
                StreamReader tr = new StreamReader(commonAppData + "//convertlibrary.seh");
                textConvert.Clear();//clear old list

                int size = Int32.Parse(tr.ReadLine());//read number of lines
                //if file is blank return nothing
                if (size == 0)
                {
                    return;
                }
                //read words from file
                for (int i = 0; i < size; i++)
                {
                    textConvert.Add(tr.ReadLine());
                }//end of for
                tr.Close();//close reader stream
            }//end of method
        }//end of getTextConvert method

        //autoconvert method 
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
            StreamWriter sw = new StreamWriter(commonAppData + "//convertlibrary.seh");
            sw.WriteLine(textConvert.Count());
            for (int j = 0; j < textConvert.Count(); j++)
            {
                sw.WriteLine(textConvert[j]);
            }//end of for
            sw.Close();//close writer stream
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
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }

        //view list
        private void button3_Click(object sender, EventArgs e)
        {
            Display newbox = new Display(textConvert,this);                
        }

        //clear list
        private void button4_Click(object sender, EventArgs e)
        {
            textConvert.Clear();
            StreamWriter sw = new StreamWriter(commonAppData + "//convertlibrary.seh");
            sw.WriteLine("0");
            sw.Close();//close writer stream
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
        }

        //remove selected
        public void removeSelected() {
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();            
        }
    }
}
