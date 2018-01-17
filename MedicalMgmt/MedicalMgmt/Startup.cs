using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MedicalMgmt.Startup))]
namespace MedicalMgmt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
