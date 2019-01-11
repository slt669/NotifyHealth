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
        public ActionResult Index()
        {
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

        public List<Programs> MyGlobalVariableInitializer()
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
        public ActionResult Create(int? organizationID)
        {
            var model = new Programs();
            //string id = RouteData.Values["userID"].ToString();
            List<Programs> dtsource = MyGlobalVariableInitializer();

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
            List<Programs> dtsource = MyGlobalVariableInitializer();

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
            List<Programs> dtsource = MyGlobalVariableInitializer();
            Programs edit = dtsource.FirstOrDefault(x => x.ProgramId == testID);

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
                List<Programs> dtsource = MyGlobalVariableInitializer();
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
            List<Programs> dtsource = MyGlobalVariableInitializer();

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
                List<Programs> dtsource = MyGlobalVariableInitializer();

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

        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// EXAMPLE
        /// </summary>
        /// <param name="apiPhoneNumber"></param>

        private void CollectPhoneStatus(string apiPhoneNumber)
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

            foreach (XmlNode node in nodes)
            {
                //clientPhone = new Phone
                //{
                //    Number = apiPhoneNumber,
                //    CarrierName = node["carrier_name"].InnerText,
                //    Address = node["sms_address"].InnerText,
                //    Wireless = node["wless"].InnerText,
                //    Status = node["status"].InnerText
                //};
            }

            apiReq.Dispose();
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



    }
}
