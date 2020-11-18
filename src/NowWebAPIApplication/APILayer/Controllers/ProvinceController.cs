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
    [RoutePrefix("api/Province")]
    public class ProvinceController : ApiController
    {
        Province_BLL province_BLL = new Province_BLL();
        public ProvinceController() { }

        [HttpGet]
        [Route("GetAllProvince/")]
        public JsonResult<List<DtoProvince>> GetAllProvince()
        {
            return Json<List<DtoProvince>>(province_BLL.GetAllProvince());
        }
    }
}
