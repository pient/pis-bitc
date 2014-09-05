using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Criterion;
using Newtonsoft.Json;

namespace PIC.Data
{
    /// <summary>
    /// 属性节点实体
    /// </summary>
    public abstract class TreeNodeEntityBaseV1<T> : EntityBase<T>, ITreeNodeEntity<T> where T : TreeNodeEntityBaseV1<T>
    {
        #region 成员变量

        public const string Prop_ParentID = "ParentID";
        public const string Prop_Path = "Path";
        public const string Prop_PathLevel = "PathLevel";
        public const string Prop_IsLeaf = "IsLeaf";
        public const string Prop_SortIndex = "SortIndex";

        private bool isParentIDChanged = false;
        private string oldParentID = String.Empty;

        [NonSerialized]
        protected T _Parent;

        [NonSerialized]
        protected IList<T> _Children;

        [NonSerialized]
        protected T _FirstChild;

        protected bool? _IsLeaf = null;

        #endregion

        #region 成员属性

        /// <summary>
        /// 路径分割符
        /// </summary>
        public virtual char PathDivChar
        {
            get
            {
                return '.';
            }
        }

        private string _parentID;

        /// <summary>
        /// 父节点ID
        /// </summary>
        public virtual string ParentID
        {
            get
            {
                return _parentID;
            }
            set
            {
                if ((_parentID == null) || (value == null) || (!value.Equals(_parentID)))
                {
                    object oldValue = _parentID;

                    _parentID = value;

                    RaisePropertyChanged(Prop_ParentID, oldValue, value);
                }
            }
        }

        /// <summary>
        /// 节点路径
        /// </summary>
        public abstract string Path
        {
            get;
            set;
        }

        /// <summary>
        /// 节点路径深度
        /// </summary>
        public virtual int? PathLevel
        {
            get
            {
                if (String.IsNullOrEmpty(this.Path))
                {
                    return 0;
                }
                else
                {
                    return this.Path.Split(PathDivChar).Length;
                }
            }
            set { }
        }

        /// <summary>
        /// 此组的所在组（父组）
        /// </summary>
        [JsonIgnore]
        public virtual T Parent
        {
            get
            {
                if (!IsTop)
                {
                    _Parent = EntityBase<T>.Find(this.ParentID);
                }

                return _Parent;
            }
        }

        /// <summary>
        /// 第一个子节点
        /// </summary>
        [JsonIgnore]
        public virtual T FirstChild
        {
            get
            {
                if (_FirstChild == null)
                {
                    _FirstChild = EntityBase<T>.FindFirstByProperties(Prop_ParentID, this.GetPrimaryValue());
                }

                return _FirstChild;
            }
        }

        /// <summary>
        /// 是否顶层组（没有父节点）
        /// </summary>
        public virtual bool IsTop
        {
            get
            {
                return String.IsNullOrEmpty(this.ParentID);
            }
            set { }
        }

        /// <summary>
        /// 是否根模块（没有子节点）
        /// </summary>
        public virtual bool? IsLeaf
        {
            get;
            set;
        }

        /// <summary>
        /// 此组的所拥有的子模块
        /// </summary>
        [JsonIgnore]
        public virtual IList<T> Children
        {
            get
            {
                if (_Children == null)
                {
                    _Children = EntityBase<T>.FindAllByProperties(Prop_ParentID, this.GetPrimaryValue().ToString());
                }

                return _Children;
            }
        }

        /// <summary>
        /// 父节点ID是否发生变化
        /// </summary>
        [JsonIgnore]
        protected virtual bool IsParentIDChanged
        {
            get
            {
                return isParentIDChanged;
            }
        }

        /// <summary>
        /// 原父节点ID
        /// </summary>
        [JsonIgnore]
        protected virtual string OldParentID
        {
            get
            {
                return oldParentID;
            }
        }

        #endregion

        #region 构造函数

        protected override void OnPropertyChanged(object sender, PICPropertyChangedEventArgs e)
        {
            // 记录原始值
            if (e.PropertyName == Prop_ParentID && isParentIDChanged == false && this.GetPrimaryValue() != null)
            {
                if (e.OldValue == null)
                {
                    oldParentID = null;
                }
                else
                {
                    oldParentID = e.OldValue.ToString();
                }

                isParentIDChanged = true;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 检查并设置叶节点状态
        /// </summary>
        /// <returns></returns>
        public virtual void CheckSetIsLeafProperty()
        {
            bool isLeaf = this.CheckIsLeaf();

            if (this.IsLeaf != isLeaf)
            {
                this.IsLeaf = this.CheckIsLeaf();

                this.Update();
            }
        }

        /// <summary>
        /// 检查是否叶节点
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckIsLeaf()
        {
            DetachedCriteria crits = DetachedCriteria.For<T>();
            crits.Add(Expression.Eq(Prop_ParentID, this.GetPrimaryValue().ToString()));
            bool isLeaf = !EntityBase<T>.Exists(crits);

            return isLeaf;
        }

        /// <summary>
        /// 获取相应层级子节点
        /// </summary>
        /// <returns></returns>
        public virtual T[] GetSubs()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取相应层级子节点
        /// </summary>
        /// <param name="level">当前层级向下层级</param>
        /// <returns></returns>
        public virtual T[] GetSubs(int level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加顶层节点
        /// </summary>
        /// <param name="module"></param>
        public virtual void CreateAsTop()
        {
            this.Path = null;

            this.Create();
        }

        /// <summary>
        /// 添加兄弟节点
        /// </summary>
        /// <param name="module"></param>
        public virtual void CreateAsSib(string sibID)
        {
            T sib = EntityBase<T>.Find(sibID);

            CreateAsSib(sib);
        }

        /// <summary>
        /// 添加兄弟节点
        /// </summary>
        /// <param name="module"></param>
        public virtual void CreateAsSib(T sib)
        {
            this.ParentID = sib.ParentID;
            this.Path = sib.Path;

            this.Create();
        }

        /// <summary>
        /// 添加子模块
        /// </summary>
        /// <param name="module"></param>
        public virtual void CreateAsSub(string parentID)
        {
            T parent = EntityBase<T>.Find(parentID);

            CreateAsSub(parent);
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="module"></param>
        public virtual void CreateAsSub(T parent)
        {
            string primaryValue = parent.GetPrimaryValue().ToString();

            this.ParentID = primaryValue;
            this.Path = String.Format("{0}{1}{2}", (parent.Path == null ? String.Empty : parent.Path), PathDivChar, primaryValue);

            this.Create();
        }

        #endregion
    }
}
