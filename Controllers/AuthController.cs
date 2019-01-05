using StudentApplication.Models;
using StudentApplication.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StudentApplication.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        [AllowAnonymous]
        public ActionResult Login()
        {
            LoginModel lm = new LoginModel();
            return View(lm);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel lm)
        {
            if(ModelState.IsValid)
            {
                bool result = BusinessLayer.BusinessAuth.VerifyLogin(lm.UserName, lm.Password);
                if(result==true)
                {
                    SessionFacade.LOGGEDIN = lm.UserName;
                    string roles = BusinessLayer.BusinessAuth.GetRolesForUser(lm.UserName);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, lm.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, roles);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);
                    /*string redirectUrl = FormsAuthentication.GetRedirectUrl(lm.UserName, false);
                    if (redirectUrl == "/default.aspx")
                        redirectUrl = "~/home/index";
                    Response.Redirect(redirectUrl);*/
                    if (roles == "Student")
                    {
                        return RedirectToAction("StudentDefault", "Student");
                    }
                    if(roles=="Admin")
                    {
                        return RedirectToAction("AdminDefault", "Admin");
                    }
                        //Response.Redirect("~/Student/StudentDefault");
                        
                    ViewBag.msg = "Logged in successfully....";                    
                }
                else
                    ViewBag.msg = "Logged in unsuccessfully....";
            }
            return View(lm);
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}