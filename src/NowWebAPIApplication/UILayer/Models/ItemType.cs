using Model.Model_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace UILayer.Models
{
    public class ItemType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public List<DtoMenuItemInfo> MenuItemInfos { get; set; }

    }
}