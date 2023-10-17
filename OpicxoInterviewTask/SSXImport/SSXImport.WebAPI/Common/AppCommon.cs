using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SSXImport.Business;
using SSXImport.Business.ImplementationManager;
using SSXImport.WebAPI.Models;
using SSXImport.WebAPI.Models.General;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SSXImport.WebAPI.Common
{
    /// <summary>
    /// Contains Static common methods used throughout the Application
    /// </summary>
    public class AppCommon
    {
        public static IWebHostEnvironment Environment { get; set; }

        private ILogger _logger;

        /// <summary>
        /// Constructor to Initialize ILogger
        /// </summary>
        /// <param name="logger"></param>
        public AppCommon(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Write String logs in file
        /// </summary>
        /// <param name="str">string to write</param>
        public static void WriteFile(string str)
        {
            return;
            /// Not used now using ILogger in latest code instead of this method
            /*
            try
            {
                if (string.IsNullOrEmpty(str)) return;
                var currentDate = DateTime.Now.ToString("MMMddyyyy");

                var filePath = Path.Combine(Environment.WebRootPath, "LogFiles");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var fileName = Path.Combine(filePath, "Log_" + currentDate + ".txt");

                if (!File.Exists(fileName))
                {
                    File.Create(fileName).Close();
                }
                using StreamWriter sw = File.AppendText(fileName);
                sw.WriteLine("#");
                sw.WriteLine(DateTime.Now.ToString());
                sw.WriteLine(str);
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            */
        }

        /// <summary>
        /// Get Current DateTime
        /// </summary>
        /// <returns>Returns Current DateTime</returns>
        public static DateTime GetDateTime()
        {
            return DateTime.UtcNow;
        }

        /// <summary>
        /// Get Connection Status if it is successful or not
        /// </summary>
        /// <param name="connection">Contains the data source and details</param>
        /// <returns>Returns the connection class object containing IsSuccessfull Status and/or connection String</returns>
        public Connection GetConnectionStatus(Connection connection)
        {
            _logger.LogDebug("Entered AppCommon GetConnectionStatus with request : {0}", JsonConvert.SerializeObject(connection));
            bool IsConnectionSuccessfull = false;
            var connectionString = string.Empty;
            if (connection.DataSourceId.Equals(AppConstant.DataSource_Excel))
            {
                if (connection.OriginSourceFileTypeId.Equals(2))
                {
                    var sessionOptions = new WinSCP.SessionOptions
                    {
                        Protocol = WinSCP.Protocol.Ftp,
                        HostName = connection.Server,
                        PortNumber = Convert.ToInt32(connection.Port),
                        UserName = connection.Username,
                        Password = connection.Password,
                    };

                    using (var session = new WinSCP.Session())
                    {
                        session.Open(sessionOptions);

                        IsConnectionSuccessfull = true;
                    }
                }
                else
                {
                    //TODO : Check Existing File Code
                    IsConnectionSuccessfull = true;
                }
            }
            else if (connection.DataSourceId.Equals(AppConstant.DataSource_API))
            {
                IsConnectionSuccessfull = true; // In case of API make it success default as it will throw exception later
            }
            else if (!string.IsNullOrEmpty(connection.Server)
                  && !string.IsNullOrEmpty(connection.Port)
                  && !string.IsNullOrEmpty(connection.Username)
                  && !string.IsNullOrEmpty(connection.Password))
            {
                if (connection.DataSourceId.Equals(2))
                {
                    var msSqlConnectionStringBuilder = new SqlConnectionStringBuilder();
                    msSqlConnectionStringBuilder.DataSource = string.Format("{0},{1}",
                        connection.Server,
                        connection.Port);

                    msSqlConnectionStringBuilder.UserID = connection.Username;
                    msSqlConnectionStringBuilder.Password = connection.Password;
                    if (!string.IsNullOrEmpty(connection.Database))
                        msSqlConnectionStringBuilder.InitialCatalog = connection.Database;

                    connectionString = msSqlConnectionStringBuilder.ConnectionString;

                    IsConnectionSuccessfull = BLMsSQLConnectionSupport.CheckCredentials(connectionString);

                }
                else if (connection.DataSourceId.Equals(3))
                {
                    var mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder();
                    mySqlConnectionStringBuilder.Server = connection.Server;
                    mySqlConnectionStringBuilder.Port = Convert.ToUInt32(connection.Port);
                    mySqlConnectionStringBuilder.UserID = connection.Username;
                    mySqlConnectionStringBuilder.Password = connection.Password;
                    if (!string.IsNullOrEmpty(connection.Database))
                        mySqlConnectionStringBuilder.Database = connection.Database;

                    connectionString = mySqlConnectionStringBuilder.ConnectionString;

                    IsConnectionSuccessfull = BLMySQLConnectionSupport.CheckCredentials(connectionString);
                }
            }

            connection.IsConnectionSuccessfull = IsConnectionSuccessfull;
            connection.ConnectionString = connectionString;
            
            _logger.LogDebug("Exited AppCommon GetConnectionStatus with response : {0}", JsonConvert.SerializeObject(connection));
            return connection;
        }

        /// <summary>
        /// Get File Status if it exists or not
        /// </summary>
        /// <param name="connection">Connection object containing file path</param>
        /// <param name="templateId">templateId used to get actual file path</param>
        /// <param name="IsHeader">flag to build the connection string</param>
        /// <returns>Returns if file exists or not, if exists then returns the OLEDB connection string</returns>
        internal Connection GetFileStatus(Connection connection, int templateId, bool IsHeader)
        {
            _logger.LogDebug("Entered AppCommon GetFileStatus with request : {0}, templateId : {1}, IsHeader : {2}"
                , JsonConvert.SerializeObject(connection)
                , templateId
                , IsHeader);

            bool IsConnectionSuccessfull = false;
            var connectionString = string.Empty;

            if (!string.IsNullOrEmpty(connection.OriginSourceFileName))
            {
                var path = Environment.WebRootPath
                                + "\\Document\\Template\\"
                                + templateId
                                + "\\"
                                + connection.OriginSourceFileName;
                if (System.IO.File.Exists(path))
                {
                    IsConnectionSuccessfull = true;
                    connectionString = GetOLEDBConnectionstring(path, IsHeader);
                }
            }

            connection.IsConnectionSuccessfull = IsConnectionSuccessfull;
            connection.ConnectionString = connectionString;

            _logger.LogDebug("Exited AppCommon GetFileStatus with request : {0}", JsonConvert.SerializeObject(connection));

            return connection;
        }

        /// <summary>
        /// Get List of Directories in FTP Server
        /// </summary>
        /// <param name="connection">Connections parameters like server, username, port and Password</param>
        /// <returns>Returns List of MasterItems containing Directory names</returns>
        internal List<MasterItem> GetFTPDirectory(Connection connection)
        {
            _logger.LogDebug("Entered AppCommon GetFTPDirectory with request : {0}", JsonConvert.SerializeObject(connection));

            var directories = new List<MasterItem>();

            if (!string.IsNullOrEmpty(connection.Server)
                && !string.IsNullOrEmpty(connection.Port)
                && !string.IsNullOrEmpty(connection.Username)
                && !string.IsNullOrEmpty(connection.Password))

            {
                var sessionOptions = new WinSCP.SessionOptions
                {
                    Protocol = WinSCP.Protocol.Ftp,
                    HostName = connection.Server,
                    PortNumber = Convert.ToInt32(connection.Port),
                    UserName = connection.Username,
                    Password = connection.Password,
                };

                using (var session = new WinSCP.Session())
                {
                    session.Open(sessionOptions);

                    var options =
                        WinSCP.EnumerationOptions.EnumerateDirectories | WinSCP.EnumerationOptions.AllDirectories;
                    IEnumerable<WinSCP.RemoteFileInfo> fileInfos =
                        session.EnumerateRemoteFiles("/", null, options);
                    fileInfos = fileInfos.Where(o => o.IsDirectory.Equals(true));

                    directories.Add(new MasterItem
                    {
                        Id = "/",
                        Name = "root"
                    });
                    foreach (var fileInfo in fileInfos)
                    {
                        directories.Add(new MasterItem
                        {
                            Id = fileInfo.FullName,
                            Name = fileInfo.Name
                        });
                    }
                }
            }

            _logger.LogDebug("Exited AppCommon GetFTPDirectory with response : {0}", JsonConvert.SerializeObject(directories));

            return directories;
        }

        /// <summary>
        /// Get List of Files in FTP Server
        /// </summary>
        /// <param name="connection">Connections parameters like server, username, port and Password</param>
        /// <returns>Returns List of MasterItems containing File names</returns>
        internal List<MasterItem> GetFTPFiles(Connection connection)
        {
            _logger.LogDebug("Exited GetFTPFiles with Request : {0}", JsonConvert.SerializeObject(connection));
            
            var files = new List<MasterItem>();

            if (!string.IsNullOrEmpty(connection.Server)
                && !string.IsNullOrEmpty(connection.Port)
                && !string.IsNullOrEmpty(connection.Username)
                && !string.IsNullOrEmpty(connection.Password))

            {
                var sessionOptions = new WinSCP.SessionOptions
                {
                    Protocol = WinSCP.Protocol.Ftp,
                    HostName = connection.Server,
                    PortNumber = Convert.ToInt32(connection.Port),
                    UserName = connection.Username,
                    Password = connection.Password,
                };

                using (var session = new WinSCP.Session())
                {
                    session.Open(sessionOptions);

                    var options =
                        WinSCP.EnumerationOptions.None;

                    IEnumerable<WinSCP.RemoteFileInfo> fileInfos =
                        session.EnumerateRemoteFiles(connection.OriginSourceFilePath,
                        null, options).Where(o => o.IsDirectory.Equals(false));

                    foreach (var fileInfo in fileInfos)
                    {
                        string extension = Path.GetExtension(fileInfo.Name);
                        if (
                            extension.Equals(".xlsx")
                            || extension.Equals(".xls")
                            || extension.Equals(".csv")
                            )
                            files.Add(new MasterItem
                            {
                                Id = fileInfo.Name,
                                Name = fileInfo.Name
                            });
                    }
                }
            }

            _logger.LogDebug("Exited GetFTPFiles with response : {0}", JsonConvert.SerializeObject(files));
            
            return files;
        }

        /// <summary>
        /// Get OLEDB Connection string to get data of Excel data source
        /// </summary>
        /// <param name="filePath">Path of file to look up</param>
        /// <param name="IsHeader">Flag to check if first row is header or not</param>
        /// <returns>Returns Connection string which can be directly used to get data from excel data source</returns>
        public static string GetOLEDBConnectionstring(string filePath, bool IsHeader)
        {
            var connectionString = string.Empty;

            var extension = Path.GetExtension(filePath);

            if (extension.Equals(".xls"))
                connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source = '" + filePath + "';Extended Properties=\"Excel 8.0;IMEX=1;HDR=" + (IsHeader ? "YES" : "NO") + "\"";
            else if (extension.Equals(".xlsx"))
                connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filePath + "';Extended Properties=\"Excel 12.0;IMEX=1;HDR=" + (IsHeader ? "YES" : "NO") + "\"";
            else if (extension.Equals(".csv"))
                connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + Path.GetDirectoryName(filePath) + "';Extended Properties=\"text;HDR=" + (IsHeader ? "YES" : "NO") + ";FMT=Delimited(,)\"";

            return connectionString;
        }

        /// <summary>
        /// Get List of Worksheets from excel file
        /// </summary>
        /// <param name="filePath">Path of file to look up</param>
        /// <param name="IsHeader">Flag to check if first row is header or not</param>
        /// <returns>Returns List of MasterItems containing Sheet Names</returns>
        public static List<MasterItem> GetAllWorksheets(string filePath, bool IsHeader)
        {
            var sheets = new List<MasterItem>();
            DataTable dt = null;

            var extension = Path.GetExtension(filePath);
            if (extension.Equals(".csv"))
            {
                sheets.Add(new MasterItem
                {
                    Id = Path.GetFileName(filePath),
                    Name = Path.GetFileName(filePath),
                });
            }
            else if (extension.Equals(".xlsx")
                || extension.Equals(".xls"))
            {
                var connectionString = AppCommon.GetOLEDBConnectionstring(filePath, IsHeader);

                dt = BLOLEDBConnectionSupport.GetAllSheets(connectionString);

                if (dt != null)
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!Convert.ToString(row["TABLE_NAME"]).Contains("FilterDatabase"))
                            sheets.Add(new MasterItem
                            {
                                Name = Convert.ToString(row["TABLE_NAME"]),
                                Id = Convert.ToString(row["TABLE_NAME"]),
                            });
                    }
            }

            return sheets;
        }

        /// <summary>
        /// Get List of columns form specified excel sheet
        /// </summary>
        /// <param name="filePath">Path of file to look up</param>
        /// <param name="IsHeader">Flag to check if first row is header or not</param>
        /// <param name="SheetName">Sheet name to fetch columns from</param>
        /// <returns>Returns List of MasterItems containing Column Names</returns>
        public static List<MasterItem> GetAllWorkSheetColumns(string filePath, bool IsHeader, string SheetName)
        {
            var columns = new List<MasterItem>();

            var extension = Path.GetExtension(filePath);
            var connectionString = AppCommon.GetOLEDBConnectionstring(filePath, IsHeader);

            DataTable dt = null;

            dt = BLOLEDBConnectionSupport.GetAllColumns(connectionString, SheetName);

            if (dt != null)
            {
                if (IsHeader)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        columns.Add(new MasterItem
                        {
                            Name = Convert.ToString(column.ColumnName),
                            Id = Convert.ToString(column.ColumnName),
                        });
                    }
                }
                else
                {
                    // If Header is not present in sheet then build new header as static name Column_1, Column_2 ....
                    var columnNo = 1;
                    foreach (DataColumn column in dt.Columns)
                    {
                        columns.Add(new MasterItem
                        {
                            Name = "Column_" + columnNo,
                            Id = "Column_" + columnNo,
                        });
                        columnNo++;
                    }
                }
            }

            return columns;
        }

        /// <summary>
        /// Get API Id using APIGUID
        /// </summary>
        /// <param name="apiGUID">API GUID</param>
        /// <returns>Returns API Id linked with API GUID</returns>
        public static int GetAPIIdByAPIGUID(string apiGUID)
        {

            var objAPIDataDetail = new BLApi(apiGUID);

            return objAPIDataDetail != null ? objAPIDataDetail.APIId.Value : 0;
        }

        /// <summary>
        /// Get DataTable Using Given JSON Data. It will work for single level of simple kind of objects
        /// </summary>
        /// <param name="JSONData">JSON Data to convert</param>
        /// <returns>Returns DataTable generated from JSON String</returns>
        public static DataTable GetDataTableFromJSON(string JSONData)
        {
            DataTable dtUsingMethodReturn = new DataTable();
            string[] jsonStringArray = Regex.Split(JSONData.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string strJSONarr in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(strJSONarr.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        if (ColumnsNameData.Contains(":"))
                        {
                            int idx = ColumnsNameData.IndexOf(":");
                            string ColumnsNameString = ColumnsNameData.Substring(0, idx).Replace("\"", "").Trim();
                            if (!ColumnsName.Contains(ColumnsNameString))
                            {
                                ColumnsName.Add(ColumnsNameString);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dtUsingMethodReturn.Columns.Add(AddColumnName);
            }
            foreach (string strJSONarr in jsonStringArray)
            {
                string[] RowData = Regex.Split(strJSONarr.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dtUsingMethodReturn.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        if (rowData.Contains(":"))
                        {
                            int idx = rowData.IndexOf(":");
                            string RowColumns = rowData.Substring(0, idx).Replace("\"", "").Trim();
                            string RowDataString = rowData.Substring(idx + 1).Replace("\"", "").Trim();
                            nr[RowColumns] = RowDataString;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                dtUsingMethodReturn.Rows.Add(nr);
            }
            return dtUsingMethodReturn;
        }

        /// <summary>
        /// Remove White space from any model : Currently not in use
        /// </summary>
        /// <param name="obj">Object to remove white space</param>
        private static void RemoveWhiteSpaceFromModel(ref object obj)
        {
            if (obj == null)
                return;

            try
            {
                Type objType = obj.GetType();
                PropertyInfo[] properties = objType.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    object propValue;

                    if (objType == typeof(string))
                    {
                        if (obj != null)
                            obj = RemoveWhiteSpaceFromString(obj.ToString());
                        return;
                    }

                    if (property.PropertyType.IsArray)
                        propValue = (Array)property.GetValue(obj);
                    else
                        propValue = property.GetValue(obj, null);

                    var elems = propValue as IList;

                    if (elems != null)
                    {
                        for (int i = 0; i < elems.Count; ++i)
                        {
                            if (objType != typeof(string))
                            {
                                var ele = elems[i];
                                RemoveWhiteSpaceFromModel(ref ele);
                                elems[i] = ele;
                            }
                            else if (objType == typeof(string))
                            {
                                elems[i] = RemoveWhiteSpaceFromString(elems[i].ToString());
                            }
                        }
                    }
                    else
                    {
                        if (property.PropertyType.Assembly == objType.Assembly && property.PropertyType.IsClass)
                        {
                            RemoveWhiteSpaceFromModel(ref propValue);
                        }
                        else
                        {
                            if (property.PropertyType == typeof(string))
                            {
                                if (propValue != null)
                                {
                                    propValue = RemoveWhiteSpaceFromString(propValue.ToString());
                                    property.SetValue(obj, propValue);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Removes training and leading white space from given string : Currently not in use
        /// </summary>
        /// <param name="strRequest">String object to remove leading and trailing white space</param>
        /// <returns>Returns trailed String</returns>
        private static string RemoveWhiteSpaceFromString(string strRequest)
        {
            return !string.IsNullOrEmpty(strRequest) ? strRequest.Trim() : strRequest;
        }
    }
}
