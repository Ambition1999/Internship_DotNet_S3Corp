using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using UILayer.Service;
using Model;
using Model.Model_Mapper;
using UILayer.Models;

namespace UILayer.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllRestaurant()
        {
            ServiceRepository serviceObject = new ServiceRepository();
            HttpResponseMessage response = serviceObject.GetResponse("api/restaurant/getallrestaurant/");
            response.EnsureSuccessStatusCode();
            List<DtoRestaurantInfo> restaurants = response.Content.ReadAsAsync<List<DtoRestaurantInfo>>().Result;
            ViewBag.Title = "All Restaurant";
            return View("~/Views/RestaurantView/Restaurant.cshtml", restaurants);

        }

        public ActionResult GetRestaurantByTagName(int kindId)
        {
            ServiceRepository serviceObject = new ServiceRepository();
            HttpResponseMessage response = serviceObject.GetResponse("api/Restaurant/GetRestaurantInfoByKindId/" + kindId);
            response.EnsureSuccessStatusCode();
            List<DtoRestaurantInfo> restaurants = response.Content.ReadAsAsync<List<DtoRestaurantInfo>>().Result;
            ViewBag.Title = "All Restaurant";
            return View("~/Views/RestaurantView/Restaurant.cshtml", restaurants);

        }

        public ActionResult GetRestaurantDetailByID()
        {
            return View("~/Views/RestaurantView/RestaurantDetail.cshtml");
        }

        public ActionResult GetRestaurantInfo_MenuItemInfoByID(int restaurantId)
        {
            
            ServiceRepository serviceObject = new ServiceRepository();
            //Get Restaurant Infomation
            HttpResponseMessage response = serviceObject.GetResponse("api/Restaurant/GetRestaurantInfoById/"+ restaurantId);
            response.EnsureSuccessStatusCode();
            DtoRestaurantInfo restaurantInfo = response.Content.ReadAsAsync<DtoRestaurantInfo>().Result;

            //Get Menu Item of Restaurant
            HttpResponseMessage responseMenuItem = serviceObject.GetResponse("api/menuitem/GetMenuRestaurantInfoByID/" + restaurantId);
            response.EnsureSuccessStatusCode(); 
            List<DtoMenuItemInfo> menuItemInfos = responseMenuItem.Content.ReadAsAsync<List<DtoMenuItemInfo>>().Result;
            var temp = menuItemInfos.GroupBy(t => t.TypeId, t => t.Id, (key, g) => new { TypeId = key, ItemIds = g.ToList(), 
                                            TypeName = menuItemInfos.Where(t=>t.TypeId == key).Select(t=>t.TypeName).FirstOrDefault()});
            List<ItemType> itemTypes = new List<ItemType>();
            foreach (var item in temp)
            {
                ItemType itemType = new ItemType();
                itemType.Id = item.TypeId;
                itemType.TypeName = item.TypeName;
                itemType.MenuItemInfos = menuItemInfos.Where(t=>t.TypeId == item.TypeId).ToList();
                itemTypes.Add(itemType);
            }
            var model = new RestaurantDetail { RestaurantInfo = restaurantInfo, ItemTypes = itemTypes };
            return View("~/Views/RestaurantView/RestaurantDetail.cshtml", model);
        }


    }
}