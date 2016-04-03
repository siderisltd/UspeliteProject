namespace Uspelite.Web.Areas.Administration.Models.Users
{
    using System;
    using System.ComponentModel;
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;

    public class UserViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Username { get; set; }

        [DisplayName("Full name")]
        public string FullName { get; set; }

        public string Email { get; set; }

        public int Posts { get; set; }

        public int Videos { get; set; }

        public string Password { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<User, UserViewModel>()
                         .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.FirstName + " " + x.LastName))
                         .ForMember(x => x.Posts, opt => opt.MapFrom(x => x.Articles.Count))
                         .ForMember(x => x.Videos, opt => opt.MapFrom(x => x.Videos.Count));
        }
    }
}