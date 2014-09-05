// Business class SysModuleType generated from SysModuleType
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
    [ActiveRecord("ModuleType")]
	public partial class ModuleType : EntityBase<ModuleType> , INotifyPropertyChanged 	
	{

		#region Property_Names

		public static string Prop_ModuleTypeID = "ModuleTypeID";
		public static string Prop_Name = "Name";

		#endregion

		#region Private_Variables

		private int _moduletypeid;
		private string _name;


		#endregion

		#region Constructors

		public ModuleType()
		{
		}

		public ModuleType(
			int p_moduletypeid,
			string p_name)
		{
			_moduletypeid = p_moduletypeid;
			_name = p_name;
		}

		#endregion

		#region Properties

		[PrimaryKey("ModuleTypeID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public int ModuleTypeID
		{
			get { return _moduletypeid; }
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
					NotifyPropertyChanged(ModuleType.Prop_Name);
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

	} // SysModuleType
}

