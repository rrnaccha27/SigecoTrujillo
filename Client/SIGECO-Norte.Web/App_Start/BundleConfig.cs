using System.Web;
using System.Web.Optimization;

namespace SIGEES.Web.App_Start
{
    public class BundleConfig
    {
        // Para obtener más información acerca de Bundling, consulte http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            /*
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
            */
            bundles.Add(new ScriptBundle("~/bundles/easyui").Include(
                "~/Content/easyui/jquery.easyui.min.js",
                "~/Scripts/shortcut.js",
                "~/Content/easyui/locale/easyui-lang-es.js",
                "~/Content/easyui/date.js"
            ));
             
            bundles.Add(new StyleBundle("~/Content/easyui").Include(
                "~/Content/easyui/themes/default/easyui.css",
                "~/Content/easyui/themes/icon.css",
                "~/Content/easyui/demo.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/easyui.mobile").Include(
                "~/Content/easyui/jquery.min.js",
                "~/Content/easyui/jquery.easyui.min.js",
                "~/Content/easyui/jquery.easyui.mobile.js",
                "~/Content/easyui/locale/easyui-lang-es.js"
            ));

            bundles.Add(new StyleBundle("~/Content/easyui.mobile").Include(
                "~/Content/easyui/themes/mobile.css",
                "~/Content/easyui/themes/icon.css",
                "~/Content/easyui/themes/metro/easyui.css"
            ));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
               "~/Content/bootstrap.css.css"
           ));
            bundles.Add(new ScriptBundle("~/bundles/Aditional").Include(
               "~/Scripts/Aditional/jquery.lib.page.js",
                "~/Scripts/Aditional/jquery.lib.message.js"
           ));
            
            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                "~/Content/sigees/jquery/jquery.lib.js",
                "~/Content/sigees/jquery/jquery.lib.page.js"
            ));            
           
        }
    }
}