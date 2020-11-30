using DAL.CombineModel;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Restaurant_DAL
    {
        NowFoodDBEntities db = new NowFoodDBEntities();
        public Restaurant_DAL() { }

        public List<Restaurant> GetAllRestaurant()
        {
            var restaurants = db.Restaurants.Include(t => t.RestaurantType).ToList();
            return restaurants;
        }

        public RestaurantInfo GetRestaurantInfoById(int restaurantId)
        {
            var restaurantInfo = from res_type in db.RestaurantTypes
                                 join res in db.Restaurants.Where(t=>t.Id == restaurantId && t.Status == 1) on res_type.Id equals res.TypeId
                                 join ward in db.Wards on res.WardId equals ward.Id
                                 join district in db.Districts on ward.DistrictID equals district.Id
                                 join province in db.Provinces on district.ProvinceId equals province.Id
                                 select new RestaurantInfo
                                 {
                                     Id = res.Id,
                                     RestaurantName = res.RestaurantName,
                                     Address = res.Address,
                                     WardId = ward.Id,
                                     OpenTime = res.OpenTime,
                                     Status = res.Status,
                                     TypeId = res_type.Id,
                                     Image = res.Image,
                                     AddressDB = res.Address + ", " + ward.Type + " " + ward.Name
                                                             + ", " + district.Type + " " + district.Name + ", "
                                                             + province.Name,
                                     WardName = ward.Name,
                                     WardType = ward.Type,
                                     DisctrictName = district.Name,
                                     DisctrictType = district.Type,
                                     ProvinceName = province.Name,
                                     RestaurantTypeName = res_type.KindRestaurant
                                 };
            return restaurantInfo.FirstOrDefault();
        }

        public List<RestaurantInfo> GetAllRestaurantInfo()
        {
            var restaurantInfo = from res_type in db.RestaurantTypes
                                 join res in db.Restaurants.Where(t => t.Status == (Int16)1) on res_type.Id equals res.TypeId
                                 join ward in db.Wards on res.WardId equals ward.Id
                                 join district in db.Districts on ward.DistrictID equals district.Id
                                 join province in db.Provinces on district.ProvinceId equals province.Id
                                 select new RestaurantInfo
                                 {
                                     Id = res.Id,
                                     RestaurantName = res.RestaurantName,
                                     Address = res.Address,
                                     WardId = ward.Id,
                                     OpenTime = res.OpenTime,
                                     Status = res.Status,
                                     TypeId = res_type.Id,
                                     Image = res.Image,
                                     AddressDB = res.Address + ", " + ward.Type + " " + ward.Name 
                                                             + ", " + district.Type + " " + district.Name + ", " 
                                                             + province.Name,
                                     WardName = ward.Name,
                                     WardType = ward.Type,
                                     DisctrictName = district.Name,
                                     DisctrictType = district.Type,
                                     ProvinceName = province.Name,
                                     RestaurantTypeName = res_type.KindRestaurant
                                 };
            return restaurantInfo.ToList();
        }

        public List<int> GetListRestaurantIdByKindId(int kindId)
        {
            var restaurantId = from res in db.Restaurants.Where(t => t.Status == (Int16)1)
                               join res_item in db.MenuItems.Where(t => t.Status == (Int16)1) on res.Id equals res_item.RestaurantId
                               join kind in db.KindItems.Where(t => t.Id == kindId) on res_item.KindId equals kind.Id
                               select (res.Id);
            return restaurantId.Distinct().ToList();
        }

        public List<RestaurantInfo> GetAllRestaurantInfoByListId(List<int> listId)
        {
            List<RestaurantInfo> restaurantInfos = new List<RestaurantInfo>();
            foreach (var item in listId)
            {
                restaurantInfos.Add(GetRestaurantInfoById(item));
            }
            return restaurantInfos;    
        }



        public List<RestaurantInfo> GetRestaurantInfoByKindId(int kindId)
        {
            var listRestaurantId = GetListRestaurantIdByKindId(kindId);
            return GetAllRestaurantInfoByListId(listRestaurantId);
        }

        public List<RestaurantInfo> GetRestaurantInfosByName(string name)
        {
            var restaurantInfo = from res_type in db.RestaurantTypes
                                 join res in db.Restaurants.Where(t => t.Status == (Int16)1 && t.RestaurantName.Contains(name)) on res_type.Id equals res.TypeId
                                 join ward in db.Wards on res.WardId equals ward.Id
                                 join district in db.Districts on ward.DistrictID equals district.Id
                                 join province in db.Provinces on district.ProvinceId equals province.Id
                                 select new RestaurantInfo
                                 {
                                     Id = res.Id,
                                     RestaurantName = res.RestaurantName,
                                     Address = res.Address,
                                     WardId = ward.Id,
                                     OpenTime = res.OpenTime,
                                     Status = res.Status,
                                     TypeId = res_type.Id,
                                     Image = res.Image,
                                     AddressDB = res.Address + ", " + ward.Type + " " + ward.Name
                                                             + ", " + district.Type + " " + district.Name + ", "
                                                             + province.Name,
                                     WardName = ward.Name,
                                     WardType = ward.Type,
                                     DisctrictName = district.Name,
                                     DisctrictType = district.Type,
                                     ProvinceName = province.Name,
                                     RestaurantTypeName = res_type.KindRestaurant
                                 };
            return restaurantInfo.ToList();
        }

       
    }
}
