using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Criterion;

namespace PIC.Data
{
    /// <summary>
    /// 可执行实体
    /// </summary>
    public abstract class EditSensitiveEntityBase<T> : EntityBase<T>, IEditSensitiveEntity where T : EntityBase<T>
    {
        #region 成员

        #endregion

        #region 属性

        public abstract string EditStatus { get; set; }

        /// <summary>
        /// 编辑状态标志
        /// </summary>
        public EditStatusEnum EditStatusFlag
        {
            get
            {
                return EditSensitiveEntityHelper.GetEditStatusFlag(this.EditStatus);
            }

            set
            {
                this.EditStatus = value.ToString();
            }
        }

        /// <summary>
        /// 是否允许创建(不允许手工创建，一般为子节点)
        /// </summary>
        public bool Creatable
        {
            get { return EditSensitiveEntityHelper.CheckEditStatus(this, EditStatusEnum.Create); }
            set
            {
                if (value)
                {
                    EditSensitiveEntityHelper.SetEditStatus(this, EditStatusEnum.Create);
                }
                else
                {
                    EditSensitiveEntityHelper.RemoveEditStatus(this, EditStatusEnum.Create);
                }
            }
        }

        /// <summary>
        /// 是否允许编辑(不允许手工编辑)
        /// </summary>
        public bool Editable
        {
            get { return EditSensitiveEntityHelper.CheckEditStatus(this, EditStatusEnum.Update); }
            set
            {
                if (value)
                {
                    EditSensitiveEntityHelper.SetEditStatus(this, EditStatusEnum.Update);
                }
                else
                {
                    EditSensitiveEntityHelper.RemoveEditStatus(this, EditStatusEnum.Update);
                }
            }
        }

        /// <summary>
        /// 是否允许删除(不允许手工删除)
        /// </summary>
        public bool Deletable
        {
            get { return EditSensitiveEntityHelper.CheckEditStatus(this, EditStatusEnum.Delete); }
            set
            {
                if (value)
                {
                    EditSensitiveEntityHelper.SetEditStatus(this, EditStatusEnum.Delete);
                }
                else
                {
                    EditSensitiveEntityHelper.RemoveEditStatus(this, EditStatusEnum.Delete);
                }
            }
        }

        /// <summary>
        /// 是否允许删除(不允许用户查看)
        /// </summary>
        public bool Readable
        {
            get { return EditSensitiveEntityHelper.CheckEditStatus(this, EditStatusEnum.Read); }
            set
            {
                if (value)
                {
                    EditSensitiveEntityHelper.SetEditStatus(this, EditStatusEnum.Read);
                }
                else
                {
                    EditSensitiveEntityHelper.RemoveEditStatus(this, EditStatusEnum.Read);
                }
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 检查编辑权限(默认可读)
        /// </summary>
        /// <param name="editStatus"></param>
        /// <returns></returns>
        public bool CheckEditStatus(EditStatusEnum editStatus)
        {
            return EditSensitiveEntityHelper.CheckEditStatus(this, editStatus);
        }

        /// <summary>
        /// 检查编辑权限
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="editStatus"></param>
        /// <returns></returns>
        public bool CheckEditStatus(string editStatusString)
        {
            return EditSensitiveEntityHelper.CheckEditStatus(this, editStatusString);
        }

        /// <summary>
        /// 赋予所有权限
        /// </summary>
        public void SetFullEditStatus()
        {
            this.EditStatus = EditSensitiveEntityHelper.FullStateString;
        }

        /// <summary>
        /// 移除所有权限
        /// </summary>
        public void RemoveFullEditStatus()
        {
            this.EditStatus = String.Empty;
        }

        /// <summary>
        /// 设置编辑状态
        /// </summary>
        /// <param name="editStatus"></param>
        public void SetEditStatus(EditStatusEnum editStatus)
        {
            EditSensitiveEntityHelper.SetEditStatus(this, editStatus);
        }

        /// <summary>
        /// 设置编辑状态
        /// </summary>
        /// <param name="editStatus"></param>
        public void SetEditStatus(string editStatusString)
        {
            EditSensitiveEntityHelper.SetEditStatus(this, editStatusString);
        }

        /// <summary>
        /// 移除编辑权限
        /// </summary>
        /// <param name="editStatus"></param>
        public void RemoveEditStatus(EditStatusEnum editStatus)
        {
            EditSensitiveEntityHelper.RemoveEditStatus(this, editStatus);
        }

        /// <summary>
        /// 移除编辑权限
        /// </summary>
        /// <param name="editStatus"></param>
        public void RemoveEditStatus(string editStatusString)
        {
            EditSensitiveEntityHelper.RemoveEditStatus(this, editStatusString);
        }

        /// <summary>
        /// 根据查询条件查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(HqlSearchCriterion criterion, EditStatusEnum editStatus)
        {
            return criterion.FindAll<T>(Expression.Eq("EditStatus", editStatus.ToString()));
        }

        /// <summary>
        /// 根据查询条件查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(HqlSearchCriterion criterion, string editStatus)
        {
            return criterion.FindAll<T>(Expression.Eq("EditStatus", editStatus));
        }

        /// <summary>
        /// 根据查询条件和Hql查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(HqlSearchCriterion criterion, EditStatusEnum editStatus, params ICriterion[] crits)
        {
            IList<ICriterion> critList = crits.ToList();
            critList.Add(Expression.Eq("EditStatus", editStatus.ToString()));

            return criterion.FindAll<T>(critList.ToArray());
        }

        /// <summary>
        /// 根据查询条件和Hql查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll(HqlSearchCriterion criterion, string editStatus, params ICriterion[] crits)
        {
            IList<ICriterion> critList = crits.ToList();
            critList.Add(Expression.Eq("EditStatus", editStatus));

            return criterion.FindAll<T>(critList.ToArray());
        }

        /// <summary>
        /// 通过编辑状态找数据
        /// </summary>
        /// <param name="editStatus"></param>
        public T[] FindAll(EditStatusEnum editStatus)
        {
            return FindAll(editStatus.ToString());
        }

        /// <summary>
        /// 通过编辑状态找数据
        /// </summary>
        /// <param name="editStatus"></param>
        public T[] FindAll(string editStatus)
        {
            return EntityBase<T>.FindAllByProperty("EditStatus", editStatus);
        }

        #endregion
    }
}
