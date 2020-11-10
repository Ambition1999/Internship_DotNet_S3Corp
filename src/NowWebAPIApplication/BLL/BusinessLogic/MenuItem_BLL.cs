using DAL.DtoClass;
using DAL.Model;
using Model.EF_Mapper;
using Model.Model_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BusinessLogic
{
    public class MenuItem_BLL
    {
        MenuItem_DAL menuItem_BLL = new MenuItem_DAL();
        public MenuItem_BLL() { }

        public List<DtoMenuItemInfo> GetMenuRestaurantInfoByID(int RestaurantId)
        {
            EntityMapper<MenuItemInfo, DtoMenuItemInfo> mapObject = new EntityMapper<MenuItemInfo, DtoMenuItemInfo>();
            List<MenuItemInfo> menuItemInfoList = menuItem_BLL.GetMenuRestaurantInfoByID(RestaurantId);
            List<DtoMenuItemInfo> menuItemInfos = new List<DtoMenuItemInfo>();
            foreach (var item in menuItemInfoList)
            {
                menuItemInfos.Add(mapObject.Translate(item));
            }
            return menuItemInfos;
        }

        public DtoMenuItemInfo GetDtoMenuItemInfoByItemId(int itemId)
        {
            EntityMapper<MenuItemInfo, DtoMenuItemInfo> mapObject = new EntityMapper<MenuItemInfo, DtoMenuItemInfo>();
            MenuItemInfo menuItemInfo = menuItem_BLL.GetMenuRestaurantInfoByItemID(itemId);
            DtoMenuItemInfo dtoMenuItemInfo = mapObject.Translate(menuItemInfo);
            return dtoMenuItemInfo;
        }
    }
}
