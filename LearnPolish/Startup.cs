using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LearnPolish.Startup))]
namespace LearnPolish
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
