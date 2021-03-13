using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CentralSport.Startup))]
namespace CentralSport
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
