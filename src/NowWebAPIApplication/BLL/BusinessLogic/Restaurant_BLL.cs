using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //public List<RestaurantInfo> GetRestaurantInfo()
        //{
        //    EntityMapper<DAL.CombineModel.RestaurantInfo, Model.Model_Mapper.RestaurantInfo> mapObj;
        //    mapObj = new EntityMapper<DAL.CombineModel.RestaurantInfo, RestaurantInfo>();
        //    List<DAL.CombineModel.RestaurantInfo> resInfoList = res_dal.GetAllRestaurantInfo();
        //    List<Co>
        //}

    }
}
