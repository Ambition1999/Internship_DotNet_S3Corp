using Model.Model_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using UILayer.Service;

namespace UILayer.CacheDictionary
{
    public class LoadDataToCache
    {
        public static Cache<int, string> Cache;

        public LoadDataToCache() { }

        public static void LoadRestaurantToCache()
        {
            ServiceRepository serviceObject = new ServiceRepository();
            HttpResponseMessage response = serviceObject.GetResponse("api/restaurant/getallrestaurant/");
            response.EnsureSuccessStatusCode();
            List<DtoRestaurantInfo> restaurants = response.Content.ReadAsAsync<List<DtoRestaurantInfo>>().Result;
            Cache = LoadRestaurantInfoToSearchCache(restaurants);
        }

        public static Cache<int, string> LoadRestaurantInfoToSearchCache(List<DtoRestaurantInfo> restaurantInfos)
        {
            Cache = new UILayer.Cache<int, string>();
            foreach (var item in restaurantInfos)
            {
                Cache.Store(item.Id, item.RestaurantName + ", " + item.RestaurantTypeName + ", " + item.WardName, TimeSpan.FromMinutes(30));
            }
            return Cache;
        }
    }
}