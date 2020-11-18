using DAL.EF;
using DAL.Model;
using Model.DTO;
using Model.EF_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BusinessLogic
{
    public class Ward_BLL
    {
        Ward_DAL ward_DAL = new Ward_DAL();
        public Ward_BLL() { }

        public List<DtoWard> GetWardByDistrictId(int districtId)
        {
            EntityMapper<Ward, DtoWard> mapObj = new EntityMapper<Ward, DtoWard>();
            List<Ward> wards = ward_DAL.GetWardByDistricId(districtId);
            List<DtoWard> dtoWards = new List<DtoWard>();
            foreach (var item in wards)
            {
                dtoWards.Add(mapObj.Translate(item));
            }
            return dtoWards;
        }
    }
}
