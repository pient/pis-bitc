// Business class HR_EmployeeTitle generated from HR_EmployeeTitle
// Creator: Ray
// Created Date: [2012-04-29]

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
	[ActiveRecord("HR_EmployeeTitle")]
	public partial class HR_EmployeeTitle : BizModelBase<HR_EmployeeTitle>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_EmployeeId = "EmployeeId";
		public static string Prop_Title = "Title";
		public static string Prop_CertiNumber = "CertiNumber";
		public static string Prop_InauguralDate = "InauguralDate";
		public static string Prop_LanguageScore = "LanguageScore";
		public static string Prop_HoldDate = "HoldDate";
		public static string Prop_ApplyStatus = "ApplyStatus";
		public static string Prop_Attachments = "Attachments";
		public static string Prop_Memo = "Memo";
		public static string Prop_CreatorId = "CreatorId";
		public static string Prop_CreatorName = "CreatorName";
		public static string Prop_CreatedDate = "CreatedDate";
		public static string Prop_LastModifiedDate = "LastModifiedDate";

		#endregion

		#region Private_Variables

		private string _id;
		private string _employeeId;
		private string _title;
		private string _certiNumber;
		private DateTime? _inauguralDate;
		private string _languageScore;
		private DateTime? _holdDate;
		private string _applyStatus;
		private string _attachments;
		private string _memo;
		private string _creatorId;
		private string _creatorName;
		private DateTime? _createdDate;
		private DateTime? _lastModifiedDate;


		#endregion

		#region Constructors

		public HR_EmployeeTitle()
		{
		}

		public HR_EmployeeTitle(
			string p_id,
			string p_employeeId,
			string p_title,
			string p_certiNumber,
			DateTime? p_inauguralDate,
			string p_languageScore,
			DateTime? p_holdDate,
			string p_applyStatus,
			string p_attachments,
			string p_memo,
			string p_creatorId,
			string p_creatorName,
			DateTime? p_createdDate,
			DateTime? p_lastModifiedDate)
		{
			_id = p_id;
			_employeeId = p_employeeId;
			_title = p_title;
			_certiNumber = p_certiNumber;
			_inauguralDate = p_inauguralDate;
			_languageScore = p_languageScore;
			_holdDate = p_holdDate;
			_applyStatus = p_applyStatus;
			_attachments = p_attachments;
			_memo = p_memo;
			_creatorId = p_creatorId;
			_creatorName = p_creatorName;
			_createdDate = p_createdDate;
			_lastModifiedDate = p_lastModifiedDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public virtual string Id
		{
			get { return _id; }
			set { _id = value; }

		}

		[Property("EmployeeId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public virtual string EmployeeId
		{
			get { return _employeeId; }
			set
			{
				if ((_employeeId == null) || (value == null) || (!value.Equals(_employeeId)))
				{
                    object oldValue = _employeeId;
					_employeeId = value;
					RaisePropertyChanged(HR_EmployeeTitle.Prop_EmployeeId, oldValue, value);
				}
			}

		}

		[Property("Title", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string Title
		{
			get { return _title; }
			set
			{
				if ((_title == null) || (value == null) || (!value.Equals(_title)))
				{
                    object oldValue = _title;
					_title = value;
					RaisePropertyChanged(HR_EmployeeTitle.Prop_Title, oldValue, value);
				}
			}

		}

		[Property("CertiNumber", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string CertiNumber
		{
			get { return _certiNumber; }
			set
			{
				if ((_certiNumber == null) || (value == null) || (!value.Equals(_certiNumber)))
				{
                    object oldValue = _certiNumber;
					_certiNumber = value;
					RaisePropertyChanged(HR_EmployeeTitle.Prop_CertiNumber, oldValue, value);
				}
			}

		}

		[Property("InauguralDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? InauguralDate
		{
			get { return _inauguralDate; }
			set
			{
				if (value != _inauguralDate)
				{
                    object oldValue = _inauguralDate;
					_inauguralDate = value;
					RaisePropertyChanged(HR_EmployeeTitle.Prop_InauguralDate, oldValue, value);
				}
			}

		}

		[Property("LanguageScore", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string LanguageScore
		{
			get { return _languageScore; }
			set
			{
				if ((_languageScore == null) || (value == null) || (!value.Equals(_languageScore)))
				{
                    object oldValue = _languageScore;
					_languageScore = value;
					RaisePropertyChanged(HR_EmployeeTitle.Prop_LanguageScore, oldValue, value);
				}
			}

		}

		[Property("HoldDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? HoldDate
		{
			get { return _holdDate; }
			set
			{
				if (value != _holdDate)
				{
                    object oldValue = _holdDate;
					_holdDate = value;
					RaisePropertyChanged(HR_EmployeeTitle.Prop_HoldDate, oldValue, value);
				}
			}

		}

		[Property("ApplyStatus", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1500)]
		public virtual string ApplyStatus
		{
			get { return _applyStatus; }
			set
			{
				if ((_applyStatus == null) || (value == null) || (!value.Equals(_applyStatus)))
				{
                    object oldValue = _applyStatus;
					_applyStatus = value;
					RaisePropertyChanged(HR_EmployeeTitle.Prop_ApplyStatus, oldValue, value);
				}
			}

		}

		[Property("Attachments", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public virtual string Attachments
		{
			get { return _attachments; }
			set
			{
				if ((_attachments == null) || (value == null) || (!value.Equals(_attachments)))
				{
                    object oldValue = _attachments;
					_attachments = value;
					RaisePropertyChanged(HR_EmployeeTitle.Prop_Attachments, oldValue, value);
				}
			}

		}

		[Property("Memo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1500)]
		public virtual string Memo
		{
			get { return _memo; }
			set
			{
				if ((_memo == null) || (value == null) || (!value.Equals(_memo)))
				{
                    object oldValue = _memo;
					_memo = value;
					RaisePropertyChanged(HR_EmployeeTitle.Prop_Memo, oldValue, value);
				}
			}

		}

		[Property("CreatorId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public virtual string CreatorId
		{
			get { return _creatorId; }
			set
			{
				if ((_creatorId == null) || (value == null) || (!value.Equals(_creatorId)))
				{
                    object oldValue = _creatorId;
					_creatorId = value;
					RaisePropertyChanged(HR_EmployeeTitle.Prop_CreatorId, oldValue, value);
				}
			}

		}

		[Property("CreatorName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string CreatorName
		{
			get { return _creatorName; }
			set
			{
				if ((_creatorName == null) || (value == null) || (!value.Equals(_creatorName)))
				{
                    object oldValue = _creatorName;
					_creatorName = value;
					RaisePropertyChanged(HR_EmployeeTitle.Prop_CreatorName, oldValue, value);
				}
			}

		}

		[Property("CreatedDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? CreatedDate
		{
			get { return _createdDate; }
			set
			{
				if (value != _createdDate)
				{
                    object oldValue = _createdDate;
					_createdDate = value;
					RaisePropertyChanged(HR_EmployeeTitle.Prop_CreatedDate, oldValue, value);
				}
			}

		}

		[Property("LastModifiedDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? LastModifiedDate
		{
			get { return _lastModifiedDate; }
			set
			{
				if (value != _lastModifiedDate)
				{
                    object oldValue = _lastModifiedDate;
					_lastModifiedDate = value;
					RaisePropertyChanged(HR_EmployeeTitle.Prop_LastModifiedDate, oldValue, value);
				}
			}

		}

		#endregion
	} // HR_EmployeeTitle
}

