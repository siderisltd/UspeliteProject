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
        public int Id { get; set; }

        public int RateType { get { return (int) RateableType.Image; } }

        public int Rating { get; set; }

        public string Title { get; set; }

        public string Resized400Picture { get; set; }

        public string OriginalSizedPicture { get; set; }

        public bool IsMain { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Image, ImageViewModel>()
                         .ForMember(x => x.Resized400Picture, opt => opt.MapFrom(x => Constants.IMAGES_PREFIX_FROM_ROOT + x.PathResizedImage))
                         .ForMember(x => x.OriginalSizedPicture, opt => opt.MapFrom(x => Constants.IMAGES_PREFIX_FROM_ROOT + x.PathOriginalSize))
                         .ForMember(x => x.Rating, opt => opt.MapFrom(x => x.Ratings.Any() ? (x.Ratings.Sum(y => y.Value) / x.Ratings.Count) : 0));
        }


    }
}