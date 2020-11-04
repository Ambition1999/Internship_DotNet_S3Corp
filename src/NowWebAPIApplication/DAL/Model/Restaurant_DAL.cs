using DAL.CombineModel;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Restaurant_DAL
    {
        NowFoodDBEntities db = new NowFoodDBEntities();
        public Restaurant_DAL() { }

        public List<Restaurant> GetAllRestaurant()
        {
            //return db.Restaurants.Select(t=>t).ToList();
            var restaurants = db.Restaurants.Include(t => t.RestaurantType).ToList();
            //foreach (var item in restaurants)
            //{
            //    item.Image = "C:\\Users\\toan.le\\source\\repos\\APILayer\\UILayer\\Content\\Image\\ImgRestaurant\\" + item.Image;
            //}
            return restaurants;
        }

        public List<RestaurantInfo> GetAllRestaurantInfo()
        {
            var restaurantInfo = from res_type in db.RestaurantTypes
                                 join res in db.Restaurants on res_type.Id equals res.TypeId
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
                                     CreateTime = res.CreateTime,
                                     TypeId = res_type.Id,
                                     Image = res.Image,
                                     AddressDB = ward.Name + ", " + district.Name + ", " + province.Name,
                                     RestaurantTypeName = res_type.KindRestaurant
                                 };
            return restaurantInfo.ToList();
        }
    }
}
