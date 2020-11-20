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
            var accountAdmin = (from useraccount in db.UserAccounts
                                join emp in db.Employees on useraccount.UserName equals emp.UserName
                                where useraccount.UserName == userName && useraccount.Password == encryptPassword
                                select emp).SingleOrDefault();   
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


        public int InsertUserAccount(User user,UserAccount userAccount)
        {
            NowFoodDBEntities context = new NowFoodDBEntities();
            using (var transaction = context.Database.BeginTransaction())
            {
                int result = -1;
                if (!UserNameIsExist(user.UserName) && user != null && userAccount != null)
                {
                    try
                    {
                        context.UserAccounts.Add(userAccount);
                        context.Users.Add(user);
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
                    result = -3;
                }
                return result;
            }
        }


        public bool UpdatePassword(UpdateAccount updateAccount)
        {
            int result = -1;
            NowFoodDBEntities dbNow = new NowFoodDBEntities();
            using (var transaction = dbNow.Database.BeginTransaction())
            {
                string encryptPassword = EncryptDecrypt.Encrypt(updateAccount.Password);
                var account = dbNow.UserAccounts.Where(t => t.UserName == updateAccount.UserName && t.Password == encryptPassword).FirstOrDefault();
                if (account != null)
                {
                    try
                    {
                        account.Password = EncryptDecrypt.Encrypt(updateAccount.NewPassword);
                        account.UpdateBy = "User";
                        account.UpdateTime = DateTime.Now;

                        result = dbNow.SaveChanges();
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
            if (result >= 1)
                return true;
            return false;
        }
    }
}
