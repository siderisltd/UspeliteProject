namespace Uspelite.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class News
    {
        private ICollection<Picture> pictures;

        public News()
        {
            this.pictures = new HashSet<Picture>();
        }
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        public virtual ICollection<Picture> Pictures
        {
            get { return this.pictures; }
            set { this.pictures = value; }
        }
    }
}
