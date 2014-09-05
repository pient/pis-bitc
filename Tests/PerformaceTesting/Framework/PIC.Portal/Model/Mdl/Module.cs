// Business class Module generated from Module
// Creator: Ray
// Created Date: [2010-03-07]

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using NHibernate.Criterion;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using PIC.Data;
	
namespace PIC.Portal.Model
{
    [Serializable]
    public partial class Module
    {
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
        [ActiveRecordTransaction]
        protected override void DoCreate()
        {
            this.DoValidate();

            if (String.IsNullOrEmpty(this.ApplicationID))
            {
                Application app = Application.Get("PORTAL");

                this.ApplicationID = app.ApplicationID;
            }

            this.CreateDate = DateTime.Now;

            // ����ʼ
            base.DoCreate();

            Auth auth = new Auth();
            auth.CreateByModule(this);
        }

        /// <summary>
        /// �޸�ģ��(ͬʱ�޸�����Ϊ1����ӦȨ�޵�����)
        /// </summary>
        /// <returns></returns>
        [ActiveRecordTransaction]
        public void DoUpdate()
        {
            this.DoValidate();

            this.LastModifiedDate = DateTime.Now;

            Auth[] auths = this.GetRelatedAuth();

            this.UpdateAndFlush();

            if (auths.Length > 0)
            {
                foreach (Auth auth in auths)
                {
                    auth.UpdateByModule(this);
                }
            }
            else
            {
                Auth auth = new Auth();
                auth.CreateByModule(this);
            }
        }

        /// <summary>
        /// ɾ��ģ��
        /// </summary>
        public void DoDelete()
        {
            DataHelper.ExecSp("usp_Mdl_DeleteModule", "@_ModuleID", this.ModuleID);
        }

        /// <summary>
        /// ��ȡģ��������Ӧ��
        /// </summary>
        /// <returns></returns>
        public Application OwnerApplication()
        {
            if (!String.IsNullOrEmpty(ApplicationID))
            {
                return Application.Find(this.ApplicationID);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ��ȡ����Ȩ��
        /// </summary>
        /// <returns></returns>
        public Auth[] GetRelatedAuth()
        {
            // TypeΪ1 ����Ӧ��Ȩ��
            Auth[] auths = Auth.FindAllByProperties("Type", 1, "ModuleID", this.ModuleID);
            return auths;
        }

        #endregion

        #region ��̬����

        /// <summary>
        /// ��ģ�����ȡģ��
        /// </summary>
        /// <param name="code"></param>
        public static Module FindByCode(string code)
        {
            Module mdl = Module.FindFirstByProperties("Code", code);
            return mdl;
        }

        /// <summary>
        /// ����ɾ������
        /// </summary>
        public static void DoBatchDelete(params object[] args)
        {
            Module[] tents = Module.FindAll(Expression.In(Prop_ModuleID, args));

            foreach (Module tent in tents)
            {
                tent.DoDelete();
            }
        }

        #endregion

    } // Module
}

