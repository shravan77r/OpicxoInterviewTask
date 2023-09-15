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

namespace WebAPI.Controllers.Transactions
{
    [Route("api/activity")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly ILogger<ActivityController> _logger;
        private readonly AppUtil appUtil;
        private BLActivityManager bLActivityManager;

        public ActivityController(ILogger<ActivityController> logger)
        {
            _logger = logger;
            appUtil = new AppUtil();
        }

        [HttpPost]
        [Route("list")]
        public async Task<Response<List<Activity>>> List(ActivityListRequest request)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, JsonConvert.SerializeObject(request));

            var response = new Response<List<Activity>>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
            };

            try
            {
                using (bLActivityManager= new BLActivityManager())
                {
                    var data = await bLActivityManager.GetList(request);

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
        public async Task<Response<Activity>> Edit(int id)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, id);

            var response = new Response<Activity>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
            };

            try
            {
                if (id > 0)
                {
                    using (bLActivityManager = new BLActivityManager())
                    {
                        var data = await bLActivityManager.GetData(id);
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
        public async Task<Response<int>> Insert(Activity request)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, JsonConvert.SerializeObject(request));

            var response = new Response<int>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = 0
            };
            try
            {
                using (bLActivityManager = new BLActivityManager())
                {
                    appUtil.GetHeaderModel(Request.Headers, out var headerModel);

                    var operationStatus = await bLActivityManager.InsertUpdateDelete(0, request, headerModel);
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

        [HttpPut]
        [Route("update/{id}")]
        public async Task<Response<int>> Update(int id, Activity request)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, JsonConvert.SerializeObject(request));
            var response = new Response<int>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = 0
            };
            try
            {
                using (bLActivityManager = new BLActivityManager())
                {
                    appUtil.GetHeaderModel(Request.Headers, out var headerModel);

                    var operationStatus = await bLActivityManager.InsertUpdateDelete(request.Id, request, headerModel);  // TODO
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
                using (bLActivityManager = new BLActivityManager())
                {
                    appUtil.GetHeaderModel(Request.Headers, out var headerModel);

                    var operationStatus = await bLActivityManager.InsertUpdateDelete(id, null, headerModel);
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
        [HttpPost]
        [Route("ActivityCount")]
        public async Task<Response<ActivityCountResponse>> ActivityCount(ActivityListRequest request)
        {
           // _logger.LogInformation(AppConstant.RequestMessage, JsonConvert.SerializeObject(request));

            var response = new Response<ActivityCountResponse>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
            };

            var dataList = new ActivityCountResponse();

            try
            {
                IList<BLActivity> listActivity = BLActivity.ActivityCollectionFromSearchFields(new BLActivity
                {
                    IsDelete = false,
                    RefTableId = Convert.ToInt32(request.RefTableId),
                    ModuleType = request.RefTableName
                });

                dataList.FollowUpCount = listActivity.Count(o => o.ActivityTypeId.GetValueOrDefault(0) == 1);
                dataList.LogACallCount = listActivity.Count(o => o.ActivityTypeId.GetValueOrDefault(0) == 2);
                dataList.TaskCount = listActivity.Count(o => o.ActivityTypeId.GetValueOrDefault(0) == 3);
                dataList.SiteVisitCount = listActivity.Count(o => o.ActivityTypeId.GetValueOrDefault(0) == 4);
                dataList.LeadActivityCount = listActivity.Count(o => o.ActivityTypeId.GetValueOrDefault(0) == 5);

                response.Count = 1;
                response.Data = dataList;
                response.Status = (int)AppConstant.APIStatus.SUCCESS;
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
        [HttpPut]
        [Route("updatestatus/{id}")]
        public async Task<Response<int>> UpdateStatus(int id, Activity request)
        {
            //_logger.LogInformation(AppConstant.RequestMessage, JsonConvert.SerializeObject(request));
            var response = new Response<int>
            {
                Status = (int)AppConstant.APIStatus.FAILURE,
                Data = 0
            };
            try
            {
                using (bLActivityManager = new BLActivityManager())
                {
                    appUtil.GetHeaderModel(Request.Headers, out var headerModel);

                    var operationStatus = await bLActivityManager.UpdateStatus(request.Id, request, headerModel);
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
    }
}
