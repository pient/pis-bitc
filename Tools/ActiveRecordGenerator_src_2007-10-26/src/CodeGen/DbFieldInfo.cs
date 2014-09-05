using System;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Text;

namespace ActiveRecordGenerator.CodeGen
{
	public class DbFieldInfo
	{
		#region Private_Variables

		private string _Column_Name;
		private string _Column_Default;
		private bool _Is_Nullable; // YES or NO
		private int _Character_Maximum_Length;
		private int _Numeric_Precision;
		private int _Numeric_Scale;
		private string _Data_Type;
		private bool _Is_Primary_Key;

		private DbForeignKeyInfo _DbForeignKeyInfo;

		// configurable option
		private bool _EnableValidationAttributes;

		#endregion

		public DbFieldInfo(
			string p_Column_Name,
			string p_Column_Default,
			bool p_Is_Nullable,
			int p_Character_Maximum_Length,
			int p_Numeric_Precision,
			int p_Numeric_Scale,
			string p_Data_Type
		)
		{
			_Column_Name = p_Column_Name;
			_Column_Default = p_Column_Default;
			_Is_Nullable = p_Is_Nullable;
			_Character_Maximum_Length = p_Character_Maximum_Length;
			_Numeric_Precision = p_Numeric_Precision;
			_Numeric_Scale = p_Numeric_Scale;
			_Data_Type = p_Data_Type;
		}

		#region Public_Properties

		public string Column_Name 
		{
			get { return _Column_Name; }
		}

		public string Column_Default 
		{
			get { return _Column_Default; }
		}

		public bool Is_Nullable 
		{
			get { return _Is_Nullable; } 
		}

		public int Character_Maximum_Length 
		{
			get { return _Character_Maximum_Length; } 
		}

		public int Numeric_Precision 
		{
			get { return _Numeric_Precision; } 
		}

		public int Numeric_Scale 
		{
			get { return _Numeric_Scale; } 
		}

		public string Data_Type
		{
			get { return _Data_Type; }
			set { _Data_Type = value.ToLowerInvariant(); }
		}

		public DbForeignKeyInfo ForeignKeyInfo
		{
			get { return _DbForeignKeyInfo; }
			set { _DbForeignKeyInfo = value; }
		}

		public bool Is_Primary_Key
		{
			get { return _Is_Primary_Key; }
			set { _Is_Primary_Key = value; }
		}

		public bool EnableValidationAttributes
		{
			get { return _EnableValidationAttributes; }
			set { _EnableValidationAttributes = value; }
		}

		#endregion

		#region Business_Method

		public bool IsPrimaryKey()
		{
			return _Is_Primary_Key;
		}

		public bool IsForeignKey()
		{
			return (_DbForeignKeyInfo != null);
		}

		public string GetPropertyName()
		{
			if (this.IsForeignKey())
			{
				if (Column_Name.EndsWith("_ID"))
				{
					return Column_Name.Substring(0, Column_Name.Length - 3);
				}
				else if (Column_Name.EndsWith("ID"))
				{
					return Column_Name.Substring(0, Column_Name.Length - 2);
				}
				else
					return Column_Name;
			}
			else
				return Column_Name;
		}

		public string GetPrivateVariableName()
		{
			string privateVariableName;

			if (IsPrimaryKey())
				privateVariableName = "_" + Column_Name.ToLowerInvariant();
			else if (this.IsForeignKey())
			{
				// subtracting 4 characters accounts for starting at index 1, and excluding suffix of "_ID"
				privateVariableName = "_" + Column_Name.Substring(0, 1).ToLowerInvariant() + Column_Name.Substring(1);
				if (privateVariableName.EndsWith("_ID"))
				{
					privateVariableName = privateVariableName.Substring(0, privateVariableName.Length - 3);
					//Debug.WriteLine("Column: "+ _Column_Name +" maps to "+ privateVariableName);
				}
				else if (privateVariableName.EndsWith("ID"))
				{
					privateVariableName = privateVariableName.Substring(0, privateVariableName.Length - 2);
					//Debug.WriteLine("Column: " + _Column_Name + " maps to " + privateVariableName);
				}
			}
			else
			{
				privateVariableName = "_" + Column_Name.Substring(0, 1).ToLowerInvariant() + Column_Name.Substring(1);
			}

			return privateVariableName;
		}

		public string GetSqlType()
		{
			if ((Data_Type.Equals("bigint"))
				|| (Data_Type.Equals("int"))
				|| (Data_Type.Equals("smallint"))
				|| (Data_Type.Equals("tinyint"))
				|| (Data_Type.Equals("bit"))
				|| (Data_Type.Equals("datetime"))
				|| (Data_Type.Equals("smalldatetime")) 
				|| (Data_Type.Equals("money"))
				|| (Data_Type.Equals("smallmoney"))
				|| (Data_Type.Equals("float"))
				|| (Data_Type.Equals("real"))
				|| (Data_Type.Equals("ntext"))
                || (Data_Type.Equals("text"))
                || (Data_Type.Equals("xml"))
                || (Data_Type.Equals("image"))
				|| (Data_Type.Equals("uniqueidentifier"))
			)
				return Data_Type;

			if ((Data_Type.Equals("char")) 
				|| (Data_Type.Equals("varchar"))
				|| (Data_Type.Equals("nchar"))
				|| (Data_Type.Equals("nvarchar"))
				|| (Data_Type.Equals("binary"))
				|| (Data_Type.Equals("varbinary"))
			)
				return String.Format("{0}({1})", Data_Type, Character_Maximum_Length);

			if ((Data_Type.Equals("decimal"))
				|| (Data_Type.Equals("numeric"))
)
				return String.Format("{0}({1},{2})", Data_Type, Numeric_Scale, Numeric_Precision);

			throw new Exception("Unexpected data type: " + Data_Type);
		}

		public string GetNetType()
		{
			if (this.IsForeignKey()) return DbTableInfo.GetSingularName(_DbForeignKeyInfo.PK_Table);

			string sqlType = Data_Type;
			// the suffix will add "?" at end if .net type is not a class and field is nullable
			string suf = (Is_Nullable) ? "?" : "";

			if (sqlType.Equals("bigint")) return "long" + suf;
			if (sqlType.Equals("int")) return "int" + suf;
			if (sqlType.Equals("smallint")) return "short" + suf;
			if (sqlType.Equals("tinyint")) return "byte" + suf;
			if (sqlType.Equals("bit")) return "bool" + suf;
			if (sqlType.Equals("decimal")) return "System.Decimal" + suf;
			if (sqlType.Equals("numeric")) return "System.Decimal" + suf;
			if (sqlType.Equals("money")) return "System.Decimal" + suf;
			if (sqlType.Equals("smallmoney")) return "System.Decimal" + suf;
			if (sqlType.Equals("float")) return "float" + suf;
			if (sqlType.Equals("real")) return "double" + suf;
            if (sqlType.Equals("datetime")) return "DateTime" + suf;
            if (sqlType.Equals("date")) return "DateTime" + suf;
			if (sqlType.Equals("smalldatetime")) return "DateTime" + suf;
			if (sqlType.Equals("char")) return "string";
			if (sqlType.Equals("varchar")) return "string";
			if (sqlType.Equals("text")) return "string"; // might be HUGE!
			if (sqlType.Equals("nchar")) return "string";
			if (sqlType.Equals("nvarchar")) return "string";
            if (sqlType.Equals("ntext")) return "string";
            if (sqlType.Equals("xml")) return "string";
			if (sqlType.Equals("binary")) return "byte[]";
			if (sqlType.Equals("varbinary")) return "byte[]";
			if (sqlType.Equals("image")) return "byte[]";
            if (sqlType.Equals("uniqueidentifier")) return "string"; // this MAY be a byte[16] array
			// if (sqlType.Equals("uniqueidentifier")) return "byte[]"; // this MAY be a byte[16] array

			throw new Exception("Unexpected data type: " + Data_Type);
		}

		// generate an Equality condition
		public string GetInEqualityTest()
		{
			if (IsForeignKey())
			{
				// test the IDs of the ActiveRecord objects
				return "(" + GetPrivateVariableName() + " == null) || (value == null) || (value." + _DbForeignKeyInfo.PK_Column + " != " + GetPrivateVariableName() + "." + _DbForeignKeyInfo.PK_Column + ")";
			}
			else if ((GetNetType().Equals("string")) || (GetNetType().Equals("datetime")))
			{
				// test object equality
				return "(" + GetPrivateVariableName() + " == null) || (value == null) || (!value.Equals(" + GetPrivateVariableName() + "))";
			}
			else
			{
				// test value equality
				return "value != " + GetPrivateVariableName();
			}
		}

		public string GetFieldAttribute()
		{
			if (IsPrimaryKey())
                return "[PrimaryKey(\"" + Column_Name + "\", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]";
			else if (IsForeignKey())
                return "[BelongsTo(\"" + Column_Name + "\", Type = typeof(" + GetPropertyName() + "), Cascade = CascadeEnum.All, Access = PropertyAccess.NosetterCamelcaseUnderscore)]";
			else
			{
				//FUTURE: if column name is reserved, add a back-tick, like "`User`"
				string prop = "[Property(\"" + Column_Name + "\", Access = PropertyAccess.NosetterCamelcaseUnderscore";
				if (!Is_Nullable) prop = prop + ", NotNull = true";

				if (
					(Data_Type.Equals("text")) ||
					(Data_Type.Equals("ntext"))
					)
				{
					prop += ", ColumnType = \"StringClob\")]";
				}
				else if (GetNetType().Equals("string") && Character_Maximum_Length > 1)
				{
					prop += ", Length = " + Character_Maximum_Length.ToString() + ")";
					if (EnableValidationAttributes)
					{
						prop += ", ValidateLength(1, " + Character_Maximum_Length.ToString() + ")]";
					}
					else
					{
						prop += "]";
					}
				}
				else
				{
					prop += ")]";
				}
				return prop;
			}
		}


		/// <summary>
		/// Makelabel - given a string, insert spaces on transition from lower case to upper case characters and replace punctuation with space
		/// </summary>
		/// <param name="pPropName">Property to be converted</param>
		/// <returns></returns>
		///TODO: make this method part of a string utility class
		internal static string MakeLabel(string pPropName)
		{
			List<char> header = new List<char>();
			header.AddRange(pPropName.ToCharArray());
			bool lastWasLower = false;
			for (int i = 0; i < header.Count; i++)
			{
				if (Char.IsPunctuation(header[i]))
				{
					//replace punctuation with space
					header[i] = ' ';
					lastWasLower = false;
				}
				else if (Char.IsLower(header[i]))
				{
					lastWasLower = true;
				}
				else if (lastWasLower && Char.IsUpper(header[i]))
				{
					// insert spaces between lower and upper characters
					header.Insert(i, ' ');
					lastWasLower = false;
				}
			}
			// replace multiple spaces with one space
			bool lastWasSpace = true;
			for (int i = header.Count - 1; i >= 0; i--)
			{
				if (Char.IsWhiteSpace(header[i]))
				{
					if (lastWasSpace)
					{
						header.RemoveAt(i);
					}
					lastWasSpace = true;
				}
				else
				{
					lastWasSpace = false;
				}
			}
			return new String(header.ToArray());
		}


		/// <summary>
		/// Gets the label.
		/// </summary>
		/// <returns>space delimited version of field containing camel-case or underscore delimited words</returns>
		public string GetLabel()
		{	
			// create a label for field
			return MakeLabel(_Column_Name);
		}

		public override string ToString()
		{
			return _Column_Name +" - "+ _Data_Type;
		}

		#endregion

	}
}
