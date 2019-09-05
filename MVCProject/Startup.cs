using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCProject.Startup))]
namespace MVCProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
