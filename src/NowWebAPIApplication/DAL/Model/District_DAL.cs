using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class District_DAL
    {
        public District_DAL() { }

        public List<District> GetDistrictByProvinceId(int provinceId)
        {
            NowFoodDBEntities db = new NowFoodDBEntities();
            var districts = db.Districts.Where(t => t.ProvinceId == provinceId).ToList();
            return districts;
        }
    }
}
