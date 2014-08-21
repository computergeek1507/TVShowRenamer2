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
			this.listBoxTVShowList = new System.Windows.Forms.ListBox();
			this.showNameTextBox = new System.Windows.Forms.TextBox();
			this.TVDBNameTextBox = new System.Windows.Forms.TextBox();
			this.TVShowFolderTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.folderButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.labelTMDbName = new System.Windows.Forms.Label();
			this.labelTMDbID = new System.Windows.Forms.Label();
			this.TMDbTextBox = new System.Windows.Forms.TextBox();
			this.TVDBIDTextBox = new System.Windows.Forms.TextBox();
			this.TMDbIDTextBox = new System.Windows.Forms.TextBox();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.listBoxSeasons = new System.Windows.Forms.ListBox();
			this.comboBoxEpisodeSelect = new System.Windows.Forms.ComboBox();
			this.buttonSet = new System.Windows.Forms.Button();
			this.buttonAddShow = new System.Windows.Forms.Button();
			this.buttonRemoveShow = new System.Windows.Forms.Button();
			this.buttonGetEpisodes = new System.Windows.Forms.Button();
			this.buttonSearchFolder = new System.Windows.Forms.Button();
			this.checkBoxHD = new System.Windows.Forms.CheckBox();
			this.folderHDButton = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.TVShowFolderHDTextBox = new System.Windows.Forms.TextBox();
			this.buttonSkip = new System.Windows.Forms.Button();
			this.checkBoxSkip = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// listBoxTVShowList
			// 
			this.listBoxTVShowList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listBoxTVShowList.FormattingEnabled = true;
			this.listBoxTVShowList.Location = new System.Drawing.Point(12, 12);
			this.listBoxTVShowList.Name = "listBoxTVShowList";
			this.listBoxTVShowList.Size = new System.Drawing.Size(209, 654);
			this.listBoxTVShowList.TabIndex = 0;
			// 
			// showNameTextBox
			// 
			this.showNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.showNameTextBox.Location = new System.Drawing.Point(318, 9);
			this.showNameTextBox.Name = "showNameTextBox";
			this.showNameTextBox.Size = new System.Drawing.Size(757, 20);
			this.showNameTextBox.TabIndex = 1;
			// 
			// TVDBNameTextBox
			// 
			this.TVDBNameTextBox.Location = new System.Drawing.Point(319, 61);
			this.TVDBNameTextBox.Name = "TVDBNameTextBox";
			this.TVDBNameTextBox.ReadOnly = true;
			this.TVDBNameTextBox.Size = new System.Drawing.Size(347, 20);
			this.TVDBNameTextBox.TabIndex = 2;
			// 
			// TVShowFolderTextBox
			// 
			this.TVShowFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TVShowFolderTextBox.Location = new System.Drawing.Point(319, 35);
			this.TVShowFolderTextBox.Name = "TVShowFolderTextBox";
			this.TVShowFolderTextBox.Size = new System.Drawing.Size(314, 20);
			this.TVShowFolderTextBox.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(227, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "TV Show Name:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(227, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "TVDB Name:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(227, 38);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(86, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "TV Show Folder:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(677, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(85, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "TVDB Series ID:";
			// 
			// folderButton
			// 
			this.folderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.folderButton.Location = new System.Drawing.Point(639, 33);
			this.folderButton.Name = "folderButton";
			this.folderButton.Size = new System.Drawing.Size(27, 23);
			this.folderButton.TabIndex = 9;
			this.folderButton.Text = "...";
			this.folderButton.UseVisualStyleBackColor = true;
			this.folderButton.Click += new System.EventHandler(this.folderButton_Click);
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(1013, 677);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(115, 23);
			this.saveButton.TabIndex = 11;
			this.saveButton.Text = "Save Settings";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.button2_Click);
			// 
			// labelTMDbName
			// 
			this.labelTMDbName.AutoSize = true;
			this.labelTMDbName.Location = new System.Drawing.Point(227, 90);
			this.labelTMDbName.Name = "labelTMDbName";
			this.labelTMDbName.Size = new System.Drawing.Size(71, 13);
			this.labelTMDbName.TabIndex = 12;
			this.labelTMDbName.Text = "TMDb Name:";
			// 
			// labelTMDbID
			// 
			this.labelTMDbID.AutoSize = true;
			this.labelTMDbID.Location = new System.Drawing.Point(677, 90);
			this.labelTMDbID.Name = "labelTMDbID";
			this.labelTMDbID.Size = new System.Drawing.Size(54, 13);
			this.labelTMDbID.TabIndex = 13;
			this.labelTMDbID.Text = "TMDb ID:";
			// 
			// TMDbTextBox
			// 
			this.TMDbTextBox.Location = new System.Drawing.Point(319, 87);
			this.TMDbTextBox.Name = "TMDbTextBox";
			this.TMDbTextBox.ReadOnly = true;
			this.TMDbTextBox.Size = new System.Drawing.Size(347, 20);
			this.TMDbTextBox.TabIndex = 15;
			// 
			// TVDBIDTextBox
			// 
			this.TVDBIDTextBox.Location = new System.Drawing.Point(788, 61);
			this.TVDBIDTextBox.Name = "TVDBIDTextBox";
			this.TVDBIDTextBox.ReadOnly = true;
			this.TVDBIDTextBox.Size = new System.Drawing.Size(339, 20);
			this.TVDBIDTextBox.TabIndex = 16;
			// 
			// TMDbIDTextBox
			// 
			this.TMDbIDTextBox.Location = new System.Drawing.Point(788, 87);
			this.TMDbIDTextBox.Name = "TMDbIDTextBox";
			this.TMDbIDTextBox.ReadOnly = true;
			this.TMDbIDTextBox.Size = new System.Drawing.Size(339, 20);
			this.TMDbIDTextBox.TabIndex = 17;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(230, 113);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(127, 23);
			this.button1.TabIndex = 18;
			this.button1.Text = "Search For TVDB Info";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(363, 113);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(119, 23);
			this.button2.TabIndex = 19;
			this.button2.Text = "Search for TMDb Info";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click_1);
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point(488, 117);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(89, 17);
			this.checkBox2.TabIndex = 20;
			this.checkBox2.Text = "Series Ended";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// dataGridView1
			// 
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(373, 142);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(754, 524);
			this.dataGridView1.TabIndex = 21;
			// 
			// listBoxSeasons
			// 
			this.listBoxSeasons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listBoxSeasons.FormattingEnabled = true;
			this.listBoxSeasons.Location = new System.Drawing.Point(230, 142);
			this.listBoxSeasons.Name = "listBoxSeasons";
			this.listBoxSeasons.Size = new System.Drawing.Size(137, 524);
			this.listBoxSeasons.TabIndex = 22;
			this.listBoxSeasons.SelectedIndexChanged += new System.EventHandler(this.listBoxSeasons_SelectedIndexChanged);
			// 
			// comboBoxEpisodeSelect
			// 
			this.comboBoxEpisodeSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxEpisodeSelect.FormattingEnabled = true;
			this.comboBoxEpisodeSelect.Location = new System.Drawing.Point(832, 115);
			this.comboBoxEpisodeSelect.Name = "comboBoxEpisodeSelect";
			this.comboBoxEpisodeSelect.Size = new System.Drawing.Size(219, 21);
			this.comboBoxEpisodeSelect.TabIndex = 23;
			// 
			// buttonSet
			// 
			this.buttonSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSet.Location = new System.Drawing.Point(1057, 113);
			this.buttonSet.Name = "buttonSet";
			this.buttonSet.Size = new System.Drawing.Size(70, 23);
			this.buttonSet.TabIndex = 24;
			this.buttonSet.Text = "Set";
			this.buttonSet.UseVisualStyleBackColor = true;
			// 
			// buttonAddShow
			// 
			this.buttonAddShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonAddShow.Location = new System.Drawing.Point(12, 677);
			this.buttonAddShow.Name = "buttonAddShow";
			this.buttonAddShow.Size = new System.Drawing.Size(69, 23);
			this.buttonAddShow.TabIndex = 25;
			this.buttonAddShow.Text = "Add...";
			this.buttonAddShow.UseVisualStyleBackColor = true;
			this.buttonAddShow.Click += new System.EventHandler(this.buttonAddShow_Click);
			// 
			// buttonRemoveShow
			// 
			this.buttonRemoveShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonRemoveShow.Location = new System.Drawing.Point(87, 677);
			this.buttonRemoveShow.Name = "buttonRemoveShow";
			this.buttonRemoveShow.Size = new System.Drawing.Size(68, 23);
			this.buttonRemoveShow.TabIndex = 26;
			this.buttonRemoveShow.Text = "Remove...";
			this.buttonRemoveShow.UseVisualStyleBackColor = true;
			this.buttonRemoveShow.Click += new System.EventHandler(this.buttonRemoveShow_Click);
			// 
			// buttonGetEpisodes
			// 
			this.buttonGetEpisodes.Location = new System.Drawing.Point(648, 113);
			this.buttonGetEpisodes.Name = "buttonGetEpisodes";
			this.buttonGetEpisodes.Size = new System.Drawing.Size(78, 23);
			this.buttonGetEpisodes.TabIndex = 27;
			this.buttonGetEpisodes.Text = "Get Episodes";
			this.buttonGetEpisodes.UseVisualStyleBackColor = true;
			this.buttonGetEpisodes.Click += new System.EventHandler(this.buttonGetEpisodes_Click);
			// 
			// buttonSearchFolder
			// 
			this.buttonSearchFolder.Location = new System.Drawing.Point(736, 113);
			this.buttonSearchFolder.Name = "buttonSearchFolder";
			this.buttonSearchFolder.Size = new System.Drawing.Size(90, 23);
			this.buttonSearchFolder.TabIndex = 28;
			this.buttonSearchFolder.Text = "Search Folder";
			this.buttonSearchFolder.UseVisualStyleBackColor = true;
			this.buttonSearchFolder.Click += new System.EventHandler(this.buttonSearchFolder_Click);
			// 
			// checkBoxHD
			// 
			this.checkBoxHD.AutoSize = true;
			this.checkBoxHD.Location = new System.Drawing.Point(583, 117);
			this.checkBoxHD.Name = "checkBoxHD";
			this.checkBoxHD.Size = new System.Drawing.Size(62, 17);
			this.checkBoxHD.TabIndex = 29;
			this.checkBoxHD.Text = "Get HD";
			this.checkBoxHD.UseVisualStyleBackColor = true;
			// 
			// folderHDButton
			// 
			this.folderHDButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.folderHDButton.Location = new System.Drawing.Point(1103, 33);
			this.folderHDButton.Name = "folderHDButton";
			this.folderHDButton.Size = new System.Drawing.Size(24, 23);
			this.folderHDButton.TabIndex = 32;
			this.folderHDButton.Text = "...";
			this.folderHDButton.UseVisualStyleBackColor = true;
			this.folderHDButton.Click += new System.EventHandler(this.folderHDButton_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(677, 38);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(105, 13);
			this.label5.TabIndex = 31;
			this.label5.Text = "HD TV Show Folder:";
			// 
			// TVShowFolderHDTextBox
			// 
			this.TVShowFolderHDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TVShowFolderHDTextBox.Location = new System.Drawing.Point(788, 35);
			this.TVShowFolderHDTextBox.Name = "TVShowFolderHDTextBox";
			this.TVShowFolderHDTextBox.Size = new System.Drawing.Size(309, 20);
			this.TVShowFolderHDTextBox.TabIndex = 30;
			// 
			// buttonSkip
			// 
			this.buttonSkip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonSkip.Location = new System.Drawing.Point(161, 677);
			this.buttonSkip.Name = "buttonSkip";
			this.buttonSkip.Size = new System.Drawing.Size(60, 23);
			this.buttonSkip.TabIndex = 33;
			this.buttonSkip.Text = "Skip";
			this.buttonSkip.UseVisualStyleBackColor = true;
			this.buttonSkip.Click += new System.EventHandler(this.buttonSkip_Click);
			// 
			// checkBoxSkip
			// 
			this.checkBoxSkip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxSkip.AutoSize = true;
			this.checkBoxSkip.Location = new System.Drawing.Point(1081, 11);
			this.checkBoxSkip.Name = "checkBoxSkip";
			this.checkBoxSkip.Size = new System.Drawing.Size(47, 17);
			this.checkBoxSkip.TabIndex = 34;
			this.checkBoxSkip.Text = "Skip";
			this.checkBoxSkip.UseVisualStyleBackColor = true;
			// 
			// TVShowOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1140, 712);
			this.Controls.Add(this.checkBoxSkip);
			this.Controls.Add(this.buttonSkip);
			this.Controls.Add(this.folderHDButton);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.TVShowFolderHDTextBox);
			this.Controls.Add(this.checkBoxHD);
			this.Controls.Add(this.buttonSearchFolder);
			this.Controls.Add(this.buttonGetEpisodes);
			this.Controls.Add(this.buttonRemoveShow);
			this.Controls.Add(this.buttonAddShow);
			this.Controls.Add(this.buttonSet);
			this.Controls.Add(this.comboBoxEpisodeSelect);
			this.Controls.Add(this.listBoxSeasons);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.TMDbIDTextBox);
			this.Controls.Add(this.TVDBIDTextBox);
			this.Controls.Add(this.TMDbTextBox);
			this.Controls.Add(this.labelTMDbID);
			this.Controls.Add(this.labelTMDbName);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.folderButton);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.TVShowFolderTextBox);
			this.Controls.Add(this.TVDBNameTextBox);
			this.Controls.Add(this.showNameTextBox);
			this.Controls.Add(this.listBoxTVShowList);
			this.MinimumSize = new System.Drawing.Size(400, 300);
			this.Name = "TVShowOptions";
			this.Text = "TVShowOptions";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox listBoxTVShowList;
		private System.Windows.Forms.TextBox showNameTextBox;
		private System.Windows.Forms.TextBox TVDBNameTextBox;
		private System.Windows.Forms.TextBox TVShowFolderTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button folderButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Label labelTMDbName;
		private System.Windows.Forms.Label labelTMDbID;
		private System.Windows.Forms.TextBox TMDbTextBox;
		private System.Windows.Forms.TextBox TVDBIDTextBox;
		private System.Windows.Forms.TextBox TMDbIDTextBox;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.ListBox listBoxSeasons;
		private System.Windows.Forms.ComboBox comboBoxEpisodeSelect;
		private System.Windows.Forms.Button buttonSet;
		private System.Windows.Forms.Button buttonAddShow;
		private System.Windows.Forms.Button buttonRemoveShow;
		private System.Windows.Forms.Button buttonGetEpisodes;
		private System.Windows.Forms.Button buttonSearchFolder;
		private System.Windows.Forms.CheckBox checkBoxHD;
		private System.Windows.Forms.Button folderHDButton;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox TVShowFolderHDTextBox;
		private System.Windows.Forms.Button buttonSkip;
		private System.Windows.Forms.CheckBox checkBoxSkip;


	}
}