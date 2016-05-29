namespace Uspelite.Services.Data
{
    using Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;
    using Uspelite.Web.Infrastructure;
    public class SocialProfilesService : ISocialProfilesService
    {
        private readonly IRepository<SocialProfile> repo;

        public SocialProfilesService(IRepository<SocialProfile> repo)
        {
            this.repo = repo;
        }

        public void RemoveAllRelatedToUser(string userId)
        {
            this.repo.All().Where(x => x.UserId == userId).ForEach(x => this.repo.HardDelete(x));
            this.repo.SaveChanges();
        }

        public void Add(ICollection<SocialProfile> profiles)
        {
            profiles.ForEach(x => this.repo.Add(x));
            this.repo.SaveChanges();
        }
    }
}
