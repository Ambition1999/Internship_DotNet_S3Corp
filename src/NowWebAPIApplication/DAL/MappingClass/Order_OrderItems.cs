using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;

namespace DAL.MappingClass
{
    public class Order_OrderItems
    {
        public Order_OrderItems() { }
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
