using DAL.EF;
using DAL.MappingClass;
using DAL.Model;
using Model.DTO;
using Model.EF_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BusinessLogic
{
    public class Account_BLL
    {
        Account_DAL account_dal = new Account_DAL();
        public Account_BLL() { }
        public BoolResult AccountIsExits(string userName, string password)
        {
            BoolResult boolResult = new BoolResult();
            boolResult.Result = account_dal.AccountIsExist(userName, password);
            return boolResult;
        }

        public BoolResult AccountAdminIsExits(string userName, string password)
        {
            BoolResult boolResult = new BoolResult();
            boolResult.Result = account_dal.AccountAdminIsExist(userName, password);
            return boolResult;
        }

        public bool UserNameIsExitst(string username)
        {
            return account_dal.UserNameIsExist(username);
        }

        public int InsertUserAccount(DtoRegisterAccount dtoRegisterAccount)
        {
            EntityMapper<DtoRegisterAccount, RegisterAccount> mapObjRegis = new EntityMapper<DtoRegisterAccount, RegisterAccount>();
            RegisterAccount registerAccount = mapObjRegis.Translate(dtoRegisterAccount);
            //Parse Data from DtoClass to Entity Model
            User user = ModuleParse.ParseDtoToEFModel.ParseDtoRegisterToUser(registerAccount);
            UserAccount userAccount = ModuleParse.ParseDtoToEFModel.ParseDtoRegisterToAccount(registerAccount);
            return account_dal.InsertUserAccount(user,userAccount);
        }

        public bool UpdateAccount(DtoUpdateAccount dtoUpdateAccount)
        {
            EntityMapper<DtoUpdateAccount, UpdateAccount> mapObjRegis = new EntityMapper<DtoUpdateAccount, UpdateAccount>();
            UpdateAccount updateAccount = mapObjRegis.Translate(dtoUpdateAccount);
            return account_dal.UpdatePassword(updateAccount);
        }


    }
}
