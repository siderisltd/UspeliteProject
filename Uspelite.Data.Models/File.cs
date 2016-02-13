namespace Uspelite.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Files")]
    public class File : FileInfo
    {
        [NotMapped]
        public byte[] Content { get; set; }
    }
}
