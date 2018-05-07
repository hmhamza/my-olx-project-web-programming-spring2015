using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Web_Finale3.Startup))]
namespace Web_Finale3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
