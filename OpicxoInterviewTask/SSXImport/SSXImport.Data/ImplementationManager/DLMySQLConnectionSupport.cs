using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SSXImport.Data.ImplementationManager
{
    /// <summary>
    /// MySQL Connection support with operations
    /// </summary>
    public class DLMySQLConnectionSupport
    {
        /// <summary>
        /// Check if credentials are valid or not
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <returns>Returns flag if connection is successful or not</returns>
        public static bool CheckCredentials(string connectionString)
        {
            var staticConnection = new MySqlConnection();
            staticConnection.ConnectionString = connectionString;

            try
            {
                staticConnection.Open();
                return true;
            }
            catch (Exception e)
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
            return ExecuteQuery(connectionString, "SELECT SCHEMA_NAME as Name FROM information_schema.schemata ORDER BY Name");
        }

        /// <summary>
        /// Get Tables List for given credentials
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>Returns List of All Tables</returns>
        public static DataTable GetAllTables(string connectionString, string database)
        {
            return ExecuteQuery(connectionString, "SELECT TABLE_NAME as Name FROM information_schema.tables WHERE table_type = 'base table' AND TABLE_SCHEMA = '" + database + "' ORDER BY Name");
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
            query.Append("FROM `INFORMATION_SCHEMA`.`COLUMNS` ");
            query.Append("WHERE `TABLE_NAME` = '" + Table + "' ");
            if (!IncludePrimaryKey)
                query.Append("AND COLUMN_KEY != 'PRI' ");
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
            query.Append("SELECT COLUMN_NAME AS Name ");
            query.Append("FROM `INFORMATION_SCHEMA`.`COLUMNS` ");
            query.Append("WHERE `TABLE_NAME` = '" + Table + "' ");
            query.Append("AND COLUMN_KEY = 'PRI' ");

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
            query.Append("SELECT IFNULL((SELECT AUTO_INCREMENT ");
            query.Append("  FROM `INFORMATION_SCHEMA`.`TABLES`");
            query.Append("  WHERE `TABLE_NAME` = '" + Table + "' ");
            query.Append("), 0) AS AUTO_INCREMENT");

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
            var staticConnection = new MySqlConnection();
            staticConnection.ConnectionString = connectionString;
            MySqlTransaction transaction = null;

            var command = new MySqlCommand
            {
                CommandText = query,
                CommandType = CommandType.Text,
                Connection = staticConnection
            };

            var objDataAdapter = new MySqlDataAdapter();
            DataSet ds = new DataSet();
            try
            {
                staticConnection.Open();
                transaction = staticConnection.BeginTransaction();

                objDataAdapter.SelectCommand = command;
                objDataAdapter.Fill(ds);

                transaction.Commit();

                return ds.Tables[0];
            }
            catch
            {
                throw;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                if (objDataAdapter != null)
                    objDataAdapter.Dispose();
                if (staticConnection != null && staticConnection.State == ConnectionState.Open)
                    staticConnection.Close();
                if (command != null)
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
            var staticConnection = new MySqlConnection();
            staticConnection.ConnectionString = connectionString;
            MySqlTransaction transaction = null;
            var command = new MySqlCommand
            {
                CommandText = query,
                CommandType = CommandType.Text,
                Connection = staticConnection
            };

            try
            {
                staticConnection.Open();
                transaction = staticConnection.BeginTransaction();
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                if (staticConnection != null && staticConnection.State == ConnectionState.Open)
                    staticConnection.Close();
                if (command != null)
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

            connectionString += ";AllowLoadLocalInfile=True";

            var staticConnection = new MySqlConnection();
            staticConnection.ConnectionString = connectionString;
            var errorMessage = new StringBuilder();

            var isSuccess = false;
            var errorCount = 0;
            try
            {
                staticConnection.Open();
                var transaction = staticConnection.BeginTransaction();

                foreach (DataRow item in dataTable.Rows)
                {
                    var query = new StringBuilder("INSERT INTO `" + TableName + "` ");
                    query.Append("( ");
                    foreach (var column in columns)
                        query.Append(" " + column.Item2 + ",");

                    query.Remove(query.Length - 1, 1);
                    query.Append(" ) VALUES ( ");

                    foreach (var column in columns)
                        query.Append(" @" + column.Item2 + " ,");

                    query.Remove(query.Length - 1, 1);
                    query.Append(" ); ");

                    query.Append("select last_insert_id();");


                    var command = new MySqlCommand
                    {
                        CommandText = query.ToString(),
                        CommandType = CommandType.Text,
                        Connection = staticConnection
                    };

                    try
                    {
                        foreach (var column in columns)
                            command.Parameters.AddWithValue(column.Item2, item[column.Item1]);

                        command.ExecuteNonQuery();
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
                    logger.LogDebug("Exception : {0}", errorMessage.ToString());
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

            logger.LogDebug("Exited Data-InsertBulkDataIntoTempTable with Status isSucccess : {0}", isSuccess);

            return isSuccess;
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
            IDictionary<string, string> DependentTableAndTableColumn, DataSet originSourceData, 
            string targetTableIdentityColumn, ILogger<T> logger)
        {
            logger.LogDebug("Entered Data-InsertBulkDataIntoTempTableOneToMany");

            connectionString += ";AllowLoadLocalInfile=True";

            var staticConnection = new MySqlConnection();
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

                    int lastInsertId = 0;
                    // Select Query to find out if the value already exists in DB
                    var slectQuery = new StringBuilder();

                    slectQuery.Append("SELECT " + targetTableIdentityColumn + " FROM `" + TableName + "` WHERE ");
                    foreach (var column in columns)
                    {
                        if (item[column.Item1] == DBNull.Value)
                            slectQuery.Append(" " + column.Item2 + " IS NULL AND ");
                        else
                            slectQuery.Append(" " + column.Item2 + " = @" + column.Item2 + " AND ");
                    }
                    slectQuery.Append(" 1 = 1 ");

                    var command = new MySqlCommand
                    {
                        CommandText = slectQuery.ToString(),
                        CommandType = CommandType.Text,
                        Connection = staticConnection
                    };


                    try
                    {
                        foreach (var column in columns)
                            command.Parameters.AddWithValue(column.Item2, item[column.Item1]);

                        lastInsertId = Convert.ToInt32(command.ExecuteScalar());
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

                    // If Value not exists in DB then only insert it otherwise 
                    if (lastInsertId == 0)
                    {
                        var insertQuery = new StringBuilder();

                        insertQuery.Append("INSERT INTO `" + TableName + "` ");
                        insertQuery.Append("( ");
                        foreach (var column in columns)
                            insertQuery.Append(" " + column.Item2 + ",");

                        insertQuery.Remove(insertQuery.Length - 1, 1);
                        insertQuery.Append(" ) VALUES ( ");

                        foreach (var column in columns)
                            insertQuery.Append(" @" + column.Item2 + " ,");

                        insertQuery.Remove(insertQuery.Length - 1, 1);
                        insertQuery.Append(" ); ");

                        insertQuery.Append("SELECT last_insert_id();");


                        command = new MySqlCommand
                        {
                            CommandText = insertQuery.ToString(),
                            CommandType = CommandType.Text,
                            Connection = staticConnection
                        };

                        try
                        {
                            foreach (var column in columns)
                                command.Parameters.AddWithValue(column.Item2, item[column.Item1]);

                            lastInsertId = Convert.ToInt32(command.ExecuteScalar());

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
                    }
                    else
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
        /// <param name="tableName">Table name</param>
        /// <param name="columns">Column names</param>
        /// <param name="DependentTableAndTableColumn">Dependent table and column names</param>
        /// <param name="originSourceData">Original Data Source</param>
        /// <param name="targetTableIdentityColumn">Actual Target table Identity Column to set data once inserted</param>
        /// <returns></returns>
        public static bool InsertBulkDataIntoTempTableManyToMany<T>(string connectionString, DataTable dataTable,
           string TableName, IEnumerable<Tuple<string, string, string>> columns,
           IDictionary<string, string> DependentTableAndTableColumn,
           ref DataSet originSourceData, string targetTableIdentityColumn, ILogger<T> logger)
        {
            logger.LogDebug("Entered Data-InsertBulkDataIntoTempTableManyToMany");

            connectionString += ";AllowLoadLocalInfile=True";

            var staticConnection = new MySqlConnection();
            staticConnection.ConnectionString = connectionString;
            var errorMessage = new StringBuilder();

            var isSuccess = false;
            var errorCount = 0;
            int lastInsertId = 0;
            try
            {
                MySqlTransaction transaction = staticConnection.BeginTransaction();

                staticConnection.Open();

                int RowCount = 0;

                foreach (DataRow item in dataTable.Rows)
                {
                    #region Check for existing Row

                    var slectQuery = new StringBuilder();

                    slectQuery.Append("SELECT " + targetTableIdentityColumn + " FROM `" + TableName + "` WHERE ");
                    foreach (var column in columns)
                    {
                        if (item[column.Item1] == DBNull.Value)
                            slectQuery.Append(" " + column.Item2 + " IS NULL AND ");
                        else
                            slectQuery.Append(" " + column.Item2 + " = @" + column.Item2 + " AND ");
                    }
                    slectQuery.Append(" 1 = 1 ");

                    var command = new MySqlCommand
                    {
                        CommandText = slectQuery.ToString(),
                        CommandType = CommandType.Text,
                        Connection = staticConnection
                    };

                    try
                    {
                        foreach (var column in columns)
                            command.Parameters.AddWithValue(column.Item2, item[column.Item1]);

                        lastInsertId = Convert.ToInt32(command.ExecuteScalar());
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

                    #endregion

                    if (lastInsertId == 0)
                    {
                        var insertQuery = new StringBuilder();

                        insertQuery.Append("INSERT INTO `" + TableName + "` ");
                        insertQuery.Append("( ");
                        foreach (var column in columns)
                            insertQuery.Append(" " + column.Item2 + ",");

                        insertQuery.Remove(insertQuery.Length - 1, 1);
                        insertQuery.Append(" ) VALUES ( ");

                        foreach (var column in columns)
                            insertQuery.Append(" @" + column.Item2 + " ,");

                        insertQuery.Remove(insertQuery.Length - 1, 1);
                        insertQuery.Append(" ); ");

                        insertQuery.Append("select last_insert_id();");


                        command = new MySqlCommand
                        {
                            CommandText = insertQuery.ToString(),
                            CommandType = CommandType.Text,
                            Connection = staticConnection
                        };

                        try
                        {
                            foreach (var column in columns)
                                command.Parameters.AddWithValue(column.Item2, item[column.Item1]);

                            lastInsertId = Convert.ToInt32(command.ExecuteScalar());
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
                    }

                    if (lastInsertId > 0)
                    {
                        string strCurrentTableName = TableName.Substring(0, TableName.IndexOf("_"));
                        originSourceData.Tables[strCurrentTableName].Rows[RowCount]["_new." + strCurrentTableName + "." + targetTableIdentityColumn] = lastInsertId;
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
