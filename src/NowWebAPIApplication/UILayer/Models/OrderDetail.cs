using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.Models
{
    public class OrderDetail
    {
        public int RestaurantId { get; set; }
        public string RestaurantAddress { get; set; }
        public string RestaurantName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string AddressDelivery { get; set; }
        public TimeSpan TimeDelivery { get; set; }
        public DateTime DateDelivery { get; set; }
        public string Distance { get; set; } 
    }
}