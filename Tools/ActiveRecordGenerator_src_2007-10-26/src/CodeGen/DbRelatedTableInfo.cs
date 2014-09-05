using System;
using System.Collections.Generic;
using System.Text;

namespace ActiveRecordGenerator.CodeGen
{
	public class DbRelatedTableInfo
	{
		private string _Column_Name;
		private string _Table_Name;

		public DbRelatedTableInfo(string p_Table_Name, string p_Column_Name)
		{
			_Table_Name = p_Table_Name;
			_Column_Name = p_Column_Name;
		}

		public string Column_Name
		{
			get { return _Column_Name; }
			//set { _Column_Name = value; }
		}

		public string Table_Name
		{
			get { return _Table_Name; }
			//set { _Table_Name = value; }
		}

		public string GetClassName()
		{
			return DbTableInfo.GetSingularName(_Table_Name);
		}
	}
}
