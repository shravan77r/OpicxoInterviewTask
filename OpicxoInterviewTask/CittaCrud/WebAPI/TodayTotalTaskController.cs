using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.ImplementationManager.Transactions;
using Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Base;
using Model.Transactions;
using Newtonsoft.Json;

namespace WebAPI.Controllers.Transactions
{
    [Route("api/todaytotaltask")]
    [ApiController]
    public class TodayTotalTaskController : ControllerBase
    {
        private readonly ILogger<TodayTotalTaskController> _logger;
        private readonly AppUtil appUtil;
        private BLTodayTotalTaskManager bLTodayTotalTaskManager;
        public TodayTotalTaskController(ILogger<TodayTotalTaskController> logger)
        {
            _logger = logger;
            appUtil = new AppUtil();
        }

        [HttpPost]
        [Route("todaystotalasklist")]
        public async Task<Response<List<Activity>>> TodayTotalTaskList(ActivityListRequest request)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, JsonConvert.SerializeObject(request));

            var response = new Response<List<Activity>>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
            };

            try
            {
                using (bLTodayTotalTaskManager = new BLTodayTotalTaskManager())
                {
                    var data = await bLTodayTotalTaskManager.GetTodayTotalTaskList(request);

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
        
    }
}
