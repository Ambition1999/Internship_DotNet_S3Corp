using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Descrypt_Encrypt;
using DAL.EF;
using DAL.MappingClass;

namespace BLL.ModuleParse
{
    public static class ParseDtoToEFModel
    {
        public static User ParseDtoRegisterToUser(RegisterAccount registerAccount)
        {
            if (registerAccount != null)
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

        public static UserAccount ParseDtoRegisterToAccount(RegisterAccount registerAccount)
        {
            if (registerAccount != null)
            {

                UserAccount userAccount = new UserAccount();
                userAccount.UserName = registerAccount.Username;
                userAccount.Password = EncryptDecrypt.Encrypt(registerAccount.Password);
                userAccount.FailedPasswordCount = 0;
                userAccount.Status = 1;
                userAccount.Role = 1;
                userAccount.CreateTime = registerAccount.CreateDay;
                userAccount.CreateBy = registerAccount.CreateBy;
                return userAccount;
            }
            return null;
        }

        //public static Order ParseDtoOrder_OrderItemsToOrder(Dto)
    }
}
