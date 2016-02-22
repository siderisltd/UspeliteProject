﻿namespace Uspelite.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Common;
    using Common.Contracts;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.BaseModels.Contracts;

    public class UspeliteDbContext : IdentityDbContext<User>, IUspeliteDbContext
    {
        public UspeliteDbContext()
            : base("UspeliteDbConnection", throwIfV1Schema: false)
        {
        }

        public IDbSet<CrawlNews> CrawlNews { get; set; }

        public IDbSet<Video> Videos { get; set; }

        public IDbSet<Rate> Rates { get; set; }

        public IDbSet<Image> Images { get; set; }

        public IDbSet<Comment> Comments { get; set; }

        public IDbSet<Category> Categories { get; set; }

        public IDbSet<Article> Articles { get; set; }

        public static UspeliteDbContext Create()
        {
            return new UspeliteDbContext();
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
                return base.SaveChanges();
        }
        
        
        private void ApplyAuditInfoRules()
        {
            foreach (var entry in
                this.ChangeTracker.Entries().Where(e => e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added && entity.CreatedOn == default(DateTime))
                {
                    entity.CreatedOn = DateTime.Now;
                    var entityAsSeoEntity = entity as ISeoEntity;
                    if (entityAsSeoEntity != null)
                    {
                        entityAsSeoEntity.Slug = SlugHelper.CreateSlug(entityAsSeoEntity.Title, Constants.SLUG_MAX_LENGTH);

                    }
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }

          
        }
    }
}
