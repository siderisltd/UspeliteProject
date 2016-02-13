namespace Uspelite.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class FileInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string OriginalFileName { get; set; }

        [Required]
        [MaxLength(200)]
        public string FileExtension { get; set; }

        public string UrlPath { get; set; }
    }
}
