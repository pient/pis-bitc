// Business class TempFileDatum generated from TempFileData
// Creator: Ray
// Created Date: [2013-07-03]

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
	[ActiveRecord("TempFileData")]
	public partial class TempFileDatum : ModelBase<TempFileDatum>
	{
		#region Property_Names

		public static string Prop_DataID = "DataID";
		public static string Prop_Tag = "Tag";
		public static string Prop_BinaryData = "BinaryData";
		public static string Prop_CreatedDate = "CreatedDate";

		#endregion

		#region Private_Variables

		private string _dataid;
		private string _tag;
		private byte[] _binaryData;
		private DateTime _createdDate;


		#endregion

		#region Constructors

		public TempFileDatum()
		{
		}

		public TempFileDatum(
			string p_dataid,
			string p_tag,
			byte[] p_binaryData,
			DateTime p_createdDate)
		{
			_dataid = p_dataid;
			_tag = p_tag;
			_binaryData = p_binaryData;
			_createdDate = p_createdDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("DataID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string DataID
		{
			get { return _dataid; }
			private set { _dataid = value; }

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
					RaisePropertyChanged(TempFileDatum.Prop_Tag, oldValue, value);
				}
			}

		}

		[Property("BinaryData", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public byte[] BinaryData
		{
			get { return _binaryData; }
			set
			{
				if (value != _binaryData)
				{
                    object oldValue = _binaryData;
					_binaryData = value;
					RaisePropertyChanged(TempFileDatum.Prop_BinaryData, oldValue, value);
				}
			}

		}

		[Property("CreatedDate", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true)]
		public DateTime CreatedDate
		{
			get { return _createdDate; }
			set
			{
				if (value != _createdDate)
				{
                    object oldValue = _createdDate;
					_createdDate = value;
					RaisePropertyChanged(TempFileDatum.Prop_CreatedDate, oldValue, value);
				}
			}

		}

		#endregion
	} // TempFileDatum
}

