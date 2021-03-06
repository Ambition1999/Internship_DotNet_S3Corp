using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DtoClass
{
    public class MenuItemInfo
    {   
        public MenuItemInfo()
        {

        }

        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public double Quantity { get; set; }
        public string Image { get; set; }
        public short Status { get; set; }
        public int RestaurantId { get; set; }
        public double BasePrice { get; set; }

    }
}
