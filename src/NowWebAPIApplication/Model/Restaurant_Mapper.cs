using Model.Model_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Restaurant_Mapper
    {
        public Restaurant_Mapper() { }

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

        //public virtual ICollection<MenuItem> MenuItems { get; set; }
        //public virtual Ward Ward { get; set; }
        //public virtual RestaurantType_Mapper RestaurantType { get; set; }

    }
}
