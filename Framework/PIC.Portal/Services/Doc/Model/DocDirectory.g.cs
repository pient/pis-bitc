// Business class DocDirectory generated from DocDirectory
// Creator: Ray
// Created Date: [2013-06-23]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
using PIC.Portal;
	
namespace PIC.Doc.Model
{
	[ActiveRecord("DocDirectory")]
    public partial class DocDirectory : DocTreeNodeModelBase<DocDirectory>
	{
		#region Property_Names

		public static string Prop_DirectoryID = "DirectoryID";
        public static string Prop_ModuleID = "ModuleID";
		public static string Prop_Code = "Code";
        public static string Prop_Name = "Name";
        public static string Prop_Status = "Status";
        public static string Prop_EditStatus = "EditStatus";
        public static string Prop_Description = "Description";
        public static string Prop_Tag = "Tag";
        public new static string Prop_ParentID = "ParentID";
        public new static string Prop_Path = "Path";
        public new static string Prop_PathLevel = "PathLevel";
        public new static string Prop_IsLeaf = "IsLeaf";
        public new static string Prop_SortIndex = "SortIndex";
		public static string Prop_OwnerID = "OwnerID";
		public static string Prop_OwnerName = "OwnerName";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreatedDate = "CreatedDate";

		#endregion

		#region Private_Variables

        private Guid? _directoryid;
        private Guid? _moduleID;
		private string _code;
        private string _name;
        private string _status;
        private string _editStatus;
        private string _description;
        private Guid? _parentID;
		private string _tag;
		private string _path;
		private int? _pathLevel;
		private bool? _isLeaf;
		private int? _sortIndex;
		private string _ownerID;
		private string _ownerName;
		private DateTime? _lastModifiedDate;
		private DateTime? _createdDate;


		#endregion

		#region Constructors

		public DocDirectory()
		{
		}

		public DocDirectory(
            Guid? p_directoryid,
            Guid? p_moduleID,
			string p_code,
            string p_name,
            string p_status,
            string p_editStatus,
            string p_description,
            Guid? p_parentID,
			string p_tag,
			string p_path,
			int? p_pathLevel,
			bool? p_isLeaf,
			int? p_sortIndex,
			string p_ownerID,
			string p_ownerName,
			DateTime? p_lastModifiedDate,
			DateTime? p_createdDate)
		{
			_directoryid = p_directoryid;
            _moduleID = p_moduleID;
			_code = p_code;
			_name = p_name;
            _status = p_status;
            _editStatus = p_editStatus;
            _description = p_description;
			_parentID = p_parentID;
			_tag = p_tag;
			_path = p_path;
			_pathLevel = p_pathLevel;
			_isLeaf = p_isLeaf;
			_sortIndex = p_sortIndex;
			_ownerID = p_ownerID;
			_ownerName = p_ownerName;
			_lastModifiedDate = p_lastModifiedDate;
			_createdDate = p_createdDate;
		}

		#endregion

		#region Properties

        [JsonConverter(typeof(JsonGuidConverter))]
        [PrimaryKey("DirectoryID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(SequentialGuidGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
        public Guid? DirectoryID
		{
			get { return _directoryid; }
			set { _directoryid = value; }

		}

        [JsonConverter(typeof(JsonGuidConverter))]
        [Property("ModuleID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 50)]
        public Guid? ModuleID
        {
            get { return _moduleID; }
            set
            {
                if ((_moduleID == null) || (value == null) || (!value.Equals(_moduleID)))
                {
                    object oldValue = _moduleID;
                    _moduleID = value;
                    RaisePropertyChanged(DocDirectory.Prop_ModuleID, oldValue, value);
                }
            }

        }

		[Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 500)]
		public string Code
		{
			get { return _code; }
			set
			{
				if ((_code == null) || (value == null) || (!value.Equals(_code)))
				{
                    object oldValue = _code;
					_code = value;
					RaisePropertyChanged(DocDirectory.Prop_Code, oldValue, value);
				}
			}

		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 150)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(DocDirectory.Prop_Name, oldValue, value);
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
                    RaisePropertyChanged(DocDirectory.Prop_Status, oldValue, value);
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
                    RaisePropertyChanged(DocDirectory.Prop_EditStatus, oldValue, value);
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
                    object oldValue = _description;
                    _description = value;
                    RaisePropertyChanged(DocDirectory.Prop_Description, oldValue, value);
                }
            }
        }

        [JsonConverter(typeof(JsonGuidConverter))]
        [Property("ParentID", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public override Guid? ParentID
		{
			get { return _parentID; }
			set
			{
				if ((_parentID == null) || (value == null) || (!value.Equals(_parentID)))
				{
                    object oldValue = _parentID;
					_parentID = value;
					RaisePropertyChanged(DocDirectory.Prop_ParentID, oldValue, value);
				}
			}

		}

		[Property("Tag", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 5000)]
		public string Tag
		{
			get { return _tag; }
			set
			{
				if ((_tag == null) || (value == null) || (!value.Equals(_tag)))
				{
                    object oldValue = _tag;
					_tag = value;
					RaisePropertyChanged(DocDirectory.Prop_Tag, oldValue, value);
				}
			}

		}

		[Property("Path", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
        public override string Path
		{
			get { return _path; }
			set
			{
				if ((_path == null) || (value == null) || (!value.Equals(_path)))
				{
                    object oldValue = _path;
					_path = value;
					RaisePropertyChanged(DocDirectory.Prop_Path, oldValue, value);
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
                    object oldValue = _pathLevel;
					_pathLevel = value;
					RaisePropertyChanged(DocDirectory.Prop_PathLevel, oldValue, value);
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
                    object oldValue = _sortIndex;
					_sortIndex = value;
					RaisePropertyChanged(DocDirectory.Prop_SortIndex, oldValue, value);
				}
			}

		}

		[Property("OwnerID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string OwnerID
		{
			get { return _ownerID; }
			set
			{
				if ((_ownerID == null) || (value == null) || (!value.Equals(_ownerID)))
				{
                    object oldValue = _ownerID;
					_ownerID = value;
					RaisePropertyChanged(DocDirectory.Prop_OwnerID, oldValue, value);
				}
			}

		}

		[Property("OwnerName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string OwnerName
		{
			get { return _ownerName; }
			set
			{
				if ((_ownerName == null) || (value == null) || (!value.Equals(_ownerName)))
				{
                    object oldValue = _ownerName;
					_ownerName = value;
					RaisePropertyChanged(DocDirectory.Prop_OwnerName, oldValue, value);
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
					RaisePropertyChanged(DocDirectory.Prop_LastModifiedDate, oldValue, value);
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
					RaisePropertyChanged(DocDirectory.Prop_CreatedDate, oldValue, value);
				}
			}

		}

		#endregion
	} // DocDirectory
}

