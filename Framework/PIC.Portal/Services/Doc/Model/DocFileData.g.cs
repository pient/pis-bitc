// Business class DocDatum generated from DocData
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
	[ActiveRecord("DocFileData")]
    public partial class DocFileData : DocModelBase<DocFileData>
	{
		#region Property_Names

		public static string Prop_DataID = "DataID";
        public static string Prop_FileID = "FileID";
        public static string Prop_BinaryData = "BinaryData";
        public static string Prop_SortIndex = "SortIndex";
        public static string Prop_Tag = "Tag";

		#endregion

		#region Private_Variables

        private Guid? _dataid;
        private Guid? _fileID;
        private byte[] _binaryData;
        private string _tag;
        private int? _sortIndex;


		#endregion

		#region Constructors

		public DocFileData()
		{
		}

		public DocFileData(
            Guid? p_dataid,
            Guid? p_fileID,
            byte[] p_binaryData,
			string p_tag,
            int? p_sortIndex)
		{
			_dataid = p_dataid;
            _fileID = p_fileID;
            _binaryData = p_binaryData;
            _tag = p_tag;
            _sortIndex = p_sortIndex;
		}

		#endregion

		#region Properties

        [PrimaryKey("DataID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(SequentialGuidGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
        public Guid? DataID
		{
			get { return _dataid; }
			set { _dataid = value; }

		}

		[Property("FileID", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public Guid? FileID
		{
			get { return _fileID; }
			set
			{
				if ((_fileID == null) || (value == null) || (!value.Equals(_fileID)))
				{
                    object oldValue = _fileID;
					_fileID = value;
					RaisePropertyChanged(DocFileData.Prop_FileID, oldValue, value);
				}
			}

		}

        [JsonIgnore]
        [Property("BinaryData", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "BinaryBlob", NotNull = false, Lazy = false)]
        public byte[] BinaryData
        {
            get { return _binaryData; }
            set
            {
                if (value != _binaryData)
                {
                    object oldValue = _binaryData;
                    _binaryData = value;
                    RaisePropertyChanged(DocFileData.Prop_BinaryData, oldValue, value);
                }
            }

        }

        [Property("Tag", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 5000)]
        public string Tag
        {
            get { return _tag; }
            set
            {
                if ((_tag == null) || (value == null) || (!value.Equals(_tag)))
                {
                    object oldValue = _tag;
                    _tag = value;
                    RaisePropertyChanged(DocFileData.Prop_Tag, oldValue, value);
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
                    RaisePropertyChanged(DocFileData.Prop_SortIndex, oldValue, value);
                }
            }

        }

		#endregion
	} // DocDatum
}

