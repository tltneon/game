using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(webapi.Startup))]

namespace webapi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //BuildUpDamnDB();
            Service1Client client = new Service1Client();
            System.Diagnostics.Debug.WriteLine("Хитрожопский трюк с поднятием БД: " + client.DbStatus());
            client.Close();
        }
    }
}
