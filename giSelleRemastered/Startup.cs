using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(giSelleRemastered.Startup))]
namespace giSelleRemastered
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
