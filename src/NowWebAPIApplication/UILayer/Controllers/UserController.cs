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
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login()
        {
            return View("~/Views/Login/Login.cshtml");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return View("~/Views/MainPage/MainPage.cshtml");
        }

        public ActionResult Register()
        {
            return View("~/Views/Login/Register.cshtml");
        }

        [HttpPost]
        public ActionResult RegisterAccount(DtoRegisterAccount dtoRegisterAccount)
        {
            if (ModelState.IsValid)
            {
                if (dtoRegisterAccount != null)
                {
                    dtoRegisterAccount.CreateDay = DateTime.Now;
                    dtoRegisterAccount.CreateBy = "User";

                    ServiceRepository service = new ServiceRepository();
                    HttpResponseMessage response = service.PostResponse("/api/useraccount/InsertRegisterAccount/",dtoRegisterAccount);
                    response.EnsureSuccessStatusCode();
                    int result = response.Content.ReadAsAsync<int>().Result;
                    if (result == 1)
                    {
                        TempData["RegisterMessage"] = "Đăng ký thành công";
                        return View("~/Views/Login/Login.cshtml");
                    }
                       
                    else if(result == 0)
                    {
                        TempData["RegisterMessage"] = "Tài khoản đã tồn tại, vui lòng thử lại";
                        TempData["RegisterMessageHTML"] = "<script>alert('Tài khoản đã tồn tại, vui lòng thử lại');</script>";
                    }
                    else
                    {
                        TempData["RegisterMessage"] = "Đăng ký thất bại";
                        TempData["RegisterMessageHTML"] = "<script>alert('Đăng ký thất bại, vui lòng thử lại');</script>";
                    }       

                    return View("~/Views/Login/Register.cshtml");
                }
                return View("~/Views/MainPage/MainPage.cshtml");
            }
            return View("~/Views/MainPage/MainPage.cshtml");
        }

        [HttpPost]
        public ActionResult LoginAccount(UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                //Success
                ServiceRepository service = new ServiceRepository();
                HttpResponseMessage responseCheckAccount = service.GetResponse("/api/useraccount/CheckUserAccount/" + userAccount.UserName + "/" + userAccount.Password);
                responseCheckAccount.EnsureSuccessStatusCode();
                if (responseCheckAccount.Content.ReadAsAsync<BoolResult>().Result.Result)
                {
                    HttpResponseMessage response = service.GetResponse("/api/user/getuserinfobyusername/"+userAccount.UserName);
                    response.EnsureSuccessStatusCode();
                    DtoUserInfo dtoUserInfo = response.Content.ReadAsAsync<DtoUserInfo>().Result;
                    UserLogin userLogin = new UserLogin();
                    userLogin.UserName = dtoUserInfo.UserName;
                    userLogin.UserId = dtoUserInfo.UserId;
                    Session["UserLogin"] = userLogin;
                    return View("~/Views/MainPage/MainPage.cshtml");
                }
                else 
                    return Redirect("/");     
            }
            else
            {
                return Redirect("/");
            }
        }
    }
}