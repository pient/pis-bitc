using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;

namespace PIC.Data
{
    /// <summary>
    /// 键值对列表
    /// </summary>
    public class HqlKeyValueResultTransformer : IResultTransformer
    {
        #region IResultTransformer Members

        public System.Collections.IList TransformList(System.Collections.IList collection)
        {
            return collection; 
        }

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            KeyValuePairList pairs = new KeyValuePairList();

            for (int i = 0; i < tuple.Length; i++)
            {
                KeyValuePair<string, object> item = new KeyValuePair<string, object>(aliases[i], tuple[i]);

                pairs.Add(item);
            }

            return pairs;
        }

        #endregion
    }
}
