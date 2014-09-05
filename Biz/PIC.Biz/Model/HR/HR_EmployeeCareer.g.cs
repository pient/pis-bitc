// Business class HR_EmployeeCareer generated from HR_EmployeeCareer
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
	[ActiveRecord("HR_EmployeeCareer")]
	public partial class HR_EmployeeCareer : BizModelBase<HR_EmployeeCareer>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_EmployeeId = "EmployeeId";
		public static string Prop_Company = "Company";
		public static string Prop_InDate = "InDate";
		public static string Prop_OutDate = "OutDate";
		public static string Prop_Department = "Department";
		public static string Prop_Position = "Position";
		public static string Prop_Description = "Description";
		public static string Prop_Achivement = "Achivement";
		public static string Prop_Status = "Status";
		public static string Prop_CreatedDate = "CreatedDate";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreatorId = "CreatorId";
		public static string Prop_CreatorName = "CreatorName";

		#endregion

		#region Private_Variables

		private string _id;
		private string _employeeId;
		private string _company;
		private DateTime? _inDate;
		private DateTime? _outDate;
		private string _department;
		private string _position;
		private string _description;
		private string _achivement;
		private string _status;
		private DateTime? _createdDate;
		private DateTime? _lastModifiedDate;
		private string _creatorId;
		private string _creatorName;


		#endregion

		#region Constructors

		public HR_EmployeeCareer()
		{
		}

		public HR_EmployeeCareer(
			string p_id,
			string p_employeeId,
			string p_company,
			DateTime? p_inDate,
			DateTime? p_outDate,
			string p_department,
			string p_position,
			string p_description,
			string p_achivement,
			string p_status,
			DateTime? p_createdDate,
			DateTime? p_lastModifiedDate,
			string p_creatorId,
			string p_creatorName)
		{
			_id = p_id;
			_employeeId = p_employeeId;
			_company = p_company;
			_inDate = p_inDate;
			_outDate = p_outDate;
			_department = p_department;
			_position = p_position;
			_description = p_description;
			_achivement = p_achivement;
			_status = p_status;
			_createdDate = p_createdDate;
			_lastModifiedDate = p_lastModifiedDate;
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
					RaisePropertyChanged(HR_EmployeeCareer.Prop_EmployeeId, oldValue, value);
				}
			}

		}

		[Property("Company", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public string Company
		{
			get { return _company; }
			set
			{
				if ((_company == null) || (value == null) || (!value.Equals(_company)))
				{
                    object oldValue = _company;
					_company = value;
					RaisePropertyChanged(HR_EmployeeCareer.Prop_Company, oldValue, value);
				}
			}

		}

		[Property("InDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? InDate
		{
			get { return _inDate; }
			set
			{
				if (value != _inDate)
				{
                    object oldValue = _inDate;
					_inDate = value;
					RaisePropertyChanged(HR_EmployeeCareer.Prop_InDate, oldValue, value);
				}
			}

		}

		[Property("OutDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? OutDate
		{
			get { return _outDate; }
			set
			{
				if (value != _outDate)
				{
                    object oldValue = _outDate;
					_outDate = value;
					RaisePropertyChanged(HR_EmployeeCareer.Prop_OutDate, oldValue, value);
				}
			}

		}

		[Property("Department", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public string Department
		{
			get { return _department; }
			set
			{
				if ((_department == null) || (value == null) || (!value.Equals(_department)))
				{
                    object oldValue = _department;
					_department = value;
					RaisePropertyChanged(HR_EmployeeCareer.Prop_Department, oldValue, value);
				}
			}

		}

		[Property("Position", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public string Position
		{
			get { return _position; }
			set
			{
				if ((_position == null) || (value == null) || (!value.Equals(_position)))
				{
                    object oldValue = _position;
					_position = value;
					RaisePropertyChanged(HR_EmployeeCareer.Prop_Position, oldValue, value);
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
                    object oldValue = _description;
					_description = value;
					RaisePropertyChanged(HR_EmployeeCareer.Prop_Description, oldValue, value);
				}
			}

		}

		[Property("Achivement", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Achivement
		{
			get { return _achivement; }
			set
			{
				if ((_achivement == null) || (value == null) || (!value.Equals(_achivement)))
				{
                    object oldValue = _achivement;
					_achivement = value;
					RaisePropertyChanged(HR_EmployeeCareer.Prop_Achivement, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeCareer.Prop_Status, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeCareer.Prop_CreatedDate, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeCareer.Prop_LastModifiedDate, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeCareer.Prop_CreatorId, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeCareer.Prop_CreatorName, oldValue, value);
				}
			}

		}

		#endregion
	} // HR_EmployeeCareer
}

