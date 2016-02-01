using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Uspelite.Web.Startup))]
namespace Uspelite.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
