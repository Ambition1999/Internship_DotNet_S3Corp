using BLL.BusinessLogic;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Http.Results;

using System.Security.Claims;
using System.Web;

namespace APILayer.Controllers
{
    [RoutePrefix("api/UserAccount")]
    public class UserAccountController : ApiController
    {
        Account_BLL account_BLL = new Account_BLL();
        public UserAccountController() { }

        [HttpGet]
        [Route("CheckUserAccount/{username}/{password}")]
        public JsonResult<BoolResult> CheckUserAccount(string username, string password)
        {
            
            return Json<BoolResult>(account_BLL.AccountIsExits(username, password));
        }

        [HttpGet]
        [Route("UserNameIsExist/{username}")]
        public bool UserNameIsExist(string username)
        {
            return (account_BLL.UserNameIsExitst(username));
        }

        [HttpGet]
        [Route("CheckAdminAccount/{username}/{password}")]
        public JsonResult<BoolResult> CheckAdminAccount(string username, string password)
        {

            return Json<BoolResult>(account_BLL.AccountAdminIsExits(username, password));
        }

        [HttpPost]
        [Route("InsertRegisterAccount")]
        public int InsertRegisterAccount(DtoRegisterAccount dtoRegisterAccount)
        {
            return account_BLL.InsertUserAccount(dtoRegisterAccount);
        }

        [HttpPut]
        [Route("UpdateAccount")]
        public bool UpdateAccount(DtoUpdateAccount dtoUpdateAccount)
        {
            return account_BLL.UpdateAccount(dtoUpdateAccount);
        }


        

        //Test token
        [HttpPost]
        [Route("GetName1/{token}/")]
        public String GetName1(string token)
        {
            Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                }
                return "Valid";
            }
            else
            {
                return "Invalid";
            }
        }

        [Authorize]
        [HttpPost]
        [Route("GetName2")]
        public Object GetName2()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var name = claims.Where(p => p.Type == "name").FirstOrDefault()?.Value;
                return new
                {
                    data = name
                };

            }
            return null;
        }
    }
}
