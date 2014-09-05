// Business class DocFile generated from DocFile
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
	[ActiveRecord("DocFile")]
    public partial class DocFile : DocModelBase<DocFile>
	{
		#region Property_Names

		public static string Prop_FileID = "FileID";
        public static string Prop_DataID = "DataID";
        public static string Prop_ModuleID = "ModuleID";
        public static string Prop_GroupID = "GroupID";
        public static string Prop_DirectoryID = "DirectoryID";
		public static string Prop_VersionGroupID = "VersionGroupID";
		public static string Prop_Name = "Name";
		public static string Prop_Tag = "Tag";
		public static string Prop_Description = "Description";
		public static string Prop_ExtName = "ExtName";
		public static string Prop_Size = "Size";
		public static string Prop_OwnerID = "OwnerID";
		public static string Prop_OwnerName = "OwnerName";
		public static string Prop_CreatedDate = "CreatedDate";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_Version = "Version";

		#endregion

		#region Private_Variables

        private Guid? _fileid;
        private Guid? _moduleID;
        private Guid? _dataID;
        private Guid? _directoryID;
        private Guid? _groupID;
        private Guid? _versionGroupID;
		private string _name;
		private string _tag;
		private string _description;
		private string _extName;
		private long? _size;
		private string _ownerID;
		private string _ownerName;
		private DateTime? _createdDate;
		private DateTime? _lastModifiedDate;
		private string _version;


		#endregion

		#region Constructors

		public DocFile()
		{
		}

		public DocFile(
            Guid? p_fileid,
            Guid? p_dataID,
            Guid? p_directoryID,
            Guid? p_groupID,
            Guid? p_versionGroupID,
			string p_name,
			string p_tag,
			string p_description,
			string p_extName,
			long? p_size,
			string p_ownerID,
			string p_ownerName,
			DateTime? p_createdDate,
			DateTime? p_lastModifiedDate,
			string p_version)
		{
			_fileid = p_fileid;
            _dataID = p_dataID;
            _directoryID = p_directoryID;
			_groupID = p_groupID;
			_versionGroupID = p_versionGroupID;
			_name = p_name;
			_tag = p_tag;
			_description = p_description;
			_extName = p_extName;
			_size = p_size;
			_ownerID = p_ownerID;
			_ownerName = p_ownerName;
			_createdDate = p_createdDate;
			_lastModifiedDate = p_lastModifiedDate;
			_version = p_version;
		}

		#endregion

		#region Properties

		[PrimaryKey("FileID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(SequentialGuidGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
        public Guid? FileID
		{
			get { return _fileid; }
			set { _fileid = value; }

		}

		[Property("DataID", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public Guid? DataID
		{
			get { return _dataID; }
			set
			{
				if ((_dataID == null) || (value == null) || (!value.Equals(_dataID)))
				{
                    object oldValue = _dataID;
					_dataID = value;
					RaisePropertyChanged(DocFile.Prop_DataID, oldValue, value);
				}
			}

		}

        [Property("ModuleID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
        public Guid? ModuleID
        {
            get { return _moduleID; }
            set
            {
                if ((_moduleID == null) || (value == null) || (!value.Equals(_moduleID)))
                {
                    object oldValue = _moduleID;
                    _moduleID = value;
                    RaisePropertyChanged(DocFile.Prop_ModuleID, oldValue, value);
                }
            }

        }

        [Property("DirectoryID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
        public Guid? DirectoryID
        {
            get { return _directoryID; }
            set
            {
                if ((_directoryID == null) || (value == null) || (!value.Equals(_directoryID)))
                {
                    object oldValue = _directoryID;
                    _directoryID = value;
                    RaisePropertyChanged(DocFile.Prop_DirectoryID, oldValue, value);
                }
            }

        }

		[Property("GroupID", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public Guid? GroupID
		{
			get { return _groupID; }
			set
			{
				if ((_groupID == null) || (value == null) || (!value.Equals(_groupID)))
				{
                    object oldValue = _groupID;
					_groupID = value;
					RaisePropertyChanged(DocFile.Prop_GroupID, oldValue, value);
				}
			}

		}

		[Property("VersionGroupID", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public Guid? VersionGroupID
		{
			get { return _versionGroupID; }
			set
			{
				if ((_versionGroupID == null) || (value == null) || (!value.Equals(_versionGroupID)))
				{
                    object oldValue = _versionGroupID;
					_versionGroupID = value;
					RaisePropertyChanged(DocFile.Prop_VersionGroupID, oldValue, value);
				}
			}

		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(DocFile.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Tag", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1500)]
		public string Tag
		{
			get { return _tag; }
			set
			{
				if ((_tag == null) || (value == null) || (!value.Equals(_tag)))
				{
                    object oldValue = _tag;
					_tag = value;
					RaisePropertyChanged(DocFile.Prop_Tag, oldValue, value);
				}
			}

		}

		[Property("Description", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1500)]
		public string Description
		{
			get { return _description; }
			set
			{
				if ((_description == null) || (value == null) || (!value.Equals(_description)))
				{
                    object oldValue = _description;
					_description = value;
					RaisePropertyChanged(DocFile.Prop_Description, oldValue, value);
				}
			}

		}

		[Property("ExtName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ExtName
		{
			get { return _extName; }
			set
			{
				if ((_extName == null) || (value == null) || (!value.Equals(_extName)))
				{
                    object oldValue = _extName;
					_extName = value;
					RaisePropertyChanged(DocFile.Prop_ExtName, oldValue, value);
				}
			}

		}

		[Property("Size", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public long? Size
		{
			get { return _size; }
			set
			{
				if (value != _size)
				{
                    object oldValue = _size;
					_size = value;
					RaisePropertyChanged(DocFile.Prop_Size, oldValue, value);
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
					RaisePropertyChanged(DocFile.Prop_OwnerID, oldValue, value);
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
					RaisePropertyChanged(DocFile.Prop_OwnerName, oldValue, value);
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
					RaisePropertyChanged(DocFile.Prop_CreatedDate, oldValue, value);
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
					RaisePropertyChanged(DocFile.Prop_LastModifiedDate, oldValue, value);
				}
			}

		}

		[Property("Version", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Version
		{
			get { return _version; }
			set
			{
				if ((_version == null) || (value == null) || (!value.Equals(_version)))
				{
                    object oldValue = _version;
					_version = value;
					RaisePropertyChanged(DocFile.Prop_Version, oldValue, value);
				}
			}

		}

		#endregion
	} // DocFile
}

