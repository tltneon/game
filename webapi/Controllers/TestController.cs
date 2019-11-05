using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace webapi.Controllers
{
//[EnableCors("http://localhost:4200", "*", "*")]
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
            System.Diagnostics.Debug.WriteLine(message.username, message.password);
            return Request.CreateResponse(HttpStatusCode.OK, "authed");
        }
        public string Get()
        {
            System.Diagnostics.Debug.WriteLine("GET REQUEST: i send some shit");

            System.Diagnostics.Debug.WriteLine("connecting to wcf...");
            Service1Client client = new Service1Client();

            System.Diagnostics.Debug.WriteLine(client.GetData(6));

            client.Close();

            return new JavaScriptSerializer().Serialize(new { username = "Odmen", password = "2891ueij1230" });
        }
    }
    public class ChatMessage
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}