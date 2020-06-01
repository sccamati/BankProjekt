using Microsoft.AspNetCore.Hosting;
using Microsoft.Owin;
using Owin;


[assembly: OwinStartupAttribute(typeof(BankProjekt.Startup))]
namespace BankProjekt
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
