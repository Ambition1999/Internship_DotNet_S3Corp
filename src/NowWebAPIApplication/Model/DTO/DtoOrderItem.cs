using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class DtoOrderItem
    {
        public DtoOrderItem() { }

        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double Quantity { get; set; }
        public short Unit { get; set; }
        public double Total { get; set; }
        public System.DateTime CreateAt { get; set; }
    }
}
