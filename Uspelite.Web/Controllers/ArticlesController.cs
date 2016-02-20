namespace Uspelite.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Infrastructure.Mapping.Contracts;
    using Models.Articles;
    using Models.Home;
    using Services.Data.Contracts;
    using Data.Common.Roles;
    using Models.Categories;

    public class ArticlesController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly ICategoriesService categoriesService;

        public ArticlesController(IArticlesService articlesService, ICategoriesService categoriesService)
        {
            this.articlesService = articlesService;
            this.categoriesService = categoriesService;
        }

        public ActionResult Show(string slug)
        {
            var model = this.articlesService.GetBySlug(slug).To<ConcreteArticleViewModel>().FirstOrDefault();
            return this.View(model);
        }

        [HttpGet]
        [Authorize(Roles = AppRoles.CLIENT_ROLE)]
        public ActionResult Add()
        {
            var model = new ArticlesBindingModel();
            IEnumerable<SelectListItem> allCategories = this.Cache.Get(
                     "allCategories",//TODO: Add 
                     () => this.categoriesService
                            .GetAll().To<CategoryViewModel>()
                            .Select(x => new SelectListItem
                            {
                                Text = x.Title ,
                                Value = x.Id.ToString()
                            })
                            .AsEnumerable(), 2000);

            this.ViewBag.categories = allCategories;

            return this.View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.CLIENT_ROLE)]
        public ActionResult Add(ArticlesBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                //add to database

                this.TempData["Notification"] = "Вие добавихте статия успешно!";
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(model);
        }

    }
}