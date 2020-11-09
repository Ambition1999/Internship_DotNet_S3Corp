using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.CombineModel;
using DAL.EF;
using DAL.Model;
using Model;
using Model.EF_Mapper;
using Model.Model_Mapper;

namespace BLL.BusinessLogic
{
    public class Restaurant_BLL
    {
        Restaurant_DAL res_dal = new Restaurant_DAL();
        

        public List<Restaurant_Mapper> GetAllRestaurant()
        {
            EntityMapper<Restaurant, Restaurant_Mapper> mapObj = new EntityMapper<Restaurant, Restaurant_Mapper>();
            List<Restaurant> resList = res_dal.GetAllRestaurant();
            List<Restaurant_Mapper> restaurants = new List<Restaurant_Mapper>();
            foreach (var item in resList)
            {
                restaurants.Add(mapObj.Translate(item));
            }
            return restaurants;
        }

        public DtoRestaurantInfo GetRestaurantInfoById(int restaurantId)
        {
            EntityMapper <RestaurantInfo, DtoRestaurantInfo > mapObj;
            mapObj = new EntityMapper<RestaurantInfo, DtoRestaurantInfo>();
            RestaurantInfo resInfo = res_dal.GetRestaurantInfoById(restaurantId);
            DtoRestaurantInfo restaurantInfo = mapObj.Translate(resInfo);
            return restaurantInfo;
        }

        public List<DtoRestaurantInfo> GetRestaurantInfoByListId(List<int> listId)
        {
            EntityMapper<RestaurantInfo, DtoRestaurantInfo> mapObj;
            mapObj = new EntityMapper<RestaurantInfo, DtoRestaurantInfo>();
            List<RestaurantInfo> resInfo = res_dal.GetAllRestaurantInfoByListId(listId);
            List<DtoRestaurantInfo> restaurantInfos = new List<DtoRestaurantInfo>();
            foreach (var item in resInfo)
            {
                restaurantInfos.Add(mapObj.Translate(item));
            }    
            return restaurantInfos;
        }

        public List<DtoRestaurantInfo> GetRestaurantInfoByKindId(int kindId)
        {
            EntityMapper<RestaurantInfo, DtoRestaurantInfo> mapObj;
            mapObj = new EntityMapper<RestaurantInfo, DtoRestaurantInfo>();
            List<RestaurantInfo> resInfoList = res_dal.GetRestaurantInfoByKindId(kindId);
            List<DtoRestaurantInfo> restaurantInfos = new List<DtoRestaurantInfo>();
            foreach (var item in resInfoList)
            {
                restaurantInfos.Add(mapObj.Translate(item));
            }
            return restaurantInfos;
        }

        public List<DtoRestaurantInfo> GetAllRestaurantInfo()
        {
            EntityMapper<RestaurantInfo, DtoRestaurantInfo> mapObj;
            mapObj = new EntityMapper<RestaurantInfo, DtoRestaurantInfo>();
            List<RestaurantInfo> resInfoList = res_dal.GetAllRestaurantInfo();
            List<DtoRestaurantInfo> restaurantInfos = new List<DtoRestaurantInfo>();
            foreach (var item in resInfoList)
            {
                restaurantInfos.Add(mapObj.Translate(item));
            }
            return restaurantInfos;
        }

        public List<DtoRestaurantInfo> GetAllRestaurantInfoByName(string name)
        {
            EntityMapper<RestaurantInfo, DtoRestaurantInfo> mapObj;
            mapObj = new EntityMapper<RestaurantInfo, DtoRestaurantInfo>();
            List<RestaurantInfo> resInfoList = res_dal.GetRestaurantInfosByName(name);
            List<DtoRestaurantInfo> restaurantInfos = new List<DtoRestaurantInfo>();
            foreach (var item in resInfoList)
            {
                restaurantInfos.Add(mapObj.Translate(item));
            }
            return restaurantInfos;
        }

    }
}
