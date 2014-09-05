// Business class OrgAuthType generated from OrgAuthType
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
	[ActiveRecord("AuthType")]
	public partial class AuthType : EntityBase<AuthType> , INotifyPropertyChanged 	
	{

		#region Property_Names

		public static string Prop_AuthTypeID = "AuthTypeID";
		public static string Prop_Name = "Name";
		public static string Prop_Description = "Description";

		#endregion

		#region Private_Variables

		private int _authtypeid;
		private string _name;
		private string _description;


		#endregion

		#region Constructors

		public AuthType()
		{
		}

		public AuthType(
			int p_authtypeid,
			string p_name,
			string p_description)
		{
			_authtypeid = p_authtypeid;
			_name = p_name;
			_description = p_description;
		}

		#endregion

		#region Properties

		[PrimaryKey("AuthTypeID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public int AuthTypeID
		{
			get { return _authtypeid; }
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
					NotifyPropertyChanged(AuthType.Prop_Name);
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
					NotifyPropertyChanged(AuthType.Prop_Description);
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

    } // OrgAuthType
}

