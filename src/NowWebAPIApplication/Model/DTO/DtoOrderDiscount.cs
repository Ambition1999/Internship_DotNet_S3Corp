using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class DtoOrderDiscount
    {
        public DtoOrderDiscount() { }
        public int Id { get; set; }
        public string DiscountName { get; set; }
        public System.DateTime CreateDay { get; set; }
        public System.DateTime ValidFrom { get; set; }
        public System.DateTime ValidUtil { get; set; }
        public string CouponCode { get; set; }
        public double MinimumDiscountValue { get; set; }
        public double MaximumDiscountValue { get; set; }
    }
}
