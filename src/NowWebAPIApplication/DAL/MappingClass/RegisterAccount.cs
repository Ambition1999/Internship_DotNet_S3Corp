using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.MappingClass
{
    public class RegisterAccount
    {
        public RegisterAccount() { }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime CreateDay { get; set; }
        public string CreateBy { get; set; }
    }
}
