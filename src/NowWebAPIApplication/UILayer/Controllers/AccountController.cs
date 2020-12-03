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
            return View("~/Views/Login/ChangePassword.cshtml",model);
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
            if (model.Email != null)
            {
                string password = Request["new-password"];
                string resetToken = Request["reset-token"];

                int resetResponse = ChangePassword(model.Username, password, resetToken);
                if (resetResponse == 1)
                {
                    ViewBag.Message = "Successfully Changed";
                }
                else
                {
                    ViewBag.Message = "Something went horribly wrong!";
                }


            }
            return View(model);
        }

        public int ChangePassword(string userName, string password, string token)
        {
            //log.Info("-- Call PUT API UpdatePassword");
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseUpdatePassword = service.GetResponse("/api/UserAccount/UpdatePasswordToken/" + userName + "/" + password + "/" + token +"/");
            return responseUpdatePassword.Content.ReadAsAsync<int>().Result;
        }
    }
}