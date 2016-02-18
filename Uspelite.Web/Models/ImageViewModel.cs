namespace Uspelite.Web.Models
{
    using System;
    using System.Linq.Expressions;
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;
    using Services.Common;

    public class ImageViewModel : IMapFrom<Image>, IMapTo<Image>, IHaveCustomMappings
    {
        public static Expression<Func<Image, ImageViewModel>> FromModel
        {
            get
            { 
                return x => new ImageViewModel
                    {
                        IsMain = x.IsMain, 
                        Title = x.Title,
                        Url = x.PathResizedImage
                };
            }
        }

        public string Title { get; set; }

        public string Url { get; set; }

        public bool IsMain { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Image, ImageViewModel>()
                         .ForMember(x => x.Url, opt => opt.MapFrom(x => Constants.IMAGES_PREFIX_FROM_ROOT + x.PathResizedImage));
        }
    }
}