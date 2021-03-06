using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Web.Http.Results;
using Model.EF_Mapper;
using Model;
using BLL.BusinessLogic;
using Model.Model_Mapper;

namespace APILayer.Controllers
{
    [RoutePrefix("api/Restaurant")]
    public class RestaurantController : ApiController
    {
        Restaurant_BLL res_bll = new Restaurant_BLL();
        public RestaurantController() { }
        //[EnableCors(origins: "*", Header: "*", methods: "*")]

        //[HttpGet]
        //public JsonResult<List<Restaurant_Mapper>> GetAllRestaurant()
        //{
        //    return Json<List<Restaurant_Mapper>>(res_bll.GetAllRestaurant());
        //}

        [HttpGet]
        public JsonResult<List<DtoRestaurantInfo>> GetAllRestaurantInfo()
        {
            return Json<List<DtoRestaurantInfo>>(res_bll.GetAllRestaurantInfo());
        }

        [HttpGet]
        [Route("GetRestaurantInfoById/{restaurantId:int}")]
        public JsonResult<DtoRestaurantInfo> GetRestaurantInfoById(int restaurantId)
        {
            return Json<DtoRestaurantInfo>(res_bll.GetRestaurantInfoById(restaurantId));
        }

        [HttpGet]
        [Route("GetRestaurantInfoByKindId/{kindId:int}")]
        public JsonResult<List<DtoRestaurantInfo>> GetRestaurantInfoByKindId(int kindId)
        {
            return Json<List<DtoRestaurantInfo>>(res_bll.GetRestaurantInfoByKindId(kindId));
        }

        [HttpGet]
        [Route("GetRestaurantInfoByName/{name}")]
        public JsonResult<List<DtoRestaurantInfo>> GetRestaurantInfoByName(string name)
        {
            return Json<List<DtoRestaurantInfo>>(res_bll.GetAllRestaurantInfoByName(name));
        }
    }
}
