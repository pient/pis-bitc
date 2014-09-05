// Business class HR_EmployeeContract generated from HR_EmployeeContract
// Creator: Ray
// Created Date: [2012-03-21]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
using PIC.Portal;
using PIC.Portal.Model;

namespace PIC.Biz.Model
{
	[ActiveRecord("HR_EmployeeContract")]
	public partial class HR_EmployeeContract : BizModelBase<HR_EmployeeContract>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_EmployeeId = "EmployeeId";
		public static string Prop_Type = "Type";
		public static string Prop_Code = "Code";
		public static string Prop_Status = "Status";
		public static string Prop_BeginDate = "BeginDate";
		public static string Prop_EndDate = "EndDate";
		public static string Prop_TrialBeginDate = "TrialBeginDate";
		public static string Prop_TrialEndDate = "TrialEndDate";
		public static string Prop_JobContent = "JobContent";
		public static string Prop_Laborage = "Laborage";
		public static string Prop_Bonus = "Bonus";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreatedDate = "CreatedDate";
		public static string Prop_CreatorId = "CreatorId";
		public static string Prop_CreatorName = "CreatorName";

		#endregion

		#region Private_Variables

		private string _id;
		private string _employeeId;
		private string _type;
		private string _code;
		private string _status;
		private DateTime? _beginDate;
		private DateTime? _endDate;
		private DateTime? _trialBeginDate;
		private DateTime? _trialEndDate;
		private string _jobContent;
		private System.Decimal? _laborage;
		private System.Decimal? _bonus;
		private DateTime? _lastModifiedDate;
		private DateTime? _createdDate;
		private string _creatorId;
		private string _creatorName;


		#endregion

		#region Constructors

		public HR_EmployeeContract()
		{
		}

		public HR_EmployeeContract(
			string p_id,
			string p_employeeId,
			string p_type,
			string p_code,
			string p_status,
			DateTime? p_beginDate,
			DateTime? p_endDate,
			DateTime? p_trialBeginDate,
			DateTime? p_trialEndDate,
			string p_jobContent,
			System.Decimal? p_laborage,
			System.Decimal? p_bonus,
			DateTime? p_lastModifiedDate,
			DateTime? p_createdDate,
			string p_creatorId,
			string p_creatorName)
		{
			_id = p_id;
			_employeeId = p_employeeId;
			_type = p_type;
			_code = p_code;
			_status = p_status;
			_beginDate = p_beginDate;
			_endDate = p_endDate;
			_trialBeginDate = p_trialBeginDate;
			_trialEndDate = p_trialEndDate;
			_jobContent = p_jobContent;
			_laborage = p_laborage;
			_bonus = p_bonus;
			_lastModifiedDate = p_lastModifiedDate;
			_createdDate = p_createdDate;
			_creatorId = p_creatorId;
			_creatorName = p_creatorName;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			// set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("EmployeeId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string EmployeeId
		{
			get { return _employeeId; }
			set
			{
				if ((_employeeId == null) || (value == null) || (!value.Equals(_employeeId)))
				{
                    object oldValue = _employeeId;
					_employeeId = value;
					RaisePropertyChanged(HR_EmployeeContract.Prop_EmployeeId, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeContract.Prop_Type, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeContract.Prop_Code, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeContract.Prop_Status, oldValue, value);
				}
			}

		}

		[Property("BeginDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? BeginDate
		{
			get { return _beginDate; }
			set
			{
				if (value != _beginDate)
				{
                    object oldValue = _beginDate;
					_beginDate = value;
					RaisePropertyChanged(HR_EmployeeContract.Prop_BeginDate, oldValue, value);
				}
			}

		}

		[Property("EndDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? EndDate
		{
			get { return _endDate; }
			set
			{
				if (value != _endDate)
				{
                    object oldValue = _endDate;
					_endDate = value;
					RaisePropertyChanged(HR_EmployeeContract.Prop_EndDate, oldValue, value);
				}
			}

		}

		[Property("TrialBeginDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? TrialBeginDate
		{
			get { return _trialBeginDate; }
			set
			{
				if (value != _trialBeginDate)
				{
                    object oldValue = _trialBeginDate;
					_trialBeginDate = value;
					RaisePropertyChanged(HR_EmployeeContract.Prop_TrialBeginDate, oldValue, value);
				}
			}

		}

		[Property("TrialEndDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? TrialEndDate
		{
			get { return _trialEndDate; }
			set
			{
				if (value != _trialEndDate)
				{
                    object oldValue = _trialEndDate;
					_trialEndDate = value;
					RaisePropertyChanged(HR_EmployeeContract.Prop_TrialEndDate, oldValue, value);
				}
			}

		}

		[Property("JobContent", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 10)]
		public string JobContent
		{
			get { return _jobContent; }
			set
			{
				if ((_jobContent == null) || (value == null) || (!value.Equals(_jobContent)))
				{
                    object oldValue = _jobContent;
					_jobContent = value;
					RaisePropertyChanged(HR_EmployeeContract.Prop_JobContent, oldValue, value);
				}
			}

		}

		[Property("Laborage", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Laborage
		{
			get { return _laborage; }
			set
			{
				if (value != _laborage)
				{
                    object oldValue = _laborage;
					_laborage = value;
					RaisePropertyChanged(HR_EmployeeContract.Prop_Laborage, oldValue, value);
				}
			}

		}

		[Property("Bonus", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Bonus
		{
			get { return _bonus; }
			set
			{
				if (value != _bonus)
				{
                    object oldValue = _bonus;
					_bonus = value;
					RaisePropertyChanged(HR_EmployeeContract.Prop_Bonus, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeContract.Prop_LastModifiedDate, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeContract.Prop_CreatedDate, oldValue, value);
				}
			}

		}

		[Property("CreatorId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CreatorId
		{
			get { return _creatorId; }
			set
			{
				if ((_creatorId == null) || (value == null) || (!value.Equals(_creatorId)))
				{
                    object oldValue = _creatorId;
					_creatorId = value;
					RaisePropertyChanged(HR_EmployeeContract.Prop_CreatorId, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeContract.Prop_CreatorName, oldValue, value);
				}
			}

		}

		#endregion
	} // HR_EmployeeContract
}

