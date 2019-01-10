using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NotifyHealth.Startup))]
//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace NotifyHealth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
