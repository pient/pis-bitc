using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC.Data;
using PIC.Common;
using Newtonsoft.Json;

namespace PIC.Portal.Model
{
    /// <summary>
    /// Model 接口
    /// </summary>
    public interface IModel
    {
        UserInfo UserInfo { get; }
    }

    [Serializable]
    public class ModelBase<T> : EntityBase<T>, IModel where T : ModelBase<T>
    {
        /// <summary>
        /// 当前用户信息
        /// </summary>
        [JsonIgnore]
        public UserInfo UserInfo
        {
            get
            {
                return PortalService.CurrentUserInfo;
            }
        }
    }
}
