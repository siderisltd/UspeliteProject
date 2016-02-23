namespace Uspelite.Web.Models.Articles
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;
    using Services.Common;

    public class ArticleImageViewModel : IMapFrom<Image>, IMapTo<Image>, IHaveCustomMappings
    {
        public string Title { get; set; }

        public string Resized400Picture { get; set; }

        public string OriginalSizedPicture { get; set; }

        public bool IsMain { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Image, ArticleImageViewModel>()
                         .ForMember(x => x.Resized400Picture, opt => opt.MapFrom(x => Constants.IMAGES_PREFIX_FROM_ROOT + x.PathResizedImage))
                         .ForMember(x => x.OriginalSizedPicture, opt => opt.MapFrom(x => Constants.IMAGES_PREFIX_FROM_ROOT + x.PathOriginalSize));
        }
    }
}
