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
            ViewBag.organizationID = 1;
            return View();
        }
        /// <summary>
        /// Programs
        /// </summary>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        //[SessionFilterAttribute]
        public ActionResult Programs(int? organizationID)
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

        public List<Programs> MyGlobalProgramsInitializer()
        {
            List<Programs> dtsource = new List<Programs>();
            dtsource = (List<Programs>)TempData["dtsource"];

            TempData["dtsource"] = dtsource;
            return dtsource;
        }


        //[SessionFilterAttribute]
        public JsonResult GetPrograms(DTParameters param, int? organizationID)
        {
            List<Programs> dtsource = new List<Programs>();


            dtsource = db.GetPrograms(1);

            TempData["dtsource"] = dtsource;
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

        // GET: Asset/Create
        //[SessionFilterAttribute]
        public ActionResult CreateProgram(int? organizationID)
        {
            var model = new Programs();
            //string id = RouteData.Values["userID"].ToString();
            List<Programs> dtsource = MyGlobalProgramsInitializer();
            model.Statuses = GetStatusList();
            model.OrganizationID = organizationID;
            return View("_CreateProgramsPartial", model);
        }

        // POST: Asset/Create
        //[SessionFilterAttribute]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreatePrograms(Programs model)
        {
            if (!ModelState.IsValid)
            {

                return View("_ProgramsModal", model);
            }
            ViewBag.Message = "Sucess or Failure Message";
            ModelState.Clear();
            List<Programs> dtsource = MyGlobalProgramsInitializer();

            char delete = 'N';
            db.UpdatePrograms(model.OrganizationID, model.Description, model.Name, model.ProgramId, delete);

            //if (task.Exception != null)
            //{
            //    ModelState.AddModelError("", "Unable to add the Asset");
            //    return View("_CreatePartial", model);
            //}

            return RedirectToAction("Programs", new { controller = "Home", organizationID = model.OrganizationID });

        }
        // GET: Asset/Edit/5
        //[SessionFilterAttribute]
        public ActionResult Edit(int? id)
        {
            var testID = 1;
            //var asset = DbContext.Assets.FirstOrDefault(x => x.AssetID == id);
            List<Programs> dtsource = MyGlobalProgramsInitializer();
            Programs edit = dtsource.FirstOrDefault(x => x.ProgramId == testID);
            edit.Statuses = GetStatusList();
            //AssetViewModel assetViewModel = MapToViewModel(asset);


            if (Request.IsAjaxRequest())
                return PartialView("_EditProgramsPartial", edit);
            return View(edit);
        }

        // POST: Asset/Edit/5
        //[SessionFilterAttribute]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditPrograms(Programs model)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    return View("_EditProgramsPartial", model);
                }
                ViewBag.Message = "Sucess or Failure Message";
                ModelState.Clear();
                List<Programs> dtsource = MyGlobalProgramsInitializer();
                char delete = 'N';
                db.UpdatePrograms(model.OrganizationID, model.Description, model.Name, model.ProgramId, delete);
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
            return PartialView("_EditProgramsPartial", model);
            //return RedirectToAction("SpeedDial", new { userID = model.VoiceUserID });

        }
        //    [SessionFilterAttribute]
        public ActionResult Delete(int? id)
        {
            var testID = 4;
            List<Programs> dtsource = MyGlobalProgramsInitializer();

            Programs delete = dtsource.FirstOrDefault(x => x.ProgramId == testID);




            if (Request.IsAjaxRequest())
                return PartialView("_DeleteProgramsPartial", delete);
            return View(delete);

        }

        // POST: Asset/Delete/5
        //[SessionFilterAttribute]
        [HttpPost]
        public ActionResult DeletePrograms(Programs model)
        {
            try
            {
                List<Programs> dtsource = MyGlobalProgramsInitializer();

                char delete = 'Y';
                db.UpdatePrograms(model.OrganizationID, model.Description, model.Name, model.ProgramId, delete);
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

            return RedirectToAction("Programs", new { organizationID = model.ProgramId });

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<Campaigns> MyGlobalCampaignsInitializer()
        {
            List<Campaigns> dtsource = new List<Campaigns>();
            dtsource = (List<Campaigns>)TempData["dtsource"];

            TempData["dtsource"] = dtsource;
            return dtsource;
        }
        /// <summary>
        /// Campaigns
        /// </summary>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        //[SessionFilterAttribute]
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


            dtsource = db.GetCampaigns(1);

            TempData["dtsource"] = dtsource;
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

        public ActionResult CreateCampaign(int? campaignId)
        {
            var model = new Campaigns();

            List<Campaigns> dtsource = MyGlobalCampaignsInitializer();
            model.Statuses = GetStatusList();
            model.CampaignId = campaignId;
            return View("_CreateCampaignsPartial", model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateCampaigns(Campaigns model)
        {
            if (!ModelState.IsValid)
            {

                return View("_CampaignsModal", model);
            }
            ViewBag.Message = "Sucess or Failure Message";
            ModelState.Clear();
            List<Campaigns> dtsource = MyGlobalCampaignsInitializer();

            char delete = 'N';
            db.UpdateCampaigns(model.CampaignId, model.Description, model.Name, model.ProgramId, delete);



            return RedirectToAction("Campaigns", new { controller = "Home", campaignId = model.CampaignId });

        }
        // GET: Asset/Edit/5
        //[SessionFilterAttribute]
        public ActionResult EditCampaign(int? id)
        {
            var testID = 1;

            List<Campaigns> dtsource = MyGlobalCampaignsInitializer();
            Campaigns edit = dtsource.FirstOrDefault(x => x.ProgramId == testID);
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
                if (!ModelState.IsValid)
                {

                    return View("_EditCampaignsPartial", model);
                }
                ViewBag.Message = "Sucess or Failure Message";
                ModelState.Clear();
                List<Campaigns> dtsource = MyGlobalCampaignsInitializer();
                char delete = 'N';
                db.UpdateCampaigns(model.CampaignId, model.Description, model.Name, model.ProgramId, delete);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;

            }
            return PartialView("_EditCampaignsPartial", model);

        }

        public ActionResult DeleteCampaign(int? id)
        {
            var testID = 4;
            List<Campaigns> dtsource = MyGlobalCampaignsInitializer();

            Campaigns delete = dtsource.FirstOrDefault(x => x.CampaignId == testID);




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
                db.UpdateCampaigns(model.CampaignId, model.Description, model.Name, model.ProgramId, delete);
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
            dtsource = (List<Notifications>)TempData["dtsource"];

            TempData["dtsource"] = dtsource;
            return dtsource;
        }
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


            dtsource = db.GetNotifications(1);

            TempData["dtsource"] = dtsource;
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

                public ActionResult NotificationWizard(int? organizationID)
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

        public ActionResult CreateNotification(int? organizationID)
        {
            var model = new Notifications();
            model.Statuses = GetStatusList();
            model.NotificationTypes = db.GetNotificationTypes();
            List<Notifications> dtsource = MyGlobalNotificationsInitializer();

            model.OrganizationID = organizationID;
            return View("_CreateNotificationsPartial", model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateNotifications(Notifications model)
        {
            if (!ModelState.IsValid)
            {

                return View("_NotificationsModal", model);
            }
            ViewBag.Message = "Sucess or Failure Message";
            ModelState.Clear();
            List<Notifications> dtsource = MyGlobalNotificationsInitializer();

            char delete = 'N';
            db.UpdateNotifications(model.CampaignId, model.Text, model.Period, model.NotificationId, delete);



            return RedirectToAction("Notifications", new { controller = "Home", campaignId = model.CampaignId });

        }
        // GET: Asset/Edit/5
        //[SessionFilterAttribute]
        public ActionResult EditNotification(int? id)
        {
            var testID = 1;

            List<Notifications> dtsource = MyGlobalNotificationsInitializer();
            Notifications edit = dtsource.FirstOrDefault(x => x.NotificationId == testID);
            edit.Statuses = GetStatusList();
            edit.NotificationTypes = db.GetNotificationTypes();


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
                if (!ModelState.IsValid)
                {

                    return View("_EditNotificationsPartial", model);
                }
                ViewBag.Message = "Sucess or Failure Message";
                ModelState.Clear();
                List<Notifications> dtsource = MyGlobalNotificationsInitializer();
                char delete = 'N';
                db.UpdateNotifications(model.CampaignId, model.Text, model.Period, model.NotificationId, delete);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;

            }
            return PartialView("_EditNotificationsPartial", model);

        }

        public ActionResult DeleteNotification(int? id)
        {
            var testID = 4;
            List<Notifications> dtsource = MyGlobalNotificationsInitializer();

            Notifications delete = dtsource.FirstOrDefault(x => x.CampaignId == testID);




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
                db.UpdateNotifications(model.CampaignId, model.Text, model.Period, model.NotificationId, delete);
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
            dtsource = (List<Clients>)TempData["dtsource"];

            TempData["dtsource"] = dtsource;
            return dtsource;
        }
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


            dtsource = db.GetClients(1);

            TempData["dtsource"] = dtsource;
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
        public ActionResult CreateClient(int? organizationID)
        {
            var model = new Clients();

            List<Clients> dtsource = MyGlobalClientsInitializer();
            model.ClientStatuses = db.GetClientStatus();
            model.AccountTypes = db.GetAccountTypes();
            model.ParticipationReasons = db.GetParticipationReasons();
            model.PhoneStatuses = db.GetPhoneStatus();
            model.ClientId = organizationID;
            return View("_CreateClientsPartial", model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateClients(Clients model)
        {
            if (!ModelState.IsValid)
            {

                return View("_ClientsModal", model);
            }
            ViewBag.Message = "Sucess or Failure Message";
            ModelState.Clear();
            List<Clients> dtsource = MyGlobalClientsInitializer();

            char delete = 'N';
            db.UpdateClients(model.OrganizationID, model.FirstName, model.LastName, model.ClientId, delete);



            return RedirectToAction("Clients", new { controller = "Home", clientId = model.ClientId });

        }
        // GET: Asset/Edit/5
        //[SessionFilterAttribute]
        public ActionResult EditClient(int? id)
        {
            var testID = 1;

            List<Clients> dtsource = MyGlobalClientsInitializer();
            Clients edit = dtsource.FirstOrDefault(x => x.ClientId == testID);
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
                if (!ModelState.IsValid)
                {

                    return View("_EditClientsPartial", model);
                }
                ViewBag.Message = "Sucess or Failure Message";
                ModelState.Clear();
                List<Clients> dtsource = MyGlobalClientsInitializer();
                char delete = 'N';
                db.UpdateClients(model.OrganizationID, model.FirstName, model.LastName, model.ClientId, delete);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;

            }
            return PartialView("_EditClientsPartial", model);

        }

        public ActionResult DeleteClient(int? id)
        {
            var testID = 4;
            List<Clients> dtsource = MyGlobalClientsInitializer();

            Clients delete = dtsource.FirstOrDefault(x => x.ClientId == testID);




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
                db.UpdateClients(model.OrganizationID, model.FirstName, model.LastName, model.ClientId, delete);
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
                    CarrierName = node["carrier_name"].InnerText,
                    Address = node["sms_address"].InnerText,
                    Wireless = node["wless"].InnerText,
                    Status = node["status"].InnerText
                };
            }

            if (Phone.Status == "OK")
            {
                Phone.ParsedStatus = 2;
            }
            else
            {
                Phone.ParsedStatus = 1;

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

