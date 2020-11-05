using BLL.BusinessLogic;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace APILayer.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        User_BLL user_BLL = new User_BLL();
        public UserController() { }

        [HttpGet]
        [Route("GetUserInfoByUserName/{username}")]
        public JsonResult<DtoUserInfo> GetUserInfoByUserName(string username)
        {
                return Json<DtoUserInfo>(user_BLL.GetUserInfoByUserName(username));
        }
    }
}
