using DAL.Descrypt_Encrypt;
using DAL.EF;
using DAL.MappingClass;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Account_DAL
    {
        NowFoodDBEntities db = new NowFoodDBEntities();
        public Account_DAL() { }

        public bool AccountIsExist(string userName, string password)
        {
            EncryptDecrypt encrypt = new EncryptDecrypt();
            var account = db.UserAccounts.Where(t => t.UserName == userName && t.Password == password).SingleOrDefault();
            if (account != null)
                return true;
            return false;
        }

        public bool UserNameIsExist(string userName)
        {
            var account = db.UserAccounts.Where(t => t.UserName == userName).SingleOrDefault();
            if (account != null)
                return true;
            return false;
        }

        public int InsertUserAccount(RegisterAccount registerAccount)
        {
            if (!UserNameIsExist(registerAccount.Username))
            {
                User user = ParseDataToUser(registerAccount);
                UserAccount userAccount = ParseDataToAccount(registerAccount);
                if (user != null && userAccount != null)
                {
                    User_DAL user_DAL = new User_DAL();
                    bool resultAddUser = user_DAL.InsertUser(user);
                    if (resultAddUser)
                    {
                        bool resultAddAccount = InsertAccount(userAccount);
                        if (resultAddAccount) 
                            return 1;
                        return -1;
                    }
                    return -1;
                }
                return -1;
            }       
            return 0;
        }

        public User ParseDataToUser(RegisterAccount registerAccount)
        {
            if(registerAccount != null)
            {
                User user = new User();
                user.Name = registerAccount.Name;
                user.Phone = registerAccount.Phone;
                user.Email = registerAccount.Email;
                user.Gender = registerAccount.Gender;
                user.RegisterAt = registerAccount.CreateDay;
                user.UserName = registerAccount.Username;
                return user;
            }
            return null;
        }

        public UserAccount ParseDataToAccount(RegisterAccount registerAccount)
        {
            if(registerAccount != null)
            {
                UserAccount userAccount = new UserAccount();
                userAccount.UserName = registerAccount.Username;
                userAccount.Password = registerAccount.Password;
                userAccount.FailedPasswordCount = 0;
                userAccount.Status = 1;
                userAccount.CreateTime = registerAccount.CreateDay;
                userAccount.CreateBy = registerAccount.CreateBy;
                return userAccount;
            }
            return null;
        }

        public bool InsertAccount(UserAccount userAccount)
        {
            db.UserAccounts.Add(userAccount);
            int result = db.SaveChanges();
            if (result == 1) 
                return true;
            return false;
        }
    }
}
