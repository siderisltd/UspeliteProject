namespace Uspelite.Services.Common
{
    using System.Web.Hosting;

    public class Constants
    {
        public static readonly string APP_ROOT_PATH = HostingEnvironment.ApplicationPhysicalPath;

        public static readonly string ROOT_IMAGES_FOLDER = APP_ROOT_PATH + "Content\\Uploads\\Images\\";

        public const int MAX_FILES_IN_DIRECTORY = 20;
    }
}
