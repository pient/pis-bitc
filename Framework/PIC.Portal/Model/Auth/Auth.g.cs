// Business class SysAuth generated from SysAuth
// Creator: Ray
// Created Date: [2000-04-27]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
	
namespace PIC.Portal.Model
{
	[ActiveRecord("Auth")]
	public partial class Auth : TreeNodeEntityBase<Auth>, INotifyPropertyChanged
	{
		#region Property_Names

		public static string Prop_AuthID = "AuthID";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
        public new static string Prop_ParentID = "ParentID";
        public new static string Prop_Path = "Path";
        public new static string Prop_PathLevel = "PathLevel";
		public static string Prop_Type = "Type";
		public static string Prop_ModuleID = "ModuleID";
		public static string Prop_Data = "Data";
		public static string Prop_Description = "Description";
        public new static string Prop_SortIndex = "SortIndex";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreateDate = "CreateDate";

		#endregion

		#region Private_Variables

		private string _authid;
		private string _code;
		private string _name;
		private string _parentID;
		private string _path;
		private int? _pathLevel;
		private int? _type;
		private string _moduleID;
		private string _data;
		private string _description;
		private int? _sortIndex;
		private DateTime? _lastModifiedDate;
		private DateTime? _createDate;

		#endregion

		#region Constructors

		public Auth()
		{
		}

		public Auth(
			string p_authid,
			string p_code,
			string p_name,
			string p_parentID,
			string p_path,
			int? p_pathLevel,
			int? p_type,
			string p_moduleID,
			string p_data,
			string p_description,
			int? p_sortIndex,
			DateTime? p_lastModifiedDate,
			DateTime? p_createDate)
		{
			_authid = p_authid;
			_code = p_code;
			_name = p_name;
			_parentID = p_parentID;
			_path = p_path;
			_pathLevel = p_pathLevel;
			_type = p_type;
			_moduleID = p_moduleID;
			_data = p_data;
			_description = p_description;
			_sortIndex = p_sortIndex;
			_lastModifiedDate = p_lastModifiedDate;
			_createDate = p_createDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("AuthID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string AuthID
		{
			get { return _authid; }
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
					NotifyPropertyChanged(Auth.Prop_Code);
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
					NotifyPropertyChanged(Auth.Prop_Name);
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
					NotifyPropertyChanged(Auth.Prop_ParentID);
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
					NotifyPropertyChanged(Auth.Prop_Path);
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
					NotifyPropertyChanged(Auth.Prop_PathLevel);
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
					NotifyPropertyChanged(Auth.Prop_Type);
				}
			}
		}

		[Property("ModuleID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ModuleID
		{
			get { return _moduleID; }
			set
			{
				if ((_moduleID == null) || (value == null) || (!value.Equals(_moduleID)))
				{
					_moduleID = value;
					NotifyPropertyChanged(Auth.Prop_ModuleID);
				}
			}
		}

		[Property("Data", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Data
		{
			get { return _data; }
			set
			{
				if ((_data == null) || (value == null) || (!value.Equals(_data)))
				{
					_data = value;
					NotifyPropertyChanged(Auth.Prop_Data);
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
					NotifyPropertyChanged(Auth.Prop_Description);
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
					NotifyPropertyChanged(Auth.Prop_SortIndex);
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
					NotifyPropertyChanged(Auth.Prop_LastModifiedDate);
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
					NotifyPropertyChanged(Auth.Prop_CreateDate);
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

    } // OrgAuth
}

