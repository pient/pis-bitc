// Business class SysMessage generated from SysMessage
// Creator: Ray
// Created Date: [2012-05-02]

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
    [ActiveRecord("Message")]
	public partial class Message : ModelBase<Message>
	{
		#region Property_Names

        public static string Prop_MessageID = "MessageID";
        public static string Prop_GroupID = "GroupID";
        public static string Prop_RefID = "RefID";
		public static string Prop_Type = "Type";
        public static string Prop_SysType = "SysType";
        public static string Prop_SendType = "SendType";
		public static string Prop_Catalog = "Catalog";
		public static string Prop_Important = "Important";
		public static string Prop_Subject = "Subject";
		public static string Prop_Content = "Content";
		public static string Prop_MessageChain = "MessageChain";
		public static string Prop_Attachments = "Attachments";
		public static string Prop_OwnerID = "OwnerID";
		public static string Prop_OwnerName = "OwnerName";
		public static string Prop_OwnerType = "OwnerType";
		public static string Prop_FromID = "FromID";
		public static string Prop_FromName = "FromName";
		public static string Prop_ToIDs = "ToIDs";
		public static string Prop_ToNames = "ToNames";
		public static string Prop_CCIDs = "CCIDs";
		public static string Prop_CCNames = "CCNames";
		public static string Prop_Status = "Status";
        public static string Prop_Tag = "Tag";
        public static string Prop_ReadedCount = "ReadedCount";
        public static string Prop_SentDate = "SentDate";
        public static string Prop_ExpiredDate = "ExpiredDate";
		public static string Prop_CreatedDate = "CreatedDate";
		public static string Prop_LastModifiedDate = "LastModifiedDate";

		#endregion

		#region Private_Variables

        private string _messageid;
        private string _groupID;
        private string _refID;
		private string _type;
        private string _sysType;
        private string _sendType;
		private string _catalog;
		private string _important;
		private string _subject;
		private string _content;
		private string _messageChain;
		private string _attachments;
		private string _ownerID;
		private string _ownerName;
		private string _ownerType;
		private string _fromID;
		private string _fromName;
		private string _toIDs;
		private string _toNames;
		private string _cCIDs;
		private string _cCNames;
		private string _status;
        private string _tag;
        private int? _readedCount;
        private DateTime? _sentDate;
        private DateTime? _expiredDate;
		private DateTime? _createdDate;
		private DateTime? _lastModifiedDate;


		#endregion

		#region Constructors

		public Message()
		{
		}

		public Message(
			string p_messageid,
			string p_type,
			string p_sysType,
			string p_catalog,
			string p_important,
			string p_subject,
			string p_content,
			string p_messageChain,
			string p_attachments,
			string p_ownerID,
			string p_ownerName,
			string p_ownerType,
			string p_fromID,
			string p_fromName,
			string p_toIDs,
			string p_toNames,
			string p_cCIDs,
			string p_cCNames,
			string p_status,
			string p_tag,
			DateTime? p_sentDate,
			DateTime? p_createdDate,
			DateTime? p_lastModifiedDate)
		{
			_messageid = p_messageid;
			_type = p_type;
			_sysType = p_sysType;
			_catalog = p_catalog;
			_important = p_important;
			_subject = p_subject;
			_content = p_content;
			_messageChain = p_messageChain;
			_attachments = p_attachments;
			_ownerID = p_ownerID;
			_ownerName = p_ownerName;
			_ownerType = p_ownerType;
			_fromID = p_fromID;
			_fromName = p_fromName;
			_toIDs = p_toIDs;
			_toNames = p_toNames;
			_cCIDs = p_cCIDs;
			_cCNames = p_cCNames;
			_status = p_status;
			_tag = p_tag;
			_sentDate = p_sentDate;
			_createdDate = p_createdDate;
			_lastModifiedDate = p_lastModifiedDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("MessageID", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public string MessageID
		{
			get { return _messageid; }
			set { _messageid = value; }

		}

        [Property("GroupID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
        public string GroupID
        {
            get { return _groupID; }
            set
            {
                if ((_groupID == null) || (value == null) || (!value.Equals(_groupID)))
                {
                    object oldValue = _groupID;
                    _groupID = value;
                    RaisePropertyChanged(Message.Prop_GroupID, oldValue, value);
                }
            }

        }

        [Property("RefID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
        public string RefID
        {
            get { return _refID; }
            set
            {
                if ((_refID == null) || (value == null) || (!value.Equals(_refID)))
                {
                    object oldValue = _refID;
                    _refID = value;
                    RaisePropertyChanged(Message.Prop_RefID, oldValue, value);
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
					RaisePropertyChanged(Message.Prop_Type, oldValue, value);
				}
			}

		}

		[Property("SysType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string SysType
		{
			get { return _sysType; }
			set
			{
				if ((_sysType == null) || (value == null) || (!value.Equals(_sysType)))
				{
                    object oldValue = _sysType;
					_sysType = value;
					RaisePropertyChanged(Message.Prop_SysType, oldValue, value);
				}
			}

		}

        [Property("SendType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
        public string SendType
        {
            get { return _sendType; }
            set
            {
                if ((_sendType == null) || (value == null) || (!value.Equals(_sendType)))
                {
                    object oldValue = _sendType;
                    _sendType = value;
                    RaisePropertyChanged(Message.Prop_SendType, oldValue, value);
                }
            }

        }

		[Property("Catalog", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 150)]
		public string Catalog
		{
			get { return _catalog; }
			set
			{
				if ((_catalog == null) || (value == null) || (!value.Equals(_catalog)))
				{
                    object oldValue = _catalog;
					_catalog = value;
					RaisePropertyChanged(Message.Prop_Catalog, oldValue, value);
				}
			}

		}

		[Property("Important", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string Important
		{
			get { return _important; }
			set
			{
				if ((_important == null) || (value == null) || (!value.Equals(_important)))
				{
                    object oldValue = _important;
					_important = value;
					RaisePropertyChanged(Message.Prop_Important, oldValue, value);
				}
			}

		}

		[Property("Subject", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public string Subject
		{
			get { return _subject; }
			set
			{
				if ((_subject == null) || (value == null) || (!value.Equals(_subject)))
				{
                    object oldValue = _subject;
					_subject = value;
					RaisePropertyChanged(Message.Prop_Subject, oldValue, value);
				}
			}

		}

		[Property("Content", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Content
		{
			get { return _content; }
			set
			{
				if ((_content == null) || (value == null) || (!value.Equals(_content)))
				{
                    object oldValue = _content;
					_content = value;
					RaisePropertyChanged(Message.Prop_Content, oldValue, value);
				}
			}

		}

		[Property("MessageChain", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 5000)]
		public string MessageChain
		{
			get { return _messageChain; }
			set
			{
				if ((_messageChain == null) || (value == null) || (!value.Equals(_messageChain)))
				{
                    object oldValue = _messageChain;
					_messageChain = value;
					RaisePropertyChanged(Message.Prop_MessageChain, oldValue, value);
				}
			}

		}

		[Property("Attachments", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 1500)]
		public string Attachments
		{
			get { return _attachments; }
			set
			{
				if ((_attachments == null) || (value == null) || (!value.Equals(_attachments)))
				{
                    object oldValue = _attachments;
					_attachments = value;
					RaisePropertyChanged(Message.Prop_Attachments, oldValue, value);
				}
			}

		}

		[Property("OwnerID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string OwnerID
		{
			get { return _ownerID; }
			set
			{
				if ((_ownerID == null) || (value == null) || (!value.Equals(_ownerID)))
				{
                    object oldValue = _ownerID;
					_ownerID = value;
					RaisePropertyChanged(Message.Prop_OwnerID, oldValue, value);
				}
			}

		}

		[Property("OwnerName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string OwnerName
		{
			get { return _ownerName; }
			set
			{
				if ((_ownerName == null) || (value == null) || (!value.Equals(_ownerName)))
				{
                    object oldValue = _ownerName;
					_ownerName = value;
					RaisePropertyChanged(Message.Prop_OwnerName, oldValue, value);
				}
			}

		}

		[Property("OwnerType", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string OwnerType
		{
			get { return _ownerType; }
			set
			{
				if ((_ownerType == null) || (value == null) || (!value.Equals(_ownerType)))
				{
                    object oldValue = _ownerType;
					_ownerType = value;
					RaisePropertyChanged(Message.Prop_OwnerType, oldValue, value);
				}
			}

		}

		[Property("FromID", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string FromID
		{
			get { return _fromID; }
			set
			{
				if ((_fromID == null) || (value == null) || (!value.Equals(_fromID)))
				{
                    object oldValue = _fromID;
					_fromID = value;
					RaisePropertyChanged(Message.Prop_FromID, oldValue, value);
				}
			}

		}

		[Property("FromName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public string FromName
		{
			get { return _fromName; }
			set
			{
				if ((_fromName == null) || (value == null) || (!value.Equals(_fromName)))
				{
                    object oldValue = _fromName;
					_fromName = value;
					RaisePropertyChanged(Message.Prop_FromName, oldValue, value);
				}
			}

		}

		[Property("ToIDs", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 3600)]
		public string ToIDs
		{
			get { return _toIDs; }
			set
			{
				if ((_toIDs == null) || (value == null) || (!value.Equals(_toIDs)))
				{
                    object oldValue = _toIDs;
					_toIDs = value;
					RaisePropertyChanged(Message.Prop_ToIDs, oldValue, value);
				}
			}

		}

		[Property("ToNames", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 3600)]
		public string ToNames
		{
			get { return _toNames; }
			set
			{
				if ((_toNames == null) || (value == null) || (!value.Equals(_toNames)))
				{
                    object oldValue = _toNames;
					_toNames = value;
					RaisePropertyChanged(Message.Prop_ToNames, oldValue, value);
				}
			}

		}

		[Property("CCIDs", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 3600)]
		public string CCIDs
		{
			get { return _cCIDs; }
			set
			{
				if ((_cCIDs == null) || (value == null) || (!value.Equals(_cCIDs)))
				{
                    object oldValue = _cCIDs;
					_cCIDs = value;
					RaisePropertyChanged(Message.Prop_CCIDs, oldValue, value);
				}
			}

		}

		[Property("CCNames", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 3600)]
		public string CCNames
		{
			get { return _cCNames; }
			set
			{
				if ((_cCNames == null) || (value == null) || (!value.Equals(_cCNames)))
				{
                    object oldValue = _cCNames;
					_cCNames = value;
					RaisePropertyChanged(Message.Prop_CCNames, oldValue, value);
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
					RaisePropertyChanged(Message.Prop_Status, oldValue, value);
				}
			}

		}

		[Property("Tag", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public string Tag
		{
			get { return _tag; }
			set
			{
				if ((_tag == null) || (value == null) || (!value.Equals(_tag)))
				{
                    object oldValue = _tag;
					_tag = value;
					RaisePropertyChanged(Message.Prop_Tag, oldValue, value);
				}
			}

		}

        [Property("ReadedCount", Access = PropertyAccess.NosetterCamelcaseUnderscore, Default="0")]
        public int ReadedCount
        {
            get { return _readedCount ?? 0; }
            set
            {
                if ((_readedCount == null) || (!value.Equals(_readedCount)))
                {
                    object oldValue = _readedCount;
                    _readedCount = value;
                    RaisePropertyChanged(Message.Prop_ReadedCount, oldValue, value);
                }
            }

        }

		[Property("SentDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? SentDate
		{
			get { return _sentDate; }
			set
			{
				if (value != _sentDate)
				{
                    object oldValue = _sentDate;
					_sentDate = value;
					RaisePropertyChanged(Message.Prop_SentDate, oldValue, value);
				}
			}

		}

        [Property("ExpiredDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
        public DateTime? ExpiredDate
        {
            get { return _expiredDate; }
            set
            {
                if (value != _expiredDate)
                {
                    object oldValue = _expiredDate;
                    _expiredDate = value;
                    RaisePropertyChanged(Message.Prop_ExpiredDate, oldValue, value);
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
					RaisePropertyChanged(Message.Prop_CreatedDate, oldValue, value);
				}
			}

		}

		[Property("LastModifiedDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public DateTime? LastModifiedDate
		{
			get { return _lastModifiedDate; }
			set
			{
				if (value != _lastModifiedDate)
				{
                    object oldValue = _lastModifiedDate;
					_lastModifiedDate = value;
					RaisePropertyChanged(Message.Prop_LastModifiedDate, oldValue, value);
				}
			}

		}

		#endregion
	} // SysMessage
}

