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
	
namespace PIC.Biz.Model
{
	[ActiveRecord("DocDirectory")]
	public partial class DocDirectory : ModelBase<DocDirectory>
	{
		#region Property_Names

		public static string Prop_DirectoryID = "DirectoryID";
		public static string Prop_Module = "Module";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Status = "Status";
		public static string Prop_EditStatus = "EditStatus";
		public static string Prop_Tag = "Tag";
		public static string Prop_Description = "Description";
		public static string Prop_Parent = "Parent";
		public static string Prop_Path = "Path";
		public static string Prop_PathLevel = "PathLevel";
		public static string Prop_IsLeaf = "IsLeaf";
		public static string Prop_SortIndex = "SortIndex";
		public static string Prop_OwnerID = "OwnerID";
		public static string Prop_OwnerName = "OwnerName";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreatedDate = "CreatedDate";

		#endregion

		#region Private_Variables

		private string _directoryid;
		private DocModule _module;
		private string _code;
		private string _name;
		private string _status;
		private string _editStatus;
		private string _tag;
		private string _description;
		private DocDirectory _parent;
		private string _path;
		private int? _pathLevel;
		private bool? _isLeaf;
		private int? _sortIndex;
		private string _ownerID;
		private string _ownerName;
		private DateTime? _lastModifiedDate;
		private DateTime? _createdDate;

		private IList<DocDirectory> _DocDirectory = new List<DocDirectory>();
		private IList<DocFile> _DocFile = new List<DocFile>();

		#endregion

		#region Constructors

		public DocDirectory()
		{
		}

		public DocDirectory(
			string p_directoryid,
			DocModule p_module,
			string p_code,
			string p_name,
			string p_status,
			string p_editStatus,
			string p_tag,
			string p_description,
			DocDirectory p_parent,
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
			_module = p_module;
			_code = p_code;
			_name = p_name;
			_status = p_status;
			_editStatus = p_editStatus;
			_tag = p_tag;
			_description = p_description;
			_parent = p_parent;
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

		[PrimaryKey("DirectoryID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string DirectoryID
		{
			get { return _directoryid; }
			private set { _directoryid = value; }

		}

		[BelongsTo("ModuleID", Type = typeof(Module), Cascade = CascadeEnum.All, Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DocModule Module
		{
			get { return _module; }
			set
			{
				if ((_module == null) || (value == null) || (value.ModuleID != _module.ModuleID))
				{
                    object oldValue = _module;
					if (value == null)
						_module = null;
					else
						_module = (value.ModuleID > 0) ? value : null;
					RaisePropertyChanged(DocDirectory.Prop_Module, oldValue, value);
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
					RaisePropertyChanged(DocDirectory.Prop_Description, oldValue, value);
				}
			}

		}

		[BelongsTo("ParentID", Type = typeof(Parent), Cascade = CascadeEnum.All, Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DocDirectory Parent
		{
			get { return _parent; }
			set
			{
				if ((_parent == null) || (value == null) || (value.DirectoryID != _parent.DirectoryID))
				{
                    object oldValue = _parent;
					if (value == null)
						_parent = null;
					else
						_parent = (value.DirectoryID > 0) ? value : null;
					RaisePropertyChanged(DocDirectory.Prop_Parent, oldValue, value);
				}
			}

		}

		[Property("Path", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Path
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
		public int? PathLevel
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

		[Property("IsLeaf", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public bool? IsLeaf
		{
			get { return _isLeaf; }
			set
			{
				if (value != _isLeaf)
				{
                    object oldValue = _isLeaf;
					_isLeaf = value;
					RaisePropertyChanged(DocDirectory.Prop_IsLeaf, oldValue, value);
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

		[JsonIgnore]
		[HasMany(typeof(DocDirectory), Table="DocDirectory", ColumnKey="ParentID", Cascade = ManyRelationCascadeEnum.All, Lazy = true)]
		public IList DocDirectory
		{
			get { return _DocDirectory; }
			set { _DocDirectory = value; }
		}
		
		[JsonIgnore]
		[HasMany(typeof(DocFile), Table="DocFile", ColumnKey="DirectoryID", Cascade = ManyRelationCascadeEnum.All, Lazy = true)]
		public IList DocFile
		{
			get { return _DocFile; }
			set { _DocFile = value; }
		}
		
		#endregion
	} // DocDirectory
}

