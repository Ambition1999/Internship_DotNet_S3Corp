using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using Model.DTO;
using DAL.MappingClass;
using Model.EF_Mapper;
using DAL.EF;

namespace BLL.BusinessLogic
{
    public class Order_BLL
    {
        Order_DAL Order_DAL = new Order_DAL();
        public Order_BLL() { }

        //public int InsertOrder(DtoOrder dtoOrder, List<DtoOrderItem> dtoOrderItems)
        //{
        //    EntityMapper<DtoOrder, Order> mapObjOrder = new EntityMapper<DtoOrder, Order>();
        //    Order order = mapObjOrder.Translate(dtoOrder);

        //    EntityMapper<DtoOrderItem, OrderItem> mapObjOrderItem = new EntityMapper<DtoOrderItem, OrderItem>();
        //    List<OrderItem> orderItems = new List<OrderItem>();

        //    foreach (var item in dtoOrderItems)
        //    {
        //        orderItems.Add(mapObjOrderItem.Translate(item));
        //    }

        //    return Order_DAL.InsertOrder(order, orderItems);
        //}

        public int InsertOrder(DtoOrder_OrderItems dtoOrder_OrderItems)
        {
            EntityMapper<DtoOrder, Order> mapObjOrder = new EntityMapper<DtoOrder, Order>();
            Order order = mapObjOrder.Translate(dtoOrder_OrderItems.OrderInfo);

            EntityMapper<DtoOrderItem, OrderItem> mapObjOrderItem = new EntityMapper<DtoOrderItem, OrderItem>();
            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (var item in dtoOrder_OrderItems.OrderItems)
            {
                orderItems.Add(mapObjOrderItem.Translate(item));
            }

            return Order_DAL.InsertOrder(order, orderItems);
        }
    }
}
