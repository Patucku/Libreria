using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(slnLibreria.Startup))]
namespace slnLibreria
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
