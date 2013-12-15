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
	enum FileOptions { Overwrite, Rename, Skip };

	public partial class FileExists : Form
	{
		bool _all = false;
		FileOptions _DialogOutput;

		public FileExists(FileInfo newFile, FileInfo existingFile)
		{
			InitializeComponent();
			labelExistingFile.Text = existingFile.FullName;
			labelExistingSize.Text = existingFile.Length.ToString() + " bytes, "+existingFile.CreationTime.ToString("G");
			labelNewFile.Text = newFile.FullName;
			labelNewSize.Text = newFile.Length.ToString() + " bytes, " + newFile.CreationTime.ToString("G");
		}

		private void buttonOverWrite_Click(object sender, EventArgs e)
		{
			_DialogOutput = FileOptions.Overwrite;
		}

		private void buttonOverWriteAll_Click(object sender, EventArgs e)
		{
			_DialogOutput = FileOptions.Overwrite;
			_all = true;
		}

		private void buttonRename_Click(object sender, EventArgs e)
		{
			_DialogOutput = FileOptions.Rename;
		}

		private void buttonRenameAll_Click(object sender, EventArgs e)
		{
			_DialogOutput = FileOptions.Rename;
			_all = true;
		}

		private void buttonSkip_Click(object sender, EventArgs e)
		{
			_DialogOutput = FileOptions.Skip;
		}

		private void buttonSkipAll_Click(object sender, EventArgs e)
		{
			_DialogOutput = FileOptions.Skip;
			_all = true;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{

		}

		public FileOptions DialogOutput
		{
			get { return _DialogOutput; }
		}
		public bool AllFiles
		{
			get { return _all; }
		}
	}
}
