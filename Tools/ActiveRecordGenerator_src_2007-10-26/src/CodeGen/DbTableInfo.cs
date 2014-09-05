using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ActiveRecordGenerator.CodeGen
{
	public class DbTableInfo
	{

		#region "Singular To Plural Mapping"

		//TODO: Create a hashmap of plural words and their singular equivalent
		// string pairs of SINGULAR, PLURAL in Proper case (first letter capitalized)
		// this list is based on a list that I found in Ruby On Rails.
		//Note: First Match Wins!
		private static string[] _Singular2Plural = new string[]{
			"Search", "Searches",
			"Switch", "Switches",
			"Fix", "Fixes",
			"Box", "Boxes",
			"Process", "Processes",
			"Address", "Addresses",
			"Case", "Cases",
			"Stack", "Stacks",
			"Wish", "Wishes",
			"Fish", "Fish",
			"Category", "Categories",
			"Query", "Queries",
			"Ability", "Abilities",
			"Agency", "Agencies",
			"Movie", "Movies",
			"Archive", "Archives",
			"Index", "Indices",
			"Wife", "Wives",
			"Safe", "Saves",
			"Half", "Halves",
			"Move", "Moves",
			"Salesperson", "Salespeople",
			"Person", "People",
			"Spokesman", "Spokesmen",
			"Man", "Men",
			"Woman", "Women",
			"Basis", "Bases",
			"Diagnosis", "Diagnoses",
			"Datum", "Data",
			"Medium", "Media",
			"Analysis", "Analyses",
			"Node_child", "Node_children",
			"Child", "Children",
			"Experience", "Experiences",
			"Day", "Days",
			"Comment", "Comments",
			"Newsletter", "Newsletters",
			"Old_News", "Old_News",
			"News", "News",
			"Series", "Series",
			"Species", "Species",
			"Quiz", "Quizzes",
			"Perspective", "Perspectives",
			"Ox", "Oxen",
			"Photo", "Photos",
			"Buffalo", "Buffaloes",
			"Tomato", "Tomatoes",
			"Dwarf", "Dwarves",
			"Elf", "Elves",
			"Information", "Information",
			"Equipment", "Equipment",
			"Bus", "Buses",
			"Status", "Statuses",
			"Status_code", "Status_codes",
			"Mouse", "Mice",
			"Louse", "Lice",
			"House", "Houses",
			"Octopus", "Octopi",
			"Virus", "Viri",
			"Alias", "Aliases",
			"Portfolio", "Portfolios",
			"Vertex", "Vertices",
			"Matrix", "Matrices",
			"Axis", "Axes",
			"Testis", "Testes",
			"Crisis", "Crises",
			"Rice", "Rice",
			"Shoe", "Shoes",
			"Horse", "Horses",
			"Prize", "Prizes",
			"Edge", "Edges"
		};

		// Find the LastWord in a mixed case string, based on upper case characters
		// (Not used at the moment.)
		public static string LastWord(string mixedCaseName)
		{
			string lastWord = mixedCaseName;
			char[] nameChars = mixedCaseName.ToCharArray();
			bool found = false;
			for (int i = nameChars.Length - 1; i >= 0; i--)
			{
				found = Char.IsUpper(nameChars[i]);
				if (found)
				{
					lastWord = mixedCaseName.Substring(i);
					break;
				}
			}
			return lastWord;
		}

		public static string GetSingularName(string tableName)
		{
			string rClass = tableName;
			int tableLen = tableName.Length;
			bool bFound = false;
			string singular, plural;

			//TODO: Add another class to manague Plurality.  Read Singular,Plural name pairs from an optional text file.

			for (int i = 0; i < _Singular2Plural.Length; i = i + 2)
			{
				if (tableName.EndsWith(_Singular2Plural[i + 1], StringComparison.CurrentCultureIgnoreCase))
				{
					singular = _Singular2Plural[i];
					plural = _Singular2Plural[i + 1];
					rClass = tableName.Substring(0, tableLen - plural.Length) + singular;
					bFound = true;
				}
			}

			if (!bFound)
			{
				if (tableName.EndsWith("sses")) { rClass = tableName.Substring(0, tableLen - 2); }
				else if (tableName.EndsWith("ches")) { rClass = tableName.Substring(0, tableLen - 2); }
				else if (tableName.EndsWith("us")) { /* do nothing */; }
				else if (tableName.EndsWith("s")) { rClass = tableName.Substring(0, tableLen - 1); }
			}
			return rClass;
			// return _TableName.EndsWith("s") ? _TableName.Substring(0, _TableName.Length - 1) : _TableName;
		}

		#endregion

		private string _TableName;
		private DbFieldInfo[] _DbFieldInfo;
		private DbRelatedTableInfo[] _DbRelatedTableInfo;

		public DbTableInfo(string p_TableName)
		{
			_TableName = p_TableName;
		}

		public DbFieldInfo[] GetFields()
		{
			return _DbFieldInfo;
		}

		public DbRelatedTableInfo[] GetDbRelatedTableInfo()
		{
			return _DbRelatedTableInfo;
		}

        public bool HasProperty(string prop)
        {
            foreach (DbFieldInfo fi in _DbFieldInfo)
            {
                if (fi.Column_Name == prop)
                {
                    return true;
                }
            }

            return false;
        }

		/// <summary>
		/// GetFieldList - gather an array of all fields in this table or view
		/// </summary>
		/// <param name="p_Conn">an open database connection</param>
		/// <returns></returns>
		internal void CollectFields(IDbConnection p_Conn)
		{
			string sqlQuery =
					"SELECT c.Column_Name, c.Column_Default, c.Is_Nullable, "
				+ " c.Data_Type, c.Character_Maximum_Length, "
				+ " c.Numeric_Precision, c.Numeric_Scale "
				+ " FROM Information_Schema.Columns c "
				+ " WHERE Table_Name = @Table "
				+ " ORDER BY Ordinal_Position ; ";

			List<DbFieldInfo> list = new List<DbFieldInfo>();
			DbFieldInfo dbFieldInfo = null;
			int i;

			// Connect to database, collect list of tables 
			IDbCommand cmd = null;
			IDataReader reader = null;

			string Column_Name;
			string Column_Default;
			string Is_Nullable;
			bool bIs_Nullable;
			int Character_Maximum_Length;
			int Numeric_Precision;
			int Numeric_Scale;
			string Data_Type;

			try
			{
				cmd = p_Conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = sqlQuery;
				IDbDataParameter p = cmd.CreateParameter();
				p.ParameterName = "Table";
				p.Value = _TableName;
				cmd.Parameters.Add(p);
				reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					// Debug.WriteLine("Field: " + reader.GetString(0));

					i = 0;
					Column_Name = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;
					Column_Default = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;
					Is_Nullable = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;
					Data_Type = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;
					Character_Maximum_Length = reader.IsDBNull(i) ? 0 : reader.GetInt32(i); i++;
					Numeric_Precision = reader.IsDBNull(i) ? (byte)0 : reader.GetByte(i); i++;
					Numeric_Scale = reader.IsDBNull(i) ? 0 : reader.GetInt32(i); i++;

					 bIs_Nullable = Is_Nullable.Equals("YES");
					dbFieldInfo = new DbFieldInfo(Column_Name, Column_Default,
						bIs_Nullable, Character_Maximum_Length, Numeric_Precision, 
						Numeric_Scale, Data_Type);
					list.Add(dbFieldInfo);
				}
			}
			finally
			{
				if (reader != null) reader.Close();
				if (cmd != null) cmd.Dispose();
			}

			_DbFieldInfo = list.ToArray();
			// identify primary key
			CollectPrimaryKeys(p_Conn);
			// load foreign keys and associate them to dbFieldInfo objects
			CollectForeignKeys(p_Conn);
			// load related tables that refer to this table via a foreign key
			CollectDependentTables(p_Conn);
		}

		/// <summary>
		/// CollectPrimaryKeys - find the primary key column name and set the associated field's flag
		/// </summary>
		/// <param name="p_Conn">an open database connection to the appropriate server and db</param>
		private void CollectPrimaryKeys(IDbConnection p_Conn)
		{
			string sqlQuery = @"SELECT tc.CONSTRAINT_NAME, tc.TABLE_NAME, ccu.COLUMN_NAME"
		+ " FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc"
		+ " JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu ON"
		+ "	ccu.CONSTRAINT_CATALOG = tc.CONSTRAINT_CATALOG"
		+ "	AND ccu.TABLE_NAME = tc.TABLE_NAME"
		+ "	AND ccu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME"
		+ " WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY'"
		+ " AND tc.TABLE_NAME = @Table";

			int i;

			// Connect to database, collect list of tables 
			IDbCommand cmd = null;
			IDataReader reader = null;

			string Constraint_Name;
			string PK_Table;
			string PK_Column;

			try
			{
				cmd = p_Conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = sqlQuery;
				IDbDataParameter p = cmd.CreateParameter();
				p.ParameterName = "Table";
				p.Value = _TableName;
				cmd.Parameters.Add(p);
				reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					i = 0;
					Constraint_Name = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;
					PK_Table = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;
					PK_Column = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;

					foreach (DbFieldInfo field in _DbFieldInfo)
					{
						if (field.Column_Name.Equals(PK_Column))
						{
							field.Is_Primary_Key = true;
						}
					}
				}
			}
			finally
			{
				if (reader != null) reader.Close();
				if (cmd != null) cmd.Dispose();
				reader = null;
				cmd = null;
			}
		}		

		private void CollectForeignKeys(IDbConnection p_Conn)
		{
			string sqlQuery = "SELECT FK.TABLE_NAME AS K_Table,"
		+ " CU.COLUMN_NAME AS FK_Column,"
		+ " PK.TABLE_NAME AS PK_Table,"
		+ " PT.COLUMN_NAME AS PK_Column,"
		+ " C.CONSTRAINT_NAME AS Constraint_Name"
		+ " FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C"
		+ " INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK"
		+ " ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME"
		+ " INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK "
		+ "  ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME"
		+ " INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU"
		+ "	ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME"
		+ " INNER JOIN ("
		+ "		SELECT i1.TABLE_NAME, i2.COLUMN_NAME"
		+ "		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1"
		+ "		INNER JOIN	INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2"
		+ "			ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME"
		+ "		WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY'"
		+ "		) PT ON PT.TABLE_NAME = PK.TABLE_NAME"
		+ " WHERE FK.TABLE_NAME = @Table";

			List<DbForeignKeyInfo> list = new List<DbForeignKeyInfo>();
			DbForeignKeyInfo dbForeignKeyInfo = null;
			int i;

			// Connect to database, collect list of tables 
			IDbCommand cmd = null;
			IDataReader reader = null;

			string K_Table;
			string FK_Column;
			string PK_Table;
			string PK_Column;
			string Constraint_Name;

			try
			{
				cmd = p_Conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = sqlQuery;
				IDbDataParameter p = cmd.CreateParameter();
				p.ParameterName = "Table";
				p.Value = _TableName;
				cmd.Parameters.Add(p);
				reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					i = 0;
					K_Table = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;
					FK_Column = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;
					PK_Table = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;
					PK_Column = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;
					Constraint_Name = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;

					// Yes, I am ignoring K_Table, and setting a bogus DescriptorColumn
					dbForeignKeyInfo = new DbForeignKeyInfo(FK_Column,
						PK_Table, PK_Column, Constraint_Name, "Name");
					list.Add(dbForeignKeyInfo);
				}
			}
			finally
			{
				if (reader != null) reader.Close();
				if (cmd != null) cmd.Dispose();
				reader = null;
				cmd = null;
			}

			// find a descriptor field (Name, Reason, FileName, etc)
			sqlQuery = "SELECT TOP 1 COLUMN_NAME "
			+" FROM INFORMATION_SCHEMA.COLUMNS"
			+" WHERE DATA_TYPE IN ('nvarchar', 'varchar')"
			+" AND CHARACTER_MAXIMUM_LENGTH > 3"
			+" AND TABLE_NAME = @Table"
			+" ORDER BY CASE WHEN COLUMN_NAME LIKE '%NAME%' THEN 0 ELSE 1 END, ORDINAL_POSITION";
			string PK_DescriptorColumn;

			// assign foreign key info to associated dbFieldInfo
			foreach (DbForeignKeyInfo fk in list)
			{
				// look up descriptor column
				try
				{
					cmd = p_Conn.CreateCommand();
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = sqlQuery;
					IDbDataParameter p = cmd.CreateParameter();
					p.ParameterName = "Table";
					p.Value = fk.PK_Table;
					cmd.Parameters.Add(p);
					reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						i = 0;
						PK_DescriptorColumn = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;

						fk.PK_DescriptorColumn = PK_DescriptorColumn;
					}
				}
				finally
				{
					if (reader != null) reader.Close();
					if (cmd != null) cmd.Dispose();
					reader = null;
					cmd = null;
				}

				foreach (DbFieldInfo fi in _DbFieldInfo)
				{ 
					if (fi.Column_Name.Equals(fk.FK_Column))
					{
						fi.ForeignKeyInfo = fk;
						break;
					}
				}
			}
		}

		/// <summary>
		/// CollectDependentTables - find tables that link to us as a foreign key
		/// </summary>
		/// SELECT k.table_name, k.column_name field_name, c.constraint_type,
		///             CASE c.is_deferrable WHEN 'NO' THEN 0 ELSE 1 END 'is_deferrable',
		///             CASE c.initially_deferred WHEN 'NO' THEN 0 ELSE 1 END 'is_deferred',
		///             rc.match_option 'match_type', rc.update_rule 'on_update', rc.delete_rule 'on_delete',
		///             ccu.table_name 'references_table', ccu.column_name 'references_field', k.ordinal_position 'field_position'
		/// FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE k
		/// LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS c
		///             ON k.table_name = c.table_name AND k.table_schema = c.table_schema
		///             AND k.table_catalog = c.table_catalog AND k.constraint_catalog = c.constraint_catalog
		///             AND k.constraint_name = c.constraint_name
		/// LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
		///             ON rc.constraint_schema = c.constraint_schema AND rc.constraint_catalog = c.constraint_catalog
		///             AND rc.constraint_name = c.constraint_name
		/// LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
		///             ON rc.unique_constraint_schema = ccu.constraint_schema
		///             AND rc.unique_constraint_catalog = ccu.constraint_catalog
		///             AND rc.unique_constraint_name = ccu.constraint_name
		/// WHERE k.constraint_catalog = DB_NAME()
		/// AND c.constraint_type = 'FOREIGN KEY'
		/// AND ccu.table_name = @Table
		/// ORDER BY k.constraint_name, k.ordinal_position
		/// 
		/// Note: This will return one-to-many and many-to-many tables where this table is
		/// listed as a foreign key reference.
		/// <param name="p_Conn">an open database connection to the appropriate server and db</param>
		private void CollectDependentTables(IDbConnection p_Conn)
		{
			string sqlQuery = @"
SELECT k.table_name, k.column_name --, k.ordinal_position
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE k
LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS c
            ON k.table_name = c.table_name AND k.table_schema = c.table_schema
            AND k.table_catalog = c.table_catalog AND k.constraint_catalog = c.constraint_catalog
            AND k.constraint_name = c.constraint_name
LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
            ON rc.constraint_schema = c.constraint_schema AND rc.constraint_catalog = c.constraint_catalog
            AND rc.constraint_name = c.constraint_name
LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
            ON rc.unique_constraint_schema = ccu.constraint_schema
            AND rc.unique_constraint_catalog = ccu.constraint_catalog
            AND rc.unique_constraint_name = ccu.constraint_name
WHERE k.constraint_catalog = DB_NAME()
AND c.constraint_type = 'FOREIGN KEY'
AND ccu.table_name = @Table
ORDER BY k.constraint_name, k.ordinal_position";

			List<DbRelatedTableInfo> list = new List<DbRelatedTableInfo>();
			DbRelatedTableInfo field;
			int i;

			// Connect to database, collect list of tables 
			IDbCommand cmd = null;
			IDataReader reader = null;

			string Rel_Table;
			string Rel_Column;
			//int Ordinal_Position;

			try
			{
				cmd = p_Conn.CreateCommand();
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = sqlQuery;
				IDbDataParameter p = cmd.CreateParameter();
				p.ParameterName = "Table";
				p.Value = _TableName;
				cmd.Parameters.Add(p);
				reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					i = 0;
					Rel_Table = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;
					Rel_Column = reader.IsDBNull(i) ? "" : reader.GetString(i); i++;
					//Ordinal_Position = reader.IsDBNull(i) ? "" : reader.GetInt32(i); i++;

					// store referred tables
					field = new DbRelatedTableInfo(Rel_Table, Rel_Column);
					list.Add(field);
				}
				_DbRelatedTableInfo = list.ToArray();
			}
			finally
			{
				if (reader != null) reader.Close();
				if (cmd != null) cmd.Dispose();
				reader = null;
				cmd = null;
			}
		}

		/// <summary>
		/// Get Class Name from Table Name
		/// </summary>
		/// Find the singular form of a name, given a multi-word name ending in its plural form
		/// <param name="p_TableName">table name</param>
		/// <returns>singular form of table name</returns>
		public string GetClassName()
		{
			return GetSingularName(_TableName);
		}

		public string GetLabel()
		{
			//TODO: Test This!
			return DbFieldInfo.MakeLabel(_TableName);
		}

		public DbFieldInfo GetPkField()
		{
			DbFieldInfo field = null;

			foreach (DbFieldInfo f in _DbFieldInfo)
			{
				if (f.Is_Primary_Key)
				{
					field = f;
					break;
				}
			}

			return field;
		}
	
		public override string ToString()
		{
			return _TableName;
		}
	}
}
