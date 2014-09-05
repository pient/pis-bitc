// Business class OrgUser generated from OrgUser
// Creator: Ray
// Created Date: [2010-04-10]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
	
namespace PIC.Portal.Model
{
    [ActiveRecord("OrgUser")]
	public partial class OrgUser : EntityBase<OrgUser> , INotifyPropertyChanged 	
	{

		#region Property_Names

		public static string Prop_UserID = "UserID";
        public static string Prop_LoginName = "LoginName";
        public static string Prop_WorkNo = "WorkNo";
		public static string Prop_Password = "Password";
		public static string Prop_Name = "Name";
		public static string Prop_Email = "Email";
		public static string Prop_Remark = "Remark";
        public static string Prop_Status = "Status";
        public static string Prop_ReportToID = "ReportToID";
        public static string Prop_ReportToName = "ReportToName";
        public static string Prop_DeptID = "DeptID";
        public static string Prop_DeptName = "DeptName";
        public static string Prop_Tag = "Tag";
		public static string Prop_LastLogIP = "LastLogIP";
		public static string Prop_LastLogDate = "LastLogDate";
		public static string Prop_SortIndex = "SortIndex";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
        public static string Prop_CreateDate = "CreateDate";
        public static string Prop_IsDeleted = "IsDeleted";

		#endregion

		#region Private_Variables

		private string _userid;
        private string _loginName;
        private string _workNo;
		private string _password;
		private string _name;
		private string _email;
		private string _remark;
        private int? _status;
        private string _reportToID;
        private string _reportToName;
        private string _deptID;
        private string _deptName;
        private string _tag;
		private string _lastLogIP;
		private DateTime? _lastLogDate;
		private int? _sortIndex;
		private DateTime? _lastModifiedDate;
        private DateTime? _createDate;
        private bool? _isDeleted;

		#endregion

		#region Constructors

		public OrgUser()
		{
		}

		public OrgUser(
			string p_userid,
            string p_loginName,
            string p_workNo,
			string p_password,
			string p_name,
			string p_email,
			string p_remark,
			byte? p_Status,
			string p_lastLogIP,
			DateTime? p_lastLogDate,
			int? p_sortIndex,
			DateTime? p_lastModifiedDate,
			DateTime? p_createDate)
		{
			_userid = p_userid;
            _loginName = p_loginName;
            _workNo = p_workNo;
			_password = p_password;
			_name = p_name;
			_email = p_email;
			_remark = p_remark;
			_status = p_Status;
			_lastLogIP = p_lastLogIP;
			_lastLogDate = p_lastLogDate;
			_sortIndex = p_sortIndex;
			_lastModifiedDate = p_lastModifiedDate;
			_createDate = p_createDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("UserID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string UserID
		{
			get { return _userid; }
            set { _userid = value; }
		}

		[Property("LoginName", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 50)]
		public string LoginName
		{
			get { return _loginName; }
			set
			{
				if ((_loginName == null) || (value == null) || (!value.Equals(_loginName)))
				{
					_loginName = value;
					NotifyPropertyChanged(OrgUser.Prop_LoginName);
				}
			}
		}

		[Property("WorkNo", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 16)]
        public string WorkNo
		{
			get { return _workNo; }
			set
			{
                if ((_workNo == null) || (value == null) || (!value.Equals(_workNo)))
				{
                    _workNo = value;
                    NotifyPropertyChanged(OrgUser.Prop_WorkNo);
				}
			}
		}

        [JsonIgnore]
		[Property("Password", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 32)]
		public string Password
		{
			get { return _password; }
			set
			{
				if ((_password == null) || (value == null) || (!value.Equals(_password)))
				{
					_password = value;
					NotifyPropertyChanged(OrgUser.Prop_Password);
				}
			}
		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 50)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
					_name = value;
					NotifyPropertyChanged(OrgUser.Prop_Name);
				}
			}
		}

		[Property("Email", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 100)]
		public string Email
		{
			get { return _email; }
			set
			{
				if ((_email == null) || (value == null) || (!value.Equals(_email)))
				{
					_email = value;
					NotifyPropertyChanged(OrgUser.Prop_Email);
				}
			}
		}

		[Property("Remark", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Remark
		{
			get { return _remark; }
			set
			{
				if ((_remark == null) || (value == null) || (!value.Equals(_remark)))
				{
					_remark = value;
					NotifyPropertyChanged(OrgUser.Prop_Remark);
				}
			}
		}

		[Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? Status
		{
			get { return _status; }
			set
			{
				if (value != _status)
				{
					_status = value;
					NotifyPropertyChanged(OrgUser.Prop_Status);
				}
			}
		}

        [Property("ReportToID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 36)]
        public string ReportToID
        {
            get { return _reportToID; }
            set
            {
                if ((_reportToID == null) || (value == null) || (!value.Equals(_reportToID)))
                {
                    _reportToID = value;
                    NotifyPropertyChanged(OrgUser.Prop_ReportToID);
                }
            }
        }

        [Property("ReportToName", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 50)]
        public string ReportToName
        {
            get { return _reportToName; }
            set
            {
                if ((_reportToName == null) || (value == null) || (!value.Equals(_reportToName)))
                {
                    _reportToName = value;
                    NotifyPropertyChanged(OrgUser.Prop_ReportToName);
                }
            }
        }

        [Property("DeptID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 36)]
        public string DeptID
        {
            get { return _deptID; }
            set
            {
                if ((_deptID == null) || (value == null) || (!value.Equals(_deptID)))
                {
                    _deptID = value;
                    NotifyPropertyChanged(OrgUser.Prop_DeptID);
                }
            }
        }

        [Property("DeptName", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 50)]
        public string DeptName
        {
            get { return _deptName; }
            set
            {
                if ((_deptName == null) || (value == null) || (!value.Equals(_deptName)))
                {
                    _deptName = value;
                    NotifyPropertyChanged(OrgUser.Prop_DeptName);
                }
            }
        }

        [Property("Tag", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public string Tag
        {
            get { return _tag; }
            set
            {
                if ((_tag == null) || (value == null) || (!value.Equals(_tag)))
                {
                    _tag = value;
                    NotifyPropertyChanged(OrgUser.Prop_Tag);
                }
            }
        }

		[Property("LastLogIP", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string LastLogIP
		{
			get { return _lastLogIP; }
			set
			{
				if ((_lastLogIP == null) || (value == null) || (!value.Equals(_lastLogIP)))
				{
					_lastLogIP = value;
					NotifyPropertyChanged(OrgUser.Prop_LastLogIP);
				}
			}
		}

		[Property("LastLogDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? LastLogDate
		{
			get { return _lastLogDate; }
			set
			{
				if (value != _lastLogDate)
				{
					_lastLogDate = value;
					NotifyPropertyChanged(OrgUser.Prop_LastLogDate);
				}
			}
		}

		[Property("SortIndex", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? SortIndex
		{
			get { return _sortIndex; }
			set
			{
				if (value != _sortIndex)
				{
					_sortIndex = value;
					NotifyPropertyChanged(OrgUser.Prop_SortIndex);
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
					_lastModifiedDate = value;
					NotifyPropertyChanged(OrgUser.Prop_LastModifiedDate);
				}
			}
		}

		[Property("CreateDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? CreateDate
		{
			get { return _createDate; }
			set
			{
				if (value != _createDate)
				{
					_createDate = value;
					NotifyPropertyChanged(OrgUser.Prop_CreateDate);
				}
			}
        }

        [Property("IsDeleted", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public Boolean? IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                if (value != _isDeleted)
                {
                    _isDeleted = value;
                    NotifyPropertyChanged(OrgUser.Prop_IsDeleted);
                }
            }
        }
		
		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			PropertyChangedEventHandler localPropertyChanged = PropertyChanged;
			if (localPropertyChanged != null)
			{
				localPropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}

		#endregion

    } // OrgUser
}

