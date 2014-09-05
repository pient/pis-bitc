// Business class OrgUserConfig generated from OrgUserConfig
// Creator: Ray
// Created Date: [2013-08-07]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
using PIC.Portal;

namespace PIC.Portal.Model
{
    [ActiveRecord("OrgUserConfig")]
    public partial class OrgUserConfig : ModelBase<OrgUserConfig>
    {
        #region Property_Names

        public static string Prop_ConfigID = "ConfigID";
        public static string Prop_UserID = "UserID";
        public static string Prop_Code = "Code";
        public static string Prop_Name = "Name";
        public static string Prop_Type = "Type";
        public static string Prop_Catalog = "Catalog";
        public static string Prop_Status = "Status";
        public static string Prop_Data = "Data";
        public static string Prop_BakData = "BakData";
        public static string Prop_Tag = "Tag";
        public static string Prop_Version = "Version";
        public static string Prop_LastModifiedTime = "LastModifiedTime";

        #endregion

        #region Private_Variables

        private string _configid;
        private string _userID;
        private string _code;
        private string _name;
        private string _type;
        private string _catalog;
        private string _status;
        private string _data;
        private string _bakData;
        private string _tag;
        private string _version;
        private DateTime? _lastModifiedTime;


        #endregion

        #region Constructors

        public OrgUserConfig()
        {
        }

        public OrgUserConfig(
            string p_configid,
            string p_userID,
            string p_code,
            string p_name,
            string p_type,
            string p_catalog,
            string p_status,
            string p_data,
            string p_tag,
            string p_version,
            DateTime? p_lastModifiedTime)
        {
            _configid = p_configid;
            _userID = p_userID;
            _code = p_code;
            _name = p_name;
            _type = p_type;
            _catalog = p_catalog;
            _status = p_status;
            _data = p_data;
            _tag = p_tag;
            _version = p_version;
            _lastModifiedTime = p_lastModifiedTime;
        }

        #endregion

        #region Properties

        [PrimaryKey("ConfigID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
        public string ConfigID
        {
            get { return _configid; }
            set { _configid = value; }

        }

        [Property("UserID", Access = PropertyAccess.NosetterCamelcaseUnderscore, NotNull = true, Length = 36)]
        public string UserID
        {
            get { return _userID; }
            set
            {
                if ((_userID == null) || (value == null) || (!value.Equals(_userID)))
                {
                    object oldValue = _userID;
                    _userID = value;
                    RaisePropertyChanged(OrgUserConfig.Prop_UserID, oldValue, value);
                }
            }

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
                    RaisePropertyChanged(OrgUserConfig.Prop_Code, oldValue, value);
                }
            }

        }

        [Property("Name", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string Name
        {
            get { return _name; }
            set
            {
                if ((_name == null) || (value == null) || (!value.Equals(_name)))
                {
                    object oldValue = _name;
                    _name = value;
                    RaisePropertyChanged(OrgUserConfig.Prop_Name, oldValue, value);
                }
            }

        }

        [Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string Type
        {
            get { return _type; }
            set
            {
                if ((_type == null) || (value == null) || (!value.Equals(_type)))
                {
                    object oldValue = _type;
                    _type = value;
                    RaisePropertyChanged(OrgUserConfig.Prop_Type, oldValue, value);
                }
            }

        }

        [Property("Catalog", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string Catalog
        {
            get { return _catalog; }
            set
            {
                if ((_catalog == null) || (value == null) || (!value.Equals(_catalog)))
                {
                    object oldValue = _catalog;
                    _catalog = value;
                    RaisePropertyChanged(OrgUserConfig.Prop_Catalog, oldValue, value);
                }
            }

        }

        [Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string Status
        {
            get { return _status; }
            set
            {
                if ((_status == null) || (value == null) || (!value.Equals(_status)))
                {
                    object oldValue = _status;
                    _status = value;
                    RaisePropertyChanged(OrgUserConfig.Prop_Status, oldValue, value);
                }
            }

        }

        [JsonIgnore]
        [Property("BakData", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
        public string BakData
        {
            get { return _bakData; }
            set
            {
                if ((_bakData == null) || (value == null) || (!value.Equals(_bakData)))
                {
                    object oldValue = _bakData;
                    _bakData = value;
                    RaisePropertyChanged(OrgUserConfig.Prop_BakData, oldValue, value);
                }
            }

        }

        [JsonIgnore]
        [Property("Data", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
        public string Data
        {
            get { return _data; }
            set
            {
                if ((_data == null) || (value == null) || (!value.Equals(_data)))
                {
                    object oldValue = _data;
                    _data = value;
                    RaisePropertyChanged(OrgUserConfig.Prop_Data, oldValue, value);
                }
            }

        }

        [JsonIgnore]
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
                    RaisePropertyChanged(OrgUserConfig.Prop_Tag, oldValue, value);
                }
            }

        }

        [Property("Version", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string Version
        {
            get { return _version; }
            set
            {
                if ((_version == null) || (value == null) || (!value.Equals(_version)))
                {
                    object oldValue = _version;
                    _version = value;
                    RaisePropertyChanged(OrgUserConfig.Prop_Version, oldValue, value);
                }
            }

        }

        [Property("LastModifiedTime", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public DateTime? LastModifiedTime
        {
            get { return _lastModifiedTime; }
            set
            {
                if (value != _lastModifiedTime)
                {
                    object oldValue = _lastModifiedTime;
                    _lastModifiedTime = value;
                    RaisePropertyChanged(OrgUserConfig.Prop_LastModifiedTime, oldValue, value);
                }
            }

        }

        #endregion
    } // OrgUserConfig
}

