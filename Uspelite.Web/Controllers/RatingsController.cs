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

                var allRates = this.ratesService.All();
                switch ((RateableType)ratingModel.RateType)
                {
                    case RateableType.Article:
                        var articleRate = allRates.FirstOrDefault(x => x.AuthorId == userId && x.ArticleId == ratingModel.RateId);
                        var articleRateToAdd = new Rate
                        {
                            ArticleId = ratingModel.RateId,
                            AuthorId = userId,
                            Value = ratingModel.RatePoints
                        };
                        returnMessage = this.AddRating(ratingModel.RatePoints, articleRate, articleRateToAdd);
                        break;
                    case RateableType.Video:
                        var videoRate = allRates.FirstOrDefault(x => x.AuthorId == userId && x.VideoId == ratingModel.RateId);
                        var videoRateToAdd = new Rate
                        {
                            VideoId = ratingModel.RateId,
                            AuthorId = userId,
                            Value = ratingModel.RatePoints
                        };
                        returnMessage = this.AddRating(ratingModel.RatePoints, videoRate, videoRateToAdd);
                        break;

                    case RateableType.Image:
                        var imageRate = allRates.FirstOrDefault(x => x.AuthorId == userId && x.PictureId == ratingModel.RateId);
                        var imageRateToAdd = new Rate
                        {
                            PictureId = ratingModel.RateId,
                            AuthorId = userId,
                            Value = ratingModel.RatePoints
                        };
                        returnMessage = this.AddRating(ratingModel.RatePoints, imageRate, imageRateToAdd);
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

        private string AddRating(int ratePoints, Rate existingRate, Rate rateToAdd)
        {
            string returnMessage;

            if (existingRate == null)
            {
                var addedId = this.ratesService.Add(rateToAdd);
                returnMessage = "Благодарим за вота! Вашият глас от {0} звезди беше отчетен";
            }
            else
            {
                existingRate.Value = ratePoints;
                this.ratesService.SaveChanges();
                returnMessage = "Благодарим за вота! Новият ви глас от {0} звезди беше отчетен";
            }
            return returnMessage;
        }


    }
}