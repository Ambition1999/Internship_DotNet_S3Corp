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
    public class Province_BLL
    {
        Province_DAL province_DAL = new Province_DAL();
        public Province_BLL() { }
        public List<DtoProvince> GetAllProvince()
        {
            EntityMapper<Province, DtoProvince> mapObj = new EntityMapper<Province, DtoProvince>();
            List<Province> provinces = province_DAL.GetAllProvince();
            List<DtoProvince> dtoProvinces = new List<DtoProvince>();
            foreach (var item in provinces)
            {
                dtoProvinces.Add(mapObj.Translate(item));
            }
            return dtoProvinces;
        }

    }
}
