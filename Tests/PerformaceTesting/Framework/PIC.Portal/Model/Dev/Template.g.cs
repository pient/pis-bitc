// Business class Template generated from Template
// Creator: Ray
// Created Date: [2013-08-22]

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
	[ActiveRecord("Template")]
	public partial class Template : ModelBase<Template>
	{
		#region Property_Names

		public static string Prop_TemplateID = "TemplateID";
		public static string Prop_CatalogID = "CatalogID";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Type = "Type";
		public static string Prop_Config = "Config";
		public static string Prop_Tag = "Tag";
		public static string Prop_Data = "Data";
		public static string Prop_Description = "Description";
		public static string Prop_Status = "Status";
		public static string Prop_SortIndex = "SortIndex";
		public static string Prop_EditStatus = "EditStatus";
		public static string Prop_CreaterID = "CreaterID";
		public static string Prop_CreaterName = "CreaterName";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreatedDate = "CreatedDate";

		#endregion

		#region Private_Variables

		private string _templateid;
		private string _catalogID;
		private string _code;
		private string _name;
		private string _type;
		private string _config;
		private string _tag;
		private string _data;
		private string _description;
		private string _status;
		private int? _sortIndex;
		private string _editStatus;
		private string _createrID;
		private string _createrName;
		private DateTime? _lastModifiedDate;
		private DateTime? _createdDate;


		#endregion

		#region Constructors

		public Template()
		{
		}

		public Template(
			string p_templateid,
			string p_catalogID,
			string p_code,
			string p_name,
			string p_type,
			string p_config,
			string p_tag,
			string p_data,
			string p_description,
			string p_status,
			int? p_sortIndex,
			string p_editStatus,
			string p_createrID,
			string p_createrName,
			DateTime? p_lastModifiedDate,
			DateTime? p_createdDate)
		{
			_templateid = p_templateid;
			_catalogID = p_catalogID;
			_code = p_code;
			_name = p_name;
			_type = p_type;
			_config = p_config;
			_tag = p_tag;
			_data = p_data;
			_description = p_description;
			_status = p_status;
			_sortIndex = p_sortIndex;
			_editStatus = p_editStatus;
			_createrID = p_createrID;
			_createrName = p_createrName;
			_lastModifiedDate = p_lastModifiedDate;
			_createdDate = p_createdDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("TemplateID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string TemplateID
		{
			get { return _templateid; }
			set { _templateid = value; }

		}

		[Property("CatalogID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CatalogID
		{
			get { return _catalogID; }
			set
			{
				if ((_catalogID == null) || (value == null) || (!value.Equals(_catalogID)))
				{
                    object oldValue = _catalogID;
					_catalogID = value;
					RaisePropertyChanged(Template.Prop_CatalogID, oldValue, value);
				}
			}

		}

		[Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Code
		{
			get { return _code; }
			set
			{
				if ((_code == null) || (value == null) || (!value.Equals(_code)))
				{
                    object oldValue = _code;
					_code = value;
					RaisePropertyChanged(Template.Prop_Code, oldValue, value);
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
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(Template.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Type
		{
			get { return _type; }
			set
			{
				if ((_type == null) || (value == null) || (!value.Equals(_type)))
				{
                    object oldValue = _type;
					_type = value;
					RaisePropertyChanged(Template.Prop_Type, oldValue, value);
				}
			}

		}

        [Property("Config", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Config
		{
			get { return _config; }
			set
			{
				if ((_config == null) || (value == null) || (!value.Equals(_config)))
				{
                    object oldValue = _config;
					_config = value;
					RaisePropertyChanged(Template.Prop_Config, oldValue, value);
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
					RaisePropertyChanged(Template.Prop_Tag, oldValue, value);
				}
			}

		}

		[Property("Data", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Data
		{
			get { return _data; }
			set
			{
				if ((_data == null) || (value == null) || (!value.Equals(_data)))
				{
                    object oldValue = _data;
					_data = value;
					RaisePropertyChanged(Template.Prop_Data, oldValue, value);
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
					RaisePropertyChanged(Template.Prop_Description, oldValue, value);
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
					RaisePropertyChanged(Template.Prop_Status, oldValue, value);
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
					RaisePropertyChanged(Template.Prop_SortIndex, oldValue, value);
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
					RaisePropertyChanged(Template.Prop_EditStatus, oldValue, value);
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
                    object oldValue = _createrID;
					_createrID = value;
					RaisePropertyChanged(Template.Prop_CreaterID, oldValue, value);
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
                    object oldValue = _createrName;
					_createrName = value;
					RaisePropertyChanged(Template.Prop_CreaterName, oldValue, value);
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
					RaisePropertyChanged(Template.Prop_LastModifiedDate, oldValue, value);
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
					RaisePropertyChanged(Template.Prop_CreatedDate, oldValue, value);
				}
			}

		}

		#endregion
	} // Template
}

