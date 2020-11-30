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

        public static List<Cache<int, DtoRestaurantInfo>> CacheRestaurants;

        public static List<Dictionary<int, DtoRestaurantInfo>> RestaurantsCache;

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

        public static List<int> SearchList(List<Dictionary<int,DtoRestaurantInfo>> restaurantsCache, string searchValue)
        {
            List<int> listId = new List<int>();
            List<Dictionary<int, DtoRestaurantInfo>> listRestaurants = restaurantsCache;

            foreach (Dictionary<int,DtoRestaurantInfo> item in listRestaurants)
            {
                var temp = item.Values.FirstOrDefault(t => t.RestaurantName.ToLower().Contains(searchValue.ToLower()) == true
                                                        || t.AddressDB.ToLower().Contains(searchValue.ToLower()) == true);
                if (temp != null)
                    listId.Add(temp.Id);
            }
            return listId;
        }

        public static void LoadAllRestaurantToCache()
        {
            ServiceRepository serviceObject = new ServiceRepository();
            HttpResponseMessage response = serviceObject.GetResponse("api/restaurant/getallrestaurant/");
            response.EnsureSuccessStatusCode();
            List<DtoRestaurantInfo> restaurants = response.Content.ReadAsAsync<List<DtoRestaurantInfo>>().Result;
            RestaurantsCache = LoadRestaurantInfoToCache(restaurants);
        }

        public static void LoadAllRestaurantToCache(List<DtoRestaurantInfo> restaurantInfos)
        {
            RestaurantsCache = LoadRestaurantInfoToCache(restaurantInfos);
        }

        public static List<Dictionary<int, DtoRestaurantInfo>> LoadRestaurantInfoToCache(List<DtoRestaurantInfo> restaurantInfos)
        {
            RestaurantsCache = new List<Dictionary<int, DtoRestaurantInfo>>();
            Dictionary<int, DtoRestaurantInfo> keyValuePairs;
            foreach (DtoRestaurantInfo item in restaurantInfos)
            {
                keyValuePairs = new Dictionary<int, DtoRestaurantInfo>();
                keyValuePairs.Add(item.Id, item);
                RestaurantsCache.Add(keyValuePairs);
            }
            return RestaurantsCache;
        }

        //public static void LoadAllRestaurantsToCache()
        //{
        //    ServiceRepository serviceObject = new ServiceRepository();
        //    HttpResponseMessage response = serviceObject.GetResponse("api/restaurant/getallrestaurant/");
        //    response.EnsureSuccessStatusCode();
        //    List<DtoRestaurantInfo> restaurants = response.Content.ReadAsAsync<List<DtoRestaurantInfo>>().Result;
        //    CacheRestaurants = LoadRestaurantInfoToCaches(restaurants);
        //}



        //public static void LoadAllRestaurantsToCache(List<DtoRestaurantInfo> restaurantInfos)
        //{
        //    CacheRestaurants = LoadRestaurantInfoToCaches(restaurantInfos);
        //}

        //public static Cache<int, DtoRestaurantInfo> LoadRestaurantInfoToCache(List<DtoRestaurantInfo> restaurantInfos)
        //{
        //    CacheRestaurant = new UILayer.Cache<int, DtoRestaurantInfo>();
        //    foreach (DtoRestaurantInfo item in restaurantInfos)
        //    {
        //        CacheRestaurant.Store(item.Id, item, TimeSpan.FromMinutes(30));
        //    }
        //    return CacheRestaurant;
        //}

        //public static List<Cache<int, DtoRestaurantInfo>> LoadRestaurantInfoToCaches(List<DtoRestaurantInfo> restaurantInfos)
        //{
        //    CacheRestaurants = new List<Cache<int, DtoRestaurantInfo>>();
        //    foreach (DtoRestaurantInfo item in restaurantInfos)
        //    {
        //        Cache<int, DtoRestaurantInfo> CacheRestaurantTemp = new Cache<int, DtoRestaurantInfo>();
        //        CacheRestaurantTemp.Store(item.Id, item, TimeSpan.FromMinutes(30));
        //        CacheRestaurants.Add(CacheRestaurantTemp);
        //    }
        //    return CacheRestaurants;
        //}


    }
}