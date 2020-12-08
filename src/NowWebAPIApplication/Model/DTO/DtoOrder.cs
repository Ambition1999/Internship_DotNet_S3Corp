using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class DtoOrder
    {
        public DtoOrder() { }

        public int Id { get; set; }
        public int UserId { get; set; }
        public double SubTotal { get; set; }
        public double Discount { get; set; }
        public double Shipping { get; set; }
        public double GrandTotal { get; set; }
        public string AddressDelivery { get; set; }
        public System.DateTime OrderDate { get; set; }
        public int PaymentId { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string CreateBy { get; set; }
        public Nullable<int> DiscountId { get; set; }
        public string FullName { get; set; }
        public string PhoneContact { get; set; }


    }
}
