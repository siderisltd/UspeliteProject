namespace Uspelite.Web.Models.Ratings
{
    using System.ComponentModel.DataAnnotations;

    public class RatingsBindingModel
    {
        public int RateId { get; set; }

        [Range(1, 5)]
        public int RatePoints { get; set; }

        public int RateType { get; set; }
    }
}