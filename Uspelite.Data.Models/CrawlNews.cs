namespace Uspelite.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using BaseModels;
    using Enum;

    public class CrawlNews : BaseModel
    {
        [Key]
        public CrawlType Id { get; set; }

        public int Count { get; set; }
    }
}
