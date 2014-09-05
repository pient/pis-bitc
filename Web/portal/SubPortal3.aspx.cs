using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PIC.Common;
using PIC.Portal;
using PIC.Portal.Web;
using PIC.Portal.Model;
using PIC.Portal.Web.UI;

namespace PIC.Portal.Web
{
    public partial class SubPortal3 : BasePage
    {
        #region 属性

        string mcode = String.Empty;

        Model.Module ent = null;
        IEnumerable<Model.Module> ents = null;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            mcode = RequestData.Get<string>("mcode", String.Empty);

            if (!String.IsNullOrEmpty(mcode))
            {
                ent = UserContext.AccessibleModules.FirstOrDefault(tent => tent.Code == mcode);

                if (ent != null)
                {
                    ents = UserContext.AccessibleModules.Where(tent => !String.IsNullOrEmpty(tent.Path) && tent.Path.IndexOf(ent.ModuleID) >= 0)
                        .OrderBy(tent => tent.PathLevel)
                        .OrderBy(tent => tent.SortIndex);

                    PageState.Add("Module", ent);
                    PageState.Add("Modules", ents);
                }
            }
        }
    }
}
