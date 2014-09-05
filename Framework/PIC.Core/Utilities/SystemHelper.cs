using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Management;
using PIC.Security;

namespace PIC
{
    public class SystemHelper
    {
        public static DateTime CurrentTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 获取方法堆栈调用信息
        /// </summary>
        /// <returns></returns>
        public static string GetStackTraceInfo()
        {
            string rtn = String.Empty;

            StackTrace st = new StackTrace(true);
            StackFrame[] sfs = st.GetFrames();

            foreach (StackFrame tsf in sfs)
            {
                rtn += tsf.GetMethod().ToString();
            }

            return rtn;
        }

        /// <summary>
        /// 由相对路径获取全路径
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetPath(string relativePath)
        {
            string path = GetPath();

            if (!String.IsNullOrEmpty(path))
            {
                path = Path.Combine(path, relativePath);
            }

            return path;
        }

        /// <summary>
        /// 获取系统路径
        /// </summary>
        public static string GetPath()
        {
            string path = null;

            if (System.Environment.CurrentDirectory != null)
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                // Mobile路径
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            }

            return path;
        }

        /// <summary>
        /// 获取机器码 (组合CPU编号，硬盘编号和网卡地址)
        /// </summary>
        /// <returns></returns>
        public static string GetMACCode()
        {
            try
            {
                string cpuid = GetCPUID();
                string hdid = GetHardDiskID();
                string mac = GetMACAddress();

                // 组合生成机器码
                string maccode_str = MD5Encrypt.Instance.GetMD5FromString(String.Format("{0}X:X{1}X:X{2}", cpuid, hdid, mac));

                Regex regex = new Regex(@"\W");
                maccode_str = regex.Replace(maccode_str, "6").ToUpper(); // 所有非字符替换成6, 并全部转换为大写

                // 拆成3组
                string[] m_arr = new string[6];
                m_arr[0] = maccode_str.Substring(0, 2);
                m_arr[1] = maccode_str.Substring(2, 2);
                m_arr[2] = maccode_str.Substring(4, 2);
                m_arr[3] = maccode_str.Substring(6, 2);
                m_arr[4] = maccode_str.Substring(8, 2);
                m_arr[5] = maccode_str.Substring(10, 2);

                string maccode = String.Format("{0}-{1}-{2}-{3}-{4}-{5}", m_arr[0], m_arr[1], m_arr[2], m_arr[3], m_arr[4], m_arr[5]);

                return maccode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取CPU编号
        /// </summary>
        /// <returns></returns>
        public static string GetCPUID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();

                String strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return strCpuID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取第一块硬盘编号
        /// </summary>
        /// <returns></returns>
        public static String GetHardDiskID()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                String strHardDiskID = null;

                foreach (ManagementObject mo in searcher.Get())
                {
                    strHardDiskID = mo["SerialNumber"].ToString().Trim();
                    break;
                }

                return strHardDiskID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取机器MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetMACAddress()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();

                if (moc.Count > 0)
                {
                    foreach (ManagementObject mo in moc)
                    {
                        if (mo["MacAddress"] != null)
                        {
                            mac = mo["MacAddress"].ToString();
                            break;
                        }

                        /*if ((bool)mo["IPEnabled"] == true)
                        {
                            mac = mo["MacAddress"].ToString();
                            break;
                        }*/
                    }
                }

                moc = null;
                mc = null;

                return mac;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
