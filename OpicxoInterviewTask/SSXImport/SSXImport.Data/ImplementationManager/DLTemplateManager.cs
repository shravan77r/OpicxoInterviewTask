using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SSXImport.Data.ImplementationManager
{
    /// <summary>
    /// Template Related operations
    /// </summary>
    public class DLTemplateManager : SSXImport_BaseData
    {
        /// <summary>
        /// Get Template list based on given parameters
        /// </summary>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static Tuple<int, DataTable> GetTemplateList(int sortCol, string sortDir, int pageIndex, int pageSize, string keyword, string applicationGuid)
        {
            var staticConnection = StaticSqlConnection;

            // Get ConnectionString based on the applicationGuid

            var command = new MySqlCommand
            {
                CommandText = "List_Template",
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

                if (!string.IsNullOrEmpty(applicationGuid))
                {
                    DataRow dr = ds.Tables[0].NewRow();

                    dr[0] = "101";
                    dr[1] = Guid.NewGuid().ToString();
                    dr[2] = applicationGuid;
                    dr[3] = "Session-Source";
                    dr[4] = "session-Target";
                    dr[5] = 0;
                    dr[6] = 1;

                    ds.Tables[0].Rows.Add(dr);
                }

                //var objUserReturnValue = Tuple.Create((int)parTotalCount.Value, ds.Tables[0]);
                var objUserReturnValue = Tuple.Create((int)ds.Tables[0].Rows.Count, ds.Tables[0]);

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
        /// Delete Template using Template GUID
        /// </summary>
        /// <param name="templateGUID"></param>
        /// <param name="updatedBy"></param>
        public static void DeleteTemplate(Guid templateGUID, int updatedBy)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "Delete_Template",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@templateGUID", templateGUID.ToString());
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
        /// Get Template Table List for given Template GUID
        /// </summary>
        /// <param name="templateGUID">Template GUID</param>
        /// <returns>Returns List of Template Tables</returns>
        public static DataTable GetTemplateTableDetailsList(string templateGUID)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "List_TemplateTableDetail",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@TemplateGUID", templateGUID);

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
        /// Delete Template Table Detail bases on given Template Table Detail Id
        /// </summary>
        /// <param name="templateTableDetailId"></param>
        /// <param name="updatedBy"></param>
        public static void DeleteTemplateTableDetail(int templateTableDetailId, int updatedBy)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "Delete_TemplateTableDetail",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@templateTableDetailId", templateTableDetailId);
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
        /// Delete Template Column Detail based on Template Table Detail GUID
        /// </summary>
        /// <param name="templateTableDetailGUID"></param>
        /// <param name="updatedBy"></param>
        public static void DeleteTemplateColumnDetail(Guid templateTableDetailGUID, int updatedBy)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "Delete_TemplateColumnDetail",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@templateTableDetailGUID", templateTableDetailGUID);
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
        /// Delete Template Columns Details based on Template GUID
        /// </summary>
        /// <param name="templateGUID"></param>
        /// <param name="updatedBy"></param>
        public static void DeleteTemplateColumnDetailFromTemplateGUID(Guid templateGUID, int updatedBy)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "Delete_TemplateColumnDetail_FromTemplateGUID",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@templateGUID", templateGUID);
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
        /// Get Template Column Details List based on Template GUID and Template Table Detail GUID
        /// </summary>
        /// <param name="templateGUID"></param>
        /// <param name="templateTableDetailGUID"></param>
        /// <returns></returns>
        public static DataSet GetTemplateColumnDetailList(Guid templateGUID, Guid templateTableDetailGUID)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "GetTemplateColumnDetailList",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@templateGUID", templateGUID);
            command.Parameters.AddWithValue("@templateTableDetailGUID", templateTableDetailGUID);

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
        /// Execute Scaler query
        /// </summary>
        /// <param name="query"></param>
        public static void ExecuteScalarQuery(string query)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = query,
                CommandType = CommandType.Text,
                Connection = staticConnection
            };

            try
            {
                staticConnection.Open();

                command.ExecuteScalar();

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



    }
}
