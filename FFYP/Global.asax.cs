using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FFYP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string con = ConfigurationManager.ConnectionStrings["FFYPContext"].ConnectionString;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SqlDependency.Start(con);
        }


        protected void Sesion_Start(object sender, EventArgs e)
        {
            RegisterComponent rc = new RegisterComponent();
            var currentTime = DateTime.Now;
            HttpContext.Current.Session["LastUpdate"] = currentTime;
            rc.RegisterNotification(currentTime);
        }
        protected void Application_End()
        {
            SqlDependency.Stop(con);
        }
    }
}
