using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace server.Controllers
{
    public class ServerController : ApiController
    {
        // GET api/Sever
        public IEnumerable<string> Get()
        {
            return new string[] { "Sample Get request" };
        }
    }
}
