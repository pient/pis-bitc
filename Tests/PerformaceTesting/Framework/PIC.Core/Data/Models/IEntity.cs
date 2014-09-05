using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Data
{
    public interface IEntity
    {
        void Create();

        void Update();

        void Delete();
    }
}
