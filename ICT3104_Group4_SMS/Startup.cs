using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ICT3104_Group4_SMS.Startup))]
namespace ICT3104_Group4_SMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
