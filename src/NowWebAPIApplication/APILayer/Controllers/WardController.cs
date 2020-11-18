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
    [RoutePrefix("api/Ward")]
    public class WardController : ApiController
    {
        Ward_BLL ward_BLL = new Ward_BLL();
        public WardController() { }

        [HttpGet]
        [Route("GetWardByDistricId/{districtId:int}")]
        public JsonResult<List<DtoWard>> GetWardByDistricId(int districtId)
        {
            return Json<List<DtoWard>>(ward_BLL.GetWardByDistrictId(districtId));
        }
    }
}
