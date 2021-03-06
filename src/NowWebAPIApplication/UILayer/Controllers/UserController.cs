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
            return Redirect(this.Request.UrlReferrer.ToString());
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