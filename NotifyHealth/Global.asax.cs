using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;
namespace NotifyHealth
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger("NotifyLog");
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
        }
        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            log.Debug("++++++++++++++++++++++++++++");
            log.Error("Exception - \n" + ex);
            log.Debug("++++++++++++++++++++++++++++");
            Session["Error"] = ex.Message;
            Server.ClearError();

            Response.Redirect("/Account/Error");


        }
    }
}
