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


        public UserInfo GetUserInfoByUserName(string userName)
        {
            var userInfo = from user in db.Users
                           join user_acc in db.UserAccounts on user.UserName equals user_acc.UserName
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
                               RoleName = "User"
                           };
            return userInfo.FirstOrDefault();
        }

        public bool InsertUser(User user)
        {
            NowFoodDBEntities dbNow = new NowFoodDBEntities();
            dbNow.Users.Add(user);
            int result = dbNow.SaveChanges();
            if (result == 1)
                return true;
            return false;

        }
        
    }
}
