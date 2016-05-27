namespace Uspelite.Web.Models.Authors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Articles;
    using AutoMapper;
    using Common;
    using Data.Models;
    using Images;
    using Infrastructure.Mapping.Contracts;
    using Videos;
    using Base;

    public class AuthorsUserViewModel : PaginationModel, IMapFrom<User>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string ShortInfo { get; set; }

        public ImageViewModel ProfileImage { get; set; }

        public ICollection<AuthorArticleViewModel> UserArticles { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<User, AuthorsUserViewModel>()
                         .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.FirstName + " " + x.LastName))
                         .ForMember(x => x.ProfileImage, opt => opt.MapFrom(x => x.ProfileImages.FirstOrDefault()));
        }

    }
}