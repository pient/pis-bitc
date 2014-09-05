using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Biz.Model.OfficeSupply
{
    public class OfficeSupplyApySheet
    {
        public const string FORM_CODE = "FL_OA_OfficeSupplyApy";

        public string Title { get; set; }
        public string Remark { get; set; }
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }

        public OfficeSupplyItems Items { get; set; }
    }
}
