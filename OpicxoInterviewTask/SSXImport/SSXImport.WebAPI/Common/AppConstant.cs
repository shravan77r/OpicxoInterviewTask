using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SSXImport.WebAPI.Common
{
    /// <summary>
    /// Contains All the static members used throughout the Application
    /// </summary>
    public class AppConstant
    {
        //public static IWebHostEnvironment Environment { get; set; }

        public const string MessageInsert = "Inserted successfully.";
        public const string MessageUpdate = "Updated successfully.";
        public const string MessageDelete = "Deleted successfully.";
        public const string MessageException = "Something went wrong!";
        public const string MessageError = "Some error occurred.";
        public const string MessageInvalidGUID = "Invalid ID passed.";

        public const string AlertSuccessType = "success";      // Green
        public const string AlertInfoType = "info";            // Blue
        public const string AlertErrorType = "danger";         // Red
        public const string AlertWarnignType = "warning";      // Yellow

        public const string MessageRecordFound = "Record found";
        public const string MessageRecordNotFound = "Record not found";

        public const int StatusFailed = 0;
        public const int StatusSuccess = 1;

        public const string DateFormat = "MM/dd/yyyy";
        public const string TimeFormat = "h:mm tt";

        public const string MsSQL_DefaultPort = "1433";
        public const string MySQL_DefaultPort = "3306";

        public const int DataSource_Excel = 1;
        public const string DataSource_Excel_Name = "Excel (.csv,.xlsx)";
        public const int DataSource_MsSQL = 2;
        public const string DataSource_MsSQL_Name = "MsSQL";
        public const int DataSource_MySQL = 3;
        public const string DataSource_MySQL_Name = "MySQL";
        public const int DataSource_API = 4;
        public const string DataSource_API_Name = "API";

        public const int APIData_Authorization_None = 1;
        public const int APIData_Authorization_Basic_Auth = 2;
        public const int APIData_Authorization_Auth2 = 3;
    }

    public static class ExtensionMethods
    {
        /// <summary>
        /// Cast One object to specified type
        /// </summary>
        /// <typeparam name="T">Type of Object in which object need to be casted</typeparam>
        /// <param name="myobj">Object which needs to be casted</param>
        /// <returns>Returns object in specified type with data</returns>
        public static T Cast<T>(this object myobj)
        {
            Type objectType = myobj.GetType();
            Type target = typeof(T);
            var x = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
               .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                try
                {
                    propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                    value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);
                    if (value != null && propertyInfo != null)
                        propertyInfo.SetValue(x, value, null);
                }
                catch { }
            }
            return (T)x;
        }

    }

}
