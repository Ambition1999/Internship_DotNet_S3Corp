using BLL.BusinessLogic;
using Model.Model_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.UI.WebControls;

namespace APILayer.Controllers
{
    [RoutePrefix("api/MenuItem")]
    public class MenuItemController : ApiController
    {
        MenuItem_BLL menuItem_BLL = new MenuItem_BLL();
        public MenuItemController() { }

        [HttpGet]
        [Route("GetMenuRestaurantInfoByID/{RestaurantId:int}")]
        public JsonResult<List<DtoMenuItemInfo>> GetMenuRestaurantInfoByID(int RestaurantId)
        {
            return Json<List<DtoMenuItemInfo>>(menuItem_BLL.GetMenuRestaurantInfoByID(RestaurantId));
        }

        [HttpGet]
        [Route("GetMenuRestaurantInfoByItemId/{itemId:int}")]
        public JsonResult<DtoMenuItemInfo> GetMenuRestaurantInfoByItemId(int itemId)
        {
            return Json<DtoMenuItemInfo>(menuItem_BLL.GetDtoMenuItemInfoByItemId(itemId));
        }
    }
}
