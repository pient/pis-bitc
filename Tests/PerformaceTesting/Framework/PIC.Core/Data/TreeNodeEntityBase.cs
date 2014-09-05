using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using NHibernate;
using NHibernate.Criterion;
using Newtonsoft.Json;
using Castle.ActiveRecord;

namespace PIC.Data
{
    /// <summary>
    /// 树形属性节点实体
    /// </summary>
    public abstract class TreeNodeEntityBase<T> : EntityBase<T>, ITreeNodeEntity<T> where T : TreeNodeEntityBase<T>
    {
        #region 成员变量

        private bool _IsParentIDChanged = false;
        private string _OldParentID = String.Empty;

        [NonSerialized]
        protected T _Parent;

        [NonSerialized]
        protected IList<T> _ChildNodes;

        [NonSerialized]
        protected IList<T> _SiblingNodes;

        #endregion

        #region 成员属性

        /// <summary>
        /// 根节点层次
        /// </summary>
        public virtual int RootNodeLevel
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 排序号首位
        /// </summary>
        public virtual int SortStartIndex
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// 排序间隔
        /// </summary>
        public virtual int SortInterval
        {
            get
            {
                return 1;
            }
        }

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

        /// <summary>
        /// 标识属性(可用来构成ParentID或Path)
        /// </summary>
        public virtual string Prop_ID
        {
            get
            {
                return PrimaryKeyName;
            }
        }

        /// <summary>
        /// 父节点标识属性字段名
        /// </summary>
        public virtual string Prop_ParentID
        {
            get
            {
                return "ParentID";
            }
        }

        /// <summary>
        /// 路径属性字段名
        /// </summary>
        public virtual string Prop_Path
        {
            get
            {
                return "Path";
            }
        }

        /// <summary>
        /// 路径深度属性字段名
        /// </summary>
        public virtual string Prop_PathLevel
        {
            get
            {
                return "PathLevel";
            }
        }

        /// <summary>
        /// 是否叶子节点属性字段名
        /// </summary>
        public virtual string Prop_IsLeaf
        {
            get
            {
                return "IsLeaf";
            }
        }

        /// <summary>
        /// 排序号属性字段名
        /// </summary>
        public virtual string Prop_SortIndex
        {
            get
            {
                return "SortIndex";
            }
        }

        /// <summary>
        /// 父节点ID(一般不允许手动修改)
        /// </summary>
        public virtual string ID
        {
            get
            {
                return CLRHelper.ConvertValue<string>(this.GetPrimaryValue());
            }
        }

        /// <summary>
        /// 父节点ID(一般不允许直接赋值)
        /// </summary>
        public virtual string ParentID
        {
            get;
            set;
        }

        /// <summary>
        /// 节点路径
        /// </summary>
        public virtual string Path
        {
            get;
            set;
        }

        /// <summary>
        /// 节点路径深度
        /// </summary>
        public virtual int? PathLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 节点排序号
        /// </summary>
        public virtual int? SortIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 是否根节点（没有子节点）
        /// </summary>
        public virtual bool? IsLeaf
        {
            get;
            set;
        }

        /// <summary>
        /// 此组的所在组（父组）
        /// </summary>
        [JsonIgnore]
        public virtual T Parent
        {
            get
            {
                if (!String.IsNullOrEmpty(this.ParentID))
                {
                    this.RefreshParent();
                }

                return _Parent;
            }
        }

        /// <summary>
        /// 此组的所拥有的子模块
        /// </summary>
        [JsonIgnore]
        public virtual IList<T> ChildNodes
        {
            get
            {
                if (_ChildNodes == null)
                {
                    this.RefreshChildNodes();
                }

                return _ChildNodes;
            }
        }

        /// <summary>
        /// 获取首子节点
        /// </summary>
        [JsonIgnore]
        public virtual T FirstChild
        {
            get
            {
                return GetFirstChild();
            }
        }

        /// <summary>
        /// 获取尾子节点
        /// </summary>
        [JsonIgnore]
        public virtual T LastChild
        {
            get
            {
                return GetLastChild();
            }
        }

        /// <summary>
        /// 首兄弟节点
        /// </summary>
        [JsonIgnore]
        public virtual T FirstSibling
        {
            get
            {
                return GetFirstSibling();
            }
        }

        /// <summary>
        /// 前置兄弟节点
        /// </summary>
        [JsonIgnore]
        public virtual T PrevSibling
        {
            get
            {
                return GetPrevSibling();
            }
        }

        /// <summary>
        /// 所有前置兄弟节点
        /// </summary>
        [JsonIgnore]
        public virtual IList<T> PrevSiblingNodes
        {
            get
            {
                return GetPrevSiblingNodes();
            }
        }

        /// <summary>
        /// 后置兄弟节点
        /// </summary>
        [JsonIgnore]
        public virtual T NextSibling
        {
            get
            {
                return GetNextSibling();
            }
        }

        /// <summary>
        /// 所有后置兄弟节点
        /// </summary>
        [JsonIgnore]
        public virtual IList<T> NextSiblingNodes
        {
            get
            {
                return GetNextSiblingNodes();
            }
        }

        /// <summary>
        /// 尾兄弟节点
        /// </summary>
        [JsonIgnore]
        public virtual T LastSibling
        {
            get
            {
                return GetLastSibling();
            }
        }

        /// <summary>
        /// 所有兄弟节点
        /// </summary>
        [JsonIgnore]
        public virtual IList<T> SiblingNodes
        {
            get
            {
                if (_SiblingNodes == null)
                {
                    RefreshSiblingNodes();
                }

                return _SiblingNodes;
            }
        }

        /// <summary>
        /// 父节点ID是否发生变化，当父节点被手动修改（一般不允许）后为true，否则为false
        /// </summary>
        [JsonIgnore]
        protected virtual bool IsParentIDChanged
        {
            get
            {
                return _IsParentIDChanged;
            }
        }

        /// <summary>
        /// 原父节点ID，当父节点被手动修改（一般不允许）后，原父节点标识
        /// </summary>
        [JsonIgnore]
        protected virtual string OldParentID
        {
            get
            {
                return _OldParentID;
            }
        }

        #endregion

        #region 构造函数

        #endregion

        #region 重载

        /// <summary>
        /// 属性改变时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(object sender, PICPropertyChangedEventArgs e)
        {
            /*
            // 记录原始值
            if (e.PropertyName == Prop_ParentID && _IsParentIDChanged == false && this.GetPrimaryValue() != null)
            {
                if (e.OldValue == null)
                {
                    _OldParentID = null;
                }
                else
                {
                    _OldParentID = e.OldValue.ToString();
                }

                _IsParentIDChanged = true;
            }
             * */
        }

        /// <summary>
        /// 刚创建的节点一定为叶节点
        /// </summary>
        public override void Create()
        {
            // throw new NotImplementedException();
            base.Create();
        }

        /// <summary>
        /// 刚创建的节点一定为叶节点
        /// </summary>
        public override void CreateAndFlush()
        {
            // throw new NotImplementedException();
            base.CreateAndFlush();
        }

        /// <summary>
        /// 一般不允许手工修改属性结构如IsLeaf, Path, PathLevel, ParentID, SortIndex
        /// </summary>
        public override void Update()
        {
            base.UpdateAndFlush();
        }

        /// <summary>
        /// 一般不允许手工修改属性结构如IsLeaf, Path, PathLevel, ParentID, SortIndex
        /// </summary>
        public override void UpdateAndFlush()
        {
            base.UpdateAndFlush();
        }

        /// <summary>
        /// 将同时删除子孙裔节点
        /// </summary>
        public override void Delete()
        {
            this.DoDelete();
        }

        /// <summary>
        /// 将同时删除子孙裔节点
        /// </summary>
        public override void DeleteAndFlush()
        {
            this.DoDelete();
        }

        [ActiveRecordTransaction]
        private void DoDelete()
        {
            ISession session = DataHelper.OpenHqlSession<T>();

            // 删除子节点及自身节点
            string hql = String.Format("DELETE FROM {0} WHERE {2} LIKE '%{1}%' OR {3} = '{1}' ", EntityBase<T>.ClassName, this.ID, 
                Prop_Path, Prop_ID);
            DataHelper.HqlUpdate(session, hql);

            // 修改后继节点位置信息
            hql = String.Format("UPDATE {0} SET {3} = {3} - {1} WHERE {4} = ? AND {3} > {2}", 
                EntityBase<T>.ClassName, SortInterval, this.SortIndex,
                Prop_SortIndex, Prop_ParentID);            

            DataHelper.HqlUpdate(session, hql, this.ParentID);

            // 检查修改父节点IsLeaf属性
            if (this.Parent != null)
            {
                this.Parent.CheckSetIsLeaf(this.ID);
            }

            System.Diagnostics.Debug.Assert(true);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 刷新当前节点(包括当前节点及其父节点，子节点，兄弟节点等信息)
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();

            RefreshParent();
            RefreshChildNodes();
            RefreshSiblingNodes();
        }

        /// <summary>
        /// 获取所有根节点
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> GetRoots()
        {
            // 父节点为空或路径级别等于根节点级别时为根节点
            ICriterion crit = SearchHelper.UnionCriterions(
                Expression.Eq(Prop_ParentID, String.Empty), 
                Expression.IsNull(Prop_ParentID), 
                Expression.Eq(Prop_PathLevel, RootNodeLevel));

            return EntityBase<T>.FindAll(crit);
        }

        /// <summary>
        /// 获取所有祖先级节点
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public virtual IList<T> GetAncestorNodes()
        {
            string sql = String.Format("{0} LIKE '%' + {1} + '%'", this.Path, Prop_ID);

            ICriterion crit = Expression.Sql(sql);

            return EntityBase<T>.FindAll(crit);
        }

        /// <summary>
        /// 获取相应层级祖先级节点
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public virtual T GetAncestor(int level)
        {
            if (level >= this.PathLevel || String.IsNullOrEmpty(this.Path))
            {
                return null;
            }
            else
            {
                string sql = String.Format("{0} LIKE '%' + {1} + '%'", this.Path, Prop_ID);

                ICriterion crit = SearchHelper.IntersectCriterions(
                        Expression.Sql(sql),
                        Expression.Eq(Prop_PathLevel, level));

                return EntityBase<T>.FindFirst(crit);
            }
        }

        /// <summary>
        /// 获取所有子孙裔节点
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> GetDescendantNodes()
        {
            ICriterion crit = Expression.Like(Prop_Path, this.ID, MatchMode.Anywhere);

            return EntityBase<T>.FindAll(crit);
        }

        /// <summary>
        /// 获取相应层级子孙裔节点
        /// </summary>
        /// <param name="level">当前层级向下层级</param>
        /// <returns></returns>
        public virtual IList<T> GetDescendantNodes(int level)
        {
            if (level <= this.PathLevel)
            {
                return null;
            }
            else
            {
                string sql = String.Format("{0} LIKE '%{1}%'", Prop_Path, this.ID);

                ICriterion crit = SearchHelper.IntersectCriterions(
                        Expression.Sql(sql),
                        Expression.Eq(Prop_PathLevel, level));

                return EntityBase<T>.FindAll(crit);
            }
        }

        /// <summary>
        /// 由子节点位置获取子节点
        /// </summary>
        /// <param name="position">从1开始</param>
        /// <returns></returns>
        public virtual T GetChildByPosition(int position)
        {
            if (this.ChildNodes != null && this.ChildNodes.Count > 0)
            {
                if (this.ChildNodes.Count(tent => tent.SortIndex == position) > 0)
                {
                    return this.ChildNodes.First(tent => tent.SortIndex == position);
                }
            }

            return default(T);
        }

        /// <summary>
        /// 添加顶层节点
        /// </summary>
        /// <param name="module"></param>
        public virtual void CreateAsRoot()
        {
            this.ParentID = null;
            this.Path = null;
            this.PathLevel = RootNodeLevel;
            this.SortIndex = SortStartIndex;

            this.DoCreate();
        }

        /// <summary>
        /// 根据ID添加兄弟节点
        /// </summary>
        /// <param name="siblingID"></param>
        public virtual void CreateAsSibling(string siblingID)
        {
            CreateAsSibling(EntityBase<T>.Find(siblingID));
        }

        /// <summary>
        /// 添加兄弟节点(添加后排序号将比当前排序号大一个间隔)
        /// </summary>
        /// <param name="module"></param>
        [ActiveRecordTransaction]
        public virtual void CreateAsSibling(T sibling)
        {
            this.ParentID = sibling.ParentID;
            this.Path = sibling.Path;
            this.PathLevel = sibling.PathLevel;

            if (!this.SortIndex.HasValue || this.SortIndex == 0)
            {
                // 所有在本节点位置以下的兄弟节点排序号增加一个间隔
                if (sibling.NextSiblingNodes != null)
                {
                    foreach (T tnode in sibling.NextSiblingNodes)
                    {
                        tnode.SortIndex += SortInterval;

                        tnode.Update();
                    }
                }

                this.SortIndex = sibling.SortIndex.GetValueOrDefault() + SortInterval;
            }

            this.DoCreate();
        }

        /// <summary>
        /// 根据ID添加兄节节点
        /// </summary>
        /// <param name="parentID">兄节点标识</param>
        /// <param name="position">添加位置</param>
        public virtual void CreateAsChild(string parentID, int position, bool adjustSibling = true)
        {
            CreateAsChild(EntityBase<T>.Find(parentID), position, adjustSibling);
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="parentID">兄节点标识</param>
        /// <param name="position">添加位置(从SortStartIndex开始)</param>
        public virtual void CreateAsChild(T parent, int position, bool adjustSibling = true)
        {
            this.InsertAt(parent, position, adjustSibling);
        }

        /// <summary>
        /// 根据ID添加子节点
        /// </summary>
        /// <param name="module"></param>
        public virtual void CreateAsChild(string parentID)
        {
            CreateAsChild(EntityBase<T>.Find(parentID));
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="parent"></param>
        public virtual void CreateAsChild(T parent)
        {
            if (this.SortIndex.HasValue && this.SortIndex != 0)
            {
                CreateAsChild(parent, this.SortIndex.Value, false);
            }
            else
            {
                CreateAsChild(parent, SortStartIndex);
            }
        }

        /// <summary>
        /// 插入到指定父节点值定位置
        /// </summary>
        /// <param name="position"></param>
        public virtual void InsertAt(T parent, int position, bool adjustSilbing = true)
        {
            this.ParentID = parent.ID;
            this.Path = CombinePath(parent.Path, parent.ID);
            this.PathLevel = parent.PathLevel + 1;

            int lastPosition = SortStartIndex;

            if (adjustSilbing == true)
            {
                if (position < SortStartIndex)
                {
                    position = SortStartIndex;
                }
                else if (parent.LastChild != null)
                {
                    lastPosition = parent.LastChild.SortIndex.GetValueOrDefault() + 1;

                    if (parent.LastChild.SortIndex < position)
                    {
                        position = lastPosition;
                    }
                }

                // 先将一点插入到父节点末尾
                this.SortIndex = lastPosition;
            }

            this.DoCreate();

            if (adjustSilbing == true)
            {
                if (position != lastPosition)
                {
                    // 移动当前节点到目标位置
                    this.ChangePosition(position);
                }
            }

            // 设置父节点IsLeaf属性
            T tent = EntityBase<T>.Find(parent.ID);
            if (tent.IsLeaf.GetValueOrDefault() == true)
            {
                tent.IsLeaf = false;
                tent.UpdateAndFlush();
            }
        }

        /// <summary>
        /// 根据ID移动为目标节点的子节点
        /// </summary>
        /// <param name="target"></param>
        public virtual void MoveAsChild(string targetID)
        {
            MoveAsChild(EntityBase<T>.Find(targetID));
        }

        /// <summary>
        /// 移动为目标节点的子节点
        /// </summary>
        /// <param name="target"></param>
        public virtual void MoveAsChild(T target)
        {
            this.ChangeParent(target, 0);
        }

        /// <summary>
        /// 根据ID移动为目标节点的兄弟节点
        /// </summary>
        public virtual void MoveAsSibling(string targetID)
        {
            MoveAsSibling(EntityBase<T>.Find(targetID));
        }

        /// <summary>
        /// 移动为目标节点兄弟节点
        /// </summary>
        public virtual void MoveAsSibling(T target)
        {
            int newSortIndex = AdjustSortIndexByInterval(target.SortIndex.GetValueOrDefault(), 1);

            if (this.ParentID != target.ParentID)
            {
                this.ChangeParent(target.Parent, newSortIndex);
            }
            else
            {
                this.ChangePosition(newSortIndex);
            }
        }

        /// <summary>
        /// 移动到首节点
        /// </summary>
        public virtual void MoveFirst()
        {
            if (FirstSibling != null)
            {
                ExchangeSiblingPosition(FirstSibling, (T)this);
            }
        }

        /// <summary>
        /// 向下移动本节点
        /// </summary>
        public virtual void MoveNext()
        {
            if (NextSibling != null)
            {
                ExchangeSiblingPosition(NextSibling, (T)this);
            }
        }

        /// <summary>
        /// 向上移动本节点
        /// </summary>
        public virtual void MovePrev()
        {
            if (PrevSibling != null)
            {
                ExchangeSiblingPosition(PrevSibling, (T)this);
            }
        }

        /// <summary>
        /// 移动到尾节点
        /// </summary>
        public virtual void MoveLast()
        {
            if (LastSibling != null)
            {
                ExchangeSiblingPosition(LastSibling, (T)this);
            }
        }

        /// <summary>
        /// 根据ID复制为目标节点子节点
        /// </summary>
        public virtual void CopyAsChild(string targetID)
        {
            CopyAsChild(EntityBase<T>.Find(targetID));
        }

        /// <summary>
        /// 复制为目标节点子节点
        /// </summary>
        public virtual void CopyAsChild(T target)
        {
            SqlConnection conn = DataHelper.GetCurrentDbConnection() as SqlConnection;
            IDbTransaction trans = conn.BeginTransaction();

            try
            {
                if (conn != null)
                {
                    DataTable copydt = GetCopyTable(target, PasteAsEnum.Child);

                    // 拷贝复制表到数据库
                    DataHelper.CopyDataToDatabase(copydt, conn, TableName);

                    // 调整同级节点SortIndex
                    string hql = String.Empty;

                    if (String.IsNullOrEmpty(target.ParentID))
                    {
                        hql = String.Format("UPDATE {0} SET {3} = ({3} + {1}) WHERE ({4} IS NULL OR TRIM({4}) = '') AND {3} >= {2}; ",
                            TableName, SortInterval, target.SortIndex,
                            Prop_SortIndex, Prop_ParentID);
                    }
                    else
                    {
                        hql = String.Format("UPDATE {0} SET {4} = ({4} + {1}) WHERE {5} = '{2}' AND {4} >= {3}; ",
                            TableName, SortInterval, target.ParentID, target.SortIndex,
                            Prop_SortIndex, Prop_ParentID);
                    }

                    // 修改目标节点IsLeaf
                    if (target.IsLeaf == true)
                    {
                        hql += String.Format(" UPDATE {0} SET {3} = 0 WHERE {1} = '{2}' ",
                            TableName, PrimaryKeyName, target.ID,
                            Prop_IsLeaf);
                    }

                    DataHelper.HqlUpdate(hql);
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();

                throw ex;
            }
        }

        /// <summary>
        /// 根据ID复制为目标节点兄弟节点
        /// </summary>
        public virtual void CopyAsSibling(string targetID)
        {
            CopyAsSibling(EntityBase<T>.Find(targetID));
        }

        /// <summary>
        /// 复制为目标节点兄弟节点
        /// </summary>
        [ActiveRecordTransaction]
        public virtual void CopyAsSibling(T target)
        {
            SqlConnection conn = DataHelper.GetCurrentDbConnection() as SqlConnection;

            if (conn != null)
            {
                DataTable copydt = GetCopyTable(target, PasteAsEnum.Sibling);

                // 拷贝复制表到数据库
                DataHelper.CopyDataToDatabase(copydt, conn, TableName);

                // 调整同级节点SortIndex
                string hql = String.Empty;

                if (String.IsNullOrEmpty(target.ParentID))
                {
                    hql = String.Format("UPDATE {0} SET SortIndex = (SortIndex + {1}) WHERE (ParentID IS NULL OR TRIM(ParentID) = '') AND SortIndex >= {2}; ",
                        EntityBase<T>.ClassName, SortInterval, target.SortIndex);
                }
                else
                {
                    hql = String.Format("UPDATE {0} SET SortIndex = (SortIndex + {1}) WHERE ParentID = '{2}' AND SortIndex >= {3}; ",
                        EntityBase<T>.ClassName, SortInterval, target.ParentID, target.SortIndex);
                }

                DataHelper.HqlUpdate(hql);
            }
        }

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        [ActiveRecordTransaction]
        public virtual void ClearChildren()
        {
            string hql = String.Format("DELETE FROM {0} WHERE Path LIKE '%{1}%' ", EntityBase<T>.ClassName, this.ID);

            DataHelper.HqlUpdate(hql);

            this.CheckSetIsLeaf();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取本节点复制表
        /// </summary>
        /// <returns></returns>
        private DataTable GetCopyTable(T target, PasteAsEnum patype)
        {
            string sql = String.Format("SELECT * FROM {0} WHERE {1} = '{2}' OR [{3}] LIKE '%{2}%'",
                TableName, Prop_ID, this.ID,
                Prop_Path);

            DataTable dt = DataHelper.QueryDataTable(sql);

            Hashtable noMapping = new Hashtable();    // 新旧id映射
            Hashtable onMapping = new Hashtable();    // 旧新id映射

            object nid, oid;
            foreach (DataRow trow in dt.Rows)
            {
                nid = GenerateNewID();
                oid = trow[Prop_ID];

                noMapping.Add(nid, oid);
                onMapping.Add(oid, nid);
            }

            int adjustLevel = 0;    // 调整PathLevel级别
            string adjustPath = String.Empty;   // 调整的路径
            string toid;    // 原id
            string topath = String.Empty;   // 临时原路径
            string tnpath = String.Empty;   // 临时新路径

            if (patype == PasteAsEnum.Sibling)
            {
                adjustLevel = target.PathLevel.GetValueOrDefault() - this.PathLevel.GetValueOrDefault();
                adjustPath = target.Path;
            }
            else if (patype == PasteAsEnum.Child)
            {
                adjustLevel = target.PathLevel.GetValueOrDefault() - this.PathLevel.GetValueOrDefault() + 1;
                adjustPath = CombinePath(target.Path, target.ID);
            }

            // 替换ParentID 和 PathLevel
            foreach (DataRow trow in dt.Rows)
            {
                toid = CLRHelper.ConvertValue<string>(trow[Prop_ID]);

                // 替换ID
                trow[Prop_ID] = onMapping[toid];

                if (toid != this.ID.ToString())
                {
                    // 替换ParentID
                    trow[Prop_ParentID] = onMapping[trow[Prop_ParentID]];

                    // 替换PathLevel
                    trow[Prop_PathLevel] = CLRHelper.ConvertValue<int>(trow[Prop_PathLevel], 0) + adjustLevel;

                    topath = trow[Prop_Path].ToString();

                    if (!String.IsNullOrEmpty(this.Path))
                    {
                        // 去掉原前导路径
                        topath = topath.Replace(this.Path, String.Empty).TrimStart(PathDivChar);
                    }

                    string[] tpathArr = topath.Split(PathDivChar);

                    for (int i = 0; i < tpathArr.Length; i++)
                    {
                        tpathArr[i] = onMapping[tpathArr[i]].ToString();
                    }

                    // 组合新路径
                    tnpath = tpathArr.Join(PathDivChar);

                    // 替换Path
                    if (!String.IsNullOrEmpty(adjustPath))
                    {
                        tnpath = CombinePath(adjustPath, tnpath);
                    }

                    trow[Prop_Path] = tnpath;
                }
                else
                {
                    if (patype == PasteAsEnum.Sibling)
                    {
                        trow[Prop_ParentID] = target.ParentID;
                        trow[Prop_PathLevel] = target.PathLevel;
                        trow[Prop_SortIndex] = target.SortIndex + SortInterval;
                    }
                    else if (patype == PasteAsEnum.Child)
                    {
                        trow[Prop_ParentID] = target.ID;
                        trow[Prop_PathLevel] = target.PathLevel - 1;
                        trow[Prop_SortIndex] = SortStartIndex;
                    }

                    trow[Prop_Path] = adjustPath;
                }
            }

            return dt;
        }

        /// <summary>
        /// 改变当前节点的父节点
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="position">当前节点在新父节点的位置</param>
        protected virtual void ChangeParent(string targetID, int position)
        {
            ChangeParent(EntityBase<T>.Find(targetID), position);
        }

        /// <summary>
        /// 改变当前节点的父节点
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position">当前节点在新父节点的位置</param>
        protected virtual void ChangeParent(T target, int position)
        {
            ISession session = DataHelper.OpenHqlSession<T>();

            ChangeParent(session.Connection, target, position);
        }

        /// <summary>
        /// 改变当前节点的父节点
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="target"></param>
        /// <param name="position"></param>
        protected virtual void ChangeParent(IDbConnection conn, T target, int position)
        {
            // 目标节点必须存在且与当前父节点不同,当前节点不能为目标节点下层节点
            if (target != null && this.ParentID != target.ID && this.ID != target.ID
                && (String.IsNullOrEmpty(target.Path) || target.Path.IndexOf(this.ID) < 0))
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    try
                    {
                        string orgParentID = this.ParentID;
                        int orgSortIndex = this.SortIndex.GetValueOrDefault();

                        this.RefreshStructureInfo(target.ID, position);

                        string sql = String.Empty;

                        if (!String.IsNullOrEmpty(orgParentID))
                        {
                            // 调整源父节点的IsLeaf状态
                            sql = String.Format("UPDATE {0} SET {3} = 1 WHERE {1} = '{2}' AND NOT EXISTS(SELECT {1} FROM {0} WHERE {4} = '{2}'); ",
                                TableName, PrimaryKeyName, orgParentID,
                                Prop_IsLeaf, Prop_ParentID);

                            // 调整源父节点的子节点的SortIndex
                            sql += String.Format("UPDATE {0} SET {3} = {3} - 1 WHERE {4} = '{1}' AND {3} > {2} ",
                                TableName, orgParentID, orgSortIndex,
                                Prop_SortIndex, Prop_ParentID);
                        }
                        else
                        {
                            // 调整源父节点的子节点的SortIndex
                            sql = String.Format("UPDATE {0} SET {1} = {1} - 1 WHERE {2} IS NULL OR LTRIM(RTRIM({2})) = '' ",
                                TableName,
                                Prop_SortIndex, Prop_ParentID);
                        }

                        DataHelper.QueryValue(sql, conn);

                        trans.VoteCommit();
                    }
                    catch (Exception ex)
                    {
                        trans.VoteRollBack();

                        throw ex;
                    }
                }

                // 5.刷新节点状态
                this.Refresh();
                target.Refresh();
            }

        }

        /// <summary>
        /// 刷新（纠正）与指定节点相关的结构信息（父节点,节点自身的Path,PathLevel,SortIndex,IsLeaf属性)
        /// 前提：已调整ParentID和SortIndex
        /// <param name="target">需要调整信息的节点</param>
        /// <param name="sortIndex">节点的心SortIndex</param>
        /// </summary>
        private void RefreshStructureInfo(string newParentID, int newPosition)
        {
            this.ParentID = newParentID;
            this.RefreshParent();   // 刷新父节点

            int pMaxChildSortIndex = 0;
            int pPathLevel = 0;
            string pPath = String.Empty;

            if (this.Parent != null)
            {
                pPathLevel = this.Parent.PathLevel.GetValueOrDefault();
                pPath = this.Parent.Path;

                if (this.Parent.LastChild != null)
                {
                    pMaxChildSortIndex = this.Parent.LastChild.SortIndex.GetValueOrDefault();
                }
            }

            // 这里使用T-SQL语句
            string sql = String.Empty;

            if (String.IsNullOrEmpty(this.ParentID))
            {
                // 调整当前节点子孙节点
                sql = String.Format("UPDATE {0} SET [{4}] = REPLACE([{4}], '{1}', ''), {5} = {5} - {2} WHERE [{4}] LIKE '%{3}%'; ",
                    TableName, (this.Path + "."), this.PathLevel.GetValueOrDefault(), this.ID,
                    Prop_Path, Prop_PathLevel);

                // 调整当前节点
                sql += String.Format("UPDATE {0} SET {4} = NULL, [{5}] = NULL, {6} = 0, {7} = {1} + 1 WHERE {2} = '{3}'; ",
                    TableName, pMaxChildSortIndex, PrimaryKeyName, this.ID,
                    Prop_ParentID, Prop_Path, Prop_PathLevel, Prop_SortIndex);
            }
            else
            {
                int adjustLevel = pPathLevel - (this.PathLevel.GetValueOrDefault() - 1);    // 调整的路径级别
                string adjustPath = String.Empty;    // 调整的路径

                if (String.IsNullOrEmpty(pPath))
                {
                    adjustPath = this.ParentID;
                }
                else
                {
                    adjustPath = pPath + '.' + this.ParentID;
                }

                // 调整当前节点子孙节点
                if (String.IsNullOrEmpty(this.Path))
                {
                    sql = String.Format("UPDATE {0} SET [{4}] = '{1}.' + [{4}], {5} = {5} + {2} WHERE [{4}] LIKE '%{3}%'; ",
                        TableName, adjustPath, adjustLevel, this.ID,
                        Prop_Path, Prop_PathLevel);
                }
                else
                {
                    sql = String.Format("UPDATE {0} SET [{5}] = REPLACE([{5}], '{1}', '{2}'), {6} = {6} + {3} WHERE [{5}] LIKE '%{4}%'; ",
                        TableName, this.Path, adjustPath, adjustLevel, this.ID,
                        Prop_Path, Prop_PathLevel);
                }

                // 调整当前节点
                sql += String.Format("UPDATE {0} SET {7} = '{1}', [{8}] = '{2}', [{9}] = [{9}] + {3}, {10} = {4} WHERE {5} = '{6}'; ",
                    TableName, this.ParentID, adjustPath, adjustLevel, (pMaxChildSortIndex + 1), PrimaryKeyName, this.ID,
                    Prop_ParentID, Prop_Path, Prop_PathLevel, Prop_SortIndex);
            }

            // 调整父节点IsLeaf
            if (this.Parent != null && this.Parent.IsLeaf == true)
            {
                sql += String.Format("UPDATE {0} SET {3} = 0 WHERE {1} = '{2}'; ",
                    TableName, PrimaryKeyName, this.ParentID, Prop_IsLeaf);
            }

            DataHelper.QueryValue(sql);

            // 调整当前节点位置
            if (this.SortIndex != null && this.SortIndex > 0 && this.SortIndex < pMaxChildSortIndex)
            {
               this.ChangePosition(newPosition);
            }
        }

        /// <summary>
        /// 改变当前节点位置(同父节点)
        /// </summary>
        /// <param name="position"></param>
        public virtual void ChangePosition(int position)
        {
            ISession session = DataHelper.OpenHqlSession<T>();

            ChangePosition(session, position);
        }

        /// <summary>
        /// 改变当前节点位置(同父节点)
        /// </summary>
        /// <param name="session"></param>
        /// <param name="position"></param>
        [ActiveRecordTransaction]
        public virtual void ChangePosition(ISession session, int position)
        {
            if (SiblingNodes != null)
            {
                if (position < SortStartIndex)
                {
                    position = SortStartIndex;
                }
                else if (this.SiblingNodes != null && this.LastSibling != null && this.LastSibling.SortIndex < position)
                {
                    position = this.LastSibling.SortIndex.GetValueOrDefault() + 1;
                }

                int oPosition = this.SortIndex.GetValueOrDefault();

                string hql = String.Empty;

                if (position < oPosition)
                {
                    hql = String.Format("UPDATE {0} SET SortIndex = SortIndex + {1}  WHERE ParentID = '{2}' AND SortIndex < {3} AND SortIndex >= {4}; ",
                        EntityBase<T>.TableName, SortInterval, this.ParentID, oPosition, position);

                    DataHelper.HqlUpdate(session, hql);
                }
                else if (position > oPosition)
                {
                    hql = String.Format("UPDATE {0} SET SortIndex = SortIndex - {1}  WHERE ParentID = '{2}' AND SortIndex > {3} AND SortIndex <= {4}; ",
                        EntityBase<T>.TableName, SortInterval, this.ParentID, oPosition, position);

                    DataHelper.HqlUpdate(session, hql);
                }

                session.Flush();

                /*if (!String.IsNullOrEmpty(this.ID))
                {
                    hql += String.Format("UPDATE {0} SET SortIndex = {1} WHERE {2} = '{3}' ",
                        EntityBase<T>.ClassName, position, Prop_ID, this.ID);
                }

                DataHelper.QueryValue(hql, session.Connection);*/

                if (!String.IsNullOrEmpty(this.ID))
                {
                    hql = String.Format("UPDATE {0} SET SortIndex = {1} WHERE {2} = '{3}' ",
                        EntityBase<T>.ClassName, position, Prop_ID, this.ID);

                    DataHelper.HqlUpdate(session, hql);
                }

                // 刷新兄弟节点
                RefreshSiblingNodes();

                session.Flush();
            }
        }

        /// <summary>
        /// 刷新父节点
        /// </summary>
        protected virtual void RefreshParent()
        {
            _Parent = this.GetParent();
        }

        /// <summary>
        /// 刷新兄弟节点
        /// </summary>
        protected virtual void RefreshSiblingNodes()
        {
            _SiblingNodes = this.GetSiblingNodes();
        }

        /// <summary>
        /// 刷新子节点
        /// </summary>
        protected virtual void RefreshChildNodes()
        {
            _ChildNodes = this.GetChildNodes();
        }

        /// <summary>
        /// 获取父节点
        /// </summary>
        /// <returns></returns>
        protected virtual T GetParent()
        {
            if (!String.IsNullOrEmpty(this.ParentID))
            {
                return EntityBase<T>.Find(this.ParentID);
            }

            return null;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <returns></returns>
        protected virtual IList<T> GetChildNodes()
        {
            IList<T> nodes = EntityBase<T>.FindAllByProperty(Prop_ParentID, this.ID);

            if (nodes != null && nodes.Count > 0)
            {
                nodes = nodes.OrderBy(tent => tent.SortIndex).ToList();
            }

            return nodes;
        }

        /// <summary>
        /// 获取最小排序号子节点
        /// </summary>
        /// <returns></returns>
        protected virtual T GetFirstChild()
        {
            if (this.ChildNodes != null && this.ChildNodes.Count > 0)
            {
                return this.ChildNodes.First();
            }

            return null;
        }

        /// <summary>
        /// 获取最大排序号子节点
        /// </summary>
        /// <returns></returns>
        protected virtual T GetLastChild()
        {
            if (this.ChildNodes != null && this.ChildNodes.Count > 0)
            {
                return this.ChildNodes.Last();
            }

            return null;
        }

        /// <summary>
        /// 获取所有兄弟节点
        /// </summary>
        /// <returns></returns>
        protected virtual IList<T> GetSiblingNodes()
        {
            IList<T> nodes = new List<T>();

            if (String.IsNullOrEmpty(this.ParentID))
            {
                ICriterion crit = SearchHelper.UnionCriterions(
                    Expression.Eq(Prop_ParentID, String.Empty),
                    Expression.IsNull(Prop_ParentID));

                nodes = EntityBase<T>.FindAll(crit);
            }
            else
            {
                nodes = EntityBase<T>.FindAllByProperty(Prop_ParentID, this.ParentID);
            }

            if (nodes != null && nodes.Count > 0)
            {
                nodes = nodes.OrderBy(tent => tent.SortIndex).ToList();
            }

            return nodes;
        }

        /// <summary>
        /// 获取最小排序号兄弟节点(可能为自身)
        /// </summary>
        /// <returns></returns>
        protected virtual T GetFirstSibling()
        {
            if (SiblingNodes.Count() > 0)
            {
                return SiblingNodes.First();
            }

            return null;
        }

        /// <summary>
        /// 获取前置兄弟节点
        /// </summary>
        /// <returns></returns>
        protected virtual T GetPrevSibling()
        {
            if (SiblingNodes != null)
            {
                IEnumerable<T> psibs = SiblingNodes.Where(tent => tent.SortIndex < this.SortIndex);
                if (psibs.Count() > 0)
                {
                    // 从大到小排序的第一个
                    return psibs.OrderByDescending(tent => tent.SortIndex).First();
                }

                return null;
            }

            return null;
        }

        /// <summary>
        /// 获取所有前置兄弟节点
        /// </summary>
        /// <returns></returns>
        protected virtual IList<T> GetPrevSiblingNodes()
        {
            if (SiblingNodes != null)
            {
                IEnumerable<T> psibs = SiblingNodes.Where(tent => tent.SortIndex < this.SortIndex);

                if (psibs.Count() > 0)
                {
                    return psibs.ToList();
                }
            }

            return null;
        }

        /// <summary>
        /// 获取后继兄弟节点
        /// </summary>
        /// <returns></returns>
        protected virtual T GetNextSibling()
        {
            if (SiblingNodes != null)
            {
                IEnumerable<T> psibs = SiblingNodes.Where(tent => tent.SortIndex > this.SortIndex);
                if (psibs.Count() > 0)
                {
                    // 从小到大排序的第一个
                    return psibs.OrderBy(tent => tent.SortIndex).First();
                }

                return null;
            }

            return null;
        }

        /// <summary>
        /// 获取所有后继兄弟节点
        /// </summary>
        /// <returns></returns>
        protected virtual IList<T> GetNextSiblingNodes()
        {
            if (SiblingNodes != null)
            {
                IEnumerable<T> nsibs = SiblingNodes.Where(tent => tent.SortIndex > this.SortIndex);

                if (nsibs.Count() > 0)
                {
                    return nsibs.ToList();
                }
            }

            return null;
        }

        /// <summary>
        /// 获取最大排序号兄弟节点(可能为自身)
        /// </summary>
        /// <returns></returns>
        protected virtual T GetLastSibling()
        {
            if (SiblingNodes.Count() > 0)
            {
                return SiblingNodes.Last();
            }

            return null;
        }

        /// <summary>
        /// 交换两兄弟节点位置
        /// </summary>
        /// <param name="sibling1ID"></param>
        /// <param name="sibling2ID"></param>
        protected virtual void ExchangeSiblingPosition(string sibling1ID, string sibling2ID)
        {
            ExchangeSiblingPosition(EntityBase<T>.Find(sibling1ID), EntityBase<T>.Find(sibling2ID));
        }

        /// <summary>
        /// 交换两兄弟节点位置
        /// </summary>
        /// <param name="sibling1"></param>
        /// <param name="sibling2"></param>
        [ActiveRecordTransaction]
        protected virtual void ExchangeSiblingPosition(T sibling1, T sibling2)
        {
            int tSortIndex = sibling1.SortIndex.GetValueOrDefault();

            sibling1.SortIndex = sibling2.SortIndex;
            sibling2.SortIndex = tSortIndex;

            sibling1.UpdateAndFlush();
            sibling2.UpdateAndFlush();
        }

        /// <summary>
        /// 检查并设置叶节点状态
        /// </summary>
        /// <param name="excepedIDs">被排除检查的节点(一般用于事务中，事务未结束，删除的节点并没有在实际库中删除)</param>
        /// <returns></returns>
        protected virtual void CheckSetIsLeaf(params object[] excepedIDs)
        {
            bool isLeaf = this.CheckIsLeaf(excepedIDs);

            if (this.IsLeaf != isLeaf)
            {
                this.IsLeaf = isLeaf;

                this.Update();
            }
        }

        /// <summary>
        /// 检查是否叶节点
        /// </summary>
        /// <param name="excepedIDs">被排除检查的节点(一般用于事务中，事务未结束，删除的节点并没有在实际库中删除)</param>
        /// <returns></returns>
        protected virtual bool CheckIsLeaf(params object[] excepedIDs)
        {
            ICriterion crit = Expression.Eq(Prop_ParentID, this.ID);

            if (excepedIDs.Length > 0)
            {
                crit = SearchHelper.IntersectCriterions(crit, Expression.Not(Expression.In(Prop_ParentID, excepedIDs)));
            }

            bool isLeaf = !EntityBase<T>.Exists(crit);

            return isLeaf;
        }

        /// <summary>
        /// 检查是否为根节点
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckIsRoot()
        {
            return String.IsNullOrEmpty(this.ParentID) || PathLevel == RootNodeLevel;
        }

        /// <summary>
        /// 执行创建
        /// </summary>
        protected virtual void DoCreate()
        {
            this.IsLeaf = true; // 第一次添加，为叶节点

            base.CreateAndFlush();

            if (Parent != null)
            {
                if (Parent.IsLeaf != false)
                {
                    Parent.IsLeaf = false;
                    Parent.UpdateAndFlush();
                }
            }
        }

        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        private string CombinePath(string path1, string path2)
        {
            if (String.IsNullOrEmpty(path1))
            {
                return path2;
            }
            else if (String.IsNullOrEmpty(path2))
            {
                return path1;
            }
            else
            {
                return String.Format("{0}{1}{2}", path1.Trim(PathDivChar), PathDivChar, path2.Trim(PathDivChar));
            }
        }

        /// <summary>
        /// 根据Interval调整SortIndex
        /// </summary>
        /// <param name="sortIndex"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private int AdjustSortIndexByInterval(int sortIndex, int offset)
        {
            int newSortIndex = sortIndex;

            int mod = sortIndex % SortInterval;

            if (mod != 0)
            {
                newSortIndex = sortIndex - mod;
            }

            newSortIndex = newSortIndex + offset * SortInterval;

            return newSortIndex;
        }

        /// <summary>
        /// 由位置信息获取真正的SortIndex信息
        /// </summary>
        /// <param name="sortIndex"></param>
        /// <returns></returns>
        private int GetSortIndex(int position)
        {
            return position * SortInterval;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 生成新的标识
        /// </summary>
        /// <returns></returns>
        public static string GenerateNewID()
        {
            //PICIdentifierGenerator idGenerator = new PICIdentifierGenerator();

            // NHibernate.Engine.ISessionImplementor
            // return idGenerator.Generate(DataHelper.OpenHqlSession(), this).ToString();

            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 过滤掉子节点
        /// </summary>
        /// <returns></returns>
        public static IList<T> FilterChildNodes(IEnumerable<T> nodes)
        {
            IList<T> rtnNodes = new List<T>();

            foreach (T tnode in nodes)
            {
                // 节点路径中不包含任何节点的Id
                if (String.IsNullOrEmpty(tnode.Path) || nodes.Count(tent => tnode.Path.Contains(CLRHelper.ConvertValue<string>(tent.ID))) <= 0)
                {
                    rtnNodes.Add(tnode);
                }
            }

            return rtnNodes;
        }

        #endregion
    }
}
