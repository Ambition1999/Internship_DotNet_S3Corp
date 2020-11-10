using Model.DTO;
using Model.Model_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.Models
{
    public class RestaurantCache
    {
        public Cache<int,DtoRestaurantInfo> ResInfoCache { get; set; }
    }
}