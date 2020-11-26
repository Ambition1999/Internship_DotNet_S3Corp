﻿using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using UILayer.Models;
using UILayer.Service;
using log4net;

namespace UILayer.Controllers
{
    public class UserController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                try
                {
                    UserLogin userLogin = (UserLogin)Session["UserLogin"];

                    ServiceRepository service = new ServiceRepository();
                    HttpResponseMessage response = service.GetResponse("/api/User/GetUserInfoById/" + userLogin.UserId);
                    response.EnsureSuccessStatusCode();
                    UserAccountInfo userAccountInfo = new UserAccountInfo();
                    userAccountInfo.UserInfo = response.Content.ReadAsAsync<DtoUserInfo>().Result;
                    userName = userAccountInfo.UserInfo.UserName;
                    //Write message to log
                    log.Info("Success to res API: GetUserInfoById");
                    return View("~/Views/Login/AccountInfo.cshtml", userAccountInfo);
                }catch(Exception ex)
                {
                    log.Error("Error to rest API: GetUserInfoById ", ex);
                    throw;
                }
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
            //bool resultAccount= false, resultUser = false;
            int resultAccountInt = -1, resultUserInt = -1;
            UserLogin userLogin;

            UserAccountInfo userAccountInfo = new UserAccountInfo();
            //Try catch to track Session user login
            try
            {
                userLogin = (UserLogin)Session["UserLogin"];
            }catch(Exception ex)
            {
                log.Error("Error to get Session User",ex);
                throw;
            }
            try
            {
                if (newPass != null && oldPass != null && oldPass != "" && userLogin != null)
                {
                    userAccountInfo.AccountUpdate = new DtoUpdateAccount();
                    userAccountInfo.AccountUpdate.UserName = userLogin.UserName;
                    userAccountInfo.AccountUpdate.Password = oldPass;
                    userAccountInfo.AccountUpdate.NewPassword = newPass;
                    HttpCookie cookie = HttpContext.Request.Cookies.Get("token");
                    string token = cookie.Value.ToString();

                    //Call API Update Account
                    ServiceRepository service = new ServiceRepository();
                    //HttpResponseMessage response = service.PutResponse("/api/UserAccount/UpdateAccount/", userAccountInfo.AccountUpdate);
                    HttpResponseMessage response = service.PutResponse("/api/UserAccount/UpdateAccount2/" + token + "/" + userLogin.UserName + "/", userAccountInfo.AccountUpdate);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                        resultAccountInt = response.Content.ReadAsAsync<int>().Result;
                }
            }catch(Exception ex)
            {
                log.Error("Error to rest Update User Account API", ex);
                throw;
            }

            try
            {
                if (email != null && email != "" || genderVal != 0)
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
                        if (genderVal == 1)
                            userAccountInfo.UserInfo.Gender = "Nam";
                        else
                            userAccountInfo.UserInfo.Gender = "Nữ";
                    }
                    HttpCookie cookie = HttpContext.Request.Cookies.Get("token");
                    string token = cookie.Value.ToString();
                    //Call API update User
                    ServiceRepository service = new ServiceRepository();
                    HttpResponseMessage response = service.PutResponse("/api/User/UpdateUserWithTokenk/" + token + "/" + userLogin.UserName + "/", userAccountInfo.UserInfo);
                    //HttpResponseMessage response = service.PutResponse("/api/User/UpdateUser/", userAccountInfo.UserInfo);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                        resultUserInt = response.Content.ReadAsAsync<int>().Result;
                }
            }catch(Exception ex)
            {
                log.Error("Error to rest Update User Info API", ex);
                //throw;
            }
                if (resultAccountInt == 1 || resultUserInt == 1)
                {
                    TempData["UpdateInfoMessage"] = "Cập nhật thông tin người dùng thành công";
                    TempData["UpdateInfoMessageColor"] = "success";
                log.Info("Success to Update User Info" + userName);
                }
                else
                {
                    TempData["UpdateInfoMessage"] = "Cập nhật thông tin người dùng thất bại";
                    TempData["UpdateInfoMessageColor"] = "danger";
                log.Error("Fail to Update User Info" + userName);
                }
                return View("~/Views/Login/AccountInfo.cshtml");
        }

        [HttpPost]
        public ActionResult RegisterAccount(DtoRegisterAccount dtoRegisterAccount)
        {
            if (ModelState.IsValid)
            {
                if (dtoRegisterAccount != null)
                {
                    int result = -1;
                    try
                    {
                        dtoRegisterAccount.CreateDay = DateTime.Now;
                        dtoRegisterAccount.CreateBy = "User";

                        ServiceRepository service = new ServiceRepository();
                        HttpResponseMessage response = service.PostResponse("/api/useraccount/InsertRegisterAccount/", dtoRegisterAccount);
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsAsync<int>().Result;
                        log.Info("Success to rest API: Insert Register User Account");
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error to rest API: Insert Register User Account", ex);
                        throw;
                    }
                    if (result >= 1)
                        {

                            TempData["RegisterMessage"] = "Đăng ký thành công";
                            TempData["RegisterMessageColor"] = "success";
                            return View("~/Views/Login/Login.cshtml");
                        }

                        else if (result == -3)
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
                BoolResult result = new BoolResult();
                ServiceRepository service = new ServiceRepository();
                try
                {
                    //Success
                    HttpResponseMessage responseCheckAccount = service.GetResponse("/api/useraccount/CheckUserAccount/" + userAccount.UserName + "/" + userAccount.Password);
                    responseCheckAccount.EnsureSuccessStatusCode();
                    log.Info("Success to rest API: Check Username");
                    result.Result = responseCheckAccount.Content.ReadAsAsync<BoolResult>().Result.Result;
                    log.Info("Success to rest API: Check User Account");
                }
                catch(Exception ex)
                {
                    log.Error("Error to rest API: Check Username", ex);
                    throw;
                }
                if (result.Result)
                {
                    DtoUserInfo dtoUserInfo;
                    try
                    {
                        HttpResponseMessage response = service.GetResponse("/api/user/getuserinfobyusername/" + userAccount.UserName);
                        response.EnsureSuccessStatusCode();
                        dtoUserInfo = response.Content.ReadAsAsync<DtoUserInfo>().Result;
                        UserLogin userLogin = new UserLogin();
                        userLogin.UserName = dtoUserInfo.UserName;
                        userLogin.UserId = dtoUserInfo.UserId;
                        Session["UserLogin"] = userLogin;
                        string token = GetToken(userLogin.UserName);
                        HttpCookie cookie = new HttpCookie("token");
                        HttpContext.Response.Cookies.Remove("token");
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.Value = token;
                        HttpContext.Response.SetCookie(cookie);
                        return View("~/Views/MainPage/MainPage.cshtml");
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error to rest API: Get UserInfo by Username", ex);
                        throw;
                    }
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

        public string GetToken(string username)
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseCheckAccount = service.GetResponse("/api/JWT/GetToken/"+username);
            responseCheckAccount.EnsureSuccessStatusCode();

            return responseCheckAccount.Content.ReadAsAsync<string>().Result;
        }
    }
}