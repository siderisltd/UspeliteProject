namespace Uspelite.Web.Areas.Management.Models
{
    using System;
    using AutoMapper;
    using Data.Models;
    using Data.Models.Enum;
    using Infrastructure.Mapping.Contracts;

    public class ManageArticleViewModel : IMapFrom<Article>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public PostStatus Status { get; set; }

        public string Category { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Article, ManageArticleViewModel>()
                         .ForMember(x => x.Author, opt => opt.MapFrom(au => au.Author.UserName))
                         .ForMember(x => x.Category, opt => opt.MapFrom(ca => ca.Category.Title));
        }
    }
}