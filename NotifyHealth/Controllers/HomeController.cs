﻿using NotifyHealth.CustomFilters;
using NotifyHealth.Data_Access_Layer;
using NotifyHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace NotifyHealth.Controllers
{
    public class HomeController : Controller
    {
        private NotifyHealthDB db = new NotifyHealthDB();
        public ActionResult Index(int? organizationID)
        {
            ViewBag.organizationID = Session["organizationID"];
            return View();
        }
        /// <summary>
        /// Programs
        /// </summary>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        [SessionFilterAttribute]
        public ActionResult Programs(int? organizationID)
        {


            try
            {

                ViewBag.organizationID = Session["organizationID"];
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View();
        }

        public List<Programs> MyGlobalProgramsInitializer()
        {
            List<Programs> dtsource = new List<Programs>();
            dtsource = (List<Programs>)TempData["Programsdtsource"];

            TempData["Programsdtsource"] = dtsource;
            return dtsource;
        }


        [SessionFilterAttribute]
        public JsonResult GetPrograms(DTParameters param, int? organizationID)
        {
            List<Programs> dtsource = new List<Programs>();


            dtsource = db.GetPrograms(Convert.ToInt32(Session["organizationID"]));

            TempData["Programsdtsource"] = dtsource;
            TempData["organizationID"] = organizationID;


            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            if (param.Length == -1)
            {
                param.Length = dtsource.Count();
            }
            List<Programs> data = new ResultSet().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, dtsource, columnSearch);
            int count = new ResultSet().Count(param.Search.Value, dtsource, columnSearch);
            DTResult<Programs> result = new DTResult<Programs>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return Json(result);
        }


        [SessionFilterAttribute]
        public ActionResult CreateProgram(int? organizationID)
        {
            var model = new Programs();
            //string id = RouteData.Values["userID"].ToString();
            List<Programs> dtsource = MyGlobalProgramsInitializer();
            model.Statuses = GetStatusList();
            model.OrganizationID = Convert.ToInt32(Session["organizationID"]);
            return View("_CreateProgramsPartial", model);
        }


        [SessionFilterAttribute]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreatePrograms(Programs model)
        {
            //if (!ModelState.IsValid)
            //{

            //    return View("_CreateProgramsPartial", model);
            //}
            ViewBag.Message = "Sucess or Failure Message";
            ModelState.Clear();
            List<Programs> dtsource = MyGlobalProgramsInitializer();

            char delete = 'N';
            db.UpdatePrograms(Convert.ToInt32(Session["organizationID"]), model.Description, model.Name, model.ProgramId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);

            //if (task.Exception != null)
            //{
            //    ModelState.AddModelError("", "Unable to add the Asset");
            //    return View("_CreatePartial", model);
            //}

            return RedirectToAction("Programs", new { controller = "Home", organizationID = model.OrganizationID });

        }
        // GET: Asset/Edit/5
        [SessionFilterAttribute]
        public ActionResult EditProgram(int? id)
        {

            List<Programs> dtsource = MyGlobalProgramsInitializer();
            Programs edit = dtsource.FirstOrDefault(x => x.ProgramId == id);
            edit.Statuses = GetStatusList();
            ViewBag.organizationID = Session["organizationID"];
            ViewBag.ProgramId = id;


            if (Request.IsAjaxRequest())
                return PartialView("_EditProgramsPartial", edit);
            return View(edit);
        }
        [SessionFilterAttribute]
        public ActionResult ProgramDetails(int? id)
        {


            List<Programs> dtsource = MyGlobalProgramsInitializer();
            Programs edit = dtsource.FirstOrDefault(x => x.ProgramId == id);
            edit.Statuses = GetStatusList();
            ViewBag.organizationID = Session["organizationID"];
            ViewBag.ProgramId = id;

            return View("ProgramDetails", edit);
        }

        // POST: Asset/Edit/5
        [SessionFilterAttribute]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditPrograms(Programs model)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    ViewBag.Message = "Failure";
                //   return RedirectToAction("Programs", new { organizationID = model.OrganizationID });

                //}

                ModelState.Clear();
                List<Programs> dtsource = MyGlobalProgramsInitializer();
                char delete = 'N';
                db.UpdatePrograms(Convert.ToInt32(Session["organizationID"]), model.Description, model.Name, model.ProgramId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;

            }


            //if (task.Exception != null)
            //{
            //    ModelState.AddModelError("", "Unable to update the Asset");
            //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    return View(Request.IsAjaxRequest() ? "_EditPartial" : "Edit", assetVM);
            //}

            //if (Request.IsAjaxRequest())
            //{
            //    return Content("success");
            //}
            return RedirectToAction("Programs", new { organizationID = model.OrganizationID });
            //return RedirectToAction("SpeedDial", new { userID = model.VoiceUserID });

        }
        [SessionFilterAttribute]
        public ActionResult DeleteProgram(int? id)
        {
            var testID = 4;
            List<Programs> dtsource = MyGlobalProgramsInitializer();

            Programs delete = dtsource.FirstOrDefault(x => x.ProgramId == id);




            if (Request.IsAjaxRequest())
                return PartialView("_DeleteProgramsPartial", delete);
            return View(delete);

        }

     
        [SessionFilterAttribute]
        [HttpPost]
        public ActionResult DeletePrograms(Programs model)
        {
            try
            {
                List<Programs> dtsource = MyGlobalProgramsInitializer();

                char delete = 'Y';
                db.UpdatePrograms(Convert.ToInt32(Session["organizationID"]), model.Description, model.Name, model.ProgramId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            //var task = DbContext.SaveChangesAsync();
            //await task;

            //if (task.Exception != null)
            //{
            //    ModelState.AddModelError("", "Unable to Delete the Asset");
            //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    AssetViewModel assetVM = MapToViewModel(asset);
            //    return View(Request.IsAjaxRequest() ? "_DeletePartial" : "Delete", assetVM);
            //}

            //if (Request.IsAjaxRequest())
            //{
            //    return Content("success");
            //}

            return RedirectToAction("Programs", new { organizationID = model.OrganizationID });

        }
        public JsonResult GetCampaignsbyProgram(DTParameters param, int? organizationID, int? ProgramId)
        {
            List<Campaigns> dtsource = new List<Campaigns>();


            dtsource = db.GetCampaignsbyProgram(Convert.ToInt32(Session["organizationID"]), ProgramId);

            TempData["CampaignsbyProgramdtsource"] = dtsource;
            TempData["organizationID"] = Convert.ToInt32(Session["organizationID"]);


            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            if (param.Length == -1)
            {
                param.Length = dtsource.Count();
            }
            List<Campaigns> data = new ResultSet().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, dtsource, columnSearch);
            int count = new ResultSet().Count(param.Search.Value, dtsource, columnSearch);
            DTResult<Campaigns> result = new DTResult<Campaigns>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return Json(result);
        }
        public JsonResult GetNotificationsbyCampaigns(DTParameters param, int? organizationID, int? CampaignID)
        {
            List<Notifications> dtsource = new List<Notifications>();


            dtsource = db.GetNotificationsbyCampaigns(Convert.ToInt32(Session["organizationID"]), CampaignID);

            TempData["NotificationsbyCampaignsdtsource"] = dtsource;
            TempData["organizationID"] = Convert.ToInt32(Session["organizationID"]);


            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            if (param.Length == -1)
            {
                param.Length = dtsource.Count();
            }
            List<Notifications> data = new ResultSet().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, dtsource, columnSearch);
            int count = new ResultSet().Count(param.Search.Value, dtsource, columnSearch);
            DTResult<Notifications> result = new DTResult<Notifications>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return Json(result);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<Campaigns> MyGlobalCampaignsInitializer()
        {
            List<Campaigns> dtsource = new List<Campaigns>();
            dtsource = (List<Campaigns>)TempData["Campaignsdtsource"];

            TempData["Campaignsdtsource"] = dtsource;
            return dtsource;
        }
        /// <summary>
        /// Campaigns
        /// </summary>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        [SessionFilterAttribute]
        public ActionResult Campaigns(int? organizationID)
        {


            try
            {

                ViewBag.organizationID = 1;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View();
        }

        public JsonResult GetCampaigns(DTParameters param, int? organizationID)
        {
            List<Campaigns> dtsource = new List<Campaigns>();


            dtsource = db.GetCampaigns(Convert.ToInt32(Session["organizationID"]));

            TempData["Campaignsdtsource"] = dtsource;
            TempData["organizationID"] = organizationID;


            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            if (param.Length == -1)
            {
                param.Length = dtsource.Count();
            }
            List<Campaigns> data = new ResultSet().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, dtsource, columnSearch);
            int count = new ResultSet().Count(param.Search.Value, dtsource, columnSearch);
            DTResult<Campaigns> result = new DTResult<Campaigns>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return Json(result);
        }
        [SessionFilterAttribute]
        public ActionResult CreateCampaign(int? organizationID)
        {
            var model = new Campaigns();

            List<Campaigns> dtsource = MyGlobalCampaignsInitializer();
            model.Programs = db.GetProgramDDL();
            model.Statuses = GetStatusList();
            model.OrganizationID = organizationID;
            return View("_CreateCampaignsPartial", model);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateCampaigns(Campaigns model)
        {
            //if (!ModelState.IsValid)
            //{

            //    return View("_CampaignsModal", model);
            //}
            //ViewBag.Message = "Sucess or Failure Message";
            ModelState.Clear();
            List<Campaigns> dtsource = MyGlobalCampaignsInitializer();

            char delete = 'N';
            db.UpdateCampaigns(Convert.ToInt32(Session["organizationID"]), model.Description, model.Name,model.CampaignId, model.ProgramId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]),model.StatusId,delete);



            return RedirectToAction("Campaigns", new { controller = "Home", campaignId = model.CampaignId });

        }
        [SessionFilterAttribute]
        public ActionResult CampaignDetails(int? id)
        {


            List<Campaigns> dtsource = MyGlobalCampaignsInitializer();
            Campaigns edit = dtsource.FirstOrDefault(x => x.CampaignId == id);
            edit.Statuses = GetStatusList();
            ViewBag.organizationID = Session["organizationID"];
            ViewBag.CampaignId = id;

            return View("CampaignDetails", edit);
        }
        // GET: Asset/Edit/5
        [SessionFilterAttribute]
        public ActionResult EditCampaign(int? id)
        {
            var testID = 1;

            List<Campaigns> dtsource = MyGlobalCampaignsInitializer();
            Campaigns edit = dtsource.FirstOrDefault(x => x.CampaignId == id);
            edit.Programs = db.GetProgramDDL();
            edit.Statuses = GetStatusList();



            if (Request.IsAjaxRequest())
                return PartialView("_EditCampaignsPartial", edit);
            return View(edit);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditCampaigns(Campaigns model)
        {
            try
            {
            //    if (!ModelState.IsValid)
            //    {

            //        return View("_EditCampaignsPartial", model);
            //    }
            //    ViewBag.Message = "Sucess or Failure Message";
            //    ModelState.Clear();
                List<Campaigns> dtsource = MyGlobalCampaignsInitializer();
                char delete = 'N';
                db.UpdateCampaigns(Convert.ToInt32(Session["organizationID"]), model.Description, model.Name, model.CampaignId, model.ProgramId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;

            }
            return RedirectToAction("Campaigns", new { controller = "Home", campaignId = model.CampaignId });

        }
        [SessionFilterAttribute]
        public ActionResult DeleteCampaign(int? id)
        {
            var testID = 4;
            List<Campaigns> dtsource = MyGlobalCampaignsInitializer();

            Campaigns delete = dtsource.FirstOrDefault(x => x.CampaignId == id);
            if (Request.IsAjaxRequest())
                return PartialView("_DeleteCampaignsPartial", delete);
            return View(delete);

        }

        [HttpPost]
        public ActionResult DeleteCampaigns(Campaigns model)
        {
            try
            {
                List<Campaigns> dtsource = MyGlobalCampaignsInitializer();

                char delete = 'Y';
                db.UpdateCampaigns(Convert.ToInt32(Session["organizationID"]), model.Description, model.Name, model.CampaignId, model.ProgramId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return RedirectToAction("Campaigns", new { campaignId = model.CampaignId });

        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// //////////////////////////////////////////////////////////////
        /// 
        /// //////////////////////////////////////////////////////////////
        /// 
        /// 
        /// <returns></returns>
        public List<Notifications> MyGlobalNotificationsInitializer()
        {
            List<Notifications> dtsource = new List<Notifications>();
            dtsource = (List<Notifications>)TempData["Notificationsdtsource"];

            TempData["Notificationsdtsource"] = dtsource;
            return dtsource;
        }
        [SessionFilterAttribute]
        public ActionResult Notifications(int? organizationID)
        {


            try
            {

                ViewBag.organizationID = 1;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View();
        }

        public JsonResult GetNotifications(DTParameters param, int? organizationID)
        {
            List<Notifications> dtsource = new List<Notifications>();


            dtsource = db.GetNotifications(Convert.ToInt32(Session["organizationID"]));

            TempData["Notificationsdtsource"] = dtsource;
            TempData["organizationID"] = Convert.ToInt32(Session["organizationID"]);


            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            if (param.Length == -1)
            {
                param.Length = dtsource.Count();
            }
            List<Notifications> data = new ResultSet().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, dtsource, columnSearch);
            int count = new ResultSet().Count(param.Search.Value, dtsource, columnSearch);
            DTResult<Notifications> result = new DTResult<Notifications>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return Json(result);
        }
        [SessionFilterAttribute]
        public ActionResult NotificationWizard(int? organizationID)
        {    var model = new Notifications();
            try
            {
                model.Statuses = GetStatusList();
                model.Programs = db.GetProgramDDL();
                //model.Campaigns = db.GetCampaignDDL(Convert.ToInt32(Session["organizationID"]));
              
                model.NotificationTypes = db.GetNotificationTypes();
                List<Notifications> dtsource = MyGlobalNotificationsInitializer();

                model.OrganizationID = Convert.ToInt32(Session["organizationID"]);
            }      
     
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
           return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult NotificationWizard(Notifications model)
        {

            ModelState.Clear();
            List<Notifications> dtsource = MyGlobalNotificationsInitializer();

            char delete = 'N';
            db.UpdateNotifications(Convert.ToInt32(Session["organizationID"]), model.Text, model.Period, model.NTypeId, model.NotificationId, model.CampaignId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);



            return RedirectToAction("Notifications", new { controller = "Home", NotificationId = model.NotificationId });

        }

        public JsonResult GetCampaignDDL(int? programID)
        {
            var model = new Notifications();
            try
            {
                model.Campaigns = db.GetCampaignDDL(programID);

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return Json(model.Campaigns, JsonRequestBehavior.AllowGet);
        }
        [SessionFilterAttribute]
        public ActionResult CreateNotification(int? organizationID)
        {
            var model = new Notifications();
            model.Statuses = GetStatusList();
            model.NotificationTypes = db.GetNotificationTypes();
            model.Programs = db.GetProgramDDL();
       
            List<Notifications> dtsource = MyGlobalNotificationsInitializer();

            model.OrganizationID = Convert.ToInt32(Session["organizationID"]);
            return View("_CreateNotificationsPartial", model);
        }
        [SessionFilterAttribute]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateNotifications(Notifications model)
        {

            ModelState.Clear();
            List<Notifications> dtsource = MyGlobalNotificationsInitializer();

            char delete = 'N';
            db.UpdateNotifications(Convert.ToInt32(Session["organizationID"]),  model.Text, model.Period,model.NTypeId, model.NotificationId, model.CampaignId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);



            return RedirectToAction("Notifications", new { controller = "Home", campaignId = model.CampaignId });

        }
        [SessionFilterAttribute]
        public ActionResult NotificationDetails(int? id)
        {


            List<Notifications> dtsource = MyGlobalNotificationsInitializer();
            Notifications edit = dtsource.FirstOrDefault(x => x.NotificationId == id);
            edit.Statuses = GetStatusList();
            edit.NotificationTypes = db.GetNotificationTypes();

            return View("NotificationDetails", edit);
        }
        // GET: Asset/Edit/5
        [SessionFilterAttribute]
        public ActionResult EditNotification(int? id)
        {

            List<Notifications> dtsource = MyGlobalNotificationsInitializer();
            Notifications edit = dtsource.FirstOrDefault(x => x.NotificationId == id);
            edit.Statuses = GetStatusList();
            edit.NotificationTypes = db.GetNotificationTypes();
            edit.Programs = db.GetProgramDDL();
            edit.Campaigns = db.GetCampaignDDL(edit.ProgramID);
            if (Request.IsAjaxRequest())
                return PartialView("_EditNotificationsPartial", edit);
            return View(edit);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditNotifications(Notifications model)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{

                //    return View("_EditNotificationsPartial", model);
                //}

                ModelState.Clear();
                List<Notifications> dtsource = MyGlobalNotificationsInitializer();
                char delete = 'N';
                db.UpdateNotifications(Convert.ToInt32(Session["organizationID"]), model.Text, model.Period, model.NTypeId, model.NotificationId, model.CampaignId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;

            }
            return RedirectToAction("Notifications", new { controller = "Home", NotificationId = model.NotificationId });

        }
        [SessionFilterAttribute]
        public ActionResult DeleteNotification(int id)
        {

            List<Notifications> dtsource = MyGlobalNotificationsInitializer();

            Notifications delete = dtsource.FirstOrDefault(x => x.NotificationId == id);
            delete.NotificationId = id;



            if (Request.IsAjaxRequest())
                return PartialView("_DeleteNotificationsPartial", delete);
            return View(delete);

        }

        [HttpPost]
        public ActionResult DeleteNotifications(Notifications model)
        {
            try
            {
                List<Notifications> dtsource = MyGlobalNotificationsInitializer();

                char delete = 'Y';
                db.UpdateNotifications(Convert.ToInt32(Session["organizationID"]), model.Text, model.Period, model.NTypeId, model.NotificationId, model.CampaignId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return RedirectToAction("Notifications", new { notificationId = model.NotificationId });

        }
        public List<Clients> MyGlobalClientsInitializer()
        {
            List<Clients> dtsource = new List<Clients>();
            dtsource = (List<Clients>)TempData["Clientsdtsource"];

            TempData["Clientsdtsource"] = dtsource;
            return dtsource;
        }
        [SessionFilterAttribute]
        public ActionResult Clients(int? organizationID)
        {


            try
            {

                ViewBag.organizationID = 1;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View();
        }

        public JsonResult GetClients(DTParameters param, int? organizationID)
        {
            List<Clients> dtsource = new List<Clients>();


            dtsource = db.GetClients(Convert.ToInt32(Session["organizationID"]));

            TempData["Clientsdtsource"] = dtsource;
            TempData["organizationID"] = Convert.ToInt32(Session["organizationID"]);


            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            if (param.Length == -1)
            {
                param.Length = dtsource.Count();
            }
            List<Clients> data = new ResultSet().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, dtsource, columnSearch);
            int count = new ResultSet().Count(param.Search.Value, dtsource, columnSearch);
            DTResult<Clients> result = new DTResult<Clients>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return Json(result);
        }
        public JsonResult GetClientMemberships(DTParameters param, int? organizationID, int? clientID )
        {
            List<ClientMemberships> dtsource = new List<ClientMemberships>();


            dtsource = db.GetClientMemberships(Convert.ToInt32(Session["organizationID"]), clientID);

            TempData["ClientMembershipsdtsource"] = dtsource;
            TempData["organizationID"] = Convert.ToInt32(Session["organizationID"]);


            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            if (param.Length == -1)
            {
                param.Length = dtsource.Count();
            }
            List<ClientMemberships> data = new ResultSet().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, dtsource, columnSearch);
            int count = new ResultSet().Count(param.Search.Value, dtsource, columnSearch);
            DTResult<ClientMemberships> result = new DTResult<ClientMemberships>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return Json(result);
        }
        public JsonResult GetTransactions(DTParameters param, int? organizationID, int? clientID)
        {
            List<Transactions> dtsource = new List<Transactions>();


            dtsource = db.GetTransactions(Convert.ToInt32(Session["organizationID"]), clientID);

            TempData["Transactionsdtsource"] = dtsource;
            TempData["organizationID"] = Convert.ToInt32(Session["organizationID"]);


            List<String> columnSearch = new List<string>();

            foreach (var col in param.Columns)
            {
                columnSearch.Add(col.Search.Value);
            }

            if (param.Length == -1)
            {
                param.Length = dtsource.Count();
            }
            List<Transactions> data = new ResultSet().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, dtsource, columnSearch);
            int count = new ResultSet().Count(param.Search.Value, dtsource, columnSearch);
            DTResult<Transactions> result = new DTResult<Transactions>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = count,
                recordsTotal = count
            };

            return Json(result);
        }
        [SessionFilterAttribute]
        public ActionResult CreateClient(int? organizationID)
        {
            var model = new Clients();

            List<Clients> dtsource = MyGlobalClientsInitializer();
            model.ClientStatuses = db.GetClientStatus();
            model.AccountTypes = db.GetAccountTypes();
            model.ParticipationReasons = db.GetParticipationReasons();
            model.PhoneStatuses = db.GetPhoneStatus();
            
            return View("_CreateClientsPartial", model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateClients(Clients model)
        {
 
       
            ModelState.Clear();
            List<Clients> dtsource = MyGlobalClientsInitializer();

            char delete = 'N';
            db.UpdateClients(Convert.ToInt32(Session["organizationID"]), model.FirstName, model.LastName, model.CStatusId, model.PStatusId,model.ATypeId,model.PhoneNumber,model.MessageAddress,model.ParticipationId,model.PhoneCarrier,model.ClientId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), delete);



            return RedirectToAction("Clients", new { controller = "Home", clientId = model.ClientId });

        }
        [SessionFilterAttribute]
        public ActionResult ClientDetails(int? id)
        {
          

            List<Clients> dtsource = MyGlobalClientsInitializer();
            Clients edit = dtsource.FirstOrDefault(x => x.ClientId == id);
            edit.ClientStatuses = db.GetClientStatus();
            edit.AccountTypes = db.GetAccountTypes();
            edit.ParticipationReasons = db.GetParticipationReasons();
            edit.PhoneStatuses = db.GetPhoneStatus();
            ViewBag.organizationID = Session["organizationID"];
            ViewBag.clientID = id;
            return View("ClientDetails", edit);
        }
      
        // GET: Asset/Edit/5
        [SessionFilterAttribute]
        public ActionResult EditClient(int? id)
        {
            //var testID = 1;

            List<Clients> dtsource = MyGlobalClientsInitializer();
            Clients edit = dtsource.FirstOrDefault(x => x.ClientId == id);
            edit.ClientStatuses = db.GetClientStatus();
            edit.AccountTypes = db.GetAccountTypes();
            edit.ParticipationReasons = db.GetParticipationReasons();
            edit.PhoneStatuses = db.GetPhoneStatus();



            if (Request.IsAjaxRequest())
                return PartialView("_EditClientsPartial", edit);
            return View(edit);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditClients(Clients model)
        {
            try
            {
      
                ModelState.Clear();
                List<Clients> dtsource = MyGlobalClientsInitializer();
                char delete = 'N';
                db.UpdateClients(Convert.ToInt32(Session["organizationID"]), model.FirstName, model.LastName, model.CStatusId, model.PStatusId, model.ATypeId, model.PhoneNumber, model.MessageAddress, model.ParticipationId,
                    model.PhoneCarrier, model.ClientId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), delete);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;

            }

            return RedirectToAction("Clients", new { controller = "Home", ClientId = model.ClientId });
        }
        [SessionFilterAttribute]
        public ActionResult DeleteClient(int? id)
        {

            List<Clients> dtsource = MyGlobalClientsInitializer();

            Clients delete = dtsource.FirstOrDefault(x => x.ClientId == id);




            if (Request.IsAjaxRequest())
                return PartialView("_DeleteClientsPartial", delete);
            return View(delete);

        }

        [HttpPost]
        public ActionResult DeleteClients(Clients model)
        {
            try
            {
                List<Clients> dtsource = MyGlobalClientsInitializer();

                char delete = 'Y';
                db.UpdateClients(Convert.ToInt32(Session["organizationID"]), model.FirstName, model.LastName, model.CStatusId, model.PStatusId, model.ATypeId, model.PhoneNumber, model.MessageAddress, model.ParticipationId, model.PhoneCarrier, model.ClientId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), delete);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return RedirectToAction("Clients", new { clientId = model.ClientId });

        }

        public SelectList GetStatusList()
        {
            SelectList asl = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{ Text = "Enabled", Value = "1"},
                new SelectListItem{ Text = "Disabled", Value = "0"},
            }, "Value", "Text");
            return asl;
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// EXAMPLE
        /// </summary>
        /// <param name="apiPhoneNumber"></param>

        public JsonResult CollectPhoneStatus(string apiPhoneNumber)
        {
            // These values dont reside in the function in the service but are here to support the example
            string apiCompany = "notify";
            string apiUsername = "notifyhealth_cpc001";
            string apiPassword = "6U7gQ458wGUNKm2tS6";

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Take from Organisation Table 



            HttpClient apiReq = new HttpClient();
            XmlDocument apiResponseXML = new XmlDocument();

            string apiURL = "https://api.data24-7.com/v/2.0";

            //eventLog1.WriteEntry("Calling Carrier Lookup API with: " + apiPhoneNumber, EventLogEntryType.Information, 8100);

            apiURL += "?compcode=" + apiCompany + "&user=" + apiUsername + "&pass=" + apiPassword + "&api=T&p1=" + apiPhoneNumber;

            //https://api.data24-7.com/v/2.0?compcode=notify&user=notifyhealth_cpc001&pass=6U7gQ458wGUNKm2tS6&api=T&p1=7194257147
            try
            {
                apiResponseXML.Load(apiURL);
            }
            catch (Exception e)
            {
                //eventLog1.WriteEntry("Error Loading XML via Carrier Lookup API. Error: " + e.Message, EventLogEntryType.Error, 8500);
            }

            XmlElement root = apiResponseXML.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("/response/results/result");

            ClientPhone Phone = null;

            foreach (XmlNode node in nodes)
            {
                Phone = new ClientPhone
                {
                    Number = apiPhoneNumber,
                    PhoneCarrier = node["carrier_name"].InnerText,
                    MessageAddress = node["sms_address"].InnerText,
                    Wireless = node["wless"].InnerText,
                    Status = node["status"].InnerText
                };
            }

            if (Phone.Status == "OK")
            {
                Phone.PStatusId = 2;
            }
            else
            {
                Phone.PStatusId = 1;

            }

            if (Phone.Wireless == "y")
            {
                if (string.IsNullOrEmpty(Phone.MessageAddress))
                { Phone.ParticipationId = 11; }
                // Popup warning that given phone number CANNOT BE USED to send messages but client will be saved.

               //  Submit button creates client account or updates user record but DO NOT INSERT into queue table

                else
                { Phone.ParticipationId = 12; }
                //  Submit button inserts new record into Queue table and adds record to clients table
                
                // ASK FOR TEST DATA API TEST
            }

            else
            {
                Phone.ParticipationId = 8;
                 // Popup warning that given phone number is NOT A WIRELESS PHONE NUMBER AND CANNOT BE USED to send messages but client will be saved

                //Submit button creates client account or updates user record but DO NOT INSERT into queue table
            }


            apiReq.Dispose();
            return Json(Phone, JsonRequestBehavior.AllowGet);
        }



    }
}
//private void QueueOnBoardingNotification()
//{
//    // These values dont reside in the function in the service but are here to support the example
//    int noticeNT = 1;

//    try
//    {
//        SqlCommand sqlCommand = new SqlCommand();

//        sqlConnection.Open();
//        sqlCommand.Connection = sqlConnection;

//        String query = @"IF NOT EXISTS (SELECT * 
//                           FROM queue 
//                           WHERE Client_ID = @c_id 
//                           AND Notification_ID = (SELECT Notification_ID 
//	                                FROM notifications
//	                                WHERE notifications.N_Type_ID = @n_t
//                                                            AND notifications.Organization_ID = @o_id))

//                        BEGIN
//                        INSERT INTO dbo.queue
//                        VALUES (@c_id,
//                             (SELECT Notification_ID 
//                              FROM notifications
//                              WHERE notifications.N_Type_ID = @n_t)
//                                    AND notifications.Organization_ID = @o_id),
//                             @t)
//                        END";

//        sqlCommand.CommandText = query;

//        sqlCommand.Parameters.AddWithValue("@c_id", Convert.ToInt32(currentPerson.Client_ID));
//        sqlCommand.Parameters.AddWithValue("@n_t", noticeNT);
//        sqlCommand.Parameters.AddWithValue("@o_id", $global: org_id);
//        sqlCommand.Parameters.AddWithValue("@t", DateTime.Now);

//        int output = sqlCommand.ExecuteNonQuery();

//        sqlConnection.Close();

//        if (output < 0)
//        {
//            eventLog1.WriteEntry("SQL activity failed while adding notification to queue.", EventLogEntryType.Error, 3120);
//        }
//        else if (output < 0)
//        {
//            eventLog1.WriteEntry("No action taken, notification type (" + noticeNT + ") already queued to client " + currentPerson.Client_ID, EventLogEntryType.Error, 3120);
//        }
//        else if (output == 1)
//        {
//            eventLog1.WriteEntry("Queued notification type (" + noticeNT + ") to client " + currentPerson.Client_ID, EventLogEntryType.Information, 2010);
//        }

//    }
//    catch (Exception e)
//    {
//        eventLog1.WriteEntry("SQL activity failed while adding transaction date with error. " + e.ToString(), EventLogEntryType.Error, 3121);
//    }
//}

