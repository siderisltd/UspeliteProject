namespace Uspelite.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Infrastructure.Mapping.Contracts;
    using Models.ChildActions;
    using Models.Home;
    using Services.Data.Contracts;
    using Data.Models;
    using Models.Categories;

    [ChildActionOnly]
    public class ChildActionsController : Controller
    {
        private readonly IPostsService postsService;

        public ChildActionsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        //[OutputCache(Duration = 5 * 60, VaryByParam = "none")]
        public ActionResult GetSlider()
        {
            var model = this.postsService.GetTopCountPostsByRatingInEveryCategory(4).To<CategoryAndPostsViewModel>().ToList();


            return this.PartialView("_Slider", model);
        }
    }
}