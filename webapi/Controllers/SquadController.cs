using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace webapi.Controllers
{
    /// <summary>
    /// Контроллер, получающий данные об отрядах
    /// </summary>
    public class SquadController : ApiController
    {
        /// <summary>
        /// Возвращает данные об отрядах
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<wcfservice.SquadEntity> GetSquads()
        public async Task<IEnumerable<wcfservice.UnitsData>> GetSquads(wcfservice.BaseAction msg)
        {
            System.Diagnostics.Debug.WriteLine("какого чёрта надо этой функции, чтобы работать?");
            Service1Client client = new Service1Client();
            var result = await client.GetBaseUnitsAsync(msg);
            client.Close();
            return result;
        }

        /// <summary>
        /// Выполняет действия игрока с отрядом
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<string> Action(wcfservice.SquadAction msg)
        {
            Service1Client client = new Service1Client();
            string result = await client.SquadActionAsync(msg);
            client.Close();
            return result;
        }
    }
}