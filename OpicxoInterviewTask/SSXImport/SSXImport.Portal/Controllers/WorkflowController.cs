using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SSXImport.Portal.Common;
using SSXImport.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SSXImport.Portal.Models.APIModel;

namespace SSXImport.Portal.Controllers
{
    public class WorkflowController : BaseController
    {

        private readonly ILogger<WorkflowController> _logger;

        /// <summary>
        /// Initialize ILogger for logging purpose
        /// </summary>
        /// <param name="logger"></param>
        public WorkflowController(ILogger<WorkflowController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// View Page of Workflow
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Workflow()
        {
            _logger.LogInformation("Workflow Called");

            return View();
        }

        /// <summary>
        /// Get Workflow List
        /// </summary>
        /// <returns>Returns List of Workflow</returns>
        [HttpPost]
        public async Task<JsonResult> GetWorkflowList()
        {
            _logger.LogInformation("Entered GetWorkflowList");
            var response = Json(new
            {
                draw = 0,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = string.Empty
            });
            try
            {
                #region Get Data from API

                var draw = Convert.ToInt32(Request.Form["draw"]);

                var baseResponse = JsonConvert.DeserializeObject<Response<List<WorkflowList>>>(
                    await new APIManager(_logger).CallSSXSnapInGetMethod("rest/workflows"));

                #endregion

                response = Json(new
                {
                    draw = draw,
                    recordsTotal = baseResponse.Data.Count,
                    recordsFiltered = baseResponse.Data.Count,
                    data = baseResponse.Data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            _logger.LogInformation("Exited GetWorkflowList");

            return response;
        }

        /// <summary>
        /// View Page for Workflow Execution
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult WorkflowExecution()
        {
            _logger.LogInformation("WorkflowExecution Called");

            return View();
        }

        /// <summary>
        /// Execute Workflow using workflowId
        /// </summary>
        /// <param name="workflowId">WorkflowId to execute</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ExecuteWorkflow(string workflowId)
        {
            _logger.LogInformation("Entered ExecuteWorkflow");

            var messageType = string.Empty;
            var message = string.Empty;
            try
            {
                var request = new Dictionary<string, object>{
                    { "workflowId", workflowId }
                };

                var baseResponse = JsonConvert.DeserializeObject<Response<string>>(
                    await new APIManager(_logger).CallSSXPostMethod("rest/workflows/runbyid", request));

                messageType = baseResponse.MessageType;
                message = baseResponse.Message;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogInformation("Exited ExecuteWorkflow");
            return Json(new
            {
                messageType,
                message,
            });
        }

        /// <summary>
        /// Get List of Workflow Execution
        /// </summary>
        /// <returns>Returns List of Workflow Execution</returns>
        [HttpPost]
        public async Task<JsonResult> GetWorkflowExecutionList()
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
                #region Get Data from API

                var draw = Convert.ToInt32(Request.Form["draw"]);

                var request = new List<Tuple<string, object>>();

                request.Add(Tuple.Create("limit", (object)Convert.ToInt32(Request.Form["length"])));
                request.Add(Tuple.Create("lastId", (object)(Convert.ToInt32(Request.Form["count"]) - Convert.ToInt32(Request.Form["start"]) + 1)));

                var baseResponse = JsonConvert.DeserializeObject<Response<WorkflowExecution>>(
                    await new APIManager(_logger).CallSSXSnapInGetMethod("rest/executions", request));

                #endregion

                response = Json(new
                {
                    draw = draw,
                    recordsTotal = baseResponse.Data.count,
                    recordsFiltered = baseResponse.Data.count,
                    data = baseResponse.Data.results,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            _logger.LogInformation("Exited GetAPIDataList");

            return response;
        }
    }
}
