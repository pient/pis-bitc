// Business class SysWfDefine generated from SysWfDefine
// Creator: Ray
// Created Date: [2012-04-27]

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
    [ActiveRecord("WfDefine")]
	public partial class WfDefine : ModelBase<WfDefine>
	{
		#region Property_Names

		public static string Prop_DefineID = "DefineID";
		public static string Prop_ModuleID = "ModuleID";
		public static string Prop_ApplicationID = "ApplicationID";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Type = "Type";
        public static string Prop_Catalog = "Catalog";
        public static string Prop_Status = "Status";
        public static string Prop_SortIndex = "SortIndex";
		public static string Prop_Config = "Config";
        public static string Prop_FormDefine = "FormDefine";
		public static string Prop_Memo = "Memo";
		public static string Prop_Tag = "Tag";
		public static string Prop_Version = "Version";
		public static string Prop_CreatorID = "CreatorID";
		public static string Prop_CreatorName = "CreatorName";
		public static string Prop_CreatedDate = "CreatedDate";
		public static string Prop_LastModifiedDate = "LastModifiedDate";

		#endregion

		#region Private_Variables

		private string _defineid;
		private string _moduleID;
		private string _applicationID;
		private string _code;
		private string _name;
		private string _type;
        private string _catalog;
        private string _status;
        private int? _sortIndex;
		private string _config;
		private string _formDefine;
		private string _memo;
		private string _tag;
		private string _version;
		private string _creatorID;
		private string _creatorName;
		private DateTime? _createdDate;
		private DateTime? _lastModifiedDate;


		#endregion

		#region Constructors

		public WfDefine()
		{
		}

		public WfDefine(
			string p_defineid,
			string p_moduleID,
			string p_applicationID,
			string p_code,
			string p_name,
			string p_type,
			string p_catalog,
			string p_status,
			string p_config,
			string p_formDefine,
			string p_memo,
			string p_tag,
			string p_version,
			string p_creatorID,
			string p_creatorName,
			DateTime? p_createdDate,
			DateTime? p_lastModifiedDate)
		{
			_defineid = p_defineid;
			_moduleID = p_moduleID;
			_applicationID = p_applicationID;
			_code = p_code;
			_name = p_name;
			_type = p_type;
			_catalog = p_catalog;
			_status = p_status;
			_config = p_config;
			_formDefine = p_formDefine;
			_memo = p_memo;
			_tag = p_tag;
			_version = p_version;
			_creatorID = p_creatorID;
			_creatorName = p_creatorName;
			_createdDate = p_createdDate;
			_lastModifiedDate = p_lastModifiedDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("DefineID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string DefineID
		{
			get { return _defineid; }
			set { _defineid = value; }

		}

		[Property("ModuleID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ModuleID
		{
			get { return _moduleID; }
			set
			{
				if ((_moduleID == null) || (value == null) || (!value.Equals(_moduleID)))
				{
                    object oldValue = _moduleID;
					_moduleID = value;
					RaisePropertyChanged(WfDefine.Prop_ModuleID, oldValue, value);
				}
			}

		}

		[Property("ApplicationID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ApplicationID
		{
			get { return _applicationID; }
			set
			{
				if ((_applicationID == null) || (value == null) || (!value.Equals(_applicationID)))
				{
                    object oldValue = _applicationID;
					_applicationID = value;
					RaisePropertyChanged(WfDefine.Prop_ApplicationID, oldValue, value);
				}
			}

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
					RaisePropertyChanged(WfDefine.Prop_Code, oldValue, value);
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
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(WfDefine.Prop_Name, oldValue, value);
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
                    object oldValue = _type;
					_type = value;
					RaisePropertyChanged(WfDefine.Prop_Type, oldValue, value);
				}
			}

		}

		[Property("Catalog", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Catalog
		{
			get { return _catalog; }
			set
			{
				if ((_catalog == null) || (value == null) || (!value.Equals(_catalog)))
				{
                    object oldValue = _catalog;
					_catalog = value;
					RaisePropertyChanged(WfDefine.Prop_Catalog, oldValue, value);
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
                    object oldValue = _status;
					_status = value;
					RaisePropertyChanged(WfDefine.Prop_Status, oldValue, value);
				}
			}

		}

        [Property("SortIndex", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public int? SortIndex
        {
            get { return _sortIndex; }
            set
            {
                if ((_sortIndex == null) || (value == null) || (!value.Equals(_sortIndex)))
                {
                    object oldValue = _sortIndex;
                    _sortIndex = value;
                    RaisePropertyChanged(WfDefine.Prop_SortIndex, oldValue, value);
                }
            }
        }

		[Property("Config", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1500)]
		public string Config
		{
			get { return _config; }
			set
			{
				if ((_config == null) || (value == null) || (!value.Equals(_config)))
				{
                    object oldValue = _config;
					_config = value;
					RaisePropertyChanged(WfDefine.Prop_Config, oldValue, value);
				}
			}

		}

        [JsonIgnore]
        [Property("FormDefine", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 5000)]
        public string FormDefine
		{
			get { return _formDefine; }
			set
			{
				if ((_formDefine == null) || (value == null) || (!value.Equals(_formDefine)))
				{
                    object oldValue = _formDefine;
					_formDefine = value;
                    RaisePropertyChanged(WfDefine.Prop_FormDefine, oldValue, value);
				}
			}

		}

		[Property("Memo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1500)]
		public string Memo
		{
			get { return _memo; }
			set
			{
				if ((_memo == null) || (value == null) || (!value.Equals(_memo)))
				{
                    object oldValue = _memo;
					_memo = value;
					RaisePropertyChanged(WfDefine.Prop_Memo, oldValue, value);
				}
			}

		}

		[Property("Tag", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 5000)]
		public string Tag
		{
			get { return _tag; }
			set
			{
				if ((_tag == null) || (value == null) || (!value.Equals(_tag)))
				{
                    object oldValue = _tag;
					_tag = value;
					RaisePropertyChanged(WfDefine.Prop_Tag, oldValue, value);
				}
			}

		}

		[Property("Version", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Version
		{
			get { return _version; }
			set
			{
				if ((_version == null) || (value == null) || (!value.Equals(_version)))
				{
                    object oldValue = _version;
					_version = value;
					RaisePropertyChanged(WfDefine.Prop_Version, oldValue, value);
				}
			}

		}

		[Property("CreatorID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CreatorID
		{
			get { return _creatorID; }
			set
			{
				if ((_creatorID == null) || (value == null) || (!value.Equals(_creatorID)))
				{
                    object oldValue = _creatorID;
					_creatorID = value;
					RaisePropertyChanged(WfDefine.Prop_CreatorID, oldValue, value);
				}
			}

		}

		[Property("CreatorName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CreatorName
		{
			get { return _creatorName; }
			set
			{
				if ((_creatorName == null) || (value == null) || (!value.Equals(_creatorName)))
				{
                    object oldValue = _creatorName;
					_creatorName = value;
					RaisePropertyChanged(WfDefine.Prop_CreatorName, oldValue, value);
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
                    object oldValue = _createdDate;
					_createdDate = value;
					RaisePropertyChanged(WfDefine.Prop_CreatedDate, oldValue, value);
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
					RaisePropertyChanged(WfDefine.Prop_LastModifiedDate, oldValue, value);
				}
			}

		}

		#endregion
	} // SysWfDefine
}

