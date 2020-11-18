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
    public class District_BLL
    {
        District_DAL district_DAL = new District_DAL();
        public District_BLL() { }
        public List<DtoDistrict> GetDistrictByProvinceId(int provinceId)
        {
            EntityMapper<District, DtoDistrict> mapObj = new EntityMapper<District, DtoDistrict>();
            List<District> districts = district_DAL.GetDistrictByProvinceId(provinceId);
            List<DtoDistrict> dtoDistricts = new List<DtoDistrict>();
            foreach (var item in districts)
            {
                dtoDistricts.Add(mapObj.Translate(item));
            }
            return dtoDistricts;
        }
    }
}
