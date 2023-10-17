using SSXImport.Data.ImplementationManager;
using System.Data;
using System.Data.SqlClient;

namespace SSXImport.Business.ImplementationManager
{
    /// <summary>
    /// Excel (OLEDB) related operations
    /// </summary>
    public class BLOLEDBConnectionSupport
    {
        /// <summary>
        /// Get All Sheets for given excel file
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <returns>Returns List of Sheet</returns>
        public static DataTable GetAllSheets(string connectionString)
        {
            try
            {
                return DLOLEDBConnectionSupport.GetAllSheets(connectionString);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get All Columns for given Excel Sheet
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="tableName">Excel sheet name</param>
        /// <returns></returns>
        public static DataTable GetAllColumns(string connectionString, string tableName)
        {
            try
            {
                return DLOLEDBConnectionSupport.GetAllColumns(connectionString, tableName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Execute Query and return Result in form of Data Table
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="query">Query to execute</param>
        /// <returns>Returns Result of Query</returns>
        public static DataTable ExecuteQuery(string connectionString, string query)
        {
            try
            {
                return DLOLEDBConnectionSupport.ExecuteQuery(connectionString, query);
            }
            catch
            {
                throw;
            }
        }

    }
}
