using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Web;
using System.Text;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]


namespace TV_Show_Renamer_Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			string path = Application.StartupPath;
            int index = path.IndexOf(@"\bin\");
            if (-1 != index)
            {
                // resource files are in project folder
                path = path.Substring(0, index);
            }
			Manager.Instance.Startup(8080, path);
			
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
