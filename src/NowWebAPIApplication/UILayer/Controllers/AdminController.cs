using Model.DTO;
using Model.Model_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using UILayer.Models;
using UILayer.Service;

namespace UILayer.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginAdmin()
        {
            return View("~/Views/Admin/LoginAdmin.cshtml");
        }

        public ActionResult LogoutAdmin()
        {
            Session.Abandon();
            return View("~/Views/Admin/LoginAdmin.cshtml");
        }

        [HttpPost]
        public ActionResult LoginProcessing(UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                //Success
                ServiceRepository service = new ServiceRepository();
                HttpResponseMessage responseCheckAccount = service.GetResponse("/api/useraccount/CheckAdminAccount/" + userAccount.UserName + "/" + userAccount.Password);
                responseCheckAccount.EnsureSuccessStatusCode();
                BoolResult result = new BoolResult();
                result.Result = responseCheckAccount.Content.ReadAsAsync<BoolResult>().Result.Result;
                if (result.Result)
                {
                    HttpResponseMessage response = service.GetResponse("/api/Employee/GetEmployeeInfoByUserName/" + userAccount.UserName);
                    response.EnsureSuccessStatusCode();
                    DtoEmployeeInfo dtoEmpInfo = response.Content.ReadAsAsync<DtoEmployeeInfo>().Result;
                    UserLogin userLogin = new UserLogin();
                    userLogin.UserName = dtoEmpInfo.UserName;
                    userLogin.UserId = dtoEmpInfo.Id;
                    Session["AdminLogin"] = userLogin;
                    return RedirectToAction("UserManagement", "Admin");
                }
                else
                {
                    TempData["LoginMessage"] = "Tài khoản hoặc mật khẩu không chính xác, vui lòng thử lại";
                    TempData["LoginMessageColor"] = "danger";
                    return Redirect("/");
                }      
            }
            else
            {
                TempData["LoginMessage"] = "Dữ liệu không hợp lệ";
                TempData["LoginMessageColor"] = "danger";
                return Redirect("/");
            }
        }

        public ActionResult AdminLoad()
        {
            return View();
        }

        public ActionResult UserManagement()
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage response = service.GetResponse("/api/User/GetAllUserInfo/");
            response.EnsureSuccessStatusCode();
            List<DtoUserInfo> userInfos = new List<DtoUserInfo>();
            userInfos = response.Content.ReadAsAsync<List<DtoUserInfo>>().Result;
            return View("~/Views/Admin/UserManagement.cshtml", userInfos);
        }

        public ActionResult RestaurantManagement()
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage response = service.GetResponse("/api/Restaurant/getallrestaurant/");
            response.EnsureSuccessStatusCode();
            List<DtoRestaurantInfo> restaurantInfos = new List<DtoRestaurantInfo>();
            restaurantInfos = response.Content.ReadAsAsync<List<DtoRestaurantInfo>>().Result;
            return View("~/Views/Admin/RestaurantManagement.cshtml", restaurantInfos);
            // fixing return value null
        }

        public ActionResult RestaurantDetail(int restaurantId)
        {
            ServiceRepository serviceObject = new ServiceRepository();
            //Get Restaurant Infomation
            HttpResponseMessage response = serviceObject.GetResponse("api/Restaurant/GetRestaurantInfoById/" + restaurantId);
            response.EnsureSuccessStatusCode();
            DtoRestaurantInfo restaurantInfo = response.Content.ReadAsAsync<DtoRestaurantInfo>().Result;

            //Get Menu Item of Restaurant
            HttpResponseMessage responseMenuItem = serviceObject.GetResponse("api/menuitem/GetMenuRestaurantInfoByID/" + restaurantId);
            response.EnsureSuccessStatusCode();
            List<DtoMenuItemInfo> menuItemInfos = responseMenuItem.Content.ReadAsAsync<List<DtoMenuItemInfo>>().Result;
            var temp = menuItemInfos.GroupBy(t => t.TypeId, t => t.Id, (key, g) => new {
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
            return View("~/Views/Admin/AdminRestaurantDetail.cshtml", model);
        }

    }
}