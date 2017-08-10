using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Hieu_TMDT
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Product",
                url: "{controller}/{action}/{id}/{TenSach}",
                defaults: new { controller = "Product", action = "Product", id = UrlParameter.Optional ,TenSach = UrlParameter.Optional}
            );

            routes.MapRoute(
                 name: "Catalog",
                 url: "{controller}/{action}/{id}/{TenSach}",
                 defaults: new { controller = "Catalog", action = "ViewCatalog", id = UrlParameter.Optional, TenSach = UrlParameter.Optional }
             );

           
        }
    }
}
