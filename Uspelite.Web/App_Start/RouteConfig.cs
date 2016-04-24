namespace Uspelite.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //            routes.MapRoute(
            //        name: "DefaultIndex",
            //        url: "{controller}/{action}",
            //        defaults: new { action = "Index" },
            //        namespaces: new string[] { "Uspelite.Web.Controllers" }
            //);

            routes.MapRoute(
                name: "SingleArticle",
                url: "{slug}",
                defaults: new { controller = "Articles", action = "Show" },
                namespaces: new string[] { "Uspelite.Web.Controllers" },
                constraints: new { slug = @"^([А-я A-z0-9""'$%#&№)(+=!?,.;:/ ]+-[А-я A-z0-9""'$%#&№)(+=!?,.;:/ ]*)+$" }
            );
            

            routes.MapRoute(
                name: "AuthorInfo",
                url: "Authors/Info/{authorId}/{authorName}",
                defaults: new { controller = "Authors", action = "Info" },
                namespaces: new string[] { "Uspelite.Web.Controllers" }
            );

            routes.MapRoute(
                name: "SingleCategory",
                url: "Categories/Show/{slug}",
                defaults: new { controller = "Categories", action = "Show" },
                namespaces: new string[] { "Uspelite.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Uspelite.Web.Controllers" }
            );
        }
    }
}
