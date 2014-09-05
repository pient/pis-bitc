namespace ActiveRecordGenerator
{
	partial class GeneratorForm
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkLbTables = new System.Windows.Forms.CheckedListBox();
            this.btnTablesSelectAll = new System.Windows.Forms.Button();
            this.btnTablesSelectNone = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.chkPartial = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkPropChange = new System.Windows.Forms.CheckBox();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkValidation = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkLbTemplates = new System.Windows.Forms.CheckedListBox();
            this.btnTemplatesSelectNone = new System.Windows.Forms.Button();
            this.btnTemplatesSelectAll = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.btnTemplatesReload = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(385, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(73, 46);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect and Scan";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(101, 12);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(278, 20);
            this.txtServer.TabIndex = 1;
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(101, 38);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(278, 20);
            this.txtDatabase.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Database";
            // 
            // chkLbTables
            // 
            this.chkLbTables.CheckOnClick = true;
            this.chkLbTables.FormattingEnabled = true;
            this.chkLbTables.Location = new System.Drawing.Point(467, 28);
            this.chkLbTables.Name = "chkLbTables";
            this.chkLbTables.Size = new System.Drawing.Size(214, 319);
            this.chkLbTables.Sorted = true;
            this.chkLbTables.TabIndex = 19;
            // 
            // btnTablesSelectAll
            // 
            this.btnTablesSelectAll.Location = new System.Drawing.Point(687, 28);
            this.btnTablesSelectAll.Name = "btnTablesSelectAll";
            this.btnTablesSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnTablesSelectAll.TabIndex = 20;
            this.btnTablesSelectAll.Text = "Select All";
            this.btnTablesSelectAll.UseVisualStyleBackColor = true;
            this.btnTablesSelectAll.Click += new System.EventHandler(this.btnTablesSelectAll_Click);
            // 
            // btnTablesSelectNone
            // 
            this.btnTablesSelectNone.Location = new System.Drawing.Point(687, 57);
            this.btnTablesSelectNone.Name = "btnTablesSelectNone";
            this.btnTablesSelectNone.Size = new System.Drawing.Size(75, 23);
            this.btnTablesSelectNone.TabIndex = 21;
            this.btnTablesSelectNone.Text = "Select None";
            this.btnTablesSelectNone.UseVisualStyleBackColor = true;
            this.btnTablesSelectNone.Click += new System.EventHandler(this.btnTablesSelectNone_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(687, 116);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 53);
            this.btnGenerate.TabIndex = 22;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(464, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Tables and Views";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Output Directory";
            // 
            // txtOutputDir
            // 
            this.txtOutputDir.Location = new System.Drawing.Point(101, 64);
            this.txtOutputDir.Name = "txtOutputDir";
            this.txtOutputDir.Size = new System.Drawing.Size(278, 20);
            this.txtOutputDir.TabIndex = 6;
            // 
            // chkPartial
            // 
            this.chkPartial.AutoSize = true;
            this.chkPartial.Checked = true;
            this.chkPartial.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPartial.Location = new System.Drawing.Point(100, 89);
            this.chkPartial.Name = "chkPartial";
            this.chkPartial.Size = new System.Drawing.Size(55, 17);
            this.chkPartial.TabIndex = 9;
            this.chkPartial.Text = "Partial";
            this.chkPartial.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Class Options";
            // 
            // chkPropChange
            // 
            this.chkPropChange.AutoSize = true;
            this.chkPropChange.Location = new System.Drawing.Point(161, 89);
            this.chkPropChange.Name = "chkPropChange";
            this.chkPropChange.Size = new System.Drawing.Size(98, 17);
            this.chkPropChange.TabIndex = 10;
            this.chkPropChange.Text = "PropertyEvents";
            this.chkPropChange.UseVisualStyleBackColor = true;
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Location = new System.Drawing.Point(101, 113);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(289, 20);
            this.txtNameSpace.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Namespace";
            // 
            // chkValidation
            // 
            this.chkValidation.AutoSize = true;
            this.chkValidation.Location = new System.Drawing.Point(265, 89);
            this.chkValidation.Name = "chkValidation";
            this.chkValidation.Size = new System.Drawing.Size(72, 17);
            this.chkValidation.TabIndex = 11;
            this.chkValidation.Text = "Validation";
            this.chkValidation.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatus,
            this.tsProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 369);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(774, 22);
            this.statusStrip1.TabIndex = 24;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsStatus
            // 
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(657, 17);
            this.tsStatus.Spring = true;
            // 
            // tsProgressBar
            // 
            this.tsProgressBar.Name = "tsProgressBar";
            this.tsProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(687, 175);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkLbTemplates
            // 
            this.chkLbTemplates.CheckOnClick = true;
            this.chkLbTemplates.FormattingEnabled = true;
            this.chkLbTemplates.Location = new System.Drawing.Point(16, 194);
            this.chkLbTemplates.Name = "chkLbTemplates";
            this.chkLbTemplates.Size = new System.Drawing.Size(295, 154);
            this.chkLbTemplates.Sorted = true;
            this.chkLbTemplates.TabIndex = 15;
            // 
            // btnTemplatesSelectNone
            // 
            this.btnTemplatesSelectNone.Location = new System.Drawing.Point(317, 223);
            this.btnTemplatesSelectNone.Name = "btnTemplatesSelectNone";
            this.btnTemplatesSelectNone.Size = new System.Drawing.Size(75, 23);
            this.btnTemplatesSelectNone.TabIndex = 17;
            this.btnTemplatesSelectNone.Text = "Select None";
            this.btnTemplatesSelectNone.UseVisualStyleBackColor = true;
            this.btnTemplatesSelectNone.Click += new System.EventHandler(this.btnTemplatesSelectNone_Click);
            // 
            // btnTemplatesSelectAll
            // 
            this.btnTemplatesSelectAll.Location = new System.Drawing.Point(317, 194);
            this.btnTemplatesSelectAll.Name = "btnTemplatesSelectAll";
            this.btnTemplatesSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnTemplatesSelectAll.TabIndex = 16;
            this.btnTemplatesSelectAll.Text = "Select All";
            this.btnTemplatesSelectAll.UseVisualStyleBackColor = true;
            this.btnTemplatesSelectAll.Click += new System.EventHandler(this.btnTemplatesSelectAll_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Templates";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(385, 64);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Text = "Browse ...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(24, 142);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(100, 139);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(290, 20);
            this.txtTitle.TabIndex = 26;
            this.txtTitle.Text = "БъЬт";
            // 
            // btnTemplatesReload
            // 
            this.btnTemplatesReload.Location = new System.Drawing.Point(317, 252);
            this.btnTemplatesReload.Name = "btnTemplatesReload";
            this.btnTemplatesReload.Size = new System.Drawing.Size(75, 23);
            this.btnTemplatesReload.TabIndex = 27;
            this.btnTemplatesReload.Text = "Reload";
            this.btnTemplatesReload.UseVisualStyleBackColor = true;
            this.btnTemplatesReload.Click += new System.EventHandler(this.btnTemplatesReload_Click);
            // 
            // GeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 391);
            this.Controls.Add(this.btnTemplatesReload);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnTemplatesSelectNone);
            this.Controls.Add(this.btnTemplatesSelectAll);
            this.Controls.Add(this.chkLbTemplates);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.chkValidation);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtNameSpace);
            this.Controls.Add(this.chkPropChange);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkPartial);
            this.Controls.Add(this.txtOutputDir);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnTablesSelectNone);
            this.Controls.Add(this.btnTablesSelectAll);
            this.Controls.Add(this.chkLbTables);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "GeneratorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Active Record Class Generator";
            this.Load += new System.EventHandler(this.GeneratorForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.TextBox txtServer;
		private System.Windows.Forms.TextBox txtDatabase;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckedListBox chkLbTables;
		private System.Windows.Forms.Button btnTablesSelectAll;
		private System.Windows.Forms.Button btnTablesSelectNone;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtOutputDir;
		private System.Windows.Forms.CheckBox chkPartial;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkPropChange;
		private System.Windows.Forms.TextBox txtNameSpace;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox chkValidation;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel tsStatus;
		private System.Windows.Forms.ToolStripProgressBar tsProgressBar;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.CheckedListBox chkLbTemplates;
		private System.Windows.Forms.Button btnTemplatesSelectNone;
		private System.Windows.Forms.Button btnTemplatesSelectAll;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Button btnTemplatesReload;
	}
}

