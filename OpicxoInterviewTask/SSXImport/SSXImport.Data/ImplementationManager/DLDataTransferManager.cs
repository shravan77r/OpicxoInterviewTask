using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SSXImport.Data.ImplementationManager
{
    /// <summary>
    /// Data Transfer related Operations
    /// </summary>
    public class DLDataTransferManager : SSXImport_BaseData
    {
        /// <summary>
        /// Get Data Transfer List based on given parameters
        /// </summary>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static Tuple<int, DataTable> GetDataTransferList(int sortCol, string sortDir,
            int pageIndex, int pageSize, string keyword)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "List_DataTransfer",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("SortCol", sortCol);
            command.Parameters.AddWithValue("SortDir", sortDir);
            command.Parameters.AddWithValue("PageIndex", pageIndex);
            command.Parameters.AddWithValue("PageSize", pageSize);
            command.Parameters.AddWithValue("Keyword", keyword);

            var parTotalCount = new MySqlParameter("RecordCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
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
        /// Get Column mapping Status for Data Transfer to check if transfer can be start or not
        /// </summary>
        /// <param name="templateGUID">Template GUID</param>
        /// <returns></returns>
        public static DataSet GetColumnMappingForDataTransfer(Guid templateGUID)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "GetColumnMappingForDataTransfer",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("templateGUID", templateGUID);

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
        /// Save Data Transfer from Template GUID (Replicate Template master data into Data Transfer Tables)
        /// </summary>
        /// <param name="templateGUID"></param>
        /// <returns></returns>
        public static DataSet SaveDataTransferFromTemplateGUID(Guid templateGUID)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "CreateDataTransferFromTemplateId",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("p_TemplateGUID", templateGUID);

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

    }
}
