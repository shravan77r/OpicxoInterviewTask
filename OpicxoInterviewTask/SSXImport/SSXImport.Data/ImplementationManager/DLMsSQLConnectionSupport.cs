using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SSXImport.Data.ImplementationManager
{
    /// <summary>
    /// MsSQL Connection support with operations
    /// </summary>
    public class DLMsSQLConnectionSupport
    {
        /// <summary>
        /// Check if credentials are valid or not
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <returns>Returns flag if connection is successful or not</returns>
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

        /// <summary>
        /// Get Database List for given credentials
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>Returns List of All Databases</returns>
        public static DataTable GetAllDatabase(string connectionString)
        {
            return ExecuteQuery(connectionString, "SELECT name as Name FROM sys.databases WHERE state_desc = 'ONLINE' ORDER BY Name");
        }

        /// <summary>
        /// Get Tables List for given credentials
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>Returns List of All Tables</returns>
        public static DataTable GetAllTables(string connectionString)
        {
            return ExecuteQuery(connectionString, "SELECT name as Name FROM sys.tables ORDER BY Name");
        }

        /// <summary>
        /// Get Columns List for given credentials
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="Table"></param>
        /// <param name="IncludePrimaryKey"></param>
        /// <returns>Returns List of All columns for given credentials</returns>
        public static DataTable GetAllColumns(string connectionString, string Table, bool IncludePrimaryKey)
        {
            var query = new StringBuilder();
            query.Append("SELECT COLUMN_NAME AS Name ");
            query.Append("FROM INFORMATION_SCHEMA.COLUMNS  ");
            query.Append("WHERE TABLE_NAME = '" + Table + "' ");
            if (!IncludePrimaryKey)
            {
                query.Append("AND DATA_TYPE <> N'uniqueidentifier' ");

                query.Append("EXCEPT ");

                query.Append("SELECT COLUMN_NAME AS Name ");
                query.Append("FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS [tc] ");
                query.Append("JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE [ku] ON tc.CONSTRAINT_NAME = ku.CONSTRAINT_NAME ");
                query.Append("AND ku.table_name = '" + Table + "'");
            }
            return ExecuteQuery(connectionString, query.ToString());
        }

        /// <summary>
        /// Get GUID Columns List for specified table
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="Table">Table Name</param>
        /// <returns>Returns List of GUID columns</returns>
        public static DataTable GetGUIDColumn(string connectionString, string Table)
        {
            var query = new StringBuilder();
            query.Append("SELECT COLUMN_NAME AS Name ");
            query.Append("FROM INFORMATION_SCHEMA.COLUMNS  ");
            query.Append("WHERE TABLE_NAME = '" + Table + "' ");
            query.Append("AND DATA_TYPE = N'uniqueidentifier' ");

            return ExecuteQuery(connectionString, query.ToString());
        }

        /// <summary>
        /// Get Unique Columns List for given credentials
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="Table">Table Name</param>
        /// <returns>Returns List of Unique Columns</returns>
        public static DataTable GetUniqueColumn(string connectionString, string Table)
        {
            var query = new StringBuilder();
            query.Append("SELECT CC.Column_Name FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC ");
            query.Append("INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CC ON TC.Constraint_Name = CC.Constraint_Name ");
            query.Append("WHERE TC.constraint_type = 'Unique' ");
            query.Append("AND TC.TABLE_NAME = '" + Table + "' ");

            return ExecuteQuery(connectionString, query.ToString());
        }

        /// <summary>
        /// Get Identity Columns List for given credentials
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="Table">Table Name</param>
        /// <returns>Returns List of Identity Columns</returns>
        public static DataTable GetIdentityColumn(string connectionString, string Table)
        {
            var query = new StringBuilder();
            query.Append("SELECT name AS Name ");
            query.Append("FROM sys.identity_columns ");
            query.Append("WHERE OBJECT_NAME(object_id) = '" + Table + "' ");

            return ExecuteQuery(connectionString, query.ToString());
        }

        /// <summary>
        /// Get AUTO INCREMENT for given credentials
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="Table">Table Name</param>
        /// <returns>Returns List of Identity Columns</returns>
        public static DataTable GetAutoIncrementValue(string connectionString, string Table)
        {
            var query = new StringBuilder();
            query.Append("SELECT ISNULL(IDENT_CURRENT('" + Table + "'), 0)");

            return ExecuteQuery(connectionString, query.ToString());
        }

        /// <summary>
        /// Execute Query and return Result in form of Data Table
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="query">Query to execute</param>
        /// <returns>Returns Result of Query</returns>
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

        /// <summary>
        /// Execute Scalar Query
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="query">Query to execute</param>
        public static void ExecuteScalarQuery(string connectionString, string query)
        {
            var staticConnection = new SqlConnection();
            staticConnection.ConnectionString = connectionString;

            var command = new SqlCommand
            {
                CommandText = query,
                CommandType = CommandType.Text,
                Connection = staticConnection
            };

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
        /// Insert Bulk Data Into Temporary Table - Normal (one to one)
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="dataTable">Datatable contains Data</param>
        /// <param name="tableName">Table name</param>
        /// <param name="columns">Column Name for mapping</param>
        /// <returns></returns>
        public static bool InsertBulkDataIntoTempTable<T>(string connectionString, DataTable dataTable,
            string TableName, IEnumerable<Tuple<string, string>> columns, ILogger<T> logger)
        {
            logger.LogDebug("Entered Data-InsertBulkDataIntoTempTable");
            var isSuccess = false;
            try
            {
                using (SqlBulkCopy sqlbc = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.CheckConstraints))
                {
                    sqlbc.DestinationTableName = TableName;
                    foreach (var column in columns)
                        sqlbc.ColumnMappings.Add(column.Item1, column.Item2);
                    sqlbc.WriteToServer(dataTable);
                }
                isSuccess = true;
                logger.LogDebug("Insert into Temporary Table {0} Success", TableName);
            }
            catch (Exception e)
            {
                logger.LogDebug("Insert into Temporary Table {0} Failed", TableName);
                // loop through all inner exceptions to see if any relate to a constraint failure
                bool dataExceptionFound = true;
                Exception tmpException = e;

                if (dataExceptionFound)
                {
                    // call the helper method to document the errors and invalid data
                    string errorMessage = GetBulkCopyFailedData(
                       connectionString,
                       TableName,
                       columns,
                       dataTable.CreateDataReader());

                    logger.LogDebug("Exception : {0}", errorMessage);
                    throw new Exception(errorMessage, e);
                }
                else
                    throw;
            }
            logger.LogDebug("Exited Data-InsertBulkDataIntoTempTable with Status isSucccess : {0}", isSuccess);
            return isSuccess;
        }

        /// <summary>
        /// Build an error message with the failed records and their related exceptions.
        /// </summary>
        /// <param name="connectionString">Connection string to the destination database</param>
        /// <param name="tableName">Table name into which the data will be bulk copied.</param>
        /// <param name="dataReader">DataReader to bulk copy</param>
        /// <returns>Error message with failed constraints and invalid data rows.</returns>
        private static string GetBulkCopyFailedData(
           string connectionString,
           string tableName,
           IEnumerable<Tuple<string, string>> columns,
           IDataReader dataReader)
        {
            StringBuilder errorMessage = new StringBuilder("Bulk Copy failures in Table : " + tableName + Environment.NewLine);
            SqlConnection connection = null;
            SqlTransaction transaction = null;
            SqlBulkCopy bulkCopy = null;
            DataTable tmpDataTable = new DataTable();

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                transaction = connection.BeginTransaction();
                bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.CheckConstraints, transaction);
                bulkCopy.DestinationTableName = tableName;

                // create a datatable with the layout of the data.
                DataTable dataSchema = dataReader.GetSchemaTable();
                foreach (DataRow row in dataSchema.Rows)
                {
                    tmpDataTable.Columns.Add(new DataColumn(
                       row["ColumnName"].ToString(),
                       (Type)row["DataType"]));
                }

                // create an object array to hold the data being transferred into tmpDataTable 
                //in the loop below.
                object[] values = new object[dataReader.FieldCount];

                foreach (var column in columns)
                    bulkCopy.ColumnMappings.Add(column.Item1, column.Item2);

                // loop through the source data
                while (dataReader.Read())
                {
                    // clear the temp DataTable from which the single-record bulk copy will be done
                    tmpDataTable.Rows.Clear();

                    // get the data for the current source row
                    dataReader.GetValues(values);

                    // load the values into the temp DataTable
                    tmpDataTable.LoadDataRow(values, true);

                    // perform the bulk copy of the one row
                    try
                    {
                        bulkCopy.WriteToServer(tmpDataTable);
                    }
                    catch (Exception ex)
                    {
                        // an exception was raised with the bulk copy of the current row. 
                        // The row that caused the current exception is the only one in the temp 
                        // DataTable, so document it and add it to the error message.
                        DataRow faultyDataRow = tmpDataTable.Rows[0];
                        errorMessage.AppendFormat("Error: {0}{1}", ex.Message, Environment.NewLine);
                        errorMessage.AppendFormat("Row data: {0}", Environment.NewLine);
                        foreach (DataColumn column in tmpDataTable.Columns)
                        {
                            errorMessage.AppendFormat(
                               "\tColumn {0} - [{1}]{2}",
                               column.ColumnName,
                               faultyDataRow[column.ColumnName].ToString(),
                               Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                   "Unable to document SqlBulkCopy errors. See inner exceptions for details.",
                   ex);
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return errorMessage.ToString();
        }

        /// <summary>
        /// Insert Bulk Data Into Temporary Table - Normal (one to many) and set the dependent column value
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="dataTable">Datatable Contains data</param>
        /// <param name="tableName">Table name</param>
        /// <param name="columns">Column names</param>
        /// <param name="DependentTableAndTableColumn">Dependent table and column names</param>
        /// <param name="originSourceData">Original Data Source</param>
        /// <param name="targetTableIdentityColumn">Actual Target table Identity Column to set data once inserted</param>
        /// <returns></returns>
        public static bool InsertBulkDataIntoTempTableOneToMany<T>(string connectionString, DataTable dataTable,
            string TableName, IEnumerable<Tuple<string, string>> columns,
            IDictionary<string, string> DependentTableAndTableColumn,
            DataSet originSourceData, string targetTableIdentityColumn, ILogger<T> logger)
        {
            logger.LogDebug("Entered Data-InsertBulkDataIntoTempTableOneToMany");

            var staticConnection = new SqlConnection();
            staticConnection.ConnectionString = connectionString;
            var errorMessage = new StringBuilder();

            var isSuccess = false;
            var errorCount = 0;
            try
            {
                staticConnection.Open();
                var transaction = staticConnection.BeginTransaction();

                int RowCount = 0;

                foreach (DataRow item in dataTable.Rows)
                {
                    var query = new StringBuilder();

                    query.Append("IF EXISTS (SELECT 'X' FROM " + TableName + " WHERE ");
                    foreach (var column in columns)
                    {
                        if (item[column.Item1] == DBNull.Value)
                            query.Append(" " + column.Item2 + " IS NULL AND ");
                        else
                            query.Append(" " + column.Item2 + " = @" + column.Item2 + " AND ");
                    }
                    query.Append(" 1 = 1 ");
                    query.Append(") ");
                    query.Append("BEGIN ");

                    query.Append("SELECT " + targetTableIdentityColumn + " ");
                    query.Append("FROM " + TableName + " ");
                    query.Append("WHERE ");
                    foreach (var column in columns)
                    {
                        if (item[column.Item1] == DBNull.Value)
                            query.Append(" " + column.Item2 + " IS NULL AND ");
                        else
                            query.Append(" " + column.Item2 + " = @" + column.Item2 + " AND ");
                    }
                    query.Append(" 1 = 1 ");

                    query.Append("END ");
                    query.Append("ELSE ");
                    query.Append("BEGIN ");
                    query.Append("INSERT INTO " + TableName + " ");
                    query.Append("( ");

                    foreach (var column in columns)
                        query.Append(" " + column.Item2 + ",");

                    query.Remove(query.Length - 1, 1);
                    query.Append(" ) VALUES ( ");

                    foreach (var column in columns)
                        query.Append(" @" + column.Item2 + " ,");

                    query.Remove(query.Length - 1, 1);
                    query.Append(" ); ");
                    query.Append("SELECT SCOPE_IDENTITY() AS LastInsertId ");
                    query.Append(" END ");

                    var command = new SqlCommand
                    {
                        CommandText = query.ToString(),
                        CommandType = CommandType.Text,
                        Connection = staticConnection,
                        Transaction = transaction
                    };

                    try
                    {
                        foreach (var column in columns)
                            command.Parameters.AddWithValue(column.Item2, item[column.Item1]);

                        int lastInsertId = Convert.ToInt32(command.ExecuteScalar());

                        if (lastInsertId > 0)
                        {
                            foreach (DataTable tableItem in originSourceData.Tables)
                            {
                                if (DependentTableAndTableColumn.ContainsKey(tableItem.TableName))
                                {
                                    string columnName = DependentTableAndTableColumn[tableItem.TableName];

                                    tableItem.Rows[RowCount][columnName] = lastInsertId;
                                }

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        errorMessage.AppendFormat("Error: {0}{1}", ex.Message, Environment.NewLine);
                        errorMessage.AppendFormat("Row data: {0}", Environment.NewLine);
                        foreach (DataColumn dataColumn in dataTable.Columns)
                        {
                            errorMessage.AppendFormat(
                               "\tColumn {0} - [{1}]{2}",
                               dataColumn.ColumnName,
                               item[dataColumn.ColumnName].ToString(),
                               Environment.NewLine);
                        }
                    }
                    finally
                    {
                        command.Dispose();
                    }
                    RowCount++;
                }

                isSuccess = errorCount == 0;

                if (isSuccess)
                {
                    transaction.Commit();
                    logger.LogDebug("Insert into Temporary Table {0} Success", TableName);
                }
                else
                {
                    transaction.Rollback();
                    logger.LogDebug("Insert into Temporary Table {0} Failed", TableName);
                    logger.LogDebug("Exception : " + errorMessage);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (staticConnection != null && staticConnection.State != ConnectionState.Closed)
                    staticConnection.Close();
            }
            logger.LogDebug("Exited Data-InsertBulkDataIntoTempTableOneToMany with IsSuccess : {0}", isSuccess);
            return isSuccess;
        }

        /// <summary>
        /// Insert Bulk Data Into Temporary Table - Normal (many to many) and set the dependent column value
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="dataTable">Datatable Contains data</param>
        /// <param name="TableName">Table name</param>
        /// <param name="columns">Column names</param>
        /// <param name="originSourceData">Original Data Source</param>
        /// <param name="targetTableIdentityColumn">Actual Target table Identity Column to set data once inserted</param>
        /// <returns></returns>
        public static bool InsertBulkDataIntoTempTableManyToMany<T>(string connectionString, DataTable dataTable,
            string TableName, IEnumerable<Tuple<string, string, string>> columns,
            ref DataSet originSourceData, string targetTableIdentityColumn, ILogger<T> logger)
        {
            logger.LogDebug("Entered Data-InsertBulkDataIntoTempTableManyToMany");

            var staticConnection = new SqlConnection();
            staticConnection.ConnectionString = connectionString;
            var errorMessage = new StringBuilder();

            var isSuccess = false;
            var errorCount = 0;
            try
            {
                staticConnection.Open();
                SqlTransaction transaction = staticConnection.BeginTransaction();

                int RowCount = 0;

                foreach (DataRow item in dataTable.Rows)
                {
                    var query = new StringBuilder();

                    query.Append("IF EXISTS (SELECT 'X' FROM " + TableName + " WHERE ");
                    foreach (var column in columns)
                    {
                        if (item[column.Item1] == DBNull.Value)
                            query.Append(" " + column.Item2 + " IS NULL AND ");
                        else
                            query.Append(" " + column.Item2 + " = @" + column.Item2 + " AND ");
                    }
                    query.Append(" 1 = 1 ");
                    query.Append(") ");
                    query.Append("BEGIN ");

                    query.Append("SELECT " + targetTableIdentityColumn + " ");
                    query.Append("FROM " + TableName + " ");
                    query.Append("WHERE ");
                    foreach (var column in columns)
                    {
                        if (item[column.Item1] == DBNull.Value)
                            query.Append(" " + column.Item2 + " IS NULL AND ");
                        else
                            query.Append(" " + column.Item2 + " = @" + column.Item2 + " AND ");
                    }
                    query.Append(" 1 = 1 ");

                    query.Append("END ");
                    query.Append("ELSE ");
                    query.Append("BEGIN ");
                    query.Append("INSERT INTO " + TableName + " ");
                    query.Append("( ");

                    foreach (var column in columns)
                        query.Append(" " + column.Item2 + ",");

                    query.Remove(query.Length - 1, 1);
                    query.Append(" ) VALUES ( ");

                    foreach (var column in columns)
                        query.Append(" @" + column.Item2 + " ,");

                    query.Remove(query.Length - 1, 1);
                    query.Append(" ); ");
                    query.Append("SELECT SCOPE_IDENTITY() AS LastInsertId ");
                    query.Append(" END ");


                    var command = new SqlCommand
                    {
                        CommandText = query.ToString(),
                        CommandType = CommandType.Text,
                        Connection = staticConnection,
                        Transaction = transaction
                    };

                    try
                    {
                        foreach (var column in columns)
                            command.Parameters.AddWithValue(column.Item2, item[column.Item1]);

                        var lastInsertId = Convert.ToInt32(command.ExecuteScalar());

                        if (lastInsertId > 0)
                        {
                            string strCurrentTableName = TableName.Substring(0, TableName.IndexOf("_"));
                            originSourceData.Tables[strCurrentTableName].Rows[RowCount]["_new." + strCurrentTableName + "." + targetTableIdentityColumn] = lastInsertId;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        errorMessage.AppendFormat("Error: {0}{1}", ex.Message, Environment.NewLine);
                        errorMessage.AppendFormat("Row data: {0}", Environment.NewLine);
                        foreach (DataColumn dataColumn in dataTable.Columns)
                        {
                            errorMessage.AppendFormat(
                               "\tColumn {0} - [{1}]{2}",
                               dataColumn.ColumnName,
                               item[dataColumn.ColumnName].ToString(),
                               Environment.NewLine);
                        }
                    }
                    finally
                    {
                        command.Dispose();
                    }
                    RowCount++;
                }

                isSuccess = errorCount == 0;

                if (isSuccess)
                {
                    transaction.Commit();
                    logger.LogDebug("Insert into Temporary Table {0} Success", TableName);
                }
                else
                {
                    transaction.Rollback();
                    logger.LogDebug("Insert into Temporary Table {0} Failed", TableName);
                    logger.LogDebug("Exception : " + errorMessage);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (staticConnection != null && staticConnection.State != ConnectionState.Closed)
                    staticConnection.Close();
            }

            logger.LogDebug("Exited Data-InsertBulkDataIntoTempTableManyToMany with IsSuccess : {0}", isSuccess);

            return isSuccess;
        }
    }
}
