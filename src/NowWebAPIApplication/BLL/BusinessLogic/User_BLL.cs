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

        public List<DtoUserInfo> GetAllUserInfo()
        {
            EntityMapper<UserInfo, DtoUserInfo> mapObj = new EntityMapper<UserInfo, DtoUserInfo>();
            List<UserInfo> userInfos = user_DAL.GetAllUser();
            List<DtoUserInfo> dtoUserInfos = new List<DtoUserInfo>();
            foreach (var item in userInfos)
            {
                dtoUserInfos.Add(mapObj.Translate(item));
            }
            return dtoUserInfos;
        }

        public DtoUserInfo GetUserInfoByUserName(string username)
        {
            EntityMapper<UserInfo, DtoUserInfo> mapObj = new EntityMapper<UserInfo, DtoUserInfo>();
            UserInfo userInfo = user_DAL.GetUserInfoByUserName(username);
            DtoUserInfo dtoUserInfo = mapObj.Translate(userInfo);
            return dtoUserInfo;
        }

        public DtoUserInfo GetUserInfoById(int userId)
        {
            EntityMapper<UserInfo, DtoUserInfo> mapObj = new EntityMapper<UserInfo, DtoUserInfo>();
            UserInfo userInfo = user_DAL.GetUserInfoById(userId);
            DtoUserInfo dtoUserInfo = mapObj.Translate(userInfo);
            return dtoUserInfo;
        }

        public bool UpdateUser(DtoUserInfo dtoUserInfo)
        {
            EntityMapper<DtoUserInfo, UserInfo> mapObj = new EntityMapper<DtoUserInfo, UserInfo>();
            UserInfo userInfo = mapObj.Translate(dtoUserInfo);
            return user_DAL.UpdateUser(userInfo);
        }
    }
}
