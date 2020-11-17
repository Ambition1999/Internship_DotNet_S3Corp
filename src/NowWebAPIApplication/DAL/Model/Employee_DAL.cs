using DAL.EF;
using DAL.MappingClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Employee_DAL
    {
        NowFoodDBEntities db = new NowFoodDBEntities();
        public Employee_DAL() { }

        public EmployeeInfo GetEmployeeInfoByUserName(string userName)
        {
            var employeeInfo = from emp in db.Employees
                           join user_acc in db.UserAccounts on emp.UserName equals user_acc.UserName
                           where emp.UserName == userName
                           select new EmployeeInfo
                           {
                               Id = emp.Id,
                               UserName = emp.UserName,
                               Name = emp.Name,
                               Email = emp.Email,
                               Phone = emp.Phone,
                               Gender = emp.Gender,
                           };
            return employeeInfo.FirstOrDefault();
        }
    }
}
