using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Biz.Model.OfficeSupply
{
    public class OfficeSupplyItems : List<OfficeSupplyItem>
    {
    }

    public class OfficeSupplyItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public float Quantity { get; set; }
        public string Unit { get; set; }
    }
}
