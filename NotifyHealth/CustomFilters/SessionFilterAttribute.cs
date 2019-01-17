using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NotifyHealth.Data_Access_Layer;

namespace NotifyHealth.CustomFilters
{
    public class SessionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
  
            NotifyHealthDB db = new NotifyHealthDB();
            RouteValueDictionary logoffDict = new RouteValueDictionary();

            filterContext.HttpContext.Session["timestamp"] = DateTime.Now.ToString("yyyy/MM/dd H:mm:ss");


            logoffDict.Add("action", "Logoff");
            logoffDict.Add("controller", "Account");
            logoffDict.Add("area", "");


            RouteValueDictionary settingsDict = new RouteValueDictionary();
            settingsDict.Add("action", "Settings");
            settingsDict.Add("controller", "Account");
            settingsDict.Add("area", "");

            string RawUrl = filterContext.HttpContext.Request.RawUrl;

            string ActionToCheck = filterContext.ActionDescriptor.ActionName;
            if (RawUrl.Contains("QuoteRemake")) ActionToCheck = "New";


            if (filterContext.HttpContext.Session["UserSessionId"] == null || filterContext.HttpContext.Session["UserSessionGUID"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(logoffDict);

            }
            else
            {

                db.CheckSession(Convert.ToInt32(filterContext.HttpContext.Session["UserSessionId"]), filterContext.HttpContext.Session["UserSessionGUID"].ToString(), ActionToCheck);
                if (db.ReturnValidationError != "0" && db.ReturnValidationError != "10105")
                {
                    logoffDict.Add("errormessage", db.ReturnValidationMessage);
                    filterContext.Result = new RedirectToRouteResult(logoffDict);
                }
                if (db.MustChangePwd == "1" || db.ReturnValidationError == "10105")
                {
                    filterContext.Result = new RedirectToRouteResult(settingsDict);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
