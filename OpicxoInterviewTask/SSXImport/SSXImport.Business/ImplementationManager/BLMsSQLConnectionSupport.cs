using Microsoft.Extensions.Logging;
using SSXImport.Data.ImplementationManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SSXImport.Business.ImplementationManager
{
    /// <summary>
   /// MsSQL Connection support with operations
   /// </summary>
    public class BLMsSQLConnectionSupport
    {
        /// <summary>
        /// Check if credentials are valid or not
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <returns>Returns flag if connection is successful or not</returns>
        public static bool CheckCredentials(string connectionString)
        {
            try
            {
                return DLMsSQLConnectionSupport.CheckCredentials(connectionString);
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Get Database List for given credentials
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>Returns List of All Databases</returns>
        public static DataTable GetAllDatabase(string connectionString)
        {
            try
            {
                return DLMsSQLConnectionSupport.GetAllDatabase(connectionString);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Tables List for given credentials
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>Returns List of All Tables</returns>
        public static DataTable GetAllTables(string connectionString)
        {
            try
            {
                return DLMsSQLConnectionSupport.GetAllTables(connectionString);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Columns List for given credentials
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="Table"></param>
        /// <param name="IncludePrimaryKey"></param>
        /// <returns>Returns List of All columns for given credentials</returns>
        public static DataTable GetAllColumns(string connectionString, string Table, bool IncludePrimaryKey = true)
        {
            try
            {
                return DLMsSQLConnectionSupport.GetAllColumns(connectionString, Table, IncludePrimaryKey);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get GUID Columns List for specified table
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="Table">Table Name</param>
        /// <returns>Returns List of GUID columns</returns>
        public static DataTable GetGUIDColumn(string connectionString, string Table)
        {
            try
            {
                return DLMsSQLConnectionSupport.GetGUIDColumn(connectionString, Table);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Unique Columns List for given credentials
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="Table">Table Name</param>
        /// <returns>Returns List of Unique Columns</returns>
        public static DataTable GetUniqueColumn(string connectionString, string Table)
        {
            try
            {
                return DLMsSQLConnectionSupport.GetUniqueColumn(connectionString, Table);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Identity Columns List for given credentials
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="Table">Table Name</param>
        /// <returns>Returns List of Identity Columns</returns>
        public static DataTable GetIdentityColumn(string connectionString, string Table)
        {
            try
            {
                return DLMsSQLConnectionSupport.GetIdentityColumn(connectionString, Table);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get AUTO INCREMENT for given credentials
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="Table">Table Name</param>
        /// <returns>Returns List of Identity Columns</returns>
        public static DataTable GetAutoIncrementValue(string connectionString, string Table)
        {
            try
            {
                return DLMsSQLConnectionSupport.GetAutoIncrementValue(connectionString, Table);
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
                return DLMsSQLConnectionSupport.ExecuteQuery(connectionString, query);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Execute Scalar Query
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="query">Query to execute</param>
        public static void ExecuteScalerQuery(string connectionString, string query)
        {
            try
            {
                DLMsSQLConnectionSupport.ExecuteScalarQuery(connectionString, query);
            }
            catch
            {
                throw;
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
            string tableName, IEnumerable<Tuple<string, string>> columns, ILogger<T> logger)
        {
            try
            {
                return DLMsSQLConnectionSupport.InsertBulkDataIntoTempTable(connectionString, dataTable, tableName, columns, logger);
            }
            catch
            {
                throw;
            }
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
        public static bool InsertBulkDataIntoTempTableOneToMany<T>(string connectionString, DataTable dataTable, string tableName, IEnumerable<Tuple<string, string>> columns,
            IDictionary<string, string> DependentTableAndTableColumn, ref DataSet originSourceData, 
            string targetTableIdentityColumn, ILogger<T> logger)
        {
            try
            {
                return DLMsSQLConnectionSupport.InsertBulkDataIntoTempTableOneToMany(connectionString, dataTable, tableName, columns,
                    DependentTableAndTableColumn, originSourceData, targetTableIdentityColumn, logger);
            }
            catch
            {
                throw;
            }
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
        public static bool InsertBulkDataIntoTempTableManyToMany<T>(string connectionString, DataTable dataTable, string TableName, IEnumerable<Tuple<string, string, string>> columns, 
            ref DataSet originSourceData, string targetTableIdentityColumn, ILogger<T> logger)
        {
            try
            {
                return DLMsSQLConnectionSupport.InsertBulkDataIntoTempTableManyToMany(connectionString, dataTable, TableName, columns,
                    ref originSourceData, targetTableIdentityColumn, logger);
            }
            catch
            {
                throw;
            }
        }
    }
}
