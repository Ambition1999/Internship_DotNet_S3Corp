using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class User_DAL
    {
        NowFoodDBEntities db = new NowFoodDBEntities();
        public User_DAL() { }

        public List<UserInfo> GetAllUser()
        {
            var userInfos = from user in db.Users
                            join user_acc in db.UserAccounts on user.UserName equals user_acc.UserName
                            join acc_role in db.AccountRoles on user_acc.Role equals acc_role.Id
                            select new UserInfo
                            {
                                UserId = user.Id,
                                UserName = user.UserName,
                                Name = user.Name,
                                Email = user.Email,
                                Phone = user.Phone,
                                Gender = user.Gender,
                                Status = user_acc.Status,
                                RoleName = acc_role.RoleName,
                                RegisterAt = user.RegisterAt
                           };
            return userInfos.ToList();
        }

        public UserInfo GetUserInfoByUserName(string userName)
        {
            var userInfo = from user in db.Users
                           join user_acc in db.UserAccounts on user.UserName equals user_acc.UserName
                           join acc_role in db.AccountRoles on user_acc.Role equals acc_role.Id
                           where user.UserName == userName
                           select new UserInfo
                           {
                               UserId = user.Id,
                               UserName = user.UserName,
                               Name = user.Name,
                               Email = user.Email,
                               Phone = user.Phone,
                               Gender = user.Gender,
                               Status = user_acc.Status,
                               RoleName = acc_role.RoleName
                           };
            return userInfo.FirstOrDefault();
        }

        public UserInfo GetUserInfoById(int userId)
        {
            var userInfo = from user in db.Users
                           join user_acc in db.UserAccounts on user.UserName equals user_acc.UserName
                           join acc_role in db.AccountRoles on user_acc.Role equals acc_role.Id
                           where user.Id == userId
                           select new UserInfo
                           {
                               UserId = user.Id,
                               UserName = user.UserName,
                               Name = user.Name,
                               Email = user.Email,
                               Phone = user.Phone,
                               Gender = user.Gender,
                               Status = user_acc.Status,
                               RoleName = acc_role.RoleName
                           };
            return userInfo.FirstOrDefault();
        }

        public bool UpdateUser(UserInfo userInfo)
        {
            NowFoodDBEntities dbNow = new NowFoodDBEntities();
            using (var transaction = dbNow.Database.BeginTransaction())
            {
                int result = -1;
                var user = dbNow.Users.Where(t => t.Id == userInfo.UserId).FirstOrDefault();
                if (user != null)
                {
                    try
                    {
                        if (userInfo.Email != null)
                            user.Email = userInfo.Email;
                        if (userInfo.Gender != null)
                            user.Gender = userInfo.Gender;
                        result = dbNow.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        result = -2;
                    }   
                }
                if (result == 1)
                    return true;
                return false;
            }  
        }

        public string EmailIsExist(string email)
        {
            NowFoodDBEntities dbNow = new NowFoodDBEntities();
            var userName = (from user in db.Users
                            join acc in db.Users on user.UserName equals acc.UserName
                            where (user.Email == email)
                            select user).SingleOrDefault();
            if(userName != null)
            {
                return userName.UserName;
            }
            return string.Empty;          
        }

    }
}
