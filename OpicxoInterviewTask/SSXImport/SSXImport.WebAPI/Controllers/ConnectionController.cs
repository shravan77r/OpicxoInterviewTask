using SSXImport.Business.ImplementationManager;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using SSXImport.WebAPI.Models;
using System.Data;
using SSXImport.WebAPI.Common;
using Microsoft.Extensions.Logging;
using SSXImport.WebAPI.Models.General;
using Newtonsoft.Json;

namespace SSXImport.WebAPI.Controllers
{
    /// <summary>
    /// Used to Get Databases, Tables, Directories and Files using connections
    /// </summary>
    [Route("api/connection")]
    [ApiController]
    public class ConnectionController : BaseController
    {

        private readonly ILogger<ConnectionController> _logger;

        /// <summary>
        /// Initialize ILogger for logging purpose
        /// </summary>
        /// <param name="logger"></param>
        public ConnectionController(ILogger<ConnectionController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Check if connection can be established or not using given parameters (Used in Test Connection button)
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Returns if Connection can be established or not</returns>
        [HttpPost]
        [Route("checkconnection")]
        [ApiVersion("1.0")]
        public virtual Response CheckValidConnection(Connection request)
        {
            _logger.LogInformation("Entered CheckValidConnection");
            _logger.LogDebug("Entered CheckValidConnection with Request : {0}", JsonConvert.SerializeObject(request));
            try
            {
                var errorMessage = string.Empty;
                if (request.DataSourceId <= 0)
                    errorMessage = "Invalid Data Source Id : " + request.DataSourceId;
                else if (string.IsNullOrEmpty(request.Server))
                    errorMessage = "Server Name is Required";
                else if (string.IsNullOrEmpty(request.Port))
                    errorMessage = "Port is Required";
                else if (string.IsNullOrEmpty(request.Username))
                    errorMessage = "Username is Required";
                else if (string.IsNullOrEmpty(request.Password))
                    errorMessage = "Password is Required";

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _logger.LogDebug(errorMessage);
                    _logger.LogError("Invalid Request : {0}", errorMessage);
                    return GetNoRecordFound(errorMessage);
                }

                #region Generate Response

                var connection = new AppCommon(_logger).GetConnectionStatus(request);
                if (connection.IsConnectionSuccessfull)
                {
                    response = GetSuccessResponse(connection, Message: "Connection Successful");
                }
                else
                {
                    response = GetNoRecordFound(Message: "Connection Could not be established!");
                }

                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited CheckValidConnection with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited CheckValidConnection");
            return response;
        }

        /// <summary>
        /// Get All Database names
        /// </summary>
        /// <param name="request">Connection string from where to fetch the names</param>
        /// <returns>Returns List of Database names</returns>
        [HttpPost]
        [ApiVersion("1.0")]
        [Route("list/database")]
        public virtual Response GetDatabaseList(Connection request)
        {
            _logger.LogInformation("Entered GetDatabaseList");
            _logger.LogDebug("Entered GetDatabaseList with Request : {0}", JsonConvert.SerializeObject(request));
            try
            {
                var errorMessage = string.Empty;
                if (request.DataSourceId <= 0)
                    errorMessage = "Invalid Data Source Id : " + request.DataSourceId;
                else if (string.IsNullOrEmpty(request.Server))
                    errorMessage = "Server Name is Required";
                else if (string.IsNullOrEmpty(request.Port))
                    errorMessage = "Port is Required";
                else if (string.IsNullOrEmpty(request.Username))
                    errorMessage = "Username is Required";
                else if (string.IsNullOrEmpty(request.Password))
                    errorMessage = "Password is Required";

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _logger.LogDebug(errorMessage);
                    _logger.LogError("Invalid Request : {0}", errorMessage);
                    return GetNoRecordFound(errorMessage);
                }

                #region Generate Response

                var connection = new AppCommon(_logger).GetConnectionStatus(request);
                if (!connection.IsConnectionSuccessfull)
                    response = GetNoRecordFound(Message: "Connection Could not be established!");
                else
                {
                    DataTable dt = new DataTable();
                    if (connection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                        dt = BLMsSQLConnectionSupport.GetAllDatabase(connection.ConnectionString);
                    else if (connection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                        dt = BLMySQLConnectionSupport.GetAllDatabase(connection.ConnectionString);

                    if (dt.Rows.Count > 0)
                        response = GetDataTableResponse(dt);
                    else
                        response = GetNoRecordFound();
                }

                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }

            _logger.LogDebug("Exited GetDatabaseList with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetDatabaseList");
            return response;
        }

        /// <summary>
        /// Get All DB Table names
        /// </summary>
        /// <param name="request">Connection string from where to fetch the names</param>
        /// <returns>Returns List of Table names</returns>
        [HttpPost]
        [ApiVersion("1.0")]
        [Route("list/table")]
        public virtual Response GetTableList(Connection request)
        {
            _logger.LogInformation("Entered GetTableList");
            _logger.LogDebug("Entered GetTableList with Request : {0}", JsonConvert.SerializeObject(request));
            try
            {
                var errorMessage = string.Empty;
                if (request.DataSourceId <= 0)
                    errorMessage = "Invalid Data Source Id : " + request.DataSourceId;
                else if (string.IsNullOrEmpty(request.Server))
                    errorMessage = "Server Name is Required";
                else if (string.IsNullOrEmpty(request.Port))
                    errorMessage = "Port is Required";
                else if (string.IsNullOrEmpty(request.Username))
                    errorMessage = "Username is Required";
                else if (string.IsNullOrEmpty(request.Password))
                    errorMessage = "Password is Required";
                else if (string.IsNullOrEmpty(request.Database))
                    errorMessage = "Database is Required";

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _logger.LogDebug(errorMessage);
                    _logger.LogError("Invalid Request : {0}", errorMessage);
                    return GetNoRecordFound(errorMessage);
                }

                #region Generate Response

                var connection = new AppCommon(_logger).GetConnectionStatus(request);
                if (!connection.IsConnectionSuccessfull)
                    response = GetNoRecordFound(Message: "Connection Could not be established!");
                else
                {
                    DataTable dt = new DataTable();
                    if (connection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                        dt = BLMsSQLConnectionSupport.GetAllTables(connection.ConnectionString);
                    else if (connection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                        dt = BLMySQLConnectionSupport.GetAllTables(connection.ConnectionString, connection.Database);

                    if (dt.Rows.Count > 0)
                        response = GetDataTableResponse(dt);
                    else
                        response = GetNoRecordFound();
                }

                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }

            _logger.LogDebug("Exited GetTableList with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetTableList");
            return response;
        }

        /// <summary>
        /// Get List of FTP Directories
        /// </summary>
        /// <param name="request">FTP Connection details</param>
        /// <returns>Returns List of Directories</returns>
        [HttpPost]
        [ApiVersion("1.0")]
        [Route("list/directories")]
        public virtual Response GetFTPDirectoryList(Connection request)
        {
            _logger.LogInformation("Entered GetFTPDirectoryList");
            _logger.LogDebug("Entered GetFTPDirectoryList with Request : {0}", JsonConvert.SerializeObject(request));
            try
            {
                var errorMessage = string.Empty;
                if (request.DataSourceId <= 0)
                    errorMessage = "Invalid Data Source Id : " + request.DataSourceId;
                else if (string.IsNullOrEmpty(request.Server))
                    errorMessage = "Server Name is Required";
                else if (string.IsNullOrEmpty(request.Port))
                    errorMessage = "Port is Required";
                else if (string.IsNullOrEmpty(request.Username))
                    errorMessage = "Username is Required";
                else if (string.IsNullOrEmpty(request.Password))
                    errorMessage = "Password is Required";

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _logger.LogDebug(errorMessage);
                    _logger.LogError("Invalid Request : {0}", errorMessage);
                    return GetNoRecordFound(errorMessage);
                }

                #region Generate Response

                var connection = new AppCommon(_logger).GetConnectionStatus(request);
                if (!connection.IsConnectionSuccessfull)
                    response = GetNoRecordFound(Message: "Connection Could not be established!");
                else
                {
                    var directories = new AppCommon(_logger).GetFTPDirectory(request).OrderBy(o => o.Name).ToList();

                    if (directories.Count > 0)
                        response = GetSuccessResponse(directories);
                    else
                        response = GetNoRecordFound();
                }

                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }

            _logger.LogDebug("Exited GetFTPDirectoryList with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetFTPDirectoryList");
            return response;
        }

        /// <summary>
        /// Get List of FTP Files
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Returns List of Files</returns>
        [HttpPost]
        [ApiVersion("1.0")]
        [Route("list/files")]
        public virtual Response GetFTPFilesList(Connection request)
        {
            _logger.LogInformation("Entered GetFTPFilesList");
            _logger.LogDebug("Entered GetFTPFilesList with Request : {0}", JsonConvert.SerializeObject(request));
            try
            {
                var errorMessage = string.Empty;
                if (request.DataSourceId <= 0)
                    errorMessage = "Invalid Data Source Id : " + request.DataSourceId;
                else if (string.IsNullOrEmpty(request.Server))
                    errorMessage = "Server Name is Required";
                else if (string.IsNullOrEmpty(request.Port))
                    errorMessage = "Port is Required";
                else if (string.IsNullOrEmpty(request.Username))
                    errorMessage = "Username is Required";
                else if (string.IsNullOrEmpty(request.Password))
                    errorMessage = "Password is Required";
                else if (string.IsNullOrEmpty(request.OriginSourceFilePath))
                    errorMessage = "Please Select Folder";

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _logger.LogDebug(errorMessage);
                    _logger.LogError("Invalid Request : {0}", errorMessage);
                    return GetNoRecordFound(errorMessage);
                }

                #region Generate Response

                var connection = new AppCommon(_logger).GetConnectionStatus(request);
                if (!connection.IsConnectionSuccessfull)
                    response = GetNoRecordFound(Message: "Connection Could not be established!");
                else
                {
                    var files = new AppCommon(_logger).GetFTPFiles(request).OrderBy(o => o.Name).ToList();

                    if (files.Count > 0)
                        response = GetSuccessResponse(files);
                    else
                        response = GetNoRecordFound();
                }

                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }

            _logger.LogDebug("Exited GetFTPFilesList with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetFTPFilesList");

            return response;
        }
    }
}
