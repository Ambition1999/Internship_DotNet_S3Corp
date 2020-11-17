using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using UILayer.Models;
using UILayer.Service;

namespace UILayer.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginAdmin()
        {
            return View("~/Views/Admin/LoginAdmin.cshtml");
        }

        public ActionResult LogoutAdmin()
        {
            Session.Abandon();
            return View("~/Views/Admin/LoginAdmin.cshtml");
        }

        [HttpPost]
        public ActionResult LoginProcessing(UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                //Success
                ServiceRepository service = new ServiceRepository();
                HttpResponseMessage responseCheckAccount = service.GetResponse("/api/useraccount/CheckAdminAccount/" + userAccount.UserName + "/" + userAccount.Password);
                responseCheckAccount.EnsureSuccessStatusCode();
                if (responseCheckAccount.Content.ReadAsAsync<BoolResult>().Result.Result)
                {
                    HttpResponseMessage response = service.GetResponse("/api/Employee/GetEmployeeInfoByUserName/" + userAccount.UserName);
                    response.EnsureSuccessStatusCode();
                    DtoEmployeeInfo dtoEmpInfo = response.Content.ReadAsAsync<DtoEmployeeInfo>().Result;
                    UserLogin userLogin = new UserLogin();
                    userLogin.UserName = dtoEmpInfo.UserName;
                    userLogin.UserId = dtoEmpInfo.Id;
                    Session["AdminLogin"] = userLogin;
                    return RedirectToAction("UserManagement", "Admin");
                }
                else
                    return Redirect("/");
            }
            else
            {
                return Redirect("/");
            }
        }

        public ActionResult AdminLoad()
        {
            return View();
        }

        public ActionResult UserManagement()
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage response = service.GetResponse("/api/User/GetAllUserInfo/");
            response.EnsureSuccessStatusCode();
            List<DtoUserInfo> userInfos = new List<DtoUserInfo>();
            userInfos = response.Content.ReadAsAsync<List<DtoUserInfo>>().Result;
            return View("~/Views/Admin/UserManagement.cshtml", userInfos);
        }
        
    }
}