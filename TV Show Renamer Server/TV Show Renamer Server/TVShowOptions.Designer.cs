namespace TV_Show_Renamer_Server
{
    partial class TVShowOptions
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.showNameTextBox = new System.Windows.Forms.TextBox();
            this.TVDBNameTextBox = new System.Windows.Forms.TextBox();
            this.TVShowFolderTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.folderButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TVRageTextBox = new System.Windows.Forms.TextBox();
            this.TVDBIDTextBox = new System.Windows.Forms.TextBox();
            this.TVRageIDTextBox = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(313, 420);
            this.listBox1.TabIndex = 0;
            // 
            // showNameTextBox
            // 
            this.showNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.showNameTextBox.Location = new System.Drawing.Point(423, 9);
            this.showNameTextBox.Name = "showNameTextBox";
            this.showNameTextBox.Size = new System.Drawing.Size(349, 20);
            this.showNameTextBox.TabIndex = 1;
            // 
            // TVDBNameTextBox
            // 
            this.TVDBNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TVDBNameTextBox.Location = new System.Drawing.Point(423, 61);
            this.TVDBNameTextBox.Name = "TVDBNameTextBox";
            this.TVDBNameTextBox.ReadOnly = true;
            this.TVDBNameTextBox.Size = new System.Drawing.Size(349, 20);
            this.TVDBNameTextBox.TabIndex = 2;
            // 
            // TVShowFolderTextBox
            // 
            this.TVShowFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TVShowFolderTextBox.Location = new System.Drawing.Point(423, 35);
            this.TVShowFolderTextBox.Name = "TVShowFolderTextBox";
            this.TVShowFolderTextBox.Size = new System.Drawing.Size(316, 20);
            this.TVShowFolderTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(331, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "TV Show Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(331, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "TVDB Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(331, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "TV Show Folder:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(331, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "TVDB Series ID:";
            // 
            // folderButton
            // 
            this.folderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.folderButton.Location = new System.Drawing.Point(745, 33);
            this.folderButton.Name = "folderButton";
            this.folderButton.Size = new System.Drawing.Size(27, 23);
            this.folderButton.TabIndex = 9;
            this.folderButton.Text = "...";
            this.folderButton.UseVisualStyleBackColor = true;
            this.folderButton.Click += new System.EventHandler(this.folderButton_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(334, 165);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(198, 17);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Convert to TVDB Numbering System";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(334, 409);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(115, 23);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Save Settings";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(331, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "TV Rage Name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(331, 142);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "TV Rage ID:";
            // 
            // TVRageTextBox
            // 
            this.TVRageTextBox.Location = new System.Drawing.Point(423, 113);
            this.TVRageTextBox.Name = "TVRageTextBox";
            this.TVRageTextBox.ReadOnly = true;
            this.TVRageTextBox.Size = new System.Drawing.Size(349, 20);
            this.TVRageTextBox.TabIndex = 15;
            // 
            // TVDBIDTextBox
            // 
            this.TVDBIDTextBox.Location = new System.Drawing.Point(423, 87);
            this.TVDBIDTextBox.Name = "TVDBIDTextBox";
            this.TVDBIDTextBox.ReadOnly = true;
            this.TVDBIDTextBox.Size = new System.Drawing.Size(349, 20);
            this.TVDBIDTextBox.TabIndex = 16;
            // 
            // TVRageIDTextBox
            // 
            this.TVRageIDTextBox.Location = new System.Drawing.Point(423, 139);
            this.TVRageIDTextBox.Name = "TVRageIDTextBox";
            this.TVRageIDTextBox.ReadOnly = true;
            this.TVRageIDTextBox.Size = new System.Drawing.Size(349, 20);
            this.TVRageIDTextBox.TabIndex = 17;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(334, 188);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "Search For TVDB Info";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(467, 188);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "Search for TVRage Info";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(538, 165);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(89, 17);
            this.checkBox2.TabIndex = 20;
            this.checkBox2.Text = "Series Ended";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // TVShowOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 452);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TVRageIDTextBox);
            this.Controls.Add(this.TVDBIDTextBox);
            this.Controls.Add(this.TVRageTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.folderButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TVShowFolderTextBox);
            this.Controls.Add(this.TVDBNameTextBox);
            this.Controls.Add(this.showNameTextBox);
            this.Controls.Add(this.listBox1);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "TVShowOptions";
            this.Text = "TVShowOptions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox showNameTextBox;
        private System.Windows.Forms.TextBox TVDBNameTextBox;
		private System.Windows.Forms.TextBox TVShowFolderTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button folderButton;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox TVRageTextBox;
		private System.Windows.Forms.TextBox TVDBIDTextBox;
		private System.Windows.Forms.TextBox TVRageIDTextBox;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox2;


    }
}