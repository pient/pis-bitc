// Business class OA_Reimbursement generated from OA_Reimbursement
// Creator: Ray
// Created Date: [2012-08-04]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
using PIC.Portal;
using PIC.Portal.Model;

namespace PIC.Biz.Model.Reimbursement
{
	[ActiveRecord("OA_Reimbursement")]
	public partial class OA_Reimbursement : BizModelBase<OA_Reimbursement>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Type = "Type";
		public static string Prop_Category = "Category";
		public static string Prop_Fee = "Fee";
		public static string Prop_FeeDate = "FeeDate";
		public static string Prop_Status = "Status";
		public static string Prop_Comments = "Comments";
		public static string Prop_Attachments = "Attachments";
		public static string Prop_Tag = "Tag";
		public static string Prop_Items = "Items";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreatorId = "CreatorId";
		public static string Prop_CreatorName = "CreatorName";
		public static string Prop_CreatedDate = "CreatedDate";

		#endregion

		#region Private_Variables

		private string _id;
		private string _code;
		private string _name;
		private string _type;
		private string _category;
		private System.Decimal? _fee;
		private DateTime? _feeDate;
		private string _status;
		private string _comments;
		private string _attachments;
		private string _tag;
		private string _items;
		private DateTime? _lastModifiedDate;
		private string _creatorId;
		private string _creatorName;
		private DateTime? _createdDate;


		#endregion

		#region Constructors

		public OA_Reimbursement()
		{
		}

		public OA_Reimbursement(
			string p_id,
			string p_code,
			string p_name,
			string p_type,
			string p_category,
			System.Decimal? p_fee,
			DateTime? p_feeDate,
			string p_status,
			string p_comments,
			string p_attachments,
			string p_tag,
			string p_items,
			DateTime? p_lastModifiedDate,
			string p_creatorId,
			string p_creatorName,
			DateTime? p_createdDate)
		{
			_id = p_id;
			_code = p_code;
			_name = p_name;
			_type = p_type;
			_category = p_category;
			_fee = p_fee;
			_feeDate = p_feeDate;
			_status = p_status;
			_comments = p_comments;
			_attachments = p_attachments;
			_tag = p_tag;
			_items = p_items;
			_lastModifiedDate = p_lastModifiedDate;
			_creatorId = p_creatorId;
			_creatorName = p_creatorName;
			_createdDate = p_createdDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public virtual string Id
		{
			get { return _id; }
			set { _id = value; }

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
					RaisePropertyChanged(OA_Reimbursement.Prop_Code, oldValue, value);
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
					RaisePropertyChanged(OA_Reimbursement.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Type
		{
			get { return _type; }
			set
			{
				if ((_type == null) || (value == null) || (!value.Equals(_type)))
				{
                    object oldValue = _type;
					_type = value;
					RaisePropertyChanged(OA_Reimbursement.Prop_Type, oldValue, value);
				}
			}

		}

		[Property("Category", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Category
		{
			get { return _category; }
			set
			{
				if ((_category == null) || (value == null) || (!value.Equals(_category)))
				{
                    object oldValue = _category;
					_category = value;
					RaisePropertyChanged(OA_Reimbursement.Prop_Category, oldValue, value);
				}
			}

		}

		[Property("Fee", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual System.Decimal? Fee
		{
			get { return _fee; }
			set
			{
				if (value != _fee)
				{
                    object oldValue = _fee;
					_fee = value;
					RaisePropertyChanged(OA_Reimbursement.Prop_Fee, oldValue, value);
				}
			}

		}

		[Property("FeeDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? FeeDate
		{
			get { return _feeDate; }
			set
			{
				if (value != _feeDate)
				{
                    object oldValue = _feeDate;
					_feeDate = value;
					RaisePropertyChanged(OA_Reimbursement.Prop_FeeDate, oldValue, value);
				}
			}

		}

		[Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Status
		{
			get { return _status; }
			set
			{
				if ((_status == null) || (value == null) || (!value.Equals(_status)))
				{
                    object oldValue = _status;
					_status = value;
					RaisePropertyChanged(OA_Reimbursement.Prop_Status, oldValue, value);
				}
			}

		}

		[Property("Comments", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public virtual string Comments
		{
			get { return _comments; }
			set
			{
				if ((_comments == null) || (value == null) || (!value.Equals(_comments)))
				{
                    object oldValue = _comments;
					_comments = value;
					RaisePropertyChanged(OA_Reimbursement.Prop_Comments, oldValue, value);
				}
			}

		}

		[Property("Attachments", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1500)]
		public virtual string Attachments
		{
			get { return _attachments; }
			set
			{
				if ((_attachments == null) || (value == null) || (!value.Equals(_attachments)))
				{
                    object oldValue = _attachments;
					_attachments = value;
					RaisePropertyChanged(OA_Reimbursement.Prop_Attachments, oldValue, value);
				}
			}

		}

		[Property("Tag", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 5000)]
		public virtual string Tag
		{
			get { return _tag; }
			set
			{
				if ((_tag == null) || (value == null) || (!value.Equals(_tag)))
				{
                    object oldValue = _tag;
					_tag = value;
					RaisePropertyChanged(OA_Reimbursement.Prop_Tag, oldValue, value);
				}
			}

		}

		[Property("Items", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 5000)]
		public virtual string Items
		{
			get { return _items; }
			set
			{
				if ((_items == null) || (value == null) || (!value.Equals(_items)))
				{
                    object oldValue = _items;
					_items = value;
					RaisePropertyChanged(OA_Reimbursement.Prop_Items, oldValue, value);
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
					RaisePropertyChanged(OA_Reimbursement.Prop_LastModifiedDate, oldValue, value);
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
					RaisePropertyChanged(OA_Reimbursement.Prop_CreatorId, oldValue, value);
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
					RaisePropertyChanged(OA_Reimbursement.Prop_CreatorName, oldValue, value);
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
					RaisePropertyChanged(OA_Reimbursement.Prop_CreatedDate, oldValue, value);
				}
			}

		}

		#endregion
	} // OA_Reimbursement
}

