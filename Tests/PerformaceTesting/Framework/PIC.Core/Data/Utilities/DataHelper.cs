using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using NHibernate;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using NHibernate.Cfg;

namespace PIC.Data
{
    public class DataHelper
    {
        #region 数据库操作

        public static ISessionFactoryHolder SessionFactoryHolder
        {
            get
            {
                return ActiveRecordMediator.GetSessionFactoryHolder();
            }
        }

        /// <summary>
        /// 获取HqlSession
        /// </summary>
        /// <returns></returns>
        public static ISession OpenHqlSession()
        {
            return OpenHqlSession<ActiveRecordBase>();
        }

        /// <summary>
        /// 获取HqlSession
        /// </summary>
        /// <returns></returns>
        public static ISession OpenHqlSession(IDbConnection conn)
        {
            return OpenHqlSession<ActiveRecordBase>(conn);
        }

        /// <summary>
        /// 获取HqlSession
        /// </summary>
        /// <returns></returns>
        public static ISession OpenHqlSession<T>()
        {
            return OpenHqlSession<T>(null);
        }

        /// <summary>
        /// 获取HqlSession
        /// </summary>
        /// <returns></returns>
        public static ISession OpenHqlSession<T>(IDbConnection conn)
        {
            ISessionFactory sessionFactory = DataHelper.SessionFactoryHolder.GetSessionFactory(typeof(T));
            ISession session = null;

            if (conn == null)
            {
                session = sessionFactory.OpenSession();
            }
            else
            {
                session = sessionFactory.OpenSession(conn);
            }

            return session;
        }

        /// <summary>
        /// 获取HqlSession
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ISession OpenHqlSession(Type type)
        {
            ISessionFactory sessionFactory = DataHelper.SessionFactoryHolder.GetSessionFactory(type);

            ISession session = sessionFactory.OpenSession();

            return session;
        }

        /// <summary>
        /// 释放Session
        /// </summary>
        /// <param name="type"></param>
        public static void ReleaseHqlSessin(ISession session)
        {
            if (session != null)
            {
                DataHelper.SessionFactoryHolder.ReleaseSession(session);
            }
        }

        /// <summary>
        /// 获取当前ActiveRecordBase的数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetCurrentDbConnection()
        {
            return DataHelper.GetCurrentDbConnection(typeof(ActiveRecordBase));
        }

        /// <summary>
        /// 获取指定类型ActiveRecordBase的数据库连接
        /// </summary>
        /// <param name="arBaseType"></param>
        /// <returns></returns>
        public static IDbConnection GetCurrentDbConnection(Type arBaseType)
        {
            return ActiveRecordMediator.GetSessionFactoryHolder()
                .GetSessionFactory(arBaseType).OpenStatelessSession().Connection;
        }

        /// <summary>
        /// 获取对象数组
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object[] HqlQueryObjects(string hql, params object[] parameters)
        {
            IList<object[]> list = HqlQueryObjectsList(hql, parameters);

            if (list.Count > 0)
            {
                return list[0];
            }

            return null;
        }

        /// <summary>
        /// 获取键值对列表(有缺陷的方法建议使用QueryKeyValuesList)
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IList<KeyValuePairList> HqlQueryKeyValuesList(string hql, params object[] parameters)
        {
            IList<object[]> rtnList = HqlQueryObjectsList(hql, parameters);
            string[] columnNames = QueryBuilder.GetColumnNames(hql);

            IList<KeyValuePairList> pairs = new List<KeyValuePairList>();

            foreach (object[] objs in rtnList)
            {
                KeyValuePairList pair = new KeyValuePairList();

                for (int i = 0; i < objs.Length; i++)
                {
                    pair.Add(new KeyValuePair<string, object>(columnNames[i], objs[i]));
                }

                pairs.Add(pair);
            }

            return pairs;
        }

        /// <summary>
        /// 获取对象数组列表
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IList<object[]> HqlQueryObjectsList(string hql, params object[] parameters)
        {
            ISession session = OpenHqlSession();

            try
            {
                IQuery qry = GetHqlQuery(session, hql, parameters);

                return qry.List<object[]>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ReleaseHqlSessin(session);
            }
        }

        /// <summary>
        /// 返回List
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IList HqlQueryList(string hql, params object[] parameters)
        {
            ISession session = OpenHqlSession();

            try
            {
                IQuery qry = GetHqlQuery(session, hql, parameters);

                return qry.List();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ReleaseHqlSessin(session);
            }
        }

        /// <summary>
        /// 返回List
        /// </summary>
        /// <param name="session"></param>
        /// <param name="hql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IList HqlQueryList(ISession session, string hql, params object[] parameters)
        {
            IQuery qry = GetHqlQuery(session, hql, parameters);

            return qry.List();
        }

        /// <summary>
        /// 执行更新操作
        /// </summary>
        /// <param name="session"></param>
        /// <param name="hql"></param>
        public static void HqlUpdate(ISession session, string hql, params object[] parameters)
        {
            IQuery qry = GetHqlQuery(session, hql, parameters);

            qry.ExecuteUpdate();
        }

        /// <summary>
        /// 执行更新操作
        /// </summary>
        /// <param name="hql"></param>
        public static void HqlUpdate(string hql, params object[] parameters)
        {
            IQuery qry = GetHqlQuery(hql, parameters);

            qry.ExecuteUpdate();
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        /// <param name="session"></param>
        /// <param name="hql"></param>
        public static void HqlDelete(ISession session, string hql)
        {
            try
            {
                session.Delete(hql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        /// <param name="hql"></param>
        public static void HqlDelete(string hql)
        {
            ISession session = OpenHqlSession();

            try
            {
                HqlDelete(session, hql);
            }
            finally
            {
                ReleaseHqlSessin(session);
            }
        }

        /// <summary>
        /// 获取Hql查询
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IQuery GetHqlQuery(string hql, params object[] parameters)
        {
            ISession session = OpenHqlSession();

            return GetHqlQuery(session, hql, parameters);
        }

        /// <summary>
        /// 获取Hql查询
        /// </summary>
        /// <param name="session"></param>
        /// <param name="hqlSqlString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IQuery GetHqlQuery(ISession session, string hql, params object[] parameters)
        {
            IQuery qry = session.CreateQuery(hql);
            
            for (int i = 0; i < parameters.Length; i++)
            {
                qry.SetParameter(i, parameters[i]);
            }

            return qry;
        }

        /// <summary>
        /// 拷贝数据到数据库
        /// </summary>
        public static void CopyDataToDatabase(DataTable dt, SqlConnection sqlConn, string targetTable)
        {
            try
            {
                SqlBulkCopy sbc = new SqlBulkCopy(sqlConn);
                sbc.DestinationTableName = targetTable;

                foreach (DataColumn dc in dt.Columns)
                {
                    sbc.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                }

                sbc.WriteToServer(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 执行无返回值的存储过程
        /// </summary>
        public static void ExecSp(string spName, params object[] parameters)
        {
            IDbConnection conn = DataHelper.GetCurrentDbConnection();

            ExecSp(conn, spName, parameters);
        }

        /// <summary>
        /// 执行无返回值的存储过程
        /// </summary>
        public static void ExecSp(IDbConnection conn, string spName, params object[] parameters)
        {
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (parameters.Length > 0)
            {
                if (parameters.Length % 2 != 0)
                {
                    throw new DataException("参数数量不匹配!");
                }
                else
                {
                    for (int i = 0; i < parameters.Length; )
                    {
                        IDbDataParameter dbDataParameter = cmd.CreateParameter();
                        dbDataParameter.ParameterName = parameters[i].ToString();
                        dbDataParameter.Value = parameters[i + 1];

                        cmd.Parameters.Add(dbDataParameter);

                        i = i + 2;
                    }
                }
            }

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 获取对象数组列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IList<object[]> QueryObjectsList(string sql)
        {
            DataTable dt = QueryDataTable(sql);
            IList<object[]> rtnList = new List<object[]>();

            foreach (DataRow row in dt.Rows)
            {
                object[] objArr = new object[dt.Columns.Count];

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    objArr[i] = row[i];
                }

                rtnList.Add(objArr);
            }

            return rtnList;
        }

        /// <summary>
        /// 获取键值对列表
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static IList<EasyDictionary> QueryDictList(string sqlString)
        {
            DataTable dt = QueryDataTable(sqlString);

            return DataTableToDictList(dt);
        }

        /// <summary>
        /// 获取键值对列表
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static IList<EasyDictionary> QueryDictList(string sqlString, IDbConnection conn)
        {
            DataTable dt = QueryDataTable(sqlString, conn);

            return DataTableToDictList(dt);
        }

        /// <summary>
        /// 查询值列表
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static IList<object> QueryValueList(string sqlString)
        {
            DataTable dt = QueryDataTable(sqlString);

            return DataTableToValueList(dt);
        }

        /// <summary>
        /// 查询值列表
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="valueField">值列</param>
        /// <returns></returns>
        public static IList<object> QueryValueList(string sqlString, string valueField)
        {
            DataTable dt = QueryDataTable(sqlString);

            return DataTableToValueList(dt, valueField);
        }

        /// <summary>
        /// 获取键值对
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="conn"></param>
        /// <param name="keyField">作为键的列</param>
        /// <param name="textField">作为显示值得列</param>
        /// <returns></returns>
        public static EasyDictionary QueryDict(string sqlString, string keyField, string textField)
        {
            DataTable dt = QueryDataTable(sqlString);

            return new EasyDictionary(dt, keyField, textField);
        }

        /// <summary>
        /// 获取键值对
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static EasyDictionary QueryDict(string sqlString)
        {
            DataTable dt = QueryDataTable(sqlString);

            return new EasyDictionary(dt);
        }

        /// <summary>
        /// 获取键值对
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="conn"></param>
        /// <param name="keyField">作为键的列</param>
        /// <param name="textField">作为显示值得列</param>
        /// <returns></returns>
        public static EasyDictionary QueryDict(string sqlString, IDbConnection conn, string keyField, string textField)
        {
            DataTable dt = QueryDataTable(sqlString, conn);

            return new EasyDictionary(dt, keyField, textField);
        }

        /// <summary>
        /// 获取键值对
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static EasyDictionary QueryDict(string sqlString, IDbConnection conn)
        {
            DataTable dt = QueryDataTable(sqlString, conn);

            return new EasyDictionary(dt);
        }

        /// <summary>
        /// 查询值
        /// </summary>
        /// <returns></returns>
        public static T QueryValue<T>(string sqlString)
        {
            return QueryValue<T>(sqlString, DataHelper.GetCurrentDbConnection());
        }

        /// <summary>
        /// 查询值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static T QueryValue<T>(string sqlString, IDbConnection conn)
        {
            T rtnobj = default(T);

            object obj = QueryValue(sqlString, conn);
            if (obj != null)
            {
                rtnobj = (T)obj;
            }

            return rtnobj;
        }

        /// <summary>
        /// 查询值
        /// </summary>
        /// <returns></returns>
        public static object QueryValue(string sqlString)
        {
            return QueryValue(sqlString, DataHelper.GetCurrentDbConnection());
        }

        /// <summary>
        /// 查询值
        /// </summary>
        /// <returns></returns>
        public static object QueryValue(string sqlString, IDbConnection conn)
        {
            object rtnobj = null;

            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlString;

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            rtnobj = cmd.ExecuteScalar();

            if (DBNull.Value == rtnobj)
            {
                return null;
            }

            return rtnobj;
        }

        /// <summary>
        /// 获取指定表结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetDataSchema(string tableName)
        {
            return GetDataSchema(tableName, GetCurrentDbConnection());
        }

        /// <summary>
        /// 获取指定表结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetDataSchema(string tableName, IDbConnection conn)
        {
            string schemasql = String.Format("SELECT * FROM {0} WHERE 1=0", tableName);

            return QueryDataTable(schemasql, conn);
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static DataTable QueryDataTable(string sqlString)
        {
            IDbConnection conn = DataHelper.GetCurrentDbConnection();

            return QueryDataTable(sqlString, conn);
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static DataTable QueryDataTable(string sqlString, IDbConnection conn)
        {
            try
            {
                DataTable dt = new DataTable();

                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlString;

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                IDataReader dreader = cmd.ExecuteReader();

                dt.Load(dreader);

                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region 实体操作

        /// <summary>
        /// 合并对象数据
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="entity2"></param>
        public static T MergeData<T>(T entity1, T entity2) where T : EntityBase<T>
        {
            foreach (PropertyInfo pi in EntityBase<T>.AllProperties)
            {
                if (pi.CanWrite)
                {
                    pi.SetValue(entity1, pi.GetValue(entity2, null), null);
                }
            }

            return entity1;
        }

        /// <summary>
        /// 合并对象数据(指定属性)
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="entity2"></param>
        public static T MergeData<T>(T entity1, T entity2, ICollection<string> keys) where T : EntityBase<T>
        {
            foreach (PropertyInfo pi in EntityBase<T>.AllProperties)
            {
                if (pi.CanWrite && keys.Contains(pi.Name))
                {
                    pi.SetValue(entity1, pi.GetValue(entity2, null), null);
                }
            }

            return entity1;
        }

        #endregion

        #region 数据操作

        /// <summary>
        /// 由DataTable 获取DictList
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<EasyDictionary> DataTableToDictList(DataTable dt)
        {
            IList<EasyDictionary> dicts = new List<EasyDictionary>();

            foreach (DataRow row in dt.Rows)
            {
                EasyDictionary dict = new EasyDictionary();

                foreach (DataColumn col in dt.Columns)
                {
                    dict.Set(col.ColumnName, row[col]);
                }

                dicts.Add(dict);
            }

            return dicts;
        }

        /// <summary>
        /// 获取值列表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<object> DataTableToValueList(DataTable dt)
        {
            string valueField = dt.Columns[0].ColumnName;

            return DataTableToValueList(dt, valueField);
        }

        /// <summary>
        /// 获取值列表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="valueField"></param>
        /// <returns></returns>
        public static IList<object> DataTableToValueList(DataTable dt, string valueField)
        {
            IList<object> vals = new List<object>();

            foreach (DataRow row in dt.Rows)
            {
                vals.Add(row[valueField]);
            }

            vals = vals.Distinct().ToList();

            return vals;
        }

        /// <summary>
        /// 转换DataTable到Xml
        /// </summary>
        /// <returns></returns>
        public static string DataTableToXMLItems(DataTable dataTable)
        {
            string xml = DataTableToXML(dataTable);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            if (xmlDoc.HasChildNodes)
            {
                xml = xmlDoc.FirstChild.InnerXml;
            }

            return xml;
        }

        /// <summary>
        /// 转换DataTable到Xml
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static string DataTableToXML(DataTable dataTable)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;
            try
            {
                stream = new MemoryStream();
                writer = new XmlTextWriter(stream, Encoding.Default);
                dataTable.WriteXml(writer);
                
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);
                UTF8Encoding utf = new UTF8Encoding();
                return utf.GetString(arr).Trim();
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }

        /// <summary>
        /// 转换Xml到DataSet
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static DataSet XMLToDataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmlData);
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch (Exception ex)
            {
                string strTest = ex.Message;
                return null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// 将DataTable实例转换成DataCollection实例
        /// </summary>
        /// <param name="dt">DataTable实例</param>
        /// <returns>DataCollection实例</returns>
        public static DataCollection DataTableToDataCollection(DataTable dt)
        {
            if (dt == null)
                return null;
            MemoryStream ms = new MemoryStream();
            StringBuilder inXml = new StringBuilder();
            StringWriter sw = new StringWriter(inXml);
            XmlTextWriter w = new XmlTextWriter(sw);

            w.Formatting = Formatting.Indented;

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                w.WriteStartElement("element");
                foreach (System.Data.DataColumn dc in dt.Columns)
                {
                    w.WriteStartAttribute(dc.ColumnName, null);
                    w.WriteString(dr[dc].ToString());
                    w.WriteEndAttribute();
                }
                w.WriteEndElement();
            }
            w.Flush();

            DataCollection dtList = new DataCollection("<collection>" + inXml.ToString() + "</collection>");

            ms.Close();
            w.Close();

            return dtList;
        }

        #endregion

        #region 编码操作

        /// <summary>
        /// 返回按特定的时间排序的GUID，可用于数据库ID以提高检索效率
        /// COMB (GUID 与时间混合型) 类型 GUID 数据
        /// </summary>
        /// <returns></returns>
        public static Guid NewCombId()
        {
            byte[] guidArray = System.Guid.NewGuid().ToByteArray();
            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));

            // Convert to a byte array
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new System.Guid(guidArray);
        }

        /// <summary>
        /// 从 CombId 中生成时间信息
        /// </summary>
        /// <param name="combId">包含时间信息的 CombId</param>
        /// <returns></returns>
        public static DateTime GetDateFromCombId(System.Guid combId)
        {
            DateTime baseDate = new DateTime(1900, 1, 1);

            byte[] daysArray = new byte[4];
            byte[] msecsArray = new byte[4];
            byte[] guidArray = combId.ToByteArray();

            // Copy the date parts of the guid to the respective byte arrays.
            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);

            // Reverse the arrays to put them into the appropriate order
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Convert the bytes to ints
            int days = BitConverter.ToInt32(daysArray, 0);
            int msecs = BitConverter.ToInt32(msecsArray, 0);
            DateTime date = baseDate.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);

            return date;
        }

        #endregion
    }
}
