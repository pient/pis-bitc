// Business class Auth generated from Auth
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Castle.ActiveRecord;
using PIC.Data;
using Newtonsoft.Json.Linq;
using NHibernate.Criterion;
	
namespace PIC.Portal.Model
{
    [Serializable]
    public partial class Auth
    {
        #region ˽�б���

        private bool? _IsLeaf = null;

        #endregion

        #region ��Ա����

        /// <summary>
        /// �Ƿ��ģ�飨û���ӽڵ㣩
        /// </summary>
        public override bool? IsLeaf
        {
            get
            {
                if (_IsLeaf == null && !String.IsNullOrEmpty(this.AuthID))
                {
                    bool isLeaf = !Auth.Exists("ParentID = ?", this.AuthID);
                    _IsLeaf = isLeaf;
                }

                return _IsLeaf.GetValueOrDefault();
            }
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��֤����
        /// </summary>
        public void DoValidate()
        {
            // ����Ƿ�����ظ���
            if (!this.IsPropertyUnique(Auth.Prop_Code))
            {
                throw new RepeatedKeyException("�����ظ��ı�� ��" + this.Code + "��");
            }
        }

        /// <summary>
        /// ����ģ��
        /// </summary>
        protected override void DoCreate()
        {
            this.DoValidate();

            this.CreateDate = DateTime.Now;

            // ����ʼ
            this.CreateAndFlush();
        }

        /// <summary>
        /// �޸�ģ��(ͬʱ�޸�����Ϊ2����ӦȨ�޵�����)
        /// </summary>
        /// <returns></returns>
        public void DoUpdate()
        {
            this.DoValidate();

            this.UpdateAndFlush();
        }

        /// <summary>
        /// ɾ��ģ��
        /// </summary>
        public void DoDelete()
        {
            base.DeleteAndFlush();
        }

        /// <summary>
        /// ��ϵͳӦ�ô���Ȩ��
        /// </summary>
        public void CreateByApplication(string appid)
        {
            Application app = Application.Find(appid);

            this.CreateByApplication(app);
        }

        /// <summary>
        /// ��ϵͳӦ�ô���Ȩ��
        /// </summary>
        public void CreateByApplication(Application app)
        {
            this.SetDataByApplication(app);  // ���ݶ�Ӧ��SysApplication��������

            this.DoCreate();
        }

        /// <summary>
        /// ��ϵͳӦ�ø���Ȩ��
        /// </summary>
        public void UpdateByApplication(string appid)
        {
            Application app = Application.Find(appid);

            this.UpdateByApplication(app);
        }

        /// <summary>
        /// ��ϵͳӦ�ø���Ȩ��
        /// </summary>
        public void UpdateByApplication(Application app)
        {
            this.SetDataByApplication(app);  // ���ݶ�Ӧ��SysApplication��������

            this.DoUpdate();
        }

        /// <summary>
        /// ��ϵͳģ�鴴��Ȩ��
        /// </summary>
        public void CreateByModule(string mdlid)
        {
            Module mdl = Module.Find(mdlid);

            this.CreateByModule(mdl);
        }

        /// <summary>
        /// ��ϵͳģ�鴴��Ȩ��
        /// </summary>
        public void CreateByModule(Module mdl)
        {
            this.SetDataByModule(mdl);  // ���ݶ�Ӧ��Module��������

            this.DoCreate();
        }

        /// <summary>
        /// ��ϵͳģ�����Ȩ��
        /// </summary>
        public void UpdateByModule(string mdlid)
        {
            Module mdl = Module.Find(mdlid);
            this.UpdateByModule(mdl);
        }

        /// <summary>
        /// ��ϵͳģ�����Ȩ��
        /// </summary>
        public void UpdateByModule(Module mdl)
        {
            this.SetDataByModule(mdl);  // ���ݶ�Ӧ��Module��������

            this.DoUpdate();
        }

        #endregion

        #region ��̬��Ա

        /// <summary>
        /// �ɱ����ȡAuth
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Auth Get(string code)
        {
            Auth[] tents = Auth.FindAllByProperty(Auth.Prop_Code, code);
            if (tents != null && tents.Length > 0)
            {
                return tents[0];
            }

            return null;
        }

        /// <summary>
        /// ��ȡĳ������Ȩ�޵ĸ�Ȩ��
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Auth GetRootAuth(int type)
        {
            Auth rootAuth = Auth.FindFirst(
                Expression.IsNull(Auth.Prop_ParentID),
                Expression.Eq(Auth.Prop_Type, type));

            return rootAuth;
        }

        /// <summary>
        /// ����ɾ������
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            Auth[] tents = Auth.FindAllByPrimaryKeys(args);

            foreach (Auth tent in tents)
            {
                tent.DoDelete();
            }
        }

        /// <summary>
        /// ��ID���ϻ�ȡȨ�޼���
        /// </summary>
        /// <param name="authIDs"></param>
        /// <returns></returns>
        public static IList GetAuthList(ICollection authIDs)
        {
            IList myAuthIDs = null;

            if (authIDs is JArray)
            {
                JArray arrAuths = authIDs as JArray;
                myAuthIDs = new List<string>(arrAuths.Values<string>());
            }
            else
            {
                myAuthIDs = authIDs as IList;
            }

            return myAuthIDs;
        }

        /// <summary>
        /// ��ID���ϻ�ȡȨ�޼���
        /// </summary>
        /// <param name="authIDs"></param>
        /// <returns></returns>
        public static ICollection<Auth> GetAuthByIDs(ICollection authIDs)
        {
            ICollection myAuthIDs = null;

            if (authIDs is JArray)
            {
                JArray arrAuths = authIDs as JArray;
                myAuthIDs = new List<string>(arrAuths.Values<string>());
            }
            else
            {
                myAuthIDs = authIDs;
            }

            Auth[] tAuths = Auth.FindAll(Expression.In("AuthID", myAuthIDs));

            return tAuths;
        }

        #endregion

        #region ֧�ַ���

        private void SetDataByApplication(Application app)
        {
            this.Name = app.Name;
            this.Code = String.Format("AUTH_APP_{0}", app.Code);
            this.Data = app.ApplicationID;
            this.Path = ".";
            this.PathLevel = 0;
            this.ParentID = null;
            this.Type = 1;  // 1Ϊϵͳ���Ȩ��
            this.Description = String.Format("Ӧ�� {0} ����Ȩ��", app.Name);
        }

        private void SetDataByModule(Module mdl)
        {
            Auth[] pAuths;
            Auth pAuth = null;

            // ��ȡ��Ȩ�ޣ���ϵͳģ���Ӧ��
            if (!String.IsNullOrEmpty(mdl.ParentID))
            {
                pAuths = Auth.FindAllByProperties("ModuleID", mdl.ParentID);
            }
            else
            {
                pAuths = Auth.FindAllByProperties("Data", mdl.ApplicationID);
            }

            if (pAuths.Length > 0)
            {
                pAuth = pAuths[0];
                this.ParentID = pAuth.AuthID;
                this.Path = String.Format("{0}.{1}", pAuth.Path, pAuth.AuthID);
                this.PathLevel = (pAuth.PathLevel == null ? 0 : (pAuth.PathLevel + 1));
            }

            this.Name = mdl.Name;
            this.Code = String.Format("AUTH_MDL_{0}", mdl.Code);
            this.Type = 1;  // 1Ϊϵͳ���Ȩ��
            this.SortIndex = mdl.SortIndex;
            this.ModuleID = mdl.ModuleID;
            this.Description = String.Format("ģ�� {0} ����Ȩ��", mdl.Name);
        }

        #endregion
    } // OrgAuth
}

