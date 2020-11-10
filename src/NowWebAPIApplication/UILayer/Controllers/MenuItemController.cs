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
            response.EnsureSuccessStatusCode();
            DtoMenuItemInfo menuItemInfos = response.Content.ReadAsAsync<DtoMenuItemInfo>().Result;
            return menuItemInfos;
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

                //return ("~/Views/RestaurantView/RestaurantCart.cshtml", restCart);
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

                //return ("~/Views/RestaurantView/RestaurantCart.cshtml", restCart);
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
    }
}