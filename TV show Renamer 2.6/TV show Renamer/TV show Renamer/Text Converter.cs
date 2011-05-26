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
                    return;                
                //read words from file
                for (int i = 0; i < size; i++)                
                    textConvert.Add(tr.ReadLine());               
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
               
        //add text to be converted
        private void button2_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text != "") || (textBox1.Text != "0000"))
            {
                textConvert.Add(textBox1.Text);
                textConvert.Add(textBox2.Text);

                textBox1.Text = null;
                textBox2.Text = null;
                if (button2.Text == "Save") button2.Text = "Add";
                int x = dataGridView1.CurrentCell.ColumnIndex;
                dataGridView1.Rows.Clear();
                for (int i = 0; i < textConvert.Count(); i = i + 2)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i / 2].Cells[0].Value = textConvert[i];
                    dataGridView1.Rows[i / 2].Cells[1].Value = "to";
                    dataGridView1.Rows[i / 2].Cells[2].Value = textConvert[i + 1];
                }
                this.dataGridView1.CurrentCell = this.dataGridView1[x, dataGridView1.RowCount - 1];
                Thread t = new Thread(new ThreadStart(convert));
                t.Start();
            }
        }
       
        //edit
        private void button4_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {                
                int u = dataGridView1.CurrentRow.Index;

                textBox2.Text = textConvert[(u * 2) + 1];
                textBox1.Text = textConvert[(u * 2)];
                textConvert.RemoveAt((u*2)+1);
                textConvert.RemoveAt(u * 2);
                button2.Text = "Save";

                dataGridView1.Rows.Clear();
                for (int i = 0; i < textConvert.Count(); i = i + 2)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i / 2].Cells[0].Value = textConvert[i];
                    dataGridView1.Rows[i / 2].Cells[1].Value = "to";
                    dataGridView1.Rows[i / 2].Cells[2].Value = textConvert[i + 1];
                }
                Thread t = new Thread(new ThreadStart(convert));
                t.Start();
            }
        }

        //remove 
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                for (int i = textConvert.Count() - 1; i >= 0; i=i-2)
                {
                    if (dataGridView1.Rows[i / 2].Cells[0].Selected || dataGridView1.Rows[i / 2].Cells[1].Selected || dataGridView1.Rows[i / 2].Cells[2].Selected)
                    {
                        textConvert.RemoveAt(i);
                        textConvert.RemoveAt(i-1);
                    }
                }
                //int y = dataGridView1.CurrentCell.RowIndex - 1;
                //int x = dataGridView1.CurrentCell.ColumnIndex;
                dataGridView1.Rows.Clear();
                for (int i = 0; i < textConvert.Count(); i = i + 2)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i / 2].Cells[0].Value = textConvert[i];
                    dataGridView1.Rows[i / 2].Cells[1].Value = "to";
                    dataGridView1.Rows[i / 2].Cells[2].Value = textConvert[i + 1];
                }                
                Thread t = new Thread(new ThreadStart(convert));
                t.Start();
            }
        }

        private void Text_Converter_Load(object sender, EventArgs e)
        {
            
            if (textConvert.Count() != 0)
            {
                dataGridView1.Rows.Clear();
                for (int i = 0; i < textConvert.Count(); i = i + 2)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i / 2].Cells[0].Value = textConvert[i];
                    dataGridView1.Rows[i / 2].Cells[1].Value = "to";
                    dataGridView1.Rows[i / 2].Cells[2].Value = textConvert[i + 1];
                }                
            }
        }

        private void Text_Converter_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            StreamWriter sw = new StreamWriter(commonAppData + "//convertlibrary.seh");
            sw.WriteLine(textConvert.Count());
            for (int j = 0; j < textConvert.Count(); j++)            
                sw.WriteLine(textConvert[j]);            
            sw.Close();//close writer stream
            this.Hide();
            Thread t = new Thread(new ThreadStart(convert));
            t.Start();
            this.Hide();
        }
        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if ((textBox1.Text != "") && (textBox1.Text != "0000") && (textBox1.Text != " ") && (textBox1.Text != null))
                {
                    textConvert.Add(textBox1.Text);
                    textConvert.Add(textBox2.Text);

                    textBox1.Text = null;
                    textBox2.Text = null;
                    if (button2.Text == "Save") button2.Text = "Add";

                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < textConvert.Count(); i = i + 2)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i / 2].Cells[0].Value = textConvert[i];
                        dataGridView1.Rows[i / 2].Cells[1].Value = "to";
                        dataGridView1.Rows[i / 2].Cells[2].Value = textConvert[i + 1];
                    }
                    Thread t = new Thread(new ThreadStart(convert));
                    t.Start();
                }
            }
        }

        private void Text_Converter_Resize(object sender, EventArgs e)
        {
            textBox1.Width = (this.Width / 2) - 39;
            textBox2.Location =new Point( (this.Width / 2) + 10,33);
            textBox2.Width = (this.Width / 2) - 39;
        }
    }//end of class
}//end of namespace
