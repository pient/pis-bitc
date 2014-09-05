using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace ActiveRecordGenerator.CodeGen
{
	public class CodeGenFactory
	{
		// get a list of tables
		public static DbTableInfo[] GetTables(BackgroundWorker bw, string server, string database)
		{
			System.Data.Common.DbProviderFactory sqlFactory = System.Data.SqlClient.SqlClientFactory.Instance;

			IDbConnection conn = null;

			// Connect to database, collect list of tables 
			conn = sqlFactory.CreateConnection();
			IDbCommand cmd = null;
			IDataReader reader = null;
			string table;
			List<DbTableInfo> dbList = new List<DbTableInfo>();
			DbTableInfo dbTableInfo;

			if (bw != null) bw.ReportProgress(0, "Connecting ...");
			conn.ConnectionString = "Data Source=" + server
				+ ";Initial Catalog=" + database
				+ ";User ID=sa;PWD=sasa";//Integrated Security=SSPI
            conn.Open();
			try
			{
				cmd = conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
//#if DEBUG
//				cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'AG%';";
//#else
				cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES;";
//#endif

				reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					table = reader.GetString(0);
					if (table.Equals("dtproperties")) continue;
					if (table.StartsWith("sys")) continue;
					dbTableInfo = new DbTableInfo(table);
					dbTableInfo.GetFields();
					dbList.Add(dbTableInfo);
				}
				reader.Close();
				if (bw != null) bw.ReportProgress(50, "Tables collected");

				foreach(DbTableInfo tableInfo in dbList)
				{
					tableInfo.CollectFields(conn);
				}
				if (bw != null) bw.ReportProgress(100, "Fields collected");
			}
			finally
			{
				if (reader != null) reader.Close();
				if (cmd != null) cmd.Dispose();
				conn.Close();
			}
			
			return dbList.ToArray();
		}
	}
}
