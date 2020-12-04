using Model.Model_Mapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using UILayer.Models;
using UILayer.Service;

namespace UILayer.Controllers
{
    public class MenuItemController : Controller
    {

        // GET: MenuItem
        public ActionResult Index()
        {
            return View();
        }

        public DtoMenuItemInfo GetDtoMenuItemInfoById(int itemId)
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage response = service.GetResponse("/api/MenuItem/GetMenuRestaurantInfoByItemId/" + itemId);
            if (response.IsSuccessStatusCode)
            {
                DtoMenuItemInfo menuItemInfos = response.Content.ReadAsAsync<DtoMenuItemInfo>().Result;
                if(menuItemInfos != null)
                    return menuItemInfos;
            }
            return null;
        }


        public ActionResult AddItemToCart(int resId, int itemId, string itemName, double price)
        {
            if (Session[resId.ToString()] == null)
            {
                RestaurantCart restCart = new RestaurantCart();
                ItemCart itemCart = new ItemCart(itemId, itemName, 1, price);
                restCart.ItemCarts = new List<ItemCart>();
                restCart.ItemCarts.Add(itemCart);
                restCart.ResId = resId;
                restCart.TotalPrice = restCart.ItemCarts.Sum(t => t.SubTotal);
                restCart.TotalAmount = restCart.ItemCarts.Sum(t => t.Quantity);

                Session[resId.ToString()] = restCart;

                //return PartialView("~/Views/RestaurantView/RestaurantCart.cshtml", restCart);
                //return PartialView("~/Views/RestaurantView/RestaurantCart.cshtml");
                return RedirectToAction("GetRestaurantInfo_MenuItemInfoByID", "Restaurant", new { restaurantId = restCart.ResId } );
            }
            else
            {
                RestaurantCart restCart = (RestaurantCart)Session[resId.ToString()];
                var itemCart = restCart.ItemCarts.Where(t => t.ItemId == itemId).FirstOrDefault();
                if (itemCart != null)
                {
                    itemCart.Quantity += 1;
                    itemCart.SubTotal = itemCart.Price * itemCart.Quantity;
                }
                else
                {
                    ItemCart itemCartTemp = new ItemCart(itemId, itemName, 1, price);
                    restCart.ItemCarts.Add(itemCartTemp);
                }   

                // Update Total Price and Total Amount
                restCart.TotalPrice = restCart.ItemCarts.Sum(t => t.SubTotal);
                restCart.TotalAmount = restCart.ItemCarts.Sum(t => t.Quantity);

                //return  PartialView("~/Views/RestaurantView/RestaurantCart.cshtml", restCart);
                //return PartialView("~/Views/RestaurantView/RestaurantCart.cshtml");
                return RedirectToAction("GetRestaurantInfo_MenuItemInfoByID", "Restaurant", new { restaurantId = restCart.ResId });
            }
        }

        public ActionResult AddingItemInCart(int resId, int itemId)
        {
            RestaurantCart restCart = (RestaurantCart)Session[resId.ToString()];
            var itemCart = restCart.ItemCarts.Where(t => t.ItemId == itemId).FirstOrDefault();
            if (itemCart != null)
            {
                itemCart.Quantity += 1;
                itemCart.SubTotal = itemCart.Price * itemCart.Quantity;

                // Update Total Price and Total Amount
                restCart.TotalPrice = restCart.ItemCarts.Sum(t => t.SubTotal);
                restCart.TotalAmount = restCart.ItemCarts.Sum(t => t.Quantity);
            }

            return RedirectToAction("GetRestaurantInfo_MenuItemInfoByID", "Restaurant", new { restaurantId = restCart.ResId });
        }

        public ActionResult ReduceItemInCart(int resId, int itemId)
        {
            RestaurantCart restCart = (RestaurantCart)Session[resId.ToString()];
            var itemCart = restCart.ItemCarts.Where(t => t.ItemId == itemId).FirstOrDefault();
            if (itemCart != null)
            {
                if(itemCart.Quantity > 1)
                {
                    itemCart.Quantity -= 1;
                    itemCart.SubTotal = itemCart.Price * itemCart.Quantity;
                    
                }
                else
                {
                    restCart.ItemCarts.Remove(itemCart);
                }

                // Update Total Price and Total Amount
                restCart.TotalPrice = restCart.ItemCarts.Sum(t => t.SubTotal);
                restCart.TotalAmount = restCart.ItemCarts.Sum(t => t.Quantity);

            }

            return RedirectToAction("GetRestaurantInfo_MenuItemInfoByID", "Restaurant", new { restaurantId = restCart.ResId });
        }

        public ActionResult OrderDetail(int restaurantId)
        {
            OrderDetail orderDetail = new OrderDetail();
            DtoRestaurantInfo dtoRestaurantInfo = GetRestaurantById(restaurantId);
            orderDetail.RestaurantId = dtoRestaurantInfo.Id;
            orderDetail.RestaurantName = dtoRestaurantInfo.RestaurantName;
            string restaurantAddress = dtoRestaurantInfo.Address + ", " + dtoRestaurantInfo.WardType + " " + dtoRestaurantInfo.WardName + ", " + dtoRestaurantInfo.DisctrictType + " " + dtoRestaurantInfo.DisctrictName + ", " + dtoRestaurantInfo.ProvinceName;
            orderDetail.RestaurantAddress = restaurantAddress;
            if(Session["UserLogin"] != null)
            {
                UserLogin userLogin = (UserLogin)Session["UserLogin"];
                orderDetail.UserId = userLogin.UserId;
                orderDetail.UserName = userLogin.UserName;
            }
            return PartialView("~/Views/RestaurantView/OrderModal.cshtml", orderDetail);
        }

        public DtoRestaurantInfo GetRestaurantById(int restaurantId)
        {
            ServiceRepository serviceObject = new ServiceRepository();
            //Get Restaurant Infomation
            HttpResponseMessage response = serviceObject.GetResponse("api/Restaurant/GetRestaurantInfoById/" + restaurantId);
            if (response.IsSuccessStatusCode)
            {
                DtoRestaurantInfo restaurantInfo = response.Content.ReadAsAsync<DtoRestaurantInfo>().Result;
                if(restaurantInfo != null)
                    return restaurantInfo;
            }
            return null;
        }

        [HttpPost]
        public ActionResult EditInfoOrder(OrderDetail orderDetail)
        {
            string fullName = Request["fullname"];
            string phone = Request["phone"];
            string address = Request["address"];
            string province = Request["province"];
            string district = Request["district"];
            string ward = Request["ward"];
            return View();
        }
    }
}