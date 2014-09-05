// Business class OA_Property generated from OA_Property
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
	[ActiveRecord("OA_Property")]
	public partial class OA_Property : BizModelBase<OA_Property>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Type = "Type";
		public static string Prop_Spec = "Spec";
		public static string Prop_Price = "Price";
		public static string Prop_Status = "Status";
		public static string Prop_Supplier = "Supplier";
		public static string Prop_Contact = "Contact";
		public static string Prop_ContactTel = "ContactTel";
		public static string Prop_Description = "Description";
		public static string Prop_CreatorId = "CreatorId";
		public static string Prop_CreatorName = "CreatorName";
		public static string Prop_CreatedDate = "CreatedDate";

		#endregion

		#region Private_Variables

		private string _id;
		private string _code;
		private string _name;
		private string _type;
		private string _spec;
		private System.Decimal? _price;
		private string _status;
		private string _supplier;
		private string _contact;
		private string _contactTel;
		private string _description;
		private string _creatorId;
		private string _creatorName;
		private DateTime? _createdDate;


		#endregion

		#region Constructors

		public OA_Property()
		{
		}

		public OA_Property(
			string p_id,
			string p_code,
			string p_name,
			string p_type,
			string p_spec,
			System.Decimal? p_price,
			string p_status,
			string p_supplier,
			string p_contact,
			string p_contactTel,
			string p_description,
			string p_creatorId,
			string p_creatorName,
			DateTime? p_createdDate)
		{
			_id = p_id;
			_code = p_code;
			_name = p_name;
			_type = p_type;
			_spec = p_spec;
			_price = p_price;
			_status = p_status;
			_supplier = p_supplier;
			_contact = p_contact;
			_contactTel = p_contactTel;
			_description = p_description;
			_creatorId = p_creatorId;
			_creatorName = p_creatorName;
			_createdDate = p_createdDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string Id
		{
			get { return _id; }
			// set { _id = value; } // 处理列表编辑时去掉注释

		}

		[Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 50)]
		public string Code
		{
			get { return _code; }
			set
			{
				if ((_code == null) || (value == null) || (!value.Equals(_code)))
				{
                    object oldValue = _code;
					_code = value;
					RaisePropertyChanged(OA_Property.Prop_Code, oldValue, value);
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
					RaisePropertyChanged(OA_Property.Prop_Name, oldValue, value);
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
					RaisePropertyChanged(OA_Property.Prop_Type, oldValue, value);
				}
			}

		}

		[Property("Spec", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public string Spec
		{
			get { return _spec; }
			set
			{
				if ((_spec == null) || (value == null) || (!value.Equals(_spec)))
				{
                    object oldValue = _spec;
					_spec = value;
					RaisePropertyChanged(OA_Property.Prop_Spec, oldValue, value);
				}
			}

		}

		[Property("Price", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public System.Decimal? Price
		{
			get { return _price; }
			set
			{
				if (value != _price)
				{
                    object oldValue = _price;
					_price = value;
					RaisePropertyChanged(OA_Property.Prop_Price, oldValue, value);
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
					RaisePropertyChanged(OA_Property.Prop_Status, oldValue, value);
				}
			}

		}

		[Property("Supplier", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public string Supplier
		{
			get { return _supplier; }
			set
			{
				if ((_supplier == null) || (value == null) || (!value.Equals(_supplier)))
				{
                    object oldValue = _supplier;
					_supplier = value;
					RaisePropertyChanged(OA_Property.Prop_Supplier, oldValue, value);
				}
			}

		}

		[Property("Contact", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public string Contact
		{
			get { return _contact; }
			set
			{
				if ((_contact == null) || (value == null) || (!value.Equals(_contact)))
				{
                    object oldValue = _contact;
					_contact = value;
					RaisePropertyChanged(OA_Property.Prop_Contact, oldValue, value);
				}
			}

		}

		[Property("ContactTel", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public string ContactTel
		{
			get { return _contactTel; }
			set
			{
				if ((_contactTel == null) || (value == null) || (!value.Equals(_contactTel)))
				{
                    object oldValue = _contactTel;
					_contactTel = value;
					RaisePropertyChanged(OA_Property.Prop_ContactTel, oldValue, value);
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
					RaisePropertyChanged(OA_Property.Prop_Description, oldValue, value);
				}
			}

		}

		[Property("CreatorId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public string CreatorId
		{
			get { return _creatorId; }
			set
			{
				if ((_creatorId == null) || (value == null) || (!value.Equals(_creatorId)))
				{
                    object oldValue = _creatorId;
					_creatorId = value;
					RaisePropertyChanged(OA_Property.Prop_CreatorId, oldValue, value);
				}
			}

		}

		[Property("CreatorName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string CreatorName
		{
			get { return _creatorName; }
			set
			{
				if ((_creatorName == null) || (value == null) || (!value.Equals(_creatorName)))
				{
                    object oldValue = _creatorName;
					_creatorName = value;
					RaisePropertyChanged(OA_Property.Prop_CreatorName, oldValue, value);
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
					RaisePropertyChanged(OA_Property.Prop_CreatedDate, oldValue, value);
				}
			}

		}

		#endregion
	} // OA_Property
}

