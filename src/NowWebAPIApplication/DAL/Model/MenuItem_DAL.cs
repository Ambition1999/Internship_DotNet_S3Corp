using DAL.DtoClass;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class MenuItem_DAL
    {
        NowFoodDBEntities db = new NowFoodDBEntities();
        public MenuItem_DAL() { }

        public List<MenuItemInfo> GetMenuRestaurantInfoByID(int RestaurantId)
        {
            var menuItem = from mnItem in db.MenuItems.Where(t => t.RestaurantId == RestaurantId)
                           join iType in db.ItemTypes on mnItem.TypeId equals iType.Id
                           join uType in db.ItemUnits on mnItem.UnitId equals uType.Id
                           from price in db.ItemPrices.Where(x => mnItem.Id == x.ItemId && x.Status == 1)
                           select new MenuItemInfo
                           {
                               Id = mnItem.Id,
                               ItemName = mnItem.ItemName,
                               Description = mnItem.Description,
                               TypeId = mnItem.TypeId,
                               TypeName = iType.TypeName,
                               UnitId = mnItem.UnitId,
                               UnitName = uType.UnitName,
                               Quantity = mnItem.Quantity,
                               Image = mnItem.Image,
                               Status = mnItem.Status,
                               RestaurantId = mnItem.RestaurantId,
                               BasePrice = price.BasePrice
                           };
            return menuItem.ToList();
        }

        public MenuItemInfo GetMenuRestaurantInfoByItemID(int itemId)
        {
            var menuItem = from mnItem in db.MenuItems.Where(t => t.Id == itemId && t.Status == 1)
                           join iType in db.ItemTypes on mnItem.TypeId equals iType.Id
                           join uType in db.ItemUnits on mnItem.UnitId equals uType.Id
                           from price in db.ItemPrices.Where(x => mnItem.Id == x.ItemId && x.Status == 1)
                           select new MenuItemInfo
                           {
                               Id = mnItem.Id,
                               ItemName = mnItem.ItemName,
                               Description = mnItem.Description,
                               TypeId = mnItem.TypeId,
                               TypeName = iType.TypeName,
                               UnitId = mnItem.UnitId,
                               UnitName = uType.UnitName,
                               Quantity = mnItem.Quantity,
                               Image = mnItem.Image,
                               Status = mnItem.Status,
                               RestaurantId = mnItem.RestaurantId,
                               BasePrice = price.BasePrice
                           };
            return menuItem.FirstOrDefault();
        }
    }
}
