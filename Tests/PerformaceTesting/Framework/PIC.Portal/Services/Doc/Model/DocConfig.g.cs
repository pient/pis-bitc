// Business class DocConfig generated from DocConfig
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
	[ActiveRecord("DocConfig")]
	public partial class DocConfig : DocModelBase<DocConfig>
	{
		#region Property_Names

		public static string Prop_ConfigID = "ConfigID";
		public static string Prop_Code = "Code";
		public static string Prop_Name = "Name";
		public static string Prop_Config = "Config";
		public static string Prop_Tag = "Tag";
		public static string Prop_Description = "Description";

		#endregion

		#region Private_Variables

        private Guid? _configid;
		private string _code;
		private string _name;
		private string _config;
		private string _tag;
		private string _description;


		#endregion

		#region Constructors

		public DocConfig()
		{
		}

		public DocConfig(
            Guid? p_configid,
			string p_code,
			string p_name,
			string p_config,
			string p_tag,
			string p_description)
		{
			_configid = p_configid;
			_code = p_code;
			_name = p_name;
			_config = p_config;
			_tag = p_tag;
			_description = p_description;
		}

		#endregion

		#region Properties

        [PrimaryKey("ConfigID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(SequentialGuidGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public Guid? ConfigID
		{
			get { return _configid; }
			set { _configid = value; }

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
					RaisePropertyChanged(DocConfig.Prop_Code, oldValue, value);
				}
			}

		}

		[Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((_name == null) || (value == null) || (!value.Equals(_name)))
				{
                    object oldValue = _name;
					_name = value;
					RaisePropertyChanged(DocConfig.Prop_Name, oldValue, value);
				}
			}

		}

		[Property("Config", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public string Config
		{
			get { return _config; }
			set
			{
				if ((_config == null) || (value == null) || (!value.Equals(_config)))
				{
                    object oldValue = _config;
					_config = value;
					RaisePropertyChanged(DocConfig.Prop_Config, oldValue, value);
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
					RaisePropertyChanged(DocConfig.Prop_Tag, oldValue, value);
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
					RaisePropertyChanged(DocConfig.Prop_Description, oldValue, value);
				}
			}

		}

		#endregion
	} // DocConfig
}

