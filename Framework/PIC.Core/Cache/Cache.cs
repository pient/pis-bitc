using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIC.Caching
{
	public class Cache
	{
        protected static ICacheProvider cs;
        protected static Cache cache;

		/// <summary>
		/// Private construtor, required for singleton design pattern.
		/// </summary>
		protected Cache()
		{
            cs = new DefaultCacheProvider();
		}

		/// <summary>
		/// Singlton method used to return the instance of Cache class
		/// </summary>
		/// <returns></returns>
		protected static Cache GetInstance()
		{
			if (cache == null)
			{
				cache = new Cache();
			}

			return cache;
		}

		/// <summary>
		/// Add the object to the underlying storage and Xml mapping document
		/// </summary>
		/// <param name="path">the hierarchical location of the object in Xml document </param>
		/// <param name="o">the object to be cached</param>
		public virtual void Set(string path, object o)
		{
			string newpath = PreparePath(path);

            cs.Set(newpath, o);
		}

		/// <summary>
		/// Retrieve the cached object using its hierarchical location
		/// </summary>
		/// <param name="xpath">hierarchical location of the object in xml document</param>
		/// <returns>cached object </returns>
		public virtual object Get(string path)
        {
            string newpath = PreparePath(path);

            object o = cs.Get(newpath);

			return o;
		}

		/// <summary>
		/// Remove the object from the storage and clear the Xml assocated with
		/// the object
		/// </summary>
		/// <param name="path">hierarchical locatioin of the object</param>
		public virtual void Remove(string path)
        {
            string newpath = PreparePath(path);

            cs.Remove(newpath);
		}

		/// <summary>
		/// clean up the xpath so that extra '/' is removed
		/// </summary>
		/// <param name="path">hierarchical location</param>
		/// <returns></returns>
        private string PreparePath(string path)
		{
			string[] pathArray = path.Split('/');
			path ="/Cache";
            foreach (string s in pathArray)
			{
				if (s != "")
				{
					path = path + "/" + s ;
				}
			}
			return path;
		}
    }
}
