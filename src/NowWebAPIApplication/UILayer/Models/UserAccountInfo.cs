using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UILayer.Models
{
    public class UserAccountInfo
    {
        public DtoUserInfo UserInfo { get; set; }
        public DtoUpdateAccount AccountUpdate { get; set; }
    }
}