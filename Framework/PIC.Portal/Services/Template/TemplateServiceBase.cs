using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC.Portal.Model;

namespace PIC.Portal.Services
{
    public abstract class TemplateServiceBase<T1>
    {
        public abstract T1 Generate(Template template);
    }
}
