using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SSXImport.Portal.Common
{
    /// <summary>
    /// Used to get HttpClient and execute API on given data
    /// </summary>
    public class APIManager
    {
        private static HttpClient httpClient;
        private readonly ILogger _logger;

        /// <summary>
        /// Initialize ILogger for logging purpose
        /// </summary>
        /// <param name="logger"></param>
        public APIManager(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns the HttpClient with base URL, Default Headers and Authorization from generated.config
        /// </summary>
        /// <returns>HttpClient with Endpoint and Authentication</returns>
        public static HttpClient GetClient()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(0, 2, 0);
            httpClient.BaseAddress = new Uri(ConfigWrapper.GetAppSettings("APIHostingURL"));
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var byteArray = Encoding.ASCII.GetBytes(ConfigWrapper.GetAppSettings("APIAuthUsername") + ":" + ConfigWrapper.GetAppSettings("APIAuthPassword"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            return httpClient;
        }

        /// <summary>
        /// Returns the HttpClient for SmartSnapIn API with base URL, Default Headers and Authorization from generated.config
        /// </summary>
        /// <returns>HttpClient with Endpoint and Authentication for SmartSnapIn API</returns>
        public static HttpClient GetSSXSnapInClient()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(0, 2, 0);
            httpClient.BaseAddress = new Uri(ConfigWrapper.GetAppSettings("SSXSnapInAPIHostingURL"));
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var byteArray = Encoding.ASCII.GetBytes(ConfigWrapper.GetAppSettings("SSXSnapInAPIAuthUsername") + ":" + ConfigWrapper.GetAppSettings("SSXSnapInAPIAuthPassword"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            return httpClient;
        }

        /// <summary>
        /// Generate String Content based on Given String
        /// </summary>
        /// <param name="data">String data which needs to convert to StringContent</param>
        /// <returns>Returns StringContent for passed string</returns>
        private StringContent GetStringContent(string data)
        {
            return new StringContent(data, Encoding.UTF8, AppConstant.MEDIA_TYPE);
        }

        /// <summary>
        /// Call HttpPost Method for given url and request
        /// </summary>
        /// <param name="url">URL of API</param>
        /// <param name="request">Request Parameter of API</param>
        /// <returns>Returns API Result</returns>
        public async Task<string> CallPostMethod(string url, object request)
        {
            _logger.LogDebug("Entered BaseController-CallPostMethod with request : {0}, {1}", url, JsonConvert.SerializeObject(request));

            var client = APIManager.GetClient();

            var response = await client.PostAsync(url,
                GetStringContent(JsonConvert.SerializeObject(request)));

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            _logger.LogDebug("Exited BaseController-CallPostMethod with response : {0}", responseString);

            return responseString;

        }

        /// <summary>
        /// Call HttpGet Method for given url and request
        /// </summary>
        /// <param name="url">URL of API</param>
        /// <param name="parameters">List of Request Parameters of API</param>
        /// <returns>Returns API Result</returns>
        public async Task<string> CallGetMethod(string url, List<Tuple<string, object>> parameters = null)
        {
            _logger.LogDebug("Entered BaseController-CallGetMethod with request : {0}, {1}", url, JsonConvert.SerializeObject(parameters));

            var client = APIManager.GetClient();

            if (parameters != null && parameters.Count > 0)
            {
                url += "?";
                foreach (var parameter in parameters)
                    url += parameter.Item1 + "=" + parameter.Item2 + "&";

                url = url.Remove(url.Length - 1, 1);
            }

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            _logger.LogDebug("Exited BaseController-CallPostMethod with response : {0}", responseString);

            return responseString;
        }

        /// <summary>
        /// Call HttpPost Method for given url and request
        /// </summary>
        /// <param name="url">URL of API</param>
        /// <param name="request">Multipart form Data of API</param>
        /// <returns>Returns API Result</returns>
        public async Task<string> CallPostMethodMultiPartForm(string url, MultipartFormDataContent request)
        {
            _logger.LogDebug("Entered BaseController-CallPostMethodMultiPartForm with request : {0}, {1}", url, JsonConvert.SerializeObject(request));

            var client = APIManager.GetClient();

            var response = await client.PostAsync(url, request);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            _logger.LogDebug("Exited BaseController-CallPostMethod with response : {0}", responseString);

            return responseString;
        }

        /// <summary>
        /// Call HttpGet Method (SSXSnapIn) for given url and request
        /// </summary>
        /// <param name="url">URL of API</param>
        /// <param name="parameters">List of Request Parameters of API</param>
        /// <returns>Returns API Result</returns>
        public Task<string> CallSSXSnapInGetMethod(string url, List<Tuple<string, object>> parameters = null)
        {
            _logger.LogDebug("Entered BaseController-CallSSXSnapInGetMethod with request : {0}, {1}", url, JsonConvert.SerializeObject(parameters));

            var client = APIManager.GetSSXSnapInClient();

            if (parameters != null && parameters.Count > 0)
            {
                url += "?";
                foreach (var parameter in parameters)
                    url += parameter.Item1 + "=" + parameter.Item2 + "&";

                url = url.Remove(url.Length - 1, 1);
            }

            var response = client.GetAsync(url);

            response.Result.EnsureSuccessStatusCode();
            var responseString = response.Result.Content.ReadAsStringAsync();

            _logger.LogDebug("Exited BaseController-CallSSXSnapInGetMethod with response : {0}", responseString);

            return responseString;
        }

        /// <summary>
        /// Call HttpPost Method (SSXSnapIn) for given url and request
        /// </summary>
        /// <param name="url">URL of API</param>
        /// <param name="request">Request Parameter of API</param>
        /// <returns>Returns API Result</returns>
        public Task<string> CallSSXPostMethod(string url, object request)
        {
            _logger.LogDebug("Entered BaseController-CallSSXPostMethod with request : {0}, {1}", url, JsonConvert.SerializeObject(request));
            var client = APIManager.GetSSXSnapInClient();

            var response = client.PostAsync(url,
                GetStringContent(JsonConvert.SerializeObject(request)));

            response.Result.EnsureSuccessStatusCode();

            var responseString = response.Result.Content.ReadAsStringAsync();

            _logger.LogDebug("Exited BaseController-CallSSXPostMethod with response : {0}", responseString);

            return responseString;
        }
    }
}
