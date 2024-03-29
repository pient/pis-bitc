﻿// Business class OrgRoleAuth generated from OrgRoleAuth
// Creator: Ray
// Created Date: [2013-06-15]

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
    [ActiveRecord("OrgRoleAuth")]
	public partial class OrgRoleAuth : ModelBase<OrgRoleAuth>
	{
		#region Property_Names

		public static string Prop_RoleAuthID = "RoleAuthID";
        public static string Prop_RoleID = "RoleID";
        public static string Prop_AuthID = "AuthID";
		public static string Prop_Tag = "Tag";
		public static string Prop_Description = "Description";
		public static string Prop_Status = "Status";
		public static string Prop_EditStatus = "EditStatus";
		public static string Prop_CreatorID = "CreatorID";
		public static string Prop_CreatorName = "CreatorName";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreatedDate = "CreatedDate";

		#endregion

		#region Private_Variables

		private string _roleauthid;
        private string _roleID;
        private string _authID;
		private string _tag;
		private string _description;
		private byte? _status;
		private string _editStatus;
		private string _creatorID;
		private string _creatorName;
		private DateTime? _lastModifiedDate;
		private DateTime? _createdDate;


		#endregion

		#region Constructors

		public OrgRoleAuth()
		{
		}

		public OrgRoleAuth(
			string p_roleauthid,
            string p_roleID,
            string p_authID,
			string p_tag,
			string p_description,
			byte? p_status,
			string p_editStatus,
			string p_creatorID,
			string p_creatorName,
			DateTime? p_lastModifiedDate,
			DateTime? p_createdDate)
		{
			_roleauthid = p_roleauthid;
            _roleID = p_roleID;
            _authID = p_authID;
			_tag = p_tag;
			_description = p_description;
			_status = p_status;
			_editStatus = p_editStatus;
			_creatorID = p_creatorID;
			_creatorName = p_creatorName;
			_lastModifiedDate = p_lastModifiedDate;
			_createdDate = p_createdDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("RoleAuthID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string RoleAuthID
		{
			get { return _roleauthid; }
			set { _roleauthid = value; }

		}

        [Property("RoleID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 36)]
        public string RoleID
        {
            get { return _roleID; }
            set
            {
                if ((_roleID == null) || (value == null) || (!value.Equals(_roleID)))
                {
                    object oldValue = _roleID;
                    _roleID = value;
                    RaisePropertyChanged(OrgRoleAuth.Prop_RoleID, oldValue, value);
                }
            }
        }

        [Property("AuthID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 36)]
        public string AuthID
        {
            get { return _authID; }
            set
            {
                if ((_authID == null) || (value == null) || (!value.Equals(_authID)))
                {
                    object oldValue = _authID;
                    _authID = value;
                    RaisePropertyChanged(OrgRoleAuth.Prop_AuthID, oldValue, value);
                }
            }
        }

		[Property("Tag", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Tag
		{
			get { return _tag; }
			set
			{
				if ((_tag == null) || (value == null) || (!value.Equals(_tag)))
				{
                    object oldValue = _tag;
					_tag = value;
					RaisePropertyChanged(OrgRoleAuth.Prop_Tag, oldValue, value);
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
					RaisePropertyChanged(OrgRoleAuth.Prop_Description, oldValue, value);
				}
			}

		}

		[Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public byte? Status
		{
			get { return _status; }
			set
			{
				if (value != _status)
				{
                    object oldValue = _status;
					_status = value;
					RaisePropertyChanged(OrgRoleAuth.Prop_Status, oldValue, value);
				}
			}

		}

		[Property("EditStatus", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string EditStatus
		{
			get { return _editStatus; }
			set
			{
				if ((_editStatus == null) || (value == null) || (!value.Equals(_editStatus)))
				{
                    object oldValue = _editStatus;
					_editStatus = value;
					RaisePropertyChanged(OrgRoleAuth.Prop_EditStatus, oldValue, value);
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
					RaisePropertyChanged(OrgRoleAuth.Prop_CreatorID, oldValue, value);
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
					RaisePropertyChanged(OrgRoleAuth.Prop_CreatorName, oldValue, value);
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
					RaisePropertyChanged(OrgRoleAuth.Prop_LastModifiedDate, oldValue, value);
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
					RaisePropertyChanged(OrgRoleAuth.Prop_CreatedDate, oldValue, value);
				}
			}

		}

		#endregion
    } // OrgRoleAuth
}

