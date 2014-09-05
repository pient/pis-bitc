using PIC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Data
{
    public class EntityComparer<T> : IEqualityComparer<T> where T : EntityBase<T>
    {
        public bool Equals(T x, T y)
        {
            if (x == null)
            {
                return y == null;
            }

            if (x.GetPrimaryValue().Equals(y.GetPrimaryValue()))
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(T obj)
        {
            if (obj == null)
                return 0;
            else
                return obj.GetPrimaryValue().GetHashCode();
        }
    }
}
