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
using System.Web.DynamicData;
using Model.DTO;
using log4net;
using System.Web.Security;

namespace UILayer.Controllers
{
    public class RestaurantController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Restaurant
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadRestaurantCache()
        {
            ServiceRepository serviceObject = new ServiceRepository();
            HttpResponseMessage response = serviceObject.GetResponse("api/restaurant/getallrestaurant/");
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                List<DtoRestaurantInfo> restaurants = response.Content.ReadAsAsync<List<DtoRestaurantInfo>>().Result;

                if(restaurants != null)
                {
                    List<Dictionary<int, DtoRestaurantInfo>> RestaurantsCache = LoadDataToCache.RestaurantsCache;

                    if (RestaurantsCache == null)
                    {
                        // Load data to Cache
                        LoadDataToCache.LoadAllRestaurantToCache(restaurants);
                        RestaurantsCache = LoadDataToCache.RestaurantsCache;
                    }

                    // Check Cookie and Auto Login
                    //if(HttpContext.Response.Cookies.AllKeys.Contains(".ASPXAUTH"))
                    if (Session["UserLogin"] == null && HttpContext.Request.Cookies.AllKeys.Contains(".ASPXAUTH"))
                    {
                        HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

                        string useriId = ticket.Name;
                        int userID = Int32.Parse(useriId);
                        AutoLoginWithCookie(userID);
                    }

                    return View("~/Views/MainPage/MainPage.cshtml", restaurants);
                }
                else
                {
                    return View("~/Views/RestaurantView/RestaurantEmptyPage.cshtml");
                }
            }
            else
            {
                return View("~/Views/RestaurantView/RestaurantEmptyPage.cshtml");
            }
        }

        public ActionResult GetAllRestaurant()
        {
            ServiceRepository serviceObject = new ServiceRepository();
            HttpResponseMessage response = serviceObject.GetResponse("api/restaurant/getallrestaurant/");
            if (response.IsSuccessStatusCode)
            {
                List<DtoRestaurantInfo> restaurants = response.Content.ReadAsAsync<List<DtoRestaurantInfo>>().Result;
                if(restaurants != null && restaurants.Count != 0)
                {
                    ViewBag.Title = "All Restaurant";
                    return View("~/Views/RestaurantView/Restaurant.cshtml", restaurants);
                }
                TempData["MainPageMessage"] = "Không tìm thấy dữ liệu, vui lòng thử lại";
                TempData["MainPageMessageColor"] = "warning";
                return View("~/Views/RestaurantView/RestaurantEmptyPage.cshtml");
            }
            else
            {
                TempData["MainPageMessage"] = "Lỗi kết nối máy chủ, vui lòng thử lại";
                TempData["MainPageMessageColor"] = "danger";
                return Redirect(this.Request.UrlReferrer.ToString());
            }
           
        }


        public ActionResult GetRestaurantByTagName(int kindId)
        {
            ServiceRepository serviceObject = new ServiceRepository();
            HttpResponseMessage response = serviceObject.GetResponse("api/Restaurant/GetRestaurantInfoByKindId/" + kindId);
            if (response.IsSuccessStatusCode)
            {
                List<DtoRestaurantInfo> restaurants = response.Content.ReadAsAsync<List<DtoRestaurantInfo>>().Result;
                if (restaurants != null)
                {
                    ViewBag.Title = "All Restaurant";
                    return View("~/Views/RestaurantView/Restaurant.cshtml", restaurants);
                }
                return View("~/Views/RestaurantView/RestaurantEmptyPage.cshtml");
            }
            else
            {
                TempData["MainPageMessage"] = "Lỗi kết nối máy chủ, vui lòng thử lại";
                TempData["MainPageMessageColor"] = "danger";
                return RedirectToAction("LoadRestaurantCache", "Restaurant");
            }
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
            //response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                DtoRestaurantInfo restaurantInfo = response.Content.ReadAsAsync<DtoRestaurantInfo>().Result;
                if (restaurantInfo != null)
                {
                    //Get Menu Item of Restaurant
                    HttpResponseMessage responseMenuItem = serviceObject.GetResponse("api/menuitem/GetMenuRestaurantInfoByID/" + restaurantId);
                    if (responseMenuItem.IsSuccessStatusCode)
                    {
                        List<DtoMenuItemInfo> menuItemInfos = responseMenuItem.Content.ReadAsAsync<List<DtoMenuItemInfo>>().Result;
                        if (menuItemInfos != null)
                        {
                            var temp = menuItemInfos.GroupBy(t => t.TypeId, t => t.Id, (key, g) => new
                            {
                                TypeId = key,
                                ItemIds = g.ToList(),
                                TypeName = menuItemInfos.Where(t => t.TypeId == key).Select(t => t.TypeName).FirstOrDefault()
                            });
                            List<ItemType> itemTypes = new List<ItemType>();
                            foreach (var item in temp)
                            {
                                ItemType itemType = new ItemType();
                                itemType.Id = item.TypeId;
                                itemType.TypeName = item.TypeName;
                                itemType.MenuItemInfos = menuItemInfos.Where(t => t.TypeId == item.TypeId).ToList();
                                itemTypes.Add(itemType);
                            }
                            RestaurantDetail model = new RestaurantDetail { RestaurantInfo = restaurantInfo, ItemTypes = itemTypes };
                            return View("~/Views/RestaurantView/RestaurantDetail.cshtml", model);
                        }
                        
                    }
                        
                }
            }
            TempData["RestaurantPageMessage"] = "Lỗi kết nối máy chủ, vui lòng thử lại";
            TempData["RestaurantPageMessageColor"] = "danger";
            return Redirect(this.Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult GetRestaurantByName()
        {
            // Get Search String
            string searchInput = Request["txtSearchHome"];

            //Cache<int, DtoRestaurantInfo> Cache = LoadDataToCache.CacheRestaurant;
            List<Dictionary<int, DtoRestaurantInfo>> cacheRestaurants = LoadDataToCache.RestaurantsCache;

            if (cacheRestaurants == null)
            {
                // Load data to Cache
                LoadDataToCache.LoadAllRestaurantToCache();
                cacheRestaurants = LoadDataToCache.RestaurantsCache;
            }

            //Need optimize and performance
            List<int> listId = LoadDataToCache.SearchList(cacheRestaurants,searchInput);

            if (listId.Count == 0)
            {
                // Alter message not found
                ViewBag.SearchMessage = "Không tìm thấy dữ liệu phù hợp";
                return View("~/Views/RestaurantView/RestaurantEmptyPage.cshtml");
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


        public ActionResult CreateRestaurantCart(int resId)
        {
            if (Session[resId.ToString()] != null)
            {
                RestaurantCart restCart = (RestaurantCart)Session[resId.ToString()];
                return PartialView("~/Views/RestaurantView/RestaurantCart.cshtml", restCart);
            }
            else
            {
                RestaurantCart restCart = new RestaurantCart();
                restCart.ResId = resId;
                restCart.TotalAmount = 0;
                restCart.TotalPrice = 0;
                restCart.ItemCarts = new List<ItemCart>();
                return PartialView("~/Views/RestaurantView/RestaurantCart.cshtml", restCart);
            }
        }

        //public ActionResult AddItemToCart(int resId, int itemId)
        //{
        //    if (Session[resId.ToString()] != null)
        //    {
        //        RestaurantCart restCart = (RestaurantCart)Session[resId.ToString()];
        //        return PartialView("~/Views/RestaurantView/RestaurantCart.cshtml", restCart);
        //    }
        //}

        public bool AutoLoginWithCookie(int userId)
        {
            DtoUserInfo dtoUserInfo;
            try
            {
                Log.Info("--- Call API GetUserInfoById");
                ServiceRepository service = new ServiceRepository();
                HttpResponseMessage response = service.GetResponse("/api/user/getuserinfobyId/" + userId);
                
                dtoUserInfo = response.Content.ReadAsAsync<DtoUserInfo>().Result;
                UserLogin userLogin = new UserLogin();
                userLogin.UserName = dtoUserInfo.UserName;
                userLogin.UserId = dtoUserInfo.UserId;

                Session["UserLogin"] = userLogin;

                //Gen User Token and save to Cookie
                string token = Modules.TokenGenerateModule.GetToken(userLogin.UserName);
                HttpCookie cookie = new HttpCookie("token");
                HttpContext.Response.Cookies.Remove("token");
                cookie.Expires = DateTime.Now.AddDays(1);
                cookie.Value = token;
                HttpContext.Response.SetCookie(cookie);
                Log.Info("[END] UserController - LoginAccount [Result: Success][Detail: Đăng nhập thành công]");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error to rest API: Get UserInfo by Username", ex);
                return false;
            }
        }

    }
}