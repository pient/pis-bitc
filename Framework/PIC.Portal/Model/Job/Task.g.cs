// Business class SysTask generated from SysTask
// Creator: Ray
// Created Date: [2012-05-08]

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
    [ActiveRecord("Task")]
	public partial class Task : ModelBase<Task>
	{
		#region Property_Names

		public static string Prop_TaskID = "TaskID";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Type = "Type";
		public static string Prop_Catalog = "Catalog";
		public static string Prop_Status = "Status";
		public static string Prop_Memo = "Memo";
		public static string Prop_Config = "Config";
		public static string Prop_Tag = "Tag";
		public static string Prop_OwnerID = "OwnerID";
		public static string Prop_OwnerName = "OwnerName";
		public static string Prop_CreatorID = "CreatorID";
		public static string Prop_CreatorName = "CreatorName";
		public static string Prop_CreatedTime = "CreatedTime";
		public static string Prop_LastModifiedTime = "LastModifiedTime";

		#endregion

		#region Private_Variables

		private string _taskid;
		private string _code;
		private string _name;
		private string _type;
		private string _catalog;
		private string _status;
		private string _memo;
		private string _config;
		private string _tag;
		private string _ownerID;
		private string _ownerName;
		private string _creatorID;
		private string _creatorName;
		private DateTime? _createdTime;
		private DateTime? _lastModifiedTime;


		#endregion

		#region Constructors

		public Task()
		{
		}

		public Task(
			string p_taskid,
			string p_code,
			string p_name,
			string p_type,
			string p_catalog,
			string p_status,
			string p_memo,
			string p_config,
			string p_tag,
			string p_ownerID,
			string p_ownerName,
			string p_creatorID,
			string p_creatorName,
			DateTime? p_createdTime,
			DateTime? p_lastModifiedTime)
		{
			_taskid = p_taskid;
			_code = p_code;
			_name = p_name;
			_type = p_type;
			_catalog = p_catalog;
			_status = p_status;
			_memo = p_memo;
			_config = p_config;
			_tag = p_tag;
			_ownerID = p_ownerID;
			_ownerName = p_ownerName;
			_creatorID = p_creatorID;
			_creatorName = p_creatorName;
			_createdTime = p_createdTime;
			_lastModifiedTime = p_lastModifiedTime;
		}

		#endregion

		#region Properties

		[PrimaryKey("TaskID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string TaskID
		{
			get { return _taskid; }
			set { _taskid = value; }

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
					RaisePropertyChanged(Task.Prop_Code, oldValue, value);
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
					RaisePropertyChanged(Task.Prop_Name, oldValue, value);
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
					RaisePropertyChanged(Task.Prop_Type, oldValue, value);
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
					RaisePropertyChanged(Task.Prop_Catalog, oldValue, value);
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
					RaisePropertyChanged(Task.Prop_Status, oldValue, value);
				}
			}

		}

		[Property("Memo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Memo
		{
			get { return _memo; }
			set
			{
				if ((_memo == null) || (value == null) || (!value.Equals(_memo)))
				{
                    object oldValue = _memo;
					_memo = value;
					RaisePropertyChanged(Task.Prop_Memo, oldValue, value);
				}
			}

		}

		[Property("Config", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Config
		{
			get { return _config; }
			set
			{
				if ((_config == null) || (value == null) || (!value.Equals(_config)))
				{
                    object oldValue = _config;
					_config = value;
					RaisePropertyChanged(Task.Prop_Config, oldValue, value);
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
					RaisePropertyChanged(Task.Prop_Tag, oldValue, value);
				}
			}

		}

		[Property("OwnerID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string OwnerID
		{
			get { return _ownerID; }
			set
			{
				if ((_ownerID == null) || (value == null) || (!value.Equals(_ownerID)))
				{
                    object oldValue = _ownerID;
					_ownerID = value;
					RaisePropertyChanged(Task.Prop_OwnerID, oldValue, value);
				}
			}

		}

		[Property("OwnerName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string OwnerName
		{
			get { return _ownerName; }
			set
			{
				if ((_ownerName == null) || (value == null) || (!value.Equals(_ownerName)))
				{
                    object oldValue = _ownerName;
					_ownerName = value;
					RaisePropertyChanged(Task.Prop_OwnerName, oldValue, value);
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
					RaisePropertyChanged(Task.Prop_CreatorID, oldValue, value);
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
					RaisePropertyChanged(Task.Prop_CreatorName, oldValue, value);
				}
			}

		}

		[Property("CreatedTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? CreatedTime
		{
			get { return _createdTime; }
			set
			{
				if (value != _createdTime)
				{
                    object oldValue = _createdTime;
					_createdTime = value;
					RaisePropertyChanged(Task.Prop_CreatedTime, oldValue, value);
				}
			}

		}

		[Property("LastModifiedTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? LastModifiedTime
		{
			get { return _lastModifiedTime; }
			set
			{
				if (value != _lastModifiedTime)
				{
                    object oldValue = _lastModifiedTime;
					_lastModifiedTime = value;
					RaisePropertyChanged(Task.Prop_LastModifiedTime, oldValue, value);
				}
			}

		}

		#endregion
	} // SysTask
}

