using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;

namespace PIC.Component.ThirdpartySupport.MsOffice
{
    /// <summary>
    /// MsProject WBS属性类别枚举
    /// </summary>
    public enum MsProjectWBSPropertyCatalogEnum
    {
        Project,
        Tasks,
        Task,
        PredecessorLink,
        OutlineCodes,
        OutlineCode,
        Masks,
        Mask,
        Values,
        Value,
        ValueList,
        ExtendedAttributes,
        ExtendedAttribute,
        Baseline,
        TimephasedData,
        Resources,
        Resource,
        AvailabilityPeriods,
        AvailabilityPeriod,
        Rates,
        Rate,
        Assignments,
        Assignment,
        WBSMasks,
        WBSMask,
        Calendars,
        Calendar,
        WeekDays,
        WeekDay,
        TimePeriod,
        WorkingTimes,
        WorkingTime,
        WorkWeeks,
        WorkWeek,
        Exceptions,
        Exception
    }

    /// <summary>
    /// Ms Office Project数据处理
    /// </summary>
    public class ProjectService : IDisposable
    {
        #region 枚举

        public enum VersionEnum
        {
            V2007,
            V2003
        }

        #endregion

        #region 私有成员

        public const string MSPROJECT_XML_NAMESPACE ="http://schemas.microsoft.com/project";

        private static ProjectService service;

        private ProjectProcessor _Processor2007 = null;
        private ProjectProcessor _Processor2003 = null;

        #endregion

        #region 成员属性

        #endregion

        #region 构造析构函数

        /// <summary>
        /// 单体模式
        /// </summary>
        private ProjectService()
        {

        }

        ~ProjectService()
        {
            this.Close();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取处理器
        /// </summary>
        /// <param name="v"></param>
        public static ProjectProcessor GetProcessor(VersionEnum v)
        {
            ProjectProcessor processor = null;

            switch (v)
            {
                case VersionEnum.V2003:
                    if (Instance._Processor2003 == null)
                    {
                        Instance._Processor2003 = new Project2003Processor();
                    }

                    processor = Instance._Processor2003;
                    break;
                case VersionEnum.V2007:
                    if (Instance._Processor2007 == null)
                    {
                        Instance._Processor2007 = new Project2007Processor();
                    }

                    processor = Instance._Processor2007;
                    break;
            }

            return processor;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 关闭服务
        /// </summary>
        private void Close()
        {
            if (_Processor2003 != null)
            {
                _Processor2003.Dispose();
            }

            if (_Processor2007 != null)
            {
                _Processor2007.Dispose();
            }
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 服务实例
        /// </summary>
        internal static ProjectService Instance
        {
            get
            {
                if (service == null)
                {
                    service = new ProjectService();
                }

                return service;
            }
        }

        /// <summary>
        /// 获取Ms Project数据表结构的DataSet形式
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static DataSet GetSchemaDataSet(VersionEnum v)
        {
            ProjectProcessor processor = GetProcessor(v);

            DataSet ds = processor.GetSchemaDataSet();
            // ds.DataSetName = MsProjectWBSPropertyCatalogEnum.Project.ToString();
            ds.Namespace = MSPROJECT_XML_NAMESPACE;

            return ds;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Close();
        }

        #endregion
    }
}
