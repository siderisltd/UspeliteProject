namespace Uspelite.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using ActionFilters;
    using Data.Models;
    using Microsoft.AspNet.Identity;
    using Models.Common;
    using Models.Ratings;
    using Services.Data.Contracts;

    public class RatingsController : BaseController
    {
        private readonly IRatesService ratesService;

        public RatingsController(IRatesService ratesService)
        {
            this.ratesService = ratesService;
        }

        [HttpPost]
        [AjaxActionFilter]
        public ActionResult Rate(RatingsBindingModel ratingModel)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.Json(new { message = "За да гласувате трябва да сте регистриран потребител" });
            }
            if (this.ModelState.IsValid)
            {
                var userId = this.User.Identity.GetUserId();
                string returnMessage = string.Empty;

                switch ((RateableType)ratingModel.RateType)
                {
                    case RateableType.Article:
                        var vote = this.ratesService
                            .All()
                            .FirstOrDefault(x => x.AuthorId == userId && x.ArticleId == ratingModel.RateId);

                        if (vote == null)
                        {
                            var rateToAdd = new Rate
                            {
                                ArticleId = ratingModel.RateId,
                                AuthorId = userId,
                                Value = ratingModel.RatePoints
                            };

                            var addedId = this.ratesService.Add(rateToAdd);

                            returnMessage = "Благодарим за вота! Вашият глас от {0} звезди беше отчетен";
                        }
                        else
                        {
                            vote.Value = ratingModel.RatePoints;
                            this.ratesService.SaveChanges();
                            returnMessage = "Благодарим за вота! Новият ви глас от {0} звезди беше отчетен";
                        }

                        break;

                    default:
                        //return some error for invalid RateableType
                        break;
                }

                return this.Json(new { message = string.Format(returnMessage, ratingModel.RatePoints) });
            }
            else
            {
                return this.Json(new {message = "Грешка при гласуването !" });
            }
        }
    }
}