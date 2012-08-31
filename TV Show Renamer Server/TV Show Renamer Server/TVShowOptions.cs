using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TV_Show_Renamer_Server
{
    public partial class TVShowOptions : Form
    {
        List<TVShowSettings> MainTVShowList;

        public TVShowOptions(List<TVShowSettings> tvShowList)
        {
            InitializeComponent();
            MainTVShowList = tvShowList;
            foreach (TVShowSettings item in tvShowList)
            {
                listBox1.Items.Add(item.SearchName);
            }
        }

        public TVShowOptions(List<string> tvShowList)
        {
            InitializeComponent();
            foreach (string item in tvShowList)
            {
                listBox1.Items.Add(item);
            }
        }

        public TVShowOptions()
        {
            InitializeComponent();
        }
    }
}
