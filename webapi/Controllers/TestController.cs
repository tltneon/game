using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace webapi.Controllers
{
    public class AccountController : ApiController
    {
        public async Task<HttpResponseMessage> Auth(WcfService.AuthData message)
        {
            if (AuthDataUtils.Check(message) || !ModelState.IsValid) return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    "Invalid input");
            Service1Client client = new Service1Client();
            string token = await client.SendAuthDataAsync(message);
            client.Close();
            return Request.CreateResponse(HttpStatusCode.OK, token);
        }
        public WcfService.AuthData GetAccountData()
        {
            return new WcfService.AuthData { username = "testuser", password = "testpass" };
        } 
    }
    class AuthDataUtils
    {
        public static bool Check(WcfService.AuthData message) { // прочекаем входные данные на вшивость
            if (message == null) return true;
            if (message.username == null || message.password == null) return true;
            if (message.username.Length < 3 || message.password.Length < 3) return true;
            //заявка на проверку регуляркой Regex.IsMatch(message.username, @"[^0-9a-zA-Z&+=\\\-&?*%:;#№@!)(]+") || Regex.IsMatch(message.password, @"[^0-9a-zA-Z&+=\\\-&?*%:;#№@!)(]+")
            return false;
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
    public class StatisticController : ApiController
    {
        public async Task<IEnumerable<WcfService.StatEntity>> GetPlayerList()
        {
            Service1Client client = new Service1Client();
            IEnumerable<WcfService.StatEntity> entities = await client.GetPlayerListAsync();
            client.Close();
            return entities;
        }
    }
    public class BaseController : ApiController
    {
        public async Task<IEnumerable<WcfService.BaseEntity>> GetBaseList()
        {
            Service1Client client = new Service1Client();
            IEnumerable<WcfService.BaseEntity> entities = await client.GetBaseListAsync();
            client.Close();
            return entities;
        }
        public async Task<WcfService.BaseEntity> RetrieveBaseData(WcfService.BaseAction msg)
        {
            Service1Client client = new Service1Client();
            WcfService.BaseEntity result = await client.GetBaseInfoAsync(msg);
            client.Close();
            return result;
        }
        public async Task<string> Action(WcfService.BaseAction msg)
        {
            Service1Client client = new Service1Client();
            string result = await client.BaseActionAsync(msg);
            client.Close();
            return result;
        }
    }
    public class SquadController : ApiController
    {
        public IEnumerable<WcfService.SquadEntity> GetSquads()
        {
            System.Diagnostics.Debug.WriteLine("какого чёрта надо этой функции, чтобы работать?");
            Service1Client client = new Service1Client();
            WcfService.SquadAction msg = null;
            IEnumerable<WcfService.SquadEntity> result = client.GetSquads(msg);
            client.Close();
            return result;
        }
        public async Task<string> Action(WcfService.SquadAction msg)
        {
            Service1Client client = new Service1Client();
            string result = await client.SquadActionAsync(msg);
            client.Close();
            return result;
        }
    }
}