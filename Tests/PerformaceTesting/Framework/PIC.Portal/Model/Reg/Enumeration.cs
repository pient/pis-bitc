
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using PIC.Data;
	
namespace PIC.Portal.Model
{
    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
	public partial class Enumeration : EditSensitiveTreeNodeEntityBase<Enumeration>
    {
        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 检查是否存在重复键
            if (!this.IsPropertyUnique(Enumeration.Prop_Code))
            {
                throw new RepeatedKeyException("存在重复的编码 “" + this.Code + "”");
            }
        }

        /// <summary>
        /// 创建操作
        /// </summary>
        protected override void DoCreate()
        {
            // 编码为空而值不为空时(默认将值作为编码后缀)
            if (String.IsNullOrEmpty(this.Code)
                && !String.IsNullOrEmpty(this.Value))
            {
                if (this.Parent == null)
                {
                    this.Code = this.Value;
                }
                else
                {
                    this.Code = String.Format("{0}.{1}", this.Parent.Code, this.Value);
                }
            }

            this.DoValidate();

            this.CreatedDate = DateTime.Now;

            // 事务开始
            base.DoCreate();
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        public void DoUpdate()
        {
            this.DoValidate();

            this.LastModifiedDate = DateTime.Now;

            base.UpdateAndFlush();
        }

        /// <summary>
        /// 转换为字典(用于前台生成枚举)
        /// </summary>
        public EasyDictionary GetDict()
        {
            EasyDictionary dict = Enumeration.GetEnumDict(this.ChildNodes);

            return dict;
        }

        #endregion
        
        #region 静态成员

        /// <summary>
        /// 获取Code为pcode枚举下值为为value的枚举项
        /// </summary>
        /// <param name="pcode"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Enumeration GetByValue(string pcode, string value)
        {
            if (!String.IsNullOrEmpty(pcode))
            {
                Enumeration tenum = Enumeration.Get(pcode);

                if (tenum != null)
                {
                    if (tenum.Value == value)
                    {
                        return tenum;
                    }
                    else
                    {
                        return Enumeration.FindFirst(
                            Expression.Like(Prop_Path, tenum.ID, MatchMode.Anywhere),
                            Expression.Eq(Prop_Value, value));
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 由编码获取Enumeration
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Enumeration Get(string code)
        {
            Enumeration[] tents = Enumeration.FindAllByProperty(Enumeration.Prop_Code, code);
            if (tents != null && tents.Length > 0)
            {
                return tents[0];
            }

            return null;
        }

        /// <summary>
        /// 由编码获取Enumeration字典
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static EasyDictionary<string, Enumeration> Get(params string[] codes)
        {
            EasyDictionary<string, Enumeration> enums = new EasyDictionary<string, Enumeration>();

            IEnumerable<Enumeration> tents = Enumeration.FindAll(Expression.In(Enumeration.Prop_Code, codes));
            tents = tents.OrderBy(tent => tent.SortIndex).ThenBy(tent => tent.CreatedDate);

            foreach (Enumeration tent in tents)
            {
                enums.Set(tent.Code, tent);
            }

            return enums;
        }

        /// <summary>
        /// 由给出的编码列表获取枚举字典列表
        /// </summary>
        /// <returns></returns>
        public static EasyDictionary<string, EasyDictionary> GetEnumDicts(params string[] codes)
        {
            EasyDictionary<string, EasyDictionary> rtndict = new EasyDictionary<string, EasyDictionary>();

            Enumeration[] tents = Enumeration.FindAll(Expression.In(Enumeration.Prop_Code, codes));
            string[] pids = tents.Select(tent => tent.EnumerationID).ToArray();

            IEnumerable<Enumeration> subents = Enumeration.FindAll(Expression.In(Enumeration.Prop_ParentID, pids));
            subents = subents.OrderBy(tsubtent => tsubtent.SortIndex).ThenBy(tsubtent => tsubtent.CreatedDate);

            foreach (Enumeration tent in tents)
            {
                EasyDictionary dict = new EasyDictionary();

                IEnumerable<Enumeration> tsubents = subents.Where(ttent => ttent.ParentID == tent.EnumerationID);
                foreach (Enumeration tsubent in tsubents)
                {
                    dict.Set(tsubent.Value, tsubent.Name);
                }

                rtndict.Set(tent.Code, dict);
            }

            return rtndict;
        }

        /// <summary>
        /// 获取所有子孙节点组合的枚举
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static EasyDictionary GetDescendantEnumDict(string code)
        {
            return GetDescendantEnumDict(code, false);
        }

        /// <summary>
        /// 获取所有子孙节点组合的枚举
        /// </summary>
        /// <param name="code"></param>
        /// <param name="includeSelf">是否包括当前节点</param>
        /// <returns></returns>
        public static EasyDictionary GetDescendantEnumDict(string code, bool includeSelf)
        {
            Enumeration tenum = Enumeration.Get(code);
            IList<Enumeration> enums = tenum.GetDescendantNodes().ToList();

            if (includeSelf)
            {
                enums.Add(tenum);
            }

            if (enums != null && enums.Count > 0)
            {
                enums = enums.OrderBy(tent => tent.PathLevel).ThenBy(tent => tent.SortIndex).ToList();
            }

            return GetEnumDict(enums); ;
        }

        /// <summary>
        /// 由枚举编码获取Enum字典
        /// </summary>
        /// <param name="enums"></param>
        /// <returns></returns>
        public static EasyDictionary GetEnumDict(string code)
        {
            Enumeration tent = Enumeration.Get(code);
            EasyDictionary dict = tent.GetDict();

            return dict;
        }

        /// <summary>
        /// 由enums列表获取Enum字典
        /// </summary>
        /// <param name="enums"></param>
        /// <returns></returns>
        public static EasyDictionary GetEnumDict(IEnumerable<Enumeration> enums)
        {
            EasyDictionary dict = new EasyDictionary();

            IEnumerable<Enumeration> tenums = null;

            if (enums is IOrderedEnumerable<Enumeration>)
            {
                tenums = enums;
            }
            else
            {
                tenums = enums.OrderBy(tent => tent.SortIndex).ThenBy(tent => tent.CreatedDate);
            }

            foreach (Enumeration item in tenums)
            {
                dict.Set(item.Value, item.Name);
            }

            return dict;
        }
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			Enumeration[] tents = Enumeration.FindAll(Expression.In("EnumerationID", args));

			foreach (Enumeration tent in tents)
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
                IList<Enumeration> allnodes = Enumeration.FindAllByPrimaryKeys(pasteIds);
                IList<Enumeration> nodes = FilterChildNodes(allnodes);

                // 只提取最高节点或无父子关联的节点进行粘贴
                foreach (Enumeration tnode in nodes)
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

    } // SysEnumeration
}


