using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Ward_DAL
    {
        public Ward_DAL() { }

        public List<Ward> GetWardByDistricId(int districtId)
        {
            NowFoodDBEntities db = new NowFoodDBEntities();
            var wards = db.Wards.Where(t => t.DistrictID == districtId).ToList();
            return wards;
        }      
    }
}
