// Business class SysEvent generated from SysEvent
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Castle.ActiveRecord;
using Newtonsoft.Json;
using PIC.Data;
	
namespace PIC.Portal.Model
{
    [ActiveRecord("Event")]
	public partial class Event : EntityBase<Event> , INotifyPropertyChanged 	
	{

		#region Property_Names

		public static string Prop_EventID = "EventID";
		public static string Prop_LoginName = "LoginName";
		public static string Prop_UserID = "UserID";
		public static string Prop_ApplicationID = "ApplicationID";
		public static string Prop_ApplicationName = "ApplicationName";
		public static string Prop_ModuleID = "ModuleID";
		public static string Prop_ModuleName = "ModuleName";
		public static string Prop_AuthID = "AuthID";
		public static string Prop_AuthName = "AuthName";
		public static string Prop_Type = "Type";
		public static string Prop_IP = "IP";
		public static string Prop_Record = "Record";
		public static string Prop_DateTime = "DateTime";

		#endregion

		#region Private_Variables

		private int _eventid;
		private string _loginName;
		private string _userID;
		private int? _applicationID;
		private string _applicationName;
		private string _moduleID;
		private string _moduleName;
		private string _authID;
		private string _authName;
        private string _type;
		private string _iP;
		private string _record;
		private DateTime? _dateTime;


		#endregion

		#region Constructors

		public Event()
		{
		}

		public Event(
			int p_eventid,
			string p_loginName,
			string p_userID,
			int? p_applicationID,
			string p_applicationName,
			string p_moduleID,
			string p_moduleName,
			string p_authID,
			string p_authName,
            string p_type,
			string p_iP,
			string p_record,
			DateTime? p_dateTime)
		{
			_eventid = p_eventid;
			_loginName = p_loginName;
			_userID = p_userID;
			_applicationID = p_applicationID;
			_applicationName = p_applicationName;
			_moduleID = p_moduleID;
			_moduleName = p_moduleName;
			_authID = p_authID;
			_authName = p_authName;
			_type = p_type;
			_iP = p_iP;
			_record = p_record;
			_dateTime = p_dateTime;
		}

		#endregion

		#region Properties

		[PrimaryKey("EventID", Generator = PrimaryKeyType.Native, Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public int EventID
		{
			get { return _eventid; }
		}

		[Property("LoginName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 20)]
		public string LoginName
		{
			get { return _loginName; }
			set
			{
				if ((_loginName == null) || (value == null) || (!value.Equals(_loginName)))
				{
					_loginName = value;
					NotifyPropertyChanged(Event.Prop_LoginName);
				}
			}
		}

		[Property("UserID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string UserID
		{
			get { return _userID; }
			set
			{
				if ((_userID == null) || (value == null) || (!value.Equals(_userID)))
				{
					_userID = value;
					NotifyPropertyChanged(Event.Prop_UserID);
				}
			}
		}

		[Property("ApplicationID", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? ApplicationID
		{
			get { return _applicationID; }
			set
			{
				if (value != _applicationID)
				{
					_applicationID = value;
					NotifyPropertyChanged(Event.Prop_ApplicationID);
				}
			}
		}

		[Property("ApplicationName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ApplicationName
		{
			get { return _applicationName; }
			set
			{
				if ((_applicationName == null) || (value == null) || (!value.Equals(_applicationName)))
				{
					_applicationName = value;
					NotifyPropertyChanged(Event.Prop_ApplicationName);
				}
			}
		}

		[Property("ModuleID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ModuleID
		{
			get { return _moduleID; }
			set
			{
				if ((_moduleID == null) || (value == null) || (!value.Equals(_moduleID)))
				{
					_moduleID = value;
					NotifyPropertyChanged(Event.Prop_ModuleID);
				}
			}
		}

		[Property("ModuleName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string ModuleName
		{
			get { return _moduleName; }
			set
			{
				if ((_moduleName == null) || (value == null) || (!value.Equals(_moduleName)))
				{
					_moduleName = value;
					NotifyPropertyChanged(Event.Prop_ModuleName);
				}
			}
		}

		[Property("AuthID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string AuthID
		{
			get { return _authID; }
			set
			{
				if ((_authID == null) || (value == null) || (!value.Equals(_authID)))
				{
					_authID = value;
					NotifyPropertyChanged(Event.Prop_AuthID);
				}
			}
		}

		[Property("AuthName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string AuthName
		{
			get { return _authName; }
			set
			{
				if ((_authName == null) || (value == null) || (!value.Equals(_authName)))
				{
					_authName = value;
					NotifyPropertyChanged(Event.Prop_AuthName);
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
					NotifyPropertyChanged(Event.Prop_Type);
				}
			}
		}

		[Property("IP", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 25)]
		public string IP
		{
			get { return _iP; }
			set
			{
				if ((_iP == null) || (value == null) || (!value.Equals(_iP)))
				{
					_iP = value;
					NotifyPropertyChanged(Event.Prop_IP);
				}
			}
		}

		[Property("Record", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Record
		{
			get { return _record; }
			set
			{
				if ((_record == null) || (value == null) || (!value.Equals(_record)))
				{
					_record = value;
					NotifyPropertyChanged(Event.Prop_Record);
				}
			}
		}

		[Property("DateTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? DateTime
		{
			get { return _dateTime; }
			set
			{
				if (value != _dateTime)
				{
					_dateTime = value;
					NotifyPropertyChanged(Event.Prop_DateTime);
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

	} // SysEvent
}

