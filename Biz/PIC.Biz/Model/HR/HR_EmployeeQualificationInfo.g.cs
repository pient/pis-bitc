// Business class HR_EmployeeQualificationInfo generated from HR_EmployeeQualificationInfo
// Creator: Ray
// Created Date: [2012-04-08]

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
	[ActiveRecord("HR_EmployeeQualificationInfo")]
	public partial class HR_EmployeeQualificationInfo : BizModelBase<HR_EmployeeQualificationInfo>
	{
		#region Property_Names

        public static string Prop_EmployeeId = "EmployeeId";
		public static string Prop_UserId = "UserId";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Qualification = "Qualification";
		public static string Prop_Memo = "Memo";
		public static string Prop_QualificationId = "QualificationId";
		public static string Prop_DepartmentId = "DepartmentId";
		public static string Prop_DepartmentName = "DepartmentName";
		public static string Prop_SupervisorId = "SupervisorId";
		public static string Prop_SupervisorName = "SupervisorName";
		public static string Prop_Sex = "Sex";
		public static string Prop_IDNumber = "IDNumber";
		public static string Prop_Type = "Type";
		public static string Prop_Status = "Status";

		#endregion

		#region Private_Variables

		private string _employeeid;
		private string _userId;
		private string _code;
		private string _name;
		private string _qualification;
		private string _memo;
		private string _qualificationId;
		private string _departmentId;
		private string _departmentName;
		private string _supervisorId;
		private string _supervisorName;
		private string _sex;
		private string _iDNumber;
		private string _type;
		private string _status;


		#endregion

		#region Constructors

		public HR_EmployeeQualificationInfo()
		{
		}

		public HR_EmployeeQualificationInfo(
			string p_employeeid,
			string p_userId,
			string p_code,
			string p_name,
			string p_qualification,
			string p_memo,
			string p_qualificationId,
			string p_departmentId,
			string p_departmentName,
			string p_supervisorId,
			string p_supervisorName,
			string p_sex,
			string p_iDNumber,
			string p_type,
			string p_status)
		{
			_employeeid = p_employeeid;
			_userId = p_userId;
			_code = p_code;
			_name = p_name;
			_qualification = p_qualification;
			_memo = p_memo;
			_qualificationId = p_qualificationId;
			_departmentId = p_departmentId;
			_departmentName = p_departmentName;
			_supervisorId = p_supervisorId;
			_supervisorName = p_supervisorName;
			_sex = p_sex;
			_iDNumber = p_iDNumber;
			_type = p_type;
			_status = p_status;
		}

		#endregion

		#region Properties

        [PrimaryKey("EmployeeId", Access = PropertyAccess.NosetterLowercaseUnderscore, Length = 36)]
        public virtual string EmployeeId
		{
			get { return _employeeid; }
			set
			{
				if ((_employeeid == null) || (value == null) || (!value.Equals(_employeeid)))
				{
                    object oldValue = _employeeid;
					_employeeid = value;
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_EmployeeId, oldValue, value);
				}
			}

		}

		[Property("UserId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public virtual string UserId
		{
			get { return _userId; }
			set
			{
				if ((_userId == null) || (value == null) || (!value.Equals(_userId)))
				{
                    object oldValue = _userId;
					_userId = value;
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_UserId, oldValue, value);
				}
			}

		}

		[Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Code
		{
			get { return _code; }
			set
			{
				if ((_code == null) || (value == null) || (!value.Equals(_code)))
				{
                    object oldValue = _code;
					_code = value;
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_Code, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Qualification", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public virtual string Qualification
		{
			get { return _qualification; }
			set
			{
				if ((_qualification == null) || (value == null) || (!value.Equals(_qualification)))
				{
                    object oldValue = _qualification;
					_qualification = value;
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_Qualification, oldValue, value);
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
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_Memo, oldValue, value);
				}
			}

		}

		[Property("QualificationId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public virtual string QualificationId
		{
			get { return _qualificationId; }
			set
			{
				if ((_qualificationId == null) || (value == null) || (!value.Equals(_qualificationId)))
				{
                    object oldValue = _qualificationId;
					_qualificationId = value;
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_QualificationId, oldValue, value);
				}
			}

		}

		[Property("DepartmentId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public virtual string DepartmentId
		{
			get { return _departmentId; }
			set
			{
				if ((_departmentId == null) || (value == null) || (!value.Equals(_departmentId)))
				{
                    object oldValue = _departmentId;
					_departmentId = value;
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_DepartmentId, oldValue, value);
				}
			}

		}

		[Property("DepartmentName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string DepartmentName
		{
			get { return _departmentName; }
			set
			{
				if ((_departmentName == null) || (value == null) || (!value.Equals(_departmentName)))
				{
                    object oldValue = _departmentName;
					_departmentName = value;
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_DepartmentName, oldValue, value);
				}
			}

		}

		[Property("SupervisorId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public virtual string SupervisorId
		{
			get { return _supervisorId; }
			set
			{
				if ((_supervisorId == null) || (value == null) || (!value.Equals(_supervisorId)))
				{
                    object oldValue = _supervisorId;
					_supervisorId = value;
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_SupervisorId, oldValue, value);
				}
			}

		}

		[Property("SupervisorName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string SupervisorName
		{
			get { return _supervisorName; }
			set
			{
				if ((_supervisorName == null) || (value == null) || (!value.Equals(_supervisorName)))
				{
                    object oldValue = _supervisorName;
					_supervisorName = value;
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_SupervisorName, oldValue, value);
				}
			}

		}

		[Property("Sex", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Sex
		{
			get { return _sex; }
			set
			{
				if ((_sex == null) || (value == null) || (!value.Equals(_sex)))
				{
                    object oldValue = _sex;
					_sex = value;
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_Sex, oldValue, value);
				}
			}

		}

		[Property("IDNumber", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string IDNumber
		{
			get { return _iDNumber; }
			set
			{
				if ((_iDNumber == null) || (value == null) || (!value.Equals(_iDNumber)))
				{
                    object oldValue = _iDNumber;
					_iDNumber = value;
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_IDNumber, oldValue, value);
				}
			}

		}

		[Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Type
		{
			get { return _type; }
			set
			{
				if ((_type == null) || (value == null) || (!value.Equals(_type)))
				{
                    object oldValue = _type;
					_type = value;
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_Type, oldValue, value);
				}
			}

		}

		[Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Status
		{
			get { return _status; }
			set
			{
				if ((_status == null) || (value == null) || (!value.Equals(_status)))
				{
                    object oldValue = _status;
					_status = value;
					RaisePropertyChanged(HR_EmployeeQualificationInfo.Prop_Status, oldValue, value);
				}
			}

		}

		#endregion
	} // HR_EmployeeQualificationInfo
}

