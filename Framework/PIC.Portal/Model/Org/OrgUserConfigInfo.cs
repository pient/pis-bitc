using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIC.Portal.Model
{
    public class OrgUserConfigInfo
    {
        #region Properties

        public UserBasicConfig Basic { get; set; }

        public UserBpmConfig Bpm { get; set; }

        #endregion

        #region Constructors

        public OrgUserConfigInfo()
        {
            Basic = new UserBasicConfig();
            Bpm = new UserBpmConfig();
        }

        #endregion
    }

    public class UserBasicConfig
    {
        #region Properties

        /// <summary>
        /// 照片
        /// </summary>
        public MinFileInfo Picture { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public MinFileInfo Signature { get; set; }

        #endregion

        #region Constructors

        public UserBasicConfig()
        {
            Picture = new MinFileInfo();
        }

        #endregion
    }

    public class UserBpmConfig
    {
        #region Properties

        public string AgentID { get; set; }
        public string AgentName { get; set; }
        public DateTime? AgentStartDate { get; set; }
        public DateTime? AgentEndDate { get; set; }

        #endregion

        #region Constructors

        public UserBpmConfig()
        {
            AgentStartDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            AgentEndDate = DateTime.Now.AddDays(1);
        }

        #endregion
    }
}
