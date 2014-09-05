// Business class OA_YellowPage generated from OA_YellowPage
// Creator: Ray
// Created Date: [2012-04-07]

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
	[ActiveRecord("OA_YellowPage")]
    public partial class OA_YellowPage : BizTreeNodeModelBase<OA_YellowPage>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
        new public static string Prop_ParentID = "ParentID";
        new public static string Prop_Path = "Path";
        new public static string Prop_PathLevel = "PathLevel";
        new public static string Prop_IsLeaf = "IsLeaf";
        new public static string Prop_SortIndex = "SortIndex";
		public static string Prop_Phone = "Phone";
		public static string Prop_Mobile = "Mobile";
		public static string Prop_Email = "Email";
		public static string Prop_Fax = "Fax";
		public static string Prop_Position = "Position";
		public static string Prop_Description = "Description";
		public static string Prop_Tag = "Tag";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreatedDate = "CreatedDate";
		public static string Prop_CreatorId = "CreatorId";
		public static string Prop_CreatorName = "CreatorName";

		#endregion

		#region Private_Variables

		private string _id;
		private string _code;
		private string _name;
		private string _parentID;
		private string _path;
		private int? _pathLevel;
		private bool? _isLeaf;
		private int? _sortIndex;
		private string _phone;
		private string _mobile;
		private string _email;
		private string _fax;
		private string _position;
		private string _description;
		private string _tag;
		private DateTime? _lastModifiedDate;
		private DateTime? _createdDate;
		private string _creatorId;
		private string _creatorName;


		#endregion

		#region Constructors

		public OA_YellowPage()
		{
		}

		public OA_YellowPage(
			string p_id,
			string p_code,
			string p_name,
			string p_parentID,
			string p_path,
			int? p_pathLevel,
			bool? p_isLeaf,
			int? p_sortIndex,
			string p_phone,
			string p_mobile,
			string p_email,
			string p_fax,
			string p_position,
			string p_description,
			string p_tag,
			DateTime? p_lastModifiedDate,
			DateTime? p_createdDate,
			string p_creatorId,
			string p_creatorName)
		{
			_id = p_id;
			_code = p_code;
			_name = p_name;
			_parentID = p_parentID;
			_path = p_path;
			_pathLevel = p_pathLevel;
			_isLeaf = p_isLeaf;
			_sortIndex = p_sortIndex;
			_phone = p_phone;
			_mobile = p_mobile;
			_email = p_email;
			_fax = p_fax;
			_position = p_position;
			_description = p_description;
			_tag = p_tag;
			_lastModifiedDate = p_lastModifiedDate;
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
			set { _id = value; } // 处理列表编辑时去掉注释

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
					RaisePropertyChanged(OA_YellowPage.Prop_Code, oldValue, value);
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
					RaisePropertyChanged(OA_YellowPage.Prop_Name, oldValue, value);
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
                    object oldValue = _parentID;
					_parentID = value;
					RaisePropertyChanged(OA_YellowPage.Prop_ParentID, oldValue, value);
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
					RaisePropertyChanged(OA_YellowPage.Prop_Path, oldValue, value);
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
					RaisePropertyChanged(OA_YellowPage.Prop_PathLevel, oldValue, value);
				}
			}

		}

		[Property("IsLeaf", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public override bool? IsLeaf
		{
			get { return _isLeaf; }
			set
			{
				if (value != _isLeaf)
				{
                    object oldValue = _isLeaf;
					_isLeaf = value;
					RaisePropertyChanged(OA_YellowPage.Prop_IsLeaf, oldValue, value);
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
					RaisePropertyChanged(OA_YellowPage.Prop_SortIndex, oldValue, value);
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
					RaisePropertyChanged(OA_YellowPage.Prop_Phone, oldValue, value);
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
					RaisePropertyChanged(OA_YellowPage.Prop_Mobile, oldValue, value);
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
					RaisePropertyChanged(OA_YellowPage.Prop_Email, oldValue, value);
				}
			}

		}

		[Property("Fax", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Fax
		{
			get { return _fax; }
			set
			{
				if ((_fax == null) || (value == null) || (!value.Equals(_fax)))
				{
                    object oldValue = _fax;
					_fax = value;
					RaisePropertyChanged(OA_YellowPage.Prop_Fax, oldValue, value);
				}
			}

		}

		[Property("Position", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public virtual string Position
		{
			get { return _position; }
			set
			{
				if ((_position == null) || (value == null) || (!value.Equals(_position)))
				{
                    object oldValue = _position;
					_position = value;
					RaisePropertyChanged(OA_YellowPage.Prop_Position, oldValue, value);
				}
			}

		}

		[Property("Description", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public virtual string Description
		{
			get { return _description; }
			set
			{
				if ((_description == null) || (value == null) || (!value.Equals(_description)))
				{
                    object oldValue = _description;
					_description = value;
					RaisePropertyChanged(OA_YellowPage.Prop_Description, oldValue, value);
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
					RaisePropertyChanged(OA_YellowPage.Prop_Tag, oldValue, value);
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
					RaisePropertyChanged(OA_YellowPage.Prop_LastModifiedDate, oldValue, value);
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
					RaisePropertyChanged(OA_YellowPage.Prop_CreatedDate, oldValue, value);
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
					RaisePropertyChanged(OA_YellowPage.Prop_CreatorId, oldValue, value);
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
					RaisePropertyChanged(OA_YellowPage.Prop_CreatorName, oldValue, value);
				}
			}

		}

		#endregion
	} // OA_YellowPage
}

