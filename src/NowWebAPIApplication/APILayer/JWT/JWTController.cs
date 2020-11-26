using BLL.BusinessLogic;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;

namespace APILayer.JWT
{
    [RoutePrefix("api/Jwt")]
    public class JWTController : ApiController
    {

        Account_BLL account_BLL = new Account_BLL();
        
        [HttpGet]
        [Route("Validate")]
        public string Validate(string token,string username)
        {
            if (account_BLL.UserNameIsExitst(username)) return "Invalid User";
            string tokenUsername = TokenManager.ValidateToken(token);
            if (username.Equals(tokenUsername))
            {
                return "Valid token";
            }
            else
                return "Invalid token";
        }

        [HttpGet]
        [Route("GetToken/{username}")]
        public string GetToken(string username)
        {
            return TokenManager.GenerateToken(username);
        }



    }
}
