using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using UILayer.Service;
using Model;
using Model.Model_Mapper;
using UILayer.Models;
using UILayer.CacheDictionary;
using UILayer.Formater;
using Microsoft.Ajax.Utilities;
using System.Runtime.CompilerServices;
using System.Web.UI;
using System.Web;


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

        //[HttpPost]
        //public ActionResult GetRestaurantByName()
        //{
        //    string searchInput = Request["txtSearchHome"];
        //    ServiceRepository serviceObject = new ServiceRepository();
        //    HttpResponseMessage response = serviceObject.GetResponse("api/Restaurant/GetRestaurantInfoByName/" + searchInput);
        //    response.EnsureSuccessStatusCode();
        //    List<DtoRestaurantInfo> restaurants = response.Content.ReadAsAsync<List<DtoRestaurantInfo>>().Result;
        //    ViewBag.Title = "All Restaurant";
        //    return View("~/Views/RestaurantView/Restaurant.cshtml", restaurants);

        //}

        [HttpPost]
        public ActionResult GetRestaurantByName()
        {
            // Get Search String
            string searchInput = Request["txtSearchHome"];

            // Get ListId Restaurant in Dictionanry
            Cache<int, string> Cache = LoadDataToCache.Cache;

            if(Cache == null)
            {
                // Load data to Cache
                LoadDataToCache.LoadRestaurantToCache();
                Cache = LoadDataToCache.Cache;
            }

            List<int> listId = Cache.GetKeysByValue(searchInput);

            if(listId.Count == 0)
            {
                // Alter message not found
                ViewBag.SearchMessage = "Không tìm thấy dữ liệu phù hợp";
                return View("~/Views/RestaurantView/Restaurant.cshtml");
            }

            // Format ListId to string for passing data to Web API
            string stringId = FormaterModel.convertListIdToString(listId);

            // Call API
            ServiceRepository serviceObject = new ServiceRepository();
            HttpResponseMessage response = serviceObject.GetResponse("api/Restaurant/GetRestaurantInfoByListId/" + stringId);
            response.EnsureSuccessStatusCode();
            List<DtoRestaurantInfo> restaurants = response.Content.ReadAsAsync<List<DtoRestaurantInfo>>().Result;
            
            return View("~/Views/RestaurantView/Restaurant.cshtml", restaurants);
        }

        public static void ShowMessage(string message)
        {
            
        }

        

    }
}