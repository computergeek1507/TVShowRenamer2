using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV_show_Renamer
{
    class SearchInfo
    {
        string _title;
        int _selected;
        
        public SearchInfo(string title, int selected)
        {
            _title = title;
            _selected = selected;
        }

        public string Title
        {
            get { return _title; }            
        }

        public int SelectedValue
        {
            get { return _selected; }            
        }
    }//end of class
}//end of namespace
