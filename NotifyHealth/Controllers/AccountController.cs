using NotifyHealth.Data_Access_Layer;
using NotifyHealth.Models.ViewModels;
using NotifyHealth.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NotifyHealth.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private NotifyHealthDB dbc = new NotifyHealthDB();
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string error, string returnUrl)
        {
            TempData["UpdateMessage"] = error;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            Session["AnalyticsCode"] = System.Configuration.ConfigurationManager.AppSettings["AnalyticsCode"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserManager usermanager = new UserManager();

            if (usermanager.IsValid(model.Email, model.Password))
            {
                var ident = new ClaimsIdentity(
                  new[] { new Claim(ClaimTypes.Email, model.Email) },
                  DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication.SignIn(
                   new AuthenticationProperties { IsPersistent = model.RememberMe }, ident);
                Session["timestamp"] = DateTime.Now.ToString("yyyy/MM/dd H:mm:ss");  //2018-01-02
                //Session["menu"] = usermanager.ar;
                //Session["DataAccessRights"] = usermanager.dar;
                //Session["UserName"] = model.Email;
                //Session["UserFullName"] = usermanager.accset.Forename + " " + usermanager.accset.Surname;
                //Session["UserSessionId"] = usermanager.SessionId;
                //Session["UserSessionGUID"] = usermanager.SessionGUID;
                //Session["UserTenantList"] = usermanager.ltn;
                //Session["UserTenant"] = usermanager.TenantId;
                //Session["UserLogonId"] = usermanager.accset.UserLogonID;
                //Session["UserRoleId"] = usermanager.accset.UserRole;
                //Session["UserList"] = usermanager.usr;
                //Session["CompanyName"] = usermanager.accset.CompanyName;
                //Session["CompanyID"] = usermanager.accset.CompanyId;
                //Session["AdminLogonId"] = "";
                //Session["CustomerType"] = usermanager.accset.CustomerType;
                //Session["AdminFlag"] = false;
                //if (returnUrl == "/") returnUrl = "/Home/Index";

                return RedirectToAction("Index", "Home"); // auth succeed 
            }
            // invalid username or password
            ModelState.AddModelError("", usermanager.strReturnValidationMessage);
            return View();


        }


        public ActionResult Logoff(string errormessage)
        {
            HttpContext.GetOwinContext().Authentication.SignOut();

            try
            {
                //dbc.LogoutSession(Convert.ToInt32(Session["UserSessionId"]), Session["UserSessionGUID"].ToString());
                TempData["UpdateMessage"] = errormessage;

            }
            catch (Exception ex)
            {
                var LoginError = ex.Message;
            }


            FormsAuthentication.SignOut();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Account", new { error = errormessage });

        }

    }
}