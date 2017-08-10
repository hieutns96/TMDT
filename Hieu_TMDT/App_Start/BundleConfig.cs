using System.Web;
using System.Web.Optimization;

namespace Hieu_TMDT
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Clear();
            bundles.ResetAll();
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.js",
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/frameWorkJs").Include(
                        "~/Scripts/notify.js", "~/Scripts/knockout-3.3.0.js",
                        "~/Scripts/knockout.validation.min.js",
                         "~/Scripts/Chart.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));



            bundles.Add(new ScriptBundle("~/bundles/chatJs").Include(
                      "~/Scripts/ui/jquery.ui.core.js",
                     "~/Scripts/ui/jquery.ui.widget.js",
                     "~/Scripts/ui/jquery.ui.mouse.js",
                     "~/Scripts/ui/jquery.ui.draggable.js",
                     "~/Scripts/ui/jquery.ui.resizable.js",
                     "~/Scripts/jquery.signalR-2.2.0.js"));
        }
    }
}
