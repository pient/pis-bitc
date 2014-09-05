// Business class SysApplication generated from SysApplication
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
    [ActiveRecord("Application")]
	public partial class Application : EntityBase<Application> , INotifyPropertyChanged 	
	{

		#region Property_Names

        public static string Prop_ApplicationID = "ApplicationID";
        public static string Prop_Code = "Code";
        public static string Prop_Name = "Name";
        public static string Prop_Type = "Type";
		public static string Prop_Description = "Description";
		public static string Prop_Url = "Url";
        public static string Prop_SortIndex = "SortIndex";
        public static string Prop_Status = "Status";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreateDate = "CreateDate";

		#endregion

		#region Private_Variables

        private string _applicationid;
        private string _code;
        private string _name;
        private string _type;
		private string _description;
		private string _url;
        private int? _sortIndex;
        private int? _status;
		private DateTime? _lastModifiedDate;
		private DateTime? _createDate;


		#endregion

		#region Constructors

		public Application()
		{
		}

		public Application(
            string p_applicationid,
            string p_code,
            string p_name,
            string p_type,
			string p_description,
			string p_url,
            int? p_sortIndex,
            int? p_Status,
			DateTime? p_lastModifiedDate,
			DateTime? p_createDate)
		{
			_applicationid = p_applicationid;
            _name = p_name;
            _code = p_code;
            _type = p_type;
			_description = p_description;
			_url = p_url;
            _sortIndex = p_sortIndex;
            _status = p_Status;
			_lastModifiedDate = p_lastModifiedDate;
			_createDate = p_createDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("ApplicationID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string ApplicationID
		{
			get { return _applicationid; }
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
                    NotifyPropertyChanged(Model.Module.Prop_Code);
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
					NotifyPropertyChanged(Application.Prop_Name);
				}
			}
        }

        [Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    NotifyPropertyChanged(Model.Module.Prop_Type);
                }
            }
        }

		[Property("Description", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 200)]
		public string Description
		{
			get { return _description; }
			set
			{
				if ((_description == null) || (value == null) || (!value.Equals(_description)))
				{
					_description = value;
					NotifyPropertyChanged(Application.Prop_Description);
				}
			}
		}

		[Property("Url", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Url
		{
			get { return _url; }
			set
			{
				if ((_url == null) || (value == null) || (!value.Equals(_url)))
				{
					_url = value;
					NotifyPropertyChanged(Application.Prop_Url);
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
					NotifyPropertyChanged(Application.Prop_SortIndex);
				}
			}
        }

        [Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public int? Status
        {
            get { return _status; }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    NotifyPropertyChanged(Model.Module.Prop_Status);
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
					NotifyPropertyChanged(Application.Prop_LastModifiedDate);
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
					NotifyPropertyChanged(Application.Prop_CreateDate);
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

	} // SysApplication
}

