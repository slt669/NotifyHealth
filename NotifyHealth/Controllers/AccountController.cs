﻿using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NotifyHealth.Data_Access_Layer;
using NotifyHealth.Models.ViewModels;
using NotifyHealth.Utils;
using System;
using System.IO;
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
                Session["UserFullName"] = usermanager.accset.Forename + " " + usermanager.accset.Surname;
                Session["UserSessionId"] = usermanager.SessionId;
                Session["UserSessionGUID"] = usermanager.SessionGUID;
                Session["Photo"] = "../Content/img/Users/" + usermanager.accset.PhotoPath ?? null;
                Session["organizationID"] = usermanager.OrganizationID;
                Session["UserLogon"] = usermanager.accset.UserLogonID;
                Session["organization"] = usermanager.Organization;
                Session["Portal"] = usermanager.Portal;
                Session["Logo"] = "../Content/img/" + usermanager.Logo ?? null; 
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
                dbc.LogoutSession(Convert.ToInt32(Session["UserSessionId"]), Session["UserSessionGUID"].ToString());
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

        public ActionResult Error(string message)
        {
            ViewBag.ErrorMessage = Session["Error"];
            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Settings(string req)
        {
            Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.
            Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
            Response.AppendHeader("Expires", "0"); // Proxies.
            AccountSettingsViewModel AccountSettings = new AccountSettingsViewModel();
            AccountSettings = dbc.GetAccountDetails(Convert.ToInt32(Session["UserSessionId"]), Session["UserSessionGUID"].ToString());
            AccountSettings.HintQuestion = dbc.GetSecurityQuestions();

            if (AccountSettings.MustChangePwd == "1") ViewBag.Message = "Please set a new password";

            return View(AccountSettings);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Settings(AccountSettingsViewModel data, string updateBtn)
        {
            Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.
            Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
            Response.AppendHeader("Expires", "0"); // Proxies.
            data.HintQuestion = dbc.GetSecurityQuestions();

            if (ModelState.IsValid)
            {
                var path = "";

                if (data.PhotoFile != null)

                {
                    if (data.PhotoFile.ContentLength > 0)

                    {
                        //for checking uploaded file is image or not

                        if (Path.GetExtension(data.PhotoFile.FileName).ToLower() == ".jpg"

                            || Path.GetExtension(data.PhotoFile.FileName).ToLower() == ".png"

                          || Path.GetExtension(data.PhotoFile.FileName).ToLower() == ".gif"

                            || Path.GetExtension(data.PhotoFile.FileName).ToLower() == ".jpeg")

                        {
                            //path = "C:/inetpub/wwwroot/NotifyHealth/Content/img/Users/";

                            path = Server.MapPath("../Content/img/Users/");
                            data.PhotoFile.SaveAs(Path.Combine(path, data.PhotoFile.FileName));

                            //data.PhotoPath = "../Content/img/Users/" + data.PhotoFile.FileName;
                            data.PhotoPath = data.PhotoFile.FileName;
                        }
                    }
                }

                string UpdateMessage = dbc.ManageAccount(Convert.ToInt32(Session["UserSessionId"]), Session["UserSessionGUID"].ToString(), data);

                if (UpdateMessage != "Account Settings updated successfully!")
                {
                    ViewBag.Message = UpdateMessage;
                    TempData["UpdateMessage"] = UpdateMessage;
                    return View(data);
                }
                else
                {
                    if (data.PhotoPath != null)
                    {
                        Session["Photo"] = "../Content/img/Users/" + data.PhotoPath;
                    }
                    TempData["UpdateMessage"] = UpdateMessage;
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(data);
        }
    }
}