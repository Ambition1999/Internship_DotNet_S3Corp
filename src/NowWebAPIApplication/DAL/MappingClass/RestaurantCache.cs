using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.MappingClass
{
    public class RestaurantCache
    {
        public int Id { get; set; }
        public List<String> RestaurantDetails { get; set; }

        public RestaurantCache() { }


    }
}
