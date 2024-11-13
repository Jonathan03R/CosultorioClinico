using System.Web;
using System.Web.Mvc;

namespace Capa1_Presentacion.Web.AspNet.ModuloPrincipal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
