using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Id;
using NHibernate.Engine;

namespace PIC.Portal.Model
{
    public class IdentifierGenerator : NumberGenerator, IIdentifierGenerator
    {
        #region NumberGenerator成员

        public override string Generate()
        {
            // return base.Generate();
            return Guid.NewGuid().ToString();
        }

        #endregion

        #region IIdentifierGenerator Members

        public object Generate(ISessionImplementor session, object obj)
        {
            // return Guid.NewGuid().ToString();
            string idstr = this.Generate();

            return idstr;
        }

        #endregion
    }
}
