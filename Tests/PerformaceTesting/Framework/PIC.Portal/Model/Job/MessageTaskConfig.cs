using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal.Model
{
    public class MessageTaskConfig : SysTaskConfig
    {
        #region 属性

        public string FromID { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public string Attachments { get; set; }
        public string ToGroupIDs { get; set; }
        public string ToUserIDs { get; set; }

        #endregion

        #region

        public MessageTaskConfig()
            : base()
        {
        }

        #endregion
    }
}
