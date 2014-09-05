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
	[ActiveRecord("TempFileData")]
    public partial class TempFileData : DocModelBase<TempFileData>
	{
		#region Property_Names

        public static string Prop_DataID = "DataID";
        public static string Prop_GroupID = "GroupID";
        public static string Prop_FileName = "FileName";
        public static string Prop_Tag = "Tag";
        public static string Prop_BinaryData = "BinaryData";
        public static string Prop_CreatedDate = "CreatedDate";

		#endregion

		#region Private_Variables

        private Guid? _dataid;
        private Guid? _groupID;
        private byte[] _binaryData;
        private string _fileName;
        private string _tag;
        private DateTime? _createdDate;


		#endregion

		#region Constructors

		public TempFileData()
		{
		}

        public TempFileData(
            Guid? p_dataid,
            Guid? p_groupID,
            byte[] p_binaryData,
            string p_fileName,
			string p_tag,
            DateTime? p_createdDate)
		{
			_dataid = p_dataid;
            _groupID = p_groupID;
            _binaryData = p_binaryData;
            _fileName = p_fileName;
            _tag = p_tag;
            _createdDate = p_createdDate;
		}

		#endregion

		#region Properties

        [PrimaryKey("DataID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(SequentialGuidGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
        public Guid? DataID
		{
			get { return _dataid; }
			set { _dataid = value; }

		}

        [Property("GroupID", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public Guid? GroupID
        {
            get { return _groupID; }
            set
            {
                if ((_groupID == null) || (value == null) || (!value.Equals(_groupID)))
                {
                    object oldValue = _groupID;
                    _groupID = value;
                    RaisePropertyChanged(TempFileData.Prop_GroupID, oldValue, value);
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
                    RaisePropertyChanged(TempFileData.Prop_BinaryData, oldValue, value);
                }
            }

        }

        [Property("FileName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if ((_fileName == null) || (value == null) || (!value.Equals(_fileName)))
                {
                    object oldValue = _fileName;
                    _fileName = value;
                    RaisePropertyChanged(TempFileData.Prop_FileName, oldValue, value);
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
                    RaisePropertyChanged(TempFileData.Prop_Tag, oldValue, value);
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
                    RaisePropertyChanged(TempFileData.Prop_CreatedDate, oldValue, value);
                }
            }

        }

		#endregion
	} // DocDatum
}

