namespace Uspelite.Web.Areas.Management.Models
{
    using System;
    using AutoMapper;
    using Data.Models;
    using Data.Models.Enum;
    using Infrastructure.Mapping.Contracts;
    using Services.Data.DTO;
    using Web.Models.Categories;

    public class ManageArticleViewModel : IMapFrom<Article>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public PostStatus Status { get; set; }

        public CategoryViewModel Category { get; set; }

        public DateTime CreatedOn { get; set; }

        public ArticlePlaceType Place { get; set; }

        public ArticleStatusDTO ArticleStatus { get; set; }

        public ArticlePlaceTypeDTO ArticlePlaceType { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<Article, ManageArticleViewModel>()
                         .ForMember(x => x.Author, opt => opt.MapFrom(au => au.Author.UserName));
        }
    }
}