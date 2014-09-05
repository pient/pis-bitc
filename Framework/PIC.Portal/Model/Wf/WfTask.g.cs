// Business class SysWfTask generated from SysWfTask
// Creator: Ray
// Created Date: [2012-04-28]

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
    [ActiveRecord("WfTask")]
	public partial class WfTask : ModelBase<WfTask>
	{
		#region Property_Names

		public static string Prop_TaskID = "TaskID";
		public static string Prop_DefineID = "DefineID";
		public static string Prop_InstanceID = "InstanceID";
		public static string Prop_Code = "Code";
		public static string Prop_Type = "Type";
		public static string Prop_Catalog = "Catalog";
		public static string Prop_Grade = "Grade";
		public static string Prop_Important = "Important";
		public static string Prop_Title = "Title";
		public static string Prop_Status = "Status";
		public static string Prop_Description = "Description";
		public static string Prop_Tag = "Tag";
		public static string Prop_StartedTime = "StartedTime";
		public static string Prop_EndedTime = "EndedTime";
		public static string Prop_OwnerID = "OwnerID";
		public static string Prop_OwnerName = "OwnerName";
		public static string Prop_CreatorID = "CreatorID";
		public static string Prop_CreatorName = "CreatorName";
		public static string Prop_CreatedTime = "CreatedTime";
		public static string Prop_LastModifiedTime = "LastModifiedTime";

		#endregion

		#region Private_Variables

		private string _taskid;
		private string _defineID;
		private string _instanceID;
		private string _code;
		private string _type;
		private string _catalog;
		private string _grade;
		private string _important;
		private string _title;
		private string _status;
		private string _description;
		private string _tag;
		private DateTime? _startedTime;
		private DateTime? _endedTime;
		private string _ownerID;
		private string _ownerName;
		private string _creatorID;
		private string _creatorName;
		private DateTime? _createdTime;
		private DateTime? _lastModifiedTime;


		#endregion

		#region Constructors

		public WfTask()
		{
		}

		public WfTask(
			string p_taskid,
			string p_defineID,
			string p_instanceID,
			string p_code,
			string p_type,
			string p_catalog,
			string p_grade,
			string p_important,
			string p_title,
			string p_status,
			string p_description,
			string p_tag,
			DateTime? p_startedTime,
			DateTime? p_endedTime,
			string p_ownerID,
			string p_ownerName,
			string p_creatorID,
			string p_creatorName,
			DateTime? p_createdTime,
			DateTime? p_lastModifiedTime)
		{
			_taskid = p_taskid;
			_defineID = p_defineID;
			_instanceID = p_instanceID;
			_code = p_code;
			_type = p_type;
			_catalog = p_catalog;
			_grade = p_grade;
			_important = p_important;
			_title = p_title;
			_status = p_status;
			_description = p_description;
			_tag = p_tag;
			_startedTime = p_startedTime;
			_endedTime = p_endedTime;
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

		[Property("DefineID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string DefineID
		{
			get { return _defineID; }
			set
			{
				if ((_defineID == null) || (value == null) || (!value.Equals(_defineID)))
				{
                    object oldValue = _defineID;
					_defineID = value;
					RaisePropertyChanged(WfTask.Prop_DefineID, oldValue, value);
				}
			}

		}

		[Property("InstanceID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string InstanceID
		{
			get { return _instanceID; }
			set
			{
				if ((_instanceID == null) || (value == null) || (!value.Equals(_instanceID)))
				{
                    object oldValue = _instanceID;
					_instanceID = value;
					RaisePropertyChanged(WfTask.Prop_InstanceID, oldValue, value);
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
					RaisePropertyChanged(WfTask.Prop_Code, oldValue, value);
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
					RaisePropertyChanged(WfTask.Prop_Type, oldValue, value);
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
					RaisePropertyChanged(WfTask.Prop_Catalog, oldValue, value);
				}
			}

		}

		[Property("Grade", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Grade
		{
			get { return _grade; }
			set
			{
				if ((_grade == null) || (value == null) || (!value.Equals(_grade)))
				{
                    object oldValue = _grade;
					_grade = value;
					RaisePropertyChanged(WfTask.Prop_Grade, oldValue, value);
				}
			}

		}

		[Property("Important", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Important
		{
			get { return _important; }
			set
			{
				if ((_important == null) || (value == null) || (!value.Equals(_important)))
				{
                    object oldValue = _important;
					_important = value;
					RaisePropertyChanged(WfTask.Prop_Important, oldValue, value);
				}
			}

		}

		[Property("Title", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public string Title
		{
			get { return _title; }
			set
			{
				if ((_title == null) || (value == null) || (!value.Equals(_title)))
				{
                    object oldValue = _title;
					_title = value;
					RaisePropertyChanged(WfTask.Prop_Title, oldValue, value);
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
					RaisePropertyChanged(WfTask.Prop_Status, oldValue, value);
				}
			}

		}

		[Property("Description", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1500)]
		public string Description
		{
			get { return _description; }
			set
			{
				if ((_description == null) || (value == null) || (!value.Equals(_description)))
				{
                    object oldValue = _description;
					_description = value;
					RaisePropertyChanged(WfTask.Prop_Description, oldValue, value);
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
					RaisePropertyChanged(WfTask.Prop_Tag, oldValue, value);
				}
			}

		}

		[Property("StartedTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? StartedTime
		{
			get { return _startedTime; }
			set
			{
				if (value != _startedTime)
				{
                    object oldValue = _startedTime;
					_startedTime = value;
					RaisePropertyChanged(WfTask.Prop_StartedTime, oldValue, value);
				}
			}

		}

		[Property("EndedTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? EndedTime
		{
			get { return _endedTime; }
			set
			{
				if (value != _endedTime)
				{
                    object oldValue = _endedTime;
					_endedTime = value;
					RaisePropertyChanged(WfTask.Prop_EndedTime, oldValue, value);
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
					RaisePropertyChanged(WfTask.Prop_OwnerID, oldValue, value);
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
					RaisePropertyChanged(WfTask.Prop_OwnerName, oldValue, value);
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
					RaisePropertyChanged(WfTask.Prop_CreatorID, oldValue, value);
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
					RaisePropertyChanged(WfTask.Prop_CreatorName, oldValue, value);
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
					RaisePropertyChanged(WfTask.Prop_CreatedTime, oldValue, value);
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
					RaisePropertyChanged(WfTask.Prop_LastModifiedTime, oldValue, value);
				}
			}

		}

		#endregion
	} // SysWfTask
}

