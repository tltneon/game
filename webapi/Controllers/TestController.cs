using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace webapi.Controllers
{
    public class AccountController : ApiController
    {
        public HttpResponseMessage Auth(WcfService.AuthData message)
        {
            System.Diagnostics.Debug.WriteLine("слава яйцам - всё работает отлично");
            if(AuthDataUtils.Check(message) || !ModelState.IsValid) return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    "Invalid input => urfag");
            Service1Client client = new Service1Client();
            string token = client.SendAuthData(message);
            client.Close();
            return Request.CreateResponse(HttpStatusCode.OK, token);
        }
        public WcfService.AuthData GetDummyUserData()
        {
            Service1Client client = new Service1Client();
            WcfService.AuthData data = client.GetDummyUserData();
            client.Close();
            return data;
        } 
    }
    class AuthDataUtils
    {
        public static bool Check(WcfService.AuthData message) { // прочекаем входные данные на вшивость
            if (message == null) return true;
            if (message.username == null || message.password == null) return true;
            if (message.username.Length < 3 || message.password.Length < 3) return true;
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
        public IEnumerable<WcfService.StatEntity> GetUserList()
        {
            Service1Client client = new Service1Client();
            IEnumerable<WcfService.StatEntity> entities = client.GetUserList();
            client.Close();
            return entities;
        }
    }
    public class BaseController : ApiController
    {
        public WcfService.BaseEntity RetrieveBaseData(WcfService.BaseAction msg)
        {
            Service1Client client = new Service1Client();
            WcfService.BaseEntity result = client.GetBaseInfo(msg);
            client.Close();
            return result;
        }
        public IEnumerable<WcfService.StructureEntity> RetrieveBaseStructures(WcfService.BaseAction msg)
        {
            Service1Client client = new Service1Client();
            IEnumerable<WcfService.StructureEntity> result = client.GetBaseStructures(msg);
            client.Close();
            return result;
        }
        public string Action(WcfService.BaseAction msg)
        {
            Service1Client client = new Service1Client();
            string result = client.BaseAction(msg);
            client.Close();
            return result;
        }
    }
    public class StructureController : ApiController
    {
    }
    public class SquadController : ApiController
    {
    }
}