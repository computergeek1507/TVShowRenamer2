using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TV_show_Renamer
{
    class LogWrite
    {
        string logFolder = null;
        StreamWriter log;
        
        /// <summary>
        /// Start Log Writer
        /// </summary>
        /// <param name="folder">log location</param>
        public void startLog(string folder)
        {
            logFolder = folder;
                        
            // Create a writer and open the file:
            if (!File.Exists(logFolder+"//logfile.txt"))
            {
                log = new StreamWriter(logFolder + "//logfile.txt");
            }
            else
            {
                log = File.AppendText(logFolder + "//logfile.txt");
            }
            // Write to the file:
            log.WriteLine(DateTime.Now + " - Program started :)");
        }

        // Close Log
        public void closeLog()
        {
            log.WriteLine(DateTime.Now + " - Program Closed :(");
            log.Close();
        }

        //save function calls 
        public void WriteLog( string oldName,string newName)
        {
            log.WriteLine(DateTime.Now + " - " + oldName + " Saved as " + newName);
        }
       
        //save function calls (List String)
        public void convertWriteLog( List<string> oldName,List<string> newName )
        {
            for (int i = 0; i < oldName.Count(); i++) 
            {
                log.WriteLine(DateTime.Now + " - " + oldName[i] + " Saved as " + newName[i]);            
            }
        }
        
        //move function calls
        public void moveWriteLog(string oldName, string directory)
        {
            log.WriteLine(DateTime.Now + " - " + oldName + " Moved to " + directory);            
        }
        
        //write string
        public void WriteLog( string error)
        {            
            log.WriteLine(DateTime.Now + " - "+error);
        }

        //write string List
        public void WriteLog( List<string> error)
        {
            foreach (string i in error)
            {
                log.WriteLine(DateTime.Now + " - " + i);            
            }
        }
    }//end of LogWrite Class
}//end of namespace
