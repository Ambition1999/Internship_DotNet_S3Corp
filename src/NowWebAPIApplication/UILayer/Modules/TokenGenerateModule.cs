using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using UILayer.Service;

namespace UILayer.Modules
{
    public static class TokenGenerateModule
    {
        public static string GetToken(string username)
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseCheckAccount = service.GetResponse("/api/JWT/GetToken/" + username);
            responseCheckAccount.EnsureSuccessStatusCode();

            return responseCheckAccount.Content.ReadAsAsync<string>().Result;
        }
    }
}