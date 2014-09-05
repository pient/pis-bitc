// Business class HR_Employee generated from HR_Employee
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
	[ActiveRecord("HR_Employee")]
	public partial class HR_Employee : BizModelBase<HR_Employee>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_UserId = "UserId";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Status = "Status";
		public static string Prop_Type = "Type";
		public static string Prop_UsedName = "UsedName";
		public static string Prop_IDNumber = "IDNumber";
		public static string Prop_Birthday = "Birthday";
		public static string Prop_Sex = "Sex";
		public static string Prop_People = "People";
		public static string Prop_Email = "Email";
		public static string Prop_Phone = "Phone";
		public static string Prop_Phone2 = "Phone2";
		public static string Prop_HomePhone = "HomePhone";
		public static string Prop_Mobile = "Mobile";
		public static string Prop_FileCode = "FileCode";
		public static string Prop_AttendanceType = "AttendanceType";
		public static string Prop_Photo = "Photo";
		public static string Prop_Region = "Region";
		public static string Prop_Post = "Post";
		public static string Prop_NativePlace = "NativePlace";
		public static string Prop_ContactAddress = "ContactAddress";
		public static string Prop_HouseholdAddress = "HouseholdAddress";
		public static string Prop_SupervisorId = "SupervisorId";
		public static string Prop_SupervisorName = "SupervisorName";
		public static string Prop_DepartmentId = "DepartmentId";
		public static string Prop_DepartmentName = "DepartmentName";
		public static string Prop_MajorCode = "MajorCode";
		public static string Prop_MajorName = "MajorName";
		public static string Prop_ContractStartDate = "ContractStartDate";
		public static string Prop_ContractEndDate = "ContractEndDate";
		public static string Prop_ProbationStartDate = "ProbationStartDate";
		public static string Prop_ProbationEndDate = "ProbationEndDate";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_RegularizedDate = "RegularizedDate";
		public static string Prop_InauguralDate = "InauguralDate";
		public static string Prop_IDPhotoFront = "IDPhotoFront";
		public static string Prop_IDPhotoBack = "IDPhotoBack";
		public static string Prop_Signature = "Signature";
		public static string Prop_PoliticalStatus = "PoliticalStatus";
		public static string Prop_MarriageStatus = "MarriageStatus";
		public static string Prop_SpouseName = "SpouseName";
		public static string Prop_Memo = "Memo";
		public static string Prop_DutyForShow = "DutyForShow";
		public static string Prop_PostQualification = "PostQualification";
		public static string Prop_School = "School";
		public static string Prop_SchoolMajor = "SchoolMajor";
		public static string Prop_GraduatedDate = "GraduatedDate";
		public static string Prop_EducationHistory = "EducationHistory";
		public static string Prop_Title = "Title";
		public static string Prop_OrigDepartment = "OrigDepartment";
		public static string Prop_InnerTitle = "InnerTitle";
		public static string Prop_TrailPay = "TrailPay";
		public static string Prop_FormalPay = "FormalPay";
		public static string Prop_PayAdjustment = "PayAdjustment";
		public static string Prop_GeneralSafety = "GeneralSafety";
		public static string Prop_Safety = "Safety";
		public static string Prop_SafetyCode = "SafetyCode";
		public static string Prop_Fund = "Fund";
		public static string Prop_FundCode = "FundCode";
		public static string Prop_ManualLabor = "ManualLabor";
		public static string Prop_ResidenceValid = "ResidenceValid";
		public static string Prop_HireStatus = "HireStatus";
		public static string Prop_Tag = "Tag";
		public static string Prop_TagA = "TagA";
		public static string Prop_TagB = "TagB";
		public static string Prop_TagC = "TagC";
		public static string Prop_CreatedDate = "CreatedDate";
		public static string Prop_CreatorId = "CreatorId";
		public static string Prop_CreatorName = "CreatorName";

		#endregion

		#region Private_Variables

		private string _id;
		private string _userId;
		private string _code;
		private string _name;
		private string _status;
		private string _type;
		private string _usedName;
		private string _iDNumber;
		private DateTime? _birthday;
		private string _sex;
		private string _people;
		private string _email;
		private string _phone;
		private string _phone2;
		private string _homePhone;
		private string _mobile;
		private string _fileCode;
		private string _attendanceType;
		private string _photo;
		private string _region;
		private string _post;
		private string _nativePlace;
		private string _contactAddress;
		private string _householdAddress;
		private string _supervisorId;
		private string _supervisorName;
		private string _departmentId;
		private string _departmentName;
		private string _majorCode;
		private string _majorName;
		private DateTime? _contractStartDate;
		private DateTime? _contractEndDate;
		private DateTime? _probationStartDate;
		private DateTime? _probationEndDate;
		private DateTime? _lastModifiedDate;
		private DateTime? _regularizedDate;
		private DateTime? _inauguralDate;
		private string _iDPhotoFront;
		private string _iDPhotoBack;
		private string _signature;
		private string _politicalStatus;
		private string _marriageStatus;
		private string _spouseName;
		private string _memo;
		private string _dutyForShow;
		private string _postQualification;
		private string _school;
		private string _schoolMajor;
		private DateTime? _graduatedDate;
		private string _educationHistory;
		private string _title;
		private string _origDepartment;
		private string _innerTitle;
		private System.Decimal? _trailPay;
		private System.Decimal? _formalPay;
		private string _payAdjustment;
		private string _generalSafety;
		private string _safety;
		private string _safetyCode;
		private string _fund;
		private string _fundCode;
		private string _manualLabor;
		private string _residenceValid;
		private string _hireStatus;
		private string _tag;
		private string _tagA;
		private string _tagB;
		private string _tagC;
		private DateTime? _createdDate;
		private string _creatorId;
		private string _creatorName;


		#endregion

		#region Constructors

		public HR_Employee()
		{
		}

		public HR_Employee(
			string p_id,
			string p_userId,
			string p_code,
			string p_name,
			string p_status,
			string p_type,
			string p_usedName,
			string p_iDNumber,
			DateTime? p_birthday,
			string p_sex,
			string p_people,
			string p_email,
			string p_phone,
			string p_phone2,
			string p_homePhone,
			string p_mobile,
			string p_fileCode,
			string p_attendanceType,
			string p_photo,
			string p_region,
			string p_post,
			string p_nativePlace,
			string p_contactAddress,
			string p_householdAddress,
			string p_supervisorId,
			string p_supervisorName,
			string p_departmentId,
			string p_departmentName,
			string p_majorCode,
			string p_majorName,
			DateTime? p_contractStartDate,
			DateTime? p_contractEndDate,
			DateTime? p_probationStartDate,
			DateTime? p_probationEndDate,
			DateTime? p_lastModifiedDate,
			DateTime? p_regularizedDate,
			DateTime? p_inauguralDate,
			string p_iDPhotoFront,
			string p_iDPhotoBack,
			string p_signature,
			string p_politicalStatus,
			string p_marriageStatus,
			string p_spouseName,
			string p_memo,
			string p_dutyForShow,
			string p_postQualification,
			string p_school,
			string p_schoolMajor,
			DateTime? p_graduatedDate,
			string p_educationHistory,
			string p_title,
			string p_origDepartment,
			string p_innerTitle,
			System.Decimal? p_trailPay,
			System.Decimal? p_formalPay,
			string p_payAdjustment,
			string p_generalSafety,
			string p_safety,
			string p_safetyCode,
			string p_fund,
			string p_fundCode,
			string p_manualLabor,
			string p_residenceValid,
			string p_hireStatus,
			string p_tag,
			string p_tagA,
			string p_tagB,
			string p_tagC,
			DateTime? p_createdDate,
			string p_creatorId,
			string p_creatorName)
		{
			_id = p_id;
			_userId = p_userId;
			_code = p_code;
			_name = p_name;
			_status = p_status;
			_type = p_type;
			_usedName = p_usedName;
			_iDNumber = p_iDNumber;
			_birthday = p_birthday;
			_sex = p_sex;
			_people = p_people;
			_email = p_email;
			_phone = p_phone;
			_phone2 = p_phone2;
			_homePhone = p_homePhone;
			_mobile = p_mobile;
			_fileCode = p_fileCode;
			_attendanceType = p_attendanceType;
			_photo = p_photo;
			_region = p_region;
			_post = p_post;
			_nativePlace = p_nativePlace;
			_contactAddress = p_contactAddress;
			_householdAddress = p_householdAddress;
			_supervisorId = p_supervisorId;
			_supervisorName = p_supervisorName;
			_departmentId = p_departmentId;
			_departmentName = p_departmentName;
			_majorCode = p_majorCode;
			_majorName = p_majorName;
			_contractStartDate = p_contractStartDate;
			_contractEndDate = p_contractEndDate;
			_probationStartDate = p_probationStartDate;
			_probationEndDate = p_probationEndDate;
			_lastModifiedDate = p_lastModifiedDate;
			_regularizedDate = p_regularizedDate;
			_inauguralDate = p_inauguralDate;
			_iDPhotoFront = p_iDPhotoFront;
			_iDPhotoBack = p_iDPhotoBack;
			_signature = p_signature;
			_politicalStatus = p_politicalStatus;
			_marriageStatus = p_marriageStatus;
			_spouseName = p_spouseName;
			_memo = p_memo;
			_dutyForShow = p_dutyForShow;
			_postQualification = p_postQualification;
			_school = p_school;
			_schoolMajor = p_schoolMajor;
			_graduatedDate = p_graduatedDate;
			_educationHistory = p_educationHistory;
			_title = p_title;
			_origDepartment = p_origDepartment;
			_innerTitle = p_innerTitle;
			_trailPay = p_trailPay;
			_formalPay = p_formalPay;
			_payAdjustment = p_payAdjustment;
			_generalSafety = p_generalSafety;
			_safety = p_safety;
			_safetyCode = p_safetyCode;
			_fund = p_fund;
			_fundCode = p_fundCode;
			_manualLabor = p_manualLabor;
			_residenceValid = p_residenceValid;
			_hireStatus = p_hireStatus;
			_tag = p_tag;
			_tagA = p_tagA;
			_tagB = p_tagB;
			_tagC = p_tagC;
			_createdDate = p_createdDate;
			_creatorId = p_creatorId;
			_creatorName = p_creatorName;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public virtual string Id
		{
			get { return _id; }
			set { _id = value; }

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
					RaisePropertyChanged(HR_Employee.Prop_UserId, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_Code, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_Name, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_Status, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_Type, oldValue, value);
				}
			}

		}

		[Property("UsedName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string UsedName
		{
			get { return _usedName; }
			set
			{
				if ((_usedName == null) || (value == null) || (!value.Equals(_usedName)))
				{
                    object oldValue = _usedName;
					_usedName = value;
					RaisePropertyChanged(HR_Employee.Prop_UsedName, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_IDNumber, oldValue, value);
				}
			}

		}

		[Property("Birthday", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? Birthday
		{
			get { return _birthday; }
			set
			{
				if (value != _birthday)
				{
                    object oldValue = _birthday;
					_birthday = value;
					RaisePropertyChanged(HR_Employee.Prop_Birthday, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_Sex, oldValue, value);
				}
			}

		}

		[Property("People", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string People
		{
			get { return _people; }
			set
			{
				if ((_people == null) || (value == null) || (!value.Equals(_people)))
				{
                    object oldValue = _people;
					_people = value;
					RaisePropertyChanged(HR_Employee.Prop_People, oldValue, value);
				}
			}

		}

		[Property("Email", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string Email
		{
			get { return _email; }
			set
			{
				if ((_email == null) || (value == null) || (!value.Equals(_email)))
				{
                    object oldValue = _email;
					_email = value;
					RaisePropertyChanged(HR_Employee.Prop_Email, oldValue, value);
				}
			}

		}

		[Property("Phone", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Phone
		{
			get { return _phone; }
			set
			{
				if ((_phone == null) || (value == null) || (!value.Equals(_phone)))
				{
                    object oldValue = _phone;
					_phone = value;
					RaisePropertyChanged(HR_Employee.Prop_Phone, oldValue, value);
				}
			}

		}

		[Property("Phone2", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Phone2
		{
			get { return _phone2; }
			set
			{
				if ((_phone2 == null) || (value == null) || (!value.Equals(_phone2)))
				{
                    object oldValue = _phone2;
					_phone2 = value;
					RaisePropertyChanged(HR_Employee.Prop_Phone2, oldValue, value);
				}
			}

		}

		[Property("HomePhone", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string HomePhone
		{
			get { return _homePhone; }
			set
			{
				if ((_homePhone == null) || (value == null) || (!value.Equals(_homePhone)))
				{
                    object oldValue = _homePhone;
					_homePhone = value;
					RaisePropertyChanged(HR_Employee.Prop_HomePhone, oldValue, value);
				}
			}

		}

		[Property("Mobile", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Mobile
		{
			get { return _mobile; }
			set
			{
				if ((_mobile == null) || (value == null) || (!value.Equals(_mobile)))
				{
                    object oldValue = _mobile;
					_mobile = value;
					RaisePropertyChanged(HR_Employee.Prop_Mobile, oldValue, value);
				}
			}

		}

		[Property("FileCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string FileCode
		{
			get { return _fileCode; }
			set
			{
				if ((_fileCode == null) || (value == null) || (!value.Equals(_fileCode)))
				{
                    object oldValue = _fileCode;
					_fileCode = value;
					RaisePropertyChanged(HR_Employee.Prop_FileCode, oldValue, value);
				}
			}

		}

		[Property("AttendanceType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string AttendanceType
		{
			get { return _attendanceType; }
			set
			{
				if ((_attendanceType == null) || (value == null) || (!value.Equals(_attendanceType)))
				{
                    object oldValue = _attendanceType;
					_attendanceType = value;
					RaisePropertyChanged(HR_Employee.Prop_AttendanceType, oldValue, value);
				}
			}

		}

		[Property("Photo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string Photo
		{
			get { return _photo; }
			set
			{
				if ((_photo == null) || (value == null) || (!value.Equals(_photo)))
				{
                    object oldValue = _photo;
					_photo = value;
					RaisePropertyChanged(HR_Employee.Prop_Photo, oldValue, value);
				}
			}

		}

		[Property("Region", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Region
		{
			get { return _region; }
			set
			{
				if ((_region == null) || (value == null) || (!value.Equals(_region)))
				{
                    object oldValue = _region;
					_region = value;
					RaisePropertyChanged(HR_Employee.Prop_Region, oldValue, value);
				}
			}

		}

		[Property("Post", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Post
		{
			get { return _post; }
			set
			{
				if ((_post == null) || (value == null) || (!value.Equals(_post)))
				{
                    object oldValue = _post;
					_post = value;
					RaisePropertyChanged(HR_Employee.Prop_Post, oldValue, value);
				}
			}

		}

		[Property("NativePlace", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string NativePlace
		{
			get { return _nativePlace; }
			set
			{
				if ((_nativePlace == null) || (value == null) || (!value.Equals(_nativePlace)))
				{
                    object oldValue = _nativePlace;
					_nativePlace = value;
					RaisePropertyChanged(HR_Employee.Prop_NativePlace, oldValue, value);
				}
			}

		}

		[Property("ContactAddress", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string ContactAddress
		{
			get { return _contactAddress; }
			set
			{
				if ((_contactAddress == null) || (value == null) || (!value.Equals(_contactAddress)))
				{
                    object oldValue = _contactAddress;
					_contactAddress = value;
					RaisePropertyChanged(HR_Employee.Prop_ContactAddress, oldValue, value);
				}
			}

		}

		[Property("HouseholdAddress", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string HouseholdAddress
		{
			get { return _householdAddress; }
			set
			{
				if ((_householdAddress == null) || (value == null) || (!value.Equals(_householdAddress)))
				{
                    object oldValue = _householdAddress;
					_householdAddress = value;
					RaisePropertyChanged(HR_Employee.Prop_HouseholdAddress, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_SupervisorId, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_SupervisorName, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_DepartmentId, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_DepartmentName, oldValue, value);
				}
			}

		}

		[Property("MajorCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string MajorCode
		{
			get { return _majorCode; }
			set
			{
				if ((_majorCode == null) || (value == null) || (!value.Equals(_majorCode)))
				{
                    object oldValue = _majorCode;
					_majorCode = value;
					RaisePropertyChanged(HR_Employee.Prop_MajorCode, oldValue, value);
				}
			}

		}

		[Property("MajorName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string MajorName
		{
			get { return _majorName; }
			set
			{
				if ((_majorName == null) || (value == null) || (!value.Equals(_majorName)))
				{
                    object oldValue = _majorName;
					_majorName = value;
					RaisePropertyChanged(HR_Employee.Prop_MajorName, oldValue, value);
				}
			}

		}

		[Property("ContractStartDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? ContractStartDate
		{
			get { return _contractStartDate; }
			set
			{
				if (value != _contractStartDate)
				{
                    object oldValue = _contractStartDate;
					_contractStartDate = value;
					RaisePropertyChanged(HR_Employee.Prop_ContractStartDate, oldValue, value);
				}
			}

		}

		[Property("ContractEndDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? ContractEndDate
		{
			get { return _contractEndDate; }
			set
			{
				if (value != _contractEndDate)
				{
                    object oldValue = _contractEndDate;
					_contractEndDate = value;
					RaisePropertyChanged(HR_Employee.Prop_ContractEndDate, oldValue, value);
				}
			}

		}

		[Property("ProbationStartDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? ProbationStartDate
		{
			get { return _probationStartDate; }
			set
			{
				if (value != _probationStartDate)
				{
                    object oldValue = _probationStartDate;
					_probationStartDate = value;
					RaisePropertyChanged(HR_Employee.Prop_ProbationStartDate, oldValue, value);
				}
			}

		}

		[Property("ProbationEndDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? ProbationEndDate
		{
			get { return _probationEndDate; }
			set
			{
				if (value != _probationEndDate)
				{
                    object oldValue = _probationEndDate;
					_probationEndDate = value;
					RaisePropertyChanged(HR_Employee.Prop_ProbationEndDate, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_LastModifiedDate, oldValue, value);
				}
			}

		}

		[Property("RegularizedDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? RegularizedDate
		{
			get { return _regularizedDate; }
			set
			{
				if (value != _regularizedDate)
				{
                    object oldValue = _regularizedDate;
					_regularizedDate = value;
					RaisePropertyChanged(HR_Employee.Prop_RegularizedDate, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_InauguralDate, oldValue, value);
				}
			}

		}

		[Property("IDPhotoFront", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string IDPhotoFront
		{
			get { return _iDPhotoFront; }
			set
			{
				if ((_iDPhotoFront == null) || (value == null) || (!value.Equals(_iDPhotoFront)))
				{
                    object oldValue = _iDPhotoFront;
					_iDPhotoFront = value;
					RaisePropertyChanged(HR_Employee.Prop_IDPhotoFront, oldValue, value);
				}
			}

		}

		[Property("IDPhotoBack", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string IDPhotoBack
		{
			get { return _iDPhotoBack; }
			set
			{
				if ((_iDPhotoBack == null) || (value == null) || (!value.Equals(_iDPhotoBack)))
				{
                    object oldValue = _iDPhotoBack;
					_iDPhotoBack = value;
					RaisePropertyChanged(HR_Employee.Prop_IDPhotoBack, oldValue, value);
				}
			}

		}

		[Property("Signature", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string Signature
		{
			get { return _signature; }
			set
			{
				if ((_signature == null) || (value == null) || (!value.Equals(_signature)))
				{
                    object oldValue = _signature;
					_signature = value;
					RaisePropertyChanged(HR_Employee.Prop_Signature, oldValue, value);
				}
			}

		}

		[Property("PoliticalStatus", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string PoliticalStatus
		{
			get { return _politicalStatus; }
			set
			{
				if ((_politicalStatus == null) || (value == null) || (!value.Equals(_politicalStatus)))
				{
                    object oldValue = _politicalStatus;
					_politicalStatus = value;
					RaisePropertyChanged(HR_Employee.Prop_PoliticalStatus, oldValue, value);
				}
			}

		}

		[Property("MarriageStatus", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string MarriageStatus
		{
			get { return _marriageStatus; }
			set
			{
				if ((_marriageStatus == null) || (value == null) || (!value.Equals(_marriageStatus)))
				{
                    object oldValue = _marriageStatus;
					_marriageStatus = value;
					RaisePropertyChanged(HR_Employee.Prop_MarriageStatus, oldValue, value);
				}
			}

		}

		[Property("SpouseName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string SpouseName
		{
			get { return _spouseName; }
			set
			{
				if ((_spouseName == null) || (value == null) || (!value.Equals(_spouseName)))
				{
                    object oldValue = _spouseName;
					_spouseName = value;
					RaisePropertyChanged(HR_Employee.Prop_SpouseName, oldValue, value);
				}
			}

		}

		[Property("Memo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 5000)]
		public virtual string Memo
		{
			get { return _memo; }
			set
			{
				if ((_memo == null) || (value == null) || (!value.Equals(_memo)))
				{
                    object oldValue = _memo;
					_memo = value;
					RaisePropertyChanged(HR_Employee.Prop_Memo, oldValue, value);
				}
			}

		}

		[Property("DutyForShow", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string DutyForShow
		{
			get { return _dutyForShow; }
			set
			{
				if ((_dutyForShow == null) || (value == null) || (!value.Equals(_dutyForShow)))
				{
                    object oldValue = _dutyForShow;
					_dutyForShow = value;
					RaisePropertyChanged(HR_Employee.Prop_DutyForShow, oldValue, value);
				}
			}

		}

		[Property("PostQualification", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string PostQualification
		{
			get { return _postQualification; }
			set
			{
				if ((_postQualification == null) || (value == null) || (!value.Equals(_postQualification)))
				{
                    object oldValue = _postQualification;
					_postQualification = value;
					RaisePropertyChanged(HR_Employee.Prop_PostQualification, oldValue, value);
				}
			}

		}

		[Property("School", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string School
		{
			get { return _school; }
			set
			{
				if ((_school == null) || (value == null) || (!value.Equals(_school)))
				{
                    object oldValue = _school;
					_school = value;
					RaisePropertyChanged(HR_Employee.Prop_School, oldValue, value);
				}
			}

		}

		[Property("SchoolMajor", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string SchoolMajor
		{
			get { return _schoolMajor; }
			set
			{
				if ((_schoolMajor == null) || (value == null) || (!value.Equals(_schoolMajor)))
				{
                    object oldValue = _schoolMajor;
					_schoolMajor = value;
					RaisePropertyChanged(HR_Employee.Prop_SchoolMajor, oldValue, value);
				}
			}

		}

		[Property("GraduatedDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? GraduatedDate
		{
			get { return _graduatedDate; }
			set
			{
				if (value != _graduatedDate)
				{
                    object oldValue = _graduatedDate;
					_graduatedDate = value;
					RaisePropertyChanged(HR_Employee.Prop_GraduatedDate, oldValue, value);
				}
			}

		}

		[Property("EducationHistory", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string EducationHistory
		{
			get { return _educationHistory; }
			set
			{
				if ((_educationHistory == null) || (value == null) || (!value.Equals(_educationHistory)))
				{
                    object oldValue = _educationHistory;
					_educationHistory = value;
					RaisePropertyChanged(HR_Employee.Prop_EducationHistory, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_Title, oldValue, value);
				}
			}

		}

		[Property("OrigDepartment", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string OrigDepartment
		{
			get { return _origDepartment; }
			set
			{
				if ((_origDepartment == null) || (value == null) || (!value.Equals(_origDepartment)))
				{
                    object oldValue = _origDepartment;
					_origDepartment = value;
					RaisePropertyChanged(HR_Employee.Prop_OrigDepartment, oldValue, value);
				}
			}

		}

		[Property("InnerTitle", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string InnerTitle
		{
			get { return _innerTitle; }
			set
			{
				if ((_innerTitle == null) || (value == null) || (!value.Equals(_innerTitle)))
				{
                    object oldValue = _innerTitle;
					_innerTitle = value;
					RaisePropertyChanged(HR_Employee.Prop_InnerTitle, oldValue, value);
				}
			}

		}

		[Property("TrailPay", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual System.Decimal? TrailPay
		{
			get { return _trailPay; }
			set
			{
				if (value != _trailPay)
				{
                    object oldValue = _trailPay;
					_trailPay = value;
					RaisePropertyChanged(HR_Employee.Prop_TrailPay, oldValue, value);
				}
			}

		}

		[Property("FormalPay", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual System.Decimal? FormalPay
		{
			get { return _formalPay; }
			set
			{
				if (value != _formalPay)
				{
                    object oldValue = _formalPay;
					_formalPay = value;
					RaisePropertyChanged(HR_Employee.Prop_FormalPay, oldValue, value);
				}
			}

		}

		[Property("PayAdjustment", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string PayAdjustment
		{
			get { return _payAdjustment; }
			set
			{
				if ((_payAdjustment == null) || (value == null) || (!value.Equals(_payAdjustment)))
				{
                    object oldValue = _payAdjustment;
					_payAdjustment = value;
					RaisePropertyChanged(HR_Employee.Prop_PayAdjustment, oldValue, value);
				}
			}

		}

		[Property("GeneralSafety", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string GeneralSafety
		{
			get { return _generalSafety; }
			set
			{
				if ((_generalSafety == null) || (value == null) || (!value.Equals(_generalSafety)))
				{
                    object oldValue = _generalSafety;
					_generalSafety = value;
					RaisePropertyChanged(HR_Employee.Prop_GeneralSafety, oldValue, value);
				}
			}

		}

		[Property("Safety", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Safety
		{
			get { return _safety; }
			set
			{
				if ((_safety == null) || (value == null) || (!value.Equals(_safety)))
				{
                    object oldValue = _safety;
					_safety = value;
					RaisePropertyChanged(HR_Employee.Prop_Safety, oldValue, value);
				}
			}

		}

		[Property("SafetyCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string SafetyCode
		{
			get { return _safetyCode; }
			set
			{
				if ((_safetyCode == null) || (value == null) || (!value.Equals(_safetyCode)))
				{
                    object oldValue = _safetyCode;
					_safetyCode = value;
					RaisePropertyChanged(HR_Employee.Prop_SafetyCode, oldValue, value);
				}
			}

		}

		[Property("Fund", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Fund
		{
			get { return _fund; }
			set
			{
				if ((_fund == null) || (value == null) || (!value.Equals(_fund)))
				{
                    object oldValue = _fund;
					_fund = value;
					RaisePropertyChanged(HR_Employee.Prop_Fund, oldValue, value);
				}
			}

		}

		[Property("FundCode", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string FundCode
		{
			get { return _fundCode; }
			set
			{
				if ((_fundCode == null) || (value == null) || (!value.Equals(_fundCode)))
				{
                    object oldValue = _fundCode;
					_fundCode = value;
					RaisePropertyChanged(HR_Employee.Prop_FundCode, oldValue, value);
				}
			}

		}

		[Property("ManualLabor", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string ManualLabor
		{
			get { return _manualLabor; }
			set
			{
				if ((_manualLabor == null) || (value == null) || (!value.Equals(_manualLabor)))
				{
                    object oldValue = _manualLabor;
					_manualLabor = value;
					RaisePropertyChanged(HR_Employee.Prop_ManualLabor, oldValue, value);
				}
			}

		}

		[Property("ResidenceValid", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string ResidenceValid
		{
			get { return _residenceValid; }
			set
			{
				if ((_residenceValid == null) || (value == null) || (!value.Equals(_residenceValid)))
				{
                    object oldValue = _residenceValid;
					_residenceValid = value;
					RaisePropertyChanged(HR_Employee.Prop_ResidenceValid, oldValue, value);
				}
			}

		}

		[Property("HireStatus", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string HireStatus
		{
			get { return _hireStatus; }
			set
			{
				if ((_hireStatus == null) || (value == null) || (!value.Equals(_hireStatus)))
				{
                    object oldValue = _hireStatus;
					_hireStatus = value;
					RaisePropertyChanged(HR_Employee.Prop_HireStatus, oldValue, value);
				}
			}

		}

		[Property("Tag", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1500)]
		public virtual string Tag
		{
			get { return _tag; }
			set
			{
				if ((_tag == null) || (value == null) || (!value.Equals(_tag)))
				{
                    object oldValue = _tag;
					_tag = value;
					RaisePropertyChanged(HR_Employee.Prop_Tag, oldValue, value);
				}
			}

		}

		[Property("TagA", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string TagA
		{
			get { return _tagA; }
			set
			{
				if ((_tagA == null) || (value == null) || (!value.Equals(_tagA)))
				{
                    object oldValue = _tagA;
					_tagA = value;
					RaisePropertyChanged(HR_Employee.Prop_TagA, oldValue, value);
				}
			}

		}

		[Property("TagB", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string TagB
		{
			get { return _tagB; }
			set
			{
				if ((_tagB == null) || (value == null) || (!value.Equals(_tagB)))
				{
                    object oldValue = _tagB;
					_tagB = value;
					RaisePropertyChanged(HR_Employee.Prop_TagB, oldValue, value);
				}
			}

		}

		[Property("TagC", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string TagC
		{
			get { return _tagC; }
			set
			{
				if ((_tagC == null) || (value == null) || (!value.Equals(_tagC)))
				{
                    object oldValue = _tagC;
					_tagC = value;
					RaisePropertyChanged(HR_Employee.Prop_TagC, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_CreatedDate, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_CreatorId, oldValue, value);
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
					RaisePropertyChanged(HR_Employee.Prop_CreatorName, oldValue, value);
				}
			}

		}

		#endregion
	} // HR_Employee
}

