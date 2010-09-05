using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IMDBDLL;
using System.Threading;

namespace TV_show_Renamer
{
    public partial class imdb : Form
    {
        #region shit
        /// <summary>
        /// Delegate to call the processResults.
        /// </summary>
        public delegate void functionCall(int type, object result);

        /// <summary>
        /// Delegate to call the errorHandler.
        /// </summary>
        /// <param name="exc">The Exception.</param>
        public delegate void errorCall(Exception exc);

        /// <summary>
        /// Delegate to call the progressUpdater.
        /// </summary>
        /// <param name="value">Value to add to the progress bar.</param>
        public delegate void progressCall(int value);

        /// <summary>
        /// Event of functionCall.
        /// </summary>
        private event functionCall formFunctionCaller;

        /// <summary>
        /// Event of errorCall.
        /// </summary>
        private event errorCall formErrorCaller;

        /// <summary>
        /// Event of progressCall.
        /// </summary>
        private event progressCall formProgressCaller;

        /// <summary>
        /// Boolean that tells if there was an error.
        /// </summary>
        private bool error = false;

        /// <summary>
        /// The api manager instance.
        /// </summary>
        private IMDbManager manag; 
        #endregion
        
        Form1 main;

        List<string> mainlist = new List<string>();

        string title = null;
        int indexes = -1;
        int format = -1;
        int season = -1;
        int episode = -1;
        string imdbTitle = null;
        
        public imdb(Form1 temp, int selected, string showMainName, int tempformat)
        {
            InitializeComponent();

            formFunctionCaller += new functionCall(processResult);
            formErrorCaller += new errorCall(errorHandler);
            formProgressCaller += new progressCall(progressUpdater);

            main = temp;
            title = showMainName;
            format = tempformat;
            indexes = selected;
            button1.Visible = false;
            this.Show();

        }
        
        //get info for imdb
        private void button1_Click(object sender, EventArgs e)
        {
            //this.Show();
            //progressBar1.Show();
            //progressBar1.Value = 0;
            //title = main.getSelectedFileNames();
            //indexes = main.getSelected();
                            
                string test = title;
            
            //string test = title[0];
                int you = -1;

                for (int i = 1; i < 40; i++)
                {
                    //varable for break command later
                    bool end = false;

                    //loop for episodes
                    for (int j = 1; j < 100; j++)
                    {
                        string newi = i.ToString();
                        string newj = j.ToString();
                        //string output = null;
                        //check if i is less than 10
                        if (i < 10)
                        {
                            newi = "0" + i.ToString();
                        }
                        //check if j is less than 10
                        if (j < 10)
                        {
                            newj = "0" + j.ToString();
                        }

                        //make string to compare changed name too
                        //string startnewname = fileName;

                        switch (format)
                        {
                            case 1:
                                you = test.IndexOf(i.ToString() + "x" + newj);
                                break;
                            case 2:
                                you = test.IndexOf(newi + newj);
                                break;
                            case 3:
                                you = test.IndexOf(i.ToString() + newj);
                                break;
                            case 4:
                                you = test.IndexOf("S" + newi + "E" + newj);
                                you = test.IndexOf("S" + newi + "e" + newj);
                                break;
                        }
                        //stop loop when name is change                    
                        if (you != -1)
                        {
                            season = i;
                            episode = j;
                            imdbTitle = test.Remove(you - 1, test.Length - (you - 1));
                            end = true;
                            break;
                        }
                    }//end of episode loop

                    //stop loop when name is change
                    if (end)
                    {
                        break;
                    }

                }//end of season loop

                //MessageBox.Show("||" + imdbTitle + "||" + season + "||" +episode+ "||");

                bool[] fields = { true, true, true, true, true, true, true, true, true, true, true }; //Parses all the fields.
                progressBar1.Value = 0;
                error = false;

                manag = new IMDbManager();
                manag.parentFunctionCaller = formFunctionCaller;

                manag.parentErrorCaller = formErrorCaller;
                manag.parentProgressUpdaterCaller = formProgressCaller;

                manag.IMDbSearch(0, imdbTitle, 1, -1, season, season, fields);

                //this.dislplayResults();
                if (mainlist.Count() != 0)
                {
                    string youknow = mainlist[episode - 1];
                    youknow = youknow.Replace(":", "").Replace("&#x27;", "'").Replace("&#xE9;", "e").Replace("&#xE0;", "a").Replace("&#x26;", "&").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("\"", "");

                    if (main.addTitle(youknow, indexes))
                    {
                        //textBox1.Text = null;
                        Thread t = new Thread(new ThreadStart(convert));
                        t.Start();
                    }
                }
                this.Close();            
        }
        
        public void processResult(int type, object result)
        {
            List<string> yoyo = new List<string>();
            if (!error) // if no errors occured
            {
                if (type == 0) // if we get a title
                {
                    IMDbTitle title = (IMDbTitle)result;
                    Console.WriteLine(title.Title);
                    for (int i = 0; i < title.Seasons.Count; i++)
                    {
                        IMDbSerieSeason season = title.Seasons[i];
                        if (season.Episodes != null)
                        {
                            int q = 1;
                            foreach (IMDbSerieEpisode ep in season.Episodes)
                            {                               
                                if (ep.AirDate != "") {
                                    //yoyo.Add("Title: " + title.Title + " Season " + season.Number + " Episode " + q + ": " + ep.Title);
                                    yoyo.Add(ep.Title);
                                    q++;
                                }                                
                            }
                        }                                         
                    }
                }
                else if (type == 1) // if we get a result list
                {
                    List<IMDbLink> results = (List<IMDbLink>)result;

                    //Results res = new Results();
                    //res.setResults(results);
                    //if (res.ShowDialog() == DialogResult.OK)
                    manag.IMDbParse(results[0].URL);
                    //res.ShowDialog();

                }
            }
            //label1.Text = yoyo[episode];
            //Display toto = new Display(yoyo);
            if (yoyo.Count() > mainlist.Count()) {
                mainlist = yoyo;
            }
            //mainlist = yoyo;
            //episodeTitle = yoyo[episode];
            //label1.Text = yoyo[episode];
            
        }

        public void errorHandler(Exception exc)
        {
            error = true;
            progressBar1.Value = 0;
            MessageBox.Show(exc.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //MessageBox.Show("FU");
        }

        public void progressUpdater(int value)
        {
            progressBar1.Value += value;
        }

        //add title button
        private void button2_Click(object sender, EventArgs e)
        {
            if (label1.Text!="")
            {
                if (main.addTitle(label1.Text))
                {
                    //textBox1.Text = null;
                    Thread t = new Thread(new ThreadStart(convert));
                    t.Start();
                }
            }
        }

        //autoconvert method 
        private void convert()
        {
            main.autoConvert();
        }

        private void dislplayResults() {
            if (mainlist.Count() != 0)
            {
                label1.Text = mainlist[episode - 1];
            }
            else {
                MessageBox.Show("No Results Found");
            }
            
        }
    }//end of class
}
