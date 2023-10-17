using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SSXImport.Portal.Common;
using SSXImport.Portal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static SSXImport.Portal.Models.APIModel;

namespace SSXImport.Portal.Controllers
{
    public class DataTransferController : BaseController
    {
        private readonly ILogger<DataTransferController> _logger;

        /// <summary>
        /// Initialize ILogger for Logging purpose
        /// </summary>
        /// <param name="logger"></param>
        public DataTransferController(ILogger<DataTransferController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// View Page of Data Transfer List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DataTransfer()
        {
            _logger.LogInformation("DataTransfer Called");

            ViewBag.message = TempData["msg"];
            ViewBag.messageType = TempData["type"];
            return View();
        }

        /// <summary>
        /// Get List of Data Transfer
        /// </summary>
        /// <returns>Returns List of Data Transfer</returns>
        [HttpPost]
        public async Task<JsonResult> GetDataTransferList()
        {
            _logger.LogInformation("Entered GetDataTransferList");
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
                request.Add(Tuple.Create("SortCol", (object)Convert.ToInt32(Request.Form["order[0][column]"])));
                request.Add(Tuple.Create("SortDir", (object)Request.Form["order[0][dir]"]));
                request.Add(Tuple.Create("PageFrom", (object)Convert.ToInt32(Request.Form["start"])));
                request.Add(Tuple.Create("PageSize", (object)Convert.ToInt32(Request.Form["length"])));
                request.Add(Tuple.Create("Keyword", (object)ToNullableString(Request.Form["keyword"])));

                #endregion

                #region Get Data from API

                var baseResponse = JsonConvert.DeserializeObject<Response<List<DataTransferList>>>(
                    await new APIManager(_logger).CallGetMethod("datatransfer/list", request));

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

            _logger.LogInformation("Exited GetDataTransferList");

            return response;
        }

        /// <summary>
        /// Get Content of Data Transfer Logfile if Data Transfer is failed
        /// </summary>
        /// <returns>Return file content in string format</returns>
        [HttpPost]
        public async Task<JsonResult> GetLogFileContent()
        {
            _logger.LogInformation("Entered GetLogFileContent");

            var messageType = string.Empty;
            var message = string.Empty;
            var logFileContent = string.Empty;
            var status = 0;
            try
            {
                var request = new List<Tuple<string, object>>();
                request.Add(Tuple.Create("dataTransferGuid", (object)ToNullableString(Request.Form["key"])));

                var baseResponse = JsonConvert.DeserializeObject<Response<string>>(
                    await new APIManager(_logger).CallGetMethod("datatransfer/logfile/getcontent", request));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
                logFileContent = baseResponse.Data;
                status = (int)baseResponse.Status;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited GetLogFileContent");

            return Json(new
            {
                messageType,
                message,
                logFileContent,
                status
            });
        }

        /// <summary>
        /// Execute Data Transfer for given Template GUID
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ExecuteTransfer()
        {
            _logger.LogInformation("Entered ExecuteTransfer");

            var messageType = string.Empty;
            var message = string.Empty;
            try
            {
                var request = new Dictionary<string, object>{
                    { "TemplateGuid", ToNullableString(Request.Form["key"]) }
                };

                var baseResponse = JsonConvert.DeserializeObject<Response<string>>(
                    await new APIManager(_logger).CallPostMethod("datatransfer/new", request));

                TempData["msg"] = baseResponse.Message;
                TempData["type"] = baseResponse.MessageType;

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogInformation("Exited ExecuteTransfer");

            return Json(new
            {
                messageType,
                message,
            });
        }
    }

}
