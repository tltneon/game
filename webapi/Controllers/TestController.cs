using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace webapi.Controllers
{
    public class AuthController : ApiController
    {
        public HttpResponseMessage Post(WcfService.AuthData message)
        {
            System.Diagnostics.Debug.WriteLine("как же я заманался с этим постом");
            if(AuthDataUtils.Check(message) || !ModelState.IsValid) return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    "Invalid input => urfag");
            System.Diagnostics.Debug.WriteLine("GOT THIS: ", message.username, message.password);
            System.Diagnostics.Debug.WriteLine(message.password, " >== BASE64 ==> ", AuthDataUtils.Base64Encode(message.password));
            System.Diagnostics.Debug.WriteLine("connecting to wcf...");
            Service1Client client = new Service1Client();
            string token = client.SendAuthData(message);
            System.Diagnostics.Debug.WriteLine(token);
            client.Close();
            return Request.CreateResponse(HttpStatusCode.OK, token);
        }
        /*public string Get()
        {
            System.Diagnostics.Debug.WriteLine("connecting to wcf...");
            Service1Client client = new Service1Client();

            System.Diagnostics.Debug.WriteLine(client.GetData(6));

            client.Close();

            System.Diagnostics.Debug.WriteLine("i send some shit");

            return new JavaScriptSerializer().Serialize(new { username = "Odmen", password = "2891ueij1230" });
        }*/
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
        public int Get()
        {
            Service1Client client = new Service1Client();
            int level = client.UpgradeBase(1);
            client.Close();
            return level;
        }
    }
    public class UpgradeController : ApiController
    {
        public int Get()
        {
            int baseid = 3;
            System.Diagnostics.Debug.WriteLine("GET REQUEST: ", baseid);

            System.Diagnostics.Debug.WriteLine("connecting to wcf...");
            Service1Client client = new Service1Client();

            int level = client.UpgradeBase(baseid);

            System.Diagnostics.Debug.WriteLine(level);

            client.Close();

            System.Diagnostics.Debug.WriteLine("i send some shit"+ level);

            return level;
        }
    }
}