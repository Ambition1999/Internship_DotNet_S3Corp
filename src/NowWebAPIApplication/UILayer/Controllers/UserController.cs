using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using UILayer.Models;
using UILayer.Service;
using log4net;
using System.Net.Mail;
using System.Text;

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

        public ActionResult ForgotPassword()
        {
            return View("~/Views/Login/ResetPassword.cshtml");
        }

        [HttpPost]
        public ActionResult ResetPassword()
        {
            string email = Request["email"];
            if(email != null && email.Trim()!= string.Empty)
            {
                ServiceRepository service = new ServiceRepository();
                HttpResponseMessage response = service.GetResponse("/api/User/CheckinEmail/" + email.Trim() + "/");
                string strResult = response.Content.ReadAsAsync<string>().Result;
                if (strResult != "")
                {
                    string temp = strResult;
                    try
                    {
                        string newPassword = CreatePassword(8);
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("chitoan2571999@gmail.com");
                            mail.To.Add(email);
                            mail.Subject = "New Password";

                            string time = DateTime.Now.ToString();
                            mail.Body = "<h3>Hi, I have receive you request to reset new password at: " + time + "</h3></br><h4>New password is: " + newPassword + "</h4>";
                            mail.IsBodyHtml = true;

                            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtpClient.Credentials = new System.Net.NetworkCredential("chitoan2571999@gmail.com", "01656727190");
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(mail);
                            }
                        }
                        HttpResponseMessage responseUpdatePassword = service.GetResponse("/api/UserAccount/UpdatePassword/" + temp + "/" + newPassword + "/");
                        bool updateResult = responseUpdatePassword.Content.ReadAsAsync<bool>().Result;
                        if (updateResult)
                        {
                            TempData["SendEmailMessage"] = "Yêu cầu tạo mới mật khẩu đã được thực hiện, vui lòng kiểm tra hộp thư email";
                            TempData["SendEmailMessageColor"] = "success";
                            return View("~/Views/Login/ResetPassword.cshtml");
                        }
                    }
                    catch (Exception)
                    {
                        TempData["SendEmailMessage"] = "Không thể kết nối với máy chủ, vui lòng thử lại sau";
                        TempData["SendEmailMessageColor"] = "danger";
                        return View("~/Views/Login/ResetPassword.cshtml");
                    }
                }
                else
                {
                    TempData["SendEmailMessage"] = "Email không chính xác, vui lòng thử lại";
                    TempData["SendEmailMessageColor"] = "danger";
                    return View("~/Views/Login/ResetPassword.cshtml");
                }
            }
            TempData["SendEmailMessage"] = "Email không phù hợp, vui lòng thử lại";
            TempData["SendEmailMessageColor"] = "danger";
            return View("~/Views/Login/ResetPassword.cshtml");
        }

        public ActionResult UserInfomation()
        {
            if (Session["UserLogin"] != null)
            {
                try
                {
                    log.Info("[START] UserController - UserInfomation");
                    UserLogin userLogin = (UserLogin)Session["UserLogin"];
                    log.Info("[START] Request API - GetUserInfoById/" + userLogin.UserId);
                    ServiceRepository service = new ServiceRepository();
                    HttpResponseMessage response = service.GetResponse("/api/User/GetUserInfoById/" + userLogin.UserId);
                    response.EnsureSuccessStatusCode();
                    UserAccountInfo userAccountInfo = new UserAccountInfo();
                    userAccountInfo.UserInfo = response.Content.ReadAsAsync<DtoUserInfo>().Result;
                    if (userAccountInfo.UserInfo != null)
                    {
                        log.Info("[END] Request API - GetUserInfoById/" + userLogin.UserId + " [Result: Success][Detail: " + response.StatusCode + "]");
                    }
                    else
                    {
                        log.Info("[END] Request API - GetUserInfoById/" + userLogin.UserId + " [Result: Failed][Detail: " + response.StatusCode + "]");
                    }
                    log.Info("[END] UserController - UserInfomation");
                    return View("~/Views/Login/AccountInfo.cshtml", userAccountInfo);
                }catch(Exception ex)
                {
                    log.Error("[ERROR] UserController - UserInfomation get error with message: ", ex);
                    throw;
                }
            }
            return View("~/Views/Login/MainPage.cshtml");
        }

        [HttpPost]
        public ActionResult UpdatUserInfo()
        {
            log.Info("[START] UserController - UpdatUserInfo");
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
                    log.Info("[START] Request API - UpdateAccount2/");
                    HttpResponseMessage response = service.PutResponse("/api/UserAccount/UpdateAccount2/" + token + "/" + userLogin.UserName + "/", userAccountInfo.AccountUpdate);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        resultAccountInt = response.Content.ReadAsAsync<int>().Result;
                        log.Info("[END] Request API - UpdateAccount2/" + " [Result: Success][Detail: " + response.StatusCode + "]");
                    }
                    else
                        log.Info("[END] Request API - UpdateAccount2/" + " [Result: Failed][Detail: " + response.StatusCode + "]");
                }
            }catch(Exception ex)
            {
                log.Error("[ERROR] UserController - UpdatUserInfo get error with message: ", ex);
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
                    log.Info("[START] Request API - UpdateUserWithToken/");
                    HttpResponseMessage response = service.PutResponse("/api/User/UpdateUserWithToken/" + token + "/" + userLogin.UserName + "/", userAccountInfo.UserInfo);
                    //HttpResponseMessage response = service.PutResponse("/api/User/UpdateUser/", userAccountInfo.UserInfo);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        resultUserInt = response.Content.ReadAsAsync<int>().Result;
                        log.Info("[END] Request API - UpdateUserWithToken/" + " [Result: Success][Detail: " + response.StatusCode + "]");
                    }
                    else
                        log.Info("[END] Request API - UpdateUserWithToken/" + " [Result: Failed][Detail: " + response.StatusCode + "]");
                }
            }catch(Exception ex)
            {
                log.Error("Error to rest Update User Info API", ex);
                //throw;
            }
            if (resultAccountInt == 1 || resultUserInt == 1)
            {
                //Response Message
                TempData["UpdateInfoMessage"] = "Cập nhật thông tin người dùng thành công";
                TempData["UpdateInfoMessageColor"] = "success";
                log.Info("Success to Update User Info" + userName);
            }
            else
            {
                TempData["UpdateInfoMessage"] = "Cập nhật thông tin người dùng thất bại";
                TempData["UpdateInfoMessageColor"] = "danger";
                log.Error("Failed to Update User Info" + userName);
            }
            log.Info("[END] UserController - UserInfomation");
            return View("~/Views/Login/AccountInfo.cshtml");
        }

        [HttpPost]
        public ActionResult RegisterAccount(DtoRegisterAccount dtoRegisterAccount)
        {
            log.Info("[START] UserController - RegisterAccount");
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
                        log.Info("[START] Request API - InsertRegisterAccount/");
                        HttpResponseMessage response = service.PostResponse("/api/useraccount/InsertRegisterAccount/", dtoRegisterAccount);
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsAsync<int>().Result;
                        if (response.IsSuccessStatusCode)
                            log.Info("[END] Request API - InsertRegisterAccount/" + " [Result: Success][Detail: " + response.StatusCode + "]");
                        else
                        {
                            log.Info("[END] Request API - InsertRegisterAccount/" + " [Result: Failed][Detail: " + response.StatusCode + "]");
                            log.Info("[END] UserController - RegisterAccount [Result: Failed][Detail: Kết nối với máy chủ thất bại]");
                            TempData["RegisterMessage"] = "Kết nối với máy chủ không thành công, vui lòng thử lại";
                            TempData["RegisterMessageColor"] = "danger";
                            return View("~/Views/Login/Login.cshtml");
                        } 
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error to rest API: Insert Register User Account", ex);
                        throw;
                    }
                    if (result >= 1)
                        {
                            log.Info("[END] UserController - RegisterAccount [Result: Success][Detail: Đăng ký thành công]");
                            TempData["RegisterMessage"] = "Đăng ký thành công";
                            TempData["RegisterMessageColor"] = "success";
                            return View("~/Views/Login/Login.cshtml");
                        }

                        else if (result == -3)
                        {
                            log.Info("[END] UserController - RegisterAccount [Result: Success][Detail: Tài khoản đã tồn tại, vui lòng thử lại]");
                            TempData["RegisterMessage"] = "Tài khoản đã tồn tại, vui lòng thử lại";
                            TempData["RegisterMessageColor"] = "warning";
                        }
                        else if (result == -2)
                        {
                            log.Info("[END] UserController - RegisterAccount [Result: Success][Detail: Lỗi Insert dữ liệu, đăng ký không thành công]");
                            TempData["RegisterMessage"] = "Lỗi dữ liệu, đăng ký không thành công";
                            TempData["RegisterMessageColor"] = "danger";

                        }
                        else
                        {
                            log.Info("[END] UserController - RegisterAccount [Result: Success][Detail: Đăng ký thất bại]");
                            TempData["RegisterMessage"] = "Đăng ký thất bại";
                            TempData["RegisterMessageColor"] = "danger";
                        }
                    return View("~/Views/Login/Register.cshtml");
                }
                log.Info("[END] UserController - RegisterAccount [Result: Success][Detail: Receive Null Paramenter]");
                return View("~/Views/MainPage/MainPage.cshtml");
            }
            log.Info("[END] UserController - RegisterAccount [Result: Success][Detail: Model Invalid]");
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

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}