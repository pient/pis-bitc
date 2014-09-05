// Business class OrgRole generated from OrgRole
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
    [ActiveRecord("OrgRole")]
	public partial class OrgRole : EntityBase<OrgRole> , INotifyPropertyChanged 	
	{
		#region Property_Names

        public static string Prop_RoleID = "RoleID";
        public static string Prop_Code = "Code";
        public static string Prop_Name = "Name";
        public static string Prop_Type = "Type";
        public static string Prop_SortIndex = "SortIndex";
        public static string Prop_Status = "Status";
        public static string Prop_EditStatus = "EditStatus";
        public static string Prop_Description = "Description";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreateDate = "CreateDate";

		#endregion

		#region Private_Variables

		private string _roleid;
        private string _code;
        private string _name;
        private int? _type;
        private int? _sortIndex;
        private int? _status;
        private string _editStatus;
        private string _description;
		private DateTime? _lastModifiedDate;
		private DateTime? _createDate;

		#endregion

		#region Constructors

		public OrgRole()
		{
		}

		public OrgRole(
            string p_roleid,
            string p_code,
            string p_name,
            int? p_type,
			string p_description,
			int? p_sortIndex,
			DateTime? p_lastModifiedDate,
			DateTime? p_createDate)
		{
            _roleid = p_roleid;
            _name = p_name;
            _code = p_code;
            _type = p_type;
			_description = p_description;
			_sortIndex = p_sortIndex;
			_lastModifiedDate = p_lastModifiedDate;
			_createDate = p_createDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("RoleID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string RoleID
		{
			get { return _roleid; }
		}

        [Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 50)]
        public string Code
        {
            get { return _code; }
            set
            {
                if ((_code == null) || (value == null) || (!value.Equals(_code)))
                {
                    _code = value;
                    NotifyPropertyChanged(Module.Prop_Code);
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
                    NotifyPropertyChanged(Module.Prop_Name);
                }
            }
        }

        [Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
        public int? Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    NotifyPropertyChanged(Module.Prop_Type);
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
					NotifyPropertyChanged(OrgRole.Prop_Description);
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
					_sortIndex = value;
					NotifyPropertyChanged(OrgRole.Prop_SortIndex);
				}
			}
		}

        [Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
        public int? Status
        {
            get { return _status; }
            set
            {
                if ((_status == null) || (value == null) || (!value.Equals(_status)))
                {
                    _status = value;
                    NotifyPropertyChanged(OrgRole.Prop_Status);
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
                    NotifyPropertyChanged(OrgRole.Prop_EditStatus);
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
					NotifyPropertyChanged(OrgRole.Prop_LastModifiedDate);
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
					NotifyPropertyChanged(OrgRole.Prop_CreateDate);
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

    } // OrgRole
}

