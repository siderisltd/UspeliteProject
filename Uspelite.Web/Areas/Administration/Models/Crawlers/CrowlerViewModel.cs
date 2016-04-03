namespace Uspelite.Web.Areas.Administration.Models.Crawlers
{
    using System.Collections.Generic;
    using Services.Data.DTO;

    public class CrowlerViewModel
    {
        public IEnumerable<CrowlerDTO> Crawlers { get; set; }
    }
}