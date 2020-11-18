using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CombineModel
{
    public class RestaurantInfo
    {
        public int Id { get; set; }
        public string RestaurantName { get; set; }
        public string Address { get; set; }
        public int WardId { get; set; }
        public string OpenTime { get; set; }
        public short Status { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public int TypeId { get; set; }
        public string Image { get; set; }
        public string AddressDB { get; set; }
        public string WardName { get; set; }
        public string WardType { get; set; }
        public string DisctrictName { get; set; }
        public string DisctrictType { get; set; }
        public string ProvinceName { get; set; }
        public string RestaurantTypeName { get; set; }
    }
}
