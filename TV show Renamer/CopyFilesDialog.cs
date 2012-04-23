using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TV_Show_Renamer
{
    public partial class CopyFilesDialog : Form
    {
        private CopyFiles f;

        public CopyFilesDialog(List<string> sourceFiles, List<string> destFiles, bool moveNotCopy)
        {
            InitializeComponent();
            f = new CopyFiles(sourceFiles, destFiles,moveNotCopy);
            f.ProgressEvent += new CopyFiles.CopyProgressHandlerDelegate(update);
            f.CopyComplete += new CopyFiles.FileCopyCompleteEventHandler(f_CopyComplete);
            f.FileCopied += new CopyFiles.FileCopiedEventHandler(f_FileCopied);
            if (!moveNotCopy)
            {
                this.Text = "Moving Files";
            }
            f.Copy();
        }

        void f_FileCopied(object sender, FileCopiedEventArgs e)
        {
            if (e.Error != null)
                e.Cancel = true;
        }

        void f_CopyComplete(object sender, FileCopyCompleteEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new CopyFiles.FileCopyCompleteEventHandler(f_CopyComplete), new object[] { sender, e });
                return;
            }

            if (e.Cancel || e.LastError != null)
                DialogResult = System.Windows.Forms.DialogResult.Cancel;

            if (e.LastError != null)
            {
                btnCancel.Text = "OK";
                MessageBox.Show(e.LastError.Message);
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        // Methods
        private void update(Int32 totalFiles, Int32 copiedFiles, Int64 totalBytes, Int64 copiedBytes, String currentFilename)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new CopyFiles.CopyProgressHandlerDelegate(update), new object[] { totalFiles, copiedFiles, totalBytes, copiedBytes, currentFilename });
                return;
            }

            Prog_TotalFiles.Maximum = totalFiles;
            Prog_TotalFiles.Value = copiedFiles;
            Prog_CurrentFile.Maximum = 100;
            if (totalBytes != 0)
            {
                Prog_CurrentFile.Value = Convert.ToInt32((100f / (totalBytes / 1024f)) * (copiedBytes / 1024f));
            }

            Lab_TotalFiles.Text = "Total files (" + copiedFiles + "/" + totalFiles + ")";
            Lab_CurrentFile.Text = currentFilename;
        }

        private void But_Cancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "OK")
                Close();
            else
            {
                f.CancelCopy();
                btnCancel.Enabled = false;
            }
        }

        private void frmCopyFiles_Closed(object sender, System.EventArgs e)
        {
            f.CancelCopy();
        }

        private void frmCopyFiles_Shown(object sender, EventArgs e)
        {
            f.Copy();
        }
    }
}
