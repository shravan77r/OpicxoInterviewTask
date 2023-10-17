using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXImport
{
    public static class AppConstant
    {
        public static string MsSQL_DefaultPort = "1433";
        public static string MySQL_DefaultPort = "3307";
        
        public static int DataSource_Excel = 1;
        public static string DataSource_Excel_Name = "Excel (.csv,.xlsx)";
        public static int DataSource_MsSQL = 2;
        public static string DataSource_MsSQL_Name = "MsSQL";
        public static int DataSource_MySQL = 3;
        public static string DataSource_MySQL_Name = "MySQL";
        public static int DataSource_API = 4;
        public static string DataSource_API_Name = "API";
    }
}
