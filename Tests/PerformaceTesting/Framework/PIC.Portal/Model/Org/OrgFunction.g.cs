// Business class OrgFunction generated from OrgFunction
// Creator: Ray
// Created Date: [2013-06-15]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
using PIC.Portal;
	
namespace PIC.Portal.Model
{
	[ActiveRecord("OrgFunction")]
	public partial class OrgFunction : ModelBase<OrgFunction>
	{
		#region Property_Names

		public static string Prop_FunctionID = "FunctionID";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Type = "Type";
		public static string Prop_SortIndex = "SortIndex";
		public static string Prop_Status = "Status";
		public static string Prop_EditStatus = "EditStatus";
		public static string Prop_Description = "Description";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreateDate = "CreateDate";

		#endregion

		#region Private_Variables

		private string _functionid;
		private string _code;
		private string _name;
		private int? _type;
		private int? _sortIndex;
		private byte? _status;
		private string _editStatus;
		private string _description;
		private DateTime? _lastModifiedDate;
		private DateTime? _createDate;

		#endregion

		#region Constructors

		public OrgFunction()
		{
		}

		public OrgFunction(
			string p_functionid,
			string p_code,
			string p_name,
			int? p_type,
			int? p_sortIndex,
            byte? p_status,
			string p_editStatus,
			string p_description,
			DateTime? p_lastModifiedDate,
			DateTime? p_createDate)
		{
			_functionid = p_functionid;
			_code = p_code;
			_name = p_name;
			_type = p_type;
			_sortIndex = p_sortIndex;
			_status = p_status;
			_editStatus = p_editStatus;
			_description = p_description;
			_lastModifiedDate = p_lastModifiedDate;
			_createDate = p_createDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("FunctionID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string FunctionID
		{
			get { return _functionid; }
			set { _functionid = value; }

		}

		[Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Code
		{
			get { return _code; }
			set
			{
				if ((_code == null) || (value == null) || (!value.Equals(_code)))
				{
                    object oldValue = _code;
					_code = value;
					RaisePropertyChanged(OrgFunction.Prop_Code, oldValue, value);
				}
			}

		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(OrgFunction.Prop_Name, oldValue, value);
				}
			}

		}

        [Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? Type
		{
			get { return _type; }
			set
			{
				if ((_type == null) || (value == null) || (!value.Equals(_type)))
				{
                    object oldValue = _type;
					_type = value;
					RaisePropertyChanged(OrgFunction.Prop_Type, oldValue, value);
				}
			}

		}

		[Property("SortIndex", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? SortIndex
		{
			get { return _sortIndex; }
			set
			{
				if (value != _sortIndex)
				{
                    object oldValue = _sortIndex;
					_sortIndex = value;
					RaisePropertyChanged(OrgFunction.Prop_SortIndex, oldValue, value);
				}
			}

		}

		[Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public byte? Status
		{
			get { return _status; }
			set
			{
				if (value != _status)
				{
                    object oldValue = _status;
					_status = value;
					RaisePropertyChanged(OrgFunction.Prop_Status, oldValue, value);
				}
			}

		}

		[Property("EditStatus", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string EditStatus
		{
			get { return _editStatus; }
			set
			{
				if ((_editStatus == null) || (value == null) || (!value.Equals(_editStatus)))
				{
                    object oldValue = _editStatus;
					_editStatus = value;
					RaisePropertyChanged(OrgFunction.Prop_EditStatus, oldValue, value);
				}
			}

		}

		[Property("Description", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 250)]
		public string Description
		{
			get { return _description; }
			set
			{
				if ((_description == null) || (value == null) || (!value.Equals(_description)))
				{
                    object oldValue = _description;
					_description = value;
					RaisePropertyChanged(OrgFunction.Prop_Description, oldValue, value);
				}
			}

		}

		[Property("LastModifiedDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? LastModifiedDate
		{
			get { return _lastModifiedDate; }
			set
			{
				if (value != _lastModifiedDate)
				{
                    object oldValue = _lastModifiedDate;
					_lastModifiedDate = value;
					RaisePropertyChanged(OrgFunction.Prop_LastModifiedDate, oldValue, value);
				}
			}

		}

		[Property("CreateDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? CreateDate
		{
			get { return _createDate; }
			set
			{
				if (value != _createDate)
				{
                    object oldValue = _createDate;
					_createDate = value;
					RaisePropertyChanged(OrgFunction.Prop_CreateDate, oldValue, value);
				}
			}

		}
		
		#endregion
    } // OrgFunction
}

