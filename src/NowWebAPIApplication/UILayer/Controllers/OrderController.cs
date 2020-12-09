using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Model.DTO;
using UILayer.Models;
using UILayer.Service;

namespace UILayer.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public string SaveDeliveryContact(string fullname, string phone, string address)
        {
            DeliveryInfo deliveryInfo = new DeliveryInfo();
            deliveryInfo.Fullname = fullname;
            deliveryInfo.Phone = phone;
            deliveryInfo.Address = address;
            Session["DeliveryInfo"] = deliveryInfo;
            return "Add delivery contact success";
        }

        public string InsertOrder(int resId)
        {
            DtoOrder_OrderItems order_OrderItems = new DtoOrder_OrderItems();
            RestaurantCart restaurantCart = (RestaurantCart)Session[resId.ToString()];
            //Define and add item in cart to order list
            List<DtoOrderItem> orderItems = new List<DtoOrderItem>();
            foreach (var item in restaurantCart.ItemCarts)
            {
                DtoOrderItem orderItem = new DtoOrderItem();
                orderItem.ItemId = item.ItemId;
                orderItem.Price = item.Price;
                orderItem.Quantity = item.Quantity;
                orderItem.Total = item.SubTotal;
                orderItem.Unit = 1;
                orderItem.Discount = 0;
                orderItem.CreateAt = DateTime.Now;
                orderItems.Add(orderItem);
            }

            order_OrderItems.OrderItems = orderItems;

            //Create and define order infomation
            UserLogin userLogin = (UserLogin)Session["UserLogin"];
            DeliveryInfo deliveryInfo = (DeliveryInfo)Session["DeliveryInfo"];
            DtoOrder dtoOrder = new DtoOrder();
            dtoOrder.UserId = userLogin.UserId;
            dtoOrder.SubTotal = restaurantCart.TotalPrice;
            dtoOrder.Shipping = 0;
            dtoOrder.GrandTotal = restaurantCart.TotalPrice;
            dtoOrder.PaymentId = 2;
            dtoOrder.Discount = 0;
            dtoOrder.DiscountId = null;
            dtoOrder.AddressDelivery = deliveryInfo.Address;
            dtoOrder.FullName = deliveryInfo.Fullname;
            dtoOrder.PhoneContact = deliveryInfo.Phone;
            dtoOrder.OrderDate = DateTime.Now;
            dtoOrder.CreateBy = "user";
            dtoOrder.CreateTime = DateTime.Now;

            order_OrderItems.OrderInfo = dtoOrder;

            ServiceRepository serviceObject = new ServiceRepository();
            HttpResponseMessage response = serviceObject.PostResponse("/api/Order/InsertOrder/", order_OrderItems);
            if (response.IsSuccessStatusCode)
            {
                int result = response.Content.ReadAsAsync<int>().Result;
                if(result>1)
                {
                    Session[resId.ToString()] = null;
                    //return RedirectToAction("GetRestaurantInfo_MenuItemInfoByID", "Restaurant", new { restaurantId = resId });
                    return "Đặt hàng thành công!!!";
                }    
                else
                    return "Đặt hàng thất bại, vui lòng thử lại.";
            }
            else
            {
                return "Mất kết nối với máy chủ, vui lòng thử lại.";
            }
        }
    }
}