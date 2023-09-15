using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Base;
using Model.Common;
using Model.Masters;
using Model.Transactions;
using Newtonsoft.Json;
using PropertyERP.Areas.Transactions.Models;
using PropertyERP.Common;

namespace PropertyERP.Areas.Transactions.Controllers
{
    [SessionExtensions.SessionTimeout]
    [Area("Transactions")]
    public class LeadController : Controller
    {
        private readonly ILogger<LeadController> _logger;
        private APIManager _apiManager;
        private readonly IMapper _mapper;
        private IWebHostEnvironment environment;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="logger"></param>        
        public LeadController(ILogger<LeadController> logger, IMapper mapper, IWebHostEnvironment environment)
        {
            _logger = logger;
            _apiManager = new APIManager();
            _mapper = mapper;
            this.environment = environment;
        }
        public void AssignRights(string name)
        {
            try
            {
                List<Page> listPage = new List<Page>();
                Page objPage = AllFunctions.SetRights(name);

                ViewBag.edit = objPage.edit;
                ViewBag.view = objPage.view;
                ViewBag.delete = objPage.delete;
                ViewBag.print = objPage.print;
                ViewBag.addnew = objPage.addnew;

                ViewBag.helpModalHeader = objPage.PageName;
                ViewBag.helpModalBody = objPage.PageHelp;
            }
            catch { }
        }

        #region Lead CRUD

        /// <summary>
        /// List Page of Lead
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List(int? PI, int? PS,int? ViewType,string EditId)
        {
            #region Active Link
            ViewBag.Lead = "active";
            #endregion

            #region Rights
            try
            {
                AssignRights(PageConstant.GetInstance().Transactions_Lead);
                if (ViewBag.view != true)
                {
                    TempData["type"] = AppConstant.AlertInfoType;
                    TempData["msg"] = AppConstant.AlertRightsWarning;
                    return RedirectToAction("Dashboard", "Home", new { Area = "Transactions" });
                }
                ViewBag.TeamRights = AllFunctions.SetRights(PageConstant.GetInstance().Lead_Team_Rights).view;
                ViewBag.AllRights = AllFunctions.SetRights(PageConstant.GetInstance().Lead_All_Rights).view;
                ViewBag.edit = AllFunctions.SetRights(PageConstant.GetInstance().Lead_Team_Rights).edit;
                ViewBag.delete = AllFunctions.SetRights(PageConstant.GetInstance().Lead_Team_Rights).delete;
                ViewBag.view = AllFunctions.SetRights(PageConstant.GetInstance().Lead_Team_Rights).view;
                ViewBag.addnew = AllFunctions.SetRights(PageConstant.GetInstance().Lead_Team_Rights).addnew;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            #endregion

            #region Default
            var obj = new LeadUI();

            var date1 = AllFunctions.GetDateTime().AddMonths(-2);
            var date2 = AllFunctions.GetDateTime();
            try
            {
                string fromdateStr = date1.ToString(AppConstant.DateFormat2);
                string todateStr = date2.ToString(AppConstant.DateFormat2);
                string FromToDate = fromdateStr + " - " + todateStr;
                ViewBag.FromToDate = FromToDate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            //TO DO
            #region Approval Status

            ViewBag.ApprovalCount = 0;//ApprovalUtility.GetApprovalSettings(PageConstant.GetInstance().CRM_Lead_Alpha).Count();

            #endregion

            #endregion

            #region GetEmployeeData_Organization from API

            var listEmployee = new List<EmployeeListOrganizationWise>();
            try
            {
                var baseResponse = JsonConvert.DeserializeObject<Response<DataSet>>(
                               await _apiManager.CallGetMethod("lead/GetEmployeeData_Organization"));

                if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS))
                {
                    var dtemp = baseResponse.Data.Tables[0];
                    int i = 0;

                    foreach (DataRow row in dtemp.Rows)
                    {
                        var FullPathIds = "";
                        listEmployee.Add(new EmployeeListOrganizationWise
                        {
                            EmployeeId = Convert.ToInt32(row["EmployeeId"]),
                            Depth = Convert.ToInt32(row["Depth"]),
                            EmployeeName = Convert.ToString(row["EmployeeName"]),
                            DesignationName = Convert.ToString(row["DesignationName"]),
                            ReportingToId = Convert.ToInt32(row["ReportTo"]),
                            ReportingToName = Convert.ToString(row["ReportingToName"]),
                            ProfileImage = AppConstant.EmployeeGetPath + "/" + Convert.ToString(row["EmployeeId"]) + "/" + Convert.ToString(row["ProfileImage"]),
                            DepthClass = FullPathIds,
                        });
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            //ViewBag.listEmployee = JsonConvert.SerializeObject(listEmployee);
            obj.ReportingStructureList = listEmployee;

            #endregion

            #region Bind Lists
            ViewBag.EmptyList = CommonMaster.BindEmptyList();
            ViewBag.AssignToId = AllFunctions.getUserID();
            ViewBag.AssignToName = AllFunctions.getUserName();
            ViewBag.PI = PI;
            ViewBag.PS = PS;
            ViewBag.ViewType = ViewType;
            ViewBag.EditId = EditId;
            #endregion
            try
            {
                var baseResponse = JsonConvert.DeserializeObject<Response<MatchingCriteria>>(
                        await _apiManager.CallPostMethod("masters/matchingcriteria/getmatchingcriteriadatabycid/", null));

                ViewBag.IsCategory = baseResponse.Data.IsCategory;
                ViewBag.IsSubCategory = baseResponse.Data.IsSubCategory;
                ViewBag.IsArea = baseResponse.Data.IsArea;
                ViewBag.IsProject = baseResponse.Data.IsProject;
                ViewBag.IsBudget = baseResponse.Data.IsBudget;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return View(obj);
        }

        /// <summary>
        /// Get List of Lead
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //public async Task<JsonResult> GetList()//for jquery datatable 
        public async Task<JsonResult> GetList(int order, string orderDir, int startRec, int pageSize, string Search,
           int AssignTypeId, string MainAssignToIds, int? IsBookMarked,
           //string AssignedIds, string AssigneeIds, 
           string RelevanceIds, string FromDate, string ToDate, string StatusIds,
           string LeadForIds, string CategoryIds, string SubcategoryIds,
           string BHKIds, string FloorIds, string LeadZoneIds, string FurnishStatusIds,
           string CityIds, string AreaIds,
           string PriceFrom, string PriceTo, string TotalAreaFrom, string TotalAreaTo, string AssignToIds)
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

                #region Initialize Input parameter Request
                var objRequest = new LeadListRequest();
                objRequest.PageIndex = startRec;
                objRequest.PageSize = pageSize;
                objRequest.SortDir = orderDir;
                objRequest.SortCol = Convert.ToInt32(order);
                //objRequest.IsActive = IsActive;
                //if (Search == null)
                //    objRequest.Keyword = "";
                //else
                objRequest.Keyword = Search;
                objRequest.FromDate = FromDate;
                objRequest.ToDate = ToDate;
                objRequest.Cid = AllFunctions.getCompanyID();
                objRequest.Bid = AllFunctions.getBranchID();

                objRequest.AssignTypeId = AssignTypeId;
                objRequest.MainAssignToIds = MainAssignToIds;
                objRequest.CurrentUserId = AllFunctions.getUserID();
                //objRequest.AssignedIds = AssignedIds;
                //objRequest.AssigneeIds = AssigneeIds;
                if (IsBookMarked > 0)
                {
                    objRequest.IsBookMarked = Convert.ToBoolean(IsBookMarked);
                }

                objRequest.RelevanceIds = RelevanceIds;
                objRequest.StatusIds = StatusIds;
                objRequest.PropertyForIds = LeadForIds;
                objRequest.CategoryIds = CategoryIds;
                objRequest.SubcategoryIds = SubcategoryIds;
                objRequest.BHKIds = BHKIds;
                objRequest.FloorIds = FloorIds;
                objRequest.PropertyZoneIds = LeadZoneIds;
                objRequest.FurnishStatusIds = FurnishStatusIds;
                objRequest.CityIds = CityIds;
                objRequest.AreaIds = AreaIds;
                objRequest.PriceFrom = PriceFrom;
                objRequest.PriceTo = PriceTo;
                objRequest.TotalAreaFrom = TotalAreaFrom;
                objRequest.TotalAreaTo = TotalAreaTo;
                objRequest.AssignToIds = AssignToIds;
                #endregion 

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<List<LeadListResponse>>>(
                    await _apiManager.CallPostMethod("lead/list", objRequest));

                #endregion

                #region Return JSON
                response = Json(new
                {
                    draw = 0,
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
        /// View form of Lead
        /// </summary>
        /// <param name="EncryptedId"></param>
        /// <returns></returns>
        public async Task<IActionResult> View_New(string EncryptedId, int? PI, int? PS, int? ViewType)
        {
            ViewBag.Lead = "active";

            var obj = new LeadUI();
            try
            {
                if (!string.IsNullOrEmpty(EncryptedId))
                {
                    EncryptedId = AppCommon.DecryptString(Convert.ToString(EncryptedId));
                    obj.Id = Convert.ToInt32(EncryptedId);
                    if (obj.Id > 0)
                    {
                        var baseResponse = JsonConvert.DeserializeObject<Response<Lead>>(
                            await _apiManager.CallPostMethod("lead/edit/" + obj.Id, null));

                        obj = _mapper.Map<LeadUI>(baseResponse.Data);
                        obj.PI = PI;
                        obj.PS = PS;
                        obj.ViewType = ViewType;
                        if (baseResponse.Status.Equals((int)AppConstant.APIStatus.FAILURE))
                        {
                            TempData["type"] = AppConstant.AlertErrorType;
                            TempData["msg"] = baseResponse.Message;
                            return RedirectToAction("List", "Lead", new { Area = "Transactions" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return View(obj);
        }

        /// <summary>
        /// New / Edit form of Lead
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="EncryptedId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Form(string mode, string EncryptedId,int? PI, int? PS,int? ViewType,int? IsOpprtunity)
        {
            #region Active Link
            ViewBag.Lead = "active";
            #endregion

            #region Default

            var obj = new LeadUI();
            obj.IsAllowEdit = true;
            obj.PI = PI;
            obj.PS = PS;
            obj.ViewType = ViewType;
            obj.IsOpprtunity = IsOpprtunity;
            var date = AllFunctions.GetDateTime();
            try
            {
                string Date = date.ToString(AppConstant.DateFormat);
                obj.LeadDate = Date;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            #region set autono
            try
            {
                var autoNoModel = await CommonMaster.GetAutoNo(DBConstant.Table_Lead);
                obj.AutoNo = autoNoModel.AutoNo;
                obj.No = autoNoModel.No;
                obj.LeadNo = autoNoModel.No;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            #endregion set autono            

            #endregion

            #region Bind Lists
            Dictionary<int?, string> objDict = new Dictionary<int?, string>();

            //obj.StateId = objDict;
            //obj.CityId = objDict;
            //obj.AreaIds = objDict;
            obj.PreferredProjectIds = objDict;
            obj.ProjectTypeIds = objDict;
            obj.LeadSubsourceId = objDict;
            //obj.AvailableForIds = objDict;
            obj.BHKs = objDict;

            Dictionary<int?, string> objDictAssign = new Dictionary<int?, string>();
            objDictAssign.Add(AllFunctions.getUserID(), AllFunctions.getUserName());
            obj.AssignTo = objDictAssign;

            ViewBag.EmptyList = CommonMaster.BindEmptyList();
            #endregion

            #region Rights
            AssignRights(PageConstant.GetInstance().Transactions_Lead);
            if (string.IsNullOrEmpty(EncryptedId))
            {
                if (ViewBag.addnew != true)
                {
                    TempData["type"] = AppConstant.AlertInfoType;
                    TempData["msg"] = AppConstant.AlertRightsWarning;
                    return RedirectToAction("Dashboard", "Home", new { Area = "Transactions" });
                }
            }
            if (!string.IsNullOrEmpty(EncryptedId) && !string.IsNullOrEmpty(mode))
            {
                ///If mid is not null and Operation is edit then check edit rights
                if (mode.Equals("edit"))
                {
                    if (ViewBag.edit != true)
                    {
                        TempData["type"] = AppConstant.AlertInfoType;
                        TempData["msg"] = AppConstant.AlertRightsWarning;
                        return RedirectToAction("Dashboard", "Home", new { Area = "Transactions" });
                    }
                }
                ///If mid is not null and Operation is view then check view rights
                else
                {
                    obj.IsAllowEdit = false;
                    if (ViewBag.view != true)
                    {
                        TempData["type"] = AppConstant.AlertInfoType;
                        TempData["msg"] = AppConstant.AlertRightsWarning;
                        return RedirectToAction("Dashboard", "Home", new { Area = "Transactions" });
                    }
                }
            }
            else
            {
                if (ViewBag.view != true)
                {
                    TempData["type"] = AppConstant.AlertInfoType;
                    TempData["msg"] = AppConstant.AlertRightsWarning;
                    return RedirectToAction("Dashboard", "Home", new { Area = "Transactions" });
                }
            }

            #endregion            

            #region Get Lead

            try
            {
                if (!string.IsNullOrEmpty(EncryptedId))
                {
                    EncryptedId = AppCommon.DecryptString(Convert.ToString(EncryptedId));
                    obj.Id = Convert.ToInt32(EncryptedId);
                    if (obj.Id > 0)
                    {
                        var baseResponse = JsonConvert.DeserializeObject<Response<Lead>>(
                            await _apiManager.CallPostMethod("lead/edit/" + obj.Id, null));

                        obj = _mapper.Map<LeadUI>(baseResponse.Data);
                        if (obj.AssignTo.Count() == 0)
                        {
                            Dictionary<int?, string> objDictAssignTo = new Dictionary<int?, string>();
                            objDictAssignTo.Add(AllFunctions.getUserID(), AllFunctions.getUserName());
                            obj.AssignTo = objDictAssignTo;
                        }
                        if (mode.Equals("edit"))
                        {
                            obj.IsAllowEdit = true;
                            obj.PI = PI;
                            obj.PS = PS;
                            obj.ViewType = ViewType;
                            obj.IsOpprtunity = IsOpprtunity;
                        }
                        else
                        {
                            obj.IsAllowEdit = false;
                        }
                        if (baseResponse.Status.Equals((int)AppConstant.APIStatus.FAILURE))
                        {
                            TempData["type"] = AppConstant.AlertErrorType;
                            TempData["msg"] = baseResponse.Message;
                            return RedirectToAction("Form", "Lead", new { Area = "Transactions" });
                        }

                    }
                }

                #region for get default selected dropdown values
                else
                {
                    // for get default selected dropdown values
                    #region LeadFor Dropdown
                    var LeadFor = new PropertyForListRequest();
                    LeadFor.Cid = AllFunctions.getCompanyID();
                    LeadFor.Bid = AllFunctions.getBranchID();
                    LeadFor.SortCol = 0;
                    LeadFor.SortDir = "asc";
                    LeadFor.PageIndex = 0;
                    LeadFor.PageSize = 10;
                    LeadFor.IsActive = null;
                    LeadFor.Keyword = "";
                    LeadFor.IsDefault = true;
                    var DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                     await _apiManager.CallPostMethod("common/GetPropertyForSearchList", LeadFor));
                    obj.PropertyForId = DefaultData.Data;

                    #endregion

                    #region Category Dropdown
                    var requestCat = new PropertyCategoryListRequest();
                    requestCat.Cid = AllFunctions.getCompanyID();
                    requestCat.Bid = AllFunctions.getBranchID();
                    requestCat.SortCol = 0;
                    requestCat.SortDir = "asc";
                    requestCat.PageIndex = 0;
                    requestCat.PageSize = 10;
                    requestCat.IsActive = null;
                    requestCat.Keyword = "";
                    requestCat.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                     await _apiManager.CallPostMethod("common/GetCategorySearchList", requestCat));
                    obj.PropertyCategoryIds = DefaultData.Data;

                    #endregion

                    #region PropertySubcategory Dropdown
                    var PropertySubcategory = new PropertySubcategoryListRequest();
                    PropertySubcategory.Cid = AllFunctions.getCompanyID();
                    PropertySubcategory.Bid = AllFunctions.getBranchID();
                    PropertySubcategory.SortCol = 0;
                    PropertySubcategory.SortDir = "asc";
                    PropertySubcategory.PageIndex = 0;
                    PropertySubcategory.PageSize = 10;
                    PropertySubcategory.IsActive = null;
                    PropertySubcategory.Keyword = "";
                    PropertySubcategory.IsDefault = true;
                    PropertySubcategory.CategoryId = obj.PropertyCategoryIds.Count > 0 ? Convert.ToInt32(obj.PropertyCategoryIds.Keys.First()) : 0;

                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                     await _apiManager.CallPostMethod("common/GetPropertySubcategorySearchList", PropertySubcategory));
                    obj.PropertySubcategoryIds = DefaultData.Data;

                    #endregion

                    #region PropertyZone Dropdown
                    var PropertyZone = new PropertyZoneListRequest();
                    PropertyZone.Cid = AllFunctions.getCompanyID();
                    PropertyZone.Bid = AllFunctions.getBranchID();
                    PropertyZone.SortCol = 0;
                    PropertyZone.SortDir = "asc";
                    PropertyZone.PageIndex = 0;
                    PropertyZone.PageSize = 10;
                    PropertyZone.IsActive = null;
                    PropertyZone.Keyword = "";
                    PropertyZone.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                     await _apiManager.CallPostMethod("common/GetPropertyZoneSearchList", PropertyZone));
                    obj.PropertyZoneIds = DefaultData.Data;

                    #endregion

                    #region Salutation Dropdown
                    var Salutation = new SalutationListRequest();
                    Salutation.Cid = AllFunctions.getCompanyID();
                    Salutation.Bid = AllFunctions.getBranchID();
                    Salutation.SortCol = 0;
                    Salutation.SortDir = "asc";
                    Salutation.PageIndex = 0;
                    Salutation.PageSize = 10;
                    Salutation.IsActive = null;
                    Salutation.Keyword = "";
                    Salutation.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                    await _apiManager.CallPostMethod("common/GetSalutationSearchList", Salutation));
                    obj.Salutation = DefaultData.Data;

                    #endregion

                    #region PropertyBHK Dropdown
                    var PropertyBHK = new PropertyBHKListRequest();
                    PropertyBHK.Cid = AllFunctions.getCompanyID();
                    PropertyBHK.Bid = AllFunctions.getBranchID();
                    PropertyBHK.SortCol = 0;
                    PropertyBHK.SortDir = "asc";
                    PropertyBHK.PageIndex = 0;
                    PropertyBHK.PageSize = 10;
                    PropertyBHK.IsActive = null;
                    PropertyBHK.Keyword = "";
                    PropertyBHK.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                     await _apiManager.CallPostMethod("common/GetPropertyBHKSearchList", PropertyBHK));
                    obj.BHKs = DefaultData.Data;

                    #endregion                                        

                    #region Floor Dropdown
                    var Floor = new PropertyFloorListRequest();
                    Floor.Cid = AllFunctions.getCompanyID();
                    Floor.Bid = AllFunctions.getBranchID();
                    Floor.SortCol = 0;
                    Floor.SortDir = "asc";
                    Floor.PageIndex = 0;
                    Floor.PageSize = 10;
                    Floor.IsActive = null;
                    Floor.Keyword = "";
                    Floor.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                    await _apiManager.CallPostMethod("common/GetFloorSearchList", Floor));
                    obj.FloorPreferenceIds = DefaultData.Data;

                    #endregion

                    #region PropertyFurnishStatus Dropdown
                    var PropertyFurnishStatus = new PropertyFurnishStatusListRequest();
                    PropertyFurnishStatus.Cid = AllFunctions.getCompanyID();
                    PropertyFurnishStatus.Bid = AllFunctions.getBranchID();
                    PropertyFurnishStatus.SortCol = 0;
                    PropertyFurnishStatus.SortDir = "asc";
                    PropertyFurnishStatus.PageIndex = 0;
                    PropertyFurnishStatus.PageSize = 10;
                    PropertyFurnishStatus.IsActive = null;
                    PropertyFurnishStatus.Keyword = "";
                    PropertyFurnishStatus.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                     await _apiManager.CallPostMethod("common/GetPropertyFurnishStatusSearchList", PropertyFurnishStatus));
                    obj.FurnishStatusIds = DefaultData.Data;

                    #endregion

                    #region DoorFacingDirection Dropdown
                    var PropertyScore = new PropertyScoreListRequest();
                    PropertyScore.Cid = AllFunctions.getCompanyID();
                    PropertyScore.Bid = AllFunctions.getBranchID();
                    PropertyScore.SortCol = 0;
                    PropertyScore.SortDir = "asc";
                    PropertyScore.PageIndex = 0;
                    PropertyScore.PageSize = 10;
                    PropertyScore.IsActive = null;
                    PropertyScore.Keyword = "";
                    PropertyScore.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                    await _apiManager.CallPostMethod("common/GetDoorFacingDirectionSearchList", PropertyScore));
                    obj.DoorFacingDirectionId = DefaultData.Data;

                    #endregion

                    #region PropertyAvailableFor Dropdown
                    var PropertyAvailableFor = new PropertyAvailableForListRequest();
                    PropertyAvailableFor.Cid = AllFunctions.getCompanyID();
                    PropertyAvailableFor.Bid = AllFunctions.getBranchID();
                    PropertyAvailableFor.SortCol = 0;
                    PropertyAvailableFor.SortDir = "asc";
                    PropertyAvailableFor.PageIndex = 0;
                    PropertyAvailableFor.PageSize = 10;
                    PropertyAvailableFor.IsActive = null;
                    PropertyAvailableFor.Keyword = "";
                    PropertyAvailableFor.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                     await _apiManager.CallPostMethod("common/GetPropertyAvailableForSearchList", PropertyAvailableFor));
                    obj.AvailableForIds = DefaultData.Data;

                    #endregion

                    #region ProjectType Dropdown
                    var ProjectType = new ProjectTypeListRequest();
                    ProjectType.Cid = AllFunctions.getCompanyID();
                    ProjectType.Bid = AllFunctions.getBranchID();
                    ProjectType.SortCol = 0;
                    ProjectType.SortDir = "asc";
                    ProjectType.PageIndex = 0;
                    ProjectType.PageSize = 10;
                    ProjectType.IsActive = null;
                    ProjectType.Keyword = "";
                    ProjectType.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                    await _apiManager.CallPostMethod("common/GetProjectTypeSearchList", ProjectType));
                    obj.ProjectTypeIds = DefaultData.Data;

                    #endregion

                    #region Unit Dropdown
                    var Unit = new UnitListRequest();
                    Unit.Cid = AllFunctions.getCompanyID();
                    Unit.Bid = AllFunctions.getBranchID();
                    Unit.SortCol = 0;
                    Unit.SortDir = "asc";
                    Unit.PageIndex = 0;
                    Unit.PageSize = 10;
                    Unit.IsActive = null;
                    Unit.Keyword = "";
                    Unit.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                    await _apiManager.CallPostMethod("common/GetUnitSearchList", Unit));
                    obj.SuperBuildupAreaUnitId = DefaultData.Data;
                    obj.CarpetAreaUnitId = DefaultData.Data;
                    #endregion                                        

                    #region LeadSource Dropdown
                    var LeadSource = new LeadSourceListRequest();
                    LeadSource.Cid = AllFunctions.getCompanyID();
                    LeadSource.Bid = AllFunctions.getBranchID();
                    LeadSource.SortCol = 0;
                    LeadSource.SortDir = "asc";
                    LeadSource.PageIndex = 1;
                    LeadSource.PageSize = 10;
                    LeadSource.IsActive = null;
                    LeadSource.Keyword = "";
                    LeadSource.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                     await _apiManager.CallPostMethod("common/GetLeadSourceSearchList", LeadSource));
                    obj.LeadSourceId = DefaultData.Data;

                    #endregion

                    #region LeadSubSource Dropdown
                    var LeadSubSource = new LeadSubsourceListRequest();
                    LeadSubSource.Cid = AllFunctions.getCompanyID();
                    LeadSubSource.Bid = AllFunctions.getBranchID();
                    LeadSubSource.SortCol = 0;
                    LeadSubSource.SortDir = "asc";
                    LeadSubSource.PageIndex = 1;
                    LeadSubSource.PageSize = 10;
                    LeadSubSource.IsActive = null;
                    LeadSubSource.Keyword = "";
                    LeadSubSource.IsDefault = true;
                    LeadSubSource.LeadSourceId = obj.LeadSourceId.Count > 0 ? Convert.ToInt32(obj.LeadSourceId.Keys.First()) : 0;

                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                     await _apiManager.CallPostMethod("common/GetLeadSubsourceSearchList", LeadSubSource));
                    obj.LeadSubsourceId = DefaultData.Data;

                    #endregion

                    #region LeadStatus Dropdown
                    var LeadStatus = new LeadStatusListRequest();
                    LeadStatus.Cid = AllFunctions.getCompanyID();
                    LeadStatus.Bid = AllFunctions.getBranchID();
                    LeadStatus.SortCol = 0;
                    LeadStatus.SortDir = "asc";
                    LeadStatus.PageIndex = 1;
                    LeadStatus.PageSize = 10;
                    LeadStatus.IsActive = null;
                    LeadStatus.Keyword = "";
                    LeadStatus.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                     await _apiManager.CallPostMethod("common/GetLeadStatusSearchList", LeadStatus));
                    obj.LeadStageId = DefaultData.Data;

                    #endregion

                    #region LeadPriority Dropdown
                    var LeadPriority = new LeadPriorityListRequest();
                    LeadPriority.Cid = AllFunctions.getCompanyID();
                    LeadPriority.Bid = AllFunctions.getBranchID();
                    LeadPriority.SortCol = 0;
                    LeadPriority.SortDir = "asc";
                    LeadPriority.PageIndex = 1;
                    LeadPriority.PageSize = 10;
                    LeadPriority.IsActive = null;
                    LeadPriority.Keyword = "";
                    LeadPriority.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                     await _apiManager.CallPostMethod("common/GetLeadPrioritySearchList", LeadPriority));
                    obj.LeadPriorityId = DefaultData.Data;

                    #endregion

                    #region LeadScore Dropdown
                    var LeadScore = new LeadScoreListRequest();
                    LeadScore.Cid = AllFunctions.getCompanyID();
                    LeadScore.Bid = AllFunctions.getBranchID();
                    LeadScore.SortCol = 0;
                    LeadScore.SortDir = "asc";
                    LeadScore.PageIndex = 1;
                    LeadScore.PageSize = 10;
                    LeadScore.IsActive = null;
                    LeadScore.Keyword = "";
                    LeadScore.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                    await _apiManager.CallPostMethod("common/GetLeadScoreSearchList", LeadScore));
                    obj.LeadScoreId = DefaultData.Data;

                    #endregion

                    #region LeadLostReason Dropdown
                    //var LeadLostReason = new LostReasonListRequest();
                    //LeadLostReason.Cid = AllFunctions.getCompanyID();
                    //LeadLostReason.Bid = AllFunctions.getBranchID();
                    //LeadLostReason.SortCol = 0;
                    //LeadLostReason.SortDir = "asc";
                    //LeadLostReason.PageIndex = 1;
                    //LeadLostReason.PageSize = 10;
                    //LeadLostReason.IsActive = null;
                    //LeadLostReason.Keyword = "";
                    //LeadLostReason.IsDefault = true;
                    //DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                    //await _apiManager.CallPostMethod("common/GetLostReasonSearchList", LeadLostReason));
                    //obj.LeadLostReasonId = DefaultData.Data;

                    #endregion

                    #region Country Dropdown
                    var request = new CountryListRequest();
                    request.Cid = AllFunctions.getCompanyID();
                    request.Bid = AllFunctions.getBranchID();
                    request.SortCol = 0;
                    request.SortDir = "asc";
                    request.PageIndex = 0;
                    request.PageSize = 10;
                    request.IsActive = null;
                    request.Keyword = "";

                    request.IsDefault = true;
                    DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                     await _apiManager.CallPostMethod("common/GetCountrySearchList", request));
                    obj.CountryId = DefaultData.Data;

                    #endregion

                    #region State Dropdown
                    if (obj.CountryId.Count > 0)
                    {
                        var requestState = new StateListRequest();
                        requestState.Cid = AllFunctions.getCompanyID();
                        requestState.Bid = AllFunctions.getBranchID();
                        requestState.SortCol = 0;
                        requestState.SortDir = "asc";
                        requestState.PageIndex = 0;
                        requestState.PageSize = 10;
                        requestState.IsActive = null;
                        requestState.Keyword = "";
                        requestState.CountryId = obj.CountryId.FirstOrDefault().Key;

                        requestState.IsDefault = true;
                        DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                         await _apiManager.CallPostMethod("common/GetStateSearchList", requestState));
                        obj.StateId = DefaultData.Data;
                    }
                    else
                    {
                        obj.StateId = objDict;
                    }
                    #endregion

                    #region City Dropdown
                    if (obj.StateId.Count > 0)
                    {
                        var requestCity = new CityListRequest();
                        requestCity.Cid = AllFunctions.getCompanyID();
                        requestCity.Bid = AllFunctions.getBranchID();
                        requestCity.SortCol = 0;
                        requestCity.SortDir = "asc";
                        requestCity.PageIndex = 0;
                        requestCity.PageSize = 10;
                        requestCity.IsActive = null;
                        requestCity.Keyword = "";
                        requestCity.CountryId = obj.CountryId.FirstOrDefault().Key;
                        requestCity.StateId = obj.StateId.FirstOrDefault().Key;

                        requestCity.IsDefault = true;
                        DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                         await _apiManager.CallPostMethod("common/GetCitySearchList", requestCity));
                        obj.CityId = DefaultData.Data;
                    }
                    else
                    {
                        obj.CityId = objDict;
                    }
                    #endregion   

                    #region Area Dropdown
                    if (obj.CityId.Count > 0)
                    {
                        var requestArea = new AreaListRequest();
                        requestArea.Cid = AllFunctions.getCompanyID();
                        requestArea.Bid = AllFunctions.getBranchID();
                        requestArea.SortCol = 0;
                        requestArea.SortDir = "asc";
                        requestArea.PageIndex = 0;
                        requestArea.PageSize = 10;
                        requestArea.IsActive = null;
                        requestArea.Keyword = "";
                        requestArea.CountryId = obj.CountryId.FirstOrDefault().Key;
                        requestArea.StateId = obj.StateId.FirstOrDefault().Key;
                        requestArea.CityId = obj.CityId.FirstOrDefault().Key;

                        requestArea.IsDefault = true;
                        DefaultData = JsonConvert.DeserializeObject<Response<Dictionary<int?, string>>>(
                         await _apiManager.CallPostMethod("common/GetAreaSearchList", requestArea));
                        obj.AreaIds = DefaultData.Data;
                    }
                    else
                    {
                        obj.AreaIds = objDict;
                    }
                    #endregion   

                }
                #endregion for get default selected dropdown values

            }
            catch (Exception ex)
            {
                TempData["type"] = AppConstant.AlertErrorType;
                TempData["msg"] = AppConstant.InternalServerError;
                _logger.LogError(ex, ex.Message);
            }
            #endregion

            return View(obj);
        }
        /// <summary>
        /// Insert / Update Lead
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Form(LeadUI obj, string button)
        {
            var messageType = AppConstantUI.AlertErrorType;
            var message = AppConstantUI.DataUpdationErrorMessage;
            var _RefTableMainId = 0;
            if (obj.PropertyCategoryNameHidden.ToLower() != "residential")
            {
                //ModelState.Remove("AvailableForIdUI");
                ModelState.Remove("BHKIdsUI");
                ModelState.Remove("QuickBHKIdsUI");
            }
            if (button == "Quick" || button == "QuickNew")
            {
                ModelState.Remove("LeadPriorityIdUI");
                ModelState.Remove("LeadStageIdUI");
            }
            try
            {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

                if (ModelState.IsValid)
                {
                    var datespl = obj.PossessionDateFrom.Split('-');
                    obj.PossessionDateFrom = datespl[0].Trim();
                    obj.PossessionDateTo = datespl[1].Trim();

                    var objmapper = _mapper.Map<LeadUI, Lead>(obj);

                    #region Ins/upd

                    if (objmapper.Id > 0)
                    {
                        var baseResponse = JsonConvert.DeserializeObject<Response<int>>(
                                await _apiManager.CallPutMethod("lead/update/" + objmapper.Id, objmapper));

                        if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                            && baseResponse.Data > 0)
                        {
                            _RefTableMainId = Convert.ToInt32(baseResponse.Data);

                            messageType = AppConstantUI.AlertSuccessType;
                            message = AppConstantUI.UpdateMessage;
                        }
                        else
                        {
                            messageType = AppConstantUI.AlertErrorType;
                            message = baseResponse.Message;
                        }
                    }
                    else
                    {
                        var baseResponse = JsonConvert.DeserializeObject<Response<int>>(
                                await _apiManager.CallPostMethod("lead/insert", objmapper));

                        if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                             && baseResponse.Data > 0)
                        {
                            messageType = AppConstantUI.AlertSuccessType;
                            message = AppConstantUI.InsertMessage;

                            _RefTableMainId = Convert.ToInt32(baseResponse.Data);

                            try
                            {
                                var requestNotification = new NotificationToolRequest();
                                requestNotification.Refid = _RefTableMainId.ToString();
                                requestNotification.Reftable = "Lead";
                                requestNotification.Command = "Add";
                                requestNotification.UserId = objmapper.AssignTo.GetNullableIntFromNullableDictionary();

                                var baseResponseNotification = JsonConvert.DeserializeObject<Response<bool>>(
                                          await _apiManager.CallPostMethod("common/AddNotification", requestNotification));

                                //if (baseResponseNotification.Status.Equals((int)AppConstant.APIStatus.SUCCESS))
                                //{
                                //messageType = AppConstantUI.AlertSuccessType;
                                //message = AppConstantUI.InsertMessage;
                                //}
                                //else
                                //{
                                //    message = "Notification not sent!";
                                //    messageType = AppConstant.AlertErrorType;
                                //}
                            }
                            catch (Exception e)
                            {
                                message = "Something went wrong! Please try again later";
                                messageType = AppConstantUI.AlertErrorType;
                                _logger.LogError(e, e.Message);
                            }
                        }
                        else
                        {
                            messageType = AppConstantUI.AlertErrorType;
                            message = baseResponse.Message;
                        }
                    }
                    #endregion Ins/upd                    
                }
                else
                {
                    //var ErrorField = errors[0].Key;
                    //LeadUI LeadUI = new LeadUI();

                    var AllFields = "";
                    for (var i= 0; i < errors.Length; i++)
                    {
                       var str =  errors[i].Key;
                        if (str.Contains("IdsUI"))
                        {
                            str = str.Replace("IdsUI", String.Empty);
                        }
                        else if (str.Contains("IdUI"))
                        {
                            str = str.Replace("IdUI", String.Empty);
                        }
                        else if (str.Contains("Id"))
                        {
                            str = str.Replace("Id", String.Empty);
                        }
                        else if (str.Contains("UI"))
                        {
                            str = str.Replace("UI", String.Empty);
                        }
                        if (AllFields == "")
                        {
                            AllFields = str;
                        }
                        else
                        {
                            AllFields = AllFields + ", " + str;
                        }
                    }
                    messageType = AppConstantUI.AlertErrorType;
                    message = "Please Enter/Select "+ AllFields + " Fields.";
                    TempData["type"] = messageType;
                    TempData["msg"] = message;
                    if (obj.Id > 0)
                    {
                        return RedirectToAction("Form", "Lead", new { Area = "Transactions", mode = "edit", EncryptedId = AppCommon.EncryptString(Convert.ToString(obj.Id)) });
                    }
                    else
                    {
                        return RedirectToAction("Form", "Lead", new { Area = "Transactions" });
                    }
                }
            }
            catch (Exception ex)
            {
                messageType = AppConstantUI.AlertErrorType;
                message = AppConstantUI.ExceptionMessage;
                _logger.LogError(ex, ex.Message);
            }

            TempData["type"] = messageType;
            TempData["msg"] = message;

            if (button.ToLower() == "new" || button.ToLower() == "quicknew")
            {
                return RedirectToAction("Form", "Lead", new { Area = "Transactions" });
            }
            if(obj.IsOpprtunity == 1)
            {
                if (obj.PI >= 0 && obj.PS > 0)
                {
                    var EncryptedId = AppCommon.EncryptString(Convert.ToString(obj.Id));
                    return RedirectToAction("List", "Opportunity", new { Area = "Transactions", PI = obj.PI, PS = obj.PS, ViewType = obj.ViewType, EditId = EncryptedId });
                }
                else
                {
                    return RedirectToAction("List", "Opportunity", new { Area = "Transactions" });
                }
            }
            else
            {
                if (obj.PI >= 0 && obj.PS > 0)
                {
                    var EncryptedId = AppCommon.EncryptString(Convert.ToString(obj.Id));
                    return RedirectToAction("List", "Lead", new { Area = "Transactions", PI = obj.PI, PS = obj.PS, ViewType = obj.ViewType, EditId = EncryptedId });
                }
                else
                {
                    return RedirectToAction("List", "Lead", new { Area = "Transactions" });
                }
            }
            
            
        }
        /// <summary>
        /// Delete Lead by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> Delete(string id)
        {
            var messageType = AppConstantUI.AlertErrorType;
            var message = AppConstantUI.ExceptionMessage;
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (Convert.ToInt32(id) > 0)
                    {
                        var _RefTableMainId = Convert.ToInt32(id);

                        var baseResponse = JsonConvert.DeserializeObject<Response<bool>>(
                               await _apiManager.CallDeleteMethod("lead/delete/" + id));

                        if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS) && baseResponse.Data)
                        {
                            messageType = AppConstantUI.AlertSuccessType;
                            message = AppConstantUI.DeleteMessage;
                            try
                            {
                                var requestNotification = new NotificationToolRequest();
                                requestNotification.Refid = id;
                                requestNotification.Reftable = "Lead";
                                requestNotification.Command = "Delete";
                                requestNotification.UserId = 0;

                                var baseResponseNotification = JsonConvert.DeserializeObject<Response<bool>>(
                                          await _apiManager.CallPostMethod("common/AddNotification", requestNotification));

                                //if (baseResponseNotification.Status.Equals((int)AppConstant.APIStatus.SUCCESS))
                                //{
                                //messageType = AppConstantUI.AlertSuccessType;
                                //message = AppConstantUI.DeleteMessage;
                                //}
                                //else
                                //{
                                //    message = "Notification not sent!";
                                //    messageType = AppConstant.AlertErrorType;
                                //}
                            }
                            catch (Exception e)
                            {
                                message = "Something went wrong! Please try again later";
                                messageType = AppConstantUI.AlertErrorType;
                                _logger.LogError(e, e.Message);
                            }

                        }
                        else
                        {
                            messageType = AppConstantUI.AlertErrorType;
                            message = baseResponse.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return Json(new { message, messageType });
        }

        /// <summary>
        /// Delete Lead by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> IsDelete(string id)
        {
            var messageType = AppConstantUI.AlertErrorType;
            var message = AppConstantUI.ExceptionMessage;
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (Convert.ToInt32(id) > 0)
                    {
                        var _RefTableMainId = Convert.ToInt32(id);

                        var baseResponse = JsonConvert.DeserializeObject<Response<bool>>(
                               await _apiManager.CallDeleteMethod("lead/isdelete/" + id));

                        if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS) && baseResponse.Data)
                        {
                            messageType = AppConstantUI.AlertSuccessType;
                            message = "";
                        }
                        else
                        {
                            messageType = AppConstantUI.AlertErrorType;
                            message = baseResponse.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return Json(new { message, messageType });
        }
        /// <summary>
        /// Check for duplicate Lead Id
        /// </summary>
        /// <param name="LeadNo"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> IsDuplicate(string LeadNo, int Id)
        {
            bool IsDuplicate = false;
            try
            {
                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("Id", (object)Id));
                request.Add(Tuple.Create("LeadNo", (object)LeadNo.Trim()));

                var baseResponse = JsonConvert.DeserializeObject<Response<bool>>(
                               await _apiManager.CallGetMethod("lead/checkduplicate", request));

                if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS))
                {
                    IsDuplicate = baseResponse.Data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Json(!IsDuplicate);
        }

        #endregion Lead CRUD

        #region Lead Ajax
        /// <summary>
        /// Update SpecialNotes
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="SpecialNotes"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> UpdateSpecialNote(int Id, string SpecialNotes)
        {
            var messageType = AppConstantUI.AlertErrorType;
            var message = AppConstantUI.ExceptionMessage;

            try
            {
                if (Id > 0 && !string.IsNullOrEmpty(SpecialNotes))
                {
                    var request = new List<Tuple<string, object>>();
                    request.Add(Tuple.Create("Id", (object)Id));
                    request.Add(Tuple.Create("SpecialNotes", (object)SpecialNotes.Trim()));

                    var baseResponse = JsonConvert.DeserializeObject<Response<bool>>(
                                   await _apiManager.CallGetMethod("lead/UpdateSpecialNote", request));

                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                        && baseResponse.Data)
                    {
                        messageType = AppConstantUI.AlertSuccessType;
                        message = "Special Notes Updated Successfully";
                    }
                    else
                    {
                        messageType = AppConstantUI.AlertErrorType;
                        message = baseResponse.Message;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SpecialNotes))
                    {
                        messageType = AppConstantUI.AlertErrorType;
                        message = "Please Enter Special Notes.";
                    }
                    else
                    {
                        messageType = AppConstantUI.AlertErrorType;
                        message = "Lead Detail Not Found!";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Json(new { messageType, message });
        }
        /// <summary>
        /// Update Bookmark
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> UpdateBookmark(int Id, int Flag)
        {
            var messageType = AppConstantUI.AlertErrorType;
            var message = AppConstantUI.ExceptionMessage;

            try
            {
                if (Id > 0)
                {
                    var request = new List<Tuple<string, object>>();
                    request.Add(Tuple.Create("Id", (object)Id));
                    request.Add(Tuple.Create("Flag", (object)Flag));

                    var baseResponse = JsonConvert.DeserializeObject<Response<bool>>(
                                   await _apiManager.CallGetMethod("lead/UpdateBookmark", request));

                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                        && baseResponse.Data)
                    {
                        messageType = AppConstantUI.AlertSuccessType;
                        if (Flag == 0)
                            message = "Bookmark Removed Successfully!";
                        else
                            message = "Bookmark Successfully!";

                    }
                    else
                    {
                        messageType = AppConstantUI.AlertErrorType;
                        message = baseResponse.Message;
                    }
                }
                else
                {
                    messageType = AppConstantUI.AlertErrorType;
                    message = "Lead Detail Not Found!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Json(new { messageType, message });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateLeadStatus(int LeadId, int LeadStatusId, string LeadStatusName, int AssignTo, int AssignedToId)
        {
            var Message = AppConstantUI.ExceptionMessage;
            var MessageType = AppConstant.AlertErrorType;
            try
            {
                if (LeadId > 0)
                {
                    var request = new List<Tuple<string, object>>();
                    request.Add(Tuple.Create("LeadId", (object)LeadId));
                    request.Add(Tuple.Create("LeadStatusId", (object)LeadStatusId));
                    request.Add(Tuple.Create("LeadStatusName", (object)LeadStatusName));
                    request.Add(Tuple.Create("AssignTo", (object)AssignTo));
                    request.Add(Tuple.Create("AssigneeId", (object)AllFunctions.getUserID()));
                    request.Add(Tuple.Create("AssignedToId", (object)AssignedToId));

                    var baseResponse = JsonConvert.DeserializeObject<Response<bool>>(
                                   await _apiManager.CallGetMethod("lead/UpdateLeadStatus", request));

                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                        && baseResponse.Data)
                    {
                        Message = "Status Updated Successfully!";
                        MessageType = AppConstant.AlertSuccessType;

                        try
                        {
                            var requestNotification = new NotificationToolRequest();
                            requestNotification.Refid = LeadId.ToString();
                            requestNotification.Reftable = "Lead";
                            requestNotification.Command = "Statuschange";
                            requestNotification.UserId = LeadStatusId;

                            var baseResponseNotification = JsonConvert.DeserializeObject<Response<bool>>(
                                      await _apiManager.CallPostMethod("common/AddNotification", requestNotification));

                            //if (baseResponseNotification.Status.Equals((int)AppConstant.APIStatus.SUCCESS))
                            //{
                            //Message = "Status Updated Successfully!";
                            //MessageType = AppConstant.AlertSuccessType;
                            //}
                            //else
                            //{
                            //    Message = "Notification not sent!";
                            //    MessageType = AppConstant.AlertErrorType;
                            //}
                        }
                        catch (Exception e)
                        {
                            Message = "Something went wrong! Please try again later";
                            _logger.LogError(e, e.Message);
                        }

                    }
                }
                else
                {
                    Message = "Lead Detail Not Found!";
                    MessageType = AppConstant.AlertErrorType;
                }
            }
            catch (Exception e)
            {
                Message = "Something went wrong! Please try again later";
                _logger.LogError(e, e.Message);
            }
            return Json(new { Message, MessageType });
        }
        [HttpPost]
        public async Task<JsonResult> UpdateAssignedTo(int LeadId, int AssignedToId, int StatusId)
        {
            var Message = AppConstantUI.ExceptionMessage;
            var MessageType = AppConstant.AlertErrorType;
            try
            {
                if (LeadId > 0)
                {
                    var request = new List<Tuple<string, object>>();
                    request.Add(Tuple.Create("LeadId", (object)LeadId));
                    request.Add(Tuple.Create("AssignedToId", (object)AssignedToId));
                    request.Add(Tuple.Create("AssigneeId", (object)AllFunctions.getUserID()));
                    request.Add(Tuple.Create("StatusId", (object)StatusId));

                    var baseResponse = JsonConvert.DeserializeObject<Response<bool>>(
                                   await _apiManager.CallGetMethod("lead/UpdateAssignedTo", request));

                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS) && baseResponse.Data)
                    {
                        Message = "Lead Assigned Successfully!";
                        MessageType = AppConstant.AlertSuccessType;

                        try
                        {
                            var requestNotification = new NotificationToolRequest();
                            requestNotification.Refid = LeadId.ToString();
                            requestNotification.Reftable = "Lead";
                            requestNotification.Command = "Assign";
                            requestNotification.UserId = AssignedToId;

                            var baseResponseNotification = JsonConvert.DeserializeObject<Response<bool>>(
                                      await _apiManager.CallPostMethod("common/AddNotification", requestNotification));

                            //if (baseResponseNotification.Status.Equals((int)AppConstant.APIStatus.SUCCESS))
                            //{
                            //Message = "Lead Assigned Successfully!";
                            //MessageType = AppConstant.AlertSuccessType;
                            //}
                            //else
                            //{
                            //    Message = "Notification not sent!";
                            //    MessageType = AppConstant.AlertErrorType;
                            //}
                        }
                        catch (Exception e)
                        {
                            Message = "Something went wrong! Please try again later";
                            _logger.LogError(e, e.Message);
                        }

                    }
                }
                else
                {
                    Message = "Lead Detail Not Found!";
                    MessageType = AppConstant.AlertErrorType;
                }

            }
            catch (Exception e)
            {
                Message = "Something went wrong! Please try again later";
                _logger.LogError(e, e.Message);
            }
            return Json(new { Message, MessageType });
        }

        #endregion Lead Ajax

        #region Property detail

        /// <summary>
        /// Get List of Lead Property Detail by Parameters
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetPropertyDetailList()
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

                var request = new PropertyListRequest();
                request.SortCol = Convert.ToInt32(Request.Form["order[0][column]"]);
                request.SortDir = Request.Form["order[0][dir]"];
                if (request.SortDir == null)
                {
                    request.SortDir = "desc";
                }
                request.PageIndex = Convert.ToInt32(Request.Form["start"]);
                request.PageSize = Convert.ToInt32(Request.Form["length"]);

                bool? IsActive = null;
                if (!string.IsNullOrEmpty(Request.Form["isactive"]))
                    IsActive = Convert.ToBoolean(Convert.ToInt32(Request.Form["isactive"]));
                request.IsActive = IsActive;

                string Search = null;
                if (!string.IsNullOrEmpty(Request.Form["search[value]"]))
                    Search = Convert.ToString(Request.Form["search[value]"]).Trim();

                if (!string.IsNullOrEmpty(Search))
                    request.Keyword = Search;

                request.Cid = AllFunctions.getCompanyID();
                request.Bid = AllFunctions.getBranchID();
                request.LeadId = Convert.ToInt32(Request.Form["LeadId"]);
                request.TypeId = Convert.ToInt32(Request.Form["TypeId"]);

                bool? IsMapCategory = null;
                if (!string.IsNullOrEmpty(Request.Form["IsMapCategory"]))
                    IsMapCategory = Convert.ToBoolean(Request.Form["IsMapCategory"]);
                request.IsMapCategory = IsMapCategory;

                bool? IsMapSubCategory = null;
                if (!string.IsNullOrEmpty(Request.Form["IsMapSubCategory"]))
                    IsMapSubCategory = Convert.ToBoolean(Request.Form["IsMapSubCategory"]);
                request.IsMapSubCategory = IsMapSubCategory;

                bool? IsMapBudget = null;
                if (!string.IsNullOrEmpty(Request.Form["IsMapBudget"]))
                    IsMapBudget = Convert.ToBoolean(Request.Form["IsMapBudget"]);
                request.IsMapBudget = IsMapBudget;

                bool? IsMapArea = null;
                if (!string.IsNullOrEmpty(Request.Form["IsMapArea"]))
                    IsMapArea = Convert.ToBoolean(Request.Form["IsMapArea"]);
                request.IsMapArea = IsMapArea;

                bool? IsMapProject = null;
                if (!string.IsNullOrEmpty(Request.Form["IsMapProject"]))
                    IsMapProject = Convert.ToBoolean(Request.Form["IsMapProject"]);
                request.IsMapProject = IsMapProject;
                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<List<PropertyListResponse>>>(
                    await _apiManager.CallPostMethod("lead/LeadPropertyDetailList", request));

                var list = new List<PropertyListResponse>();
                if (baseResponse.Data != null)
                {
                    for (int i = 0; i < baseResponse.Data.Count; i++)
                    {
                        var obj = baseResponse.Data[i];
                        if (!AllFunctions.CheckFileExist("/Docs/Property/" + obj.Id + "/" + obj.Brochure))
                        {
                            obj.Brochure = "/adminpanel/images/file-not-found.jpg";
                        }
                        else
                        {
                            obj.Brochure = "/Docs/Property/" + obj.Id + "/" + obj.Brochure;
                        }
                        list.Add(obj);
                    }
                }
                #endregion

                #region Return JSON
                response = Json(new
                {
                    draw = draw,
                    recordsTotal = baseResponse.Count,
                    recordsFiltered = baseResponse.Count,
                    data = list//baseResponse.Data
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
        /// Delete Lead Mapped PropertyDetail by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> DeleteMappedPropertyDetail(string id)
        {
            var messageType = AppConstantUI.AlertErrorType;
            var message = AppConstantUI.ExceptionMessage;
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (Convert.ToInt32(id) > 0)
                    {
                        var _RefTableMainId = Convert.ToInt32(id);

                        var baseResponse = JsonConvert.DeserializeObject<Response<bool>>(
                               await _apiManager.CallDeleteMethod("lead/DeleteMappedPropertyDetail/" + id));

                        if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                            && baseResponse.Data)
                        {
                            messageType = AppConstantUI.AlertSuccessType;
                            message = AppConstantUI.DeleteMessage;

                        }
                        else
                        {
                            messageType = AppConstantUI.AlertErrorType;
                            message = baseResponse.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return Json(new { message, messageType });
        }
        /// <summary>
        /// MapUnmap Lead Property by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> MapUnmapLeadProperty(int id, int LeadId, int PropertyId, int Type)
        {
            var messageType = AppConstantUI.AlertErrorType;
            var message = AppConstantUI.ExceptionMessage;
            try
            {
                if (LeadId > 0 && PropertyId > 0)
                {
                    if (Type == 1)
                    {
                        var objmapper = new LeadPropertyDetail();
                        objmapper.LeadId = LeadId;
                        objmapper.PropertyId = PropertyId;
                        var baseResponse = JsonConvert.DeserializeObject<Response<int>>(
                               await _apiManager.CallPostMethod("lead/InsertLeadPropertyDetail", objmapper));

                        if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                             && baseResponse.Data > 0)
                        {
                            messageType = AppConstantUI.AlertSuccessType;
                            message = "Property Mapped Successfully";
                        }
                        else
                        {
                            messageType = AppConstantUI.AlertErrorType;
                            message = baseResponse.Message;
                        }
                    }
                    else
                    {

                        var baseResponse = JsonConvert.DeserializeObject<Response<bool>>(
                               await _apiManager.CallDeleteMethod("lead/DeleteMappedPropertyDetail/" + id));

                        if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                            && baseResponse.Data)
                        {
                            messageType = AppConstantUI.AlertSuccessType;
                            message = "Property Unmapped Successfully";
                        }
                        else
                        {
                            messageType = AppConstantUI.AlertErrorType;
                            message = baseResponse.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return Json(new { message, messageType });
        }
        #endregion Property detail

        #region Activity
        string GetPageNameFromIsFrom(int IsFrom)
        {
            if (IsFrom == 1)
            {
                return "Lead";
            }
            else if (IsFrom == 2)
            {
                return "Opportunity";
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get List of Lead Activity Details by Parameters
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PartialViewResult> GetActivityList(string RefTableId, int IsFrom, int TypeId)
        {
            List<ActivityUI> list = new List<ActivityUI>();
            string _PartialViewName = string.Empty;
            try
            {
                #region Get Activity Data
                RefTableId = AppCommon.DecryptString(RefTableId);
                string RefTableName = GetPageNameFromIsFrom(IsFrom);

                if (Convert.ToInt32(RefTableId) > 0)
                {
                    #region Get Data from API
                    var objRequest = new ActivityListRequest();
                    objRequest.RefTableId = Convert.ToInt32(RefTableId);
                    objRequest.RefTableName = RefTableName;
                    objRequest.IsFrom = IsFrom;
                    objRequest.TypeId = TypeId;
                    objRequest.Cid = AllFunctions.getCompanyID();
                    objRequest.Bid = AllFunctions.getBranchID();

                    var baseResponse = JsonConvert.DeserializeObject<Response<List<ActivityUI>>>(
                        await _apiManager.CallPostMethod("activity/list", objRequest));

                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS))
                    {
                        list = baseResponse.Data;
                    }
                    #endregion

                }
                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            if (TypeId == 1)
            {
                _PartialViewName = "_FollowUpListPartial";
            }
            else if (TypeId == 2)
            {
                _PartialViewName = "_LogACallListPartial";
            }
            else if (TypeId == 3)
            {
                _PartialViewName = "_TaskListPartial";
            }
            else if (TypeId == 4)
            {
                _PartialViewName = "_SiteVisitListPartial";
            }
            else if (TypeId == 5)
            {
                _PartialViewName = "_LeadActivityListPartial";
            }

            return PartialView(_PartialViewName, list);
        }

        /// <summary>
        /// Get Count of Lead Activity Details by Parameters
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetActivityCount(string LeadId, int IsFrom)
        {
            int FollowUpCount = 0;
            int LogACallCount = 0;
            int TaskCount = 0;
            int SiteVisitCount = 0;
            int LeadActivityCount = 0;
            try
            {
                if (!string.IsNullOrEmpty(LeadId))
                {
                    LeadId = AppCommon.DecryptString(LeadId);

                    string RefTableName = GetPageNameFromIsFrom(IsFrom);

                    #region Get Data from API
                    var objRequest = new ActivityListRequest();
                    objRequest.RefTableId = Convert.ToInt32(LeadId);
                    objRequest.RefTableName = RefTableName;
                    objRequest.IsFrom = IsFrom;
                    objRequest.Cid = AllFunctions.getCompanyID();
                    objRequest.Bid = AllFunctions.getBranchID();

                    var baseResponse = JsonConvert.DeserializeObject<Response<ActivityCountResponse>>(
                        await _apiManager.CallPostMethod("activity/ActivityCount", objRequest));

                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS))
                    {
                        FollowUpCount = baseResponse.Data.FollowUpCount;
                        LogACallCount = baseResponse.Data.LogACallCount;
                        TaskCount = baseResponse.Data.TaskCount;
                        SiteVisitCount = baseResponse.Data.SiteVisitCount;
                        LeadActivityCount = baseResponse.Data.LeadActivityCount;
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            return Json(new
            {
                FollowUpCount,
                LogACallCount,
                TaskCount,
                SiteVisitCount,
                LeadActivityCount
            });
        }
        /// <summary>
        /// Insert / Update / Delete Activity
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ActivityOperation(Activity obj)
        {
            int StatusCode = 0;
            string Message = AppConstantUI.ExceptionMessage;
            string MessageType = AppConstant.AlertErrorType;
            try
            {
                string LeadId = AppCommon.DecryptString(obj.RefTableId);
                string RefTableName = GetPageNameFromIsFrom(obj.IsFrom);
                obj.ModuleType = RefTableName;
                obj.ModuleTypeId = obj.IsFrom;

                if (obj.Id > 0 && obj.IsDelete == 0)
                {
                    var baseResponse = JsonConvert.DeserializeObject<Response<int>>(
                                await _apiManager.CallPutMethod("activity/update/" + obj.Id, obj));
                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                            && baseResponse.Data > 0)
                    {
                        StatusCode = 1;
                        Message = "Record Updated Successfully!";
                        MessageType = AppConstant.AlertSuccessType;
                    }
                }
                if (obj.Id == 0 && obj.IsDelete == 0)
                {
                    var baseResponse = JsonConvert.DeserializeObject<Response<int>>(
                        await _apiManager.CallPostMethod("activity/insert", obj));

                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                         && baseResponse.Data > 0)
                    {
                        StatusCode = 1;
                        Message = "Record Inserted Successfully!";
                        MessageType = AppConstant.AlertSuccessType;
                    }
                }
                if (obj.Id > 0 && obj.IsDelete == 1)
                {
                    var baseResponse = JsonConvert.DeserializeObject<Response<bool>>(
                               await _apiManager.CallDeleteMethod("activity/delete/" + obj.Id));

                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                        && baseResponse.Data)
                    {
                        StatusCode = 1;
                        Message = "Record Deleted Successfully!";
                        MessageType = AppConstant.AlertSuccessType;
                    }
                }
            }
            catch (Exception ex)
            {
                StatusCode = 0;
                Message = "Something Went Wrong!";
                MessageType = AppConstant.AlertErrorType;
                _logger.LogError(ex, ex.Message);
            }

            return Json(new
            {
                StatusCode,
                Message,
                MessageType
            });
        }
        /// <summary>
        /// Get Activity By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetActivityData(int Id)
        {
            int StatusCode = 0;
            string Message = AppConstantUI.ExceptionMessage;
            string MessageType = AppConstant.AlertErrorType;
            Activity obj = new Activity();
            try
            {
                #region Get Activity Data
                if (Id > 0)
                {
                    var baseResponse = JsonConvert.DeserializeObject<Response<Activity>>(
                           await _apiManager.CallPostMethod("activity/edit/" + Id, null));
                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS))
                    {
                        obj = baseResponse.Data;
                        StatusCode = 1;
                        Message = "Record Fetched Successfully!";
                        MessageType = AppConstant.AlertSuccessType;
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                StatusCode = 0;
                Message = "Something Went Wrong!";
                MessageType = AppConstant.AlertErrorType;
                _logger.LogError(e, e.Message);
            }
            return Json(new
            {
                StatusCode,
                Message,
                MessageType,
                Data = obj
            });
        }
        /// <summary>
        /// Update Activity Follow status
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UpdateActivityStatus(Activity obj)
        {
            int StatusCode = 0;
            string Message = AppConstantUI.ExceptionMessage;
            string MessageType = AppConstant.AlertErrorType;
            try
            {
                #region Update
                if (obj.Id > 0 && obj.IsDelete == 0)
                {
                    var baseResponse = JsonConvert.DeserializeObject<Response<int>>(
                               await _apiManager.CallPutMethod("activity/updatestatus/" + obj.Id, obj));
                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                            && baseResponse.Data > 0)
                    {
                        StatusCode = 1;
                        Message = "Status Updated Successfully!";
                        MessageType = AppConstant.AlertSuccessType;
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                StatusCode = 0;
                Message = "Something Went Wrong!";
                MessageType = AppConstant.AlertErrorType;
                _logger.LogError(e, e.Message);
            }
            return Json(new
            {
                StatusCode,
                Message,
                MessageType
            });
        }
        #endregion Activity

        [HttpPost]
        public async Task<ActionResult> LeadShareEmailSend(int LeadId, string Ids, string UserEmail)
        {
            bool IsEmailValidate = false;
            var messageType = AppConstantUI.AlertErrorType;
            string message = "Email Address Not Set.";
            try
            {

                if (Ids != "")
                {
                    #region Email

                    #region Initialize Input parameter of SP

                    var request = new PropertyListRequest();
                    request.SortCol = 0;
                    request.SortDir = "desc";

                    request.PageIndex = 0;
                    request.PageSize = 1000;

                    bool? IsActive = true;
                    request.IsActive = IsActive;

                    request.Cid = AllFunctions.getCompanyID();
                    request.Bid = AllFunctions.getBranchID();
                    request.LeadId = Convert.ToInt32(LeadId);
                    request.TypeId = 1;

                    bool? IsMapCategory = false;
                    request.IsMapCategory = IsMapCategory;

                    bool? IsMapSubCategory = false;
                    request.IsMapSubCategory = IsMapSubCategory;

                    bool? IsMapBudget = false;
                    request.IsMapBudget = IsMapBudget;

                    bool? IsMapArea = false;
                    request.IsMapArea = IsMapArea;

                    bool? IsMapProject = false;
                    request.IsMapProject = IsMapProject;
                    #endregion

                    #region Get Data from API

                    var baseResponse = JsonConvert.DeserializeObject<Response<List<PropertyListResponse>>>(
                        await _apiManager.CallPostMethod("lead/LeadPropertyDetailList", request));

                    List<PropertyListResponse> PropertyList = new List<PropertyListResponse>();
                    string[] values = Ids.Split(',');
                    foreach (var LId in values)
                    {
                        PropertyList.Add(baseResponse.Data.Where(c => c.Id == Convert.ToInt32(LId)).FirstOrDefault());
                    }

                    #endregion

                    Dictionary<string, string> emailParameters = new Dictionary<string, string>();
                    var templatePath = Path.Combine(environment.WebRootPath, "EmailTemplates", "Lead_Share_Probro.html");

                    string html = "";
                    for (int i = 0; i < PropertyList.Count(); i++)
                    {
                        html += "<tr>";
                        html += "<td>" + PropertyList[i].RowNo.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].PropertyID.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].AvailabilityName.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].CategoryName.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].SubCategoryName.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].PropertyFor.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].PropertyBHKName.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].AreaName.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].SuperBuildupArea.ToString() + PropertyList[i].SuperBuildupAreaUnitName.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].CarpetArea.ToString() + PropertyList[i].CarpetAreaUnitName.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].ProjectName.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].Date.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].PropertyStatusName.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].PropertyFurnishStatusName.ToString() + "</td>";
                        html += "<td>" + PropertyList[i].PropertyAvailableFor.ToString() + "</td>";
                        var brachurepath = "/Docs/Property/" + PropertyList[i].Id.ToString() + "/" + PropertyList[i].Brochure.ToString();
                        if (!AllFunctions.CheckFileExist(brachurepath))
                        {
                            brachurepath = "/adminpanel/images/file-not-found.jpg";
                        }
                        //html += "<td>" + "<a target='_blank' href=" + "'" + ConfigWrapper.GetAppSettings("DomainName") + "/Docs/Property/" + PropertyList[i].Id.ToString() + "/" + PropertyList[i].Brochure.ToString() + "'" + ">VIEW BROCHURE</a>" + "</td>";
                        html += "<td>" + "<a target='_blank' href=" + "'" + ConfigWrapper.GetAppSettings("DomainName") + brachurepath + "'" + ">VIEW BROCHURE</a>" + "</td>";
                        html += "</tr>";
                    }

                    emailParameters.Add("htmlstring", html);
                    var body = PopulateMailBody(emailParameters, templatePath);

                    EmailUtility EmailSend = new EmailUtility();
                    var emailresponse = await EmailSend.SendEmail(UserEmail, "", "Share Property", body, "", AllFunctions.getCompanyID());

                    if (emailresponse.Value.ToString() == "success")
                    {
                        IsEmailValidate = true;
                        messageType = AppConstantUI.AlertSuccessType;
                        message = "Email sent successfully.";
                    }else if (emailresponse.Value.ToString() == "warning")
                    {
                        IsEmailValidate = false;
                        messageType = AppConstantUI.AlertErrorType;
                        message = Convert.ToString(emailresponse.SerializerSettings);
                    }
                    else
                    {
                        IsEmailValidate = false;
                        messageType = AppConstantUI.AlertErrorType;
                        message = "Email not sent.";
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                IsEmailValidate = false;
                messageType = AppConstantUI.AlertErrorType;
                message = "Something was Wrong.";
            }

            return Json(new { IsEmailValidate, message, messageType });
        }

        public static string PopulateMailBody(Dictionary<string, string> parameters, string TemplatePath)
        {
            string body = string.Empty;
            try
            {

                using (StreamReader reader = new StreamReader(TemplatePath))
                {
                    body = reader.ReadToEnd();
                }
                foreach (var item in parameters)
                {
                    body = body.Replace("#" + item.Key + "#", item.Value);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return body;
        }

        /// <summary>
        /// Assign Multiple Lead by Ids
        /// </summary>
        /// <param name="LeadIds"></param>
        /// <param name="AssignToId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> AssignMultiple(string LeadIds, int AssignToId)
        {
            var messageType = AppConstantUI.AlertErrorType;
            var message = AppConstantUI.ExceptionMessage;

            try
            {
                if (!string.IsNullOrEmpty(LeadIds) && AssignToId > 0)
                {
                    var request = new List<Tuple<string, object>>();
                    request.Add(Tuple.Create("LeadIds", (object)LeadIds));
                    request.Add(Tuple.Create("AssignToId", (object)AssignToId));

                    var baseResponse = JsonConvert.DeserializeObject<Response<bool>>(
                               await _apiManager.CallGetMethod("lead/AssignMultiple", request));

                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                        && baseResponse.Data)
                    {
                        messageType = AppConstantUI.AlertSuccessType;
                        message = AppConstantUI.UpdateMessage;
                    }
                    else
                    {
                        messageType = AppConstantUI.AlertErrorType;
                        message = baseResponse.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Json(new { messageType, message });
        }

    }
}
