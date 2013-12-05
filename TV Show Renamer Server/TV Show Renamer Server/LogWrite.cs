using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TV_Show_Renamer_Server
{
	public class LogWrite
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		string logFolder = null;
		//StreamWriter log;
		
		//save function calls 
		public void WriteLog( string oldName,string newName)
		{
			log.Info("[" + oldName + "] Saved as [" + newName + "]");
		}
	   
		//save function calls (List String)
		//public void convertWriteLog2( List<string> oldName,List<string> newName )
		//{
		 //   for (int i = 0; i < oldName.Count(); i++)			 
		 //	   log.WriteLine(DateTime.Now + " - [" + oldName[i] + "] Saved as [" + newName[i]+"]");
		//}
		
		//move function calls
		public void moveWriteLog(string oldName, string directory)
		{
			log.Info("[" + oldName + "] Moved to [" + directory + "]");			
		}
		
		//write string
		public void WriteLog( string error)
		{			
			log.Info(error);
		}

		//write string List
		public void WriteLog( List<string> error)
		{
			foreach (string i in error)			
				log.Info(i);			
		}
	}//end of LogWrite Class
}//end of namespace
