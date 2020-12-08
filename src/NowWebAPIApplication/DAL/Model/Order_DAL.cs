using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;

namespace DAL.Model
{
    public class Order_DAL
    {
        public Order_DAL() { }

        public int InsertOrder(Order order, List<OrderItem> orderItems)
        {
            NowFoodDBEntities dbNow = new NowFoodDBEntities();
            using (var transaction = dbNow.Database.BeginTransaction())
            {
                dbNow.Orders.Add(order);
                dbNow.SaveChanges();
                int orderId = order.Id;
                foreach (var item in orderItems)
                {
                    item.OrderId = orderId;
                    dbNow.OrderItems.Add(item);
                }
                try
                {
                    int result = dbNow.SaveChanges();
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return -2;
                }
            }
        }
    }
}
