using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    public static class RestComunator
    {
        //private static string SERVICE_URL = "http://desktop-2b7ngkl:9063/";
        private static string SERVICE_URL; // = "https://desktop-2b7ngkl:443/";

        static RestComunator()
        {
            SERVICE_URL = ConfigurationManager.AppSettings["ServerAddress"];
        }
         
        public static Server GetWebClient()
        {
            //_log.Info(uri);
            var client = new Server(new Uri(SERVICE_URL), new BasicAuthenticationCredentials());
            return client;
        }
    }
}
