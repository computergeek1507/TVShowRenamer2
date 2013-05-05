using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TV_Show_Renamer
{
	public class LogWrite
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
			log = File.AppendText(logFolder + Path.DirectorySeparatorChar + "logfile.txt");			
			// Write to the file:
			log.WriteLine(DateTime.Now + " - Program started :)");
			//log.Close();
		}

		// Close Log
		public void closeLog()
		{
			if (log != null)
			{
				log.WriteLine(DateTime.Now + " - Program Closed :(");
				log.Close();
			}
		}

		//save function calls 
		public void WriteLog( string oldName,string newName)
		{
			log.WriteLine(DateTime.Now + " - [" + oldName + "] Saved as [" + newName + "]");
		}
	   
		//save function calls (List string)
		//public void convertWriteLog2( List<string> oldName,List<string> newName )
		//{
		 //   for (int i = 0; i < oldName.Count(); i++)			 
		 //	   log.WriteLine(DateTime.Now + " - [" + oldName[i] + "] Saved as [" + newName[i]+"]");
		//}
		
		//move function calls
		public void moveWriteLog(string oldName, string directory)
		{
			log.WriteLine(DateTime.Now + " - [" + oldName + "] Moved to [" + directory + "]");			
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
				log.WriteLine(DateTime.Now + " - " + i);			
		}
	}//end of LogWrite Class
}//end of namespace
