using Model.Model_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using UILayer.Service;

namespace UILayer.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult MainPage()
        {
            return RedirectToAction("LoadRestaurantCache", "Restaurant");
            
        }
    }
}