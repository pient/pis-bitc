// Business class OrgGroup generated from OrgGroup
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
	[ActiveRecord("OrgGroup")]
    public partial class OrgGroup : TreeNodeEntityBase<OrgGroup>, INotifyPropertyChanged
	{
		#region Property_Names

		public static string Prop_GroupID = "GroupID";
        public static string Prop_Name = "Name";
        public static string Prop_Code = "Code";
        public static string Prop_Attr = "Attr";
        public static string Prop_PrincipalID = "PrincipalID";
        public static string Prop_PrincipalName = "PrincipalName";
        public new static string Prop_ParentID = "ParentID";
        public new static string Prop_Path = "Path";
        public new static string Prop_PathLevel = "PathLevel";
		public static string Prop_Type = "Type";
        public static string Prop_Status = "Status";
        public static string Prop_EditStatus = "EditStatus";
        public new static string Prop_SortIndex = "SortIndex";
        public static string Prop_Description = "Description";
        public static string Prop_Tag = "Tag";
        public static string Prop_ModifiedSortIndex = "ModifiedsortIndex";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreateDate = "CreateDate";

		#endregion

		#region Private_Variables

		private string _groupid;
        private string _name;
        private string _code;
        private string _attr;
        private string _principalID;
        private string _principalName;
        private string _parentID;
        private string _path;
        private int? _pathLevel;
		private int? _type;
        private byte? _status;
        private string _editStatus;
        private int? _sortIndex;
        private string _description;
        private string _tag;
        private int? _modifiedSortIndex;
		private DateTime? _lastModifiedDate;
		private DateTime? _createDate;

		#endregion

		#region Constructors

		public OrgGroup()
		{
		}

		public OrgGroup(
			string p_groupid,
			string p_name,
            string p_code,
            string p_attr,
            string p_parentID,
            string p_path,
            int? p_pathLevel,
			int? p_type,
			byte? p_status,
            int? p_sortIndex,
            string p_description,
            string p_tag,
            int? p_modifiedSortIndex,
			DateTime? p_lastModifiedDate,
			DateTime? p_createDate)
		{
			_groupid = p_groupid;
            _name = p_name;
            _code = p_code;
            _attr = p_attr;
            _parentID = p_parentID;
            _path = p_path;
            _pathLevel = p_pathLevel;
			_type = p_type;
			_status = p_status;
            _sortIndex = p_sortIndex;
            _description = p_description;
            _tag = p_tag;
			_lastModifiedDate = p_lastModifiedDate;
			_createDate = p_createDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("GroupID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string GroupID
		{
			get { return _groupid; }
            set { _groupid=value; }
        }

        [Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 30)]
        public string Code
        {
            get { return _code; }
            set
            {
                if ((_code == null) || (value == null) || (!value.Equals(_code)))
                {
                    _code = value;
                    NotifyPropertyChanged(OrgGroup.Prop_Code);
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
					NotifyPropertyChanged(OrgGroup.Prop_Name);
				}
			}
        }

        [Property("Attr", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string Attr
        {
            get { return _attr; }
            set
            {
                if ((_attr == null) || (value == null) || (!value.Equals(_attr)))
                {
                    _attr = value;
                    NotifyPropertyChanged(OrgGroup.Prop_Attr);
                }
            }
        }

        [Property("PrincipalID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
        public string PrincipalID
        {
            get { return _principalID; }
            set
            {
                if ((_principalID == null) || (value == null) || (!value.Equals(_principalID)))
                {
                    _principalID = value;
                    NotifyPropertyChanged(OrgGroup.Prop_PrincipalID);
                }
            }
        }

        [Property("PrincipalName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string PrincipalName
        {
            get { return _principalName; }
            set
            {
                if ((_principalName == null) || (value == null) || (!value.Equals(_principalName)))
                {
                    _principalName = value;
                    NotifyPropertyChanged(OrgGroup.Prop_PrincipalName);
                }
            }
        }

        [Property("ParentID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
        public override string ParentID
        {
            get { return _parentID; }
            set
            {
                if ((_parentID == null) || (value == null) || (!value.Equals(_parentID)))
                {
                    _parentID = value;
                    NotifyPropertyChanged(OrgGroup.Prop_ParentID);
                }
            }
        }

		[Property("Path", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1000)]
        public override string Path
		{
			get { return _path; }
			set
			{
				if ((_path == null) || (value == null) || (!value.Equals(_path)))
				{
					_path = value;
					NotifyPropertyChanged(OrgGroup.Prop_Path);
				}
			}
        }

        [Property("PathLevel", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public override int? PathLevel
        {
            get { return _pathLevel; }
            set
            {
                if (value != _pathLevel)
                {
                    _pathLevel = value;
                    NotifyPropertyChanged(Module.Prop_PathLevel);
                }
            }
        }

		[Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? Type
		{
			get { return _type; }
			set
			{
				if (value != _type)
				{
					_type = value;
					NotifyPropertyChanged(OrgGroup.Prop_Type);
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
					_status = value;
					NotifyPropertyChanged(OrgGroup.Prop_Status);
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
                    _editStatus = value;
                    NotifyPropertyChanged(OrgGroup.Prop_EditStatus);
                }
            }
        }

        [Property("Description", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 255)]
        public string Description
        {
            get { return _description; }
            set
            {
                if ((_description == null) || (value == null) || (!value.Equals(_description)))
                {
                    _description = value;
                    NotifyPropertyChanged(OrgGroup.Prop_Description);
                }
            }
        }

		[Property("SortIndex", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public override int? SortIndex
		{
			get { return _sortIndex; }
			set
			{
				if (value != _sortIndex)
				{
					_sortIndex = value;
					NotifyPropertyChanged(OrgGroup.Prop_SortIndex);
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
                    RaisePropertyChanged(OrgGroup.Prop_Tag, oldValue, value);
                }
            }

        }

        [Property("ModifiedSortIndex", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public int? ModifiedSortIndex
        {
            get { return _modifiedSortIndex; }
            set
            {
                if (value != _modifiedSortIndex)
                {
                    _modifiedSortIndex = value;
                    NotifyPropertyChanged(OrgGroup.Prop_ModifiedSortIndex);
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
					NotifyPropertyChanged(OrgGroup.Prop_LastModifiedDate);
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
					NotifyPropertyChanged(OrgGroup.Prop_CreateDate);
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

    } // OrgGroup
}

