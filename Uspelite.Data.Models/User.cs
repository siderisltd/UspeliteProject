namespace Uspelite.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        public ICollection<Post> posts;

        public ICollection<Video> videos;

        public ICollection<Rate> rates;

        public ICollection<Picture> pictures;

        public ICollection<Comment> comments;

        public User()
        {
            this.rates = new HashSet<Rate>();
            this.videos = new HashSet<Video>();
            this.posts = new HashSet<Post>();
            this.pictures = new HashSet<Picture>();
            this.comments = new HashSet<Comment>();
        }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Ip { get; set; }

        public virtual ICollection<Post> Posts
        {
            get { return this.posts; }
            set { this.posts = value; }
        }

        public virtual ICollection<Video> Videos
        {
            get { return this.videos; }
            set { this.videos = value; }
        }

        public virtual ICollection<Picture> Pictures
        {
            get { return this.pictures; }
            set { this.pictures = value; }
        }

        public virtual ICollection<Rate> Rates
        {
            get { return this.rates; }
            set { this.rates = value; }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        [NotMapped]
        public float Rating
        {
            get
            {
                float sum = 0.0f;
                int count = 0;
                foreach (Post post in this.Posts)
                {
                    sum += post.CalculatedRating;
                    count++;
                }

                foreach (Video video in this.Videos)
                {
                    sum += video.CalculatedRating;
                    count++;
                }

                return sum / count;
            }
        }

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
