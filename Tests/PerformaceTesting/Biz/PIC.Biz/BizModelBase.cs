using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Castle.ActiveRecord;
using PIC.Data;
using PIC.Portal.Model;

namespace PIC.Biz.Model
{
    [Serializable]
    public abstract class BizModelBase<T> : ModelBase<T>, IModel, IEntity where T : BizModelBase<T>
    {
    }
}
