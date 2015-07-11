using System.Web;
using System.Web.Optimization;

namespace DotNet.CloudFarm.WebSite
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/common.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/houtaijs").Include(
            //        "~/Scripts/houtai/jquery*",
            //        "~/Scripts/houtai/jquery-*",
            //        "~/Scripts/houtai/bootstrap*",
            //        "~/Scripts/houtai/charisma.js",
            //        "~/Scripts/houtai/excanvas.js",
            //        "~/Scripts/houtai/fullcalendar.min.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/knockout-3.3.0.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/reset.css",
                      "~/Content/sheep_guest.css"));

            bundles.Add(new StyleBundle("~/Content/houtai/css").Include(
                "~/Content/houtai/bootstrap-cerulean.css",
                "~/Content/houtai/bootstrap-responsive.css",
                "~/Content/houtai/charisma-app.css",
                "~/Content/houtai/jquery-ui-1.8.21.custom.css",
                "~/Content/houtai/fullcalendar.css",
                "~/Content/houtai/fullcalendar.print.css",
                "~/Content/houtai/chosen.css",
                "~/Content/houtai/uniform.default.css",
                "~/Content/houtai/colorbox.css",
                "~/Content/houtai/jquery.cleditor.css",
                "~/Content/houtai/jquery.noty.css",
                "~/Content/houtai/noty_theme_default.css",
                "~/Content/houtai/elfinder.min.css",
                "~/Content/houtai/elfinder.theme.css",
                "~/Content/houtai/jquery.iphone.toggle.css",
                "~/Content/houtai/opa-icons.css",
                "~/Content/houtai/uploadify.css"
                ));
        }
    }
}
