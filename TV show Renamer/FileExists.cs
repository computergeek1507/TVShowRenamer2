using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TV_Show_Renamer
{
	public partial class FileExists : Form
	{
		bool _all = false;

		public FileExists(FileInfo newFile, FileInfo existingFile)
		{
			InitializeComponent();
		}

		private void buttonOverWrite_Click(object sender, EventArgs e)
		{

		}

		private void buttonOverWriteAll_Click(object sender, EventArgs e)
		{
			_all = true;
		}

		private void buttonRename_Click(object sender, EventArgs e)
		{

		}

		private void buttonRenameAll_Click(object sender, EventArgs e)
		{
			_all = true;
		}

		private void buttonSkip_Click(object sender, EventArgs e)
		{

		}

		private void buttonSkipAll_Click(object sender, EventArgs e)
		{
			_all = true;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{

		}
		
	}
}
