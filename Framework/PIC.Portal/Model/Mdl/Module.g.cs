// Business class Module generated from SysModule
// Creator: Ray
// Created Date: [2010-04-10]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
	
namespace PIC.Portal.Model
{
    [ActiveRecord("Module")]
	public partial class Module : TreeNodeEntityBase<Module> , INotifyPropertyChanged 	
	{

		#region Property_Names

        public static string Prop_ModuleID = "ModuleID";
        public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Type = "Type";
		public static string Prop_ApplicationID = "ApplicationID";
        new public static string Prop_ParentID = "ParentID";
        new public static string Prop_Path = "Path";
        new public static string Prop_PathLevel = "PathLevel";
        new public static string Prop_IsLeaf = "IsLeaf";
		public static string Prop_Url = "Url";
		public static string Prop_Icon = "Icon";
		public static string Prop_Description = "Description";
		public static string Prop_Status = "Status";
		public static string Prop_IsSystem = "IsSystem";
        new public static string Prop_SortIndex = "SortIndex";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreateDate = "CreateDate";

		#endregion

		#region Private_Variables

        private string _moduleid;
        private string _code;
		private string _name;
		private int? _type;
		private string _applicationID;
		private string _parentID;
		private string _path;
        private int? _pathLevel;
        private bool? _isLeaf;
		private string _url;
		private string _icon;
		private string _description;
		private int? _status;
        private int? _isSystem;
        private int? _sortIndex;
		private DateTime? _lastModifiedDate;
		private DateTime? _createDate;

		#endregion

		#region Constructors

		public Module()
		{
		}

		public Module(
            string p_moduleid,
            string p_code,
			string p_name,
			int? p_type,
			string p_applicationID,
			string p_parentID,
			string p_path,
            int? p_pathLevel,
            bool? p_isLeaf,
			string p_url,
			string p_icon,
			string p_description,
			int? p_Status,
			int? p_isSystem,
            int? p_sortIndex,
			DateTime? p_lastModifiedDate,
			DateTime? p_createDate)
		{
			_moduleid = p_moduleid;
			_name = p_name;
            _code = p_code;
			_type = p_type;
			_applicationID = p_applicationID;
			_parentID = p_parentID;
			_path = p_path;
            _pathLevel = p_pathLevel;
            _isLeaf = p_isLeaf;
			_url = p_url;
			_icon = p_icon;
			_description = p_description;
			_status = p_Status;
			_isSystem = p_isSystem;
            _sortIndex = p_sortIndex;
			_lastModifiedDate = p_lastModifiedDate;
			_createDate = p_createDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("ModuleID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string ModuleID
		{
			get { return _moduleid; }
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
                    NotifyPropertyChanged(Module.Prop_Code);
                }
            }
        }

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 50)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
					_name = value;
					NotifyPropertyChanged(Module.Prop_Name);
				}
			}
		}

		[Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? Type
		{
			get { return _type; }
			set
			{
				if (value != _type)
				{
					_type = value;
					NotifyPropertyChanged(Module.Prop_Type);
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
					_applicationID = value;
					NotifyPropertyChanged(Module.Prop_ApplicationID);
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
                    _parentID = value;
                    NotifyPropertyChanged(Module.Prop_ParentID);
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
					_path = value;
					NotifyPropertyChanged(Module.Prop_Path);
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
					_pathLevel = value;
					NotifyPropertyChanged(Module.Prop_PathLevel);
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
                    RaisePropertyChanged(Enumeration.Prop_IsLeaf, oldValue, value);
                }
            }
        }

		[Property("Url", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Url
		{
			get { return _url; }
			set
			{
				if ((_url == null) || (value == null) || (!value.Equals(_url)))
				{
					_url = value;
					NotifyPropertyChanged(Module.Prop_Url);
				}
			}
		}

		[Property("Icon", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Icon
		{
			get { return _icon; }
			set
			{
				if ((_icon == null) || (value == null) || (!value.Equals(_icon)))
				{
					_icon = value;
					NotifyPropertyChanged(Module.Prop_Icon);
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
					NotifyPropertyChanged(Module.Prop_Description);
				}
			}
		}

		[Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? Status
		{
			get { return _status; }
			set
			{
				if (value != _status)
				{
					_status = value;
					NotifyPropertyChanged(Module.Prop_Status);
				}
			}
		}

		[Property("IsSystem", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? IsSystem
		{
			get { return _isSystem; }
			set
			{
				if (value != _isSystem)
				{
					_isSystem = value;
					NotifyPropertyChanged(Module.Prop_IsSystem);
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
					_sortIndex = value;
					NotifyPropertyChanged(Module.Prop_SortIndex);
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
					NotifyPropertyChanged(Module.Prop_LastModifiedDate);
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
					_createDate = value;
					NotifyPropertyChanged(Module.Prop_CreateDate);
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

    } // Module
}

