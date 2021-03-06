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
        
    }
}
