using PIC.Data;
using PIC.Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal.Workflow
{
    /// <summary>
    /// 当前的工作流环境
    /// </summary>
    public class WfDataContext : IEntityContext
    {
        #region 成员

        private EasyCollection<IEntity> AddedList { get; set; }

        private EasyCollection<IEntity> ModifiedList { get; set; }

        private EasyCollection<IEntity> DeletedList { get; set; }

        #endregion

        #region 构造函数

        public WfDataContext()
        {
            AddedList = new EasyCollection<IEntity>();
            ModifiedList = new EasyCollection<IEntity>();
            DeletedList = new EasyCollection<IEntity>();
        }

        #endregion

        public void Register(IEntity entity, EntityObjectState state)
        {
            switch (state)
            {
                case EntityObjectState.Added:
                    this.AddedList.Add(entity);
                    break;
                case EntityObjectState.Modified:
                    this.ModifiedList.Add(entity);
                    break;
                case EntityObjectState.Deleted:
                    this.DeletedList.Add(entity);
                    break;
            }
        }

        /// <summary>
        /// 统一保存
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            foreach (var e in AddedList)
            {
                if (e is WfAction)
                {
                    ((WfAction)e).DoCreate();
                }
                else if (e is WfTask)
                {
                    ((WfTask)e).DoCreate();
                }
                else if (e is WfInstance)
                {
                    ((WfInstance)e).DoCreate();
                }
                else
                {
                    e.Create();
                }
            }

            foreach (var e in ModifiedList)
            {
                if (e is WfAction)
                {
                    ((WfAction)e).DoUpdate();
                }
                else if (e is WfTask)
                {
                    ((WfTask)e).DoUpdate();
                }
                else if (e is WfInstance)
                {
                    ((WfInstance)e).DoUpdate();
                }
                else
                {
                    e.Update();
                }
            }

            foreach (var e in DeletedList)
            {
                if (e is WfAction)
                {
                    ((WfAction)e).DoDelete();
                }
                else if (e is WfTask)
                {
                    ((WfTask)e).DoDelete();
                }
                else if (e is WfInstance)
                {
                    ((WfInstance)e).DoDelete();
                }
                else
                {
                    e.Delete();
                }
            }

            int count = AddedList.Count + ModifiedList.Count + DeletedList.Count;

            return count;
        }

        public void Dispose()
        {
            this.AddedList.Clear();
            this.ModifiedList.Clear();
            this.DeletedList.Clear();
        }
    }
}
