namespace TV_Show_Renamer
{
    partial class CopyFilesDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Lab_TotalFiles = new System.Windows.Forms.Label();
            this.Lab_CurrentFile = new System.Windows.Forms.Label();
            this.Prog_TotalFiles = new System.Windows.Forms.ProgressBar();
            this.Prog_CurrentFile = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Lab_TotalFiles
            // 
            this.Lab_TotalFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Lab_TotalFiles.Location = new System.Drawing.Point(12, 13);
            this.Lab_TotalFiles.Name = "Lab_TotalFiles";
            this.Lab_TotalFiles.Size = new System.Drawing.Size(392, 18);
            this.Lab_TotalFiles.TabIndex = 0;
            this.Lab_TotalFiles.Text = "label1";
            // 
            // Lab_CurrentFile
            // 
            this.Lab_CurrentFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Lab_CurrentFile.Location = new System.Drawing.Point(12, 65);
            this.Lab_CurrentFile.Name = "Lab_CurrentFile";
            this.Lab_CurrentFile.Size = new System.Drawing.Size(392, 17);
            this.Lab_CurrentFile.TabIndex = 1;
            this.Lab_CurrentFile.Text = "label2";
            // 
            // Prog_TotalFiles
            // 
            this.Prog_TotalFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Prog_TotalFiles.Location = new System.Drawing.Point(12, 34);
            this.Prog_TotalFiles.Name = "Prog_TotalFiles";
            this.Prog_TotalFiles.Size = new System.Drawing.Size(392, 23);
            this.Prog_TotalFiles.TabIndex = 2;
            // 
            // Prog_CurrentFile
            // 
            this.Prog_CurrentFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Prog_CurrentFile.Location = new System.Drawing.Point(12, 85);
            this.Prog_CurrentFile.Name = "Prog_CurrentFile";
            this.Prog_CurrentFile.Size = new System.Drawing.Size(392, 23);
            this.Prog_CurrentFile.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(173, 114);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.But_Cancel_Click);
            // 
            // CopyFilesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 142);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.Prog_CurrentFile);
            this.Controls.Add(this.Prog_TotalFiles);
            this.Controls.Add(this.Lab_CurrentFile);
            this.Controls.Add(this.Lab_TotalFiles);
            this.Name = "CopyFilesDialog";
            this.Text = "Copying Files...";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Lab_TotalFiles;
        private System.Windows.Forms.Label Lab_CurrentFile;
        private System.Windows.Forms.ProgressBar Prog_TotalFiles;
        private System.Windows.Forms.ProgressBar Prog_CurrentFile;
        private System.Windows.Forms.Button btnCancel;
    }
}