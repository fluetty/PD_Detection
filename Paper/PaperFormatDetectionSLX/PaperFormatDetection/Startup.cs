using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PaperFormatDetection.Startup))]
namespace PaperFormatDetection
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
