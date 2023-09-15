using Business;
using Business.ImplementationManager.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Helper;
using Model.Base;
using Model.Transactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers.Transactions
{
    [Route("api/lead")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly ILogger<LeadController> _logger;
        private readonly AppUtil appUtil;
        private BLLeadManager bLLeadManager;

        public LeadController(ILogger<LeadController> logger)
        {
            _logger = logger;
            appUtil = new AppUtil();
        }

        [HttpPost]
        [Route("list")]
        public async Task<Response<List<LeadListResponse>>> List(LeadListRequest request)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, JsonConvert.SerializeObject(request));

            var response = new Response<List<LeadListResponse>>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
            };

            try
            {
                using (bLLeadManager = new BLLeadManager())
                {
                    var data = await bLLeadManager.GetList(request);

                    response.Count = data.Item1;
                    response.Data = data.Item2;
                    response.Status = (int)AppConstant.APIStatus.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = null;
            }

            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost]
        [Route("edit/{id}")]
        public async Task<Response<Lead>> Edit(int id)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, id);

            var response = new Response<Lead>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
            };

            try
            {
                if (id > 0)
                {
                    using (bLLeadManager = new BLLeadManager())
                    {
                        var data = await bLLeadManager.GetData(id);
                        if (data != null)
                        {
                            response.Status = (int)AppConstant.APIStatus.SUCCESS;
                            response.Data = data;
                        }
                        else
                        {
                            response.Status = (int)AppConstant.APIStatus.FAILURE;
                            response.Data = null;
                            response.Message = String.Format(AppConstant.RecordNotFoundMessage, id);

                            _logger.LogError(response.Message);
                        }
                    }
                }
                else
                {
                    response.Status = (int)AppConstant.APIStatus.FAILURE;
                    response.Data = null;
                    response.Message = string.Format(AppConstant.InvalidIdMessage, id);

                    _logger.LogError(response.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = null;
            }
            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }

        [Route("insert")]
        [HttpPost]
        public async Task<Response<int>> Insert(Lead request)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, JsonConvert.SerializeObject(request));

            var response = new Response<int>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = 0
            };
            try
            {
                using (bLLeadManager = new BLLeadManager())
                {
                    appUtil.GetHeaderModel(Request.Headers, out var headerModel);

                    //request.WildSearch = GetWildCard_Lecture(request);

                    var operationStatus = await bLLeadManager.InsertUpdateDelete(0, request, headerModel);
                    response.Status = operationStatus != 0 ? (int)AppConstant.APIStatus.SUCCESS : (int)AppConstant.APIStatus.FAILURE;
                    response.Message = operationStatus != 0 ? AppConstant.InsertMessage : AppConstant.InternalServerError;
                    response.Data = operationStatus;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = 0;
            }
            // _logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<Response<int>> Update(int id, Lead request)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, JsonConvert.SerializeObject(request));
            var response = new Response<int>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = 0
            };
            try
            {
                using (bLLeadManager = new BLLeadManager())
                {
                    appUtil.GetHeaderModel(Request.Headers, out var headerModel);

                    //request.WildSearch = GetWildCard_Lecture(request);

                    var operationStatus = await bLLeadManager.InsertUpdateDelete(request.Id, request, headerModel);  // TODO
                    response.Status = operationStatus != 0 ? (int)AppConstant.APIStatus.SUCCESS : (int)AppConstant.APIStatus.FAILURE;
                    response.Message = operationStatus != 0 ? AppConstant.UpdateMessage : AppConstant.InternalServerError;
                    response.Data = operationStatus;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = 0;
            }
            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }

        //internal string GetWildCard_Lecture(Lead obj)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(obj.LeadNo))
        //        {
        //            sb.Append(obj.LeadNo);
        //            sb.Append(",");
        //        }
        //        if (!string.IsNullOrEmpty(obj.LeadDate))
        //        {
        //            sb.Append(obj.LeadDate);
        //            sb.Append(",");
        //        }

        //        try
        //        {
        //            if (!string.IsNullOrEmpty(Convert.ToString(obj.PropertyForId.GetNullableIntFromNullableDictionary())) && obj.PropertyForId.GetNullableIntFromNullableDictionary() > 0)
        //            {
        //                using (var u = new BLPropertyFor(Convert.ToInt32(obj.PropertyForId.GetNullableIntFromNullableDictionary())))
        //                {
        //                    sb.Append(u.PropertyFor);
        //                    sb.Append(",");
        //                }
        //            }
        //        }
        //        catch (Exception) { }

        //        try
        //        {
        //            if (!string.IsNullOrEmpty(Convert.ToString(obj.PropertyCategoryIds.GetNullableIntFromNullableDictionary())) && Convert.ToInt32(obj.PropertyCategoryIds.GetNullableIntFromNullableDictionary()) > 0)
        //            {
        //                using (var u = new BLPropertyCategory(Convert.ToInt32(obj.PropertyCategoryIds.GetNullableIntFromNullableDictionary())))
        //                {
        //                    sb.Append(u.CategoryName);
        //                    sb.Append(",");
        //                }

        //            }
        //        }
        //        catch (Exception) { }
        //        try
        //        {
        //            if (!string.IsNullOrEmpty(Convert.ToString(obj.PropertySubcategoryIds.GetCommaSeperatedStringFromNullableDictionary())))
        //            {
        //                string[] SplitId = obj.PropertySubcategoryIds.GetCommaSeperatedStringFromNullableDictionary().Split(',');
        //                foreach (var val in SplitId)
        //                {
        //                    using (var u = new BLPropertySubcategory(Convert.ToInt32(val)))
        //                    {
        //                        sb.Append(u.SubategoryName);
        //                        sb.Append(",");
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception) { }
        //        if (!string.IsNullOrEmpty(obj.FirstName))
        //        {
        //            sb.Append(obj.FirstName);
        //            sb.Append(",");
        //        }
        //        if (!string.IsNullOrEmpty(obj.LastName))
        //        {
        //            sb.Append(obj.LastName);
        //            sb.Append(",");
        //        }
        //        if (!string.IsNullOrEmpty(obj.MobileNo))
        //        {
        //            sb.Append(obj.MobileNo);
        //            sb.Append(",");
        //        }
        //        if (!string.IsNullOrEmpty(obj.Email))
        //        {
        //            sb.Append(obj.Email);
        //            sb.Append(",");
        //        }
        //        try
        //        {
        //            if (!string.IsNullOrEmpty(Convert.ToString(obj.AreaIds.GetCommaSeperatedStringFromNullableDictionary())))
        //            {
        //                string[] SplitId = obj.AreaIds.GetCommaSeperatedStringFromNullableDictionary().Split(',');
        //                foreach (var val in SplitId)
        //                {
        //                    using (var u = new BLArea(Convert.ToInt32(val)))
        //                    {
        //                        sb.Append(u.AreaName);
        //                        sb.Append(",");
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception) { }

        //        if (!string.IsNullOrEmpty(obj.Requirement))
        //        {
        //            sb.Append(obj.Requirement);
        //            sb.Append(",");
        //        }
        //        if (!string.IsNullOrEmpty(obj.SpecialNote))
        //        {
        //            sb.Append(obj.SpecialNote);
        //            sb.Append(",");
        //        }
        //        try
        //        {
        //            if (!string.IsNullOrEmpty(Convert.ToString(obj.LeadSourceId.GetNullableIntFromNullableDictionary())) && obj.LeadSourceId.GetNullableIntFromNullableDictionary() > 0)
        //            {
        //                using (var u = new BLLeadSource(Convert.ToInt32(obj.LeadSourceId.GetNullableIntFromNullableDictionary())))
        //                {
        //                    sb.Append(u.LeadSourceName);
        //                    sb.Append(",");
        //                }
        //            }
        //        }
        //        catch (Exception) { }
        //        try
        //        {
        //            if (!string.IsNullOrEmpty(Convert.ToString(obj.AssignTo.GetNullableIntFromNullableDictionary())) && obj.AssignTo.GetNullableIntFromNullableDictionary() > 0)
        //            {
        //                using (var u = new BLEmployee(Convert.ToInt32(obj.AssignTo.GetNullableIntFromNullableDictionary())))
        //                {
        //                    sb.Append(u.FirstName + " " + u.LastName);
        //                    sb.Append(",");
        //                }
        //            }
        //        }
        //        catch (Exception) { }

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //    }
        //    return sb.ToString();
        //}

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<Response<int>> Delete(int id)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, id);

            var response = new Response<int>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = 0
            };
            try
            {
                using (bLLeadManager = new BLLeadManager())
                {
                    appUtil.GetHeaderModel(Request.Headers, out var headerModel);

                    var Opportunitydata = BLOpportunity.OpportunityCollectionFromSearchFields(new BLOpportunity
                    {
                        LeadId = id,
                        IsDelete = false,
                    }).FirstOrDefault();

                    if (Opportunitydata == null)
                    {
                        var operationStatus = await bLLeadManager.InsertUpdateDelete(id, null, headerModel);
                        response.Status = operationStatus != 0 ? (int)AppConstant.APIStatus.SUCCESS : (int)AppConstant.APIStatus.FAILURE;
                        response.Message = operationStatus != 0 ? AppConstant.DeleteMessage : AppConstant.InternalServerError;
                        response.Data = operationStatus;
                    }
                    else
                    {
                        response.Status = (int)AppConstant.APIStatus.FAILURE;
                        response.Message = AppConstant.RecordNotDeleteError; //"You can't delete this Qualified Lead";
                        response.Data = id;
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = 0;
            }
            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpDelete]
        [Route("isdelete/{id}")]
        public async Task<Response<int>> IsDelete(int id)
        {
            var response = new Response<int>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = 0
            };
            try
            {
                using (bLLeadManager = new BLLeadManager())
                {
                    appUtil.GetHeaderModel(Request.Headers, out var headerModel);

                    var Opportunitydata = BLOpportunity.OpportunityCollectionFromSearchFields(new BLOpportunity
                    {
                        LeadId = id,
                        IsDelete = false,
                    }).FirstOrDefault();

                    if (Opportunitydata == null)
                    {
                        response.Status = (int)AppConstant.APIStatus.SUCCESS;
                        response.Message = ""; //"You can't delete this Qualified Lead";
                        response.Data = id;
                    }
                    else
                    {
                        response.Status = (int)AppConstant.APIStatus.FAILURE;
                        response.Message = AppConstant.RecordNotDeleteError; //"You can't delete this Qualified Lead";
                        response.Data = id;
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = 0;
            }

            return response;
        }

        [HttpGet]
        [Route("checkduplicate")]
        public async Task<Response<bool>> CheckDuplicate(int id, string leadNo)
        {
            //_logger.LogInformation("Entered with id {0}, LeadNo : {1}", id, leadNo);

            var response = new Response<bool>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = false
            };

            try
            {
                appUtil.GetHeaderModel(Request.Headers, out var headerModel);
                using (bLLeadManager = new BLLeadManager())
                {
                    response.Status = (int)AppConstant.APIStatus.SUCCESS;
                    response.Data = await bLLeadManager.IsDuplicate(id, leadNo, headerModel.Cid);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = false;
            }

            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }
        [HttpGet]
        [Route("UpdateSpecialNote")]
        public async Task<Response<bool>> UpdateSpecialNote(int Id, string SpecialNotes)
        {
            //_logger.LogInformation("Entered with id {0}, SpecialNotes : {1} ", Id, SpecialNotes);

            var response = new Response<bool>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = false
            };

            try
            {
                appUtil.GetHeaderModel(Request.Headers, out var headerModel);
                using (bLLeadManager = new BLLeadManager())
                {
                    response.Status = (int)AppConstant.APIStatus.SUCCESS;
                    response.Data = await bLLeadManager.UpdateSpecialNote(Id, SpecialNotes, headerModel.Uid);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = false;
            }

            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }
        [HttpGet]
        [Route("UpdateBookmark")]
        public async Task<Response<bool>> UpdateBookmark(int Id, int Flag)
        {
            //_logger.LogInformation("Entered with id {0}, Flag : {1} ", Id, Flag);

            var response = new Response<bool>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = false
            };

            try
            {
                appUtil.GetHeaderModel(Request.Headers, out var headerModel);
                using (bLLeadManager = new BLLeadManager())
                {
                    response.Status = (int)AppConstant.APIStatus.SUCCESS;
                    response.Data = await bLLeadManager.UpdateBookmark(Id, Flag, headerModel.Uid);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = false;
            }

            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }
        [HttpGet]
        [Route("UpdateLeadStatus")]
        public async Task<Response<bool>> UpdateLeadStatus(int LeadId, int LeadStatusId, string LeadStatusName, int AssignTo, int AssigneeId, int AssignedToId)
        {
            //_logger.LogInformation("Entered with LeadId {0}, LeadStatusId : {1}, LeadStatusName : {2}, AssignTo : {3}, AssignedToId : {4} ", LeadId, LeadStatusId, LeadStatusName, AssignTo, AssignedToId);

            var response = new Response<bool>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = false
            };

            try
            {
                appUtil.GetHeaderModel(Request.Headers, out var headerModel);
                using (bLLeadManager = new BLLeadManager())
                {
                    response.Status = (int)AppConstant.APIStatus.SUCCESS;
                    response.Data = await bLLeadManager.UpdateLeadStatus(LeadId, LeadStatusId, LeadStatusName, AssignTo, AssigneeId, headerModel, AssignedToId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = false;
            }

            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }
        [HttpGet]
        [Route("UpdateAssignedTo")]
        public async Task<Response<bool>> UpdateAssignedTo(int LeadId, int AssignedToId, int AssigneeId, int StatusId)
        {
            //_logger.LogInformation("Entered with LeadId {0}, AssignedToId : {1}, AssigneeId : {2}, StatusId : {3} ", LeadId, AssignedToId, AssigneeId, StatusId);

            var response = new Response<bool>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = false
            };

            try
            {
                appUtil.GetHeaderModel(Request.Headers, out var headerModel);
                using (bLLeadManager = new BLLeadManager())
                {
                    response.Status = (int)AppConstant.APIStatus.SUCCESS;
                    response.Data = await bLLeadManager.UpdateAssignedTo(LeadId, AssignedToId, AssigneeId, headerModel, StatusId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = false;
            }

            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }

        #region Property detail
        [HttpPost]
        [Route("LeadPropertyDetailList")]
        public async Task<Response<List<PropertyListResponse>>> LeadPropertyDetailList(PropertyListRequest request)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, JsonConvert.SerializeObject(request));

            var response = new Response<List<PropertyListResponse>>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
            };

            try
            {
                using (bLLeadManager = new BLLeadManager())
                {
                    var data = await bLLeadManager.LeadPropertyDetailList(request);

                    response.Count = data.Item1;
                    response.Data = data.Item2;
                    response.Status = (int)AppConstant.APIStatus.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = null;
            }

            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }
        [Route("InsertLeadPropertyDetail")]
        [HttpPost]
        public async Task<Response<int>> InsertLeadPropertyDetail(LeadPropertyDetail request)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, JsonConvert.SerializeObject(request));

            var response = new Response<int>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = 0
            };
            try
            {
                using (bLLeadManager = new BLLeadManager())
                {
                    appUtil.GetHeaderModel(Request.Headers, out var headerModel);

                    var operationStatus = await bLLeadManager.InsertUpdateDeleteLeadPropertyDetail(0, request, headerModel);
                    response.Status = operationStatus != 0 ? (int)AppConstant.APIStatus.SUCCESS : (int)AppConstant.APIStatus.FAILURE;
                    response.Message = operationStatus != 0 ? AppConstant.InsertMessage : AppConstant.InternalServerError;
                    response.Data = operationStatus;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = 0;
            }
            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }
        [HttpDelete]
        [Route("DeleteMappedPropertyDetail/{id}")]
        public async Task<Response<int>> DeleteMappedPropertyDetail(int id)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, id);

            var response = new Response<int>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = 0
            };
            try
            {
                using (bLLeadManager = new BLLeadManager())
                {
                    appUtil.GetHeaderModel(Request.Headers, out var headerModel);

                    var operationStatus = await bLLeadManager.InsertUpdateDeleteLeadPropertyDetail(id, null, headerModel);
                    response.Status = operationStatus != 0 ? (int)AppConstant.APIStatus.SUCCESS : (int)AppConstant.APIStatus.FAILURE;
                    response.Message = operationStatus != 0 ? AppConstant.DeleteMessage : AppConstant.InternalServerError;
                    response.Data = operationStatus;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = 0;
            }
            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion Property detail

        [HttpGet]
        [Route("GetEmployeeData_Organization")]
        public async Task<Response<DataSet>> GetEmployeeData_Organization()
        {
            //_logger.LogInformation("Entered with id {0}", "GetEmployeeData_Organization");

            var response = new Response<DataSet>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = null
            };

            try
            {
                appUtil.GetHeaderModel(Request.Headers, out var headerModel);
                using (bLLeadManager = new BLLeadManager())
                {
                    response.Status = (int)AppConstant.APIStatus.SUCCESS;
                    response.Data = await bLLeadManager.GetEmployeeData_Organization(headerModel.Uid, headerModel.Cid, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = null;
            }

            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpGet]
        [Route("AssignMultiple")]
        public async Task<Response<bool>> AssignMultiple(string LeadIds, int AssignToId)
        {
            //_logger.LogInformation("Entered with LeadIds {0}, AssignToId : {1}", LeadIds, AssignToId);

            var response = new Response<bool>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = false
            };
            try
            {
                appUtil.GetHeaderModel(Request.Headers, out var headerModel);

                using (bLLeadManager = new BLLeadManager())
                {
                    var operationStatus = await bLLeadManager.AssignMultiple(LeadIds, AssignToId, headerModel.Uid);
                    response.Status = operationStatus ? (int)AppConstant.APIStatus.SUCCESS : (int)AppConstant.APIStatus.FAILURE;
                    response.Message = operationStatus ? string.Empty : AppConstant.InternalServerError;

                    response.Data = true;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = false;
            }

            //_logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }


        internal Model.DefaultHeader GetHeaderModel(string UserName)
        {
            var headerModel = new Model.DefaultHeader();
            try
            {
                var objEmployee = BLEmployee.EmployeeCollectionFromSearchFields(
                     new BLEmployee
                     {
                         IsDelete = false,
                         UserName = UserName
                     }).FirstOrDefault();
                if (objEmployee != null)
                {
                    headerModel.Cid = Convert.ToInt32(objEmployee.Cid);
                    headerModel.Bid = Convert.ToInt32(objEmployee.Bid);
                    headerModel.Uid = Convert.ToInt32(objEmployee.Id);
                    headerModel.IsFrom = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return headerModel;
        }
        public class APILead
        {
            public string APIUserName { get; set; }
            public string LeadDate { get; set; }
            public string MobileNo { get; set; }
            public string FirstName { get; set; }
            public string Email { get; set; }
            public string Requirement { get; set; }
        }
        [AllowAnonymous]//for disable authentication and headers
        [HttpPost]
        [Route("HousingPushAPI")]
        public async Task<Response<int>> HousingPushAPI(APILead APIRequest)
        {
            _logger.LogInformation("HousingPushAPI request :- ", JsonConvert.SerializeObject(APIRequest));

            var response = new Response<int>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = 0
            };
            try
            {
                var headerModel = GetHeaderModel(APIRequest.APIUserName);

                var request = new Lead();
                request.LeadDate = APIRequest.LeadDate;
                request.MobileNo = APIRequest.MobileNo;
                request.FirstName = APIRequest.FirstName;
                request.Email = APIRequest.Email;
                request.Requirement = APIRequest.Requirement;

                using (bLLeadManager = new BLLeadManager())
                {
                    //appUtil.GetHeaderModel(Request.Headers, out var headerModel);
                    var operationStatus = await bLLeadManager.DumpLeadAPIData(0, request, headerModel);
                    response.Status = operationStatus != 0 ? (int)AppConstant.APIStatus.SUCCESS : (int)AppConstant.APIStatus.FAILURE;
                    response.Message = operationStatus != 0 ? AppConstant.InsertMessage : AppConstant.InternalServerError;
                    response.Data = operationStatus;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = (int)AppConstant.APIStatus.FAILURE;
                response.Message = ex.Message;
                response.Data = 0;
            }
            // _logger.LogInformation(AppConstant.ResponseMessage, JsonConvert.SerializeObject(response));

            return response;
        }


    }
}
