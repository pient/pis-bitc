// Business class SysTableStructure generated from SysTableStructure
// Creator: Ray
// Created Date: [2010-06-23]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
	
namespace PIC.Portal.Model
{
	[ActiveRecord("SysTableStructure")]
	public partial class SysTableStructure : EntityBase<SysTableStructure> 	
	{

		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_ColOrder = "ColOrder";
		public static string Prop_TableName = "TableName";
		public static string Prop_TableDesc = "TableDesc";
		public static string Prop_FieldName = "FieldName";
		public static string Prop_IsPrimary = "IsPrimary";
		public static string Prop_IsNullable = "IsNullable";
		public static string Prop_Type = "Type";
		public static string Prop_Byte = "Byte";
		public static string Prop_Length = "Length";
		public static string Prop_DefaultValue = "DefaultValue";
		public static string Prop_FieldDesc = "FieldDesc";

		#endregion

		#region Private_Variables

		private string _id;
		private short? _colOrder;
		private string _tableName;
		private string _tableDesc;
		private string _fieldName;
		private int _isPrimary;
		private int? _isNullable;
		private string _type;
		private short? _byte;
		private int? _length;
        private string _defaultValue;
		private string _fieldDesc;


		#endregion

		#region Constructors

		public SysTableStructure()
		{
		}

		public SysTableStructure(
			string p_id,
			short? p_colOrder,
			string p_tableName,
			string p_tableDesc,
			string p_fieldName,
			int p_isPrimary,
			int? p_isNullable,
			string p_type,
			short? p_byte,
			int? p_length,
			string p_defaultValue,
			string p_fieldDesc)
		{
			_id = p_id;
			_colOrder = p_colOrder;
			_tableName = p_tableName;
			_tableDesc = p_tableDesc;
			_fieldName = p_fieldName;
			_isPrimary = p_isPrimary;
			_isNullable = p_isNullable;
			_type = p_type;
			_byte = p_byte;
			_length = p_length;
            _defaultValue = p_defaultValue;
			_fieldDesc = p_fieldDesc;
		}

		#endregion

		#region Properties

        [PrimaryKey("Id", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public string Id
		{
			get { return _id; }
		}

		[Property("ColOrder", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public short? ColOrder
		{
			get { return _colOrder; }
		}

		[Property("TableName", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 128)]
		new public string TableName
		{
			get { return _tableName; }
		}

		[Property("TableDesc", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 4000)]
		public string TableDesc
		{
			get { return _tableDesc; }
		}

		[Property("FieldName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 128)]
		public string FieldName
		{
			get { return _fieldName; }
		}

		[Property("IsPrimary", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
		public int IsPrimary
		{
			get { return _isPrimary; }
		}

		[Property("IsNullable", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? IsNullable
		{
			get { return _isNullable; }
		}

		[Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 128)]
		public string Type
		{
			get { return _type; }
		}

		[Property("Byte", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public short? Byte
		{
			get { return _byte; }
		}

		[Property("Length", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? Length
		{
			get { return _length; }
		}

		[Property("DefaultValue", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 4000)]
        public string DefaultValue
		{
			get { return _defaultValue; }
		}

		[Property("FieldDesc", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 4000)]
		public string FieldDesc
		{
			get { return _fieldDesc; }
		}

		#endregion

	} // SysTableStructure
}

