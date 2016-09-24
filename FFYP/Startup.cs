using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FFYP.Startup))]
namespace FFYP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
