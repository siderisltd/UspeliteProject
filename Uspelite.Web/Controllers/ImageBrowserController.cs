namespace Uspelite.Web.Controllers
{
    using System.IO;
    using Kendo.Mvc.UI;

    public class ImageBrowserController : EditorImageBrowserController
    {
        private const string contentFolderRoot = "~/Content/Uploads";
        private const string prettyName = "Images/";

        public override string ContentPath
        {
            get { return Path.Combine(contentFolderRoot, prettyName); }
        }
    }
}