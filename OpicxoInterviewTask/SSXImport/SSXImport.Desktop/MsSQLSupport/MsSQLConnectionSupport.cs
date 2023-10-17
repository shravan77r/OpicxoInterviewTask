using System.Data;
using System.Data.SqlClient;

namespace SSXImport.MsSQLSupport
{
    public class MsSQLConnectionSupport
    {
        public static bool CheckCredentials(string connectionString)
        {
            var staticConnection = new SqlConnection();
            staticConnection.ConnectionString = connectionString;

            try
            {
                staticConnection.Open();
                return true;
            }
            catch
            {
                return false;
                //throw; 
            }
            finally
            {
                staticConnection.Close();
            }
        }

        public static DataTable GetAllDatabase(string connectionString)
        {
            return ExecuteQuery(connectionString, "SELECT name as Name FROM sys.databases WHERE state_desc = 'ONLINE'");
        }

        public static DataTable GetAllTables(string connectionString)
        {
            return ExecuteQuery(connectionString, "SELECT name as Name FROM sys.tables");
        }

        public static DataTable ExecuteQuery(string connectionString, string query)
        {
            var staticConnection = new SqlConnection();
            staticConnection.ConnectionString = connectionString;

            var command = new SqlCommand
            {
                CommandText = query,
                CommandType = CommandType.Text,
                Connection = staticConnection
            };

            var objDataAdapter = new SqlDataAdapter();
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
