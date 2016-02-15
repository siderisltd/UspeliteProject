namespace Uspelite.Web.Models.Images
{
    using System.Collections.Generic;
    using System.Web;
    using System.IO;

    public class AddImageModel
    {
        public IEnumerable<HttpPostedFileBase> imageFiles { get; set; }

        public int ContentLength { get; set; }

        public string CategoryName { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public Stream InputStream { get; set; }
    }
}