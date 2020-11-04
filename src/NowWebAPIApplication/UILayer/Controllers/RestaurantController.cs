using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using UILayer.Service;
using Model;

namespace UILayer.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllRestaurant()
        {
            ServiceRepository serviceOject = new ServiceRepository();
            HttpResponseMessage response = serviceOject.GetResponse("api/restaurant/getallrestaurant/");
            response.EnsureSuccessStatusCode();
            List<Restaurant_Mapper> restaurants = response.Content.ReadAsAsync<List<Restaurant_Mapper>>().Result;
            ViewBag.Title = "All Restaurant";
            return View("~/Views/RestaurantView/Restaurant.cshtml", restaurants);

        }

        public ActionResult GetRestaurantDetailByID()
        {
            return View("~/Views/RestaurantView/RestaurantDetail.cshtml");
        }


    }
}