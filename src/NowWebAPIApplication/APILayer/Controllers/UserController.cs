using APILayer.JWT;
using BLL.BusinessLogic;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using log4net;

namespace APILayer.Controllers
{


    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        User_BLL user_BLL = new User_BLL();
        Account_BLL account_BLL = new Account_BLL();
        public UserController() { }

        [HttpGet]
        [Route("GetAllUserInfo/")]
        public JsonResult<List<DtoUserInfo>> GetAllUserInfo()
        {
            //Log start
            Log.Info("[START] UserController - GetAllUserInfo");
            return Json<List<DtoUserInfo>>(user_BLL.GetAllUserInfo());
        }

        [HttpGet]
        [Route("CheckinEmail/{email}")]
        public string CheckinEmail(string email)
        {
            //Log start
            Log.Info("[START] UserController - EmailIsExist");
            return (user_BLL.EmailIsExist(email));
        }

        [HttpGet]
        [Route("GetUserInfoByUserName/{username}")]
        public JsonResult<DtoUserInfo> GetUserInfoByUserName(string username)
        {
            //Log start
            Log.Info("[START] UserController - GetUserInfoByUserName");
            return Json<DtoUserInfo>(user_BLL.GetUserInfoByUserName(username));
        }

        [HttpGet]
        [Route("GetUserInfoById/{userId:int}")]
        public JsonResult<DtoUserInfo> GetUserInfoById(int userId)
        {
            //Log start
            Log.Info("[START] UserController - GetUserInfoById");
            return Json<DtoUserInfo>(user_BLL.GetUserInfoById(userId));
        }

        [HttpPut]
        [Route("UpdateUser")]
        public bool UpdatUser(DtoUserInfo dtoUserInfo)
        {
            //Log start
            Log.Info("[START] UserController - UpdatUser");
            return (user_BLL.UpdateUser(dtoUserInfo));
        }

        [HttpPut]
        [Route("UpdateUserWithToken/{token}/{username}/")]
        public int UpdateUserWithToken(string token, string username, DtoUserInfo dtoUserInfo)
        {
            //Log start
            Log.Info("[START] UserController - UpdateUserWithToken");
            if (!account_BLL.UserNameIsExitst(username)) return -2; //User name is not exist
            string tokenUsername = TokenManager.ValidateToken(token);
            if (username.Equals(tokenUsername))
            {
                if (user_BLL.UpdateUser(dtoUserInfo))
                {
                    Log.Info("[END] UserController - UpdateUserWithToken [Result: Success][Detail: Success]");
                    return 1; //Update success
                }
                Log.Info("[END] UserController - UpdateUserWithToken [Result: Failed][Detail: Failed to Update]");
                return -1; //Failed to update
            }
            else
            {
                Log.Info("[END] UserController - UpdateUserWithToken [Result: Failed][Detail: InvalidToken]");
                return -3; //Invalid token
            }
                
        }

        
    }
}
