using SSXImport.Business.ImplementationManager;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using SSXImport.WebAPI.Models;
using System.Data;
using SSXImport.WebAPI.Common;
using SSXImport.WebAPI.Helper;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using SSXImport.WebAPI.Models.General;

namespace SSXImport.WebAPI.Controllers
{
    /// <summary>
    /// Used for SmartSnapIn Login POC authentication purpose
    /// </summary>
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        /// <summary>
        /// Validate Provided Credentials via generated config data
        /// </summary>
        /// <param name="username">Username to match</param>
        /// <param name="password">Password to match</param>
        /// <returns>Returns if credentials are valid or not</returns>
        [HttpGet]
        [Route("validatecredentials")]
        [ApiVersion("1.0")]
        public virtual Response CheckValidConnection(string username, string password)
        {
            var APIAuthUsername = ConfigWrapper.GetAppSettings("APIAuthUsername");
            var APIAuthPassword = ConfigWrapper.GetAppSettings("APIAuthPassword");

            var IsValid = username.Equals(APIAuthUsername) && password.Equals(APIAuthPassword);
            if (IsValid)
                return GetSuccessResponse("1");
            else
                return GetNoRecordFound();

        }

        /// <summary>
        /// Validate Provided Credentials via static Data
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        [ApiVersion("1.0")]
        public virtual Response Login(string username, string password)
        {
            var loginInfo = new LoginInfo()
            {
                IsValidLogin = false
            };

            if ("admin1".Equals(username) && string.Compare(password, "admin1", false) == 0)
            {
                loginInfo.Username = username;
                loginInfo.ClientName = "Ashish Sonawane";
                loginInfo.ClientId = "1";
                loginInfo.IsValidLogin = true;
            }
            if ("admin2".Equals(username) && string.Compare(password, "admin2", false) == 0)
            {
                loginInfo.Username = username;
                loginInfo.ClientName = "Farshad Farhand";
                loginInfo.ClientId = "2";
                loginInfo.IsValidLogin = true;
            }
            if ("admin3".Equals(username) && string.Compare(password, "admin3", false) == 0)
            {
                loginInfo.Username = username;
                loginInfo.ClientName = "Hemant Gupta";
                loginInfo.ClientId = "3";
                loginInfo.IsValidLogin = true;
            }
            return GetSuccessResponse(loginInfo);
        }

        /// <summary>
        /// LoginInfo Structure to return Logged In User's Data
        /// </summary>
        struct LoginInfo
        {
            public bool IsValidLogin { get; set; }
            public string Username { get; set; }
            public string ClientName { get; set; }
            public string ClientId { get; set; }
        }
    }
}
