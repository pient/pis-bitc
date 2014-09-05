// Business class SysWfAction generated from SysWfAction
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
    [ActiveRecord("WfAction")]
	public partial class WfAction : ModelBase<WfAction>
	{
		#region Property_Names

		public static string Prop_ActionID = "ActionID";
		public static string Prop_TaskID = "TaskID";
        public static string Prop_InstanceID = "InstanceID";
        public static string Prop_RoleCode = "RoleCode";
		public static string Prop_Title = "Title";
		public static string Prop_Type = "Type";
		public static string Prop_Catalog = "Catalog";
		public static string Prop_Grade = "Grade";
		public static string Prop_Rate = "Rate";
		public static string Prop_Important = "Important";
		public static string Prop_Status = "Status";
		public static string Prop_Deadline = "Deadline";
		public static string Prop_OwnerID = "OwnerID";
        public static string Prop_OwnerName = "OwnerName";
        public static string Prop_AgentID = "AgentID";
        public static string Prop_AgentName = "AgentName";
		public static string Prop_OpenedTime = "OpenedTime";
		public static string Prop_OpenorID = "OpenorID";
		public static string Prop_OpenorName = "OpenorName";
		public static string Prop_StartedTime = "StartedTime";
		public static string Prop_ExecutorID = "ExecutorID";
		public static string Prop_ExecutorName = "ExecutorName";
		public static string Prop_ClosedTime = "ClosedTime";
		public static string Prop_CloserID = "CloserID";
		public static string Prop_CloserName = "CloserName";
		public static string Prop_Description = "Description";
		public static string Prop_Tag = "Tag";
		public static string Prop_CreatorID = "CreatorID";
		public static string Prop_CreatorName = "CreatorName";
		public static string Prop_CreatedTime = "CreatedTime";
		public static string Prop_LastModifiedTime = "LastModifiedTime";

		#endregion

		#region Private_Variables

		private string _actionid;
		private string _taskID;
        private string _instanceID;
        private string _roleCode;
		private string _title;
		private string _type;
		private string _catalog;
		private string _grade;
		private System.Decimal? _rate;
		private string _important;
		private string _status;
		private DateTime? _deadline;
		private string _ownerID;
        private string _ownerName;
        private string _agentID;
        private string _agentName;
		private DateTime? _openedTime;
		private string _openorID;
		private string _openorName;
		private DateTime? _startedTime;
		private string _executorID;
		private string _executorName;
		private DateTime? _closedTime;
		private string _closerID;
		private string _closerName;
		private string _description;
		private string _tag;
		private string _creatorID;
		private string _creatorName;
		private DateTime? _createdTime;
		private DateTime? _lastModifiedTime;


		#endregion

		#region Constructors

		public WfAction()
		{
		}

		public WfAction(
			string p_actionid,
			string p_taskID,
            string p_instanceID,
            string p_roleCode,
			string p_title,
			string p_type,
			string p_catalog,
			string p_grade,
			System.Decimal? p_rate,
			string p_important,
			string p_status,
			DateTime? p_deadline,
			string p_ownerID,
			string p_ownerName,
			DateTime? p_openedTime,
			string p_openorID,
			string p_openorName,
			DateTime? p_startedTime,
			string p_executorID,
			string p_executorName,
			DateTime? p_closedTime,
			string p_closerID,
			string p_closerName,
			string p_description,
			string p_tag,
			string p_creatorID,
			string p_creatorName,
			DateTime? p_createdTime,
			DateTime? p_lastModifiedTime)
		{
			_actionid = p_actionid;
			_taskID = p_taskID;
            _instanceID = p_instanceID;
            _roleCode = p_roleCode;
			_title = p_title;
			_type = p_type;
			_catalog = p_catalog;
			_grade = p_grade;
			_rate = p_rate;
			_important = p_important;
			_status = p_status;
			_deadline = p_deadline;
			_ownerID = p_ownerID;
			_ownerName = p_ownerName;
			_openedTime = p_openedTime;
			_openorID = p_openorID;
			_openorName = p_openorName;
			_startedTime = p_startedTime;
			_executorID = p_executorID;
			_executorName = p_executorName;
			_closedTime = p_closedTime;
			_closerID = p_closerID;
			_closerName = p_closerName;
			_description = p_description;
			_tag = p_tag;
			_creatorID = p_creatorID;
			_creatorName = p_creatorName;
			_createdTime = p_createdTime;
			_lastModifiedTime = p_lastModifiedTime;
		}

		#endregion

		#region Properties

		[PrimaryKey("ActionID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string ActionID
		{
			get { return _actionid; }
			set { _actionid = value; }

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
					RaisePropertyChanged(WfAction.Prop_TaskID, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_InstanceID, oldValue, value);
				}
			}

		}

        [Property("RoleCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string RoleCode
        {
            get { return _roleCode; }
            set
            {
                if ((_roleCode == null) || (value == null) || (!value.Equals(_roleCode)))
                {
                    object oldValue = _roleCode;
                    _roleCode = value;
                    RaisePropertyChanged(WfAction.Prop_RoleCode, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_Title, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_Type, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_Catalog, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_Grade, oldValue, value);
				}
			}

		}

		[Property("Rate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Rate
		{
			get { return _rate; }
			set
			{
				if (value != _rate)
				{
                    object oldValue = _rate;
					_rate = value;
					RaisePropertyChanged(WfAction.Prop_Rate, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_Important, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_Status, oldValue, value);
				}
			}

		}

		[Property("Deadline", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? Deadline
		{
			get { return _deadline; }
			set
			{
				if (value != _deadline)
				{
                    object oldValue = _deadline;
					_deadline = value;
					RaisePropertyChanged(WfAction.Prop_Deadline, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_OwnerID, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_OwnerName, oldValue, value);
				}
			}

        }

        [Property("AgentID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
        public string AgentID
        {
            get { return _agentID; }
            set
            {
                if ((_agentID == null) || (value == null) || (!value.Equals(_agentID)))
                {
                    object oldValue = _agentID;
                    _agentID = value;
                    RaisePropertyChanged(WfAction.Prop_AgentID, oldValue, value);
                }
            }

        }

        [Property("AgentName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string AgentName
        {
            get { return _agentName; }
            set
            {
                if ((_agentName == null) || (value == null) || (!value.Equals(_agentName)))
                {
                    object oldValue = _agentName;
                    _agentName = value;
                    RaisePropertyChanged(WfAction.Prop_AgentName, oldValue, value);
                }
            }

        }

		[Property("OpenedTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? OpenedTime
		{
			get { return _openedTime; }
			set
			{
				if (value != _openedTime)
				{
                    object oldValue = _openedTime;
					_openedTime = value;
					RaisePropertyChanged(WfAction.Prop_OpenedTime, oldValue, value);
				}
			}

		}

		[Property("OpenorID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string OpenorID
		{
			get { return _openorID; }
			set
			{
				if ((_openorID == null) || (value == null) || (!value.Equals(_openorID)))
				{
                    object oldValue = _openorID;
					_openorID = value;
					RaisePropertyChanged(WfAction.Prop_OpenorID, oldValue, value);
				}
			}

		}

		[Property("OpenorName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string OpenorName
		{
			get { return _openorName; }
			set
			{
				if ((_openorName == null) || (value == null) || (!value.Equals(_openorName)))
				{
                    object oldValue = _openorName;
					_openorName = value;
					RaisePropertyChanged(WfAction.Prop_OpenorName, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_StartedTime, oldValue, value);
				}
			}

		}

		[Property("ExecutorID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ExecutorID
		{
			get { return _executorID; }
			set
			{
				if ((_executorID == null) || (value == null) || (!value.Equals(_executorID)))
				{
                    object oldValue = _executorID;
					_executorID = value;
					RaisePropertyChanged(WfAction.Prop_ExecutorID, oldValue, value);
				}
			}

		}

		[Property("ExecutorName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ExecutorName
		{
			get { return _executorName; }
			set
			{
				if ((_executorName == null) || (value == null) || (!value.Equals(_executorName)))
				{
                    object oldValue = _executorName;
					_executorName = value;
					RaisePropertyChanged(WfAction.Prop_ExecutorName, oldValue, value);
				}
			}

		}

		[Property("ClosedTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? ClosedTime
		{
			get { return _closedTime; }
			set
			{
				if (value != _closedTime)
				{
                    object oldValue = _closedTime;
					_closedTime = value;
					RaisePropertyChanged(WfAction.Prop_ClosedTime, oldValue, value);
				}
			}

		}

		[Property("CloserID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CloserID
		{
			get { return _closerID; }
			set
			{
				if ((_closerID == null) || (value == null) || (!value.Equals(_closerID)))
				{
                    object oldValue = _closerID;
					_closerID = value;
					RaisePropertyChanged(WfAction.Prop_CloserID, oldValue, value);
				}
			}

		}

		[Property("CloserName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CloserName
		{
			get { return _closerName; }
			set
			{
				if ((_closerName == null) || (value == null) || (!value.Equals(_closerName)))
				{
                    object oldValue = _closerName;
					_closerName = value;
					RaisePropertyChanged(WfAction.Prop_CloserName, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_Description, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_Tag, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_CreatorID, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_CreatorName, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_CreatedTime, oldValue, value);
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
					RaisePropertyChanged(WfAction.Prop_LastModifiedTime, oldValue, value);
				}
			}

		}

		#endregion
	} // SysWfAction
}

