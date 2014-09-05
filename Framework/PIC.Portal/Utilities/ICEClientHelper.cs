using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal.Utilities
{
    public static class ICEClientHelper
    {
        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int CheckPassword(string loginName, string password)
        {
            int result = 0;

            Idstar.CIdentityManager idStarObj = GetIdentityManagerInstance();

            if (idStarObj != null)
            {
                result = idStarObj.CheckPassword(loginName, password);
            }

            return result;
        }

        /// <summary>
        /// 获取IdentityManager
        /// </summary>
        /// <returns></returns>
        public static Idstar.CIdentityManager GetIdentityManagerInstance()
        {
            Type idMangerType = Type.GetTypeFromProgID("Idstar.IdentityManager");
            Idstar.CIdentityManager idStarObj = Activator.CreateInstance(idMangerType) as Idstar.CIdentityManager;


            return idStarObj;
        }
    }
}
