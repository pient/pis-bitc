namespace ActiveRecordGenerator.CodeGen
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Text;

	using Commons.Collections;
	using NVelocity;
	using NVelocity.App;

	public delegate void FileExists(string p_OutputDir, string p_FileName, ref FileHandlingResult fhResult);

	public class ModelGenerator
	{
        string _NameSpace;
        string _Title;
		bool _MakePartial;
		bool _PropChange;
		bool _EnableValidationAttributes;
		private FileHandlingResult _fhResult;
		private FileExists _FileExists;

		VelocityEngine engine = null;

        public ModelGenerator(FileExists p_FileExists, string p_NameSpace, string p_Title,
			bool p_MakePartial, bool p_PropChange, bool p_Validate)
		{
			_FileExists = p_FileExists;			
			_NameSpace = p_NameSpace;
            _Title = p_Title;
			_MakePartial = p_MakePartial;
			_PropChange = p_PropChange;
			_EnableValidationAttributes = p_Validate;
			_fhResult = FileHandlingResult.None;

			// Initialize NVelocity
			engine = new VelocityEngine();
			ExtendedProperties props = new ExtendedProperties();
			props.AddProperty("file.resource.loader.path", new ArrayList(new string[] { ".", @".\Templates" }));
			engine.Init(props);
		}

		public FileHandlingResult GetFileHandlingResult()
		{
			return _fhResult;
		}

		/// <summary>
		/// Generate an ActiveRecord model usable by the Castle Project's 
		/// ActiveRecord library
		/// </summary>
		/// <param name="p_Table">database table or view name</param>
		/// <param name="p_OutputDir">the target directory</param>
		public void GenerateClass(string sTemplate, DbTableInfo p_Table, string p_OutputDir)
		{
            Template template = engine.GetTemplate(sTemplate, "GB2312");
			// Generate ActiveRecord Classes

			string className = p_Table.GetClassName();
			DbFieldInfo[] fieldList = p_Table.GetFields();
			DbRelatedTableInfo[] related = p_Table.GetDbRelatedTableInfo();
			// replicate flag to children
			//TODO: add to [new] code generation context
			for (int i = 0; i < fieldList.Length; i++)
			{
				fieldList[i].EnableValidationAttributes = _EnableValidationAttributes;
			}
			Debug.WriteLine("Table: " + p_Table + " -> " + className);
			string fileName;

			VelocityContext context = new VelocityContext();

            context.Put("namespace", _NameSpace);
            context.Put("title", _Title);
			context.Put("developer", Environment.UserName);
			context.Put("Partial", _MakePartial ? "partial" : "");
			context.Put("PropChange", _PropChange);
			context.Put("table", p_Table);
			context.Put("fields", fieldList);
			context.Put("related", related);
			context.Put("date", DateTime.Now.ToString("yyyy-MM-dd"));

			//get suggested class name
			fileName = GetFileName(sTemplate, context);

			if (!CanWriteThisFile(p_OutputDir, className, fileName)) return;
			StreamWriter wr = new StreamWriter(p_OutputDir + fileName, false, Encoding.UTF8);
			try
			{
				StringWriter writer = new StringWriter();
				template.Merge(context, writer);
				wr.WriteLine(writer.GetStringBuilder().ToString());
			}
			finally
			{
				wr.Close();
			}
		}

		private string GetFileName(string sTemplate, VelocityContext context)
		{
			StreamReader fReader = new StreamReader(@".\Templates\" + sTemplate);
			string sFileNameTemplate = fReader.ReadLine();
			if (sFileNameTemplate.StartsWith("##FILENAME:"))
			{
				sFileNameTemplate = sFileNameTemplate.Substring(11);
			}
			else
			{
				// default to base filename
				sFileNameTemplate = "${table.GetClassName}.cs";
			}
			StringReader reader = new StringReader(sFileNameTemplate);
			StringWriter writer = new StringWriter();
			engine.Evaluate(context, writer, null, reader);
			string fileName = writer.GetStringBuilder().ToString();
			return fileName;
		}

		/// <summary>
		/// Ask the user how to handle file collisions
		/// </summary>
		/// <param name="p_OutputDir"></param>
		/// <param name="p_FileName"></param>
		/// <returns></returns>
		private bool CanWriteThisFile(string p_OutputDir, string p_ClassName, string p_FileName)
		{
			if (File.Exists(p_OutputDir + p_FileName))
			{
				// only ask if they did not check "same for All files"
				if ((_fhResult & FileHandlingResult.All) == 0)
				{
					_FileExists.Invoke(p_OutputDir, p_FileName, ref _fhResult);
				}
				// if they said either cancel or skip, tell the caller NOT to write the file
				if ((_fhResult & FileHandlingResult.Cancel) > 0)
				{
					return false;
				}
				else if ((_fhResult & FileHandlingResult.Skip) > 0)
				{
					OneTimeFileRequestHandled();
					return false;
				}
				else if ((_fhResult & FileHandlingResult.Rename) > 0)
				{
					// rename the file
					// -- find a free extension
					int i = 0;
					string ext;
					do
					{
						if (++i > 999) throw new Exception("Unable to rename file!");
						ext = String.Format(".{0:00#}", i);
					} while (File.Exists(p_OutputDir + p_ClassName + ext));
					// -- finally rename the file
					File.Move(p_OutputDir + p_FileName, p_OutputDir + p_ClassName + ext);
					// we handled a one-time request ...
					OneTimeFileRequestHandled();
				}
				else if ((_fhResult & FileHandlingResult.OverWrite) > 0)
				{
					OneTimeFileRequestHandled();
				}
			}

			// file does not exist, or otherwise safe to write
			return true;
		}

		private void OneTimeFileRequestHandled()
		{
			// if this is a one-time request (not ALL)
			if ((_fhResult & FileHandlingResult.All) == 0)
			{
				_fhResult = FileHandlingResult.None;
			}
		}

	}
}
