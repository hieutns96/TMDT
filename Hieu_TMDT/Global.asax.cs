using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.IO;

namespace Hieu_TMDT
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application.Lock();
            Application["OnlineUsers"] = 0;
            Application.UnLock();
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            int a = 0;
            string filepath = Server.MapPath("~/global.txt");
            if (!File.Exists(filepath))
            {
                a++;
            }
            StreamReader read = new System.IO.StreamReader(filepath);
            a = int.Parse(read.ReadLine());
            read.Close();
            Application.Lock();
            Application["TotalView"] = a;
            int onlineUsers = int.Parse(Application["OnlineUsers"].ToString());
            Application["OnlineUsers"] = onlineUsers + 1;
            Application.UnLock();
            a++;
            System.IO.StreamWriter writer = new System.IO.StreamWriter(filepath);
            writer.WriteLine(a);
            writer.Close();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            int a = int.Parse(Application["OnlineUsers"].ToString());
            Application["OnlineUsers"] = a - 1;
            Application.UnLock();
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
