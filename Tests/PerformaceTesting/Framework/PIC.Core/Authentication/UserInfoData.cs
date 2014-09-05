using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using PIC.Data;

namespace PIC.Common.Authentication
{
    /// <summary>
    /// 用户信息的Xml版本
    /// </summary>
    public class UserInfoData : DataContainer
    {
        #region 私有变量

        private string _userid;
        private string _loginName;
        private string _name;
        private string _email;
        private string _remark;
        private byte? _State;
        private string _lastLogIp;
        private DateTime? _lastLogDate;
        private int? _sortIndex;
        private DateTime? _lastModifiedDate;
        private DateTime? _createDate;

        private AuthInfo _authInfo = new AuthInfo();
        private GroupInfo _groupInfo = new GroupInfo();
        private RoleInfo _roleInfo = new RoleInfo();

        #endregion

        #region 属性

        public string UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }

        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        public byte? State
        {
            get { return _State; }
            set { _State = value; }
        }

        public string LastLogIp
        {
            get { return _lastLogIp; }
            set { _lastLogIp = value; }
        }

        public DateTime? LastLogDate
        {
            get { return _lastLogDate; }
            set { _lastLogDate = value; }
        }

        public int? SortIndex
        {
            get { return _sortIndex; }
            set { _sortIndex = value; }
        }

        public DateTime? LastModifiedDate
        {
            get { return _lastModifiedDate; }
            set { _lastModifiedDate = value; }
        }

        public DateTime? CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        // 用户权限信息
        public AuthInfo AuthInfo
        {
            get { return _authInfo; }
            set { _authInfo = value; }
        }

        // 用户组信息
        public GroupInfo GroupInfo
        {
            get { return _groupInfo; }
            set { _groupInfo = value; }
        }

        // 用户角色信息
        public RoleInfo RoleInfo
        {
            get { return _roleInfo; }
            set { _roleInfo = value; }
        }

        #endregion

        #region 构造函数

        public UserInfoData() { }

        #endregion
    }

    /// <summary>
    /// 模块元素
    /// </summary>
    public class ModuleElement : DataElement
    {
        #region 构造函数

        public ModuleElement()
            : base()
        {
        }

        public ModuleElement(XmlDocument doc, XmlElement ele)
            : base(doc, ele)
        {
        }

        #endregion

        #region 属性

        public string ModuleID
        {
            get
            {
                return this.GetAttr("ModuleID");
            }
            set
            {
                this.SetAttr("ModuleID", value);
            }
        }

        public string Key
        {
            get
            {
                return this.GetAttr("Key");
            }
            set
            {
                this.SetAttr("Key", value);
            }
        }

        public string Name
        {
            get
            {
                return this.GetAttr("Name");
            }
            set
            {
                this.SetAttr("Name", value);
            }
        }

        public int? ModuleType
        {
            get
            {
                try { return Convert.ToInt32(this.GetAttr("Type")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("Type", value.ToString());
            }
        }

        public string ApplicationID
        {
            get
            {
                return this.GetAttr("ApplicationID");
            }
            set
            {
                this.SetAttr("ApplicationID", value);
            }
        }

        public string ParentID
        {
            get
            {
                return this.GetAttr("ParentID");
            }
            set
            {
                this.SetAttr("ParentID", value);
            }
        }

        public string ModulePath
        {
            get
            {
                return this.GetAttr("ModulePath");
            }
            set
            {
                this.SetAttr("ModulePath", value);
            }
        }

        public int? PathLevel
        {
            get
            {
                try { return Convert.ToInt32(this.GetAttr("PathLevel")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("PathLevel", value.ToString());
            }
        }

        public string Url
        {
            get
            {
                return this.GetAttr("Url");
            }
            set
            {
                this.SetAttr("Url", value);
            }
        }

        public string Icon
        {
            get
            {
                return this.GetAttr("Icon");
            }
            set
            {
                this.SetAttr("Icon", value);
            }
        }

        public string Description
        {
            get
            {
                return this.GetAttr("Description");
            }
            set
            {
                this.SetAttr("Description", value);
            }
        }

        public byte? State
        {
            get
            {
                try { return Convert.ToByte(this.GetAttr("State")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("State", value.ToString());
            }
        }

        public byte? IsSystem
        {
            get
            {
                try { return Convert.ToByte(this.GetAttr("IsSystem")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("IsSystem", value.ToString());
            }
        }

        public int? SortIndex
        {
            get
            {
                try { return Convert.ToInt32(this.GetAttr("SortIndex")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("SortIndex", value.ToString());
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                try { return Convert.ToDateTime(this.GetAttr("LastModifiedDate")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("LastModifiedDate", value.ToString());
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                try { return Convert.ToDateTime(this.GetAttr("CreateDate")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("CreateDate", value.ToString());
            }
        }

        #endregion
    }

    /// <summary>
    /// 用户权限项
    /// </summary>
    public class AuthElement : DataElement
    {
        #region 构造函数

        public AuthElement()
            : base()
        {
        }

        public AuthElement(XmlDocument doc, XmlElement ele)
            : base(doc, ele)
        {
        }

        #endregion

        #region 属性

        public string AuthID
        {
            get
            {
                return this.GetAttr("AuthID");
            }
            set
            {
                this.SetAttr("AuthID", value);
            }
        }

        public string Name
        {
            get
            {
                return this.GetAttr("Name");
            }
            set
            {
                this.SetAttr("Name", value);
            }
        }

        public string AuthPath
        {
            get
            {
                return this.GetAttr("AuthPath");
            }
            set
            {
                this.SetAttr("AuthPath", value);
            }
        }

        public int? AuthType
        {
            get
            {
                try { return Convert.ToInt32(this.GetAttr("Type")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("Type", value.ToString());
            }
        }

        public string Data
        {
            get
            {
                return this.GetAttr("Data");
            }
            set
            {
                this.SetAttr("Data", value);
            }
        }

        public int? SortIndex
        {
            get
            {
                try { return Convert.ToInt32(this.GetAttr("SortIndex")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("SortIndex", value.ToString());
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                try { return Convert.ToDateTime(this.GetAttr("LastModifiedDate")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("LastModifiedDate", value.ToString());
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                try { return Convert.ToDateTime(this.GetAttr("CreateDate")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("CreateDate", value.ToString());
            }
        }

        /// <summary>
        /// Module即此Element的Value值
        /// </summary>
        public ModuleElement Module
        {
            get
            {
                ModuleElement me = new ModuleElement();
                me.SetXmlValue(this.GetValue());

                return me;
            }
            set
            {
                this.SetXmlValue(value.GetXmlValue());
            }
        }

        #endregion
    }

    /// <summary>
    /// 用户权限信息
    /// </summary>
    public class AuthInfo : DataCollection
    {
        #region 构造函数

        public AuthInfo()
            : base()
        {}

        public AuthInfo(string xmlsrc)
            : base(xmlsrc)
        {
        }

        public AuthInfo(XmlElement ele)
            : base(ele)
        {
        }

        public AuthInfo(DataCollection dc)
            : base(dc.ToString())
        {
        }

        #endregion
    }

    /// <summary>
    /// 用户组项
    /// </summary>
    public class GroupElement : DataElement
    {
        #region 构造函数

        public GroupElement()
            : base()
        {
        }

        public GroupElement(XmlDocument doc, XmlElement ele)
            : base(doc, ele)
        {
        }

        #endregion

        #region 属性

		public string GroupID
		{
			get {
                return this.GetAttr("GroupID"); 
            }
            set{
                this.SetAttr("GroupID", value);
            }
		}

		public string Name
		{
			get {
                return this.GetAttr("Name"); 
            }
            set{
                this.SetAttr("Name", value);
            }
        }

        public string ParentID
        {
			get {
                return this.GetAttr("ParentID"); 
            }
            set{
                this.SetAttr("ParentID", value);
            }
        }

		public string GroupPath
		{
			get {
                return this.GetAttr("GroupPath"); 
            }
            set{
                this.SetAttr("GroupPath", value);
            }
		}

		public int? GroupType
        {
            get
            {
                try { return Convert.ToInt32(this.GetAttr("Type")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("Type", value.ToString());
            }
		}

		public byte? State
        {
            get
            {
                try { return Convert.ToByte(this.GetAttr("State")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("State", value.ToString());
            }
		}

		public int? SortIndex
        {
            get
            {
                try { return Convert.ToInt32(this.GetAttr("SortIndex")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("SortIndex", value.ToString());
            }
		}

		public DateTime? LastModifiedDate
        {
            get
            {
                try { return Convert.ToDateTime(this.GetAttr("LastModifiedDate")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("LastModifiedDate", value.ToString());
            }
		}

		public DateTime? CreateDate
        {
            get
            {
                try { return Convert.ToDateTime(this.GetAttr("CreateDate")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("CreateDate", value.ToString());
            }
		}
		
		#endregion
    }

    /// <summary>
    /// 用户组信息
    /// </summary>
    public class GroupInfo : DataCollection
    {
        #region 构造函数

        public GroupInfo()
            : base()
        {}

        public GroupInfo(string xmlsrc)
            : base(xmlsrc)
        {
        }

        public GroupInfo(XmlElement ele)
            : base(ele)
        {
        }

        public GroupInfo(DataCollection dc)
            : base(dc.ToString())
        {
        }

        #endregion
    }

    /// <summary>
    /// 用户角色项
    /// </summary>
    public class RoleElement : DataElement
    {
        #region 构造函数

        public RoleElement()
            : base()
        {
        }

        public RoleElement(XmlDocument doc, XmlElement ele)
            : base(doc, ele)
        {
        }

        #endregion

        #region 属性

        public string RoleID
        {
            get
            {
                return this.GetAttr("RoleID");
            }
            set
            {
                this.SetAttr("RoleID", value);
            }
        }

        public string Name
        {
            get
            {
                return this.GetAttr("Name");
            }
            set
            {
                this.SetAttr("Name", value);
            }
        }

        public string Description
        {
            get
            {
                return this.GetAttr("Description");
            }
            set
            {
                this.SetAttr("Description", value);
            }
        }

        public int? SortIndex
        {
            get
            {
                try { return Convert.ToInt32(this.GetAttr("SortIndex")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("RoleID", value.ToString());
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                try { return Convert.ToDateTime(this.GetAttr("LastModifiedDate")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("LastModifiedDate", value.ToString());
            }
        }

        public DateTime? CreateDate
        {
            get
            {
                try { return Convert.ToDateTime(this.GetAttr("CreateDate")); }
                catch { return null; }
            }
            set
            {
                this.SetAttr("CreateDate", value.ToString());
            }
        }

        #endregion
    }

    /// <summary>
    /// 用户角色信息
    /// </summary>
    public class RoleInfo : DataCollection
    {
        #region 构造函数

        public RoleInfo()
            : base()
        {}

        public RoleInfo(string xmlsrc)
            : base(xmlsrc)
        {
        }

        public RoleInfo(XmlElement ele)
            : base(ele)
        {
        }

        public RoleInfo(DataCollection dc)
            : base(dc.ToString())
        {
        }

        #endregion
    }
}
