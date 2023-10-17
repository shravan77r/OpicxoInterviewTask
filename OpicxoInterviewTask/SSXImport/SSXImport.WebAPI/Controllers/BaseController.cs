using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SSXImport.Business.ImplementationManager;
using SSXImport.WebAPI.Common;
using SSXImport.WebAPI.Models.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;

namespace SSXImport.WebAPI.Controllers
{
    /// <summary>
    /// Used for All API Controllers provides basic methods like Logs and Type Conversions
    /// </summary>
    public class BaseController : ControllerBase
    {

        #region Response Methods

        protected static Response response = null;

        protected static Response GetException(Exception e = null, string MessageType = "danger")
        {
            return new Response
            {
                Status = AppConstant.StatusFailed,
                Message = e != null ? AppConstant.MessageException : AppConstant.MessageError,
                Count = 0,
                MessageType = MessageType,
                Data = e.Message
            };
        }

        protected static Response GetNoRecordFound(string Message = "Record not found.", string MessageType = "danger")
        {
            return new Response
            {
                Status = AppConstant.StatusFailed,
                Message = Message,
                MessageType = MessageType,
                Count = 0,
            };
        }

        protected static Response GetSuccessResponse(object Object, int Count = 1, string Message = "Record found", string MessageType = "success")
        {
            return new Response
            {
                Status = AppConstant.StatusSuccess,
                Count = Count,
                MessageType = MessageType,
                Message = Message,
                Data = Object ?? string.Empty
            };
        }

        #endregion

        #region Log Writer
        
        protected static void WriteDataTransferFile(int dataTransferId, string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return;

                var filePath = Path.Combine(AppCommon.Environment.WebRootPath, "Document", "DataTransfer", dataTransferId.ToString());

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                var fileName = Path.Combine(filePath, "Logs.txt");

                if (!System.IO.File.Exists(fileName))
                {
                    System.IO.File.Create(fileName).Close();
                }
                using StreamWriter sw = System.IO.File.AppendText(fileName);
                sw.WriteLine("#");
                sw.WriteLine(DateTime.Now.ToString());
                sw.WriteLine(str);
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }

        #endregion

        #region Extra Methods
        protected static bool IsValidInt(int? id)
        {
            return (id != null && id >= 0);
        }
        protected static bool IsValidId(int? id)
        {
            return (id != null && id > 0);
        }
        protected static bool IsValidId(string id)
        {
            return ToInt(id) > 0;
        }
        protected static bool IsStr(string str)
        {
            return !string.IsNullOrEmpty(str);
        }
        protected static string toStr(string str)
        {
            return !string.IsNullOrEmpty(str) ? str.Trim() : null;
        }
        protected static string toStr(object str)
        {
            return !string.IsNullOrEmpty(Convert.ToString(str)) ? Convert.ToString(str).Trim() : null;
        }
        protected static bool IsNumeric(string str)
        {
            return int.TryParse(str, out _);
        }
        protected static int? toNullableInt(object val)
        {
            if (val != null && val != DBNull.Value)
                return Convert.ToInt32(val);
            return null;
        }
        protected static long ConvertToUnixTime(DateTime datetime)
        {
            var sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(datetime - sTime).TotalSeconds;
        }
        protected static DateTime UnixTimeToDateTime(long unixtime)
        {
            var sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return sTime.AddSeconds(unixtime);
        }
        protected static DateTime? ConvertDate(string date)
        {
            DateTime? _dateTime = null;
            try
            {
                if (IsStr(date))
                    _dateTime = DateTime.ParseExact(date, AppConstant.DateFormat, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw;
            }
            return _dateTime;
        }
        protected static string ConvertDate(DateTime? dateTime)
        {
            string _date = string.Empty;
            try
            {
                if (dateTime != null)
                    _date = Convert.ToDateTime(dateTime).ToString(AppConstant.DateFormat, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw;
            }
            return _date;
        }
        protected static TimeSpan? ConvertTime(string date)
        {
            TimeSpan? _time = null;
            try
            {
                if (IsStr(date))
                    _time = DateTime.ParseExact(date, AppConstant.TimeFormat, CultureInfo.InvariantCulture).TimeOfDay;
            }
            catch 
            {
                throw;
            }
            return _time;
        }
        protected static string ConvertTime(TimeSpan? dateTime)
        {
            string _time = string.Empty;
            try
            {
                if (dateTime != null)
                    _time = DateTime.Today.Add(dateTime.Value).ToString(AppConstant.TimeFormat, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw;
            }
            return _time;
        }
        public static string GetGUID()
        {
            return Guid.NewGuid().ToString();
        }
        

        #endregion

        #region Type Conversion Methods

        protected static int? ToNullableInt(int? id)
        {
            return (id != null && id >= 0) ? id : null;
        }
        protected static int ToFinalInt(int? val, int defaultValue = 0)
        {
            return (val != null && val > 0) ? ToInt(val) : defaultValue;
        }
        protected static string ToNullableString(string val, string defaultValue = null)
        {
            return IsStr(val) ? val.Trim() : defaultValue;
        }
        protected static string ToStr(string str)
        {
            return IsStr(str) ? str : string.Empty;
        }
        protected static string ToStr(int? str)
        {
            return Convert.ToString(ToFinalInt(str));
        }
        protected static int ToInt(string str)
        {
            return IsStr(str) ? Convert.ToInt32(str) : 0;
        }
        protected static int ToInt(int? num)
        {
            return num != null ? Convert.ToInt32(num) : 0;
        }
        protected static DateTime ToDateTime(string str)
        {
            return IsStr(str) ? DateTime.Parse(str, CultureInfo.InvariantCulture) : new DateTime();
        }
        protected static DateTime? ToNullableDate(string str)
        {
            DateTime? nullablDate = null;
            if (IsStr(str))
            {
                nullablDate = DateTime.Parse(str, CultureInfo.InvariantCulture);
            }
            return nullablDate;
        }
        protected static decimal? ToNullableDecimal(string str)
        {
            decimal? nullablDate = null;
            if (IsStr(str))
            {
                nullablDate = Convert.ToDecimal(str);
            }
            return nullablDate;
        }
        protected static decimal? ToDecimal(string str)
        {
            return IsStr(str) ? Convert.ToDecimal(str) : 0;
        }
        protected static bool? ToBoolean(string str)
        {
            return IsStr(str) ? Convert.ToBoolean(ToInt(str)) : false;
        }
        protected static DateTime GetDateTime() { return AppCommon.GetDateTime(); }

        #endregion

        #region DataTable response generator

        protected static List<Dictionary<string, string>> GetDataTableData(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                List<Dictionary<string, string>> items = new List<Dictionary<string, string>>();

                foreach (DataRow dr in dt.Rows)
                {
                    var dict = new Dictionary<string, string>();

                    foreach (DataColumn column in dt.Columns)
                    {
                        var ColumnName = column.ColumnName;
                        var ColumnData = dr[column].ToString();
                        dict.Add(ColumnName, ColumnData);
                    }
                    items.Add(dict);
                }
                return items;
            }
            return null;
        }

        protected static Response GetDataTableResponse(DataTable dt)
        {
            List<Dictionary<string, string>> items = new List<Dictionary<string, string>>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var dict = new Dictionary<string, string>();

                    foreach (DataColumn column in dt.Columns)
                    {
                        var ColumnName = column.ColumnName;
                        var ColumnData = dr[column].ToString();
                        dict.Add(ColumnName, ColumnData);
                    }
                    items.Add(dict);
                }
            }

            return GetSuccessResponse(items, items.Count);
        }

        protected static Response GetDataTableRowResponse(DataTable dt)
        {
            Dictionary<string, string> items = new Dictionary<string, string>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    var ColumnName = column.ColumnName;
                    var ColumnData = dt.Rows[0][column].ToString();
                    items.Add(ColumnName, ColumnData);
                }
            }

            return GetSuccessResponse(items, items.Count);
        }

        #endregion

        #region Create Folders if Not Exist

        protected static bool CreateFolderIfNotExists(string path)
        {
            bool result = false;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    result = true;
                }
                catch
                {
                    throw;
                }
            }
            return result;
        }

        #endregion

    }
}


