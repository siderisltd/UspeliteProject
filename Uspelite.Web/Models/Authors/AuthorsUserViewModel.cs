namespace Uspelite.Web.Models.Authors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Articles;
    using AutoMapper;
    using Common;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;
    using Videos;

    public class AuthorsUserViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string ShortInfo { get; set; }

        public ArticleImageViewModel ProfileImage { get; set; }

        public ICollection<AuthorArticleViewModel> Articles { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<User, AuthorsUserViewModel>()
                         .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.FirstName + " " + x.LastName))
                         .ForMember(x => x.ProfileImage, opt => opt.MapFrom(x => x.ProfileImages.FirstOrDefault(y => y.IsMainProfilePicture)))
                         .ForMember(x => x.Articles, opt => opt.MapFrom(x => x.Articles.OrderBy(y => y.CreatedOn).Skip(0).Take(10)));
        }

    }
}