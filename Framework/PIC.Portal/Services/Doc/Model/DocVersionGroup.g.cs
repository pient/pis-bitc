// Business class DocVersionGroup generated from DocVersionGroup
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
	[ActiveRecord("DocVersionGroup")]
    public partial class DocVersionGroup : DocModelBase<DocVersionGroup>
	{
		#region Property_Names

        public static string Prop_GroupID = "GroupID";
        public static string Prop_Name = "Name";
		public static string Prop_Tag = "Tag";
		public static string Prop_Description = "Description";

		#endregion

		#region Private_Variables

        private Guid? _groupid;
        private string _tag;
        private string _name;
		private string _description;


		#endregion

		#region Constructors

		public DocVersionGroup()
		{
		}

		public DocVersionGroup(
            Guid? p_groupid,
            string p_name,
            string p_tag,
			string p_description)
		{
            _groupid = p_groupid;
            _name = p_name;
			_tag = p_tag;
			_description = p_description;
		}

		#endregion

		#region Properties

        [PrimaryKey("GroupID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(SequentialGuidGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
        public Guid? GroupID
		{
			get { return _groupid; }
			set { _groupid = value; }

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
                    RaisePropertyChanged(DocVersionGroup.Prop_Name, oldValue, value);
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
					RaisePropertyChanged(DocVersionGroup.Prop_Tag, oldValue, value);
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
					RaisePropertyChanged(DocVersionGroup.Prop_Description, oldValue, value);
				}
			}

		}

		#endregion
	} // DocVersionGroup
}

