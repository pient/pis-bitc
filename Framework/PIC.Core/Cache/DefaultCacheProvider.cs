using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Caching
{
    public interface ICacheProvider
    {
        void Set(string objId, object o);
        void Remove(string objId);
        object Get(string objId);
    }

    /// <summary>
    /// the sample cache strategy implementation which 
    /// shows how you create an pluggable component for SAF.Cache 
    /// to customize the way object is cahced and retrieved.
    /// </summary>
    public class DefaultCacheProvider : ICacheProvider
    {
        private Hashtable objectTable;

        /// <summary>
        /// constructor to instantiate the internal hashtable.
        /// </summary>
        public DefaultCacheProvider()
        {
            objectTable = new Hashtable();
        }

        /// <summary>
        /// Add an object to the underlying storage
        /// </summary>
        /// <param name="objId">key for the object</param>
        /// <param name="o">object</param>
        public void Set(string objId, object o)
        {
            lock (objectTable)
            {
                if (objectTable.ContainsKey(objId))
                {
                    objectTable[objId] = o;
                }
                else
                {
                    objectTable.Add(objId, o);
                }
            }
        }
        /// <summary>
        /// Remove an object from the underlying storage
        /// </summary>
        /// <param name="objId">key for the object</param>
        public void Remove(string objId)
        {
            if (objectTable.ContainsKey(objId))
            {
                lock (objectTable)
                {
                    objectTable.Remove(objId);
                }
            }
        }
        /// <summary>
        /// Retrieve an object from the underlying storage
        /// </summary>
        /// <param name="objId">key for the object</param>
        /// <returns>object</returns>
        public object Get(string objId)
        {
            return objectTable[objId];
        }
    }
}
