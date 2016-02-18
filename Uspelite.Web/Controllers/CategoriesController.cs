namespace Uspelite.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Linq;
    using Infrastructure.Mapping.Contracts;
    using Models.Categories;
    using Services.Data.Contracts;

    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public ActionResult Show(string slug)
        {
            var model = this.categoriesService.GetBySlug(slug).To<CategoryViewModel>().FirstOrDefault();

            if(model == null)
            {
                throw new ArgumentNullException(string.Format("Searched category is null : [{0}]", slug));
            }

            model.TopArticle = model.Articles.FirstOrDefault();

            return this.View(model);
        }
    }
}