using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();

        IEntityContext Context { get; }
    }
}
