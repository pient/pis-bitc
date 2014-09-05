using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC
{
    /// <summary>
    /// 键值对列表
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class KeyValuePairList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
    {
    }

    /// <summary>
    /// 普通键值列表
    /// </summary>
    public class KeyValuePairList : KeyValuePairList<string, object>
    {
    }
}
