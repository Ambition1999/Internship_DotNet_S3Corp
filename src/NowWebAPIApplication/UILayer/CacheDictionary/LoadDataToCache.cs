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

        public static Cache<int, DtoRestaurantInfo> CacheRestaurant;

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


        public static void LoadAllRestaurantToCache()
        {
            ServiceRepository serviceObject = new ServiceRepository();
            HttpResponseMessage response = serviceObject.GetResponse("api/restaurant/getallrestaurant/");
            response.EnsureSuccessStatusCode();
            List<DtoRestaurantInfo> restaurants = response.Content.ReadAsAsync<List<DtoRestaurantInfo>>().Result;
            CacheRestaurant = LoadRestaurantInfoToCache(restaurants);
        }

        public static Cache<int, DtoRestaurantInfo> LoadRestaurantInfoToCache(List<DtoRestaurantInfo> restaurantInfos)
        {
            CacheRestaurant = new UILayer.Cache<int, DtoRestaurantInfo>();
            foreach (DtoRestaurantInfo item in restaurantInfos)
            {
                CacheRestaurant.Store(item.Id, item, TimeSpan.FromMinutes(30));
            }
            return CacheRestaurant;
        }
    }
}