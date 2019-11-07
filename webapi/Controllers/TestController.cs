using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace webapi.Controllers
{
    public class AuthController : ApiController
    {
        public HttpResponseMessage Post(WcfService.AuthData message)
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
        public WcfService.AuthData Get()
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
            IEnumerable<WcfService.StatEntity> level = client.GetUserList();
            client.Close();
            return level;
        }
    }
    public class UpgradeController : ApiController
    {
        public int Get()
        {
            int baseid = 3;
            Service1Client client = new Service1Client();
            int level = client.UpgradeBase(baseid);
            client.Close();
            return level;
        }
    }
}