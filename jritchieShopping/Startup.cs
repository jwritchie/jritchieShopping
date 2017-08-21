using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jritchieShopping.Startup))]
namespace jritchieShopping
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
