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
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ResetPasswordEmail2(string UserName, string Email, string Code)
        {
            ResetPasswordModel model = new ResetPasswordModel();
            model.ResetToken = Code;
            model.Email = Email;
            model.Username = UserName;
            TempData[Code] = UserName;
            return View("~/Views/Login/ChangePassword.cshtml", model);
        }

        public ActionResult ResetPassword2()
        {
            ResetPasswordModel model = new ResetPasswordModel();
            model.ResetToken = "abc";
            return View("~/Views/Login/ChangePassword.cshtml", model);
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            string password = Request["new-password"].Trim();
            string resetToken = Request["reset-token"].Trim();
            if(TempData[resetToken] != null)
            {
                string username = TempData[resetToken].ToString();
                int resetResponse = ChangePassword(username, password, resetToken);
                if (resetResponse == 1)
                {
                    TempData["SendEmailForgotPasswordMessage"] = "Thay đổi mật khẩu thành công";
                    TempData["SendEmailForgotPasswordMessageColor"] = "success";
                    return View("~/Views/Login/ChangePassword.cshtml");
                }
                else
                {
                    TempData["SendEmailForgotPasswordMessage"] = "Đổi mật khẩu thất bại";
                    TempData["SendEmailForgotPasswordMessageColor"] = "danger";
                    return View("~/Views/Login/ChangePassword.cshtml");
                }
            }
            return View("~/Views/Login/ChangePassword.cshtml");
        } 

        public int ChangePassword(string userName, string newpassword, string token)
        {
            //log.Info("-- Call PUT API UpdatePassword");
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseUpdatePassword = service.GetResponse("/api/UserAccount/UpdatePasswordToken/" + userName + "/" + newpassword + "/" + token +"/");
            return responseUpdatePassword.Content.ReadAsAsync<int>().Result;
        }
    }
}