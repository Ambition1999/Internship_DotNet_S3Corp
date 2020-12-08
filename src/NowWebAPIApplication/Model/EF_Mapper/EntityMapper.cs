using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.CombineModel;
using DAL.DtoClass;
using DAL.EF;
using DAL.MappingClass;
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

            Mapper.CreateMap<DtoRegisterAccount, RegisterAccount>();
            Mapper.CreateMap<RegisterAccount, DtoRegisterAccount>();

            Mapper.CreateMap<DtoUpdateAccount, UpdateAccount>();
            Mapper.CreateMap<UpdateAccount, DtoUpdateAccount>();

            Mapper.CreateMap<DtoEmployeeInfo, EmployeeInfo>();
            Mapper.CreateMap<EmployeeInfo, DtoEmployeeInfo>();

            Mapper.CreateMap<DtoWard, Ward>();
            Mapper.CreateMap<Ward, DtoWard>();

            Mapper.CreateMap<DtoDistrict, District>();
            Mapper.CreateMap<District, DtoDistrict>();

            Mapper.CreateMap<DtoProvince, Province>();
            Mapper.CreateMap<Province, DtoProvince>();

            Mapper.CreateMap<DtoAccountInfo, AccountInfo>();
            Mapper.CreateMap<AccountInfo, DtoAccountInfo>();

            Mapper.CreateMap<DtoOrder_OrderItems, Order_OrderItems>();
            Mapper.CreateMap<Order_OrderItems, DtoOrder_OrderItems>();

            Mapper.CreateMap<DtoOrder, Order>();
            Mapper.CreateMap<Order, DtoOrder>();

            Mapper.CreateMap<DtoOrderItem, OrderItem>();
            Mapper.CreateMap<OrderItem, DtoOrderItem>();

        }


        public TDestination Translate(TSource obj)
        {
            return Mapper.Map<TDestination>(obj);
        }
    }
}
