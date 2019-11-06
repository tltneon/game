using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Web.Http;

namespace webapi.Controllers
{
    [Authorize]
    public class HomeController : ApiController
    {
        public IHttpActionResult Index()
        {
            return View();
        }

        private IHttpActionResult View()
        {
            throw new NotImplementedException();
        }
    }
    public class TestController : ApiController
    {
        public string Get()
        {
            System.Diagnostics.Debug.WriteLine("i send some shit");
            return new JavaScriptSerializer().Serialize( new { username = "Odmen", password = "2891ueij1230"});
        }
        public async Task Post([FromBody] string str)
        {
            System.Diagnostics.Debug.WriteLine("i've got some shit for you");
            System.Diagnostics.Debug.WriteLine(str);
            object t = new JavaScriptSerializer().Deserialize<object>(str);
            System.Diagnostics.Debug.WriteLine(t.ToString());
            return;
        }

        [Route("Test/Post4")]
        [HttpPost]
        public string Post4(HttpRequestMessage context)
        {
            System.Diagnostics.Debug.WriteLine("i ve got some shit4");
            var contentResult = context.Content.ReadAsStringAsync();
            string result = contentResult.Result;
            return result;
        }
    }
}