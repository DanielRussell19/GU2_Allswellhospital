using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GU2_Allswellhospital.Startup))]
namespace GU2_Allswellhospital
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
