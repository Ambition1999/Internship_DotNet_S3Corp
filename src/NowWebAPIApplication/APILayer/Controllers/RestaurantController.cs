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
    public class RestaurantController : ApiController
    {
        Restaurant_BLL res_bll = new Restaurant_BLL();
        public RestaurantController() { }
        //[EnableCors(origins: "*", Header: "*", methods: "*")]

        [HttpGet]
        public JsonResult<List<Restaurant_Mapper>> GetAllRestaurant()
        {
            return Json<List<Restaurant_Mapper>>(res_bll.GetAllRestaurant());
        }

        //[HttpGet]
        //public JsonResult<List<Restaurant_ResType_Ward_District_Province>> GetAllRestaurantInfo()
        //{
        //    return Json<List<Restaurant_ResType_Ward_District_Province>>(res_bll.GetRestaurantInfo());
        //}
    }
}
