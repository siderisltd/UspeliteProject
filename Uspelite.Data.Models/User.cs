namespace Uspelite.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BaseModels.Contracts;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser, ICommentableRateableBaseModel, IBaseModel
    {
        private ICollection<Article> articles;

        private ICollection<Video> videos;

        private ICollection<Rate> ratings;

        private ICollection<Image> images;

        private ICollection<Image> profileImages;

        private ICollection<Comment> comments;

        public User()
        {
            this.ratings = new HashSet<Rate>();
            this.videos = new HashSet<Video>();
            this.articles = new HashSet<Article>();
            this.images = new HashSet<Image>();
            this.comments = new HashSet<Comment>();
            this.CreatedOn = DateTime.Now;
        }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        public virtual ICollection<Article> Articles
        {
            get { return this.articles; }
            set { this.articles = value; }
        }

        public virtual ICollection<Video> Videos
        {
            get { return this.videos; }
            set { this.videos = value; }
        }

        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        public virtual ICollection<Image> ProfileImages
        {
            get { return this.profileImages; }
            set { this.profileImages = value; }
        }


        public virtual ICollection<Rate> Ratings
        {
            get { return this.ratings; }
            set { this.ratings = value; }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }


        [NotMapped]
        public virtual float CalculatedRating
        {
            get
            {
                float sum = 0.0f;
                int count = 0;
                foreach (Article post in this.Articles)
                {
                    sum += post.CalculatedRating;
                    count++;
                }

                foreach (Video video in this.Videos)
                {
                    sum += video.CalculatedRating;
                    count++;
                }
                if (this.Videos.Count == 0 && this.Articles.Count == 0)
                {
                    return 0;
                }
                return sum / count;
            }
        }

        [NotMapped]
        public string FullName { get { return this.FirstName + " " + this.LastName; } }

        public string ShortInfo { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBanned { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ClaimsIdentity GenerateUserIdentity(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            return Task.FromResult(this.GenerateUserIdentity(manager));
        }
    }
}
