// Business class OrgGroupType generated from OrgGroupType
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Castle.ActiveRecord;
using PIC.Data;
	
namespace PIC.Portal.Model
{
    [ActiveRecord("OrgType")]
	public partial class OrgType : EntityBase<OrgType> , INotifyPropertyChanged 	
	{
		#region Property_Names

        public static string Prop_OrgTypeID = "OrgTypeID";
        public static string Prop_Name = "Name";
        public static string Prop_Tag = "Tag";
        public static string Prop_Description = "Description";

		#endregion

		#region Private_Variables

		private int _orgtypeid;
        private string _name;
        private string _tag;
        private string _description;

		#endregion

		#region Constructors

		public OrgType()
		{
		}

		public OrgType(
            int p_grouptypeid,
            string p_name,
            string p_tag,
            string p_description)
		{
            _orgtypeid = p_grouptypeid;
            _name = p_name;
            _tag = p_tag;
            _description = p_description;
		}

		#endregion

		#region Properties

        [PrimaryKey("OrgTypeID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
        public int OrgTypeID
		{
            get { return _orgtypeid; }
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
					NotifyPropertyChanged(OrgType.Prop_Name);
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
                    RaisePropertyChanged(OrgGroup.Prop_Tag, oldValue, value);
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
                    _description = value;
                    NotifyPropertyChanged(OrgGroup.Prop_Description);
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

    } // OrgGroupType
}

