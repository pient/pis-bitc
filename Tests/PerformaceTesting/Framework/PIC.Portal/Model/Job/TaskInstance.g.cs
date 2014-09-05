// Business class SysTaskInstance generated from SysTaskInstance
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
    [ActiveRecord("TaskInstance")]
	public partial class TaskInstance : ModelBase<TaskInstance>
	{
		#region Property_Names

		public static string Prop_InstanceID = "InstanceID";
		public static string Prop_TaskID = "TaskID";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Status = "Status";
		public static string Prop_Result = "Result";
		public static string Prop_StartedTime = "StartedTime";
		public static string Prop_EndedTime = "EndedTime";
		public static string Prop_Tag = "Tag";
		public static string Prop_OwnerID = "OwnerID";
		public static string Prop_OwnerName = "OwnerName";
		public static string Prop_CreatorID = "CreatorID";
		public static string Prop_CreatorName = "CreatorName";
		public static string Prop_CreatedTime = "CreatedTime";
		public static string Prop_LastModifiedTime = "LastModifiedTime";

		#endregion

		#region Private_Variables

		private string _instanceid;
		private string _taskID;
		private string _code;
		private string _name;
		private string _status;
		private string _result;
		private DateTime? _startedTime;
		private DateTime? _endedTime;
		private string _tag;
		private string _ownerID;
		private string _ownerName;
		private string _creatorID;
		private string _creatorName;
		private DateTime? _createdTime;
		private DateTime? _lastModifiedTime;


		#endregion

		#region Constructors

		public TaskInstance()
		{
		}

		public TaskInstance(
			string p_instanceid,
			string p_taskID,
			string p_code,
			string p_name,
			string p_status,
			string p_result,
			DateTime? p_startedTime,
			DateTime? p_endedTime,
			string p_tag,
			string p_ownerID,
			string p_ownerName,
			string p_creatorID,
			string p_creatorName,
			DateTime? p_createdTime,
			DateTime? p_lastModifiedTime)
		{
			_instanceid = p_instanceid;
			_taskID = p_taskID;
			_code = p_code;
			_name = p_name;
			_status = p_status;
			_result = p_result;
			_startedTime = p_startedTime;
			_endedTime = p_endedTime;
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

		[PrimaryKey("InstanceID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string InstanceID
		{
			get { return _instanceid; }
			set { _instanceid = value; }

		}

		[Property("TaskID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string TaskID
		{
			get { return _taskID; }
			set
			{
				if ((_taskID == null) || (value == null) || (!value.Equals(_taskID)))
				{
                    object oldValue = _taskID;
					_taskID = value;
					RaisePropertyChanged(TaskInstance.Prop_TaskID, oldValue, value);
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
					RaisePropertyChanged(TaskInstance.Prop_Code, oldValue, value);
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
					RaisePropertyChanged(TaskInstance.Prop_Name, oldValue, value);
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
					RaisePropertyChanged(TaskInstance.Prop_Status, oldValue, value);
				}
			}

		}

        [Property("Result", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Result
		{
			get { return _result; }
			set
			{
				if ((_result == null) || (value == null) || (!value.Equals(_result)))
				{
                    object oldValue = _result;
					_result = value;
					RaisePropertyChanged(TaskInstance.Prop_Result, oldValue, value);
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
					RaisePropertyChanged(TaskInstance.Prop_StartedTime, oldValue, value);
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
					RaisePropertyChanged(TaskInstance.Prop_EndedTime, oldValue, value);
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
					RaisePropertyChanged(TaskInstance.Prop_Tag, oldValue, value);
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
					RaisePropertyChanged(TaskInstance.Prop_OwnerID, oldValue, value);
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
					RaisePropertyChanged(TaskInstance.Prop_OwnerName, oldValue, value);
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
					RaisePropertyChanged(TaskInstance.Prop_CreatorID, oldValue, value);
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
					RaisePropertyChanged(TaskInstance.Prop_CreatorName, oldValue, value);
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
					RaisePropertyChanged(TaskInstance.Prop_CreatedTime, oldValue, value);
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
					RaisePropertyChanged(TaskInstance.Prop_LastModifiedTime, oldValue, value);
				}
			}

		}

		#endregion
	} // SysTaskInstance
}

