using Model.Model_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.Models
{
    public class RestaurantDetail
    {
        public DtoRestaurantInfo RestaurantInfo { get; set; }
        public List<ItemType> ItemTypes { get; set; }
    }
}