// Business class DocFileDatum generated from DocFileData
// Creator: Ray
// Created Date: [2013-06-26]

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
	[ActiveRecord("DocFileData")]
	public partial class DocFileDatum : ModelBase<DocFileDatum>
	{
		#region Property_Names

		public static string Prop_DataID = "DataID";
		public static string Prop_File = "File";
		public static string Prop_BinaryData = "BinaryData";
		public static string Prop_SortIndex = "SortIndex";
		public static string Prop_Tag = "Tag";

		#endregion

		#region Private_Variables

		private string _dataid;
		private DocFile _file;
		private byte[] _binaryData;
		private int? _sortIndex;
		private string _tag;


		#endregion

		#region Constructors

		public DocFileDatum()
		{
		}

		public DocFileDatum(
			string p_dataid,
			DocFile p_file,
			byte[] p_binaryData,
			int? p_sortIndex,
			string p_tag)
		{
			_dataid = p_dataid;
			_file = p_file;
			_binaryData = p_binaryData;
			_sortIndex = p_sortIndex;
			_tag = p_tag;
		}

		#endregion

		#region Properties

		[PrimaryKey("DataID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string DataID
		{
			get { return _dataid; }
			private set { _dataid = value; }

		}

		[BelongsTo("FileID", Type = typeof(File), Cascade = CascadeEnum.All, Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DocFile File
		{
			get { return _file; }
			set
			{
				if ((_file == null) || (value == null) || (value.FileID != _file.FileID))
				{
                    object oldValue = _file;
					if (value == null)
						_file = null;
					else
						_file = (value.FileID > 0) ? value : null;
					RaisePropertyChanged(DocFileDatum.Prop_File, oldValue, value);
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
					RaisePropertyChanged(DocFileDatum.Prop_BinaryData, oldValue, value);
				}
			}

		}

		[Property("SortIndex", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public int? SortIndex
		{
			get { return _sortIndex; }
			set
			{
				if (value != _sortIndex)
				{
                    object oldValue = _sortIndex;
					_sortIndex = value;
					RaisePropertyChanged(DocFileDatum.Prop_SortIndex, oldValue, value);
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
					RaisePropertyChanged(DocFileDatum.Prop_Tag, oldValue, value);
				}
			}

		}

		#endregion
	} // DocFileDatum
}

