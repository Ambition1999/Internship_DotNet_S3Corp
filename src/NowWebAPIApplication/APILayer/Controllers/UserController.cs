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
        [Route("GetAllUserInfo/")]
        public JsonResult<List<DtoUserInfo>> GetAllUserInfo()
        {
            return Json<List<DtoUserInfo>>(user_BLL.GetAllUserInfo());
        }

        [HttpGet]
        [Route("GetUserInfoByUserName/{username}")]
        public JsonResult<DtoUserInfo> GetUserInfoByUserName(string username)
        {
                return Json<DtoUserInfo>(user_BLL.GetUserInfoByUserName(username));
        }

        [HttpGet]
        [Route("GetUserInfoById/{userId:int}")]
        public JsonResult<DtoUserInfo> GetUserInfoById(int userId)
        {
            return Json<DtoUserInfo>(user_BLL.GetUserInfoById(userId));
        }

        [HttpPut]
        [Route("UpdateUser")]
        public bool UpdatUser(DtoUserInfo dtoUserInfo)
        {
            return (user_BLL.UpdateUser(dtoUserInfo));
        }
    }
}
