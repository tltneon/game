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
            Routine();
        }

        /// <summary>
        /// Функция Routine - аналог правильного цикла Routine, исполняемого во многих играх - основной цикл, где выполняются основные действия движка/мода
        /// </summary>
        private async void Routine()
        {
            while (true)
            {
                Thread.Sleep(6000);
                await Task.Run(() => {
                    var client = new Service1Client();
                    client.DbStatus();
                    client.Close();
                });
            }
            // хороший костыль лучше нехорошего кода
            // только тут и хорошего костыля нет
        }
    }
}
