using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;
using PIC.Data;

namespace PIC.Doc
{
    public static class DocDataHelper
    {
        #region Consts

        public const string MODEL_TYPE = "PIC.Doc.Model.DocModelBase`1, PIC.Doc.Model";

        #endregion

        public static ISession OpenSession()
        {
            Type type = Type.GetType(MODEL_TYPE);

            ISession session = DataHelper.OpenHqlSession(type);

            return session;
        }

        /// <summary>
        /// 获取当前ActiveRecordBase的数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetCurrentDbConnection()
        {
            Type type = Type.GetType(MODEL_TYPE);

            IDbConnection conn =DataHelper.GetCurrentDbConnection(type);

            return conn;
        }

        /// <summary>
        /// 获取Hql查询
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IQuery GetHqlQuery(string hql, params object[] parameters)
        {
            ISession session = OpenSession();

            return DataHelper.GetHqlQuery(session, hql, parameters);
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static DataTable QueryDataTable(string sqlString)
        {
            IDbConnection conn = DocDataHelper.GetCurrentDbConnection();

            DataTable dt = DataHelper.QueryDataTable(sqlString, conn);

            return dt;
        }

        /// <summary>
        /// 查询值列表
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static IList<object> QueryValueList(string sqlString)
        {
            DataTable dt = QueryDataTable(sqlString);

            return DataHelper.DataTableToValueList(dt);
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

            return DataHelper.DataTableToValueList(dt, valueField);
        }

        /// <summary>
        /// 执行无返回值的存储过程
        /// </summary>
        public static void ExecSp(string spName, params object[] parameters)
        {
            IDbConnection conn = DocDataHelper.GetCurrentDbConnection();

            DataHelper.ExecSp(conn, spName, parameters);
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
            DataTable dt = DocDataHelper.QueryDataTable(sqlString);

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
            DataTable dt = DocDataHelper.QueryDataTable(sqlString);

            return new EasyDictionary(dt);
        }

        /// <summary>
        /// 获取键值对列表
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static IList<EasyDictionary> QueryDictList(string sqlString)
        {
            DataTable dt = DocDataHelper.QueryDataTable(sqlString);

            return DataHelper.DataTableToDictList(dt);
        }

        /// <summary>
        /// 查询值
        /// </summary>
        /// <returns></returns>
        public static object QueryValue(string sqlString)
        {
            return DataHelper.QueryValue(sqlString, DocDataHelper.GetCurrentDbConnection());
        }

        /// <summary>
        /// 查询值
        /// </summary>
        /// <returns></returns>
        public static T QueryValue<T>(string sqlString)
        {
            return DataHelper.QueryValue<T>(sqlString, DocDataHelper.GetCurrentDbConnection());
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
        /// 获取对象数组列表
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IList<object[]> HqlQueryObjectsList(string hql, params object[] parameters)
        {
            ISession session = OpenSession();

            try
            {
                IQuery qry = DataHelper.GetHqlQuery(session, hql, parameters);

                return qry.List<object[]>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataHelper.ReleaseHqlSessin(session);
            }
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
        /// <param name="hql"></param>
        public static void HqlDelete(string hql)
        {
            ISession session = OpenSession();

            try
            {
                DataHelper.HqlDelete(session, hql);
            }
            finally
            {
                DataHelper.ReleaseHqlSessin(session);
            }
        }
    }
}