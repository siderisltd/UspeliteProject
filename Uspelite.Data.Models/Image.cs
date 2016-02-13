namespace Uspelite.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Table("Images")]
    public class Image : FileInfo
    {
        public int? ProjectId { get; set; }
    }
}
