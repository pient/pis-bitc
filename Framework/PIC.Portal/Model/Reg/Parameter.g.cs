// Business class SysParameter generated from SysParameter
// Creator: Ray
// Created Date: [2010-06-25]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
	
namespace PIC.Portal.Model
{
    [ActiveRecord("Parameter")]
	public partial class Parameter : EntityBase<Parameter> , INotifyPropertyChanged 	
	{

		#region Property_Names

		public static string Prop_ParameterID = "ParameterID";
		public static string Prop_Code = "Code";
        public static string Prop_Name = "Name";
        public static string Prop_Status = "Status";
		public static string Prop_CatalogID = "CatalogID";
		public static string Prop_Value = "Value";
		public static string Prop_Type = "Type";
		public static string Prop_RegexStr = "RegexStr";
		public static string Prop_EditStatus = "EditStatus";
		public static string Prop_SortIndex = "SortIndex";
		public static string Prop_Description = "Description";
		public static string Prop_CreaterID = "CreaterID";
		public static string Prop_CreaterName = "CreaterName";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreatedDate = "CreatedDate";

		#endregion

		#region Private_Variables

		private string _parameterid;
		private string _code;
        private string _name;
        private string _status;
		private string _catalogID;
		private string _value;
		private string _type;
		private string _regexStr;
		private string _editStatus;
		private int? _sortIndex;
		private string _description;
		private string _createrID;
		private string _createrName;
		private DateTime? _lastModifiedDate;
		private DateTime? _createdDate;


		#endregion

		#region Constructors

		public Parameter()
		{
		}

		public Parameter(
			string p_parameterid,
			string p_code,
			string p_name,
			string p_catalogID,
			string p_value,
			string p_type,
			string p_regexStr,
			string p_editStatus,
			int? p_sortIndex,
			string p_description,
			string p_createrID,
			string p_createrName,
			DateTime? p_lastModifiedDate,
			DateTime? p_createdDate)
		{
			_parameterid = p_parameterid;
			_code = p_code;
			_name = p_name;
			_catalogID = p_catalogID;
			_value = p_value;
			_type = p_type;
			_regexStr = p_regexStr;
			_editStatus = p_editStatus;
			_sortIndex = p_sortIndex;
			_description = p_description;
			_createrID = p_createrID;
			_createrName = p_createrName;
			_lastModifiedDate = p_lastModifiedDate;
			_createdDate = p_createdDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("ParameterID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string ParameterID
		{
			get { return _parameterid; }
		}

		[Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Code
		{
			get { return _code; }
			set
			{
				if ((_code == null) || (value == null) || (!value.Equals(_code)))
				{
					_code = value;
					NotifyPropertyChanged(Parameter.Prop_Code);
				}
			}
		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
					_name = value;
					NotifyPropertyChanged(Parameter.Prop_Name);
				}
			}
		}

        [Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string Status
        {
            get { return _status; }
            set
            {
                if ((_status == null) || (value == null) || (!value.Equals(_status)))
                {
                    _status = value;
                    NotifyPropertyChanged(Parameter.Prop_Status);
                }
            }
        }

		[Property("CatalogID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CatalogID
		{
			get { return _catalogID; }
			set
			{
				if ((_catalogID == null) || (value == null) || (!value.Equals(_catalogID)))
				{
					_catalogID = value;
					NotifyPropertyChanged(Parameter.Prop_CatalogID);
				}
			}
		}

		[Property("Value", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 5000)]
		public string Value
		{
			get { return _value; }
			set
			{
				if ((_value == null) || (value == null) || (!value.Equals(_value)))
				{
					_value = value;
					NotifyPropertyChanged(Parameter.Prop_Value);
				}
			}
		}

		[Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Type
		{
			get { return _type; }
			set
			{
				if ((_type == null) || (value == null) || (!value.Equals(_type)))
				{
					_type = value;
					NotifyPropertyChanged(Parameter.Prop_Type);
				}
			}
		}

		[Property("RegexStr", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string RegexStr
		{
			get { return _regexStr; }
			set
			{
				if ((_regexStr == null) || (value == null) || (!value.Equals(_regexStr)))
				{
					_regexStr = value;
					NotifyPropertyChanged(Parameter.Prop_RegexStr);
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
					_editStatus = value;
					NotifyPropertyChanged(Parameter.Prop_EditStatus);
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
					_sortIndex = value;
					NotifyPropertyChanged(Parameter.Prop_SortIndex);
				}
			}
		}

		[Property("Description", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Description
		{
			get { return _description; }
			set
			{
				if ((_description == null) || (value == null) || (!value.Equals(_description)))
				{
					_description = value;
					NotifyPropertyChanged(Parameter.Prop_Description);
				}
			}
		}

		[Property("CreaterID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CreaterID
		{
			get { return _createrID; }
			set
			{
				if ((_createrID == null) || (value == null) || (!value.Equals(_createrID)))
				{
					_createrID = value;
					NotifyPropertyChanged(Parameter.Prop_CreaterID);
				}
			}
		}

		[Property("CreaterName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CreaterName
		{
			get { return _createrName; }
			set
			{
				if ((_createrName == null) || (value == null) || (!value.Equals(_createrName)))
				{
					_createrName = value;
					NotifyPropertyChanged(Parameter.Prop_CreaterName);
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
					_lastModifiedDate = value;
					NotifyPropertyChanged(Parameter.Prop_LastModifiedDate);
				}
			}
		}

		[Property("CreatedDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? CreatedDate
		{
			get { return _createdDate; }
			set
			{
				if (value != _createdDate)
				{
					_createdDate = value;
					NotifyPropertyChanged(Parameter.Prop_CreatedDate);
				}
			}
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			PropertyChangedEventHandler localPropertyChanged = PropertyChanged;
			if (localPropertyChanged != null)
			{
				localPropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}

		#endregion

	} // SysParameter
}

