using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuanLySanBanh.Startup))]
namespace QuanLySanBanh
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
