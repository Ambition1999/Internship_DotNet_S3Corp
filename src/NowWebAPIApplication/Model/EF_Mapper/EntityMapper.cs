﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.CombineModel;
using DAL.EF;
using DAL.Model;
using Model.Model_Mapper;

namespace Model.EF_Mapper
{
    public class EntityMapper<TSource, TDestination> where TSource : class where TDestination : class
    {

        public EntityMapper()
        {
            Mapper.CreateMap<Restaurant_Mapper, Restaurant>();
            Mapper.CreateMap<Restaurant, Restaurant_Mapper>();
            //Mapper.CreateMap<Model.Model_Mapper.Restaurant_ResType_Ward_District_Province, DAL.CombineModel.Restaurant_ResType_Ward_District_Province>();
            //Mapper.CreateMap<DAL.CombineModel.Restaurant_ResType_Ward_District_Province, Model.Model_Mapper.Restaurant_ResType_Ward_District_Province>();
            Mapper.CreateMap<Model_Mapper.RestaurantInfo, DAL.CombineModel.RestaurantInfo>();
            Mapper.CreateMap<DAL.CombineModel.RestaurantInfo, Model_Mapper.RestaurantInfo>();
        }


        public TDestination Translate(TSource obj)
        {
            return Mapper.Map<TDestination>(obj);
        }
    }
}
