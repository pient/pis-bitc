namespace ActiveRecordGenerator
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Diagnostics;
	using System.Windows.Forms;

	static class Program
	{
		/// <summary>
		/// This application will create data entities for the Castle Project's ActiveRecord library,
		/// suitable for use with NHibernate 1.2.
		/// 
		/// The author intends to refactor the code generation such that it will be usable
		/// from the command line as well as from other GUIs.
		/// 
		/// Future design:
		/// [possible: One DbCatalogInfo will contain an array of DbTableInfo objects.]
		/// DbTableInfo will contain an array of DbFieldInfo objects as well as relations.
		/// The DbProviderFactory will be selectable among known types.
		/// A Schema Info interface will create the provider, connect to the selected
		/// database and load tables, fields and relations.
		/// Future targets are MySQL 5.x and FireBird 2.x
		/// 
		/// It was written by Roy Tate on 4/08/2007 on his own time.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			GeneratorForm gf = new GeneratorForm();

			System.Collections.Specialized.NameValueCollection settings = ConfigurationManager.AppSettings;

			gf.Server = settings["Server"];
			gf.Database = settings["Database"];
			gf.OutputDir = settings["OutputDir"];
			gf.NameSpace = settings["Namespace"];

			// due to limitations on out parameters, we have to set a local bool, then assign to a property
			bool b;

			bool.TryParse(settings["Partial"], out b);
			gf.Partial = b;

			bool.TryParse(settings["PropertyEvents"], out b);
			gf.PropertyEvents = b;

			bool.TryParse(settings["Validation"], out b);
			gf.Validation = b;

			// allow command line arguments to over-ride app config
			if (args.Length == 3)
			{
				gf.Server = args[0];
				gf.Database = args[1];
				gf.OutputDir = args[2];
			}
			Application.Run(gf);
		}
	}
}