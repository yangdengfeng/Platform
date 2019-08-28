using Microsoft.Owin;
using Owin;


[assembly: OwinStartupAttribute(typeof(PkpmGX.Startup))]
namespace PkpmGX
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           ConfigureAuth(app);
        }
    }
}
