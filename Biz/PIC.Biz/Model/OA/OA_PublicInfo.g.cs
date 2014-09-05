// Business class OA_PublicInfo generated from OA_PublicInfo
// Creator: Ray
// Created Date: [2012-04-30]

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
using PIC.Portal;
using PIC.Portal.Model;

namespace PIC.Biz.Model
{
	[ActiveRecord("OA_PublicInfo")]
	public partial class OA_PublicInfo : BizModelBase<OA_PublicInfo>
	{
		#region Property_Names

		public static string Prop_Id = "Id";
		public static string Prop_Code = "Code";
		public static string Prop_Title = "Title";
		public static string Prop_Type = "Type";
		public static string Prop_Keywords = "Keywords";
		public static string Prop_AuthorId = "AuthorId";
		public static string Prop_AuthorName = "AuthorName";
		public static string Prop_Content = "Content";
		public static string Prop_Grade = "Grade";
		public static string Prop_PublisherId = "PublisherId";
		public static string Prop_PublisherName = "PublisherName";
		public static string Prop_GroupId = "GroupId";
		public static string Prop_GroupName = "GroupName";
		public static string Prop_Status = "Status";
		public static string Prop_Clicks = "Clicks";
		public static string Prop_Picture = "Picture";
		public static string Prop_Attachment = "Attachment";
		public static string Prop_SortIndex = "SortIndex";
		public static string Prop_IsPopup = "IsPopup";
		public static string Prop_IsExpired = "IsExpired";
		public static string Prop_PopupEndDate = "PopupEndDate";
		public static string Prop_PublishDate = "PublishDate";
		public static string Prop_ExpireDate = "ExpireDate";
		public static string Prop_LastModifiedDate = "LastModifiedDate";
		public static string Prop_CreatedDate = "CreatedDate";

		#endregion

		#region Private_Variables

		private string _id;
		private string _code;
		private string _title;
		private string _type;
		private string _keywords;
		private string _authorId;
		private string _authorName;
		private string _content;
		private string _grade;
		private string _publisherId;
		private string _publisherName;
		private string _groupId;
		private string _groupName;
		private string _status;
		private int? _clicks;
		private string _picture;
		private string _attachment;
		private int? _sortIndex;
		private string _isPopup;
		private string _isExpired;
		private DateTime? _popupEndDate;
		private DateTime? _publishDate;
		private DateTime? _expireDate;
		private DateTime? _lastModifiedDate;
		private DateTime? _createdDate;


		#endregion

		#region Constructors

		public OA_PublicInfo()
		{
		}

		public OA_PublicInfo(
			string p_id,
			string p_code,
			string p_title,
			string p_type,
			string p_keywords,
			string p_authorId,
			string p_authorName,
			string p_content,
			string p_grade,
			string p_publisherId,
			string p_publisherName,
			string p_groupId,
			string p_groupName,
			string p_status,
			int? p_clicks,
			string p_picture,
			string p_attachment,
			int? p_sortIndex,
			string p_isPopup,
			string p_isExpired,
			DateTime? p_popupEndDate,
			DateTime? p_publishDate,
			DateTime? p_expireDate,
			DateTime? p_lastModifiedDate,
			DateTime? p_createdDate)
		{
			_id = p_id;
			_code = p_code;
			_title = p_title;
			_type = p_type;
			_keywords = p_keywords;
			_authorId = p_authorId;
			_authorName = p_authorName;
			_content = p_content;
			_grade = p_grade;
			_publisherId = p_publisherId;
			_publisherName = p_publisherName;
			_groupId = p_groupId;
			_groupName = p_groupName;
			_status = p_status;
			_clicks = p_clicks;
			_picture = p_picture;
			_attachment = p_attachment;
			_sortIndex = p_sortIndex;
			_isPopup = p_isPopup;
			_isExpired = p_isExpired;
			_popupEndDate = p_popupEndDate;
			_publishDate = p_publishDate;
			_expireDate = p_expireDate;
			_lastModifiedDate = p_lastModifiedDate;
			_createdDate = p_createdDate;
		}

		#endregion

		#region Properties

		[PrimaryKey("Id", Generator = PrimaryKeyType.Custom, CustomGenerator = typeof(PICIdentifierGenerator), Access = PropertyAccess.NosetterLowercaseUnderscore)]
		public virtual string Id
		{
			get { return _id; }
			set { _id = value; }

		}

		[Property("Code", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Code
		{
			get { return _code; }
			set
			{
				if ((_code == null) || (value == null) || (!value.Equals(_code)))
				{
                    object oldValue = _code;
					_code = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_Code, oldValue, value);
				}
			}

		}

		[Property("Title", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public virtual string Title
		{
			get { return _title; }
			set
			{
				if ((_title == null) || (value == null) || (!value.Equals(_title)))
				{
                    object oldValue = _title;
					_title = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_Title, oldValue, value);
				}
			}

		}

		[Property("Type", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Type
		{
			get { return _type; }
			set
			{
				if ((_type == null) || (value == null) || (!value.Equals(_type)))
				{
                    object oldValue = _type;
					_type = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_Type, oldValue, value);
				}
			}

		}

		[Property("Keywords", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public virtual string Keywords
		{
			get { return _keywords; }
			set
			{
				if ((_keywords == null) || (value == null) || (!value.Equals(_keywords)))
				{
                    object oldValue = _keywords;
					_keywords = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_Keywords, oldValue, value);
				}
			}

		}

		[Property("AuthorId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public virtual string AuthorId
		{
			get { return _authorId; }
			set
			{
				if ((_authorId == null) || (value == null) || (!value.Equals(_authorId)))
				{
                    object oldValue = _authorId;
					_authorId = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_AuthorId, oldValue, value);
				}
			}

		}

		[Property("AuthorName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string AuthorName
		{
			get { return _authorName; }
			set
			{
				if ((_authorName == null) || (value == null) || (!value.Equals(_authorName)))
				{
                    object oldValue = _authorName;
					_authorName = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_AuthorName, oldValue, value);
				}
			}

		}

		[Property("Content", Access = PropertyAccess.NosetterCamelcaseUnderscore, ColumnType = "StringClob")]
		public virtual string Content
		{
			get { return _content; }
			set
			{
				if ((_content == null) || (value == null) || (!value.Equals(_content)))
				{
                    object oldValue = _content;
					_content = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_Content, oldValue, value);
				}
			}

		}

		[Property("Grade", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Grade
		{
			get { return _grade; }
			set
			{
				if ((_grade == null) || (value == null) || (!value.Equals(_grade)))
				{
                    object oldValue = _grade;
					_grade = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_Grade, oldValue, value);
				}
			}

		}

		[Property("PublisherId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public virtual string PublisherId
		{
			get { return _publisherId; }
			set
			{
				if ((_publisherId == null) || (value == null) || (!value.Equals(_publisherId)))
				{
                    object oldValue = _publisherId;
					_publisherId = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_PublisherId, oldValue, value);
				}
			}

		}

		[Property("PublisherName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string PublisherName
		{
			get { return _publisherName; }
			set
			{
				if ((_publisherName == null) || (value == null) || (!value.Equals(_publisherName)))
				{
                    object oldValue = _publisherName;
					_publisherName = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_PublisherName, oldValue, value);
				}
			}

		}

		[Property("GroupId", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 36)]
		public virtual string GroupId
		{
			get { return _groupId; }
			set
			{
				if ((_groupId == null) || (value == null) || (!value.Equals(_groupId)))
				{
                    object oldValue = _groupId;
					_groupId = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_GroupId, oldValue, value);
				}
			}

		}

		[Property("GroupName", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string GroupName
		{
			get { return _groupName; }
			set
			{
				if ((_groupName == null) || (value == null) || (!value.Equals(_groupName)))
				{
                    object oldValue = _groupName;
					_groupName = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_GroupName, oldValue, value);
				}
			}

		}

		[Property("Status", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string Status
		{
			get { return _status; }
			set
			{
				if ((_status == null) || (value == null) || (!value.Equals(_status)))
				{
                    object oldValue = _status;
					_status = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_Status, oldValue, value);
				}
			}

		}

		[Property("Clicks", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual int? Clicks
		{
			get { return _clicks; }
			set
			{
				if (value != _clicks)
				{
                    object oldValue = _clicks;
					_clicks = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_Clicks, oldValue, value);
				}
			}

		}

		[Property("Picture", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public virtual string Picture
		{
			get { return _picture; }
			set
			{
				if ((_picture == null) || (value == null) || (!value.Equals(_picture)))
				{
                    object oldValue = _picture;
					_picture = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_Picture, oldValue, value);
				}
			}

		}

		[Property("Attachment", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 500)]
		public virtual string Attachment
		{
			get { return _attachment; }
			set
			{
				if ((_attachment == null) || (value == null) || (!value.Equals(_attachment)))
				{
                    object oldValue = _attachment;
					_attachment = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_Attachment, oldValue, value);
				}
			}

		}

		[Property("SortIndex", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual int? SortIndex
		{
			get { return _sortIndex; }
			set
			{
				if (value != _sortIndex)
				{
                    object oldValue = _sortIndex;
					_sortIndex = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_SortIndex, oldValue, value);
				}
			}

		}

		[Property("IsPopup", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string IsPopup
		{
			get { return _isPopup; }
			set
			{
				if ((_isPopup == null) || (value == null) || (!value.Equals(_isPopup)))
				{
                    object oldValue = _isPopup;
					_isPopup = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_IsPopup, oldValue, value);
				}
			}

		}

		[Property("IsExpired", Access = PropertyAccess.NosetterCamelcaseUnderscore, Length = 50)]
		public virtual string IsExpired
		{
			get { return _isExpired; }
			set
			{
				if ((_isExpired == null) || (value == null) || (!value.Equals(_isExpired)))
				{
                    object oldValue = _isExpired;
					_isExpired = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_IsExpired, oldValue, value);
				}
			}

		}

		[Property("PopupEndDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? PopupEndDate
		{
			get { return _popupEndDate; }
			set
			{
				if (value != _popupEndDate)
				{
                    object oldValue = _popupEndDate;
					_popupEndDate = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_PopupEndDate, oldValue, value);
				}
			}

		}

		[Property("PublishDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? PublishDate
		{
			get { return _publishDate; }
			set
			{
				if (value != _publishDate)
				{
                    object oldValue = _publishDate;
					_publishDate = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_PublishDate, oldValue, value);
				}
			}

		}

		[Property("ExpireDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? ExpireDate
		{
			get { return _expireDate; }
			set
			{
				if (value != _expireDate)
				{
                    object oldValue = _expireDate;
					_expireDate = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_ExpireDate, oldValue, value);
				}
			}

		}

		[Property("LastModifiedDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? LastModifiedDate
		{
			get { return _lastModifiedDate; }
			set
			{
				if (value != _lastModifiedDate)
				{
                    object oldValue = _lastModifiedDate;
					_lastModifiedDate = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_LastModifiedDate, oldValue, value);
				}
			}

		}

		[Property("CreatedDate", Access = PropertyAccess.NosetterCamelcaseUnderscore)]
		public virtual DateTime? CreatedDate
		{
			get { return _createdDate; }
			set
			{
				if (value != _createdDate)
				{
                    object oldValue = _createdDate;
					_createdDate = value;
					RaisePropertyChanged(OA_PublicInfo.Prop_CreatedDate, oldValue, value);
				}
			}

		}

		#endregion
	} // OA_PublicInfo
}

