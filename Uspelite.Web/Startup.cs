using Microsoft.Owin;
using Microsoft.AspNet.SignalR;
using Owin;

[assembly: OwinStartupAttribute(typeof(Uspelite.Web.Startup))]
namespace Uspelite.Web
{


    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
            app.MapSignalR("/signalr", new HubConfiguration());
            app.MapSignalR();
        }
    }
}
