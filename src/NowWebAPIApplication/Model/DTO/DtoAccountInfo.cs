using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class DtoAccountInfo
    {
        public DtoAccountInfo() { }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int Status { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string CreateBy { get; set; }
    }
}
