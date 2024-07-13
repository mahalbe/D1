using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;
using System.Globalization;
using System.Threading;
using System.Configuration;
using System.Data.SqlClient;

namespace ISas.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        string connString = ConfigurationManager.ConnectionStrings["iSASDB"].ConnectionString;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            Bootstrapper.Initialise();
            GlobalFilters.Filters.Add(new HandleErrorAttribute()); // Adding this to handle error by Shailendra Kumar on 02-Dec-2017
            WebSecurity.InitializeDatabaseConnection("iSASDB", "Users", "Id", "UserName", autoCreateTables: true);

            //Start SqlDependency with application initialization
                //SqlDependency.Start(connString);
        }

        protected void Application_BeginRequest()
        {
            if (!Context.Request.IsSecureConnection && !Context.Request.IsLocal) // to avoid switching to https when local testing
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["SwithcToHttps"]))
                    Response.Redirect(Context.Request.Url.ToString().Insert(4, "s"));  // Only insert an "s" to the "http:", and avoid replacing wrongly http: in the url parameters
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture = newCulture;
            CultureInfo ci = new CultureInfo("en-IN");
            ci.NumberFormat.CurrencySymbol = "₹";
            Thread.CurrentThread.CurrentCulture = ci;

        }

        protected void Application_End()
        {
            SqlDependency.Stop(connString);
        }
    }
}