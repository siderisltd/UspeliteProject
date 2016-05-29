namespace Uspelite.Web.Models.Common
{
    using Data.Models.Enum;
    using Uspelite.Data.Models;
    using Uspelite.Web.Infrastructure.Mapping.Contracts;

    public class SocialProfileViewModel : IMapFrom<SocialProfile>
    {
        public string Url { get; set; }

        public SocialProfileType SocialProfileType { get; set; }

        public string UserId { get; set; }
    }
}