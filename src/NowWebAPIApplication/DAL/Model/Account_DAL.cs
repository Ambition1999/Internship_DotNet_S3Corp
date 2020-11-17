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
            string encryptPassword = EncryptDecrypt.Encrypt(password);
            var account = db.UserAccounts.Where(t => t.UserName == userName && t.Password == encryptPassword).SingleOrDefault();
            if (account != null)
                return true;
            return false;
        }

        public bool AccountAdminIsExist(string userName, string password)
        {
            string encryptPassword = EncryptDecrypt.Encrypt(password);
            var accountAdmin = from useraccount in db.UserAccounts
                               join emp in db.Employees on useraccount.UserName equals emp.UserName
                               where useraccount.UserName == userName && useraccount.Password == encryptPassword
                               select (emp.Id);   
            if (accountAdmin != null)
                return true;
            return false;
        }

        public bool UserNameIsExist(string userName)
        {
            NowFoodDBEntities dbNow = new NowFoodDBEntities();
            var account = dbNow.UserAccounts.Where(t => t.UserName == userName).SingleOrDefault();
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

        public int InsertUserAccount2(RegisterAccount registerAccount)
        {
            NowFoodDBEntities context = new NowFoodDBEntities();
            using (var transaction = context.Database.BeginTransaction())
            {
                int result = -1;
                if (!UserNameIsExist(registerAccount.Username))
                {
                    User user = ParseDataToUser(registerAccount);
                    UserAccount userAccount = ParseDataToAccount(registerAccount);
                    if (user != null && userAccount != null)
                    {
                        try
                        {
                            User_DAL user_DAL = new User_DAL();
                            user_DAL.InsertUser2(user, context);
                            InsertAccount2(userAccount, context);
                            result = context.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            result = -2;
                        }   
                    }
                    else
                    {
                        result = -1;
                    }
                }
                else
                {
                    result = -3;
                }
                return result;
            }
            
        }

        public User ParseDataToUser(RegisterAccount registerAccount)
        {
            if(registerAccount != null)
            {
                User user = new User();
                user.Id = 0;
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
                userAccount.Password = EncryptDecrypt.Encrypt(registerAccount.Password);
                userAccount.FailedPasswordCount = 0;
                userAccount.Status = 1;
                userAccount.CreateTime = registerAccount.CreateDay;
                userAccount.CreateBy = registerAccount.CreateBy;
                return userAccount;
            }
            return null;
        }

        public bool UpdatePassword(UpdateAccount updateAccount)
        {
            int result = -1;
            NowFoodDBEntities dbNow = new NowFoodDBEntities();
            string encryptPassword = EncryptDecrypt.Encrypt(updateAccount.Password);
            var account = dbNow.UserAccounts.Where(t => t.UserName == updateAccount.UserName && t.Password == encryptPassword).FirstOrDefault();
            if (account != null)
            {
                account.Password = EncryptDecrypt.Encrypt(updateAccount.NewPassword);
                account.UpdateBy = "User";
                account.UpdateTime = DateTime.Now;
                
                result = dbNow.SaveChanges();
            }
            if (result == 1)
                return true;
            return false;
        }

        public bool InsertAccount(UserAccount userAccount)
        {
            NowFoodDBEntities dbNow = new NowFoodDBEntities();
            dbNow.UserAccounts.Add(userAccount);
            int result = dbNow.SaveChanges();
            if (result == 1) 
                return true;
            return false;
        }

        public void InsertAccount2(UserAccount userAccount, NowFoodDBEntities context)
        {
            context.UserAccounts.Add(userAccount);
        }
    }
}
