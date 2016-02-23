namespace Uspelite.Web.Models.Images
{
    using System.Linq;
    using AutoMapper;
    using Common;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;
    using Services.Common;

    public class ImageViewModel : IRateable, IMapFrom<Image>, IHaveCustomMappings
    {
        private int rating = 0;

        public int Id { get; set; }

        public int RateType { get { return (int) RateableType.Image; } }

        public int? Rating
        {
            get { return this.rating; }
            set
            {
                if (value != null)
                {
                    this.rating = (int)value;
                }

            }
        }

        public string Title { get; set; }

        public string Resized400Picture { get; set; }

        public string OriginalSizedPicture { get; set; }

        public bool IsMain { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Image, ImageViewModel>()
                         .ForMember(x => x.Resized400Picture, opt => opt.MapFrom(x => Constants.IMAGES_PREFIX_FROM_ROOT + x.PathResizedImage))
                         .ForMember(x => x.OriginalSizedPicture, opt => opt.MapFrom(x => Constants.IMAGES_PREFIX_FROM_ROOT + x.PathOriginalSize))
                         .ForMember(x => x.Rating, opt => opt.MapFrom(x => x.Ratings.DefaultIfEmpty(new Rate { Value = 0 }).Sum(y => y.Value) / x.Ratings.DefaultIfEmpty(new Rate()).Count()));
        }


    }
}