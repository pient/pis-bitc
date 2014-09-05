using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Data
{
    public class EditSensitiveEntityHelper
    {
        public const string FullStateString = "CRUD";
        public const string CreateStateString = "C";
        public const string ReadStateString = "R";
        public const string UpdateStateString = "U";
        public const string DeleteStateString = "D";

        /// <summary>
        /// 获取执行状态
        /// </summary>
        public static EditStatusEnum GetEditStatusFlag(string editStatusString)
        {
            try
            {
                return (EditStatusEnum)Enum.Parse(typeof(EditStatusEnum), editStatusString, true);
            }
            catch
            {
                return EditStatusEnum.Other;
            }
        }

        /// <summary>
        /// 检查编辑权限(默认可读)
        /// </summary>
        /// <param name="editStatus"></param>
        /// <returns></returns>
        public static bool CheckEditStatus(IEditSensitiveEntity ent, EditStatusEnum editStatus)
        {
            if (String.IsNullOrEmpty(ent.EditStatus))
            {
                return false;
            }

            switch (editStatus)
            {
                case EditStatusEnum.Create:
                    return CheckEditStatus(ent, CreateStateString);
                case EditStatusEnum.Update:
                    return CheckEditStatus(ent, UpdateStateString);
                case EditStatusEnum.Delete:
                    return CheckEditStatus(ent, DeleteStateString);
                case EditStatusEnum.Read:
                    return CheckEditStatus(ent, ReadStateString);
            }

            return false;
        }

        /// <summary>
        /// 检查编辑权限
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="editStatus"></param>
        /// <returns></returns>
        public static bool CheckEditStatus(IEditSensitiveEntity ent, string editStatusString)
        {
            if (String.IsNullOrEmpty(ent.EditStatus))
            {
                return false;
            }

            return ent.EditStatus.Contains(editStatusString);
        }

        /// <summary>
        /// 赋予所有权限
        /// </summary>
        public static void SetFullEditStatus(IEditSensitiveEntity ent)
        {
            ent.EditStatus = FullStateString;
        }

        /// <summary>
        /// 移除所有权限
        /// </summary>
        public static void RemoveFullEditStatus(IEditSensitiveEntity ent)
        {
            ent.EditStatus = String.Empty;
        }

        /// <summary>
        /// 设置编辑状态
        /// </summary>
        /// <param name="editStatus"></param>
        public static void SetEditStatus(IEditSensitiveEntity ent, EditStatusEnum editStatus)
        {
            if (ent.EditStatus == null)
            {
                ent.EditStatus = String.Empty;
            }

            switch (editStatus)
            {
                case EditStatusEnum.Create:
                    SetEditStatus(ent, CreateStateString);
                    return;
                case EditStatusEnum.Update:
                    SetEditStatus(ent, UpdateStateString);
                    return;
                case EditStatusEnum.Delete:
                    SetEditStatus(ent, DeleteStateString);
                    return;
                case EditStatusEnum.Read:
                    SetEditStatus(ent, ReadStateString);
                    return;
            }
        }

        /// <summary>
        /// 设置编辑状态
        /// </summary>
        /// <param name="editStatus"></param>
        public static void SetEditStatus(IEditSensitiveEntity ent, string editStatusString)
        {
            if (ent.EditStatus == null)
            {
                ent.EditStatus = String.Empty;
            }

            if (!CheckEditStatus(ent, editStatusString))
            {
                ent.EditStatus += editStatusString;
            }
        }

        /// <summary>
        /// 移除编辑权限
        /// </summary>
        /// <param name="editStatus"></param>
        public static void RemoveEditStatus(IEditSensitiveEntity ent, EditStatusEnum editStatus)
        {
            switch (editStatus)
            {
                case EditStatusEnum.Create:
                    RemoveEditStatus(ent, CreateStateString);
                    return;
                case EditStatusEnum.Update:
                    RemoveEditStatus(ent, UpdateStateString);
                    return;
                case EditStatusEnum.Delete:
                    RemoveEditStatus(ent, DeleteStateString);
                    return;
                case EditStatusEnum.Read:
                    RemoveEditStatus(ent, ReadStateString);
                    return;
            }
        }

        /// <summary>
        /// 移除编辑权限
        /// </summary>
        /// <param name="editStatus"></param>
        public static void RemoveEditStatus(IEditSensitiveEntity ent, string editStatusString)
        {
            if (String.IsNullOrEmpty(ent.EditStatus))
            {
                return;
            }

            ent.EditStatus = ent.EditStatus.Replace(editStatusString, String.Empty);
        }
    }
}
