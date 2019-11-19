using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace webapi.Controllers
{
    /// <summary>
    /// Контроллер, получающий данные о действиях с базой
    /// </summary>
    public class BaseController : ApiController
    {
        /// <summary>
        /// Возвращает данные о базах
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<wcfservice.BaseEntity>> GetBaseList()
        {
            var client = new Service1Client();
            var entities = await client.GetBaseListAsync();
            client.Close();
            return entities;
        }

        /// <summary>
        /// Возвращает данные о базе игрока
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<wcfservice.BaseEntity> RetrieveBaseData(wcfservice.BaseAction msg)
        {
            var client = new Service1Client();
            var result = await client.GetBaseInfoAsync(msg);
            client.Close();
            return result;
        }

        /// <summary>
        /// Выполняет действия игрока с базой
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<string> Action(wcfservice.BaseAction msg)
        {
            var client = new Service1Client();
            var result = await client.BaseActionAsync(msg);
            client.Close();
            return result;
        }
    }

}