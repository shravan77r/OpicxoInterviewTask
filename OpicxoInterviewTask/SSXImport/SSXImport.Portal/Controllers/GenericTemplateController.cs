using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using DevExpress.Office.Utils;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SSXImport.Portal.Common;
using SSXImport.Portal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SSXImport.Portal.Models.APIModel;

namespace SSXImport.Portal.Controllers
{
    /// <summary>
    /// CRUD Operation of Generic Template with Execution
    /// </summary>
    public class GenericTemplateController : BaseController
    {
        private readonly ILogger<GenericTemplateController> _logger;

        /// <summary>
        /// Initialize ILogger for logging purpose
        /// </summary>
        /// <param name="logger"></param>
        public GenericTemplateController(ILogger<GenericTemplateController> logger)
        {
            _logger = logger;
        }

        public IActionResult FroalaEditor()
        {
            _logger.LogInformation("Generic Template Called");
            return View();
        }
        public IActionResult GenericTemplate()
        {
            _logger.LogInformation("Generic Template Called");
            return View();
        }

        /// <summary>
        /// Get Generic Template List
        /// </summary>
        /// <returns>Returns List of Template</returns>
        [HttpPost]
        public async Task<JsonResult> GetTemplateList()
        {
            _logger.LogInformation("Entered GetGenericTemplateList");
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

                var baseResponse = JsonConvert.DeserializeObject<Response<List<EmailTemplateList>>>(
                    await new APIManager(_logger).CallGetMethod("generictemplate/list", request));

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
            _logger.LogInformation("Exited GetGenericTemplateList");
            return response;
        }

        /// <summary>
        /// Declaration of Amazaon s3 client
        /// </summary>
        private static IAmazonS3 client;

        /// <summary>
        /// View form of Email Template
        /// </summary>
        /// <param name="key">Template GUId for edit time</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GenericTemplateForm(string key)
        {
            _logger.LogInformation("Entered Generic Template Form with key : {0}", key);

            #region Default

            ModelState.Clear();

            var template = new GenericTemplate();
            var dt = new DataTable();
            template.IsActive = true;
            #endregion

            #region Dropdown Binding

            var ObjectTypeList = new List<MasterItem>();
            ObjectTypeList.Add(new MasterItem
            {
                Id = "1",
                Name = "Table",
            });
            ObjectTypeList.Add(new MasterItem
            {
                Id = "2",
                Name = "View",
            });
            template.ObjectTypeList = ObjectTypeList;


            var ObjectList = new List<MasterItem>();



            template.ObjectList = ObjectList;
            #endregion

            try
            {
                #region Get Template Details
                if (!string.IsNullOrEmpty(key))
                {
                    var request = new List<Tuple<string, object>>();
                    request.Add(Tuple.Create("emailtemplateid", (object)key));

                    var baseResponse = JsonConvert.DeserializeObject<Response<DataTable>>(
                        await new APIManager(_logger).CallGetMethod("generictemplate/edit", request));

                    dt = baseResponse.Data;
                    if (baseResponse.Status.Equals(0))
                    {
                        TempData["type"] = baseResponse.MessageType;
                        TempData["msg"] = baseResponse.Message;
                        return RedirectToAction("GenericTemplateForm");
                    }

                    template.EmailTemplateId = Convert.ToInt32(dt.Rows[0]["EmailTemplateId"].ToString());
                    template.EmailTemplateName = dt.Rows[0]["EmailTemplateName"].ToString();
                    template.EmailTemplateFile = dt.Rows[0]["EmailTemplateFile"].ToString();
                    template.EmailTemplateGUID = dt.Rows[0]["EmailTemplateGUID"].ToString();
                    template.ObjectType = dt.Rows[0]["ObjectType"].ToString();
                    template.Object = dt.Rows[0]["Object"].ToString();

                    if (!string.IsNullOrEmpty(template.ObjectType))
                    {
                        if (template.ObjectType == "1")
                        {
                            template.ObjectList = await GetTablesList();
                        }
                        if (template.ObjectType == "2")
                        {
                            template.ObjectList = await GetViewsList();
                        }

                    }

                    if (!string.IsNullOrEmpty(template.Object))
                    {
                        template.FieldArray = await GetFieldListByTableName(template.Object);
                    }
                    //Get File from AWS Bucket

                    IAmazonS3 s3Client = new AmazonS3Client(ConfigWrapper.GetAppSettings("AWSAccessKey"), ConfigWrapper.GetAppSettings("AWSSecretKey"), RegionEndpoint.USWest2);

                    MemoryStream file = new MemoryStream();
                    string htmlString = "";
                    try
                    {
                        GetObjectResponse r = await s3Client.GetObjectAsync(new GetObjectRequest()
                        {
                            BucketName = ConfigWrapper.GetAppSettings("AWSBucketName"),
                            Key = "EmailTemplate/" + template.EmailTemplateFile
                        });
                        try
                        {
                            StreamReader reader = new StreamReader(r.ResponseStream);
                            htmlString = reader.ReadToEnd();
                        }
                        finally
                        {
                        }
                    }
                    catch (AmazonS3Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }

                    template.EmailTemplateDesc = htmlString;

                }
                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited Generic Template Form with response : {0}", JsonConvert.SerializeObject(template));

            return View(template);
        }

        /// <summary>
        /// Insert / Update of Generic Template
        /// </summary>
        /// <param name="emailTemplate">Template object</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GenericTemplateForm(SSXImport.Portal.Models.GenericTemplate genericTemplate)
        {
            _logger.LogInformation("Entered Manage Generic Template");

            var messageType = string.Empty;
            var message = string.Empty;
            var templateID = string.Empty;
            var status = 0;
            try
            {
                var baseResponse = JsonConvert.DeserializeObject<Response<string>>(
                    await new APIManager(_logger).CallPostMethod("generictemplate/manage", genericTemplate));

                status = baseResponse.Status.GetValueOrDefault(0);
                messageType = baseResponse.MessageType;
                message = baseResponse.Message;

                if (genericTemplate.EmailTemplateId > 0)
                {
                    templateID = genericTemplate.EmailTemplateId.ToString();
                    IAmazonS3 client = new AmazonS3Client(ConfigWrapper.GetAppSettings("AWSAccessKey"), ConfigWrapper.GetAppSettings("AWSSecretKey"), RegionEndpoint.USWest2);
                    await client.DeleteObjectAsync(new Amazon.S3.Model.DeleteObjectRequest() { BucketName = ConfigWrapper.GetAppSettings("AWSBucketName"), Key = ConfigWrapper.GetAppSettings("AWSBucketName") + "/" + templateID + ".html" });
                }
                else
                {
                    templateID = baseResponse.Data;
                }

                //Create HTML File and Save in S3 Bucket

                byte[] byteArray = Encoding.UTF8.GetBytes(genericTemplate.EmailTemplatHiddenDesc);
                MemoryStream stream = new MemoryStream(byteArray);
                string name = templateID + ".html";
                string myBucketName = "ssx-dashboard-reports"; //your s3 bucket name goes here  
                string s3DirectoryName = "EmailTemplate";
                string s3FileName = name;
                bool a;
                AmazonUploader myUploader = new AmazonUploader();
                a = myUploader.sendMyFileToS3(stream, myBucketName, s3DirectoryName, s3FileName);
                if (a == true)
                {
                    _logger.LogInformation("File Saved in AWS Bucket");
                }
                else
                {
                    _logger.LogInformation("File Save Error in AWS Bucket");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited Manage Generic Template");
            return RedirectToAction("GenericTemplate");
        }

        /// <summary>
        /// Delete Generic Template based on Template GUID
        /// </summary>
        /// <param name="key">Template ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> DeleteEmailTemplate(string key)
        {
            _logger.LogInformation("Entered Delete Generic Template with Key : {0}", key);

            var message = "Something Went Wrong!";
            var messageType = AppConstant.AlertErrorType;

            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var genericTemplate = new GenericTemplate();
                    genericTemplate.EmailTemplateGUID = key;
                    genericTemplate.IsDelete = true;

                    var baseResponse = JsonConvert.DeserializeObject<Response<object>>(
                        await new APIManager(_logger).CallPostMethod("generictemplate/manage", genericTemplate));

                    messageType = baseResponse.MessageType;
                    message = baseResponse.Message;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited Delete Generic Template with response : {0}", JsonConvert.SerializeObject(new { messageType, message }));
             return Json(new { messageType, message });
        }

        /// <summary>
        /// Get List of tables
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetTables()
        {
            _logger.LogInformation("Entered GetTables");
            var message = "Something Went Wrong!";
            var messageType = AppConstant.AlertErrorType;
            List<string> data = new List<string>();
            try
            {

                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("emailtemplateid", (object)0));
                var baseResponse = JsonConvert.DeserializeObject<Response<List<string>>>(
                        await new APIManager(_logger).CallPostMethod("generictemplate/db/tables", request));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
                data = baseResponse.Data;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited GetTables");
            return Json(new { data });
        }

        /// <summary>
        /// Get Table List for binding model property
        /// </summary>
        /// <returns></returns>
        public async Task<List<MasterItem>> GetTablesList()
        {
            _logger.LogInformation("Entered GetTablesList");
            var message = "Something Went Wrong!";
            var messageType = AppConstant.AlertErrorType;
            List<string> data = new List<string>();
            var ObjectList = new List<MasterItem>();
            try
            {

                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("emailtemplateid", (object)0));
                var baseResponse = JsonConvert.DeserializeObject<Response<List<string>>>(
                        await new APIManager(_logger).CallPostMethod("generictemplate/db/tables", request));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
                data = baseResponse.Data;
                foreach (var str in data)
                {
                    ObjectList.Add(new MasterItem
                    {
                        Id = str,
                        Name = str
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited GetTablesList");
            return ObjectList;
        }

        /// <summary>
        /// Get View List for binding model property
        /// </summary>
        /// <returns></returns>
        public async Task<List<MasterItem>> GetViewsList()
        {
            _logger.LogInformation("Entered GetViewsList");
            var message = "Something Went Wrong!";
            var messageType = AppConstant.AlertErrorType;
            List<string> data = new List<string>();
            var ObjectList = new List<MasterItem>();
            try
            {
                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("emailtemplateid", (object)0));
                var baseResponse = JsonConvert.DeserializeObject<Response<List<string>>>(
                        await new APIManager(_logger).CallPostMethod("generictemplate/db/views", request));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
                data = baseResponse.Data;
                foreach (var str in data)
                {
                    ObjectList.Add(new MasterItem
                    {
                        Id = str,
                        Name = str
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited GetViewsList");
            return ObjectList;
        }

        /// <summary>
        /// Get List of Views
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetViews()
        {

            _logger.LogInformation("Entered GetViews");
            var message = "Something Went Wrong!";
            var messageType = AppConstant.AlertErrorType;
            List<string> data = new List<string>();
            try
            {
                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("emailtemplateid", (object)0));
                var baseResponse = JsonConvert.DeserializeObject<Response<List<string>>>(
                        await new APIManager(_logger).CallPostMethod("generictemplate/db/views", request));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
                data = baseResponse.Data;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited GetViews");
            return Json(new { data });
        }

        /// <summary>
        /// Get List of Fields by Table Name
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetFieldsByTableName(string key)
        {

            _logger.LogInformation("Entered GetFieldsByTableName");
            var message = "Something Went Wrong!";
            var messageType = AppConstant.AlertErrorType;
            DataTable data = new DataTable();
            try
            {
                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("key", (object)key));
                var baseResponse = JsonConvert.DeserializeObject<Response<DataTable>>(
                        await new APIManager(_logger).CallGetMethod("generictemplate/db/tables/fields", request));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
                data = baseResponse.Data;

                string[] fieldarray = data
                 .AsEnumerable()
                 .Select(row => row.Field<string>("COLUMN_NAME")).ToArray();
                return Json(new { fieldarray });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited GetFieldsByTableName");
            return Json(new { data });
        }

        /// <summary>
        /// Get List of Fields for binding modal property
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string[]> GetFieldListByTableName(string key)
        {

            _logger.LogInformation("Entered GetFieldListByTableName");
            var message = "Something Went Wrong!";
            var messageType = AppConstant.AlertErrorType;
            DataTable data = new DataTable();
            string[] fieldarray = null;
            try
            {
                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("key", (object)key));
                var baseResponse = JsonConvert.DeserializeObject<Response<DataTable>>(
                        await new APIManager(_logger).CallGetMethod("generictemplate/db/tables/fields", request));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
                data = baseResponse.Data;

                fieldarray = data
                 .AsEnumerable()
                 .Select(row => row.Field<string>("COLUMN_NAME")).ToArray();
                return fieldarray;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited GetFieldListByTableName");
            return fieldarray;
        }

        /// <summary>
        /// Execute Generic Template By GUId
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreateMultipleGenericTemplate(string key)
        {
            _logger.LogInformation("Entered CreateMultipleGenericTemplate with key : {0}", key);
            #region Default

            ModelState.Clear();
            var template = new GenericTemplate();
            var dt = new DataTable();
            template.IsActive = true;
            #endregion
            try
            {
                #region Get Template Details
                if (!string.IsNullOrEmpty(key))
                {
                    var request = new List<Tuple<string, object>>();
                    request.Add(Tuple.Create("emailtemplateid", (object)key));

                    var baseResponse = JsonConvert.DeserializeObject<Response<DataTable>>(
                        await new APIManager(_logger).CallGetMethod("generictemplate/edit", request));

                    dt = baseResponse.Data;
                    if (baseResponse.Status.Equals(0))
                    {
                        TempData["type"] = baseResponse.MessageType;
                        TempData["msg"] = baseResponse.Message;
                        return RedirectToAction("GenericTemplate");
                    }

                    template.EmailTemplateId = Convert.ToInt32(dt.Rows[0]["EmailTemplateId"].ToString());
                    template.EmailTemplateName = dt.Rows[0]["EmailTemplateName"].ToString();
                    template.EmailTemplateFile = dt.Rows[0]["EmailTemplateFile"].ToString();
                    template.EmailTemplateGUID = dt.Rows[0]["EmailTemplateGUID"].ToString();
                    template.ObjectType = dt.Rows[0]["ObjectType"].ToString();
                    template.Object = dt.Rows[0]["Object"].ToString();

                    if (!string.IsNullOrEmpty(template.ObjectType))
                    {
                        if (template.ObjectType == "1")
                        {
                            template.ObjectList = await GetTablesList();
                        }
                        if (template.ObjectType == "2")
                        {
                            template.ObjectList = await GetViewsList();
                        }
                    }

                    if (!string.IsNullOrEmpty(template.Object))
                    {
                        template.FieldArray = await GetFieldListByTableName(template.Object);
                    }

                    //Get File from AWS Bucket

                    IAmazonS3 s3Client = new AmazonS3Client(ConfigWrapper.GetAppSettings("AWSAccessKey"), ConfigWrapper.GetAppSettings("AWSSecretKey"), RegionEndpoint.USWest2);
                    MemoryStream file = new MemoryStream();
                    string htmlString = "";
                    try
                    {
                        GetObjectResponse r = await s3Client.GetObjectAsync(new GetObjectRequest()
                        {
                            BucketName = ConfigWrapper.GetAppSettings("AWSBucketName"),
                            Key = "EmailTemplate/" + template.EmailTemplateFile
                        });
                        try
                        {
                            StreamReader reader = new StreamReader(r.ResponseStream);
                            htmlString = reader.ReadToEnd();
                        }
                        finally
                        {
                        }
                    }
                    catch (AmazonS3Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }

                    template.EmailTemplateDesc = htmlString;
                    List<EmailTemplateContent> ListEmailTemplateContent = new List<EmailTemplateContent>();

                    try
                    {
                        //Get Data from Selected Object
                        DataTable dtObject = new DataTable();
                        var requestObj = new List<Tuple<string, object>>();
                        requestObj.Add(Tuple.Create("key", (object)template.Object));
                        var baseResponseSelectedObject = JsonConvert.DeserializeObject<Response<DataTable>>(
                           await new APIManager(_logger).CallGetMethod("generictemplate/db/tables/data", requestObj));

                        dtObject = baseResponseSelectedObject.Data;
                        if (dtObject != null)
                        {
                            if (dtObject.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtObject.Rows)
                                {
                                    var obj = new EmailTemplateContent();
                                    string templteString = htmlString;
                                    foreach (var item in template.FieldArray)
                                    {
                                        if (item == dtObject.Columns[item].ToString())
                                            templteString = templteString.Replace("#" + item, dr[item].ToString());
                                    }
                                    obj.Content = templteString;
                                    ListEmailTemplateContent.Add(obj);
                                }
                            }
                        }

                        template.ObjectDataTable = dtObject;
                    }
                    catch (Exception e) { _logger.LogError(e, e.Message); }

                    template.ListEmailTemplateContent = ListEmailTemplateContent;
                }
                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited CreateMultipleGenericTemplate with response : {0}", JsonConvert.SerializeObject(template));
            return View(template);
        }
    }
}
