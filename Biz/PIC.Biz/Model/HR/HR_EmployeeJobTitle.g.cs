// Business class HR_EmployeeJobTitle generated from HR_EmployeeJobTitle
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
	[ActiveRecord("HR_EmployeeJobTitle")]
	public partial class HR_EmployeeJobTitle : BizModelBase<HR_EmployeeJobTitle>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_EmployeeId = "EmployeeId";
		public static string Prop_Name = "Name";
		public static string Prop_Number = "Number";
		public static string Prop_HoldDate = "HoldDate";
		public static string Prop_HoldWay = "HoldWay";
		public static string Prop_ValidityDate = "ValidityDate";
		public static string Prop_AuditDate = "AuditDate";
		public static string Prop_RegisterDate = "RegisterDate";
		public static string Prop_RegisterPlace = "RegisterPlace";
		public static string Prop_Picture = "Picture";
		public static string Prop_Attachments = "Attachments";
		public static string Prop_Memo = "Memo";
		public static string Prop_ApplyStatus = "ApplyStatus";
		public static string Prop_CreatorId = "CreatorId";
		public static string Prop_CreatorName = "CreatorName";
		public static string Prop_CreatedDate = "CreatedDate";
		public static string Prop_LastModifiedDate = "LastModifiedDate";

		#endregion

		#region Private_Variables

		private string _id;
		private string _employeeId;
		private string _name;
		private string _number;
		private DateTime? _holdDate;
		private string _holdWay;
		private DateTime? _validityDate;
		private DateTime? _auditDate;
		private DateTime? _registerDate;
		private string _registerPlace;
		private string _picture;
		private string _attachments;
		private string _memo;
		private string _applyStatus;
		private string _creatorId;
		private string _creatorName;
		private DateTime? _createdDate;
		private DateTime? _lastModifiedDate;


		#endregion

		#region Constructors

		public HR_EmployeeJobTitle()
		{
		}

		public HR_EmployeeJobTitle(
			string p_id,
			string p_employeeId,
			string p_name,
			string p_number,
			DateTime? p_holdDate,
			string p_holdWay,
			DateTime? p_validityDate,
			DateTime? p_auditDate,
			DateTime? p_registerDate,
			string p_registerPlace,
			string p_picture,
			string p_attachments,
			string p_memo,
			string p_applyStatus,
			string p_creatorId,
			string p_creatorName,
			DateTime? p_createdDate,
			DateTime? p_lastModifiedDate)
		{
			_id = p_id;
			_employeeId = p_employeeId;
			_name = p_name;
			_number = p_number;
			_holdDate = p_holdDate;
			_holdWay = p_holdWay;
			_validityDate = p_validityDate;
			_auditDate = p_auditDate;
			_registerDate = p_registerDate;
			_registerPlace = p_registerPlace;
			_picture = p_picture;
			_attachments = p_attachments;
			_memo = p_memo;
			_applyStatus = p_applyStatus;
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
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_EmployeeId, oldValue, value);
				}
			}

		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Number", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Number
		{
			get { return _number; }
			set
			{
				if ((_number == null) || (value == null) || (!value.Equals(_number)))
				{
                    object oldValue = _number;
					_number = value;
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_Number, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_HoldDate, oldValue, value);
				}
			}

		}

		[Property("HoldWay", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string HoldWay
		{
			get { return _holdWay; }
			set
			{
				if ((_holdWay == null) || (value == null) || (!value.Equals(_holdWay)))
				{
                    object oldValue = _holdWay;
					_holdWay = value;
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_HoldWay, oldValue, value);
				}
			}

		}

		[Property("ValidityDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? ValidityDate
		{
			get { return _validityDate; }
			set
			{
				if (value != _validityDate)
				{
                    object oldValue = _validityDate;
					_validityDate = value;
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_ValidityDate, oldValue, value);
				}
			}

		}

		[Property("AuditDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? AuditDate
		{
			get { return _auditDate; }
			set
			{
				if (value != _auditDate)
				{
                    object oldValue = _auditDate;
					_auditDate = value;
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_AuditDate, oldValue, value);
				}
			}

		}

		[Property("RegisterDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? RegisterDate
		{
			get { return _registerDate; }
			set
			{
				if (value != _registerDate)
				{
                    object oldValue = _registerDate;
					_registerDate = value;
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_RegisterDate, oldValue, value);
				}
			}

		}

		[Property("RegisterPlace", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string RegisterPlace
		{
			get { return _registerPlace; }
			set
			{
				if ((_registerPlace == null) || (value == null) || (!value.Equals(_registerPlace)))
				{
                    object oldValue = _registerPlace;
					_registerPlace = value;
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_RegisterPlace, oldValue, value);
				}
			}

		}

		[Property("Picture", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public virtual string Picture
		{
			get { return _picture; }
			set
			{
				if ((_picture == null) || (value == null) || (!value.Equals(_picture)))
				{
                    object oldValue = _picture;
					_picture = value;
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_Picture, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_Attachments, oldValue, value);
				}
			}

		}

		[Property("Memo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public virtual string Memo
		{
			get { return _memo; }
			set
			{
				if ((_memo == null) || (value == null) || (!value.Equals(_memo)))
				{
                    object oldValue = _memo;
					_memo = value;
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_Memo, oldValue, value);
				}
			}

		}

		[Property("ApplyStatus", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string ApplyStatus
		{
			get { return _applyStatus; }
			set
			{
				if ((_applyStatus == null) || (value == null) || (!value.Equals(_applyStatus)))
				{
                    object oldValue = _applyStatus;
					_applyStatus = value;
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_ApplyStatus, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_CreatorId, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_CreatorName, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_CreatedDate, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeJobTitle.Prop_LastModifiedDate, oldValue, value);
				}
			}

		}

		#endregion
	} // HR_EmployeeJobTitle
}

