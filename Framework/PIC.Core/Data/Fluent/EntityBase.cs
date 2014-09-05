using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Data.Fluent
{
    [Serializable]
    public class EntityBase<T> : PIC.Data.EntityBase<T> where T : EntityBase<T>
    {
        #region 成员属性

        public virtual event PICPropertyChangedEventHandler PICPropertyChanged;

        #endregion

        #region 构造函数

        public EntityBase()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            PICPropertyChanged += new PICPropertyChangedEventHandler(OnPropertyChanged);
        }

        /// <summary>
        /// 属性发生变化时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPropertyChanged(object sender, PICPropertyChangedEventArgs e)
        {

        }

        public virtual void RaisePropertyChanged(string propertyName, object oldValue, object newValue)
        {
            if (PICPropertyChanged != null)
            {
                PICPropertyChanged(this, new PICPropertyChangedEventArgs(propertyName, oldValue, newValue));
            }
        }

        #endregion

        #region EntityBase<T> 成员

        /// <summary>
        /// 获取主键值
        /// </summary>
        public override object GetPrimaryValue()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 判断属性是否唯一
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public override bool IsPropertyUnique(string property)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
