namespace Uspelite.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Linq;
    using Infrastructure.Mapping.Contracts;
    using Models.Categories;
    using Services.Data.Contracts;
    using Services.Data.DTO;

    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public ActionResult Show(string slug, int page = 1, int pageSize = 4)
        {
            var model = this.categoriesService.GetPaged(slug, page, pageSize).To<PageableCategoryArticlesViewModel>().FirstOrDefault();

            if(model == null)
            {
                throw new ArgumentNullException(string.Format("Searched category is null : [{0}]", slug));
            }

            model.Slug = slug;
            model.PageSize = pageSize;
            //model.TopArticle = model.Articles.FirstOrDefault();

            return this.View(model);
        }
    }
}