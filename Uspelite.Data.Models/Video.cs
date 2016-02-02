namespace Uspelite.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Video
    {
        private ICollection<Comment> comments;

        private ICollection<Category> categories;

        private ICollection<Rate> rates;

        public Video()
        {
            this.categories = new HashSet<Category>();
            this.comments = new HashSet<Comment>();
            this.CreatedOn = DateTime.Now;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        [Index(IsUnique = true)]
        [StringLength(300)]
        [Required]
        public string Route { get; set; }

        public string VideoUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<Category> Categories
        {
            get { return this.categories; }
            set { this.categories = value; }
        }

        public virtual ICollection<Rate> Rates
        {
            get { return this.rates; }
            set { this.rates = value; }
        }
    }
}
