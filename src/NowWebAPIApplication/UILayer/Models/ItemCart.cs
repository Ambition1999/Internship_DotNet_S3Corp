using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.Models
{
    public class ItemCart
    {
        public ItemCart() { }

        public ItemCart(int itemId, string itemName, int quantity, double price) 
        {
            this.ItemId = itemId;
            this.ItemName = itemName;
            this.Quantity = quantity;
            this.Price = price;
            this.SubTotal = quantity * price;
        }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double SubTotal { get; set; }
    }
}