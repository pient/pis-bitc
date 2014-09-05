// Business class SysParameterCatalog generated from SysParameterCatalog
// Creator: Ray
// Created Date: [2010-06-25]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
	
namespace PIC.Portal.Model
{
    [ActiveRecord("ParameterCatalog")]
	public partial class ParameterCatalog : EditSensitiveTreeNodeEntityBase<ParameterCatalog> , INotifyPropertyChanged 	
	{

		#region Property_Names

		public static string Prop_ParameterCatalogID = "ParameterCatalogID";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
        public static string Prop_EditStatus = "EditStatus";
        public new static string Prop_ParentID = "ParentID";
        public new static string Prop_Path = "Path";
        public new static string Prop_PathLevel = "PathLevel";
        public new static string Prop_IsLeaf = "IsLeaf";
        public new static string Prop_SortIndex = "SortIndex";
		public static string Prop_Description = "Description";
		public static string Prop_CreaterID = "CreaterID";
		public static string Prop_CreaterName = "CreaterName";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreatedDate = "CreatedDate";

		#endregion

		#region Private_Variables

		private string _parametercatalogid;
		private string _code;
		private string _name;
        private string _editStatus;
        private int? _sortIndex;
		private string _parentID;
		private string _path;
        private int? _pathLevel;
        private bool? _isLeaf;
		private string _description;
		private string _createrID;
		private string _createrName;
		private DateTime? _lastModifiedDate;
		private DateTime? _createdDate;


		#endregion

		#region Constructors

		public ParameterCatalog()
		{
		}

		public ParameterCatalog(
			string p_parametercatalogid,
			string p_code,
			string p_name,
			string p_editStatus,
			string p_parentID,
			string p_path,
            int? p_pathLevel,
            bool? p_isLeaf,
			string p_description,
			string p_createrID,
			string p_createrName,
			DateTime? p_lastModifiedDate,
			DateTime? p_createdDate)
		{
			_parametercatalogid = p_parametercatalogid;
			_code = p_code;
			_name = p_name;
			_editStatus = p_editStatus;
			_parentID = p_parentID;
			_path = p_path;
            _pathLevel = p_pathLevel;
            _isLeaf = p_isLeaf;
			_description = p_description;
			_createrID = p_createrID;
			_createrName = p_createrName;
			_lastModifiedDate = p_lastModifiedDate;
			_createdDate = p_createdDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("ParameterCatalogID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string ParameterCatalogID
		{
			get { return _parametercatalogid; }
		}

		[Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Code
		{
			get { return _code; }
			set
			{
				if ((_code == null) || (value == null) || (!value.Equals(_code)))
				{
					_code = value;
					NotifyPropertyChanged(ParameterCatalog.Prop_Code);
				}
			}
		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
					_name = value;
					NotifyPropertyChanged(ParameterCatalog.Prop_Name);
				}
			}
		}

		[Property("EditStatus", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public override string EditStatus
		{
			get { return _editStatus; }
			set
			{
				if ((_editStatus == null) || (value == null) || (!value.Equals(_editStatus)))
				{
					_editStatus = value;
					NotifyPropertyChanged(ParameterCatalog.Prop_EditStatus);
				}
			}
		}

		[Property("ParentID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public override string ParentID
		{
			get { return _parentID; }
			set
			{
				if ((_parentID == null) || (value == null) || (!value.Equals(_parentID)))
				{
					_parentID = value;
					NotifyPropertyChanged(Prop_ParentID);
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
					NotifyPropertyChanged(Prop_Path);
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
					NotifyPropertyChanged(Prop_PathLevel);
				}
			}
        }

        [Property("IsLeaf", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public override bool? IsLeaf
        {
            get { return _isLeaf; }
            set
            {
                if ((_isLeaf == null) || (value == null) || (!value.Equals(_isLeaf)))
                {
                    _isLeaf = value;
                    NotifyPropertyChanged(Prop_IsLeaf);
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
                    NotifyPropertyChanged(Prop_SortIndex);
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
					_description = value;
					NotifyPropertyChanged(ParameterCatalog.Prop_Description);
				}
			}
		}

		[Property("CreaterID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CreaterID
		{
			get { return _createrID; }
			set
			{
				if ((_createrID == null) || (value == null) || (!value.Equals(_createrID)))
				{
					_createrID = value;
					NotifyPropertyChanged(ParameterCatalog.Prop_CreaterID);
				}
			}
		}

		[Property("CreaterName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CreaterName
		{
			get { return _createrName; }
			set
			{
				if ((_createrName == null) || (value == null) || (!value.Equals(_createrName)))
				{
					_createrName = value;
					NotifyPropertyChanged(ParameterCatalog.Prop_CreaterName);
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
					NotifyPropertyChanged(ParameterCatalog.Prop_LastModifiedDate);
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
					_createdDate = value;
					NotifyPropertyChanged(ParameterCatalog.Prop_CreatedDate);
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

	} // SysParameterCatalog
}

