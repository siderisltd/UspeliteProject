namespace Uspelite.Web.Models.Common
{
    using System.Linq;
    using Articles;
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;

    public class UserViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string ShortInfo { get; set; }

        public ArticleImageViewModel ProfileImage { get; set; }

        public void CreateMappings(IMapperConfiguration configuration)
        {
            configuration.CreateMap<User, UserViewModel>()
                         .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.FirstName + " " + x.LastName))
                         .ForMember(x => x.ProfileImage, opt => opt.MapFrom(x => x.ProfileImages.FirstOrDefault(y => y.IsMainProfilePicture)));
        }
    }
}