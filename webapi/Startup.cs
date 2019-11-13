using Microsoft.Owin;
using Owin;
using System.Threading;
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
            /*try
            {
                Service1Client client = new Service1Client();
                System.Diagnostics.Debug.WriteLine("Проверка коннекта к БД: " + client.DbStatus());
                client.Close();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("WCF не хочет подниматься не смотря на параметр minFreeMemoryPercentageToActivateService=\"0\" и кидает System.ServiceModel.ServiceActivationException");
            }*/

            Routine();
        }
        public async void Routine()
        {
            while (true)
            {
                Thread.Sleep(6000);
                await Task.Factory.StartNew(() => {
                    Service1Client client = new Service1Client();
                    System.Diagnostics.Debug.WriteLine("пинаем wcf");
                    client.DbStatus();
                    client.Close();
                });
            }
            // хороший костыль лучше нехорошего кода
            // только тут и хорошего костыля нет
        }
    }
}
