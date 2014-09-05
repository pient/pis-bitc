using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Id;
using NHibernate.Engine;
using NHibernate.Criterion;

namespace PIC.Data
{
    public class PICIdentifierGenerator : IIdentifierGenerator
    {
        #region IIdentifierGenerator Members

        public object Generate(ISessionImplementor session, object obj)
        {
            return DataHelper.NewCombId().ToString();
        }

        #endregion
    }

    public class SequentialGuidGenerator : IIdentifierGenerator
    {
        #region IIdentifierGenerator Members

        public object Generate(ISessionImplementor session, object obj)
        {
            return DataHelper.NewCombId();
        }

        #endregion
    }
}
