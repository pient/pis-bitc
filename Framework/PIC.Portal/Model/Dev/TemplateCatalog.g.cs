// Business class TemplateCatalog generated from TemplateCatalog
// Creator: Ray
// Created Date: [2013-08-22]

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
	[ActiveRecord("TemplateCatalog")]
    public partial class TemplateCatalog : EditSensitiveTreeNodeEntityBase<TemplateCatalog>
	{
		#region Property_Names

		public static string Prop_CatalogID = "CatalogID";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Type = "Type";
        public new static string Prop_ParentID = "ParentID";
        public new static string Prop_Path = "Path";
        public new static string Prop_PathLevel = "PathLevel";
        public new static string Prop_IsLeaf = "IsLeaf";
        public new static string Prop_SortIndex = "SortIndex";
		public static string Prop_EditStatus = "EditStatus";
		public static string Prop_Config = "Config";
		public static string Prop_Tag = "Tag";
		public static string Prop_Description = "Description";
		public static string Prop_CreaterID = "CreaterID";
		public static string Prop_CreaterName = "CreaterName";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreatedDate = "CreatedDate";

		#endregion

		#region Private_Variables

		private string _catalogid;
		private string _code;
		private string _name;
		private string _type;
		private string _parentID;
		private string _path;
		private int? _pathLevel;
		private bool? _isLeaf;
		private int? _sortIndex;
		private string _editStatus;
		private string _config;
		private string _tag;
		private string _description;
		private string _createrID;
		private string _createrName;
		private DateTime? _lastModifiedDate;
		private DateTime? _createdDate;


		#endregion

		#region Constructors

		public TemplateCatalog()
		{
		}

		public TemplateCatalog(
			string p_catalogID,
			string p_code,
			string p_name,
			string p_type,
			string p_parentID,
			string p_path,
			int? p_pathLevel,
			bool? p_isLeaf,
			int? p_sortIndex,
			string p_editStatus,
			string p_config,
			string p_tag,
			string p_description,
			string p_createrID,
			string p_createrName,
			DateTime? p_lastModifiedDate,
			DateTime? p_createdDate)
		{
			_catalogid = p_catalogID;
			_code = p_code;
			_name = p_name;
			_type = p_type;
			_parentID = p_parentID;
			_path = p_path;
			_pathLevel = p_pathLevel;
			_isLeaf = p_isLeaf;
			_sortIndex = p_sortIndex;
			_editStatus = p_editStatus;
			_config = p_config;
			_tag = p_tag;
			_description = p_description;
			_createrID = p_createrID;
			_createrName = p_createrName;
			_lastModifiedDate = p_lastModifiedDate;
			_createdDate = p_createdDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("CatalogID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string CatalogID
		{
			get { return _catalogid; }
			set { _catalogid = value; }

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
					RaisePropertyChanged(TemplateCatalog.Prop_Code, oldValue, value);
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
					RaisePropertyChanged(TemplateCatalog.Prop_Name, oldValue, value);
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
					RaisePropertyChanged(TemplateCatalog.Prop_Type, oldValue, value);
				}
			}

		}

		[Property("ParentID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
        public override string ParentID
		{
			get { return _parentID; }
			set
			{
				if ((_parentID == null) || (value == null) || (!value.Equals(_parentID)))
				{
                    object oldValue = _parentID;
					_parentID = value;
					RaisePropertyChanged(TemplateCatalog.Prop_ParentID, oldValue, value);
				}
			}

		}

		[Property("Path", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1000)]
        public override string Path
		{
			get { return _path; }
			set
			{
				if ((_path == null) || (value == null) || (!value.Equals(_path)))
				{
                    object oldValue = _path;
					_path = value;
					RaisePropertyChanged(TemplateCatalog.Prop_Path, oldValue, value);
				}
			}

		}

		[Property("PathLevel", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public override int? PathLevel
		{
			get { return _pathLevel; }
			set
			{
				if (value != _pathLevel)
				{
                    object oldValue = _pathLevel;
					_pathLevel = value;
					RaisePropertyChanged(TemplateCatalog.Prop_PathLevel, oldValue, value);
				}
			}

		}

		[Property("IsLeaf", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public override bool? IsLeaf
		{
			get { return _isLeaf; }
			set
			{
				if (value != _isLeaf)
				{
                    object oldValue = _isLeaf;
					_isLeaf = value;
					RaisePropertyChanged(TemplateCatalog.Prop_IsLeaf, oldValue, value);
				}
			}

		}

		[Property("SortIndex", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public override int? SortIndex
		{
			get { return _sortIndex; }
			set
			{
				if (value != _sortIndex)
				{
                    object oldValue = _sortIndex;
					_sortIndex = value;
					RaisePropertyChanged(TemplateCatalog.Prop_SortIndex, oldValue, value);
				}
			}

		}

		[Property("EditStatus", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public override string EditStatus
		{
			get { return _editStatus; }
			set
			{
				if ((_editStatus == null) || (value == null) || (!value.Equals(_editStatus)))
				{
                    object oldValue = _editStatus;
					_editStatus = value;
					RaisePropertyChanged(TemplateCatalog.Prop_EditStatus, oldValue, value);
				}
			}

		}

		[Property("Config", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 5000)]
		public string Config
		{
			get { return _config; }
			set
			{
				if ((_config == null) || (value == null) || (!value.Equals(_config)))
				{
                    object oldValue = _config;
					_config = value;
					RaisePropertyChanged(TemplateCatalog.Prop_Config, oldValue, value);
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
					RaisePropertyChanged(TemplateCatalog.Prop_Tag, oldValue, value);
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
                    object oldValue = _description;
					_description = value;
					RaisePropertyChanged(TemplateCatalog.Prop_Description, oldValue, value);
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
                    object oldValue = _createrID;
					_createrID = value;
					RaisePropertyChanged(TemplateCatalog.Prop_CreaterID, oldValue, value);
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
                    object oldValue = _createrName;
					_createrName = value;
					RaisePropertyChanged(TemplateCatalog.Prop_CreaterName, oldValue, value);
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
					RaisePropertyChanged(TemplateCatalog.Prop_LastModifiedDate, oldValue, value);
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
					RaisePropertyChanged(TemplateCatalog.Prop_CreatedDate, oldValue, value);
				}
			}

		}

        #endregion
	} // TemplateCatalog
}

