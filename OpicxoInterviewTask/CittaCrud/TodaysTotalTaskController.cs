using Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Base;
using Model.Masters;
using Model.Transactions;
using Newtonsoft.Json;
using PropertyERP.Areas.Transactions.Models;
using PropertyERP.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PropertyERP.Areas.Transactions.Controllers
{
    [SessionExtensions.SessionTimeout]
    [Area("Transactions")]
    public class TodaysTotalTaskController : Controller
    {
        private readonly ILogger<TodaysTotalTaskController> _logger;
        private APIManager _apiManager;
        private IWebHostEnvironment environment;
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="logger"></param>
        [Obsolete]
        public TodaysTotalTaskController(ILogger<TodaysTotalTaskController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _apiManager = new APIManager();
            this.environment = environment;
        }
        [HttpGet]
        public IActionResult TodaysTotalTask(string Type)
        {
            ViewBag.Type = Type;
            if (Type == "1")
            {
                ViewBag.followupsTab = "active";
                ViewBag.followupsShowTab = "active show";
                ViewBag.IsFrom_hdn = 1;
            }
            if (Type == "4")
            {
                ViewBag.SitesVisitsTab = "active";
                ViewBag.SitesVisitsShowTab = "active show";
                ViewBag.IsFrom_hdn = 2;
            }

            var obj = new LeadUI();
            #region Bind Lists
            ViewBag.EmptyList = CommonMaster.BindEmptyList();
            Dictionary<int?, string> objDictAssign = new Dictionary<int?, string>();
            objDictAssign.Add(AllFunctions.getUserID(), AllFunctions.getUserName());
            ViewBag.SiteVisitAssignTo = objDictAssign;
            ViewBag.AssignToId = AllFunctions.getUserID();
            ViewBag.AssignToName = AllFunctions.getUserName();
            #endregion
            return View(obj);
        }

        /// <summary>
        /// Get List of TodaysTotalTask
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetListTodaysTotalTask()
        {
            var response = Json(new
            {
                draw = 0,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = string.Empty
            });

            try
            {
                #region Initialize Input parameter of SP
                var draw = Convert.ToInt32(Request.Form["draw"]);

                var request = new ActivityListRequest();
                request.SortCol = Convert.ToInt32(Request.Form["order[0][column]"]);
                request.SortDir = Request.Form["order[0][dir]"];
                request.PageIndex = Convert.ToInt32(Request.Form["start"]);
                request.PageSize = Convert.ToInt32(Request.Form["length"]);

                request.Cid = AllFunctions.getCompanyID();
                request.Bid = AllFunctions.getBranchID();
                request.Uid = AllFunctions.getUserID();

                string FromDate = null;
                if (!string.IsNullOrEmpty(Request.Form["FromDate"]))
                    FromDate = Convert.ToString(Request.Form["FromDate"]).Trim();

                if (!string.IsNullOrEmpty(FromDate))
                    request.FromDate = FromDate;

                string ToDate = null;
                if (!string.IsNullOrEmpty(Request.Form["ToDate"]))
                    ToDate = Convert.ToString(Request.Form["ToDate"]).Trim();

                if (!string.IsNullOrEmpty(ToDate))
                    request.ToDate = ToDate;

                request.ActivityTypeId = Convert.ToInt32(Request.Form["ActivityTypeId"]);
                request.AssignTo = Convert.ToString(Request.Form["AssignTo"]);

                string StatusIds = null;
                if (!string.IsNullOrEmpty(Request.Form["StatusIds"]))
                    StatusIds = Convert.ToString(Request.Form["StatusIds"]).Trim();

                if (!string.IsNullOrEmpty(StatusIds))
                    request.StatusIds = StatusIds;

                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<List<Activity>>>(
                    await _apiManager.CallPostMethod("todaytotaltask/todaystotalasklist", request));

                #endregion

                #region Return JSON
                response = Json(new
                {
                    draw = draw,
                    recordsTotal = baseResponse.Count,
                    recordsFiltered = baseResponse.Count,
                    data = baseResponse.Data
                });

                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

            }
            return response;
        }

        /// <summary>
        /// Get List of SitesVisits
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetListSitesVisits()
        {
            var response = Json(new
            {
                draw = 0,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = string.Empty
            });

            try
            {
                #region Initialize Input parameter of SP
                var draw = Convert.ToInt32(Request.Form["draw"]);

                var request = new EmployeeListRequest();
                request.SortCol = Convert.ToInt32(Request.Form["order[0][column]"]);
                request.SortDir = Request.Form["order[0][dir]"];
                request.PageIndex = Convert.ToInt32(Request.Form["start"]);
                request.PageSize = Convert.ToInt32(Request.Form["length"]);

                bool? IsActive = null;
                if (!string.IsNullOrEmpty(Request.Form["isactive"]))
                    IsActive = Convert.ToBoolean(Convert.ToInt32(Request.Form["isactive"]));
                request.IsActive = IsActive;

                string Search = null;
                if (!string.IsNullOrEmpty(Request.Form["search"]))
                    Search = Convert.ToString(Request.Form["search"]).Trim();

                if (!string.IsNullOrEmpty(Search))
                    request.Keyword = Search;

                request.Cid = AllFunctions.getCompanyID();
                request.IsDefault = null;
                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<List<DeveloperListResponse>>>(
                    await _apiManager.CallPostMethod("masters/developer/list", request));

                #endregion

                #region Return JSON
                response = Json(new
                {
                    draw = draw,
                    recordsTotal = baseResponse.Count,
                    recordsFiltered = baseResponse.Count,
                    data = baseResponse.Data
                });

                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

            }
            return response;
        }


        [HttpGet]
        public IActionResult ActivityPartial(string IsFrom_hdn)
        {
            
            if (IsFrom_hdn == "1")
            {
                ViewBag.followupsTab = "active";
                ViewBag.followupsShowTab = "active show";
                ViewBag.IsFrom_hdn = 1;
                ViewBag.Page = "Lead";
            }
            if (IsFrom_hdn == "2")
            {
                ViewBag.SitesVisitsTab = "active";
                ViewBag.SitesVisitsShowTab = "active show";
                ViewBag.IsFrom_hdn = 2;
                ViewBag.Page = "Opportunity";
            }
            LeadUI obj = new LeadUI();

            //#region Default

            //var date1 = AllFunctions.GetDateTime().AddMonths(-2);
            //var date2 = AllFunctions.GetDateTime();
            //try
            //{
            //    string fromdateStr = date1.ToString(AppConstant.DateFormat2);
            //    string todateStr = date2.ToString(AppConstant.DateFormat2);
            //    string FromToDate = fromdateStr + " - " + todateStr;
            //    ViewBag.FromToDate = FromToDate;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, ex.Message);
            //}
            //#endregion

            #region Bind Lists
            ViewBag.EmptyList = CommonMaster.BindEmptyList();
            Dictionary<int?, string> objDictAssign = new Dictionary<int?, string>();
            objDictAssign.Add(AllFunctions.getUserID(), AllFunctions.getUserName());
            ViewBag.SiteVisitAssignTo = objDictAssign;
            ViewBag.AssignToId = AllFunctions.getUserID();
            ViewBag.AssignToName = AllFunctions.getUserName();
            #endregion
            return PartialView("_ActivityPartial", obj.listActivity);
        }

        //[HttpGet]
        //public JsonResult AddAssignToSession(string AssignToIds)
        //{
        //    var messageType = AppConstantUI.AlertSuccessType;

        //    AllFunctions.setSessionByKeyValue("DashboardAssignToIds", Convert.ToString(AssignToIds));

        //    return Json(new { messageType });
        //}

    }
}
