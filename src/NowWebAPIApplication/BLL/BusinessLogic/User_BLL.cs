using DAL.Model;
using Model.DTO;
using Model.EF_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BusinessLogic
{
    public class User_BLL
    {
        User_DAL user_DAL = new User_DAL();
        public User_BLL() { }

        public DtoUserInfo GetUserInfoByUserName(string username)
        {
            EntityMapper<UserInfo, DtoUserInfo> mapObj = new EntityMapper<UserInfo, DtoUserInfo>();
            UserInfo userInfo = user_DAL.GetUserInfoByUserName(username);
            DtoUserInfo dtoUserInfo = mapObj.Translate(userInfo);
            return dtoUserInfo;
        }
    }
}
