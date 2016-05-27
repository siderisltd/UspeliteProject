using Uspelite.Data.Models.BaseModels;
using Uspelite.Data.Models.Enum;

namespace Uspelite.Data.Models
{
    public class SocialProfile : BaseModel
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public SocialProfileType SocialProfileType { get; set; }
    }
}
