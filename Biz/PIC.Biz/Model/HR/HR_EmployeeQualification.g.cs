// Business class HR_EmployeeQualification generated from HR_EmployeeQualification
// Creator: Ray
// Created Date: [2012-04-08]

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
	[ActiveRecord("HR_EmployeeQualification")]
	public partial class HR_EmployeeQualification : BizModelBase<HR_EmployeeQualification>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_EmployeeId = "EmployeeId";
		public static string Prop_Qualification = "Qualification";
		public static string Prop_Memo = "Memo";

		#endregion

		#region Private_Variables

		private string _id;
		private string _employeeId;
		private string _qualification;
		private string _memo;


		#endregion

		#region Constructors

		public HR_EmployeeQualification()
		{
		}

		public HR_EmployeeQualification(
			string p_id,
			string p_employeeId,
			string p_qualification,
			string p_memo)
		{
			_id = p_id;
			_employeeId = p_employeeId;
			_qualification = p_qualification;
			_memo = p_memo;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public virtual string Id
		{
			get { return _id; }
			set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("EmployeeId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public virtual string EmployeeId
		{
			get { return _employeeId; }
			set
			{
				if ((_employeeId == null) || (value == null) || (!value.Equals(_employeeId)))
				{
                    object oldValue = _employeeId;
					_employeeId = value;
					RaisePropertyChanged(HR_EmployeeQualification.Prop_EmployeeId, oldValue, value);
				}
			}

		}

		[Property("Qualification", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public virtual string Qualification
		{
			get { return _qualification; }
			set
			{
				if ((_qualification == null) || (value == null) || (!value.Equals(_qualification)))
				{
                    object oldValue = _qualification;
					_qualification = value;
					RaisePropertyChanged(HR_EmployeeQualification.Prop_Qualification, oldValue, value);
				}
			}

		}

		[Property("Memo", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public virtual string Memo
		{
			get { return _memo; }
			set
			{
				if ((_memo == null) || (value == null) || (!value.Equals(_memo)))
				{
                    object oldValue = _memo;
					_memo = value;
					RaisePropertyChanged(HR_EmployeeQualification.Prop_Memo, oldValue, value);
				}
			}

		}

		#endregion
	} // HR_EmployeeQualification
}

