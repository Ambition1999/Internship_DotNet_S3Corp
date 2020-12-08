using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class DtoOrder_OrderItems
    {
        public DtoOrder_OrderItems() { }
        public DtoOrder OrderInfo { get; set; }
        public List<DtoOrderItem> OrderItems { get; set; }
    }
}
