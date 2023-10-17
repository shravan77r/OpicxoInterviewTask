using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using SSXImport.Business;
using SSXImport.Business.ImplementationManager;
using SSXImport.WebAPI.Models;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Cors;
using System.Net.Http.Headers;
using SSXImport.WebAPI.Common;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using SSXImport.WebAPI.Controllers;
using Microsoft.Extensions.Logging;
using SSXImport.WebAPI.Models.General;

namespace SSXImport.WebAPI.Controllers
{
    /// <summary>
    /// Used For API Data Related API's
    /// </summary>
    [Route("api/apidata")]
    [ApiController]
    public class APIDataController : BaseController
    {
        private readonly ILogger<APIDataController> _logger;

        /// <summary>
        /// Initialize ILogger for logging purpose
        /// </summary>
        /// <param name="logger"></param>
        public APIDataController(ILogger<APIDataController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Executes the API based of ApiGuid
        /// </summary>
        /// <param name="apiGuid">GUID of API which needs to be executed</param>
        /// <returns>Returns Object which has been returned by actual called API</returns> 
        [HttpGet]
        [Route("executeapi")]
        [ApiVersion("1.0")]
        public virtual object ExecuteAPI(string apiGuid)
        {
            _logger.LogInformation("Entered ExecuteAPI");
            var objResponse = new APIExecutionResponse()
            {
                Status = 0,
            };
            try
            {
                objResponse = new APIManager(_logger).ExecuteAPI(apiGuid);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                objResponse.MessageType = AppConstant.AlertErrorType;
                objResponse.Message = e.Message;
            }
            _logger.LogInformation("Exited ExecuteAPI");
            return objResponse;
        }

        /// <summary>
        /// Get API List based on requested parameters
        /// </summary>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageFrom"></param>
        /// <param name="keyword"></param>
        /// <returns>Returns API List DataTable</returns>
        [HttpGet]
        [Route("list")]
        [ApiVersion("1.0")]
        public virtual Response GetAPIList(int sortCol = 0, string sortDir = "desc", int pageSize = int.MaxValue, int pageFrom = 0, string keyword = null)
        {
            _logger.LogInformation("Entered GetAPIList");
            _logger.LogDebug("Entered GetAPIList with Parameters sortCol: {0},sortDir: {1},pageSize: {2},pageFrom: {3},keyword: {4}",
                sortCol, sortDir, pageSize, pageFrom, keyword);
            try
            {
                var data = BLAPIDataManager.GetAPIList(
                    sortCol,
                    sortDir,
                    pageFrom,
                    pageSize,
                    keyword
                    );

                var dt = data.Item2;

                response = GetDataTableResponse(dt);

                response.Count = data.Item1;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited GetAPIList with response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetAPIList");
            return response;
        }

        /// <summary>
        /// Insert / Update / Delete API Data
        /// </summary>
        /// <param name="form">APIData object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("manage")]
        [ApiVersion("1.0")]
        public virtual Response ManageAPIData(APIData form)
        {
            _logger.LogInformation("Entered ManageAPIData");
            _logger.LogDebug("Entered ManageAPIData with Request : {0}", JsonConvert.SerializeObject(form));
            try
            {
                bool isEdit = false;
                BLApi data = null;
                var oldFTPFileName = string.Empty;

                #region Check Record Existence
                if (!string.IsNullOrEmpty(form.APIGUID))
                {
                    data = new BLApi(form.APIGUID);

                    isEdit = true;
                    _logger.LogDebug("Existing Record to update");
                }
                else
                {
                    data = new BLApi();
                    isEdit = false;
                    _logger.LogDebug("New Record to Insert");
                }

                if (isEdit && data == null)
                {
                    _logger.LogError("API Record not found in Database for given API GUID : {0}", form.APIGUID);
                    _logger.LogDebug("Exited ManageAPIData with API Record not found in Database");
                    _logger.LogInformation("Exited ManageAPIData");
                    return GetNoRecordFound();
                }
                #endregion

                #region Delete Operation
                if (form.IsDelete.Equals(true))
                {
                    _logger.LogDebug("Existing Record to Delete");

                    BLAPIDataManager.DeleteAPI(Guid.Parse(data.Apiguid), form.UpdatedBy.Value);
                    response = GetSuccessResponse(data.APIId, Message: AppConstant.MessageDelete);

                    _logger.LogDebug("Record Deleted");
                }
                #endregion

                #region Main Operation
                else
                {
                    #region Insert / Update Operation

                    if (form.IsDelete == false)
                    {

                        #region Check Already Exist
                        var IsDuplicate = false;

                        var count = new BLApi().Collection<BLApi>(
                              new Criteria<BLApi>()
                                  .Add(Expression.Eq("Name", form.Name.Trim()))
                                  .Add(Expression.Eq("IsDelete", false))
                                  ).Count(o => o.Apiguid != form.APIGUID);

                        if (count > 0)
                            IsDuplicate = true;

                        if (IsDuplicate == true)
                        {
                            _logger.LogError("API Name {0} already Exists in Database", form.Name.Trim());
                            return GetNoRecordFound("API Name is already exist.");
                        }
                        #endregion

                        data.Name = form.Name;
                        data.APIEndPoint = form.APIEndPoint;
                        data.Type = form.Type;
                        data.Description = form.Description;
                        data.AuthorizationType = form.AuthorizationType;
                        data.AuthorizationUsername = form.AuthorizationUsername;
                        data.AuthorizationPassword = form.AuthorizationPassword;
                        data.InputParameterType = form.InputParameterType;
                        data.BodyParameterType = form.BodyParameterType;
                        data.OutPutParameterJson = form.OutPutParameterJson;
                        data.IsActive = form.IsActive;
                        data.AuthorizationOathAPIId = form.AuthorizationOathAPIId;
                        data.AuthorizationTokenName = form.AuthorizationTokenName;

                    }
                    data.IsDelete = form.IsDelete;
                    data.UpdatedBy = form.UpdatedBy;
                    data.UpdatedDate = GetDateTime();

                    if (isEdit)
                    {
                        data.Update();
                        _logger.LogDebug("Record Updated");
                    }
                    else
                    {
                        data.Apiguid = GetGUID();
                        data.EnteredDate = GetDateTime();
                        data.EnteredBy = form.EnteredBy;
                        data.SaveNew();
                        _logger.LogDebug("Record Inserted with Id :{0}", data.APIId);
                    }
                    #endregion

                    #region Generate Response
                    response = GetSuccessResponse(data.Apiguid,
                        Message: isEdit
                            ? AppConstant.MessageUpdate
                            : AppConstant.MessageInsert);
                    #endregion
                }
                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited ManageAPIData with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited ManageAPIData");
            return response;
        }

        /// <summary>
        /// Get API Data Details By API GUID
        /// </summary>
        /// <param name="apiGuid">API GUID to fetch Data</param>
        /// <returns></returns>
        [HttpGet]
        [Route("edit")]
        [ApiVersion("1.0")]
        public virtual Response GetAPIDataDetails(Guid apiGuid)
        {
            _logger.LogInformation("Entered GetAPIDataDetails");
            _logger.LogDebug("Entered GetAPIDataDetails with API GUID : {0}", apiGuid);
            try
            {
                var details = new APIManager(_logger).GetAPIData(apiGuid.ToString()).Item1;

                if (details != null)
                {
                    response = GetSuccessResponse(details);
                }
                else
                {
                    _logger.LogError("API Details not found in database for API GUID : {0}", apiGuid);
                    _logger.LogDebug("API Details not found in database for API GUID : {0}", apiGuid);
                    response = GetNoRecordFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }

            _logger.LogDebug("Exited GetAPIDataDetails with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetAPIDataDetails");
            return response;
        }

        /// <summary>
        /// Insert / Update / Delete API Input Parameter Details
        /// </summary>
        /// <param name="form">APIInputParameter object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("inputparam/manage")]
        [ApiVersion("1.0")]
        public virtual Response ManageAPIInputParameterDetail(APIInputParameter form)
        {
            _logger.LogInformation("Entered ManageAPIInputParameterDetail");
            _logger.LogDebug("Entered ManageAPIInputParameterDetail with Request : {0}", JsonConvert.SerializeObject(form));
            try
            {
                bool isEdit = false;
                BLAPIInputParameter data = null;

                #region Check Record Existence
                if (!string.IsNullOrEmpty(form.InputParameterGUID))
                {
                    data = new BLAPIInputParameter(form.InputParameterGUID);
                    isEdit = true;
                    if (form.IsDelete.Equals(true))
                        _logger.LogDebug("Existing Record to Delete");
                    else
                        _logger.LogDebug("Existing Record to Update");
                }
                else
                {
                    data = new BLAPIInputParameter();
                    isEdit = false;
                    _logger.LogDebug("New Record to Insert");
                }

                if (isEdit && data == null)
                {
                    _logger.LogError("API Record not found in Database for given API Input Parameter GUID : {0}", form.InputParameterGUID);
                    _logger.LogDebug("Exited ManageAPIInputParameterDetail with API Record not found in Database");
                    _logger.LogInformation("Exited ManageAPIInputParameterDetail");
                    return GetNoRecordFound();
                }
                #endregion

                #region Insert / Update / Delete operation
                if (form.IsDelete.Equals(false))
                {
                    #region Fetch APIInputData from APIIdGUID

                    form.APIId = AppCommon.GetAPIIdByAPIGUID(form.APIGUID);

                    #endregion

                    #region Check Already Exist
                    var IsDuplicate = false;

                    var count = new BLAPIInputParameter().Collection<BLAPIInputParameter>(
                          new Criteria<BLAPIInputParameter>()
                              .Add(Expression.Eq("APIId", form.APIId))
                              .Add(Expression.Eq("KeyColumn", form.KeyColumn))
                              .Add(Expression.Eq("IsDelete", false))
                              ).Count();

                    if (count > 0)
                        IsDuplicate = true;

                    if (IsDuplicate == true)
                        return GetNoRecordFound("Input Parameter Key : (" + form.KeyColumn.Trim() + ") is already exist.");


                    #endregion

                    data.APIId = form.APIId;
                    data.InputParameterTypeId = form.InputParameterTypeId;
                    data.BodyType = form.BodyType;
                    data.KeyColumn = form.KeyColumn;
                    data.ValueColumn = form.ValueColumn;
                    data.IsActive = form.IsActive;
                }

                data.IsDelete = form.IsDelete;
                data.UpdatedBy = form.UpdatedBy;
                data.UpdatedDate = GetDateTime();

                if (isEdit)
                {
                    data.Update();
                    if (form.IsDelete.Equals(true))
                        _logger.LogDebug("Record Deleted with Id :{0}", data.InputParameterId);
                    else
                        _logger.LogDebug("Record Updated with Id :{0}", data.InputParameterId);
                }
                else
                {
                    data.InputParameterGUID = GetGUID();
                    data.EnteredDate = GetDateTime();
                    data.EnteredBy = form.EnteredBy;
                    data.SaveNew();
                    _logger.LogDebug("Record Inserted with Id :{0}", data.InputParameterId);
                }
                #endregion

                #region Generate Response
                if (data.IsDelete == true)
                    response = GetSuccessResponse(data.InputParameterGUID, Message: AppConstant.MessageDelete);
                else
                    response = GetSuccessResponse(data.InputParameterGUID, Message: isEdit
                        ? AppConstant.MessageUpdate
                        : AppConstant.MessageInsert);
                #endregion

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited ManageAPIData with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited ManageAPIData");
            return response;
        }

        /// <summary>
        /// Get API Input Parameter List based on requested parameters
        /// </summary>
        /// <param name="apiGUID"></param>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageFrom"></param>
        /// <param name="keyword"></param>
        /// <returns>Returns API Input Parameter List DataTable</returns>
        [HttpGet]
        [Route("inputparam/list")]
        [ApiVersion("1.0")]
        public virtual Response GetAPIInputParameterDetailList([Required] string apiGUID, int sortCol = 0, string sortDir = "desc",
            int pageSize = int.MaxValue, int pageFrom = 0, string keyword = null)
        {
            _logger.LogInformation("Entered GetAPIInputParameterDetailList");
            _logger.LogDebug("Entered GetAPIInputParameterDetailList with Parameters apiGUID:{5}, sortCol: {0},sortDir: {1},pageSize: {2},pageFrom: {3},keyword: {4}",
                sortCol, sortDir, pageSize, pageFrom, keyword, apiGUID);
            try
            {
                var dt = BLAPIDataManager.GetAPIInputParameterDetailsList(apiGUID);

                response = GetDataTableResponse(dt);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited GetAPIInputParameterDetailList with response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetAPIInputParameterDetailList");
            return response;
        }

        /// <summary>
        /// Insert / Update / Delete API Output Parameter Details
        /// </summary>
        /// <param name="form">APIOutputParameter object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("outputparam/manage")]
        [ApiVersion("1.0")]
        public virtual Response ManageAPIOutputParameterDetail(APIOutputParameter form)
        {
            _logger.LogInformation("Entered ManageAPIOutputParameterDetail");
            _logger.LogDebug("Entered ManageAPIOutputParameterDetail with Request : {0}", JsonConvert.SerializeObject(form));
            try
            {
                bool isEdit = false;
                BLAPIOutputParameter data = null;

                #region Check Record Existence
                if (!string.IsNullOrEmpty(form.OutputParameterGUID))
                {
                    data = new BLAPIOutputParameter(form.OutputParameterGUID);
                    isEdit = true;
                    if (form.IsDelete.Equals(true))
                        _logger.LogDebug("Existing Record to Delete");
                    else
                        _logger.LogDebug("Existing Record to Update");
                }
                else
                {
                    data = new BLAPIOutputParameter();
                    isEdit = false;
                    _logger.LogDebug("New Record to Insert");
                }

                if (isEdit && data == null)
                {
                    _logger.LogError("API Record not found in Database for given API Input Parameter GUID : {0}", form.OutputParameterGUID);
                    _logger.LogDebug("Exited ManageAPIOutputParameterDetail with API Record not found in Database");
                    _logger.LogInformation("Exited ManageAPIOutputParameterDetail");
                    return GetNoRecordFound();
                }
                #endregion

                #region Insert / Update / Delete operation
                if (form.IsDelete.Equals(false))
                {
                    #region Fetch APIOutputData from APIId

                    form.APIId = AppCommon.GetAPIIdByAPIGUID(form.APIGUID);

                    #endregion

                    #region Check Already Exist
                    var IsDuplicate = false;

                    var count = new BLAPIOutputParameter().Collection<BLAPIOutputParameter>(
                          new Criteria<BLAPIOutputParameter>()
                              .Add(Expression.Eq("APIId", form.APIId))
                              .Add(Expression.Eq("KeyColumn", form.KeyColumn))
                              .Add(Expression.Eq("IsDelete", false))
                              ).Count();

                    if (count > 0)
                        IsDuplicate = true;

                    if (IsDuplicate == true)
                        return GetNoRecordFound("Output Parameter Key : (" + form.KeyColumn.Trim() + ") is already exist.");


                    #endregion

                    data.APIId = form.APIId;
                    data.KeyColumn = form.KeyColumn;
                    data.Type = form.Type;
                    data.IsActive = form.IsActive;
                }

                data.IsDelete = form.IsDelete;
                data.UpdatedBy = form.UpdatedBy;
                data.UpdatedDate = GetDateTime();

                if (isEdit)
                {
                    data.Update();
                    if (form.IsDelete.Equals(true))
                        _logger.LogDebug("Record Deleted with Id :{0}", data.OutputParameterId);
                    else
                        _logger.LogDebug("Record Updated with Id :{0}", data.OutputParameterId);
                }
                else
                {
                    data.OutputParameterGUID = GetGUID();
                    data.EnteredDate = GetDateTime();
                    data.EnteredBy = form.EnteredBy;
                    data.SaveNew();
                    _logger.LogDebug("Record Inserted with Id :{0}", data.OutputParameterId);

                }
                #endregion

                #region Generate Response
                if (data.IsDelete == true)
                    response = GetSuccessResponse(data.OutputParameterGUID, Message: AppConstant.MessageDelete);
                else
                    response = GetSuccessResponse(data.OutputParameterGUID, Message: isEdit
                        ? AppConstant.MessageUpdate
                        : AppConstant.MessageInsert);
                #endregion

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited ManageAPIData with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited ManageAPIData");
            return response;
        }

        /// <summary>
        /// Get API Output Parameter List based on requested parameters/// 
        /// </summary>
        /// <param name="apiGUID"></param>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageFrom"></param>
        /// <param name="keyword"></param>
        /// <returns>Returns API Output Parameter List DataTable</returns>
        [HttpGet]
        [Route("outputparam/list")]
        [ApiVersion("1.0")]
        public virtual Response GetAPIOutputParameterDetailList([Required] string apiGUID, int sortCol = 0, string sortDir = "desc",
            int pageSize = int.MaxValue, int pageFrom = 0, string keyword = null)
        {
            _logger.LogInformation("Entered GetAPIOutputParameterDetailList");
            _logger.LogDebug("Entered GetAPIOutputParameterDetailList with Parameters apiGUID:{5}, sortCol: {0},sortDir: {1},pageSize: {2},pageFrom: {3},keyword: {4}",
                sortCol, sortDir, pageSize, pageFrom, keyword, apiGUID);
            try
            {
                var dt = BLAPIDataManager.GetAPIOutputParameterDetailsList(apiGUID);
                response = GetDataTableResponse(dt);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited GetAPIOutputParameterDetailList with response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetAPIOutputParameterDetailList");
            return response;
        }

        /// <summary>
        /// Get API Template List for selection in Authentication API Type
        /// </summary>
        /// <returns>Returns List of API Template</returns>
        [HttpPost]
        [ApiVersion("1.0")]
        [Route("apitemplate/list")]
        public virtual Response GetAPITemplateList()
        {
            _logger.LogInformation("Entered GetAPITemplateList");
            _logger.LogDebug("Entered GetAPITemplateList");
            try
            {
                var dbItems = new BLApi().Collection<BLApi>(new Criteria<BLApi>()).Where(o => o.IsDelete.Equals(false)).ToList();

                if (dbItems != null && dbItems.Count > 0)
                {
                    var items = new List<MasterItem>();

                    foreach (var dbItem in dbItems)
                    {
                        items.Add(new MasterItem
                        {
                            Id = ToStr(dbItem.APIId),
                            Name = ToStr(dbItem.Name)
                        });
                    }

                    response = GetSuccessResponse(items, items.Count);
                }
                else
                    response = GetNoRecordFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited GetAPITemplateList with response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetAPITemplateList");

            return response;
        }

    }
}
