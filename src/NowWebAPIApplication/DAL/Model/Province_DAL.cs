using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Province_DAL
    {
        public Province_DAL() { }

        public List<Province> GetAllProvince()
        {
            NowFoodDBEntities db = new NowFoodDBEntities();
            var provinces = db.Provinces.Select(t => t).ToList();
            return provinces;
        }
    }
}
