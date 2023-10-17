using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SSXImport.Portal.Common;
using SSXImport.Portal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static SSXImport.Portal.Models.APIModel;
namespace SSXImport.Portal.Controllers
{
    public class TemplateController : BaseController
    {
        private readonly ILogger<TemplateController> _logger;
        private string applicationGuid;
        /// <summary>
        /// Initialize ILogger for logging purpose
        /// </summary>
        /// <param name="logger"></param>
        public TemplateController(ILogger<TemplateController> logger)
        {
            _logger = logger;
            //applicationGuid = HttpContext.Session.GetString("ClientGUId");
        }

        /// <summary>
        /// View Page of Template List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Template()
        {
            _logger.LogInformation("Template Called");

            var applicationGuid = HttpContext.Session.GetString("ClientGUId");
            ViewBag.applicationGuid = applicationGuid;
            ViewData["key1"] = "some value-1";
            return View();
        }

        /// <summary>
        /// Get Template List
        /// </summary>
        /// <returns>Returns List of Template</returns>
        [HttpPost]
        public async Task<JsonResult> GetTemplateList()
        {





            _logger.LogInformation("Entered GetTemplateList");

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
                request.Add(Tuple.Create("ApplicationGuid", (object)applicationGuid));


                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<List<TemplateList>>>(
                    await new APIManager(_logger).CallGetMethod("template/list", request));

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
            _logger.LogInformation("Exited GetTemplateList");

            return response;
        }

        /// <summary>
        /// View form of Template
        /// </summary>
        /// <param name="key">Template GUID for edit time</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> TemplateForm(string key)
        {
            _logger.LogInformation("Entered TemplateForm with key : {0}", key);

            #region Default

            ModelState.Clear();

            var template = new Template();
            template.IsActive = true;
            template.OriginSourceTypeId = 2;
            template.TargetSourceTypeId = 2;
            template.IsValidOriginConnection = 0;
            template.IsValidTargetConnection = 0;
            template.OriginConnectionString = string.Empty;
            template.TargetConnectionString = string.Empty;

            #endregion

            try
            {
                #region Dropdown Binding

                ViewBag.EmptyList = AppCommon.BindEmptyList();
                ViewBag.TemplateTypeList = AppCommon.GetTemplateTypeList();
                ViewBag.OriginDataSourceList = AppCommon.GetDataSourceList();
                ViewBag.TargetDataSourceList = AppCommon.GetDataSourceList(false);
                ViewBag.APITemplateList = new AppCommon(_logger).GetAPITemplateList();

                #endregion

                #region Get Template Details
                if (!string.IsNullOrEmpty(key))
                {
                    var request = new List<Tuple<string, object>>();
                    request.Add(Tuple.Create("templateGuid", (object)key));

                    var baseResponse = JsonConvert.DeserializeObject<Response<Template>>(
                        await new APIManager(_logger).CallGetMethod("template/edit", request));

                    template = baseResponse.Data;

                    if (baseResponse.Status.Equals(0))
                    {
                        TempData["type"] = baseResponse.MessageType;
                        TempData["msg"] = baseResponse.Message;
                        return RedirectToAction("TemplateForm");
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            #region Default Dropdown Binding

            template.OriginSourceDatabaseList = template.OriginSourceDatabaseList != null ? template.OriginSourceDatabaseList : new List<MasterItem>();
            template.OriginSourceFileNameList = template.OriginSourceFileNameList != null ? template.OriginSourceFileNameList : new List<MasterItem>();
            template.OriginSourceFilePathList = template.OriginSourceFilePathList != null ? template.OriginSourceFilePathList : new List<MasterItem>();
            template.OriginSourceTableList = template.OriginSourceTableList != null ? template.OriginSourceTableList : new List<MasterItem>();
            template.TargetSourceDatabaseList = template.TargetSourceDatabaseList != null ? template.TargetSourceDatabaseList : new List<MasterItem>();
            template.TargetSourceTableList = template.TargetSourceTableList != null ? template.TargetSourceTableList : new List<MasterItem>();

            #endregion

            _logger.LogInformation("Exited TemplateForm with response : {0}", JsonConvert.SerializeObject(template));

            return View(template);
        }

        /// <summary>
        /// Insert / Update Template
        /// </summary>
        /// <param name="template">Template object</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ManageTemplate(Template template)
        {
            _logger.LogInformation("Entered ManageTemplate");

            var messageType = string.Empty;
            var message = string.Empty;
            var templateGUID = string.Empty;
            var status = 0;
            try
            {
                var baseResponse = JsonConvert.DeserializeObject<Response<string>>(
                    await new APIManager(_logger).CallPostMethod("template/manage", template));

                status = baseResponse.Status.GetValueOrDefault(0);
                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
                templateGUID = baseResponse.Data;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited ManageTemplate");

            return Json(new
            {
                status,
                messageType,
                message,
                templateGUID,
            });
        }

        /// <summary>
        /// Delete Template based on Template GUID
        /// </summary>
        /// <param name="key">Template GUID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> DeleteTemplate(string key)
        {
            _logger.LogInformation("Entered DeleteTemplate with Key : {0}", key);

            var message = "Something Went Wrong!";
            var messageType = AppConstant.AlertErrorType;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var organization = new Template();
                    organization.TemplateGUID = key;
                    organization.IsDelete = true;

                    var baseResponse = JsonConvert.DeserializeObject<Response<object>>(
                        await new APIManager(_logger).CallPostMethod("template/manage", organization));

                    messageType = baseResponse.MessageType;
                    message = baseResponse.Message;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited DeleteTemplate with response : {0}", JsonConvert.SerializeObject(new { messageType, message }));

            return Json(new { messageType, message });
        }

        /// <summary>
        /// Validate Connection based on provided credentials
        /// </summary>
        /// <param name="connection">Connection object containing credentials</param>
        /// <returns>Returns connection status along with Databases List/Files List</returns>
        [HttpPost]
        public async Task<JsonResult> ValidateConnection(Connection connection)
        {
            _logger.LogInformation("Entered ValidateConnection");

            var isValidConnection = 0;
            var connectionString = string.Empty;
            var messageType = string.Empty;
            var message = string.Empty;
            var databases = string.Empty;
            var directories = string.Empty;
            var files = string.Empty;
            try
            {
                var baseResponse = JsonConvert.DeserializeObject<Response<Connection>>(
                    await new APIManager(_logger).CallPostMethod("connection/checkconnection", connection));

                isValidConnection = baseResponse.Status == 1 ? 1 : 2;
                connectionString = baseResponse.Status == 1 ? baseResponse.Data.ConnectionString : string.Empty;
                if (isValidConnection.Equals(1))
                {
                    if (baseResponse.Data.DataSourceId.Equals(1))
                    {
                        var baseResponseDirectories = JsonConvert.DeserializeObject<Response<List<MasterItem>>>(
                        await new APIManager(_logger).CallPostMethod("connection/list/directories", connection)); // All the FTP Directories for given path

                        if (baseResponseDirectories.Status.GetValueOrDefault(0).Equals(1))
                            directories = JsonConvert.SerializeObject(baseResponseDirectories.Data);

                        if (baseResponseDirectories.Status.Equals(1))
                        {
                            connection.OriginSourceFilePath = "/";
                            var baseResponseFiles = JsonConvert.DeserializeObject<Response<List<MasterItem>>>(
                            await new APIManager(_logger).CallPostMethod("connection/list/files", connection)); // All the FTP Directories for given first directory

                            if (baseResponseFiles.Status.GetValueOrDefault(0).Equals(1))
                                files = JsonConvert.SerializeObject(baseResponseFiles.Data);
                        }
                    }
                    else if (baseResponse.Data.DataSourceId.Equals(2) || baseResponse.Data.DataSourceId.Equals(3))
                    {
                        var baseResponseDatabases = JsonConvert.DeserializeObject<Response<List<MasterItem>>>(
                        await new APIManager(_logger).CallPostMethod("connection/list/database", connection)); // All the Databases for given credentials

                        if (baseResponseDatabases.Status.GetValueOrDefault(0).Equals(1))
                            databases = JsonConvert.SerializeObject(baseResponseDatabases.Data);
                    }
                }
                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited ValidateConnection");

            return Json(new
            {
                isValidConnection,
                connectionString,
                messageType,
                message,
                databases,
                directories,
                files
            });
        }

        /// <summary>
        /// Get Tables List
        /// </summary>
        /// <param name="connection">Connection object containing Credentials</param>
        /// <returns>Returns List of Tables</returns>
        [HttpPost]
        public async Task<JsonResult> GetTableNames(Connection connection)
        {
            _logger.LogInformation("Entered GetTableNames");

            var isValidConnection = 0;
            var connectionString = string.Empty;
            var messageType = string.Empty;
            var message = string.Empty;
            var tables = string.Empty;
            try
            {
                var baseResponse = JsonConvert.DeserializeObject<Response<Connection>>(
                    await new APIManager(_logger).CallPostMethod("connection/checkconnection", connection));

                isValidConnection = baseResponse.Status == 1 ? 1 : 2;
                connectionString = baseResponse.Status == 1 ? baseResponse.Data.ConnectionString : string.Empty;
                if (isValidConnection.Equals(1))
                {
                    var baseResponseDatabases = JsonConvert.DeserializeObject<Response<object>>(
                    await new APIManager(_logger).CallPostMethod("connection/list/table", connection));

                    if (baseResponseDatabases.Status.GetValueOrDefault(0).Equals(1))
                        tables = JsonConvert.SerializeObject(baseResponseDatabases.Data);

                    messageType = baseResponseDatabases.MessageType;
                    message = baseResponseDatabases.Message;
                }
                else
                {
                    messageType = baseResponse.MessageType;
                    message = baseResponse.Message;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited GetTableNames");

            return Json(new { isValidConnection, connectionString, messageType, message, tables });
        }

        /// <summary>
        /// Get Sheets from excel file
        /// </summary>
        /// <param name="TemplateGUID">Template GUID</param>
        /// <returns>Returns List of Sheets available in Excel file</returns>
        [HttpPost]
        public async Task<JsonResult> GetSheetNames(string TemplateGUID)
        {
            _logger.LogInformation("Entered GetSheetNames");

            var messageType = string.Empty;
            var message = string.Empty;
            var sheets = string.Empty;
            try
            {
                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("templateGUID", (object)TemplateGUID));

                var baseResponse = JsonConvert.DeserializeObject<Response<List<MasterItem>>>(
                    await new APIManager(_logger).CallGetMethod("template/excel/list/sheet", request));

                if (baseResponse.Status.GetValueOrDefault(0).Equals(1))
                    sheets = JsonConvert.SerializeObject(baseResponse.Data);
                else
                {
                    messageType = baseResponse.MessageType;
                    message = baseResponse.Message;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited GetSheetNames");

            return Json(new { messageType, message, sheets });
        }

        /// <summary>
        /// Get File List for given FTP Directory
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetFileNames(Connection connection)
        {
            _logger.LogInformation("Entered GetFileNames");

            var isValidConnection = 0;
            var messageType = string.Empty;
            var message = string.Empty;
            var files = string.Empty;
            try
            {
                var baseResponse = JsonConvert.DeserializeObject<Response<Connection>>(
                    await new APIManager(_logger).CallPostMethod("connection/checkconnection", connection));

                isValidConnection = baseResponse.Status == 1 ? 1 : 2;
                if (isValidConnection.Equals(1))
                {
                    var baseResponseFiles = JsonConvert.DeserializeObject<Response<List<MasterItem>>>(
                        await new APIManager(_logger).CallPostMethod("connection/list/files", connection));

                    if (baseResponseFiles.Status.GetValueOrDefault(0).Equals(1))
                        files = JsonConvert.SerializeObject(baseResponseFiles.Data);

                    messageType = baseResponseFiles.MessageType;
                    message = baseResponseFiles.Message;
                }
                else
                {
                    messageType = baseResponse.MessageType;
                    message = baseResponse.Message;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited GetFileNames");

            return Json(new
            {
                isValidConnection,
                messageType,
                message,
                files
            });
        }

        /// <summary>
        /// Insert / Update / Delete Template Table Details
        /// </summary>
        /// <param name="templateTableDetail">TemplateTableDetail object</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ManageTemplateTableDetail(TemplateTableDetail templateTableDetail)
        {
            _logger.LogInformation("Entered ManageTemplateTableDetail");

            var messageType = string.Empty;
            var message = string.Empty;
            try
            {
                var baseResponse = JsonConvert.DeserializeObject<Response<string>>(
                    await new APIManager(_logger).CallPostMethod("template/table/manage", templateTableDetail));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited ManageTemplateTableDetail");

            return Json(new
            {
                messageType,
                message,
            });
        }

        /// <summary>
        /// Get Template Table List for given Template GUID
        /// </summary>
        /// <returns>Returns List of Template table</returns>
        [HttpPost]
        public async Task<JsonResult> GetTemplateTableList()
        {
            _logger.LogInformation("Entered GetTemplateTableList");
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
                request.Add(Tuple.Create("templateGUID", (object)ToNullableString(Request.Form["TemplateGUID"])));

                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<List<TemplateTableDetailList>>>(
                    await new APIManager(_logger).CallGetMethod("template/table/list", request));

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

            _logger.LogInformation("Exited GetTemplateTableList");

            return response;
        }

        /// <summary>
        /// Update Table Row Order details for execution
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UpdateTemplateTableOrder()
        {
            _logger.LogInformation("Entered UpdateTemplateTableOrder");

            var messageType = string.Empty;
            var message = string.Empty;
            try
            {
                #region Initialize Input parameter of SP

                var UpdatedTemplateTableRowsDetails = ToNullableString(Request.Form["UpdatedTemplateTableRowsDetails"]);
                var request = new List<TemplateReOrderTableDetail>();
                if (!string.IsNullOrEmpty(UpdatedTemplateTableRowsDetails))
                    request = JsonConvert.DeserializeObject<List<TemplateReOrderTableDetail>>(UpdatedTemplateTableRowsDetails);

                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<object>>(
                    await new APIManager(_logger).CallPostMethod("template/table/updateorder", request));

                #endregion

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited UpdateTemplateTableOrder");

            return Json(new
            {
                messageType,
                message,
            });
        }

        /// <summary>
        /// Insert / Update / Delete Template Column Detail
        /// </summary>
        /// <param name="templateColumnDetail">TemplateColumnDetail object</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ManageTemplateColumnDetail(TemplateColumnDetail templateColumnDetail)
        {
            _logger.LogInformation("Entered ManageTemplateColumnDetail");

            var messageType = string.Empty;
            var message = string.Empty;
            try
            {
                var baseResponse = JsonConvert.DeserializeObject<Response<string>>(
                    await new APIManager(_logger).CallPostMethod("template/column/manage", templateColumnDetail));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited ManageTemplateColumnDetail");

            return Json(new
            {
                messageType,
                message,
            });
        }

        /// <summary>
        /// Get Template Column List for given Template GUID and Template Table GUID
        /// </summary>
        /// <returns>Returns list of Template Columns</returns>
        [HttpPost]
        public async Task<JsonResult> GetTemplateColumnList()
        {
            _logger.LogInformation("Entered GetTemplateColumnList");

            var messageType = string.Empty;
            var message = string.Empty;
            try
            {
                #region Initialize Input parameter of SP

                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("templateGUID", (object)ToNullableString(Request.Form["TemplateGUID"])));
                request.Add(Tuple.Create("templateTableDetailGUID", (object)ToNullableString(Request.Form["TemplateTableDetailGUID"])));

                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<Tuple<List<TemplateColumnDetailList>, List<TemplateColumnDetailList>, List<TemplateColumnDetailList>>>>(
                    await new APIManager(_logger).CallGetMethod("template/column/list", request));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;

                return Json(new
                {
                    messageType,
                    message,
                    originSourceColumnDetails = baseResponse.Data.Item1,
                    originSourceDependentColumnDetails = baseResponse.Data.Item2,
                    targetSourceColumnDetails = baseResponse.Data.Item3,
                });

                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited GetTemplateColumnList");

            return Json(new
            {
                messageType,
                message,
                originSourceColumnDetails = string.Empty,
                originSourceDependentColumnDetails = string.Empty,
                targetSourceColumnDetails = string.Empty
            });
        }

        /// <summary>
        /// Remove Column mapping for given Template GUID
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RemoveColumnMapping()
        {
            _logger.LogInformation("Entered RemoveColumnMapping");

            var messageType = string.Empty;
            var message = string.Empty;
            try
            {
                #region Initialize Input parameter of SP


                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("templateGUID", (object)ToNullableString(Request.Form["TemplateGUID"])));

                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<object>>(
                    await new APIManager(_logger).CallGetMethod("template/column/removemapping", request));

                #endregion

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited RemoveColumnMapping");

            return Json(new
            {
                messageType,
                message,
            });
        }

        /// <summary>
        /// Insert / Update / Delete Template Filter Detail
        /// </summary>
        /// <param name="templateFilterDetail">TemplateFilterDetail object</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ManageTemplateFilterDetail(TemplateFilterDetail templateFilterDetail)
        {
            _logger.LogInformation("Entered ManageTemplateFilterDetail");

            var messageType = string.Empty;
            var message = string.Empty;
            try
            {
                var baseResponse = JsonConvert.DeserializeObject<Response<string>>(
                    await new APIManager(_logger).CallPostMethod("template/filter/manage", templateFilterDetail));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited ManageTemplateFilterDetail");

            return Json(new
            {
                messageType,
                message,
            });
        }

        /// <summary>
        /// Get Template Filter list for given Template GUID and Template Table Detail GUID
        /// </summary>
        /// <returns>Returns List of Template Filter Details</returns>
        [HttpPost]
        public async Task<JsonResult> GetTemplateFilterList()
        {
            _logger.LogInformation("Entered GetTemplateFilterList");

            var messageType = string.Empty;
            var message = string.Empty;
            try
            {
                #region Initialize Input parameter of SP

                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("templateGUID", (object)ToNullableString(Request.Form["TemplateGUID"])));
                request.Add(Tuple.Create("templateTableDetailGUID", (object)ToNullableString(Request.Form["TemplateTableDetailGUID"])));

                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<Tuple<List<TemplateFilterDetailList>, List<MasterItem>>>>(
                    await new APIManager(_logger).CallGetMethod("template/filter/list", request));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;

                return Json(new
                {
                    messageType,
                    message,
                    originSourceColumnDetails = baseResponse.Data.Item1,
                    operators = baseResponse.Data.Item2,
                });

                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited GetTemplateFilterList");

            return Json(new
            {
                messageType,
                message,
                originSourceColumnDetails = string.Empty,
                operators = string.Empty
            });
        }

        /// <summary>
        /// Upload file against Template GUID
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadFile()
        {
            _logger.LogInformation("Entered UploadFile");

            var messageType = string.Empty;
            var message = string.Empty;
            var IsSuccess = 0;
            try
            {
                var request = new Request();

                request.TemplateGUID = Convert.ToString(Request.Form["TemplateGUID"]);
                var file = Request.Form.Files[0];

                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var bytesArray = ms.ToArray();
                    request.FileContent = Convert.ToBase64String(bytesArray);
                    request.FileName = file.FileName;

                    var baseResponse = JsonConvert.DeserializeObject<Response<int>>(
                        await new APIManager(_logger).CallPostMethod("template/excel/upload", request));

                    IsSuccess = baseResponse.Data;
                    messageType = baseResponse.MessageType;
                    message = baseResponse.Message;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited UploadFile");

            return Json(new
            {
                messageType,
                message,
                IsSuccess
            });
        }

        #region WorkFlow - Testing

        /// <summary>
        /// Call Workflow based on template
        /// </summary>
        /// <param name="template"></param>
        private void CallWorkFlow(Template template)
        {
            try
            {
                //var request = new List<Tuple<string, object>>();
                //
                //request.Add(Tuple.Create("templateGUID", (object)template));

                //var response = CallSSXPostMethod("/webhook/ssxtestwebhook", template);

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        #endregion
    }
}
