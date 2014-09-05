// Business class SysScheduler generated from SysScheduler
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
    [ActiveRecord("Scheduler")]
	public partial class Scheduler : ModelBase<Scheduler>
	{
		#region Property_Names

		public static string Prop_SchedulerID = "SchedulerID";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Type = "Type";
		public static string Prop_Catalog = "Catalog";
		public static string Prop_Status = "Status";
		public static string Prop_Config = "Config";
		public static string Prop_Memo = "Memo";
		public static string Prop_Tag = "Tag";
		public static string Prop_CreatorID = "CreatorID";
		public static string Prop_CreatorName = "CreatorName";
		public static string Prop_CreatedDate = "CreatedDate";
        public static string Prop_LastModifiedDate = "LastModifiedDate";
        public static string Prop_LastExecutedTime = "LastExecutedTime";

		#endregion

		#region Private_Variables

		private string _schedulerid;
		private string _code;
		private string _name;
		private string _type;
		private string _catalog;
		private string _status;
		private string _config;
		private string _memo;
		private string _tag;
		private string _creatorID;
		private string _creatorName;
		private DateTime? _createdDate;
		private DateTime? _lastModifiedDate;
        private DateTime? _lastExecutedTime;

		#endregion

		#region Constructors

		public Scheduler()
		{
		}

		public Scheduler(
			string p_schedulerid,
			string p_code,
			string p_name,
			string p_type,
			string p_catalog,
			string p_status,
			string p_config,
			string p_memo,
			string p_tag,
			string p_creatorID,
			string p_creatorName,
			DateTime? p_createdDate,
            DateTime? p_lastModifiedDate,
            DateTime? p_lastExecutedTime)
		{
			_schedulerid = p_schedulerid;
			_code = p_code;
			_name = p_name;
			_type = p_type;
			_catalog = p_catalog;
			_status = p_status;
			_config = p_config;
			_memo = p_memo;
			_tag = p_tag;
			_creatorID = p_creatorID;
			_creatorName = p_creatorName;
			_createdDate = p_createdDate;
			_lastModifiedDate = p_lastModifiedDate;
            _lastExecutedTime = p_lastExecutedTime;
		}

		#endregion

		#region Properties

		[PrimaryKey("SchedulerID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string SchedulerID
		{
			get { return _schedulerid; }
			set { _schedulerid = value; }

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
					RaisePropertyChanged(Scheduler.Prop_Code, oldValue, value);
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
					RaisePropertyChanged(Scheduler.Prop_Name, oldValue, value);
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
					RaisePropertyChanged(Scheduler.Prop_Type, oldValue, value);
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
					RaisePropertyChanged(Scheduler.Prop_Catalog, oldValue, value);
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
					RaisePropertyChanged(Scheduler.Prop_Status, oldValue, value);
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
					RaisePropertyChanged(Scheduler.Prop_Config, oldValue, value);
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
					RaisePropertyChanged(Scheduler.Prop_Memo, oldValue, value);
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
					RaisePropertyChanged(Scheduler.Prop_Tag, oldValue, value);
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
					RaisePropertyChanged(Scheduler.Prop_CreatorID, oldValue, value);
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
					RaisePropertyChanged(Scheduler.Prop_CreatorName, oldValue, value);
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
					RaisePropertyChanged(Scheduler.Prop_CreatedDate, oldValue, value);
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
					RaisePropertyChanged(Scheduler.Prop_LastModifiedDate, oldValue, value);
				}
			}

		}

        [Property("LastExecutedTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public DateTime? LastExecutedTime
        {
            get { return _lastExecutedTime; }
            set
            {
                if (value != _lastExecutedTime)
                {
                    object oldValue = _lastExecutedTime;
                    _lastExecutedTime = value;
                    RaisePropertyChanged(Scheduler.Prop_LastExecutedTime, oldValue, value);
                }
            }

        }

		#endregion
	} // SysScheduler
}

