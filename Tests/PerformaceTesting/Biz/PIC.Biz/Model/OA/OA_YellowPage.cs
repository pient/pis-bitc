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
using PIC.Common;
using PIC.Data;
using PIC.Portal;
	
namespace PIC.Biz.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
    public partial class OA_YellowPage : BizTreeNodeModelBase<OA_YellowPage>
    {
        #region 成员变量

        #endregion

        #region 重载

        /// <summary>
        /// 创建操作
        /// </summary>
        protected override void DoCreate()
        {
            this.DoValidate();

            this.CreatorId = UserInfo.UserID;
            this.CreatorName = UserInfo.Name;
            this.CreatedDate = DateTime.Now;

            // 事务开始
            base.DoCreate();

        }

        /// <summary>
        /// 更新操作
        /// </summary>
        public override void Update()
        {
            this.DoValidate();

            this.LastModifiedDate = DateTime.Now;

            base.Update();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public virtual void DoValidate()
        {
            // 检查是否存在重复键
            /*if (!this.IsPropertyUnique("Code"))
            {
                throw new RepeatedKeyException("存在重复的编码 “" + this.Code + "”");
            }*/
        }

        #endregion

        #region 静态成员

        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            OA_YellowPage[] tents = OA_YellowPage.FindAll(Expression.In(OA_YellowPage.Prop_Id, args));

            foreach (OA_YellowPage tent in tents)
            {
                tent.Delete();
            }
        }

        /// <summary>
        /// 批量粘贴操作
        /// </summary>
        /// <param name="patype">sib, sub</param>
        /// <param name="targetId"></param>
        /// <param name="args"></param>
        public static void DoPaste(PasteDataSourceEnum pdstype, PasteAsEnum patype, string targetId, params object[] pasteIds)
        {
            if (!String.IsNullOrEmpty(targetId) && pasteIds.Length > 0)
            {
                IList<OA_YellowPage> allnodes = OA_YellowPage.FindAllByPrimaryKeys(pasteIds);
                IList<OA_YellowPage> nodes = FilterChildNodes(allnodes);

                // 只提取最高节点或无父子关联的节点进行粘贴
                foreach (OA_YellowPage tnode in nodes)
                {
                    switch (pdstype)
                    {
                        case PasteDataSourceEnum.Copy:
                            if (patype == PasteAsEnum.Sibling)
                            {
                                tnode.CopyAsSibling(targetId);
                            }
                            else if (patype == PasteAsEnum.Child)
                            {
                                tnode.CopyAsChild(targetId);
                            }
                            break;
                        case PasteDataSourceEnum.Cut:
                            if (patype == PasteAsEnum.Sibling)
                            {
                                tnode.MoveAsSibling(targetId);
                            }
                            else if (patype == PasteAsEnum.Child)
                            {
                                if (tnode.ParentID == targetId)
                                {
                                    tnode.ChangePosition(0);
                                }
                                else
                                {
                                    tnode.MoveAsChild(targetId);
                                }
                            }
                            break;
                    }
                }
            }
        }
        
        #endregion

    } // OA_YellowPage
}


