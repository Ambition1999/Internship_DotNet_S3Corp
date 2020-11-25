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

        public string userName;
        // GET: User
        public ActionResult Login()
        {
            return View("~/Views/Login/Login.cshtml");
        }

        private string checkLoginCookie()
        {
            if(Request.Cookies.Get("username") != null)
            {
                return Request.Cookies["username"].Value;
            }
            return string.Empty;
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login","User");
        }

        public ActionResult Register()
        {
            return View("~/Views/Login/Register.cshtml");
        }

        public ActionResult UserInfomation()
        {
            if (Session["UserLogin"] != null)
            {
                UserLogin userLogin = (UserLogin)Session["UserLogin"];

                ServiceRepository service = new ServiceRepository();
                HttpResponseMessage response = service.GetResponse("/api/User/GetUserInfoById/" + userLogin.UserId);
                response.EnsureSuccessStatusCode();
                UserAccountInfo userAccountInfo = new UserAccountInfo();
                userAccountInfo.UserInfo = response.Content.ReadAsAsync<DtoUserInfo>().Result;
                userName = userAccountInfo.UserInfo.UserName;
                return View("~/Views/Login/AccountInfo.cshtml", userAccountInfo);
            }
            return View("~/Views/Login/MainPage.cshtml");
        }

        [HttpPost]
        public ActionResult UpdatUserInfo()
        {
            string oldPass = Request["old-password"];
            string newPass = Request["re-password"];
            string email = Request["email"];
            var gender = Request["gender"];
            int genderVal = Convert.ToInt32(gender);
            bool resultAccount= false, resultUser = false;
            UserAccountInfo userAccountInfo = new UserAccountInfo();
            UserLogin userLogin = (UserLogin)Session["UserLogin"];

            if (newPass != null && oldPass != null && oldPass != "" && userLogin != null)
            {
                userAccountInfo.AccountUpdate = new DtoUpdateAccount();
                userAccountInfo.AccountUpdate.UserName = userLogin.UserName;
                userAccountInfo.AccountUpdate.Password = oldPass;
                userAccountInfo.AccountUpdate.NewPassword = newPass;
                //Call API Update
                ServiceRepository service = new ServiceRepository();
                HttpResponseMessage response = service.PutResponse("/api/UserAccount/UpdateAccount/", userAccountInfo.AccountUpdate);
                response.EnsureSuccessStatusCode();
                resultAccount = response.Content.ReadAsAsync<bool>().Result;
            }     

            if (email != null || email != "" || genderVal != 0)
            {
                userAccountInfo.UserInfo = new DtoUserInfo();
                if (email != null)
                {
                    userAccountInfo.UserInfo.UserId = userLogin.UserId;
                    userAccountInfo.UserInfo.Email = email;
                }
                if (genderVal != 0)
                {
                    userAccountInfo.UserInfo.UserId = userLogin.UserId;
                    if(genderVal == 1)
                        userAccountInfo.UserInfo.Gender = "Nam";
                    else
                        userAccountInfo.UserInfo.Gender = "Nữ";
                }
                //Call API update
                ServiceRepository service = new ServiceRepository();
                HttpResponseMessage response = service.PutResponse("/api/User/UpdateUser/", userAccountInfo.UserInfo);
                response.EnsureSuccessStatusCode();
                resultUser = response.Content.ReadAsAsync<bool>().Result;
            }
            if (resultAccount || resultUser)
                return RedirectToAction("UserInfomation", "User");
            return View("~/Views/Login/MainPage.cshtml");
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
                    if (result >= 1)
                    {
                        
                        TempData["RegisterMessage"] = "Đăng ký thành công";
                        TempData["RegisterMessageColor"] = "success";
                        return View("~/Views/Login/Login.cshtml");
                    }
                       
                    else if(result == -3)
                    {
                        TempData["RegisterMessage"] = "Tài khoản đã tồn tại, vui lòng thử lại";
                        TempData["RegisterMessageColor"] = "warning";
                        
                    }
                    else if (result == -2)
                    {
                        TempData["RegisterMessage"] = "Lỗi Insert dữ liệu, đăng ký không thành công, dữ liệu đã đc rollback";
                        TempData["RegisterMessageColor"] = "danger";
                       
                    }
                    else
                    {
                        TempData["RegisterMessage"] = "Đăng ký thất bại";
                        TempData["RegisterMessageColor"] = "danger";
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
                    HttpCookie cookie = new HttpCookie("token", GetToken());
                    Response.Cookies.Add(cookie);
                    return View("~/Views/MainPage/MainPage.cshtml");
                }
                else
                {
                    TempData["UserLoginMessage"] = "Tài khoản hoặc mật khẩu không chính xác, vui lòng thử lại";
                    TempData["UserLoginMessageColor"] = "danger";
                    return View("~/Views/Login/Login.cshtml");
                }                       
            }
            else
            {
                TempData["UserLoginMessage"] = "Dữ liệu không hợp lệ, vui lòng thử lại";
                TempData["UserLoginMessageColor"] = "danger";
                return View("~/Views/Login/Login.cshtml");
            }
        }

        public string GetToken()
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseCheckAccount = service.GetResponse("/api/JWT/GetToken/");
            responseCheckAccount.EnsureSuccessStatusCode();

            return responseCheckAccount.Content.ReadAsAsync<string>().Result;
        }
    }
}