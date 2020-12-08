using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLL.BusinessLogic;
using Model.DTO;

namespace APILayer.Controllers
{
    [RoutePrefix("api/Order")]
    public class OrderController : ApiController
    {
        public OrderController() { }

        Order_BLL Order_BLL = new Order_BLL();

        [HttpPost]
        [Route("InsertOrder")]
        public int InsertOrder(DtoOrder_OrderItems dtoOrder_OrderItems)
        {
            return Order_BLL.InsertOrder(dtoOrder_OrderItems);
        }
    }
}
