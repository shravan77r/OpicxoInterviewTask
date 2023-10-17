using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace SSXImport.Portal.Common
{
    /// <summary>
    /// Contains All the static members used throughout the Application
    /// </summary>
    public class AppConstant
    {
        //public static IWebHostEnvironment Environment { get; set; }

        public const string DateFormat = "MM/dd/yyyy";
        public const string DateStringFormat = "yyyy-MM-dd'T'hh:mm:ss";
        public const string TimeFormat = "h:mm tt";
        public const string AlertSuccessType = "success";
        public const string AlertInfoType = "info";
        public const string AlertErrorType = "danger";
        public const string AlertWarnignType = "warning";
        public const string AlertRightsWarning = "Please Contact Administrator";

        public const string DummyPicturesPath = "/Document/ProfilePicture";
        public const string TempFilePath = "/Document/Temp/";
        public const string MEDIA_TYPE = "application/json";

        public const string FTP_DefaultPort = "21";
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
        public const int TempType_Value_Normal = 1;
        public const string TempType_Name_Normal = "Normal";
        public const int TempType_Value_OneToMany = 2;
        public const string TempType_Name_OneToMany = "One To Many";
        public const int TempType_Value_ManyToMany = 3;
        public const string TempType_Name_ManyToMany = "Many To Many";

        public const int APIData_Authorization_Value_None = 1;
        public const string APIData_Authorization_Name_None = "None";

        public const int APIData_Authorization_Value_Basic_Auth = 2;
        public const string APIData_Authorization_Name_Basic_Auth = "Basic Authentication";

        public const int APIData_Authorization_Value_Auth2 = 3;
        public const string APIData_Authorization_Name_Auth2 = "Auth2";


        public const int Input_Param_Value_Query_String = 1;
        public const string Input_Param_Name_Query_String = "Query String";

        public const int Input_Param_Value_Header = 2;
        public const string Input_Param_Name_Header = "Header";

        public const int Input_Param_Value_Body = 3;
        public const string Input_Param_Name_Body = "Body";

        public const int Input_Param_Body_Value_Form_Data = 1;
        public const string Input_Param_Body_Name_Form_Data = "form-data";

        public const int Input_Param_Body_Value_Urlencoded = 2;
        public const string Input_Param_Body_Name_Urlencoded = "x-www-form-urlencoded";

        public const int Input_Param_Body_Value_Raw = 3;
        public const string Input_Param_Body_Name_Raw = "raw";

        public const int Output_Param_Value_String = 1;
        public const string Output_Param_Name_String = "String";

        public const int Output_Param_Value_Number = 2;
        public const string Output_Param_Name_Number = "Number";

        public const int Output_Param_Value_Boolean = 3;
        public const string Output_Param_Name_Boolean = "Boolean";

        public const int Output_Param_Value_DateTime = 4;
        public const string Output_Param_Name_DateTime = "DateTime";
    }
}
