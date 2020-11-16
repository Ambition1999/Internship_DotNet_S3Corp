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

        public int InsertUserAccount(DtoRegisterAccount dtoRegisterAccount)
        {
            EntityMapper<DtoRegisterAccount, RegisterAccount> mapObjRegis = new EntityMapper<DtoRegisterAccount, RegisterAccount>();
            RegisterAccount registerAccountTemp = mapObjRegis.Translate(dtoRegisterAccount); 
            return account_dal.InsertUserAccount2(registerAccountTemp);
        }

        public bool UpdateAccount(DtoUpdateAccount dtoUpdateAccount)
        {
            EntityMapper<DtoUpdateAccount, UpdateAccount> mapObjRegis = new EntityMapper<DtoUpdateAccount, UpdateAccount>();
            UpdateAccount updateAccount = mapObjRegis.Translate(dtoUpdateAccount);
            return account_dal.UpdatePassword(updateAccount);
        }
    }
}
