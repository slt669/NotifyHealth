using Newtonsoft.Json;
using NotifyHealth.CustomFilters;
using NotifyHealth.Data_Access_Layer;
using NotifyHealth.Models;
using NotifyHealth.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using System.Xml;

namespace NotifyHealth.Controllers
{
    public class HomeController : Controller
    {
        private NotifyHealthDB db = new NotifyHealthDB();

        [SessionFilterAttribute]
        public ActionResult Index()
        {
            DashboardViewModel model = db.GetDashboardDetails(Convert.ToInt32(Session["organizationID"]));
            List<BarChart> Bar = db.GetBarChartDetails(Convert.ToInt32(Session["organizationID"]));

            var json = JsonConvert.SerializeObject(Bar);

            ViewBag.BarData = json;

            ViewBag.organizationID = Session["organizationID"];
            return View(model);
        }

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
            model.StatusId = 1;
            model.Statuses = DropDownListUtility.GetStatusList(model.StatusId);
            model.OrganizationID = Convert.ToInt32(Session["organizationID"]);
            return View("_CreateProgramsPartial", model);
        }

        [SessionFilterAttribute]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreatePrograms(Programs model)
        {
            ViewBag.Message = "Sucess or Failure Message";
            ModelState.Clear();
            List<Programs> dtsource = MyGlobalProgramsInitializer();

            char delete = 'N';
            db.UpdatePrograms(Convert.ToInt32(Session["organizationID"]), model.Description, model.Name, model.ProgramId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);

            return RedirectToAction("Programs", new { controller = "Home", organizationID = model.OrganizationID });
        }

        // GET: Asset/Edit/5
        [SessionFilterAttribute]
        public ActionResult EditProgram(int? id)
        {
            List<Programs> dtsource = MyGlobalProgramsInitializer();
            Programs edit = dtsource.FirstOrDefault(x => x.ProgramId == id);
            edit.Statuses = DropDownListUtility. GetStatusList(edit.StatusId);
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
            edit.Statuses = DropDownListUtility. GetStatusList(edit.StatusId);
            ViewBag.organizationID = Session["organizationID"];
            ViewBag.ProgramId = id;

            return View("ProgramDetails", edit);
        }

        [SessionFilterAttribute]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditPrograms(Programs model)
        {
            try
            {
                ModelState.Clear();
                List<Programs> dtsource = MyGlobalProgramsInitializer();
                char delete = 'N';
                db.UpdatePrograms(Convert.ToInt32(Session["organizationID"]), model.Description, model.Name, model.ProgramId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return RedirectToAction("Programs", new { organizationID = model.OrganizationID });
        }

        [SessionFilterAttribute]
        public ActionResult DeleteProgram(int? id)
        {
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

        [SessionFilterAttribute]
        public ActionResult Campaigns(int? organizationID)
        {
            try
            {
                ViewBag.organizationID = Convert.ToInt32(Session["organizationID"]);
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
        public ActionResult CreateCampaign(int? organizationID, int? programID = null)
        {
            var model = new Campaigns();

            List<Campaigns> dtsource = MyGlobalCampaignsInitializer();
            model.Programs = db.GetProgramDDL(Convert.ToInt32(Session["organizationID"]), programID.ToString());
            model.StatusId = 1;
            model.Statuses = DropDownListUtility.GetStatusList(model.StatusId);
            model.OrganizationID = organizationID;
            return View("_CreateCampaignsPartial", model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateCampaigns(Campaigns model)
        {
            ModelState.Clear();
            List<Campaigns> dtsource = MyGlobalCampaignsInitializer();

            char delete = 'N';
            db.UpdateCampaigns(Convert.ToInt32(Session["organizationID"]), model.Description, model.Name, model.CampaignId, model.ProgramId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);

            return RedirectToAction("Campaigns", new { controller = "Home", campaignId = model.CampaignId });
        }

        [SessionFilterAttribute]
        public ActionResult CampaignDetails(int? id)
        {
            List<Campaigns> dtsource = MyGlobalCampaignsInitializer();
            Campaigns edit = dtsource.FirstOrDefault(x => x.CampaignId == id);
            edit.Statuses = DropDownListUtility.GetStatusList(edit.StatusId.ToString());
            ViewBag.organizationID = Session["organizationID"];
            ViewBag.CampaignId = id;

            return View("CampaignDetails", edit);
        }

        [SessionFilterAttribute]
        public ActionResult EditCampaign(int? id)
        {
            List<Campaigns> dtsource = MyGlobalCampaignsInitializer();
            Campaigns edit = dtsource.FirstOrDefault(x => x.CampaignId == id);
            edit.Programs = db.GetProgramDDL(Convert.ToInt32(Session["organizationID"]));
            //edit.Statuses = GetStatusList();
            edit.Statuses = DropDownListUtility.GetStatusList(edit.StatusId);
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

            return RedirectToAction("Campaigns");
        }

        public List<Notifications> MyGlobalNotificationsInitializer()
        {
            List<Notifications> dtsource = new List<Notifications>();
            dtsource = (List<Notifications>)TempData["NotificationsbyCampaignsdtsource"];

            TempData["NotificationsbyCampaignsdtsource"] = dtsource;
            return dtsource;
        }

        [SessionFilterAttribute]
        public ActionResult Notifications(int? organizationID)
        {
            try
            {
                ViewBag.organizationID = Convert.ToInt32(Session["organizationID"]);
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
        {
            var model = new Notifications();
            try
            {
                model.StatusId = 1;
                model.Statuses = DropDownListUtility.GetStatusList(model.StatusId);
                model.Programs = db.GetProgramDDL(Convert.ToInt32(Session["organizationID"]));

                model.NotificationTypes = db.GetNotificationTypes();

                var types = model.NotificationTypes.ToList();

                types.Remove(types.First(x => x.Value == "1"));
                types.Remove(types.First(x => x.Value == "2"));
                types.Remove(types.First(x => x.Value == "3"));
                types.Remove(types.First(x => x.Value == "4"));
                types.Remove(types.First(x => x.Value == "5"));
                types.Remove(types.First(x => x.Value == "6"));
                types.Remove(types.First(x => x.Value == "7"));

                model.NotificationTypes = types;
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

            return RedirectToAction("Notifications");
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
        public ActionResult CreateNotification(int? organizationID, int? campaignID = null)
        {
            var model = new Notifications();
            model.StatusId = 1;
            model.Statuses = DropDownListUtility.GetStatusList(model.StatusId);
            model.NotificationTypes = db.GetNotificationTypes();
            model.Programs = db.GetProgramDDL(Convert.ToInt32(Session["organizationID"]));
            if (campaignID != null)
            {
                model.CampaignId = Convert.ToInt32(campaignID);
            }


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
            db.UpdateNotifications(Convert.ToInt32(Session["organizationID"]), model.Text, model.Period, model.NTypeId, model.NotificationId, model.CampaignId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);

            return RedirectToAction("CampaignDetails", new { controller = "Home", id = model.CampaignId });
        }

        [SessionFilterAttribute]
        public ActionResult NotificationDetails(int? id)
        {
            List<Notifications> dtsource = MyGlobalNotificationsInitializer();
            Notifications edit = dtsource.FirstOrDefault(x => x.NotificationId == id);
            edit.Statuses = DropDownListUtility.GetStatusList(edit.StatusId);
            edit.NotificationTypes = db.GetNotificationTypes();

            return View("NotificationDetails", edit);
        }

        [SessionFilterAttribute]
        public ActionResult EditNotification(int? id)
        {
            List<Notifications> dtsource = MyGlobalNotificationsInitializer();
            Notifications edit = dtsource.FirstOrDefault(x => x.NotificationId == id);
            edit.Statuses = DropDownListUtility.GetStatusList(edit.StatusId);
            edit.NotificationTypes = db.GetNotificationTypes();
            edit.Programs = db.GetProgramDDL(Convert.ToInt32(Session["organizationID"]));
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
                ModelState.Clear();
                List<Notifications> dtsource = MyGlobalNotificationsInitializer();
                char delete = 'N';
                db.UpdateNotifications(Convert.ToInt32(Session["organizationID"]), model.Text, model.Period, model.NTypeId, model.NotificationId, model.CampaignId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return RedirectToAction("CampaignDetails", new { controller = "Home", id = model.CampaignId });
        }

        [SessionFilterAttribute]
        public ActionResult DeleteNotification(int id)
        {
            List<Notifications> dtsource = MyGlobalNotificationsInitializer();

            Notifications delete = dtsource.FirstOrDefault(x => x.NotificationId == id);
            List<Campaigns> CL = db.GetCampaigns(Convert.ToInt32(Session["organizationID"]));
            delete.Campaign = CL.FirstOrDefault(c => c.CampaignId.Equals(delete.CampaignId)).ToString();

            var find = CL.FirstOrDefault(x => x.CampaignId == delete.CampaignId);
            if (find != null)
            {
                delete.CampaignId = find.CampaignId;
                delete.Campaign = find.Name;
            }

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

            return RedirectToAction("CampaignDetails", new { controller = "Home", id = model.CampaignId });
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
                ViewBag.organizationID = Convert.ToInt32(Session["organizationID"]);
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

        public JsonResult GetClientMemberships(DTParameters param, int? organizationID, int? clientID)
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
            var PhoneCarrierBindingBroke = Request.Form["PhoneCarrier"];
            var MessageAddressBindingBroke = Request.Form["MessageAddress"];

            PhoneCarrierBindingBroke = PhoneCarrierBindingBroke.Replace(",", "");
            MessageAddressBindingBroke = MessageAddressBindingBroke.Replace(",", "");

            ModelState.Clear();
            List<Clients> dtsource = MyGlobalClientsInitializer();

            char delete = 'N';
            db.UpdateClients(Convert.ToInt32(Session["organizationID"]), model.FirstName, model.LastName, model.CStatusId, model.PStatusId, model.ATypeId, model.PhoneNumber, MessageAddressBindingBroke, model.ParticipationId, PhoneCarrierBindingBroke, model.ClientId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), delete);

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
        public ActionResult Schedule()
        {
  
            return View();
        }
        public JsonResult GetNotifications123()
        {
            List<Notifications> dtsource = new List<Notifications>();

            dtsource = db.GetNotifications(Convert.ToInt32(Session["organizationID"]));
        
                return new JsonResult { Data = dtsource, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
         
        }
        [HttpPost]
        public JsonResult SaveNotification(Notifications model)
        {
            var status = false;

            ModelState.Clear();
            List<Notifications> dtsource = MyGlobalNotificationsInitializer();
            char delete = 'N';
            db.UpdateNotifications(Convert.ToInt32(Session["organizationID"]), model.Text, model.Period, model.NTypeId, model.NotificationId, model.CampaignId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);
            status = true;

           
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult DeleteNotification(Notifications model)
        {
            var status = false;
            char delete = 'Y';
            db.UpdateNotifications(Convert.ToInt32(Session["organizationID"]), model.Text, model.Period, model.NTypeId, model.NotificationId, model.CampaignId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), model.StatusId, delete);

            status = true;
                
     
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult ValuesAdded(string[][] Memberships)
        {
            int Clientid = Convert.ToInt32(Memberships[0][0]);
            int rowToRemove = 0;

            Memberships = Memberships.Where((el, i) => i != rowToRemove).ToArray();

            if (Memberships == null)
            {
                List<Campaigns> Camp = db.GetCampaigns(Convert.ToInt32(Session["organizationID"]));
                int count = Camp.Count();
                foreach (Campaigns C in Camp)
                {
                    
                    db.UpdateClientMembership(Convert.ToInt32(Session["organizationID"]), Convert.ToInt32(C.CampaignId), Clientid,DateTime.Now, 'Y');
                }
            }
            else
            {
                List<string[]> d = Memberships.Cast<string[]>().ToList();

                foreach (string[] value in d)
                {
                    foreach (string valueinner in value)
                    {
                        string[] Membership = valueinner.Split('|');
                        int Campaign = Convert.ToInt32(Membership[0]);
                        DateTime start = Convert.ToDateTime(Membership[1]);
                        db.UpdateClientMembership(Convert.ToInt32(Session["organizationID"]), Campaign, Clientid, start, 'N');
                    }
                }
            }
            return Json(new { Memberships }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ValuesDeleted(string[][] Memberships)
        {
            int Clientid = Convert.ToInt32(Memberships[0][0]);
            int rowToRemove = 0;

            Memberships = Memberships.Where((el, i) => i != rowToRemove).ToArray();

            if (Memberships == null)
            {
            }
            else
            {
                List<string[]> d = Memberships.Cast<string[]>().ToList();

                foreach (string[] value in d)
                {
                    foreach (string valueinner in value)
                    {
                        string[] Membership = valueinner.Split('|');
                        int Campaign = Convert.ToInt32(Membership[0]);
                        DateTime start = Convert.ToDateTime(Membership[1]);
                        db.UpdateClientMembership(Convert.ToInt32(Session["organizationID"]), Campaign, Clientid, start, 'Y');
                    }
                }
            }

            return Json(new { Memberships }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ClientMembershipsStartDate(int id)
        {
            List<Clients> dtsource = MyGlobalClientsInitializer();
            Clients edit = dtsource.FirstOrDefault(x => x.ClientId == id);
            List<ClientMemberships> cl = db.GetClientMemberships(Convert.ToInt32(Session["organizationID"]), edit.ClientId);
            //List<Campaigns> Camp = db.GetCampaigns(Convert.ToInt32(Session["organizationID"]));

            return PartialView("_ClientMembershipsStartDatePartial", edit);
        }

        [SessionFilterAttribute]
        public ActionResult EditClient(int? id)
        {
            List<Clients> dtsource = MyGlobalClientsInitializer();
            Clients edit = dtsource.FirstOrDefault(x => x.ClientId == id);
            edit.ClientStatuses = db.GetClientStatus();
            edit.AccountTypes = db.GetAccountTypes();
            edit.ParticipationReasons = db.GetParticipationReasons();
            edit.PhoneStatuses = db.GetPhoneStatus();

            List<ClientMemberships> cl = db.GetClientMemberships(Convert.ToInt32(Session["organizationID"]), edit.ClientId);
            List<Campaigns> Camp = db.GetCampaigns(Convert.ToInt32(Session["organizationID"]));
            edit.ClientMemberships = cl;
            
            int count = cl.Count();

            for (int i = 0; i < count; i++)
            {
                ClientMemberships num = cl.ElementAt(i);

                Camp.RemoveAll(remov => remov.CampaignId == num.CampaignId);
            }

            ViewBag.ClientMembershipsSelected = cl.Select(x =>

                                  new SelectListItem()
                                  {
                                      Text = x.Campaign,
                                      Value = x.CampaignId.ToString() + "|" + x.Start.ToString()
                                  });

            ViewBag.Unselected = Camp.Select(x =>

                              new SelectListItem()
                              {
                                  Text = x.Name,
                                  Value = x.CampaignId.ToString() + "|" + DateTime.Now.ToString()
                              });

            return View(edit);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditClients(Clients model)
        {
            try
            {
                var PhoneCarrierBindingBroke = Request.Form["PhoneCarrier"];
                var MessageAddressBindingBroke = Request.Form["MessageAddress"];
                PhoneCarrierBindingBroke = PhoneCarrierBindingBroke.Replace(",", "");
                MessageAddressBindingBroke = MessageAddressBindingBroke.Replace(",", "");
                ModelState.Clear();
                List<Clients> dtsource = MyGlobalClientsInitializer();
                char delete = 'N';
                db.UpdateClients(Convert.ToInt32(Session["organizationID"]), model.FirstName, model.LastName, model.CStatusId, model.PStatusId, model.ATypeId, model.PhoneNumber, MessageAddressBindingBroke, model.ParticipationId,
                    PhoneCarrierBindingBroke, model.ClientId, Convert.ToInt32(Session["UserLogon"]), Convert.ToInt32(Session["UserLogon"]), delete);
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

            return RedirectToAction("Clients");
        }
        public static class DropDownListUtility
        {
            public static IEnumerable<SelectListItem> GetStatusList(object selectedValue)
            {
                return new List<SelectListItem>
        {
            new SelectListItem{ Text="Enabled", Value = "1", Selected = "1" == selectedValue.ToString()},
            new SelectListItem{ Text="Disabled", Value = "0", Selected = "0" == selectedValue.ToString()},
        };
            }
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// EXAMPLE
        /// </summary>
        /// <param name="apiPhoneNumber"></param>

        public JsonResult CollectPhoneStatus(string apiPhoneNumber, int organizationID)
        {
            // These values dont reside in the function in the service but are here to support the example
            string apiCompany = "notify";
            //string apiUsername = "notifyhealth_cpc001";
            //string apiPassword = "6U7gQ458wGUNKm2tS6";

            SMSAccount sms = db.GetSMSAccount(organizationID);
            ClientPhone Phone = null;
            if (apiPhoneNumber.Length != 10)
            {
                return Json(Phone, JsonRequestBehavior.AllowGet);
            }

            HttpClient apiReq = new HttpClient();
            XmlDocument apiResponseXML = new XmlDocument();

            string apiURL = "https://api.data24-7.com/v/2.0";

            apiURL += "?compcode=" + apiCompany + "&user=" + sms.API_Username + "&pass=" + sms.API_Password + "&api=T&p1=" + apiPhoneNumber;

            //https://api.data24-7.com/v/2.0?compcode=notify&user=notifyhealth_cpc001&pass=6U7gQ458wGUNKm2tS6&api=T&p1=7194257147
            try
            {
                apiResponseXML.Load(apiURL);
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }

            XmlElement root = apiResponseXML.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("/response/results/result");

            //D247_INVALID_PHONE1234567891

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
                {
                    Phone.ParticipationId = 11;
                    TempData["alertMessage"] = "Popup warning that given phone number CANNOT BE USED to send messages but client will be saved.";
                    Phone.Warning = "Popup warning that given phone number CANNOT BE USED to send messages but client will be saved.";
                }
                //  Submit button creates client account or updates user record but DO NOT INSERT into queue table
                else
                {
                    Phone.ParticipationId = 12;
                    TempData["alertMessage"] = "Submit button inserts new record into Queue table and adds record to clients table";
                    Phone.Warning = "Submit button inserts new record into Queue table and adds record to clients table";
                }
                //

                // ASK FOR TEST DATA API TEST
            }
            else
            {
                Phone.ParticipationId = 8;
                TempData["alertMessage"] = "Popup warning that given phone number is NOT A WIRELESS PHONE NUMBER AND CANNOT BE USED to send messages but client will be saved";
                Phone.Warning = "Popup warning that given phone number is NOT A WIRELESS PHONE NUMBER AND CANNOT BE USED to send messages but client will be saved";

                //Submit button creates client account or updates user record but DO NOT INSERT into queue table
            }

            apiReq.Dispose();
            return Json(Phone, JsonRequestBehavior.AllowGet);
        }
    }
}