using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ActiveRecordGenerator
{
	public partial class FileExistsForm : Form
	{
		public FileExistsForm()
		{
			InitializeComponent();
		}

		#region Properties

		public string Directory
		{
			get { return txtDirectory.Text; }
			set { txtDirectory.Text = value; }
		}

		public string File
		{
			get { return txtFile.Text; }
			set { txtFile.Text = value; }
		}

		private FileHandlingResult _fileHandlingResult = FileHandlingResult.None;

		public FileHandlingResult Result
		{
			get { return _fileHandlingResult; }
			set { _fileHandlingResult = value; }
		}

		#endregion

		private void btnOverwrite_Click(object sender, EventArgs e)
		{
			_fileHandlingResult = FileHandlingResult.OverWrite;
			if (chkUseForAll.Checked) _fileHandlingResult |= FileHandlingResult.All;
			this.Close();
		}

		private void btnSkip_Click(object sender, EventArgs e)
		{
			_fileHandlingResult = FileHandlingResult.Skip;
			if (chkUseForAll.Checked) _fileHandlingResult |= FileHandlingResult.All;
			this.Close();
		}

		private void btnRename_Click(object sender, EventArgs e)
		{
			_fileHandlingResult = FileHandlingResult.Rename;
			if (chkUseForAll.Checked) _fileHandlingResult |= FileHandlingResult.All;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			_fileHandlingResult = FileHandlingResult.Cancel | FileHandlingResult.All;
			this.Close();
		}

		private void FileExistsForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			// if they haven't clicked one of the buttons, consider this to be a cancel all
			if (_fileHandlingResult == FileHandlingResult.None)
			{
				_fileHandlingResult = FileHandlingResult.Cancel | FileHandlingResult.All;
			}
		}

	}
}