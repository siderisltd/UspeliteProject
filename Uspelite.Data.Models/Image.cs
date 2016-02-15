namespace Uspelite.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.IO;
    using System.Text;
    using Common;
    using Enum;

    public class Image
    {
        private ICollection<Rate> rates;

        private string title;

        public Image()
        {
            this.rates = new HashSet<Rate>();
            this.CreatedOn = DateTime.Now;
        }

        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(300)]
        [Required]
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                var words = value.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var cycleCount = words.Length < Constants.MAX_WORDS_IN_IMAGE_NAME ? words.Length : Constants.MAX_WORDS_IN_IMAGE_NAME;
                StringBuilder sb = new StringBuilder();
                var randomGen = new RandomGenerator();
                var randomStr = randomGen.RandomString(1, 5);
                sb.Append(randomStr);
                sb.Append('_');
                for (int i = 0; i < cycleCount; i++)
                {
                    sb.Append(words[i]);
                    if(i != cycleCount - 1)
                    {
                        sb.Append('-');
                    }
                }
                this.title = sb.ToString();
            }
        }

        public ImageType ImageType { get; set; }

        public string AltTag { get; set; }

        public string PathOriginalSize { get; set; }

        public string PathResizedImage { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsMainPicture { get; set; }

        [NotMapped]
        public Stream Stream { get; set; }

        public virtual ICollection<Rate> Rates
        {
            get { return this.rates; }
            set { this.rates = value; }
        }
    }
}
