namespace TV_Show_Renamer
{
	partial class FileExists
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelExistingSize = new System.Windows.Forms.Label();
			this.labelExistingFile = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.labelNewSize = new System.Windows.Forms.Label();
			this.labelNewFile = new System.Windows.Forms.Label();
			this.buttonOverWrite = new System.Windows.Forms.Button();
			this.buttonOverWriteAll = new System.Windows.Forms.Button();
			this.buttonRename = new System.Windows.Forms.Button();
			this.buttonRenameAll = new System.Windows.Forms.Button();
			this.buttonSkip = new System.Windows.Forms.Button();
			this.buttonSkipAll = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.labelExistingSize);
			this.groupBox1.Controls.Add(this.labelExistingFile);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(392, 61);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Would you like to replace the existing file";
			// 
			// labelExistingSize
			// 
			this.labelExistingSize.AutoSize = true;
			this.labelExistingSize.Location = new System.Drawing.Point(7, 37);
			this.labelExistingSize.Name = "labelExistingSize";
			this.labelExistingSize.Size = new System.Drawing.Size(35, 13);
			this.labelExistingSize.TabIndex = 1;
			this.labelExistingSize.Text = "label2";
			// 
			// labelExistingFile
			// 
			this.labelExistingFile.AutoSize = true;
			this.labelExistingFile.Location = new System.Drawing.Point(7, 20);
			this.labelExistingFile.Name = "labelExistingFile";
			this.labelExistingFile.Size = new System.Drawing.Size(35, 13);
			this.labelExistingFile.TabIndex = 0;
			this.labelExistingFile.Text = "label1";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.labelNewSize);
			this.groupBox2.Controls.Add(this.labelNewFile);
			this.groupBox2.Location = new System.Drawing.Point(12, 79);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(392, 59);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "with this one?";
			// 
			// labelNewSize
			// 
			this.labelNewSize.AutoSize = true;
			this.labelNewSize.Location = new System.Drawing.Point(7, 37);
			this.labelNewSize.Name = "labelNewSize";
			this.labelNewSize.Size = new System.Drawing.Size(35, 13);
			this.labelNewSize.TabIndex = 1;
			this.labelNewSize.Text = "label4";
			// 
			// labelNewFile
			// 
			this.labelNewFile.AutoSize = true;
			this.labelNewFile.Location = new System.Drawing.Point(7, 20);
			this.labelNewFile.Name = "labelNewFile";
			this.labelNewFile.Size = new System.Drawing.Size(35, 13);
			this.labelNewFile.TabIndex = 0;
			this.labelNewFile.Text = "label3";
			// 
			// buttonOverWrite
			// 
			this.buttonOverWrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOverWrite.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOverWrite.Location = new System.Drawing.Point(409, 20);
			this.buttonOverWrite.Name = "buttonOverWrite";
			this.buttonOverWrite.Size = new System.Drawing.Size(75, 23);
			this.buttonOverWrite.TabIndex = 2;
			this.buttonOverWrite.Text = "Overwrite";
			this.buttonOverWrite.UseVisualStyleBackColor = true;
			this.buttonOverWrite.Click += new System.EventHandler(this.buttonOverWrite_Click);
			// 
			// buttonOverWriteAll
			// 
			this.buttonOverWriteAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOverWriteAll.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOverWriteAll.Location = new System.Drawing.Point(490, 20);
			this.buttonOverWriteAll.Name = "buttonOverWriteAll";
			this.buttonOverWriteAll.Size = new System.Drawing.Size(44, 23);
			this.buttonOverWriteAll.TabIndex = 3;
			this.buttonOverWriteAll.Text = "All";
			this.buttonOverWriteAll.UseVisualStyleBackColor = true;
			this.buttonOverWriteAll.Click += new System.EventHandler(this.buttonOverWriteAll_Click);
			// 
			// buttonRename
			// 
			this.buttonRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRename.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonRename.Location = new System.Drawing.Point(409, 51);
			this.buttonRename.Name = "buttonRename";
			this.buttonRename.Size = new System.Drawing.Size(75, 23);
			this.buttonRename.TabIndex = 4;
			this.buttonRename.Text = "Rename";
			this.buttonRename.UseVisualStyleBackColor = true;
			this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
			// 
			// buttonRenameAll
			// 
			this.buttonRenameAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRenameAll.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonRenameAll.Location = new System.Drawing.Point(490, 51);
			this.buttonRenameAll.Name = "buttonRenameAll";
			this.buttonRenameAll.Size = new System.Drawing.Size(44, 23);
			this.buttonRenameAll.TabIndex = 5;
			this.buttonRenameAll.Text = "All";
			this.buttonRenameAll.UseVisualStyleBackColor = true;
			this.buttonRenameAll.Click += new System.EventHandler(this.buttonRenameAll_Click);
			// 
			// buttonSkip
			// 
			this.buttonSkip.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonSkip.Location = new System.Drawing.Point(409, 81);
			this.buttonSkip.Name = "buttonSkip";
			this.buttonSkip.Size = new System.Drawing.Size(75, 23);
			this.buttonSkip.TabIndex = 6;
			this.buttonSkip.Text = "Skip";
			this.buttonSkip.UseVisualStyleBackColor = true;
			this.buttonSkip.Click += new System.EventHandler(this.buttonSkip_Click);
			// 
			// buttonSkipAll
			// 
			this.buttonSkipAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSkipAll.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonSkipAll.Location = new System.Drawing.Point(491, 81);
			this.buttonSkipAll.Name = "buttonSkipAll";
			this.buttonSkipAll.Size = new System.Drawing.Size(43, 23);
			this.buttonSkipAll.TabIndex = 7;
			this.buttonSkipAll.Text = "All";
			this.buttonSkipAll.UseVisualStyleBackColor = true;
			this.buttonSkipAll.Click += new System.EventHandler(this.buttonSkipAll_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(409, 110);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(124, 23);
			this.buttonCancel.TabIndex = 8;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// FileExists
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(543, 146);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonSkipAll);
			this.Controls.Add(this.buttonSkip);
			this.Controls.Add(this.buttonRenameAll);
			this.Controls.Add(this.buttonRename);
			this.Controls.Add(this.buttonOverWriteAll);
			this.Controls.Add(this.buttonOverWrite);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.MinimumSize = new System.Drawing.Size(400, 184);
			this.Name = "FileExists";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "File Already Exists";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label labelExistingSize;
		private System.Windows.Forms.Label labelExistingFile;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label labelNewSize;
		private System.Windows.Forms.Label labelNewFile;
		private System.Windows.Forms.Button buttonOverWrite;
		private System.Windows.Forms.Button buttonOverWriteAll;
		private System.Windows.Forms.Button buttonRename;
		private System.Windows.Forms.Button buttonRenameAll;
		private System.Windows.Forms.Button buttonSkip;
		private System.Windows.Forms.Button buttonSkipAll;
		private System.Windows.Forms.Button buttonCancel;
	}
}