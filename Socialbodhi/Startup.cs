using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Socialbodhi.Startup))]
namespace Socialbodhi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
