// Business class OrgType generated from OrgType
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
    public partial class OrgType
    {
        #region ��̬����

        /// <summary>
        /// ��ȡö��
        /// </summary>
        public static DataEnum GetOrgTypeEnum()
        {
            DataEnum de = new DataEnum();

            OrgType[] ents = OrgType.FindAll();

            foreach (OrgType ent in ents)
            {
                de.Add(ent.OrgTypeID.ToString(), ent.Name);
            }

            return de;
        }

        #endregion

    } // OrgType
}

