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
    public partial class StatusBar : Form
    {
        public StatusBar(int size)
        {
            InitializeComponent();
        }

        public void ProgressBarSize(int size)
        {
            progressBar1.Maximum = size;        
        }

        public void ProgressBarSet(int progress)
        {
            if (progress < progressBar1.Maximum) {
            progressBar1.Value = progress;
            }
            if (progress == progressBar1.Maximum)
            {
                this.Hide();
            }
        }
    }//end of class
}//end of namespace
