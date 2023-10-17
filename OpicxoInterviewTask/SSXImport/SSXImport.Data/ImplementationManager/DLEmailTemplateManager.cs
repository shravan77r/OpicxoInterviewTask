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
    public class DLEmailTemplateManager : SSXImport_BaseData
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
        public static Tuple<int, DataTable> GetTemplateList(int sortCol, string sortDir,
            int pageIndex, int pageSize, string keyword)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "List_EmailTemplate",
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
        /// Delete Template using Template GUID
        /// </summary>
        /// <param name="templateGUID"></param>
        /// <param name="updatedBy"></param>
        public static void DeleteTemplate(string templatGUID, int updatedBy)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "Delete_EmailTemplate",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@EmailTemplateGUID", templatGUID);
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


        public static Int32? InsertTemplate(string templateName, string templateFile, int enteredBy, string objectType, string ddlObject)
        {
            var staticConnection = StaticSqlConnection;

            Int32? _emailTemplateId = 0;
            var command = new MySqlCommand
            {
                CommandText = "Insert_EmailTemplate",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@emailTemplateName", templateName);

            command.Parameters.AddWithValue("@emailTemplateFile", templateFile);
            command.Parameters.AddWithValue("@enteredBy", enteredBy);
            command.Parameters.AddWithValue("@templateGUID", GetGUID());
            command.Parameters.AddWithValue("@objectType", objectType);
            command.Parameters.AddWithValue("@ddlObject", ddlObject);
            command.Parameters.Add(new MySqlParameter("@emailTemplateId", MySqlDbType.Int32, 0, ParameterDirection.Output, true, 10, 0, "", DataRowVersion.Proposed, 0));


            try
            {
                staticConnection.Open();
                command.ExecuteNonQuery();
                _emailTemplateId = Convert.IsDBNull(command.Parameters["@emailTemplateId"].Value) ? (Int32?)null : (Int32?)command.Parameters["@emailTemplateId"].Value;

                staticConnection.Close();

                return _emailTemplateId;
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

        public static Int32? UpdateTemplate(string templateName, string templateFile,  int templatID, int updatedBy, string objectType, string ddlObject)
        {
            var staticConnection = StaticSqlConnection;

            Int32? _emailTemplateId = 0;
            var command = new MySqlCommand
            {
                CommandText = "Update_EmailTemplate",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@emailTemplateName", templateName);

            command.Parameters.AddWithValue("@emailTemplateFile", templateFile);
            command.Parameters.AddWithValue("@emailTemplateId", templatID);
            command.Parameters.AddWithValue("@updatedBy", updatedBy);

            try
            {
                staticConnection.Open();
                command.ExecuteNonQuery();

                staticConnection.Close();
                return templatID;
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

        public static DataTable GetEmailTemplateByID(string emailtemplateId)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "Get_EmailTemplateById",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@emailTemplateGuId", emailtemplateId);
         
            var objDataAdapter = new MySqlDataAdapter();
            var dt = new DataTable();
            try
            {
                staticConnection.Open();
                objDataAdapter.SelectCommand = command;
                objDataAdapter.Fill(dt);

              
                staticConnection.Close();
                return dt;
            }
            catch
            {
                throw;
            }
            finally
            {
                dt.Dispose();
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


        public static List<string> GetTables()
        {
            var staticConnection = StaticSqlConnection;
            try
            {
               
                    staticConnection.Open();
                    DataTable schema = staticConnection.GetSchema("Tables");
                    List<string> TableNames = new List<string>();
                    foreach (DataRow row in schema.Rows)
                    {
                        TableNames.Add(row[2].ToString());
                    }
                    return TableNames;
                
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                staticConnection.Close();
               
            }
           
        }

        public static DataTable GetFieldsByTableName(string tableName)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "Get_FieldsByTableName",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@tableName", tableName);
          
            var objDataAdapter = new MySqlDataAdapter();
            var dt = new DataTable();
            try
            {
                staticConnection.Open();
                objDataAdapter.SelectCommand = command;
                objDataAdapter.Fill(dt);

                
                staticConnection.Close();
                return dt;
            }
            catch
            {
                throw;
            }
            finally
            {
                dt.Dispose();
                objDataAdapter.Dispose();
                staticConnection.Close();
                command.Dispose();
            }

        }

        public static DataTable GetDataByTableName(string tableName)
        {
            var staticConnection = StaticSqlConnection;
            var command = new MySqlCommand
            {
                CommandText = "Get_DataByTableName",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };

            command.Parameters.AddWithValue("@tableName", tableName);

            var objDataAdapter = new MySqlDataAdapter();
            var dt = new DataTable();
            try
            {
                staticConnection.Open();
                objDataAdapter.SelectCommand = command;
                objDataAdapter.Fill(dt);


                staticConnection.Close();
                return dt;
            }
            catch
            {
                throw;
            }
            finally
            {
                dt.Dispose();
                objDataAdapter.Dispose();
                staticConnection.Close();
                command.Dispose();
            }

        }

        public static List<string> GetViews()
        {
            var staticConnection = StaticSqlConnection;
            try
            {

                staticConnection.Open();
                DataTable schema = staticConnection.GetSchema("Views");
                List<string> TableNames = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    TableNames.Add(row[2].ToString());
                }
                return TableNames;

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                staticConnection.Close();

            }

        }


        public static string GetGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
