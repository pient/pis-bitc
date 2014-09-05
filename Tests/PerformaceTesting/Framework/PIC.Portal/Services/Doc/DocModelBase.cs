using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using NHibernate;
using PIC.Data;
using PIC.Portal.Model;

namespace PIC.Doc.Model
{
    [Serializable]
    public abstract class DocModelBase<T> : ModelBase<T>, IModel, IEntity where T : DocModelBase<T>
    {
    }
}
