using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC.Portal.Template
{
    [Serializable]
    public abstract class TemplateBase<T1>
    {
        public abstract T1 Generate(); 
    }
}
