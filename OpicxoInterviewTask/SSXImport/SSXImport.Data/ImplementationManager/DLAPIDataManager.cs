using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SSXImport.Data.ImplementationManager
{
    /// <summary>
    /// API Data Related operations
    /// </summary>
    public class DLAPIDataManager : SSXImport_BaseData
    {

        /// <summary>
        /// Get API Template List based on given parameters
        /// </summary>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static Tuple<int, DataTable> GetAPIList(int sortCol, string sortDir,
            int pageIndex, int pageSize, string keyword)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "List_APIData",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@sortCol", sortCol);
            command.Parameters.AddWithValue("@sortDir", sortDir);
            command.Parameters.AddWithValue("@pageIndex", pageIndex);
            command.Parameters.AddWithValue("@pageSize", pageSize);
            command.Parameters.AddWithValue("@keyword", keyword);

            var parTotalCount = new MySqlParameter("@recordCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(parTotalCount);

            var objDataAdapter = new MySqlDataAdapter();
            var ds = new DataSet();
            try
            {
                staticConnection.Open();
                objDataAdapter.SelectCommand = command;
                objDataAdapter.Fill(ds);

                var objUserReturnValue = Tuple.Create((int)parTotalCount.Value, ds.Tables[0]);
                staticConnection.Close();
                return objUserReturnValue;
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

        /// <summary>
        /// Delete API Data based on apiGUID
        /// </summary>
        /// <param name="apiGUID"></param>
        /// <param name="updatedBy"></param>
        public static void DeleteAPI(Guid apiGUID, int updatedBy)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "Delete_API",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@apiGUID", apiGUID.ToString());
            command.Parameters.AddWithValue("@updatedBy", updatedBy);

            try
            {
                staticConnection.Open();
                command.ExecuteNonQuery();

                staticConnection.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                staticConnection.Close();
                command.Dispose();
            }
        }

        /// <summary>
        /// Get API Data, Input Parameter Data and Output Parameter data for API Execution.
        /// </summary>
        /// <param name="apiGUID">API GUID to fetch data from</param>
        /// <returns>Returns Multiple Tables Containing main API Data with Authentication API Data</returns>
        public static DataSet GetAPIData(string apiGUID)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "GetAPIData",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@apiGUID", apiGUID);

            var objDataAdapter = new MySqlDataAdapter();
            var ds = new DataSet();
            try
            {
                staticConnection.Open();
                objDataAdapter.SelectCommand = command;
                objDataAdapter.Fill(ds);

                staticConnection.Close();
                return ds;
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

        /// <summary>
        /// Get API Input Parameter List based on API GUID
        /// </summary>
        /// <param name="apiGUID">API GUID</param>
        /// <returns></returns>
        public static DataTable GetAPIInputParameterDetailsList(string apiGUID)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "List_APIInputParameterDetail",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@APIGUID", apiGUID);

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

        /// <summary>
        /// Get API Output Parameter List based on AP GUID
        /// </summary>
        /// <param name="apiGUID"></param>
        /// <returns></returns>
        public static DataTable GetAPIOutputParameterDetailsList(string apiGUID)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "List_APIOutputParameterDetail",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@APIGUID", apiGUID);

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
