namespace TV_show_Renamer
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button3 = new System.Windows.Forms.Button();
            this.x01ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.s01E01ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button4 = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.capitalizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeExtraCrapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addForTitleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeYearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seasonEpisodeFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x01ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.s01E01ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTitleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textConverterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addJunkWordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.defaultSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTVFolderLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Select Folder with videos in it";
            this.folderBrowserDialog1.SelectedPath = "C:\\Users\\Scott\\Documents";
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(12, 445);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "Convert";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // x01ToolStripMenuItem1
            // 
            this.x01ToolStripMenuItem1.Name = "x01ToolStripMenuItem1";
            this.x01ToolStripMenuItem1.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(32, 19);
            // 
            // s01E01ToolStripMenuItem
            // 
            this.s01E01ToolStripMenuItem.Name = "s01E01ToolStripMenuItem";
            this.s01E01ToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // ToolStripMenuItem
            // 
            this.ToolStripMenuItem.Name = "ToolStripMenuItem";
            this.ToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.Location = new System.Drawing.Point(93, 445);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 2;
            this.button4.Text = "Undo";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            this.openFileDialog2.Multiselect = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.seasonEpisodeFormatToolStripMenuItem,
            this.otherOptionsToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(597, 24);
            this.menuStrip1.TabIndex = 23;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFilesToolStripMenuItem,
            this.addFolderToolStripMenuItem,
            this.clearListToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // addFilesToolStripMenuItem
            // 
            this.addFilesToolStripMenuItem.AutoToolTip = true;
            this.addFilesToolStripMenuItem.Name = "addFilesToolStripMenuItem";
            this.addFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.addFilesToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.addFilesToolStripMenuItem.Text = "Add Files";
            this.addFilesToolStripMenuItem.ToolTipText = "Add Files To be Coverted";
            this.addFilesToolStripMenuItem.Click += new System.EventHandler(this.addFilesToolStripMenuItem_Click);
            // 
            // addFolderToolStripMenuItem
            // 
            this.addFolderToolStripMenuItem.Name = "addFolderToolStripMenuItem";
            this.addFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.addFolderToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.addFolderToolStripMenuItem.Text = "Add Folder";
            this.addFolderToolStripMenuItem.ToolTipText = "Add a Folder of Files To be Coverted";
            this.addFolderToolStripMenuItem.Click += new System.EventHandler(this.addFolderToolStripMenuItem_Click);
            // 
            // clearListToolStripMenuItem
            // 
            this.clearListToolStripMenuItem.Name = "clearListToolStripMenuItem";
            this.clearListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.clearListToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.clearListToolStripMenuItem.Text = "Clear List ";
            this.clearListToolStripMenuItem.ToolTipText = "Clear All Files from List";
            this.clearListToolStripMenuItem.Click += new System.EventHandler(this.clearListToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convertToolStripMenuItem,
            this.convertToToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.capitalizeToolStripMenuItem,
            this.removeExtraCrapToolStripMenuItem,
            this.addForTitleToolStripMenuItem,
            this.removeYearToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.optionsToolStripMenuItem.Text = "Conversion Options";
            // 
            // convertToolStripMenuItem
            // 
            this.convertToolStripMenuItem.AutoToolTip = true;
            this.convertToolStripMenuItem.Checked = true;
            this.convertToolStripMenuItem.CheckOnClick = true;
            this.convertToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.convertToolStripMenuItem.Name = "convertToolStripMenuItem";
            this.convertToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.convertToolStripMenuItem.Text = "Convert \".\" to \" \"";
            // 
            // convertToToolStripMenuItem
            // 
            this.convertToToolStripMenuItem.Checked = true;
            this.convertToToolStripMenuItem.CheckOnClick = true;
            this.convertToToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.convertToToolStripMenuItem.Name = "convertToToolStripMenuItem";
            this.convertToToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.convertToToolStripMenuItem.Text = "Convert \"_\" to \" \"";
            this.convertToToolStripMenuItem.ToolTipText = "Convert \"_\" to \" \"";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Checked = true;
            this.removeToolStripMenuItem.CheckOnClick = true;
            this.removeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.removeToolStripMenuItem.Text = "Remove \"-\"";
            this.removeToolStripMenuItem.ToolTipText = "Remove all Dashes";
            // 
            // capitalizeToolStripMenuItem
            // 
            this.capitalizeToolStripMenuItem.Checked = true;
            this.capitalizeToolStripMenuItem.CheckOnClick = true;
            this.capitalizeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.capitalizeToolStripMenuItem.Name = "capitalizeToolStripMenuItem";
            this.capitalizeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.capitalizeToolStripMenuItem.Text = "Capitalize";
            this.capitalizeToolStripMenuItem.ToolTipText = "Capitalize First Letter After Spaces";
            // 
            // removeExtraCrapToolStripMenuItem
            // 
            this.removeExtraCrapToolStripMenuItem.Checked = true;
            this.removeExtraCrapToolStripMenuItem.CheckOnClick = true;
            this.removeExtraCrapToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.removeExtraCrapToolStripMenuItem.Name = "removeExtraCrapToolStripMenuItem";
            this.removeExtraCrapToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.removeExtraCrapToolStripMenuItem.Text = "Remove Extra Crap";
            this.removeExtraCrapToolStripMenuItem.ToolTipText = "Remove Stuff if in Junk Library";
            // 
            // addForTitleToolStripMenuItem
            // 
            this.addForTitleToolStripMenuItem.Checked = true;
            this.addForTitleToolStripMenuItem.CheckOnClick = true;
            this.addForTitleToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.addForTitleToolStripMenuItem.Name = "addForTitleToolStripMenuItem";
            this.addForTitleToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.addForTitleToolStripMenuItem.Text = "Add \" - \" for Title";
            this.addForTitleToolStripMenuItem.ToolTipText = "Add Dash if Title Is Added";
            // 
            // removeYearToolStripMenuItem
            // 
            this.removeYearToolStripMenuItem.Checked = true;
            this.removeYearToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.removeYearToolStripMenuItem.Name = "removeYearToolStripMenuItem";
            this.removeYearToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.removeYearToolStripMenuItem.Text = "Remove Year";
            this.removeYearToolStripMenuItem.ToolTipText = "Remove Year If Exists";
            // 
            // seasonEpisodeFormatToolStripMenuItem
            // 
            this.seasonEpisodeFormatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x01ToolStripMenuItem,
            this.toolStripMenuItem3,
            this.s01E01ToolStripMenuItem1,
            this.dateToolStripMenuItem});
            this.seasonEpisodeFormatToolStripMenuItem.Name = "seasonEpisodeFormatToolStripMenuItem";
            this.seasonEpisodeFormatToolStripMenuItem.Size = new System.Drawing.Size(143, 20);
            this.seasonEpisodeFormatToolStripMenuItem.Text = "Season/Episode Format";
            // 
            // x01ToolStripMenuItem
            // 
            this.x01ToolStripMenuItem.Checked = true;
            this.x01ToolStripMenuItem.CheckOnClick = true;
            this.x01ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.x01ToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.x01ToolStripMenuItem.Name = "x01ToolStripMenuItem";
            this.x01ToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.x01ToolStripMenuItem.Text = "1x01";
            this.x01ToolStripMenuItem.ToolTipText = "Sample Show 1x01.avi";
            this.x01ToolStripMenuItem.Click += new System.EventHandler(this.x01ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.CheckOnClick = true;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(155, 22);
            this.toolStripMenuItem3.Text = "0101";
            this.toolStripMenuItem3.ToolTipText = "Sample Show 0101.avi";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // s01E01ToolStripMenuItem1
            // 
            this.s01E01ToolStripMenuItem1.CheckOnClick = true;
            this.s01E01ToolStripMenuItem1.Name = "s01E01ToolStripMenuItem1";
            this.s01E01ToolStripMenuItem1.Size = new System.Drawing.Size(155, 22);
            this.s01E01ToolStripMenuItem1.Text = "S01E01";
            this.s01E01ToolStripMenuItem1.ToolTipText = "Sample Show S01E01.avi";
            this.s01E01ToolStripMenuItem1.Click += new System.EventHandler(this.s01E01ToolStripMenuItem1_Click);
            // 
            // dateToolStripMenuItem
            // 
            this.dateToolStripMenuItem.CheckOnClick = true;
            this.dateToolStripMenuItem.Name = "dateToolStripMenuItem";
            this.dateToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.dateToolStripMenuItem.Text = "1-1-2010 (Date)";
            this.dateToolStripMenuItem.ToolTipText = "Sample Show 1-1-2010.avi";
            this.dateToolStripMenuItem.Click += new System.EventHandler(this.dateToolStripMenuItem_Click);
            // 
            // otherOptionsToolStripMenuItem
            // 
            this.otherOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTitleToolStripMenuItem,
            this.textConverterToolStripMenuItem,
            this.addJunkWordsToolStripMenuItem,
            this.toolStripSeparator3,
            this.defaultSettingsToolStripMenuItem,
            this.setTVFolderLocationToolStripMenuItem});
            this.otherOptionsToolStripMenuItem.Name = "otherOptionsToolStripMenuItem";
            this.otherOptionsToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.otherOptionsToolStripMenuItem.Text = "Other Options";
            // 
            // addTitleToolStripMenuItem
            // 
            this.addTitleToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addTitleToolStripMenuItem.Name = "addTitleToolStripMenuItem";
            this.addTitleToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.addTitleToolStripMenuItem.Text = "Add Title...";
            this.addTitleToolStripMenuItem.ToolTipText = "Add Title After Season/Episode Data";
            this.addTitleToolStripMenuItem.Click += new System.EventHandler(this.addTitleToolStripMenuItem_Click);
            // 
            // textConverterToolStripMenuItem
            // 
            this.textConverterToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.textConverterToolStripMenuItem.Name = "textConverterToolStripMenuItem";
            this.textConverterToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.textConverterToolStripMenuItem.Text = "Text Converter...";
            this.textConverterToolStripMenuItem.ToolTipText = "Convert Text to other Text";
            this.textConverterToolStripMenuItem.Click += new System.EventHandler(this.textConverterToolStripMenuItem_Click);
            // 
            // addJunkWordsToolStripMenuItem
            // 
            this.addJunkWordsToolStripMenuItem.Name = "addJunkWordsToolStripMenuItem";
            this.addJunkWordsToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.addJunkWordsToolStripMenuItem.Text = "Add Junk Words...";
            this.addJunkWordsToolStripMenuItem.ToolTipText = "Make List of Junk Words";
            this.addJunkWordsToolStripMenuItem.Click += new System.EventHandler(this.addJunkWordsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(198, 6);
            // 
            // defaultSettingsToolStripMenuItem
            // 
            this.defaultSettingsToolStripMenuItem.Name = "defaultSettingsToolStripMenuItem";
            this.defaultSettingsToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.defaultSettingsToolStripMenuItem.Text = "Restore Default Settings";
            this.defaultSettingsToolStripMenuItem.ToolTipText = "Restore Default Settings of Form";
            this.defaultSettingsToolStripMenuItem.Click += new System.EventHandler(this.defaultSettingsToolStripMenuItem_Click);
            // 
            // setTVFolderLocationToolStripMenuItem
            // 
            this.setTVFolderLocationToolStripMenuItem.Name = "setTVFolderLocationToolStripMenuItem";
            this.setTVFolderLocationToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.setTVFolderLocationToolStripMenuItem.Text = "Set TV Folder Location...";
            this.setTVFolderLocationToolStripMenuItem.ToolTipText = "Set Folder For Files to be Copied To";
            this.setTVFolderLocationToolStripMenuItem.Click += new System.EventHandler(this.setTVFolderLocationToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForUpdatesToolStripMenuItem,
            this.toolStripSeparator2,
            this.aboutToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.ShowShortcutKeys = false;
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates...";
            this.checkForUpdatesToolStripMenuItem.ToolTipText = "Go Online and See If New Version Is Available";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(170, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.ToolTipText = "Useful Info";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.richTextBox1.DetectUrls = false;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.richTextBox1.Location = new System.Drawing.Point(12, 27);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(573, 406);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(336, 450);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 28;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(174, 445);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Move To";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(255, 445);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Copy To";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(597, 475);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(470, 130);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "TV Show Renamer 2.1 BETA";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripMenuItem x01ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem s01E01ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem capitalizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeExtraCrapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addForTitleToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripMenuItem seasonEpisodeFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x01ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem s01E01ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem dateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addTitleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textConverterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addJunkWordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem removeYearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setTVFolderLocationToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

