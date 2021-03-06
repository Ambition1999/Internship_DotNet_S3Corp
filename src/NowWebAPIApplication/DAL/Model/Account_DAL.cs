using DAL.Descrypt_Encrypt;
using DAL.EF;
using System;
using System.Collections.Generic;
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
    }
}
