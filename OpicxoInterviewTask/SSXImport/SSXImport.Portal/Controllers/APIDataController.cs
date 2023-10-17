using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SSXImport.Portal.Models;
using SSXImport.Portal.Common;
using Newtonsoft.Json;
using static SSXImport.Portal.Models.APIModel;
using Microsoft.Extensions.Logging;

namespace SSXImport.Portal.Controllers
{
    /// <summary>
    /// Used for API Data related Operations
    /// </summary>
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
        /// View Page for API Template List
        /// </summary>
        /// <returns></returns>
        public IActionResult APIData()
        {
            _logger.LogInformation("APIData Called");

            return View();
        }

        /// <summary>
        /// Get List of API Template
        /// </summary>
        /// <returns>List of API Template</returns>
        [HttpPost]
        public async Task<JsonResult> GetAPIDataList()
        {
            _logger.LogInformation("Entered GetAPIDataList");
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

                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("sortCol", (object)Convert.ToInt32(Request.Form["order[0][column]"])));
                request.Add(Tuple.Create("sortDir", (object)Request.Form["order[0][dir]"]));
                request.Add(Tuple.Create("pageFrom", (object)Convert.ToInt32(Request.Form["start"])));
                request.Add(Tuple.Create("pageSize", (object)Convert.ToInt32(Request.Form["length"])));
                request.Add(Tuple.Create("keyword", (object)ToNullableString(Request.Form["keyword"])));

                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<List<APIDataList>>>(
                    await new APIManager(_logger).CallGetMethod("apidata/list", request));

                #endregion

                response = Json(new
                {
                    draw = draw,
                    recordsTotal = baseResponse.Count,
                    recordsFiltered = baseResponse.Count,
                    data = baseResponse.Data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            _logger.LogInformation("Exited GetAPIDataList");

            return response;
        }

        /// <summary>
        /// View Page of API Template Form
        /// </summary>
        /// <param name="key">API Template GUID in case of Edit</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> APIDataForm(string key)
        {
            _logger.LogInformation("Entered APIDataForm with key : {0}", key);

            #region Default

            ModelState.Clear();

            var APIData = new APIData();

            #endregion

            try
            {
                #region Dropdown Binding

                ViewBag.APITypeList = AppCommon.GetAPITypeList();
                ViewBag.APIDataAuthorizationTypeList = AppCommon.GetAPIDataAuthorizationTypeList();
                ViewBag.InputParamTypeList = AppCommon.GetInputParameterTypeList();
                ViewBag.InputParamBodyTypeList = AppCommon.GetInputParameterBodyTypeList();
                ViewBag.APITemplateList = new AppCommon(_logger).GetAPITemplateList();
                ViewBag.OutPutTypeList = AppCommon.GetOutputParameterTypeList();

                #endregion
                
                #region Get API Template Details

                if (!string.IsNullOrEmpty(key))
                {
                    var request = new List<Tuple<string, object>>();
                    request.Add(Tuple.Create("apiGuid", (object)key));

                    var baseResponse = JsonConvert.DeserializeObject<Response<APIData>>(
                        await new APIManager(_logger).CallGetMethod("apidata/edit", request));

                    APIData = baseResponse.Data;

                    if (baseResponse.Status.Equals(0))
                    {
                        TempData["type"] = baseResponse.MessageType;
                        TempData["msg"] = baseResponse.Message;
                        return RedirectToAction("APIDataForm");
                    }
                }

                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            #region Dropdown Binding

            APIData.APITypeList = APIData.APITypeList != null ? APIData.APITypeList : new List<MasterItem>();

            #endregion

            _logger.LogInformation("Exited APIDataForm with response : {0}", JsonConvert.SerializeObject(APIData));

            return View(APIData);
        }

        /// <summary>
        /// Insert / Update of API Template
        /// </summary>
        /// <param name="APIData">APIData object</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ManageAPIData(APIData APIData)
        {
            _logger.LogInformation("Entered ManageAPIData");

            var messageType = string.Empty;
            var message = string.Empty;
            var apiGUID = string.Empty;
            var status = 0;
            try
            {
                var baseResponse = JsonConvert.DeserializeObject<Response<string>>(
                    await new APIManager(_logger).CallPostMethod("apidata/manage", APIData));

                status = baseResponse.Status.GetValueOrDefault(0);
                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
                apiGUID = baseResponse.Data;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited ManageAPIData");

            return Json(new
            {
                status,
                messageType,
                message,
                apiGUID,
            });
        }

        /// <summary>
        /// Delete API Template
        /// </summary>
        /// <param name="key">Template GIUID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> DeleteAPIData(string key)
        {
            _logger.LogInformation("Entered DeleteAPIData with Key : {0}", key);

            var message = "Something Went Wrong!";
            var messageType = AppConstant.AlertErrorType;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var organization = new APIData();
                    organization.APIGUID = key;
                    organization.IsDelete = true;

                    var baseResponse = JsonConvert.DeserializeObject<Response<object>>(
                        await new APIManager(_logger).CallPostMethod("apidata/manage", organization));

                    messageType = baseResponse.MessageType;
                    message = baseResponse.Message;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            
            _logger.LogInformation("Exited DeleteAPIData with response : {0}", JsonConvert.SerializeObject(new { messageType, message }));

            return Json(new { messageType, message });
        }

        /// <summary>
        /// Get API Input Parameter List
        /// </summary>
        /// <returns>Returns List of API Input Parameter</returns>
        [HttpPost]
        public async Task<JsonResult> GetInputParameterList()
        {
            _logger.LogInformation("Entered GetInputParameterList");
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

                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("sortCol", (object)Convert.ToInt32(Request.Form["order[0][column]"])));
                request.Add(Tuple.Create("sortDir", (object)Request.Form["order[0][dir]"]));
                request.Add(Tuple.Create("pageFrom", (object)Convert.ToInt32(Request.Form["start"])));
                request.Add(Tuple.Create("pageSize", (object)Convert.ToInt32(Request.Form["length"])));
                request.Add(Tuple.Create("keyword", (object)ToNullableString(Request.Form["keyword"])));
                request.Add(Tuple.Create("apiGUID", (object)ToNullableString(Request.Form["apiGUID"])));

                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<List<APIInputParameterList>>>(
                    await new APIManager(_logger).CallGetMethod("apidata/inputparam/list", request));

                #endregion

                response = Json(new
                {
                    draw = draw,
                    recordsTotal = baseResponse.Count,
                    recordsFiltered = baseResponse.Count,
                    data = baseResponse.Data
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited GetInputParameterList");

            return response;
        }

        /// <summary>
        /// Insert / Update / Delete API Input Parameter Details
        /// </summary>
        /// <param name="InputParameterDetail">APIInputParameter object</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ManageInputParameterDetail(APIInputParameter InputParameterDetail)
        {
            _logger.LogInformation("Entered ManageInputParameterDetail");

            var messageType = string.Empty;
            var message = string.Empty;
            try
            {
                var baseResponse = JsonConvert.DeserializeObject<Response<string>>(
                    await new APIManager(_logger).CallPostMethod("apidata/inputparam/manage", InputParameterDetail));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited ManageInputParameterDetail");

            return Json(new
            {
                messageType,
                message,
            });
        }

        /// <summary>
        /// Get API Output Parameter List
        /// </summary>
        /// <returns>Returns List of API Output Parameter</returns>
        [HttpPost]
        public async Task<JsonResult> GetOutputParameterList()
        {
            _logger.LogInformation("Entered GetOutputParameterList");
            var response = Json(new
            {
                draw = 0,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = string.Empty
            });
            try
            {
                #region Initialize output parameter of SP
                var draw = Convert.ToInt32(Request.Form["draw"]);

                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("sortCol", (object)Convert.ToInt32(Request.Form["order[0][column]"])));
                request.Add(Tuple.Create("sortDir", (object)Request.Form["order[0][dir]"]));
                request.Add(Tuple.Create("pageFrom", (object)Convert.ToInt32(Request.Form["start"])));
                request.Add(Tuple.Create("pageSize", (object)Convert.ToInt32(Request.Form["length"])));
                request.Add(Tuple.Create("keyword", (object)ToNullableString(Request.Form["keyword"])));
                request.Add(Tuple.Create("apiGUID", (object)ToNullableString(Request.Form["apiGUID"])));

                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<List<APIOutputParameterList>>>(
                    await new APIManager(_logger).CallGetMethod("apidata/outputparam/list", request));

                #endregion

                response = Json(new
                {
                    draw = draw,
                    recordsTotal = baseResponse.Count,
                    recordsFiltered = baseResponse.Count,
                    data = baseResponse.Data
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited GetOutputParameterList");

            return response;
        }

        /// <summary>
        /// Insert / Update / Delete API Output Parameter Details
        /// </summary>
        /// <param name="OutputParameterDetail">APIOutputParameter object</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ManageOutputParameterDetail(APIOutputParameter OutputParameterDetail)
        {
            _logger.LogInformation("Entered ManageOutputParameterDetail");

            var messageType = string.Empty;
            var message = string.Empty;
            try
            {
                var baseResponse = JsonConvert.DeserializeObject<Response<string>>(
                    await new APIManager(_logger).CallPostMethod("apidata/outputparam/manage", OutputParameterDetail));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited ManageOutputParameterDetail");

            return Json(new
            {
                messageType,
                message,
            });
        }

        /// <summary>
        /// Execute API based of API Template GUID
        /// </summary>
        /// <param name="key">API Template GUID</param>
        /// <returns>Actual Result returned by API after execution</returns>
        [HttpGet]
        public async Task<IActionResult> RunAPI(string key)
        {
            _logger.LogInformation("Entered RunAPI with key : {0}", key);

            ModelState.Clear();
            var objAPIData = new APIExecutionResponse();

            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var request = new List<Tuple<string, object>>();
                    request.Add(Tuple.Create("apiGuid", (object)key));

                    var baseResponse = JsonConvert.DeserializeObject<APIExecutionResponse>(
                        await new APIManager(_logger).CallGetMethod("apidata/executeapi", request));

                    if (baseResponse.Status.Equals(1))
                    {
                        objAPIData.Name = baseResponse.Name;

                        objAPIData.APIEndPoint = baseResponse.APIEndPoint;

                        objAPIData.APIResultData = baseResponse.APIResultData?.Trim();

                        _logger.LogInformation("Exited RunAPI with Result");

                        return View(objAPIData);
                    }
                    else
                    {
                        TempData["type"] = baseResponse.MessageType;
                        TempData["msg"] = baseResponse.Message;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited RunAPI with Failure");

            return RedirectToAction("APIData");
        }
    }
}
