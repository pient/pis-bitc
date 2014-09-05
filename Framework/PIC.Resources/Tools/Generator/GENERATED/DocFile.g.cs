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
	
namespace PIC.Biz.Model
{
	[ActiveRecord("DocFile")]
	public partial class DocFile : ModelBase<DocFile>
	{
		#region Property_Names

		public static string Prop_FileID = "FileID";
		public static string Prop_ModuleID = "ModuleID";
		public static string Prop_DataID = "DataID";
		public static string Prop_Directory = "Directory";
		public static string Prop_Group = "Group";
		public static string Prop_VersionGroup = "VersionGroup";
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

		private string _fileid;
		private string _moduleID;
		private string _dataID;
		private DocDirectory _directory;
		private DocFileGroup _group;
		private DocVersionGroup _versionGroup;
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

		private IList<DocFileDatum> _DocFileData = new List<DocFileDatum>();

		#endregion

		#region Constructors

		public DocFile()
		{
		}

		public DocFile(
			string p_fileid,
			string p_moduleID,
			string p_dataID,
			DocDirectory p_directory,
			DocFileGroup p_group,
			DocVersionGroup p_versionGroup,
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
			_moduleID = p_moduleID;
			_dataID = p_dataID;
			_directory = p_directory;
			_group = p_group;
			_versionGroup = p_versionGroup;
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

		[PrimaryKey("FileID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string FileID
		{
			get { return _fileid; }
			private set { _fileid = value; }

		}

		[Property("ModuleID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
		public string ModuleID
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

		[Property("DataID", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public string DataID
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

		[BelongsTo("DirectoryID", Type = typeof(Directory), Cascade = CascadeEnum.All, Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DocDirectory Directory
		{
			get { return _directory; }
			set
			{
				if ((_directory == null) || (value == null) || (value.DirectoryID != _directory.DirectoryID))
				{
                    object oldValue = _directory;
					if (value == null)
						_directory = null;
					else
						_directory = (value.DirectoryID > 0) ? value : null;
					RaisePropertyChanged(DocFile.Prop_Directory, oldValue, value);
				}
			}

		}

		[BelongsTo("GroupID", Type = typeof(Group), Cascade = CascadeEnum.All, Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DocFileGroup Group
		{
			get { return _group; }
			set
			{
				if ((_group == null) || (value == null) || (value.GroupID != _group.GroupID))
				{
                    object oldValue = _group;
					if (value == null)
						_group = null;
					else
						_group = (value.GroupID > 0) ? value : null;
					RaisePropertyChanged(DocFile.Prop_Group, oldValue, value);
				}
			}

		}

		[BelongsTo("VersionGroupID", Type = typeof(VersionGroup), Cascade = CascadeEnum.All, Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DocVersionGroup VersionGroup
		{
			get { return _versionGroup; }
			set
			{
				if ((_versionGroup == null) || (value == null) || (value.GroupID != _versionGroup.GroupID))
				{
                    object oldValue = _versionGroup;
					if (value == null)
						_versionGroup = null;
					else
						_versionGroup = (value.GroupID > 0) ? value : null;
					RaisePropertyChanged(DocFile.Prop_VersionGroup, oldValue, value);
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

		[JsonIgnore]
		[HasMany(typeof(DocFileDatum), Table="DocFileData", ColumnKey="FileID", Cascade = ManyRelationCascadeEnum.All, Lazy = true)]
		public IList DocFileData
		{
			get { return _DocFileData; }
			set { _DocFileData = value; }
		}
		
		#endregion
	} // DocFile
}

