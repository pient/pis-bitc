// Business class OrgUserGroup generated from OrgUserGroup
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
    [ActiveRecord("OrgUserGroup")]
	public partial class OrgUserGroup : ModelBase<OrgUserGroup>
	{
		#region Property_Names

		public static string Prop_UserGroupID = "UserGroupID";
        public static string Prop_UserID = "UserID";
        public static string Prop_GroupID = "GroupID";
		public static string Prop_RoleID = "RoleID";
		public static string Prop_Status = "Status";
		public static string Prop_EditStatus = "EditStatus";
        public static string Prop_Description = "Description";
        public static string Prop_Tag = "Tag";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreatedDate = "CreatedDate";

		#endregion

		#region Private_Variables

		private string _usergroupid;
        private string _userID;
        private string _groupID;
        private string _roleID;
		private byte? _status;
		private string _editStatus;
        private string _description;
        private string _tag;
		private DateTime? _lastModifiedDate;
		private DateTime? _createdDate;


		#endregion

		#region Constructors

		public OrgUserGroup()
		{
		}

		public OrgUserGroup(
            string p_usergroupid,
            string p_groupID,
            string p_userID,
            string p_roleID,
			byte? p_status,
			string p_editStatus,
            string p_description,
            string p_tag,
			DateTime? p_lastModifiedDate,
			DateTime? p_createdDate)
		{
			_usergroupid = p_usergroupid;
            _userID = p_userID;
            _groupID = p_groupID;
            _roleID = p_roleID;
			_status = p_status;
			_editStatus = p_editStatus;
            _description = p_description;
            _tag = p_tag;
			_lastModifiedDate = p_lastModifiedDate;
			_createdDate = p_createdDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("UserGroupID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
        public string UserGroupID
		{
			get { return _usergroupid; }
			set { _usergroupid = value; }

		}

        [Property("UserID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 36)]
        public string UserID
        {
            get { return _userID; }
            set
            {
                if ((_userID == null) || (value == null) || (!value.Equals(_userID)))
                {
                    object oldValue = _userID;
                    _userID = value;
                    RaisePropertyChanged(OrgUserGroup.Prop_UserID, oldValue, value);
                }
            }
        }

        [Property("GroupID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 36)]
        public string GroupID
        {
            get { return _groupID; }
            set
            {
                if ((_groupID == null) || (value == null) || (!value.Equals(_groupID)))
                {
                    object oldValue = _groupID;
                    _groupID = value;
                    RaisePropertyChanged(OrgUserGroup.Prop_GroupID, oldValue, value);
                }
            }
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
					RaisePropertyChanged(OrgUserGroup.Prop_RoleID, oldValue, value);
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
					RaisePropertyChanged(OrgUserGroup.Prop_Status, oldValue, value);
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
					RaisePropertyChanged(OrgUserGroup.Prop_EditStatus, oldValue, value);
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
					RaisePropertyChanged(OrgUserGroup.Prop_Description, oldValue, value);
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
                    RaisePropertyChanged(OrgUserAuth.Prop_Tag, oldValue, value);
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
					RaisePropertyChanged(OrgUserGroup.Prop_LastModifiedDate, oldValue, value);
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
					RaisePropertyChanged(OrgUserGroup.Prop_CreatedDate, oldValue, value);
				}
			}

		}

		#endregion
    } // OrgUserGroup
}

