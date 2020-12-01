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
using APILayer.JWT;

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

        [HttpPut]
        [Route("UpdateAccount2/{token}/{username}/")]
        public int UpdateAccount2(string token,string username,DtoUpdateAccount dtoUpdateAccount)
        {
            if (!account_BLL.UserNameIsExitst(username)) return -2; //User name is not exist
            string tokenUsername = TokenManager.ValidateToken(token);
            if (username.Equals(tokenUsername))
            {
                if (account_BLL.UpdateAccount(dtoUpdateAccount))
                    return 1; //Update success
                return -1; //Failed to update
            }
            else
                return -3; //Invalid token
        }

        [HttpGet]
        [Route("UpdatePassword/{username}/{newpassword}/")]
        public bool UpdatePassword(string username, string newpassword)
        {
            return account_BLL.UpdatePassword(username, newpassword);
        }

    }
}
