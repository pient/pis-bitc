using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Pattern
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly IEntityContext context;

        #region Constructors

        public UnitOfWork(IEntityContext ctx)
        {
            context = ctx;
        }

        #endregion

        #region IUnitOfWork Members

        public virtual int Commit()
        {
            return Context.SaveChanges();
        }

        public virtual IEntityContext Context
        {
            get { return context; }
        }

        public virtual void Dispose()
        {
            Context.Dispose();
        }

        #endregion
    }

    public class UnitOfWork<TContext> : UnitOfWork where TContext : IEntityContext, new()
    {
        #region Constructors

        public UnitOfWork() : base(new TContext())
        {
        }

        #endregion
    }
}
