using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using PIC.Data;
using PIC.Doc;
	
namespace PIC.Portal.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
	public partial class Message
    {
        #region Consts & Enums

        public const string AttachDirCode = "SYS.Msg.Attachment";

        /// <summary>
        /// 消息状态
        /// </summary>
        public enum StatusEnum
        {
            New,    // 新消息
            Sent,   // 已发送消息
            Readed, // 已读
            Active, // 活动的（针对弹出消息）
            Extinct,// 失活的（针对弹出消息）
            Unknown // 状态未知
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        public enum TypeEnum
        {
            Draft,  // 草稿
            Received,   // 收件
            Sent,   // 发件
            Unknow  // 未知状态
        }

        /// <summary>
        /// 消息系统类型
        /// </summary>
        public enum SysTypeEnum
        {
            Message,  // 一般消息
            Popup,  // 弹出消息
            Task,   // 任务
            PubInfo,   // 公共信息
            Unknow  // 未知种类
        }

        /// <summary>
        /// 消息发送类型
        /// </summary>
        public enum SendTypeEnum
        {
            Send,   // 一般发送
            Reply,  // 回复
            Forward,  // 转发
            Unknow  // 未知
        }

        /// <summary>
        /// 消息所有者类型
        /// </summary>
        public enum OwnerTypeEnum
        {
            User,  // 个人消息
            Group,   // 组消息
            System, // 系统消息
            Unknow  // 未知
        }

        /// <summary>
        /// 消息重要度
        /// </summary>
        public enum ImportantEnum
        {
            Normal,  // 普通消息
            Important,   // 重要消息
            Unknow  // 未知
        }

        #endregion

        #region 成员变量

        [NonSerialized]
        protected MessageTag messageTag;

        #endregion

        #region 成员属性

        /// <summary>
        /// 枚举消息类型
        /// </summary>
        [JsonIgnore]
        public TypeEnum MessageType
        {
            get { return CLRHelper.GetEnum<TypeEnum>(this.Type); }
            set { this.Type = value.ToString(); }
        }

        /// <summary>
        /// 枚举消息系统类型
        /// </summary>
        [JsonIgnore]
        public SysTypeEnum MessageSysType
        {
            get { return CLRHelper.GetEnum<SysTypeEnum>(this.SysType); }
            set { this.SysType = value.ToString(); }
        }

        /// <summary>
        /// 枚举消息发送类型
        /// </summary>
        [JsonIgnore]
        public SendTypeEnum MessageSendType
        {
            get { return CLRHelper.GetEnum<SendTypeEnum>(this.SendType, SendTypeEnum.Send); }
            set { this.SendType = value.ToString(); }
        }

        /// <summary>
        /// 枚举消息类型
        /// </summary>
        [JsonIgnore]
        public OwnerTypeEnum MessageOwnerType
        {
            get { return CLRHelper.GetEnum<OwnerTypeEnum>(this.OwnerType); }
            set { this.Type = value.ToString(); }
        }

        /// <summary>
        /// 枚举消息状态
        /// </summary>
        [JsonIgnore]
        public StatusEnum MessageStatus
        {
            get { return CLRHelper.GetEnum<StatusEnum>(this.Status); }
            set { this.Status = value.ToString(); }
        }

        /// <summary>
        /// 枚举消息状态
        /// </summary>
        [JsonIgnore]
        public ImportantEnum MessageImportant
        {
            get { return CLRHelper.GetEnum<ImportantEnum>(this.Important); }
            set { this.Important = value.ToString(); }
        }

        /// <summary>
        /// 流程活动状态
        /// </summary>
        [JsonIgnore]
        public MessageTag MessageTag
        {
            get
            {
                if (messageTag == null)
                {
                    if (String.IsNullOrEmpty(this.Tag))
                    {
                        messageTag = new MessageTag();
                    }
                    else
                    {
                        messageTag = JsonHelper.GetObject<MessageTag>(this.Tag);
                    }
                }

                return messageTag;
            }

            internal set { this.messageTag = value; }
        }
        
        #endregion

        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            if (String.IsNullOrEmpty(this.Subject))
            {
                throw new MessageException("标题不能为空。");
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void DoSave()
        {
            if (String.IsNullOrEmpty(MessageID))
            {
                this.DoCreate();
            }
            else
            {
                this.DoUpdate();
            }
        }

        /// <summary>
        /// 创建操作
        /// </summary>
        public void DoCreate()
        {
            this.DoValidate();

            if (String.IsNullOrEmpty(this.SysType))
            {
                this.MessageSysType = SysTypeEnum.Message;
            }

            if (String.IsNullOrEmpty(this.Type))
            {
                this.MessageType = TypeEnum.Draft;
            }

            if (String.IsNullOrEmpty(this.Status))
            {
                this.MessageStatus = StatusEnum.New;
            }

            this.CreatedDate = DateTime.Now;

            var fileObjData = DocService.GetFileObjectData(this.Attachments);

            // 归档附件
            if (fileObjData != null)
            {
                var newFileObjData = DocService.ArchiveFileObjectData(fileObjData, AttachDirCode, false);

                this.Attachments = DocService.GetFileObjectDataStr(newFileObjData);
            }

            // 事务开始
            this.CreateAndFlush();
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            this.DoValidate();

            this.LastModifiedDate = DateTime.Now;

            this.UpdateAndFlush();
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            // 同时删除附件
            var fileObjData = DocService.GetFileObjectData(this.Attachments);

            // 归档附件
            if (fileObjData != null)
            {
                DocService.Delete(fileObjData);
            }

            this.DeleteAndFlush();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        [ActiveRecordTransaction]
        public void DoSend()
        {
            // 设置组名
            if (String.IsNullOrEmpty(this.GroupID))
            {
                if (!String.IsNullOrEmpty(this.RefID))
                {
                    var refMsg = Message.Find(this.RefID);
                    if (refMsg != null)
                    {
                        this.GroupID = refMsg.GroupID;
                    }
                }
            }

            if (String.IsNullOrEmpty(this.GroupID))
            {
                this.GroupID = DataHelper.NewCombId().ToString();
            }

            string[] toIDs;

            if (String.IsNullOrEmpty(this.FromID))
            {
                throw new MessageException("发送人不能为空。");
            }
            else if (String.IsNullOrEmpty(this.ToIDs))
            {
                throw new MessageException("接收人不能为空。");
            }
            else
            {
                toIDs = this.ToIDs.Split(',', ';');
            }

            if (toIDs != null && ToIDs.Length > 0)
            {
                if (this.OwnerID == OrgUser.SYSTEM_USER_ID)
                {
                    this.MessageOwnerType = OwnerTypeEnum.System;
                }

                // 邮件未保存
                if (String.IsNullOrEmpty(this.MessageID))
                {
                    this.OwnerID = this.FromID;
                    this.OwnerName = this.FromName;

                    this.MessageType = TypeEnum.Sent;
                    this.MessageStatus = StatusEnum.New;

                    this.SentDate = DateTime.Now;

                    this.DoCreate();
                }
                else
                {
                    this.OwnerID = this.FromID;
                    this.OwnerName = this.FromName;

                    this.MessageType = TypeEnum.Sent;
                    this.MessageStatus = StatusEnum.New;

                    this.SentDate = DateTime.Now;

                    this.DoUpdate();
                }

                OrgUser[] users = OrgUser.FindAllByPrimaryKeys(toIDs);

                foreach (OrgUser tuser in users)
                {
                    SingleSend(tuser);
                }
            }
        }

        /// <summary>
        /// 单个发送消息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected Message SingleSend(OrgUser user)
        {
            Message msg = this.GetCopy() as Message;

            msg.OwnerID = user.UserID;
            msg.OwnerName = user.Name;

            msg.SentDate = DateTime.Now;
            msg.MessageType = TypeEnum.Received;
            msg.MessageSysType = SysTypeEnum.Message;
            msg.MessageStatus = StatusEnum.New;

            msg.DoCreate();

            return msg;
        }

        /// <summary>
        /// 如果为弹出消息，这里执行弹出操作
        /// </summary>
        protected void DoPopup()
        {
            if (this.MessageSysType == SysTypeEnum.Popup
                && this.MessageStatus == StatusEnum.Active)
            {
                this.MessageTag.PopupCount = this.messageTag.PopupCount - 1;

                if (this.messageTag.PopupCount <= 0)
                {
                    this.MessageStatus = StatusEnum.Extinct;
                }

                this.DoUpdate();
            }
        }

        #endregion
        
        #region 静态成员

        /// <summary>
        /// 获取当前账户所有有效弹出消息
        /// </summary>
        [ActiveRecordTransaction]
        public static void GetPopup()
        {
            IList<Message> msgs = Message.FindAllByProperties(
                Message.Prop_OwnerID, PortalService.CurrentUserInfo.UserID,
                Message.Prop_Status, "Active");

        }
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            Message[] tents = Message.FindAll(Expression.In("MessageID", args));

			foreach (Message tent in tents)
			{
				tent.DoDelete();
			}
        }

        /// <summary>
        /// 消息发送操作
        /// </summary>
        /// <param name="messageId"></param>
        public static void SendAll(string userId)
        {
            Message[] msg = Message.FindAllByProperties(Prop_OwnerID, userId, Prop_Type, "Draft");

            msg.All((ent) =>
            {
                ent.DoSend();

                return true;
            });
        }

        /// <summary>
        /// 发送系统消息
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="attachments"></param>
        /// <param name="toIds"></param>
        public static void SysSend(string subject, string content, string link, string attachments, params string[] toIds)
        {
            Send(OrgUser.SYSTEM_USER_ID, subject, content, link, attachments, toIds);
        }

        /// <summary>
        /// 发送消息到指定组
        /// </summary>
        [ActiveRecordTransaction]
        public static void SysSendTo(string groupCode, string roleCode, string subject, string content, string link, string attachments)
        {
            var group = OrgGroup.Get(groupCode);
            var usrList = group.GetUserListByRoleCode(roleCode);

            Send(OrgUser.SystemUser, subject, content, link, attachments, usrList.ToArray());
        }

        /// <summary>
        /// 消息发送操作
        /// </summary>
        public static void Send(OrgUser fromUser, string subject, string content, string link, string attachments, params OrgUser[] toUsers)
        {
            if (fromUser == null)
            {
                throw new MessageException("发送人不能为空");
            }

            if (String.IsNullOrEmpty(subject))
            {
                throw new MessageException("主题不能为空");
            }

            if (toUsers.Length <= 0)
            {
                throw new MessageException("接收人不能为空");
            }

            string toIdsStr = toUsers.Select(ent => ent.UserID).Join();
            string toNamesStr = toUsers.Select(ent => ent.Name).Join();

            Message msg = new Message();
            msg.FromID = fromUser.UserID;
            msg.FromName = fromUser.Name;

            msg.ToIDs = toIdsStr;
            msg.ToNames = toNamesStr;

            msg.Subject = subject;
            msg.Type = "Normal";
            msg.MessageTag.Link = link;
            msg.Content = content;
            msg.Attachments = attachments;

            msg.DoSend();
        }

        /// <summary>
        /// 消息发送操作
        /// </summary>
        public static void Send(string fromId, string subject, string content, string link, string attachments, params string[] toIds)
        {
            OrgUser fromUser = null;

            if (StringHelper.IsEqualsIgnoreCase(fromId, OrgUser.SYSTEM_USER_ID))
            {
                fromUser = OrgUser.SystemUser;
            }
            else
            {
                fromUser = OrgUser.Find(fromId);
            }

            IList<string> toDistIds = toIds.Where(tid => !String.IsNullOrWhiteSpace(tid)).Distinct().ToList();

            OrgUser[] toUsers = OrgUser.FindAll(Expression.In(OrgUser.Prop_UserID, toIds));

            Send(fromUser, subject, content, link, attachments, toUsers);
        }

        /// <summary>
        /// 由所有人，消息类型获取指定消息
        /// </summary>
        /// <returns></returns>
        public static IList<Message> Get(string ownerId, TypeEnum type)
        {
            IList<Message> msg = Message.FindAllByProperties(
                Prop_OwnerID, ownerId,
                Prop_Type, type.ToString());

            return msg;
        }

        /// <summary>
        /// 由所有人，消息类型，消息状态获取指定消息
        /// </summary>
        /// <returns></returns>
        public static IList<Message> GetTasks(string ownerId, TypeEnum type, StatusEnum status)
        {
            IList<Message> msg = Message.FindAll(
                Expression.Eq(Prop_OwnerID, ownerId),
                Expression.Eq(Prop_Type, type.ToString()),
                Expression.Eq(Prop_SysType, SysTypeEnum.Task.ToString()),
                Expression.Eq(Prop_Status, status.ToString()));

            return msg;
        }

        /// <summary>
        /// 由所有人，消息类型，消息状态获取指定消息
        /// </summary>
        /// <returns></returns>
        public static IList<Message> Get(string ownerId, TypeEnum type, StatusEnum status)
        {
            IList<Message> msg = Message.FindAll(
                Expression.Eq(Prop_OwnerID, ownerId),
                Expression.Eq(Prop_Type, type.ToString()),
                Expression.Not(Expression.Eq(Prop_SysType, SysTypeEnum.Task.ToString())),
                Expression.Eq(Prop_Status, status.ToString()));

            return msg;
        }

        /// <summary>
        /// 由所有人，消息类型，消息状态获取指定消息
        /// </summary>
        /// <returns></returns>
        public static IList<Message> Get(string ownerId, TypeEnum type, SysTypeEnum sysType, StatusEnum status)
        {
            IList<Message> msg = Message.FindAllByProperties(
                Prop_OwnerID, ownerId,
                Prop_Type, type.ToString(),
                Prop_SysType, sysType.ToString(),
                Prop_Status, status.ToString());

            return msg;
        }

        #endregion

    } // SysMessage
}


