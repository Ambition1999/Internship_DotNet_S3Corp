using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.CombineModel;
using DAL.DtoClass;
using DAL.EF;
using DAL.Model;
using Model.DTO;
using Model.Model_Mapper;

namespace Model.EF_Mapper
{
    public class EntityMapper<TSource, TDestination> where TSource : class where TDestination : class
    {

        public EntityMapper()
        {
            Mapper.CreateMap<Restaurant_Mapper, Restaurant>();
            Mapper.CreateMap<Restaurant, Restaurant_Mapper>();
            
            Mapper.CreateMap<DtoRestaurantInfo, DAL.CombineModel.RestaurantInfo>();
            Mapper.CreateMap<DAL.CombineModel.RestaurantInfo, DtoRestaurantInfo>();
            
            Mapper.CreateMap<DtoMenuItemInfo, MenuItemInfo>();
            Mapper.CreateMap<MenuItemInfo, DtoMenuItemInfo>();

            Mapper.CreateMap<DtoUserInfo, UserInfo>();
            Mapper.CreateMap<UserInfo, DtoUserInfo>();
        }


        public TDestination Translate(TSource obj)
        {
            return Mapper.Map<TDestination>(obj);
        }
    }
}
