﻿namespace Uspelite.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "AuthorInfo",
                url: "Authors/Info/{authorId}/{authorName}",
                defaults: new { controller = "Authors", action = "Info" }
            );

            routes.MapRoute(
                name: "SingleCategory",
                url: "Categories/Show/{slug}",
                defaults: new { controller = "Categories", action = "Show" }
            );

            routes.MapRoute(
                name: "SingleArticle",
                url: "Articles/Show/{slug}",
                defaults: new { controller = "Articles", action = "Show" }
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
