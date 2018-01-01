using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PokerASC.Startup))]
namespace PokerASC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
