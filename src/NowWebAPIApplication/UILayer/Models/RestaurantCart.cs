using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.Models
{
    public class RestaurantCart
    {
        public int ResId { get; set; }
        public List<ItemCart> ItemCarts { get; set; }
        public double TotalPrice { get; set; }
        public int TotalAmount { get; set; }
    }
}