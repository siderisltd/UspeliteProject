namespace Uspelite.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Category
    {
        private ICollection<Post> posts;

        private ICollection<Video> videos;

        public Category()
        {
            this.CreatedOn = DateTime.Now;
            this.videos = new HashSet<Video>();
            this.posts = new HashSet<Post>();
        }

        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(300)]
        [Required]
        public string Title { get; set; }
         
        public ICollection<Post> Posts
        {
            get { return this.posts; }
            set { this.posts = value; }
        }

        public ICollection<Video> Videos
        {
            get { return this.videos; }
            set { this.videos = value; }
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
