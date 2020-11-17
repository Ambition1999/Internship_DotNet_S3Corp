using BLL.BusinessLogic;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace APILayer.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {
        Employee_BLL emp_BLL = new Employee_BLL();
        public EmployeeController() { }

        [HttpGet]
        [Route("GetEmployeeInfoByUserName/{username}")]
        public JsonResult<DtoEmployeeInfo> GetEmployeeInfoByUserName(string username)
        {
            return Json<DtoEmployeeInfo>(emp_BLL.GetEmployeeInfoByUserName(username));
        }
    }
}
