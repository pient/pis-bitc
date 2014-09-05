using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using PIC.Data;
using PIC.Doc;
using PIC.Doc.Model;
	
namespace PIC.Portal.Model
{
    public class DataSignature
    {
        public string Name { get; set; }
        public IFileInfo File { get; set; }
    }

    /// <summary>
    /// 用户签名
    /// </summary>
    public class OrgUserSignature : DataSignature
    {
        public string UserID { get; set; }
    }

    /// <summary>
    /// 自定义实体类
    /// </summary>
    [Serializable]
	public partial class OrgUserConfig
    {
        #region Consts & Enums

        public const string UserPicDirCode = "SYS.File.User.Picture";
        public const string UserSignDirCode = "SYS.File.User.Signature";

        #endregion

        #region 成员变量

        [NonSerialized]
        protected OrgUserConfigInfo cfgInfo;

        #endregion

        #region 成员属性

        /// <summary>
        /// 流程定义状态
        /// </summary>
        [JsonIgnore]
        public OrgUserConfigInfo ConfigInfo
        {
            get
            {
                if (cfgInfo == null)
                {
                    if (String.IsNullOrEmpty(this.Data))
                    {
                        cfgInfo = new OrgUserConfigInfo();
                    }
                    else
                    {
                        cfgInfo = JsonHelper.GetObject<OrgUserConfigInfo>(this.Data);
                    }
                }

                return cfgInfo;
            }

            internal set { this.cfgInfo = value; }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 验证操作
        /// </summary>
        public void DoValidate()
        {
            // 用户已经拥有配置数据,目前只支持用户单配置
            if (!this.IsPropertyUnique(OrgUserConfig.Prop_UserID))
            {
                throw new RepeatedKeyException("用户已经拥有配置数据。" );
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void DoSave()
        {
            if (String.IsNullOrEmpty(ConfigID))
            {
                this.DoCreate();
            }
            else
            {
                this.DoUpdate();
            }
        }

        /// <summary>
        /// 创建操作
        /// </summary>
        public void DoCreate()
        {
            this.DoValidate();

            this.OnDataSave();

            // 事务开始
            this.CreateAndFlush();
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            this.DoValidate();

            this.LastModifiedTime = DateTime.Now;

            this.OnDataSave();

            this.UpdateAndFlush();
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        public void DoDelete()
        {
            this.Delete();
        }

        #endregion
        
        #region 静态成员
        
        /// <summary>
        /// 批量删除操作
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
			OrgUserConfig[] tents = OrgUserConfig.FindAll(Expression.In("ConfigID", args));

			foreach (OrgUserConfig tent in tents)
			{
				tent.DoDelete();
			}
        }
        
        #endregion

        #region 支持方法

        /// <summary>
        /// 归档签名和照片
        /// </summary>
        private void OnDataSave()
        {
            if (!String.IsNullOrEmpty(this.ConfigInfo.Basic.Picture.FileID))
            {
                var fid = this.ConfigInfo.Basic.Picture.FileID.ToGuid();

                if (fid != null && !DocFile.Exists(fid))
                {
                    var tmpFile = TempFileData.Find(fid);

                    var docFile = DocService.ArchiveTemp(fid.Value, UserPicDirCode);

                    this.ConfigInfo.Basic.Picture.FileID = docFile.FileID.ToString();
                    this.ConfigInfo.Basic.Picture.Name = docFile.Name;
                }
            }

            if (this.ConfigInfo.Basic.Signature != null && !String.IsNullOrEmpty(this.ConfigInfo.Basic.Signature.FileID))
            {
                var fid = this.ConfigInfo.Basic.Signature.FileID.ToGuid();

                if (fid != null && !DocFile.Exists(fid))
                {
                    var tmpFile = TempFileData.Find(fid);

                    var docFile = DocService.ArchiveTemp(fid.Value, UserSignDirCode);

                    this.ConfigInfo.Basic.Signature.FileID = docFile.FileID.ToString();
                    this.ConfigInfo.Basic.Signature.Name = docFile.Name;
                }
            }

            this.Data = JsonHelper.GetJsonString(this.cfgInfo);
        }

        /// <summary>
        /// 获取备份配置
        /// </summary>
        /// <returns></returns>
        private OrgUserConfigInfo GetBackConfigInfo()
        {
            var cfg = new OrgUserConfigInfo();

            if (!String.IsNullOrEmpty(this.BakData))
            {
                cfgInfo = JsonHelper.GetObject<OrgUserConfigInfo>(this.BakData);
            }

            return cfg;
        }

        #endregion

    } // OrgUserConfig
}


