using PIC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC
{
    public enum EntityObjectState
    {
        Added,
        Modified,
        Deleted
    }

    public interface IEntityContext : IDisposable
    {
        void Register(IEntity entity, EntityObjectState state);

        int SaveChanges();
    }
}
