using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV_Show_Renamer
{
    public class SearchInfo
    {
        string _title;
        int _selected;
        
        public SearchInfo(string title, int selected)
        {
            _title = title;
            _selected = selected;
        }
        public SearchInfo()
        {
            _title = "";
            _selected = -1;
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public int SelectedValue
        {
            get { return _selected; }
            set { _selected = value; }
        }
    }//end of class
}//end of namespace
