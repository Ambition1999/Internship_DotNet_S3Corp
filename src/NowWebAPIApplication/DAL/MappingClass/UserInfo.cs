using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class UserInfo
    {
        public UserInfo() { }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int Status { get; set; }
        public string RoleName { get; set; }
        public System.DateTime RegisterAt { get; set; }

    }
}
