using Microsoft.Owin;
using Owin;
using TextumReader.WebUI.App_Start;

[assembly: OwinStartup(typeof(Startup))]
namespace TextumReader.WebUI.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
