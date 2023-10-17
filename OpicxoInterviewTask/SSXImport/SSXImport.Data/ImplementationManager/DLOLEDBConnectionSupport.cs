using System.Data;
using System.Data.OleDb;

namespace SSXImport.Data.ImplementationManager
{
    /// <summary>
    /// Excel (OLEDB) related operations
    /// </summary>
    public class DLOLEDBConnectionSupport
    {
        /// <summary>
        /// Get All Sheets for given excel file
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <returns>Returns List of Sheet</returns>
        public static DataTable GetAllSheets(string connectionString)
        {
            OleDbConnection objConn = null;
            DataTable dt = null;
            try
            {
                objConn = new OleDbConnection(connectionString);
                objConn.Open();

                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                objConn.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                    dt.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Get All Columns for given Excel Sheet
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="tableName">Excel sheet name</param>
        /// <returns></returns>
        public static DataTable GetAllColumns(string connectionString, string sheetName)
        {
            return ExecuteQuery(connectionString, "SELECT top 1 * From [" + sheetName + "]");
        }

        /// <summary>
        /// Execute Query and return Result in form of Data Table
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="query">Query to execute</param>
        /// <returns>Returns Result of Query</returns>
        public static DataTable ExecuteQuery(string connectionString, string query)
        {
            var staticConnection = new OleDbConnection();
            staticConnection.ConnectionString = connectionString;

            var command = new OleDbCommand
            {
                CommandText = query,
                CommandType = CommandType.Text,
                Connection = staticConnection
            };

            var objDataAdapter = new OleDbDataAdapter();
            var ds = new DataSet();
            try
            {
                staticConnection.Open();
                objDataAdapter.SelectCommand = command;
                objDataAdapter.Fill(ds);

                staticConnection.Close();
                return ds.Tables[0];
            }
            catch
            {
                throw;
            }
            finally
            {
                ds.Dispose();
                objDataAdapter.Dispose();
                staticConnection.Close();
                command.Dispose();
            }
        }
    }
}
