using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using SSXImport.Business;
using SSXImport.Business.ImplementationManager;
using SSXImport.WebAPI.Helper;
using SSXImport.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SSXImport.WebAPI.Common
{
    /// <summary>
    /// Used to get HttpClient and execute API on given data
    /// </summary>
    public class APIManager
    {
        private ILogger _logger;

        /// <summary>
        /// Constructor to Initialize ILogger
        /// </summary>
        /// <param name="logger"></param>
        public APIManager(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Execute API based on API GUID
        /// </summary>
        /// <param name="apiGuid"></param>
        /// <returns>Returns Result of API returned by Actual API</returns>
        internal APIExecutionResponse ExecuteAPI(string apiGuid)
        {
            _logger.LogDebug("Entered with API GUID : {0}", apiGuid);
            APIExecutionResponse objResponse;

            var apiData = GetAPIData(apiGuid);
            var apiDataModel = apiData.Item1;

            if (apiDataModel != null)
            {
                if (apiDataModel.AuthorizationType.Equals(AppConstant.APIData_Authorization_Auth2))
                {
                    _logger.LogDebug("Basic Authorization found");

                    var apiAuthDataModel = apiData.Item2;

                    var authAPIResponseString = CallAPIEndPoint(apiAuthDataModel);

                    object authAPIResponse = null;
                    if (!string.IsNullOrEmpty(authAPIResponseString))
                        authAPIResponse = JsonConvert.DeserializeObject<object>(authAPIResponseString);

                    if (authAPIResponse != null)
                        apiDataModel.AuthorizationToken = GetProperty(authAPIResponse, apiDataModel.AuthorizationTokenName).ToString();

                    if (string.IsNullOrEmpty(apiDataModel.AuthorizationToken))
                        throw new Exception("Authorization token not found in Basic Authorization API Response!");
                }

                var apiResponseString = CallAPIEndPoint(apiDataModel);

                objResponse = new APIExecutionResponse()
                {
                    APIId = apiDataModel.APIId,
                    Name = apiDataModel.Name,
                    APIEndPoint = apiDataModel.APIEndPoint,
                    APIResultData = apiResponseString,
                    Status = 1
                };
            }
            else
            {
                throw new Exception("API Data Can not be fetched from Database for API GUID : " + apiGuid);
            }

            _logger.LogDebug("Response Returned : {0} ", JsonConvert.SerializeObject(objResponse));
            _logger.LogDebug("Exited with API GUID : {0}", apiGuid);

            return objResponse;
        }

        /// <summary>
        /// Get Json Value from property name
        /// </summary>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private object GetProperty(object target, string name)
        {
            var site = System.Runtime.CompilerServices.CallSite<Func<System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, name, target.GetType(), new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) }));
            return site.Target(site, target);
        }

        /// <summary>
        /// Get API Data for Execution along with Authentication API request Data
        /// </summary>
        /// <param name="apiGUID">API GUID to fetch the data</param>
        /// <returns>Returns Main API data and Authentication API Data</returns>
        internal Tuple<APIData, APIData> GetAPIData(string apiGUID)
        {
            _logger.LogDebug("Entered APIManager GetAPIData with API GUID : {0}", apiGUID);

            APIData details = null;
            APIData authDetails = null;

            if (!string.IsNullOrEmpty(apiGUID))
            {
                var ds = BLAPIDataManager.GetAPIData(apiGUID);

                _logger.LogDebug("Dataset Returned from Database : {0} ", JsonConvert.SerializeObject(ds));

                #region API Data
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var drMainAPI = ds.Tables[0].Rows[0];
                    details = new APIData()
                    {
                        APIId = Convert.ToInt32(drMainAPI["APIId"]),
                        APIGUID = Convert.ToString(drMainAPI["APIGUID"]),
                        Name = Convert.ToString(drMainAPI["Name"]),
                        APIEndPoint = Convert.ToString(drMainAPI["APIEndPoint"]),
                        Type = Convert.ToInt32(drMainAPI["Type"]),
                        Description = Convert.ToString(drMainAPI["Description"]),
                        AuthorizationType = Convert.ToInt32(drMainAPI["AuthorizationType"]),
                        AuthorizationUsername = Convert.ToString(drMainAPI["AuthorizationUsername"]),
                        AuthorizationPassword = Convert.ToString(drMainAPI["AuthorizationPassword"]),
                        AuthorizationOathAPIId = Convert.ToInt32(drMainAPI["AuthorizationOathAPIId"]),
                        AuthorizationTokenName = Convert.ToString(drMainAPI["AuthorizationTokenName"]),
                        InputParameterType = Convert.ToInt32(drMainAPI["InputParameterType"]),
                        BodyParameterType = Convert.ToInt32(drMainAPI["BodyParameterType"]),
                        OutPutParameterJson = Convert.ToString(drMainAPI["OutPutParameterJson"])
                    };
                }
                #endregion

                #region Input Parameter Detail

                details.lstInputParameter = new List<APIInputParameter>();
                var dtInputParameters = ds.Tables[1];

                if (dtInputParameters.Rows.Count > 0)
                {
                    foreach (DataRow drInputParameter in dtInputParameters.Rows)
                    {
                        details.lstInputParameter.Add(new APIInputParameter()
                        {
                            InputParameterId = Convert.ToInt32(drInputParameter["InputParameterId"]),
                            InputParameterGUID = Convert.ToString(drInputParameter["InputParameterGUID"]),
                            APIId = Convert.ToInt32(drInputParameter["APIId"]),
                            InputParameterTypeId = Convert.ToInt32(drInputParameter["InputParameterTypeId"]),
                            KeyColumn = Convert.ToString(drInputParameter["KeyColumn"]),
                            ValueColumn = Convert.ToString(drInputParameter["ValueColumn"]),
                            BodyType = Convert.ToInt32(drInputParameter["BodyType"])
                        });
                    }
                }

                #endregion

                #region Output Parameter Detail

                details.lstOutputParametere = new List<APIOutputParameter>();
                var dtOutputParameters = ds.Tables[2];

                if (dtOutputParameters.Rows.Count > 0)
                {
                    foreach (DataRow drOutputParameter in dtOutputParameters.Rows)
                    {
                        details.lstOutputParametere.Add(new APIOutputParameter()
                        {
                            OutputParameterId = Convert.ToInt32(drOutputParameter["OutputParameterId"]),
                            OutputParameterGUID = Convert.ToString(drOutputParameter["OutputParameterGUID"]),
                            APIId = Convert.ToInt32(drOutputParameter["APIId"]),
                            KeyColumn = Convert.ToString(drOutputParameter["KeyColumn"]),
                            Type = Convert.ToInt32(drOutputParameter["Type"]),
                        });
                    }
                }

                #endregion

                #region Authentication API Data

                if (details.AuthorizationType.Equals(AppConstant.APIData_Authorization_Auth2))
                {
                    var drAuthAPI = ds.Tables[3].Rows[0];
                    authDetails = new APIData()
                    {
                        APIId = Convert.ToInt32(drAuthAPI["APIId"]),
                        APIGUID = Convert.ToString(drAuthAPI["APIGUID"]),
                        Name = Convert.ToString(drAuthAPI["Name"]),
                        APIEndPoint = Convert.ToString(drAuthAPI["APIEndPoint"]),
                        Type = Convert.ToInt32(drAuthAPI["Type"]),
                        Description = Convert.ToString(drAuthAPI["Description"]),
                        AuthorizationType = Convert.ToInt32(drAuthAPI["AuthorizationType"]),
                        AuthorizationUsername = Convert.ToString(drAuthAPI["AuthorizationUsername"]),
                        AuthorizationPassword = Convert.ToString(drAuthAPI["AuthorizationPassword"]),
                        AuthorizationOathAPIId = Convert.ToInt32(drAuthAPI["AuthorizationOathAPIId"]),
                        AuthorizationTokenName = Convert.ToString(drAuthAPI["AuthorizationTokenName"]),
                        InputParameterType = Convert.ToInt32(drAuthAPI["InputParameterType"]),
                        BodyParameterType = Convert.ToInt32(drAuthAPI["BodyParameterType"]),
                        OutPutParameterJson = Convert.ToString(drAuthAPI["OutPutParameterJson"])
                    };

                    #region Input Parameter Data

                    authDetails.lstInputParameter = new List<APIInputParameter>();
                    var dtAuthInputParameters = ds.Tables[4];

                    if (dtAuthInputParameters.Rows.Count > 0)
                    {
                        foreach (DataRow drInputParameter in dtAuthInputParameters.Rows)
                        {
                            authDetails.lstInputParameter.Add(new APIInputParameter()
                            {
                                InputParameterId = Convert.ToInt32(drInputParameter["InputParameterId"]),
                                InputParameterGUID = Convert.ToString(drInputParameter["InputParameterGUID"]),
                                APIId = Convert.ToInt32(drInputParameter["APIId"]),
                                InputParameterTypeId = Convert.ToInt32(drInputParameter["InputParameterTypeId"]),
                                KeyColumn = Convert.ToString(drInputParameter["KeyColumn"]),
                                ValueColumn = Convert.ToString(drInputParameter["ValueColumn"]),
                                BodyType = Convert.ToInt32(drInputParameter["BodyType"])
                            });
                        }
                    }

                    #endregion

                    #region API Output Parameter Data

                    authDetails.lstOutputParametere = new List<APIOutputParameter>();
                    var dtAuthOutputParameters = ds.Tables[5];

                    if (dtAuthOutputParameters.Rows.Count > 0)
                    {
                        foreach (DataRow drOutputParameter in dtAuthOutputParameters.Rows)
                        {
                            authDetails.lstOutputParametere.Add(new APIOutputParameter()
                            {
                                OutputParameterId = Convert.ToInt32(drOutputParameter["OutputParameterId"]),
                                OutputParameterGUID = Convert.ToString(drOutputParameter["OutputParameterGUID"]),
                                APIId = Convert.ToInt32(drOutputParameter["APIId"]),
                                KeyColumn = Convert.ToString(drOutputParameter["KeyColumn"]),
                                Type = Convert.ToInt32(drOutputParameter["Type"]),
                            });
                        }
                    }

                    #endregion

                }
                #endregion

                _logger.LogDebug("APIData Tuple Data Returned : {0} ", JsonConvert.SerializeObject(Tuple.Create(details, authDetails)));
            }
            else
            {
                _logger.LogDebug("API GUID is NULL or empty");
            }

            _logger.LogDebug("Exited APIManager GetAPIData with API GUID : {0}", apiGUID);

            return Tuple.Create(details, authDetails);
        }

        /// <summary>
        /// Execute API using provided request data
        /// </summary>
        /// <param name="objAPIDataModel">APIData Class object containing details required to execute any API</param>
        /// <returns>API Result in string format</returns>
        private string CallAPIEndPoint(APIData objAPIDataModel)
        {
            _logger.LogDebug("Entered APIManager CallAPIEndPoint");

            string strResponce;

            Method ApiMethod = (Method)objAPIDataModel.Type;

            string apiUrl = objAPIDataModel.APIEndPoint + "?";

            var request = new RestRequest(ApiMethod);

            if (objAPIDataModel.AuthorizationType.Equals(AppConstant.APIData_Authorization_Basic_Auth))
            {
                if (String.IsNullOrEmpty(objAPIDataModel.AuthorizationUsername)
                    || String.IsNullOrEmpty(objAPIDataModel.AuthorizationPassword))
                    throw new Exception("Please Provide Username and Password for Basic Authentication");

                byte[] bytes = Encoding.GetEncoding(28591).GetBytes(objAPIDataModel.AuthorizationUsername
                    + ":"
                    + objAPIDataModel.AuthorizationPassword);

                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(bytes));
            }
            else if (objAPIDataModel.AuthorizationType.Equals(AppConstant.APIData_Authorization_Auth2))
            {
                request.AddHeader("Authorization", "Bearer " + objAPIDataModel.AuthorizationToken);
            }

            if (objAPIDataModel.InputParameterType == 3)
            {
                if (objAPIDataModel.BodyParameterType == 1)
                {
                    request.AlwaysMultipartFormData = true;
                }
                else if (objAPIDataModel.BodyParameterType == 2)
                {
                    request.AddHeader("Content-Type", value: "application/x-www-form-urlencoded");
                }
                else if (objAPIDataModel.BodyParameterType == 3)
                {
                    request.AddHeader("Content-Type", "application/json");
                }
            }

            if (objAPIDataModel.lstInputParameter != null && objAPIDataModel.lstInputParameter.Count > 0)
            {
                var requestJsonObj = new Dictionary<object, object>();

                foreach (var inputParamItem in objAPIDataModel.lstInputParameter)
                {
                    if (objAPIDataModel.InputParameterType == 1) // Query String Parameter
                    {
                        if (apiUrl.EndsWith('?'))
                            apiUrl += inputParamItem.KeyColumn + "=" + inputParamItem.ValueColumn;
                        else
                            apiUrl += "&" + inputParamItem.KeyColumn + "=" + inputParamItem.ValueColumn;
                    }
                    else if (objAPIDataModel.InputParameterType == 2) // Headers
                    {
                        request.AddHeader(inputParamItem.KeyColumn, inputParamItem.ValueColumn);
                    }
                    else if (objAPIDataModel.InputParameterType == 3) // Body Parameters
                    {
                        if (objAPIDataModel.BodyParameterType == 1 || objAPIDataModel.BodyParameterType == 2)
                            request.AddParameter(inputParamItem.KeyColumn, inputParamItem.ValueColumn);
                        else
                            requestJsonObj.Add(inputParamItem.KeyColumn, inputParamItem.ValueColumn);
                    }
                }

                if (requestJsonObj.Count > 0) // If Body Type is JSON then SerializeObject request Data
                {
                    request.AddParameter("application/json", JsonConvert.SerializeObject(requestJsonObj), ParameterType.RequestBody);
                }
            }
            var stopWatch = new Stopwatch();
            IRestResponse response = null;
            try
            {
                var client = new RestClient(apiUrl);
                client.Timeout = -1;
                stopWatch.Start();
                response = client.Execute(request);
                stopWatch.Stop();
                strResponce = response.Content;

                if (response.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                    throw new Exception("Unauthorized Request");
                if (response.StatusCode.Equals(System.Net.HttpStatusCode.Forbidden))
                    throw new Exception("Access Forbidden for Request");
                if (response.StatusCode.Equals(System.Net.HttpStatusCode.BadRequest))
                    throw new Exception("Bad Request");
                if (!response.IsSuccessful)
                    throw new Exception("Request Failed!");
            }
            finally
            {
                LogRequest(request, response, stopWatch.ElapsedMilliseconds, apiUrl);

            }

            _logger.LogDebug("Exited APIManager CallAPIEndPoint");

            return strResponce;
        }

        private void LogRequest(IRestRequest request, IRestResponse response, long durationMs, string apiUrl)
        {
            var requestToLog = new
            {
                resource = request.Resource,
                // Parameters are custom anonymous objects in order to have the parameter type as a nice string
                // otherwise it will just show the ENUM value
                parameters = request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                // ToString() here to have the method as a nice string otherwise it will just show the ENUM value
                method = request.Method.ToString(),
                // This will generate the actual Uri used in the request
                uri = apiUrl,
            };
            var responseToLog = new
            {
                statusCode = response.StatusCode,
                content = response.Content,
                headers = response.Headers,
                // The Uri that actually responded (could be different from the requestUri if a redirection occurred)
                responseUri = response.ResponseUri,
                errorMessage = response.ErrorMessage,
            };
            _logger.LogDebug(string.Format("Request completed in {0} ms, Request: {1}, Response: {2}",
                    durationMs,
                    JsonConvert.SerializeObject(requestToLog),
                    JsonConvert.SerializeObject(responseToLog)));
        }
    }
}
