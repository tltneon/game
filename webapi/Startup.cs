using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(webapi.Startup))]

namespace webapi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //BuildUpDamnDB();
            try
            {
                Service1Client client = new Service1Client();
                System.Diagnostics.Debug.WriteLine("Хитрожопский трюк с поднятием БД: " + client.DbStatus());
                client.Close();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("WCF не хочет подниматься не смотря на параметр minFreeMemoryPercentageToActivateService=\"0\" и кидает System.ServiceModel.ServiceActivationException");
            }
        }
        public async void Main() {
            await Task.Factory.StartNew(() => System.Diagnostics.Debug.WriteLine("123"));
        }
    }
}
