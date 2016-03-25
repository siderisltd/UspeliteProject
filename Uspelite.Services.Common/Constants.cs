namespace Uspelite.Services.Common
{
    using System.Web.Hosting;

    public class Constants
    {
        public static readonly string APP_ROOT_PATH = HostingEnvironment.ApplicationPhysicalPath; //@"C:\Users\Alex\Desktop\USPELITE_PROJECT\UspeliteProject-master\Uspelite.Web\";//

        public static readonly string ROOT_IMAGES_FOLDER = APP_ROOT_PATH + "Content\\Uploads\\Images\\";

        public const string IMAGES_PREFIX_FROM_ROOT = "Content\\Uploads\\Images\\";

        public const int FOLDERS_SEPARATION_COUNT = 2;
    }
}
