using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Data.XQuery
{
    public class SqlXQueryProvider : IXQueryProvider
    {
        public string Query(string qrystr)
        {
            throw new NotImplementedException();
        }

        public string Query<T>(EntityBase<T> entity, string field, string path) where T : EntityBase<T>
        {
            // 默认所有的主键是字符串类型
            string qrystr = String.Format("SELECT {0}.query('{1}') FROM {2} WHERE {3} = '{4}'", field, path, 
                EntityBase<T>.TableName, EntityBase<T>.PrimaryKeyName, entity.GetPrimaryValue());

            string data = DataHelper.QueryValue(qrystr) as String;

            return data;
        }

        public void Modify(string qrystring)
        {
            throw new NotImplementedException();
        }
    }
}
