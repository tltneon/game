using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

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
            System.Diagnostics.Debug.WriteLine(message.username);
            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }

    public class ChatMessage
    {
        public string username { get; set; }
    }
}
