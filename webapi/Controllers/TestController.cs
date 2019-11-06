using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace webapi.Controllers
{
    public class MessageController : ApiController
    {
        public HttpResponseMessage Post([FromBody]ChatMessage message)
        {
            System.Diagnostics.Debug.WriteLine("как же я заманался с этим постом");
            if (message == null || !ModelState.IsValid)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    "Invalid input");
            }
            System.Diagnostics.Debug.WriteLine("GOT THIS: ", message.username, message.password);
            System.Diagnostics.Debug.WriteLine(message.password, " >== BASE64 ==> ", BASE64.Base64Encode(message.password));
            System.Diagnostics.Debug.WriteLine("connecting to wcf...");
            Service1Client client = new Service1Client();
            System.Diagnostics.Debug.WriteLine(client.SendData(message.username, BASE64.Base64Encode(message.password)));
            client.Close();
            return Request.CreateResponse(HttpStatusCode.OK, "authed");
        }
        [HttpGet]
        public string UpgradeBase(int baseid)
        {
            System.Diagnostics.Debug.WriteLine("GET REQUEST: ", baseid);

            System.Diagnostics.Debug.WriteLine("connecting to wcf...");
            Service1Client client = new Service1Client();

            System.Diagnostics.Debug.WriteLine(client.GetData(baseid));

            client.Close();

            return "Success";
        }
        public string Get(string action)
        {
            System.Diagnostics.Debug.WriteLine("GET REQUEST: ", action);

            System.Diagnostics.Debug.WriteLine("connecting to wcf...");
            Service1Client client = new Service1Client();

            System.Diagnostics.Debug.WriteLine(client.GetData(6));

            client.Close();

            System.Diagnostics.Debug.WriteLine("i send some shit");

            return new JavaScriptSerializer().Serialize(new { username = "Odmen", password = "2891ueij1230" });
        }
    }
    public class ChatMessage
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    class BASE64 {
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
}