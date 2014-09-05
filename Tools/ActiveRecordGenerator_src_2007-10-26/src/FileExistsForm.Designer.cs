namespace ActiveRecordGenerator
{
	partial class FileExistsForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtDirectory = new System.Windows.Forms.TextBox();
			this.txtFile = new System.Windows.Forms.TextBox();
			this.btnOverwrite = new System.Windows.Forms.Button();
			this.btnRename = new System.Windows.Forms.Button();
			this.btnSkip = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkUseForAll = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(100, 86);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "This file exists.  What should I do?";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(40, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(26, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "File:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(14, 13);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(52, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Directory:";
			// 
			// txtDirectory
			// 
			this.txtDirectory.Location = new System.Drawing.Point(72, 12);
			this.txtDirectory.Name = "txtDirectory";
			this.txtDirectory.ReadOnly = true;
			this.txtDirectory.Size = new System.Drawing.Size(286, 20);
			this.txtDirectory.TabIndex = 1;
			this.txtDirectory.TabStop = false;
			// 
			// txtFile
			// 
			this.txtFile.Location = new System.Drawing.Point(72, 42);
			this.txtFile.Name = "txtFile";
			this.txtFile.ReadOnly = true;
			this.txtFile.Size = new System.Drawing.Size(286, 20);
			this.txtFile.TabIndex = 3;
			this.txtFile.TabStop = false;
			// 
			// btnOverwrite
			// 
			this.btnOverwrite.Location = new System.Drawing.Point(12, 119);
			this.btnOverwrite.Name = "btnOverwrite";
			this.btnOverwrite.Size = new System.Drawing.Size(75, 23);
			this.btnOverwrite.TabIndex = 5;
			this.btnOverwrite.Text = "Overwrite";
			this.btnOverwrite.UseVisualStyleBackColor = true;
			this.btnOverwrite.Click += new System.EventHandler(this.btnOverwrite_Click);
			// 
			// btnRename
			// 
			this.btnRename.Location = new System.Drawing.Point(192, 119);
			this.btnRename.Name = "btnRename";
			this.btnRename.Size = new System.Drawing.Size(75, 23);
			this.btnRename.TabIndex = 7;
			this.btnRename.Text = "Rename";
			this.btnRename.UseVisualStyleBackColor = true;
			this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
			// 
			// btnSkip
			// 
			this.btnSkip.Location = new System.Drawing.Point(102, 119);
			this.btnSkip.Name = "btnSkip";
			this.btnSkip.Size = new System.Drawing.Size(75, 23);
			this.btnSkip.TabIndex = 6;
			this.btnSkip.Text = "Skip";
			this.btnSkip.UseVisualStyleBackColor = true;
			this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(282, 119);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// chkUseForAll
			// 
			this.chkUseForAll.AutoSize = true;
			this.chkUseForAll.Location = new System.Drawing.Point(81, 159);
			this.chkUseForAll.Name = "chkUseForAll";
			this.chkUseForAll.Size = new System.Drawing.Size(208, 17);
			this.chkUseForAll.TabIndex = 9;
			this.chkUseForAll.Text = "Use this answer for all subsequent files";
			this.chkUseForAll.UseVisualStyleBackColor = true;
			// 
			// FileExistsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(370, 193);
			this.Controls.Add(this.chkUseForAll);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSkip);
			this.Controls.Add(this.btnRename);
			this.Controls.Add(this.btnOverwrite);
			this.Controls.Add(this.txtFile);
			this.Controls.Add(this.txtDirectory);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FileExistsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ActiveRecord Generator";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FileExistsForm_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtDirectory;
		private System.Windows.Forms.TextBox txtFile;
		private System.Windows.Forms.Button btnOverwrite;
		private System.Windows.Forms.Button btnRename;
		private System.Windows.Forms.Button btnSkip;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkUseForAll;
	}
}