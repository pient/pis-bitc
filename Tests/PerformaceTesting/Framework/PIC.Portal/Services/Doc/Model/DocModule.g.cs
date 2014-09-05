// Business class DocModule generated from DocModule
// Creator: Ray
// Created Date: [2013-06-23]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
using PIC.Portal;
	
namespace PIC.Doc.Model
{
	[ActiveRecord("DocModule")]
    public partial class DocModule : DocModelBase<DocModule>
	{
		#region Property_Names

		public static string Prop_ModuleID = "ModuleID";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Provider = "Provider";
		public static string Prop_Tag = "Tag";
		public static string Prop_Description = "Description";

		#endregion

		#region Private_Variables

        private Guid? _moduleid;
		private string _code;
		private string _name;
		private string _provider;
		private string _tag;
		private string _description;

		#endregion

		#region Constructors

		public DocModule()
		{
		}

		public DocModule(
            Guid? p_moduleid,
			string p_code,
			string p_name,
			string p_provider,
			string p_tag,
			string p_description)
		{
			_moduleid = p_moduleid;
			_code = p_code;
			_name = p_name;
			_provider = p_provider;
			_tag = p_tag;
			_description = p_description;
		}

		#endregion

		#region Properties

        [PrimaryKey("ModuleID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(SequentialGuidGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
        public Guid? ModuleID
		{
			get { return _moduleid; }
			set { _moduleid = value; }

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
					RaisePropertyChanged(DocModule.Prop_Code, oldValue, value);
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
					RaisePropertyChanged(DocModule.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Provider", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Provider
		{
			get { return _provider; }
			set
			{
				if ((_provider == null) || (value == null) || (!value.Equals(_provider)))
				{
                    object oldValue = _provider;
					_provider = value;
					RaisePropertyChanged(DocModule.Prop_Provider, oldValue, value);
				}
			}

		}

		[Property("Tag", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Tag
		{
			get { return _tag; }
			set
			{
				if ((_tag == null) || (value == null) || (!value.Equals(_tag)))
				{
                    object oldValue = _tag;
					_tag = value;
					RaisePropertyChanged(DocModule.Prop_Tag, oldValue, value);
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
					RaisePropertyChanged(DocModule.Prop_Description, oldValue, value);
				}
			}

		}
		
		#endregion
	} // DocModule
}

