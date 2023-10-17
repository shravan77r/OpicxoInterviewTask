using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXImport.MySQLSupport
{
    public class MySQLConnectionSupport
    {
        public static bool CheckCredentials(string connectionString)
        {
            var staticConnection = new MySqlConnection();
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
            return ExecuteQuery(connectionString, "SELECT SCHEMA_NAME as Name FROM information_schema.schemata");
        }

        public static DataTable GetAllTables(string connectionString)
        {
            return ExecuteQuery(connectionString, "SELECT TABLE_NAME as Name FROM information_schema.tables WHERE table_type = 'base table'");
        }

        public static DataTable ExecuteQuery(string connectionString, string query)
        {
            var staticConnection = new MySqlConnection();
            staticConnection.ConnectionString = connectionString;

            var command = new MySqlCommand
            {
                CommandText = query,
                CommandType = CommandType.Text,
                Connection = staticConnection
            };

            var objDataAdapter = new MySqlDataAdapter();
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
