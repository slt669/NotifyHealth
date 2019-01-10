using NotifyHealth.Data_Access_Layer;
using NotifyHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}
