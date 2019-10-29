using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace webapi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*", SupportsCredentials = true)]
    public class TestController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> TestConnect()
        {
            System.Diagnostics.Debug.WriteLine("i ve got some shit");
            return null;
        }
        public string Get()
        {
            System.Diagnostics.Debug.WriteLine("i send some shit");
            return new JavaScriptSerializer().Serialize( new { username = "Odmen", password = "2891ueij1230"});
        }
        public HttpResponseMessage Post()
        {
            System.Diagnostics.Debug.WriteLine("i ve got some shit");
            return new HttpResponseMessage()
            {
                Content = new StringContent("POST: Test message")
            };
        }
        /*[HttpPost]
        [AllowAnonymous]
        public string Post()
        {
            string data = Request.ToString();
            System.Diagnostics.Debug.WriteLine("i ve got some shit");
            return data;
        }*/
        /*public Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext context,
                                                    CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                IHttpRouteData rd = context.RouteData;
                string output = "Неверный запрос";
                if (rd.Values.ContainsKey("id"))
                {
                    int sum;
                    if (int.TryParse((string)rd.Values["id"], out sum) && sum > 0)
                    {
                        double result = 1.3 * sum;
                        output = string.Format("За {0} евро вы получите {1} долларов", sum, result);
                    }
                }
                return context.Request.CreateResponse(HttpStatusCode.OK, output);
            });
        }*/
    }
}
