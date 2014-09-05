using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft;
using NHibernate.Criterion;
using PIC.Portal.Model;

namespace PIC.Portal.Workflow
{
    public class WfActor
    {
        #region 枚举

        public enum TypeEnum
        {
            Auto,
            User,
            Role,
            Group,
            Func
        }

        #endregion

        #region 属性

        public string UserIds { get; set; }

        public string UserCode { get; set; }

        public string FuncCode { get; set; }

        public string RoleId { get; set; }

        public string RoleCode { get; set; }

        public string GroupId { get; set; }

        public string GroupCode { get; set; }

        /// <summary>
        /// 执行人类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 执行人类型
        /// </summary>
        public TypeEnum ActorType
        {
            get { return CLRHelper.GetEnum<TypeEnum>(this.Type, TypeEnum.Auto); }
            set { this.Type = value.ToString(); }
        }

        public EasyDictionary Tag
        {
            get;
            set;
        }

        #endregion

        #region 构造函数

        public WfActor()
            : this(TypeEnum.Auto)
        {
        }

        public WfActor(TypeEnum type)
        {
            this.ActorType = type;

            Tag = new EasyDictionary();
        }

        #endregion

        #region 共有方法

        public IList<OrgUser> GetUserList(FlowContext ctx = null)
        {
            return (new WfActorHelper()).GetUserList(this, ctx);
        }

        #endregion
    }

    [CollectionDataContract]
    public class WfActorCollection : Collection<WfActor>
    {
        #region 构造函数

        public WfActorCollection()
            : base()
        {
        }

        #endregion

        #region 方法

        public IList<OrgUser> GetUserList(FlowContext ctx = null)
        {
            return (new WfActorHelper()).GetUserList(this, ctx);
        }

        /// <summary>
        /// 根据用户ID, 添加执行人
        /// </summary>
        public WfActor AddUsersByID(params string[] usrids)
        {
            if (usrids.Length > 0)
            {
                var actor = new WfActor(WfActor.TypeEnum.User);
                actor.Tag.Add("Ids", usrids.Join());

                this.Add(actor);

                return actor;
            }

            return null;
        }

        /// <summary>
        /// 根据用户编号, 添加执行人
        /// </summary>
        public WfActor AddUsersByCode(params string[] usrcodes)
        {
            if (usrcodes.Length > 0)
            {
                var actor = new WfActor(WfActor.TypeEnum.User);
                actor.Tag.Add("WorkNos", usrcodes.Join());

                return actor;
            }

            return null;
        }

        /// <summary>
        /// 根据用户ID, 添加执行人
        /// </summary>
        public WfActor SetUsersByID(params string[] usrids)
        {
            this.Clear();

            return this.AddUsersByID(usrids);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="usrs"></param>
        public WfActor AddUsers(params OrgUser[] usrs)
        {
            if (usrs.Length > 0)
            {
                var usrids = usrs.Select(u => u.UserID).ToArray();

                return this.AddUsersByID(usrids);
            }

            return null;
        }

        /// <summary>
        /// 根据用户编号, 添加执行人
        /// </summary>
        public WfActor SetUsersByCode(params string[] usrcodes)
        {
            this.Clear();

            return this.AddUsersByCode(usrcodes);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="usrs"></param>
        public WfActor SetUsers(params OrgUser[] usrs)
        {
            this.Clear();

            return this.AddUsers(usrs);
        }

        #endregion
    }
}
