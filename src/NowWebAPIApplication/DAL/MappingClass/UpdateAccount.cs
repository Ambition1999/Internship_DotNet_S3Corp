using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.MappingClass
{
    public class UpdateAccount
    {
        public String UserName { get; set; }
        public String Password { get; set; }
        public String NewPassword { get; set; }
    }
}
