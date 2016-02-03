namespace Uspelite.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/Libraries/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/Libraries/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/Libraries/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/Libraries/bootstrap.js",
                      "~/Scripts/Libraries/bootstrap-datepicker.js",
                      "~/Scripts/Libraries/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                     "~/Scripts/Libraries/jquery.sidr.min.js",
                     "~/Scripts/Libraries/jquery.carouFredSel-6.2.1-packed.js",
                     "~/Scripts/Libraries/jquery.touchSwipe.min.js",
                     "~/Scripts/Libraries/jquery.photobox.js",
                     "~/Scripts/Libraries/functions.js"
                ));

            bundles.Add(new StyleBundle("~/theme/styles").Include(
                   "~/Content/Theme/styles/bootstrap.min.css",
                   "~/Content/Theme/styles/font-awesome.min.css",
                   "~/Content/Theme/styles/weather-icons.min.css",
                   "~/Content/Theme/styles/jquery.sidr.dark.css",
                   "~/Content/Theme/styles/photobox.css",
                   "~/Content/Theme/styles/datepicker.css",
                   "~/Content/Theme/styles/style.css",
                   "~/Content/Theme/styles/colors.css"

                ));


            bundles.Add(new StyleBundle("~/site/styles").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/JQueryUI/themes/base/jquery.ui.all.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                     "~/Scripts/Libraries/jquery.ui.components/jquery.ui.core.js",
                     "~/Scripts/Libraries/jquery.ui.components/jquery.ui.widget.js",
                     "~/Scripts/Libraries/jquery.ui.components/jquery.ui.mouse.js",
                     "~/Scripts/Libraries/jquery.ui.components/jquery.ui.draggable.js",
                     "~/Scripts/Libraries/jquery.ui.components/jquery.ui.resizable.js"
                  ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
