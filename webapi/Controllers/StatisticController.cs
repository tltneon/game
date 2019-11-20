using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace webapi.Controllers
{
    /// <summary>
    /// Контроллер, получающий данные о статистике
    /// </summary>
    public class StatisticController : ApiController
    {
        /// <summary>
        /// Возвращает список игроков
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<wcfservice.PlayerData>> GetPlayerList()
        {
            var client = new Service1Client();
            var entities = await client.GetPlayerListAsync();
            client.Close();
            return entities;
        }
        
        /// <summary>
        /// Возвращает данные статистики
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<wcfservice.StatsData>> GetStats()
        {
            var client = new Service1Client();
            var entities = await client.GetStatsAsync();
            client.Close();
            return entities;
        }

        /// <summary>
        /// Возвращает данные о юнитах на базе
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<wcfservice.UnitsData>> GetBaseUnitsList(wcfservice.BaseAction q)
        {
            var client = new Service1Client();
            var entities = await client.GetBaseUnitsListAsync(q);
            client.Close();
            return entities;
        }

        /// <summary>
        /// Возвращает журнал событий
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<wcfservice.StatsData>> GetJournal()
        {
            var client = new Service1Client();
            var entities = await client.GetStatsAsync();
            client.Close();
            return entities;
        }
    }
}