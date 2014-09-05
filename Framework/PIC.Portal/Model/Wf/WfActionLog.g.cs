// Business class SysWfActionLog generated from SysWfActionLog
// Creator: Ray
// Created Date: [2012-04-27]

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
    [ActiveRecord("WfActionLog")]
	public partial class WfActionLog : ModelBase<WfActionLog>
	{
		#region Property_Names

		public static string Prop_LogID = "LogID";
		public static string Prop_ActionID = "ActionID";
		public static string Prop_Code = "Code";
		public static string Prop_Title = "Title";
		public static string Prop_OperatorID = "OperatorID";
		public static string Prop_OperatorName = "OperatorName";
		public static string Prop_OperatedTime = "OperatedTime";
		public static string Prop_Description = "Description";
		public static string Prop_Tag = "Tag";

		#endregion

		#region Private_Variables

		private string _logid;
		private string _actionID;
		private string _code;
		private string _title;
		private string _operatorID;
		private string _operatorName;
		private DateTime? _operatedTime;
		private string _description;
		private string _tag;


		#endregion

		#region Constructors

		public WfActionLog()
		{
		}

		public WfActionLog(
			string p_logid,
			string p_actionID,
			string p_code,
			string p_title,
			string p_operatorID,
			string p_operatorName,
			DateTime? p_operatedTime,
			string p_description,
			string p_tag)
		{
			_logid = p_logid;
			_actionID = p_actionID;
			_code = p_code;
			_title = p_title;
			_operatorID = p_operatorID;
			_operatorName = p_operatorName;
			_operatedTime = p_operatedTime;
			_description = p_description;
			_tag = p_tag;
		}

		#endregion

		#region Properties

		[PrimaryKey("LogID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string LogID
		{
			get { return _logid; }
			set { _logid = value; }

		}

		[Property("ActionID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string ActionID
		{
			get { return _actionID; }
			set
			{
				if ((_actionID == null) || (value == null) || (!value.Equals(_actionID)))
				{
                    object oldValue = _actionID;
					_actionID = value;
					RaisePropertyChanged(WfActionLog.Prop_ActionID, oldValue, value);
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
					RaisePropertyChanged(WfActionLog.Prop_Code, oldValue, value);
				}
			}

		}

		[Property("Title", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public string Title
		{
			get { return _title; }
			set
			{
				if ((_title == null) || (value == null) || (!value.Equals(_title)))
				{
                    object oldValue = _title;
					_title = value;
					RaisePropertyChanged(WfActionLog.Prop_Title, oldValue, value);
				}
			}

		}

		[Property("OperatorID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string OperatorID
		{
			get { return _operatorID; }
			set
			{
				if ((_operatorID == null) || (value == null) || (!value.Equals(_operatorID)))
				{
                    object oldValue = _operatorID;
					_operatorID = value;
					RaisePropertyChanged(WfActionLog.Prop_OperatorID, oldValue, value);
				}
			}

		}

		[Property("OperatorName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string OperatorName
		{
			get { return _operatorName; }
			set
			{
				if ((_operatorName == null) || (value == null) || (!value.Equals(_operatorName)))
				{
                    object oldValue = _operatorName;
					_operatorName = value;
					RaisePropertyChanged(WfActionLog.Prop_OperatorName, oldValue, value);
				}
			}

		}

		[Property("OperatedTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? OperatedTime
		{
			get { return _operatedTime; }
			set
			{
				if (value != _operatedTime)
				{
                    object oldValue = _operatedTime;
					_operatedTime = value;
					RaisePropertyChanged(WfActionLog.Prop_OperatedTime, oldValue, value);
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
					RaisePropertyChanged(WfActionLog.Prop_Description, oldValue, value);
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
					RaisePropertyChanged(WfActionLog.Prop_Tag, oldValue, value);
				}
			}

		}

		#endregion
	} // SysWfActionLog
}

