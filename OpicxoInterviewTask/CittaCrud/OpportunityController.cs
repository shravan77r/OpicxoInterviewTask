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
    public class OpportunityController : Controller
    {

        private readonly ILogger<OpportunityController> _logger;
        private APIManager _apiManager;
        private readonly IMapper _mapper;
        private IWebHostEnvironment environment;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="logger"></param>        
        public OpportunityController(ILogger<OpportunityController> logger, IMapper mapper, IWebHostEnvironment environment)
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

        #region Opportunity CRUD   


        /// <summary>
        /// List Page of Opportunity
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List(int? PI, int? PS, int? ViewType, string EditId)
        {
            #region Active Link
            ViewBag.Opportunity = "active";
            ViewBag.Page = "Opportunity";
            #endregion

            #region Rights
            try
            {
                AssignRights(PageConstant.GetInstance().Transactions_Opportunity);
                if (ViewBag.view != true)
                {
                    TempData["type"] = AppConstant.AlertInfoType;
                    TempData["msg"] = AppConstant.AlertRightsWarning;
                    return RedirectToAction("Dashboard", "Home", new { Area = "Transactions" });
                }
                ViewBag.TeamRights = AllFunctions.SetRights(PageConstant.GetInstance().Opportunity_Team_Rights).view;
                ViewBag.AllRights = AllFunctions.SetRights(PageConstant.GetInstance().Opportunity_All_Rights).view;
                ViewBag.edit = AllFunctions.SetRights(PageConstant.GetInstance().Opportunity_Team_Rights).edit;
                ViewBag.delete = AllFunctions.SetRights(PageConstant.GetInstance().Opportunity_Team_Rights).delete;
                ViewBag.view = AllFunctions.SetRights(PageConstant.GetInstance().Opportunity_Team_Rights).view;
                ViewBag.addnew = AllFunctions.SetRights(PageConstant.GetInstance().Opportunity_Team_Rights).addnew;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            #endregion

            #region Default
            var obj = new OpportunityUI();

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
            Dictionary<int?, string> objDictAssign = new Dictionary<int?, string>();
            objDictAssign.Add(AllFunctions.getUserID(), AllFunctions.getUserName());
            ViewBag.SiteVisitAssignTo = objDictAssign;
            ViewBag.AssignToId = AllFunctions.getUserID();
            ViewBag.AssignToName = AllFunctions.getUserName();
            ViewBag.PI = PI;
            ViewBag.PS = PS;
            ViewBag.ViewType = ViewType;
            ViewBag.EditId = EditId;
            #endregion

            return View(obj);
        }

        /// <summary>
        /// Get List of Opportunity
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetList(int order, string orderDir, int startRec, int pageSize, string Search,
           int AssignTypeId, string MainAssignToIds, int? IsBookMarked,
           string RelevanceIds, string FromDate, string ToDate, string StatusIds,
           string CategoryIds, string SubcategoryIds,
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
                var objRequest = new OpportunityListRequest();
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

                var baseResponse = JsonConvert.DeserializeObject<Response<List<OpportunityListResponse>>>(
                    await _apiManager.CallPostMethod("opportunity/list", objRequest));

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
        /// View form of Opportunity
        /// </summary>
        /// <param name="EncryptedId"></param>
        /// <returns></returns>
        public async Task<IActionResult> View_New(string EncryptedId, int? PI, int? PS, int? ViewType)
        {
            ViewBag.Opportunity = "active";

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
                            return RedirectToAction("List", "Opportunity", new { Area = "Transactions" });
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

        #endregion Opportunity CRUD

        #region Opportunity Ajax
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
                                   await _apiManager.CallGetMethod("opportunity/UpdateSpecialNote", request));

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
                        message = "Please Enter Special Notes";
                    }
                    else
                    {
                        messageType = AppConstantUI.AlertErrorType;
                        message = "Opportunity Detail Not Found!";
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
                                   await _apiManager.CallGetMethod("opportunity/UpdateBookmark", request));

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
                    message = "Opportunity Detail Not Found!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Json(new { messageType, message });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateOpportunityStatus(int OpportunityId, int OpportunityStageId, string OpportunityStageName, int AssignTo, int LostReasonId, string Remarks,  int WonPropertyId
            , string SellerName, string SellerMobileNo, string SellerEmail, int SellerCountry, int SellerState, int SellerCity, int SellerArea, string SellerAddress, bool IsSellerBrokerInvolved, string SellerBrokerName, string BuyerName, string BuyerMobileNo, string BuyerEmail, int BuyerCountry, int BuyerState, int BuyerCity, int BuyerArea, string BuyerAddress, bool IsBuyerBrokerInvolved, string BuyerBrokerName, decimal TransactionValue, decimal OtherValue, decimal SaleDeedValue, decimal TotalTransactionValue, string TimePeriod, decimal LoanAmount, decimal Brokerage
            , string PaymentDetails, int SellerMobileNoIsEdit, int BuyerMobileNoIsEdit,int AssignedToId)
        {
            var Message = AppConstantUI.ExceptionMessage;
            var MessageType = AppConstant.AlertErrorType;
            try
            {
                if (OpportunityId > 0)
                {
                    OpportunityRequest request = new OpportunityRequest();
                    request.OpportunityId = OpportunityId;
                    request.OpportunityStageId = OpportunityStageId;
                    request.OpportunityStageName = OpportunityStageName;
                    request.AssignTo = AssignTo;
                    request.WonPropertyId = WonPropertyId;
                    request.Remarks = Remarks;
                    request.LostReasonId = LostReasonId;
                    request.AssigneeId = AllFunctions.getUserID();

                    request.SellerName = SellerName;
                    request.SellerMobileNo = SellerMobileNo;
                    request.SellerEmail = SellerEmail;
                    request.SellerCountry = SellerCountry;
                    request.SellerState = SellerState;
                    request.SellerCity = SellerCity;
                    request.SellerArea = SellerArea;
                    request.SellerAddress = SellerAddress;
                    request.IsSellerBrokerInvolved = IsSellerBrokerInvolved;
                    request.SellerBrokerName = SellerBrokerName;
                    request.BuyerName = BuyerName;
                    request.BuyerMobileNo = BuyerMobileNo;
                    request.BuyerEmail = BuyerEmail;
                    request.BuyerCountry = BuyerCountry;
                    request.BuyerState = BuyerState;
                    request.BuyerCity = BuyerCity;
                    request.BuyerArea = BuyerArea;
                    request.BuyerAddress = BuyerAddress;
                    request.IsBuyerBrokerInvolved = IsBuyerBrokerInvolved;
                    request.BuyerBrokerName = BuyerBrokerName;
                    request.TransactionValue = TransactionValue;
                    request.OtherValue = OtherValue;
                    request.SaleDeedValue = SaleDeedValue;
                    request.TotalTransactionValue = TotalTransactionValue;
                    request.TimePeriod = TimePeriod;
                    request.LoanAmount = LoanAmount;
                    request.Brokerage = Brokerage;
                    request.SellerMobileNoIsEdit = SellerMobileNoIsEdit;
                    request.BuyerMobileNoIsEdit = BuyerMobileNoIsEdit;
                    request.AssignedToId = AssignedToId;

                    var baseResponse = JsonConvert.DeserializeObject<Response<OpportunityListResponse>>(
                                   await _apiManager.CallPostMethod("opportunity/UpdateOpportunityStatus", request));

                    var listPaymentDetails = new List<DealProvisionalPaymentDetails>();

                    try
                    {
                        if (!string.IsNullOrEmpty(PaymentDetails))
                            listPaymentDetails = JsonConvert.DeserializeObject<List<DealProvisionalPaymentDetails>>(PaymentDetails);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }

                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS)
                        && baseResponse.Data.DealId > 0)
                    {

                        #region Insert Details

                        var listPaymentInsertDetails = listPaymentDetails.Where(o => o.DetailId == 0).ToList();
                        foreach (var item in listPaymentInsertDetails)
                        {
                            try
                            {
                                var objCD = new DealProvisionalPaymentDetails();
                                
                                objCD.DealId = baseResponse.Data.DealId;
                                objCD.Date = item.Date;
                                objCD.Description = item.Description;
                                objCD.Amount = item.Amount;

                                var baseResponseInsert = JsonConvert.DeserializeObject<Response<int>>(
                                    await _apiManager.CallPostMethod("masters/dealProvisionalPaymentDetails/insert", objCD));
                            }
                            catch (Exception e)
                            {
                                _logger.LogError(e, e.Message);
                            }
                        }
                        #endregion Insert Details
                    }

                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS))
                    {
                        Message = "Status Updated Successfully!";
                        MessageType = AppConstant.AlertSuccessType;

                        try
                        {
                            var requestNotification = new NotificationToolRequest();
                            requestNotification.Refid = OpportunityId.ToString();
                            requestNotification.Reftable = "Opportunity";
                            requestNotification.Command = "Statuschange";
                            requestNotification.UserId = OpportunityStageId;

                            var baseResponseNotification = JsonConvert.DeserializeObject<Response<bool>>(
                                      await _apiManager.CallPostMethod("common/AddNotification", requestNotification));

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
                    MessageType = AppConstantUI.AlertErrorType;
                    Message = "Opportunity Detail Not Found!";
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
        public async Task<JsonResult> UpdateAssignedTo(int OpportunityId, int AssignedToId, int StatusId)
        {
            var Message = AppConstantUI.ExceptionMessage;
            var MessageType = AppConstant.AlertErrorType;
            try
            {
                if (OpportunityId > 0)
                {
                    var request = new List<Tuple<string, object>>();
                    request.Add(Tuple.Create("OpportunityId", (object)OpportunityId));
                    request.Add(Tuple.Create("AssignedToId", (object)AssignedToId));
                    request.Add(Tuple.Create("AssigneeId", (object)AllFunctions.getUserID()));
                    request.Add(Tuple.Create("StatusId", (object)StatusId));

                    var baseResponse = JsonConvert.DeserializeObject<Response<bool>>(
                                   await _apiManager.CallGetMethod("opportunity/UpdateAssignedTo", request));

                    if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS) && baseResponse.Data)
                    {
                        Message = "Opportunity Assigned Successfully!";
                        MessageType = AppConstant.AlertSuccessType;

                        try
                        {
                            var requestNotification = new NotificationToolRequest();
                            requestNotification.Refid = OpportunityId.ToString();
                            requestNotification.Reftable = "Opportunity";
                            requestNotification.Command = "Assign";
                            requestNotification.UserId = AssignedToId;

                            var baseResponseNotification = JsonConvert.DeserializeObject<Response<bool>>(
                                      await _apiManager.CallPostMethod("common/AddNotification", requestNotification));

                            //if (baseResponseNotification.Status.Equals((int)AppConstant.APIStatus.SUCCESS))
                            //{
                                //Message = "Opportunity Assigned Successfully!";
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
                    MessageType = AppConstantUI.AlertErrorType;
                    Message = "Opportunity Detail Not Found!";
                }

            }
            catch (Exception e)
            {
                Message = "Something went wrong! Please try again later";
                _logger.LogError(e, e.Message);
            }
            return Json(new { Message, MessageType });
        }

        /// PropertySellerBuyer
        /// </summary>
        /// <param name="WonPropertyId"></param>
        /// <param name="LeadId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetPropertySellerBuyerDetails(int WonPropertyId, int LeadId)
        {
            var response = Json(new
            {
                recordsTotal = 0,
                recordsFiltered = 0,
                property = string.Empty,
                lead = string.Empty,
            });
            try
            {
                    if (WonPropertyId > 0)
                    {
                    var request = new List<Tuple<string, object>>();
                    request.Add(Tuple.Create("WonPropertyId", (object)WonPropertyId));
                    request.Add(Tuple.Create("LeadId", (object)LeadId));

                    var baseResponse = JsonConvert.DeserializeObject<Response<OpportunityListResponse>>(
                            await _apiManager.CallGetMethod("opportunity/PropertySellerBuyerDetails", request));

                        if (baseResponse.Status.Equals((int)AppConstant.APIStatus.SUCCESS))
                        {
                            #region Return JSON
                            response = Json(new
                            {
                                recordsTotal = baseResponse.Count,
                                recordsFiltered = baseResponse.Count,
                                property = baseResponse.Data.PropertyResponse,
                                lead = baseResponse.Data.LeadResponse
                            });
                            #endregion
                        }
                    }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return response;
        }

        /// <summary>
        /// Check for duplicate Customer Mobile No
        /// </summary>
        /// <param name="MobileNo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> IsDuplicateMobileNo(string MobileNo)
        {
            var response = Json(new
            {
                CustomerId = 0,
                MobileNo = string.Empty,
                CustomerName = string.Empty,
            });
            try
            {
                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("MobileNo", (object)MobileNo.Trim()));

                var baseResponse = JsonConvert.DeserializeObject<Response<Customer>>(
                               await _apiManager.CallGetMethod("masters/Customer/checkduplicatemobileno", request));

                if (baseResponse.Data != null)
                {
                    #region Return JSON
                    response = Json(new
                    {
                        CustomerId = baseResponse.Data.Id,
                        MobileNo = baseResponse.Data.MobileNo,
                        CustomerName = baseResponse.Data.CustomerName,
                    });
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Json(response);
        }

        #endregion Opportunity Ajax

        #region Property detail

        /// <summary>
        /// Get List of Opportunity Property Detail by Parameters
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
                if (!string.IsNullOrEmpty(Request.Form["search"]))
                    Search = Convert.ToString(Request.Form["search"]).Trim();

                if (!string.IsNullOrEmpty(Search))
                    request.Keyword = Search;

                request.Cid = AllFunctions.getCompanyID();
                request.LeadId = Convert.ToInt32(Request.Form["LeadId"]);
                request.TypeId = Convert.ToInt32(Request.Form["TypeId"]);
                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<List<PropertyListResponse>>>(
                    await _apiManager.CallPostMethod("lead/LeadPropertyDetailList", request));

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
                TaskCount
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
    }
}
