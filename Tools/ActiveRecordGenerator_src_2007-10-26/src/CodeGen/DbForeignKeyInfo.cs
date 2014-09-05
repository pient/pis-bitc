using System;
using System.Collections.Generic;
using System.Text;

namespace ActiveRecordGenerator.CodeGen
{
	public class DbForeignKeyInfo
	{
		//public string K_Table;
		private string _FK_Column;
		private string _PK_Table;
		private string _PK_Column;
		private string _Constraint_Name;
		// The descriptor column is intended to be the Primary String Identifier for a table,
		// and suitable for Journalling or Auditing.
		private string _PK_DescriptorColumn; // the first varchar or nvarchar column in PK table

		public DbForeignKeyInfo(//string p_K_Table,
			string p_FK_Column,
			string p_PK_Table,
			string p_PK_Column,
			string p_Constraint_Name,
			string p_PK_DescriptorColumn)
		{
			_FK_Column = p_FK_Column;
			_PK_Table = p_PK_Table;
			_PK_Column = p_PK_Column;
			_Constraint_Name = p_Constraint_Name;
			_PK_DescriptorColumn = p_PK_DescriptorColumn;
		}

		public string FK_Column { get { return _FK_Column; } }
		public string PK_Table { get { return _PK_Table; } }
		public string PK_Column { get { return _PK_Column; } }
		public string Constraint_Name { get { return _Constraint_Name; } }

		public string PK_DescriptorColumn 
		{
			get { return _PK_DescriptorColumn; }
			set { _PK_DescriptorColumn = value; }
		}

	}
}
