using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Data.XQuery
{
    public interface IXQueryProvider
    {
        /// <summary>
        /// 查询并返回string
        /// </summary>
        /// <param name="qrystring"></param>
        /// <returns></returns>
        string Query(string qrystring);

        string Query<T>(EntityBase<T> entity, string field, string path) where T : EntityBase<T>;

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="qrystring"></param>
        void Modify(string qrystring);
    }
}
