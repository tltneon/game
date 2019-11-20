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
        /// Возвращает данные о юнитах на базе
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<wcfservice.UnitsData>> GetBaseUnitsList(wcfservice.BaseAction msg)
        {
            var client = new Service1Client();
            var entities = await client.GetBaseUnitsListAsync(msg);
            client.Close();
            return entities;
        }

        /// <summary>
        /// Выполняет действия игрока с отрядом
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<string> Action(wcfservice.SquadAction msg)
        {
            var client = new Service1Client();
            var result = await client.SquadActionAsync(msg);
            client.Close();
            return result;
        }
    }
}