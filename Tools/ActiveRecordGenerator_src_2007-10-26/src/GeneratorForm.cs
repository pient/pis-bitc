using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using ActiveRecordGenerator.CodeGen;

namespace ActiveRecordGenerator
{
	public partial class GeneratorForm : Form
    {
        #region properties

        public string Server
		{
			get { return txtServer.Text; }
			set { if (value != null) txtServer.Text = value; }
		}

		public string Database
		{
			get { return txtDatabase.Text; }
			set { if (value != null) txtDatabase.Text = value; }
		}

		public string OutputDir
		{
			get 
			{
				return txtOutputDir.Text;
			}
			set
			{
				if (value != null) txtOutputDir.Text = value.EndsWith("\\") ? value : value + "\\"; 
			}
		}

		public string NameSpace
		{
			get 
			{
				string ns = txtNameSpace.Text.Trim();
				if (ns.Length == 0) ns = "Application";
				return ns;  
			}
			set 
			{
				if (value != null) txtNameSpace.Text = value; 
			}
        }

        public string Title
        {
            get
            {
                string title = txtTitle.Text.Trim();
                return title;
            }
            set
            {
                if (value != null) txtTitle.Text = value;
            }
        }

		public bool Partial 
		{
			get { return chkPartial.Checked; }
			set { chkPartial.Checked = value; }
		}

		public bool PropertyEvents
		{
			get { return chkPropChange.Checked; }
			set { chkPropChange.Checked = value; }
		}

		public bool Validation
		{
			get { return chkValidation.Checked; }
			set { chkValidation.Checked = value; }
        }

        #endregion

        public GeneratorForm()
		{
			InitializeComponent();
        }

        #region private method

        private void ReloadTemplates()
        {
            chkLbTemplates.Items.Clear();

            // load templates
            string[] tFiles = Directory.GetFiles(@".\Templates\", "*.vm", SearchOption.TopDirectoryOnly);

            // strip off path information
            for (int i = 0; i < tFiles.Length; i++)
            {
                tFiles[i] = new FileInfo(tFiles[i]).Name;
            }

            chkLbTemplates.Items.AddRange(tFiles);
        }

        private void ReloadTables()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //TODO: Integrate a BackgroundWorker
                DbTableInfo[] dbTables = CodeGenFactory.GetTables(null, Server, Database);
                chkLbTables.Items.Clear();
                chkLbTables.Items.AddRange(dbTables);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Debug.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void Generate()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                ModelGenerator modelGen = ReloadModelGenerator();

                DbTableInfo table;
                string sTemplate;
                tsStatus.Text = "Generating ... ";
                int curPct = 0;
                tsProgressBar.Value = curPct;
                for (int i = 0; i < chkLbTables.Items.Count; i++)
                {
                    curPct = (int)((100.0 * i) / chkLbTables.Items.Count);
                    if (curPct >= tsProgressBar.Value + 5)
                    {
                        tsProgressBar.Value = curPct;
                        this.Refresh();
                    }
                    if (chkLbTables.GetItemChecked(i))
                    {
                        table = (DbTableInfo)chkLbTables.Items[i];

                        for (int j = 0; j < chkLbTemplates.Items.Count; j++)
                        {
                            if (chkLbTemplates.GetItemChecked(j))
                            {
                                sTemplate = (string)chkLbTemplates.Items[j];
                                modelGen.GenerateClass(sTemplate, table, OutputDir);
                                // cancel all
                                if (modelGen.GetFileHandlingResult() == (FileHandlingResult.Cancel | FileHandlingResult.All))
                                {
                                    tsProgressBar.Value = 0;
                                    goto loop_i;
                                }
                            }
                        }
                    }
                }
            loop_i:
                tsProgressBar.Value = 100;
                tsStatus.Text = "Code Generation Complete!";

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private ModelGenerator ReloadModelGenerator()
        {
            // create an anonymous delegate
            FileExists fileExists = delegate(string p_OutputDir, string p_FileName, ref FileHandlingResult fhResult)
            {
                // "File " + fileName +" exists.  What should I do?" [Overwrite], [Rename], [Cancel]
                FileExistsForm fileExistsForm = new FileExistsForm();
                fileExistsForm.Directory = p_OutputDir;
                fileExistsForm.File = p_FileName;
                fileExistsForm.ShowDialog(this);
                fhResult = fileExistsForm.Result;
                fileExistsForm.Dispose();
            };

            ModelGenerator modelGen = new ModelGenerator(fileExists, NameSpace, Title, chkPartial.Checked, chkPropChange.Checked, chkValidation.Checked);

            return modelGen;
        }

        #endregion

        #region event method

        private void GeneratorForm_Load(object sender, EventArgs e)
		{
            ReloadTemplates();
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
            ReloadTables();
		}

		private void btnTemplatesSelectAll_Click(object sender, EventArgs e)
		{
			// select all items in chkLbTemplates
			SetChecked(chkLbTemplates, true);
		}

		private void btnTemplatesSelectNone_Click(object sender, EventArgs e)
		{
			SetChecked(chkLbTemplates, false);
		}

		private void btnTablesSelectAll_Click(object sender, EventArgs e)
		{
			// select all items in chkLbTables
			SetChecked(chkLbTables, true);
		}

		private void btnTablesSelectNone_Click(object sender, EventArgs e)
		{
			//un-select all items in chkLbTables
			SetChecked(chkLbTables, false);
		}

		private void SetChecked(CheckedListBox clb, bool isChecked)
		{
			for (int i = 0; i < clb.Items.Count; i++)
			{
				clb.SetItemChecked(i, isChecked);
			}
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
            Generate();
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog dlg = new FolderBrowserDialog();

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				this.OutputDir = dlg.SelectedPath;
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

        private void btnTablesReload_Click(object sender, EventArgs e)
        {

        }

        private void btnTemplatesReload_Click(object sender, EventArgs e)
        {
            ReloadTemplates();
        }

        #endregion



    }
}