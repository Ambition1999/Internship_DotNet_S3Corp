using DAL.MappingClass;
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
    public class Employee_BLL
    {
        Employee_DAL employee_DAL = new Employee_DAL();
        public Employee_BLL() { }

        public DtoEmployeeInfo GetEmployeeInfoByUserName(string username)
        {
            EntityMapper<EmployeeInfo, DtoEmployeeInfo> mapObj = new EntityMapper<EmployeeInfo, DtoEmployeeInfo>();
            EmployeeInfo employeeInfo = employee_DAL.GetEmployeeInfoByUserName(username);
            DtoEmployeeInfo dtoEmployeeInfo = mapObj.Translate(employeeInfo);
            return dtoEmployeeInfo;
        }
    }
}
