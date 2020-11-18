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
    [RoutePrefix("api/District")]
    public class DistrictController : ApiController
    {
        District_BLL distric_BLL = new District_BLL();
        public DistrictController() { }

        [HttpGet]
        [Route("GetDistrictByProvinceId/{provinceId:int}")]
        public JsonResult<List<DtoDistrict>> GetDistrictByProvinceId(int provinceId)
        {
            return Json<List<DtoDistrict>>(distric_BLL.GetDistrictByProvinceId(provinceId));
        }
    }
}
