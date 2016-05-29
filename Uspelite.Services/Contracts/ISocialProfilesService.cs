namespace Uspelite.Services.Data.Contracts
{
    using System.Collections.Generic;
    using Uspelite.Data.Models;

    public interface ISocialProfilesService
    {
        void RemoveAllRelatedToUser(string userId);

        void Add(ICollection<SocialProfile> profiles);
    }
}
