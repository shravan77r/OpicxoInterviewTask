using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SSXImport.Business;
using SSXImport.Business.ImplementationManager;
using SSXImport.WebAPI.Common;
using SSXImport.WebAPI.Models;
using SSXImport.WebAPI.Models.General;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXImport.WebAPI.Controllers
{
    /// <summary>
    /// Used for Data Transfer related Operations
    /// </summary>
    [Route("api/datatransfer")]
    [ApiController]
    public class DataTransferController : BaseController
    {
        public IBackgroundTaskQueue _queue { get; }

        private readonly ILogger<DataTransferController> _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Initialize IBackgroundTaskQueue, IServiceScopeFactory and ILogger for background queue operations
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="serviceScopeFactory"></param>
        /// <param name="logger"></param>
        public DataTransferController(IBackgroundTaskQueue queue, IServiceScopeFactory serviceScopeFactory, ILogger<DataTransferController> logger)
        {
            _queue = queue;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        /// <summary>
        /// Get List of Data Transfer based of given parameters
        /// </summary>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageFrom"></param>
        /// <param name="keyword"></param>
        /// <returns>Returns List of Data Transfer</returns>
        [HttpGet]
        [Route("list")]
        [ApiVersion("1.0")]
        public virtual Response GetDataTransferList(int sortCol = 0, string sortDir = "desc", int pageSize = int.MaxValue, int pageFrom = 0, string keyword = null)
        {
            _logger.LogInformation("Entered GetDataTransferList");
            _logger.LogDebug("Entered GetDataTransferList with Parameters sortCol: {0},sortDir: {1},pageSize: {2},pageFrom: {3},keyword: {4}",
                sortCol, sortDir, pageSize, pageFrom, keyword);
            try
            {
                var data = BLDataTransferManager.GetDataTransferList(
                    sortCol,
                    sortDir,
                    pageFrom,
                    pageSize,
                    keyword
                    );

                var dt = data.Item2;

                response = GetDataTableResponse(dt);

                response.Count = data.Item1;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited GetDataTransferList with response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetDataTransferList");
            return response;
        }

        /// <summary>
        /// Get Content of Data Transfer Logfile if Data Transfer is failed
        /// </summary>
        /// <param name="dataTransferGuid"></param>
        /// <returns>Return file content in string format</returns>
        [HttpGet]
        [Route("logfile/getcontent")]
        [ApiVersion("1.0")]
        public virtual Response GetLogFileContent(string dataTransferGuid)
        {
            _logger.LogInformation("Entered GetLogFileContent with dataTransferGuid : {0}", dataTransferGuid);
            _logger.LogDebug("Entered GetLogFileContent with dataTransferGuid : {0}", dataTransferGuid);
            try
            {
                var dataTransfer = new BLDataTransfer(dataTransferGuid);
                var logFileContent = string.Empty;
                if (dataTransfer != null)
                {
                    var filePath = Path.Combine(AppCommon.Environment.WebRootPath, "Document", "DataTransfer", dataTransfer.DataTransferId.ToString(), "Logs.log");

                    if (System.IO.File.Exists(filePath))
                    {
                        // Read file using StreamReader. Reads file line by line  
                        using (StreamReader file = new StreamReader(filePath))
                        {
                            int counter = 0;
                            string ln;

                            while ((ln = file.ReadLine()) != null)
                            {
                                logFileContent += ln;
                                logFileContent += Environment.NewLine;
                                counter++;
                            }
                            file.Close();
                        }
                        response = GetSuccessResponse(logFileContent);
                    }
                    else
                    {
                        _logger.LogDebug("File Not found : {0}", filePath);
                        response = GetNoRecordFound();
                    }
                }
                else
                {
                    _logger.LogDebug("Data Transfer not found for GUID : {0}", dataTransferGuid);
                    response = GetNoRecordFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited GetLogFileContent with response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetLogFileContent with dataTransferGuid : {0}", dataTransferGuid);
            return response;
        }

        /// <summary>
        /// Create New Data Transfer using Template Id
        /// </summary>
        /// <param name="dataTransferRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("new")]
        [ApiVersion("1.0")]
        public virtual Response CreateDataTransfer(DataTransferRequest dataTransferRequest)
        {
            _logger.LogInformation("Entered CreateDataTransfer with request : {0}", JsonConvert.SerializeObject(dataTransferRequest));
            _logger.LogDebug("Entered CreateDataTransfer with request : {0}", JsonConvert.SerializeObject(dataTransferRequest));
            try
            {
                if (!ValidateColumnMappingForDataTransfer(dataTransferRequest.TemplateGuid))
                    return GetNoRecordFound("Data Transfer can't be done, Please Map all the table columns");

                _queue.QueueBackgroundWorkItem(async token =>
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        ExecuteDataTransfer(dataTransferRequest.TemplateGuid);
                        await Task.Delay(TimeSpan.FromSeconds(5), token);
                    }
                });

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            _logger.LogDebug("Exited CreateDataTransfer with request : {0}", JsonConvert.SerializeObject(dataTransferRequest));
            _logger.LogInformation("Exited CreateDataTransfer with request : {0}", JsonConvert.SerializeObject(dataTransferRequest));

            return GetSuccessResponse("", 1, "Data Transfer is started, you can check Transfer status for update");
        }

        /// <summary>
        /// Actual Execution of Data Transfer using template GUID
        /// </summary>
        /// <param name="templateGuid">Template GUID for which data transfer needs to be executed</param>
        private void ExecuteDataTransfer(Guid templateGuid)
        {
            _logger.LogDebug("Entered ExecuteDataTransfer for Template GUID {0}", templateGuid);
            try
            {
                var dataTransfer = SaveDataTransfer(templateGuid);

                if (dataTransfer.BLDatatransfer.DataTransferId > 0)
                {
                    var dataTransferLogFilePath = Path.Combine(AppCommon.Environment.WebRootPath,
                            "Document",
                            "DataTransfer",
                            dataTransfer.BLDatatransfer.DataTransferId.ToString());

                    using (_logger.BeginScope(new[] { new KeyValuePair<string, object>("DataTransferLogsDir", dataTransferLogFilePath) }))
                    {
                        _logger.LogDebug("Data Transfer Created with DataTransferId : {0}", dataTransfer.BLDatatransfer.DataTransferId.ToString());
                        dataTransfer = GetConnectionStatus(dataTransfer);
                        if (dataTransfer.IsValidConnection)
                        {
                            DataSet originSourceData = new DataSet();

                            bool? IsDataReadSuceess = null;

                            #region One To One - Normal Case

                            if (dataTransfer.TemplateType == 1)
                            {
                                IsDataReadSuceess = PerformOneToOneDataTransfer(dataTransfer, originSourceData, IsDataReadSuceess);
                            }

                            #endregion

                            #region One To Many

                            else if (dataTransfer.TemplateType == 2)
                            {
                                PerformOneToManyDataTransfer(dataTransfer, ref originSourceData, ref IsDataReadSuceess);
                            }

                            #endregion

                            #region Many To Many

                            else if (dataTransfer.TemplateType == 3)
                            {
                                PerformManyToManyDataTransfer(dataTransfer, ref originSourceData, ref IsDataReadSuceess);
                            }

                            #endregion

                        }

                        #region Update Data Transfer Status

                        // Update Data Transfer Status
                        if (dataTransfer.DataTransferStatus > 1)
                        {
                            using (var bLDataTransfer = new BLDataTransfer(dataTransfer.BLDatatransfer.DataTransferId.GetValueOrDefault(0)))
                            {
                                bLDataTransfer.TransferStatus = dataTransfer.DataTransferStatus;
                                bLDataTransfer.UpdatedBy = 1;
                                bLDataTransfer.UpdatedDate = GetDateTime();

                                bLDataTransfer.Update();
                            }
                            _logger.LogDebug("Data Transfer Status {0} Updated in database", dataTransfer.DataTransferStatus);
                        }

                        #endregion
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogDebug("Exited ExecuteDataTransfer for Template GUID {0}", templateGuid);
        }

        private void PerformManyToManyDataTransfer(DataTransfer dataTransfer, ref DataSet originSourceData, ref bool? IsDataReadSuceess)
        {
            _logger.LogDebug("Data Transfer Type : Many to Many");

            #region Read Data From origin Source

            var bLDataTransferTableDetails = dataTransfer.BLDataTransferTableDetails
                                   .OrderBy(o => o.ExecutionOrder);

            foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
            {
                var columns = dataTransfer.BLDataTransferColumnDetails
                    .Where(o => o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId))
                    .Select(o => new
                    {
                        SourceColumn = o.SourceColumn,
                        IsUniqueColumn = o.IsUniqueColumn.GetValueOrDefault(false),
                        SourceDependentColumn = o.SourceDependentColumn,
                    });

                var filters = dataTransfer.BLDataTransferFilterDetails
                    .Where(o => o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId)
                        && o.FilterOperator > 0)
                    .Select(o => new
                    {
                        o.FilterColumn,
                        FilterOperator = o.FilterOperator.GetValueOrDefault(0),
                        o.FilterValue
                    });

                try
                {
                    if (columns.Count() > 0)
                    {
                        DataTable dataTable = null;
                        string TargetIdentityColumn = string.Empty;
                        var hashColumnList = new List<string>();

                        var query = new StringBuilder();

                        #region MSSQL

                        if (dataTransfer.OriginSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                        {
                            _logger.LogDebug("Data Read Started for MsSQL Origin Source");

                            // Origin Source current Table Identity Column with values treated as _old.OriginTableName.PrimaryColumn 
                            var SourceIdentityColumn = BLMsSQLConnectionSupport.GetIdentityColumn(dataTransfer.OriginSourceConnection.ConnectionString,
                                    bLDataTransferTableDetail.SourceTable).Rows[0][0];

                            if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                            {
                                var TargetIdentityColumnDetail = BLMsSQLConnectionSupport.GetIdentityColumn(dataTransfer.TargetSourceConnection.ConnectionString,
                                    bLDataTransferTableDetail.TargetTable).Rows[0][0];
                                TargetIdentityColumn = TargetIdentityColumnDetail != null ? TargetIdentityColumnDetail.ToString() : string.Empty;
                            }
                            else if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                            {
                                var TargetIdentityColumnDetail = BLMySQLConnectionSupport.GetIdentityColumn(dataTransfer.TargetSourceConnection.ConnectionString,
                                    bLDataTransferTableDetail.TargetTable).Rows[0][0];
                                TargetIdentityColumn = TargetIdentityColumnDetail != null ? TargetIdentityColumnDetail.ToString() : string.Empty;
                            }

                            query.Append("SELECT ");

                            // Origin Source Table primary column with origin value
                            query.Append(SourceIdentityColumn.ToString() + " AS '_old." + bLDataTransferTableDetail.TargetTable + "." + TargetIdentityColumn + "', ");

                            // All Columns selected by User in UI at the time of mapping
                            foreach (var column in columns)
                            {
                                // Actual Source Column except dependent columns as dependent column is present in SourceDependentColumn
                                query.Append(" " + column.SourceColumn + " AS " + column.SourceColumn + ",");

                                // Source Dependent Columns
                                if (column.SourceDependentColumn.StartsWith("#2"))
                                {
                                    hashColumnList.Add(bLDataTransferTableDetail.TargetTable
                                        + "."
                                        + column.SourceDependentColumn.Replace("#2", ""));

                                    query.Append(" "
                                        + column.SourceColumn
                                        + " AS '_old."
                                        + bLDataTransferTableDetail.TargetTable
                                        + "." + column.SourceDependentColumn.Replace("#2", "") + "',");
                                }
                            }

                            query.Remove(query.Length - 1, 1);
                            query.Append(" FROM");

                            query.Append(" [" + bLDataTransferTableDetail.SourceTable + "] ");

                            if (filters.Count() > 0)
                            {
                                query.Append(" WHERE");
                                foreach (var filter in filters)
                                {
                                    query.Append(" " + GetFilterdColumn(
                                        "[" + filter.FilterColumn + "]",
                                        filter.FilterOperator,
                                        filter.FilterValue));
                                    query.Append(" AND");
                                }
                                query.Remove(query.Length - 4, 4);
                            }

                            _logger.LogDebug("Data Read for MsSQL Origin Source Query : {0}", query.ToString());

                            var data = BLMsSQLConnectionSupport.ExecuteQuery(dataTransfer.OriginSourceConnection.ConnectionString
                                , query.ToString());

                            dataTable = data.Copy();

                            _logger.LogDebug("Data Read for MsSQL Response : {0}", JsonConvert.SerializeObject(dataTable));
                            _logger.LogDebug("Data Read Ended for MsSQL Origin Source");

                            data.Dispose();

                            // Target Source current Table primary key without value, Value will be calculated at run time and updated in appropriate tables
                            var cuttentTableNewIdentityColum = dataTable.Columns.Add("_new."
                                    + bLDataTransferTableDetail.TargetTable
                                    + "."
                                    + TargetIdentityColumn, typeof(Int32));
                            cuttentTableNewIdentityColum.AllowDBNull = true;
                        }

                        #endregion

                        #region MySQL

                        else if (dataTransfer.OriginSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                        {
                            _logger.LogDebug("Data Read Started for MySQL Origin Source");

                            // Origin Source current Table Identity Column with values treated as _old.OriginTableName.PrimaryColumn 
                            var SourceIdentityColumn = BLMySQLConnectionSupport.GetIdentityColumn(dataTransfer.OriginSourceConnection.ConnectionString, bLDataTransferTableDetail.SourceTable).Rows[0][0];

                            if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                            {
                                var TargetIdentityColumnDetail = BLMySQLConnectionSupport.GetIdentityColumn(dataTransfer.TargetSourceConnection.ConnectionString, bLDataTransferTableDetail.TargetTable).Rows[0][0];
                                TargetIdentityColumn = TargetIdentityColumnDetail != null ? TargetIdentityColumnDetail.ToString() : string.Empty;
                            }
                            else
                            {
                                var TargetIdentityColumnDetail = BLMsSQLConnectionSupport.GetIdentityColumn(dataTransfer.TargetSourceConnection.ConnectionString, bLDataTransferTableDetail.TargetTable).Rows[0][0];
                                TargetIdentityColumn = TargetIdentityColumnDetail != null ? TargetIdentityColumnDetail.ToString() : string.Empty;
                            }


                            query.Append("SELECT ");
                            // Origin Source Table primary column with origin value
                            query.Append(SourceIdentityColumn.ToString() + " AS `_old." + bLDataTransferTableDetail.TargetTable + "." + TargetIdentityColumn + "`, ");

                            // All Columns selected by User in UI at the time of mapping
                            foreach (var column in columns)
                            {
                                // Actual Source Column except dependent columns as dependent column is present in SourceDependentColumn
                                query.Append(" " + column.SourceColumn + " AS `" + column.SourceColumn + "`,");

                                // Source Dependent Columns
                                if (column.SourceDependentColumn.StartsWith("#2"))
                                {
                                    hashColumnList.Add(bLDataTransferTableDetail.TargetTable
                                        + "."
                                        + column.SourceDependentColumn.Replace("#2", ""));
                                    query.Append(" "
                                        + column.SourceColumn
                                        + " AS `_old."
                                        + bLDataTransferTableDetail.TargetTable
                                        + "."
                                        + column.SourceDependentColumn.Replace("#2", "") + "`,");
                                }
                            }

                            query.Remove(query.Length - 1, 1);
                            query.Append(" FROM");
                            query.Append(" `" + bLDataTransferTableDetail.SourceTable + "` ");

                            if (filters.Count() > 0)
                            {
                                query.Append(" WHERE");
                                foreach (var filter in filters)
                                {
                                    query.Append(" " + GetFilterdColumn(
                                        "`" + filter.FilterColumn + "`",
                                        filter.FilterOperator,
                                        filter.FilterValue));
                                    query.Append(" AND");
                                }
                                query.Remove(query.Length - 4, 4);
                            }

                            _logger.LogDebug("Data Read for MySQL Origin Source Query : {0}", query.ToString());

                            var data = BLMySQLConnectionSupport.ExecuteQuery(dataTransfer.OriginSourceConnection.ConnectionString
                                , query.ToString());

                            dataTable = data.Copy();

                            _logger.LogDebug("Data Read for MySQL Response : {0}", JsonConvert.SerializeObject(dataTable));
                            _logger.LogDebug("Data Read Ended for MySQL Origin Source");
                            data.Dispose();

                            // Target Source current Table primary key without value, Value will be calculated at run time and updated in appropriate tables
                            var cuttentTableNewIdentityColum = dataTable.Columns.Add("_new."
                                + bLDataTransferTableDetail.TargetTable
                                + "."
                                + TargetIdentityColumn, typeof(Int32));
                            cuttentTableNewIdentityColum.AllowDBNull = true;
                        }

                        #endregion

                        #region Excel

                        else if (dataTransfer.OriginSourceConnection.DataSourceId.Equals(AppConstant.DataSource_Excel))
                        {
                            _logger.LogDebug("Many to Many Transfer functionality is not implemented for Excel Origin Data Source");
                            throw new Exception("Many to Many Transfer functionality is not implemented for Excel Origin Data Source");
                        }

                        #endregion

                        _logger.LogDebug("Dependent Columns List : {0}", JsonConvert.SerializeObject(hashColumnList));
                        // Add Dependent Columns in Original Data Source (Data Table) with nullable Type
                        foreach (string hasColumnItem in hashColumnList)
                        {
                            DataColumn preTableColumn = dataTable.Columns.Add(hasColumnItem, typeof(Int32));
                            preTableColumn.AllowDBNull = true;
                        }

                        dataTable.TableName = bLDataTransferTableDetail.TargetTable;

                        originSourceData.Tables.Add(dataTable);

                        IsDataReadSuceess = true;
                    }
                }
                catch (Exception e)
                {
                    dataTransfer.ErrorMessage = e.ToString();
                    _logger.LogError(e, e.Message);
                }
            }

            if (IsDataReadSuceess.Equals(true))
                _logger.LogDebug("Data Read Successful");
            else
                _logger.LogDebug("Data Read Unsuccessful");

            #endregion

            #region Execute Transfer

            if (IsDataReadSuceess.GetValueOrDefault(false))
            {
                var IsTempTableDataInsertSuccess = true;

                #region Create Temporary Tables and Dump Data

                _logger.LogDebug("Started Create Temporary Table");

                foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
                {
                    var columns = dataTransfer.BLDataTransferColumnDetails
                        .Where(o => o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId))
                        .Select(o =>
                            Tuple.Create(
                                o.SourceColumn.Replace("Column_", "F").Replace("#2", ""),
                                o.TargetColumn,
                                o.SourceDependentColumn));

                    #region MsSQL

                    if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                    {
                        try
                        {
                            //Create new Temp table with same Structure as Origin table
                            var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;
                            var createTempTableQuery = "DROP TABLE IF EXISTS " + tempTableName +
                               " ;" +
                               " SELECT * INTO" +
                               " " + tempTableName +
                               " FROM " +
                               " " + bLDataTransferTableDetail.TargetTable + " ; ";

                            _logger.LogDebug("Create Temporary Table MsSQL Query : {0}", createTempTableQuery);

                            BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                createTempTableQuery);

                            // Add GUID constraints into temp table
                            var guidColumns = BLMsSQLConnectionSupport.GetGUIDColumn(dataTransfer.TargetSourceConnection.ConnectionString, bLDataTransferTableDetail.TargetTable);
                            _logger.LogDebug("GUID Columns Fetched from Database : {0}", JsonConvert.SerializeObject(guidColumns));
                            if (guidColumns != null && guidColumns.Rows.Count > 0)
                            {
                                createTempTableQuery = string.Empty;
                                foreach (DataRow guidColumn in guidColumns.Rows)
                                {
                                    var constraintName = "df_" + guidColumn[0] + "_" + bLDataTransferTableDetail.DataTransferTableDetailId;

                                    createTempTableQuery += "IF EXISTS(SELECT * " +
                                        "FROM sys.foreign_keys " +
                                        "WHERE object_id = OBJECT_ID(N'" + constraintName + "') " +
                                        "AND parent_object_id = OBJECT_ID(N'dbo." + tempTableName + "') " +
                                        ") " +
                                        "ALTER TABLE[dbo].[" + tempTableName + "] DROP CONSTRAINT[" + constraintName + "]; ";

                                    createTempTableQuery += " ALTER TABLE " + tempTableName +
                                    " ADD CONSTRAINT " + constraintName +
                                    " DEFAULT NEWID() FOR " + guidColumn[0] + " ;";
                                }
                                _logger.LogDebug("Create GUID Columns MsSQL Query : {0}", createTempTableQuery);

                                BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                    createTempTableQuery);
                            }

                            // Add UNIQUE constraints into temp table
                            var uniqueColumns = BLMsSQLConnectionSupport.GetUniqueColumn(dataTransfer.TargetSourceConnection.ConnectionString, bLDataTransferTableDetail.TargetTable);
                            _logger.LogDebug("Unique Columns Fetched from Database : {0}", JsonConvert.SerializeObject(guidColumns));
                            if (uniqueColumns != null && uniqueColumns.Rows.Count > 0)
                            {
                                createTempTableQuery = string.Empty;
                                foreach (DataRow uniqueColumn in uniqueColumns.Rows)
                                {
                                    var constraintName = "ak_" + uniqueColumn[0] + "_" + bLDataTransferTableDetail.DataTransferTableDetailId;

                                    createTempTableQuery += "IF EXISTS(SELECT * " +
                                        "FROM sys.foreign_keys " +
                                        "WHERE object_id = OBJECT_ID(N'" + constraintName + "') " +
                                        "AND parent_object_id = OBJECT_ID(N'dbo." + tempTableName + "') " +
                                        ") " +
                                        "ALTER TABLE[dbo].[" + tempTableName + "] DROP CONSTRAINT[" + constraintName + "]; ";

                                    createTempTableQuery += " ALTER TABLE " + tempTableName +
                                    " ADD CONSTRAINT " + constraintName +
                                    " UNIQUE ( " + uniqueColumn[0] + " ) ;";
                                }

                                _logger.LogDebug("Create Unique Columns MsSQL Query : {0}", createTempTableQuery);

                                BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, createTempTableQuery);
                            }

                            //Get Identity Column Name of Table
                            var drIdentityColumn = BLMsSQLConnectionSupport.GetIdentityColumn(dataTransfer.TargetSourceConnection.ConnectionString,
                                bLDataTransferTableDetail.TargetTable).Rows[0][0];
                            _logger.LogDebug("Identity Column Name: {0}", JsonConvert.SerializeObject(drIdentityColumn));

                            // Get Last Auto Increment Value
                            var autoIncrementValue = Convert.ToInt32(BLMsSQLConnectionSupport.GetAutoIncrementValue(dataTransfer.TargetSourceConnection.ConnectionString,
                                bLDataTransferTableDetail.TargetTable).Rows[0][0]);
                            _logger.LogDebug("Auto Increment Value: {0}", autoIncrementValue);

                            // Set Temp Table Auto Increment Value to Actual Table Auto Increment Value
                            // Will be useful in case of any records are deleted in between of Actual Table
                            if (autoIncrementValue > 0)
                            {
                                var setAutoIncrementValueQuery = "DBCC CHECKIDENT(" + tempTableName + ", RESEED, " + autoIncrementValue + ")";
                                _logger.LogDebug("Set Auto Increment Value query : {0}", setAutoIncrementValueQuery);
                                BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                    setAutoIncrementValueQuery);
                            }

                            // Current Target Table name with identity column e.g. : Country.CountryId
                            string strCurrentTargetTableName = bLDataTransferTableDetail.TargetTable + "." + drIdentityColumn;
                            string strDependentTableName = string.Empty;

                            // <Table, ColumnName>
                            // Dependent Table and Column name
                            var DependentTableAndTableColumn = new Dictionary<string, string>();

                            // Iterate over all the Origin source to check dependency column of this table
                            foreach (DataTable TableItem in originSourceData.Tables)
                            {
                                // TODO : Change in Entire Code
                                // var CurrentTableColumns = TableItem.Columns;
                                strDependentTableName = TableItem.Columns.Contains(TableItem.TableName + "." + strCurrentTargetTableName)
                                    ? TableItem.TableName : "";

                                // If current target table identity column found in some other table then add it into list
                                if (!string.IsNullOrEmpty(strDependentTableName))
                                    DependentTableAndTableColumn.Add(TableItem.TableName, TableItem.TableName + "." + strCurrentTargetTableName);
                            }

                            _logger.LogDebug("Dependent Table and Columns to Update Values : {0}", JsonConvert.SerializeObject(DependentTableAndTableColumn));

                            //Copy all the Source rows into temp table for duplication and Validation purpose and set the primary key value in _new.TableName.ColumnName back in origin source
                            var isOperationSuccess = BLMsSQLConnectionSupport.InsertBulkDataIntoTempTableManyToMany(
                                    dataTransfer.TargetSourceConnection.ConnectionString,
                                    originSourceData.Tables[bLDataTransferTableDetail.TargetTable],
                                    tempTableName,
                                    columns,
                                    ref originSourceData,
                                    drIdentityColumn.ToString(),
                                    _logger
                                );

                            IsTempTableDataInsertSuccess = isOperationSuccess;
                            // Update Reference column in all tables 
                            string currentTableOldColumnName = "_old." + bLDataTransferTableDetail.TargetTable + "." + drIdentityColumn;
                            string currentTableNewColumnName = "_new." + bLDataTransferTableDetail.TargetTable + "." + drIdentityColumn;

                            // Iterate over current table from origin source which contains both origin source primary key
                            // and target source primary key value
                            foreach (DataRow dtRow in originSourceData.Tables[bLDataTransferTableDetail.TargetTable].Rows)
                            {
                                int intDependentOldId = Convert.ToInt32(dtRow[currentTableOldColumnName]);
                                int intDependentNewId = Convert.ToInt32(dtRow[currentTableNewColumnName]);

                                // TODO : Add Proper Comments in below loop
                                // Iterate over each table of Data Set
                                foreach (DataTable tableItem in originSourceData.Tables)
                                {
                                    // Check if current table dependency is contained by this table
                                    if (DependentTableAndTableColumn.ContainsKey(tableItem.TableName))
                                    {
                                        string strNewColumnName = DependentTableAndTableColumn[tableItem.TableName]
                                            .Replace(tableItem.TableName.ToString() + ".", "#2");

                                        var intDataTransferTableDetailId = dataTransfer.BLDataTransferTableDetails
                                            .Where(x =>
                                                x.TargetTable == tableItem.TableName)
                                            .FirstOrDefault()
                                            .DataTransferTableDetailId.Value;

                                        var originalColName = dataTransfer.BLDataTransferColumnDetails
                                            .Where(x =>
                                                x.SourceDependentColumn == strNewColumnName &&
                                                x.DataTransferTableDetailId == intDataTransferTableDetailId)
                                            .FirstOrDefault().SourceColumn;

                                        foreach (DataRow dtItem in originSourceData.Tables[tableItem.TableName].Rows)
                                        {
                                            string oldDependentColumnName = "_old."
                                                + tableItem.TableName
                                                + "."
                                                + bLDataTransferTableDetail.TargetTable
                                                + "." + drIdentityColumn;

                                            if (Convert.ToInt32(dtItem[oldDependentColumnName]) == intDependentOldId)
                                            {
                                                dtItem[originalColName] = intDependentNewId;
                                            }
                                        }
                                    }
                                }

                            }

                            //Delete all the existing rows from temp table
                            if (autoIncrementValue > 0)
                            {
                                var deleteExistingRowsFromTempTableQuery = "DELETE FROM [" + tempTableName + "] WHERE [" + Convert.ToString(drIdentityColumn) + "] < " + autoIncrementValue;
                                _logger.LogDebug("Delete all the existing rows from Temp table query : {0}", deleteExistingRowsFromTempTableQuery);
                                BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                    deleteExistingRowsFromTempTableQuery);
                            }
                            _logger.LogDebug("Create Temp Table MsSQL Success");

                        }
                        catch (Exception e)
                        {
                            IsTempTableDataInsertSuccess = false;
                            _logger.LogDebug("Create Temp Table MsSQL Failed with error : {0}", e.ToString());
                            _logger.LogError(e, e.Message);
                        }
                    }

                    #endregion

                    #region MySQL

                    else if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                    {
                        try
                        {

                            //Create new Temp table with same Structure as Origin table
                            var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;
                            var createTempTableQuery = "DROP TABLE IF EXISTS " + tempTableName +
                               " ;" +
                               " CREATE TABLE " +
                               " " + tempTableName +
                               " LIKE " +
                               " " + bLDataTransferTableDetail.TargetTable + " ; " +
                               "INSERT INTO " + tempTableName + " SELECT * FROM " + bLDataTransferTableDetail.TargetTable + "; ";

                            _logger.LogDebug("Create Temporary Table MySQL Query : {0}", createTempTableQuery);

                            BLMySQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, createTempTableQuery);

                            //Get Identity Column Name of Table
                            var drIdentityColumn = BLMySQLConnectionSupport.GetIdentityColumn(dataTransfer.TargetSourceConnection.ConnectionString,
                                bLDataTransferTableDetail.TargetTable).Rows[0][0];
                            _logger.LogDebug("Identity Column Name: {0}", JsonConvert.SerializeObject(drIdentityColumn));

                            // Get Last Auto Increment Value
                            var autoIncrementValue = Convert.ToInt32(BLMySQLConnectionSupport.GetAutoIncrementValue(dataTransfer.TargetSourceConnection.ConnectionString,
                                bLDataTransferTableDetail.TargetTable).Rows[0][0]);
                            _logger.LogDebug("Auto Increment Value: {0}", autoIncrementValue);

                            // Set Temp Table Auto Increment Value to Actual Table Auto Increment Value
                            // Will be useful in case of any records are deleted in between of Actual Table
                            if (autoIncrementValue > 0)
                            {
                                var setAutoIncrementValueQuery = "ALTER TABLE `" + tempTableName + "` AUTO_INCREMENT = " + autoIncrementValue;
                                _logger.LogDebug("Set Auto Increment Value query : {0}", setAutoIncrementValueQuery);
                                BLMySQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                    setAutoIncrementValueQuery);
                            }

                            // Current Target Table name with identity column e.g. : Country.CountryId
                            string strCurrentTargetTableName = bLDataTransferTableDetail.TargetTable + "." + drIdentityColumn;
                            string strDependentTableName = string.Empty;

                            // <Table, ColumnName>
                            // Dependent Table and Column name
                            var DependentTableAndTableColumn = new Dictionary<string, string>();

                            // Iterate over all the Origin source to check dependency column of this table
                            foreach (DataTable TableItem in originSourceData.Tables)
                            {
                                // TODO : Change in Entire Code
                                // var CurrentTableColumns = TableItem.Columns;
                                strDependentTableName = TableItem.Columns.Contains(TableItem.TableName + "." + strCurrentTargetTableName)
                                    ? TableItem.TableName : "";

                                // If current target table identity column found in some other table then add it into list
                                if (!string.IsNullOrEmpty(strDependentTableName))
                                    DependentTableAndTableColumn.Add(TableItem.TableName, TableItem.TableName + "." + strCurrentTargetTableName);

                            }

                            //Copy all the Source rows into temp table for duplication and Validation purpose and set the primary key value in _new.TableName.ColumnName back in origin source
                            var isOperationSuccess = BLMySQLConnectionSupport.InsertBulkDataIntoTempTableManyToMany(
                                    dataTransfer.TargetSourceConnection.ConnectionString,
                                    originSourceData.Tables[bLDataTransferTableDetail.TargetTable],
                                    tempTableName,
                                    columns,
                                    DependentTableAndTableColumn,
                                    ref originSourceData,
                                    drIdentityColumn.ToString(),
                                    _logger
                                );

                            IsTempTableDataInsertSuccess = isOperationSuccess;
                            //Update Reference column in all tables 

                            string currentTableOldColumnName = "_old." + bLDataTransferTableDetail.TargetTable + "." + drIdentityColumn;
                            string currentTableNewColumnName = "_new." + bLDataTransferTableDetail.TargetTable + "." + drIdentityColumn;

                            // Iterate over current table from origin source which contains both origin source primary key
                            // and target source primary key value
                            foreach (DataRow dtRow in originSourceData.Tables[bLDataTransferTableDetail.TargetTable].Rows)
                            {
                                int intDependentOldId = Convert.ToInt32(dtRow[currentTableOldColumnName]);
                                int intDependentNewId = Convert.ToInt32(dtRow[currentTableNewColumnName]);

                                // TODO : Add Proper Comments in below loop
                                // Iterate over each table of Data Source
                                foreach (DataTable tableItem in originSourceData.Tables)
                                {

                                    if (DependentTableAndTableColumn.ContainsKey(tableItem.TableName))
                                    {
                                        string strNewColumnName = DependentTableAndTableColumn[tableItem.TableName].Replace(tableItem.TableName.ToString() + ".", "#2");

                                        var intDataTransferTableDetailId = dataTransfer.BLDataTransferTableDetails
                                            .Where(x =>
                                                x.TargetTable == tableItem.TableName)
                                            .FirstOrDefault()
                                            .DataTransferTableDetailId.Value;

                                        var originalColName = dataTransfer.BLDataTransferColumnDetails
                                            .Where(x =>
                                                x.SourceDependentColumn == strNewColumnName &&
                                                x.DataTransferTableDetailId == intDataTransferTableDetailId)
                                            .FirstOrDefault().SourceColumn;

                                        foreach (DataRow dtItem in originSourceData.Tables[tableItem.TableName].Rows)
                                        {
                                            string oldDependentColumnName = "_old." + tableItem.TableName + "." + bLDataTransferTableDetail.TargetTable + "." + drIdentityColumn;
                                            if (Convert.ToInt32(dtItem[oldDependentColumnName]) == intDependentOldId)
                                            {
                                                dtItem[originalColName] = intDependentNewId;
                                            }
                                        }
                                    }
                                }

                            }

                            //Delete all the existing rows from temp table
                            if (autoIncrementValue > 0)
                            {
                                var deleteExistingRowsFromTempTableQuery = "DELETE FROM `" + tempTableName + "` WHERE `" + Convert.ToString(drIdentityColumn) + "` < " + autoIncrementValue;
                                _logger.LogDebug("Delete all the existing rows from Temp table query : {0}", deleteExistingRowsFromTempTableQuery);
                                BLMySQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                    deleteExistingRowsFromTempTableQuery);
                            }
                            _logger.LogDebug("Create Temp Table MySQL Success");

                        }
                        catch (Exception e)
                        {
                            IsTempTableDataInsertSuccess = false;
                            _logger.LogDebug("Create Temp Table MySQL Failed with error : {0}", e.ToString());
                            _logger.LogError(e, e.Message);
                        }
                    }

                    #endregion
                }

                _logger.LogDebug("Ended Create Temporary Table");

                #endregion

                #region Dump Data From Temporary Table to Actual Table

                if (IsTempTableDataInsertSuccess)
                {
                    _logger.LogDebug("Started Dump Data From Temporary Table to Actual Table");

                    #region MsSQL

                    if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                    {

                        try
                        {
                            var tables = dataTransfer.BLDataTransferTableDetails
                                .Select(o => Tuple.Create(o.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId, o.TargetTable));

                            var actualTransferQuery = new StringBuilder();

                            foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
                            {
                                var columns = dataTransfer.BLDataTransferColumnDetails
                                    .Where(o => o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId))
                                    .Select(o => o.TargetColumn);

                                var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;

                                actualTransferQuery.Append("INSERT INTO " + bLDataTransferTableDetail.TargetTable + " (");

                                foreach (var column in columns)
                                    actualTransferQuery.Append("[" + column + "] ,");
                                actualTransferQuery.Remove(actualTransferQuery.Length - 1, 1);
                                actualTransferQuery.Append(") ");

                                actualTransferQuery.Append("SELECT ");
                                foreach (var column in columns)
                                    actualTransferQuery.Append("[" + column + "] ,");
                                actualTransferQuery.Remove(actualTransferQuery.Length - 1, 1);
                                actualTransferQuery.Append(" FROM " + tempTableName + "; ");

                            }

                            _logger.LogDebug("Dump data from MsSQL Temp Table to Actual Table query : {0}", actualTransferQuery.ToString());

                            BLMsSQLConnectionSupport.ExecuteScalerQuery(
                                           dataTransfer.TargetSourceConnection.ConnectionString,
                                           actualTransferQuery.ToString()
                                       );

                            dataTransfer.DataTransferStatus = 2;

                            _logger.LogDebug("Dump data from MsSQL Temp Table to Actual Table Success");

                        }
                        catch (Exception e)
                        {
                            IsTempTableDataInsertSuccess = false;
                            dataTransfer.DataTransferStatus = 3;
                            _logger.LogDebug("Dump data from MsSQL Temp Table to Actual Table Failed with exception : {0}", e.ToString());
                            _logger.LogError("Dump data from MsSQL Temp Table to Actual Table Failed with exception : {0}", e.ToString());
                        }
                    }

                    #endregion

                    #region MySQL

                    if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                    {
                        try
                        {
                            var tables = dataTransfer.BLDataTransferTableDetails
                                .Select(o => Tuple.Create(o.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId, o.TargetTable));

                            var actualTransferQuery = new StringBuilder();

                            foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
                            {
                                var columns = dataTransfer.BLDataTransferColumnDetails
                                    .Where(o => o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId))
                                    .Select(o => o.TargetColumn);

                                var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;

                                actualTransferQuery.Append("INSERT INTO " + bLDataTransferTableDetail.TargetTable + " (");

                                foreach (var column in columns)
                                    actualTransferQuery.Append("`" + column + "` ,");
                                actualTransferQuery.Remove(actualTransferQuery.Length - 1, 1);
                                actualTransferQuery.Append(") ");

                                actualTransferQuery.Append("SELECT ");
                                foreach (var column in columns)
                                    actualTransferQuery.Append("`" + column + "` ,");
                                actualTransferQuery.Remove(actualTransferQuery.Length - 1, 1);
                                actualTransferQuery.Append(" FROM " + tempTableName + "; ");

                            }
                            _logger.LogDebug("Dump data from MySQL Temp Table to Actual Table query : {0}", actualTransferQuery.ToString());

                            BLMySQLConnectionSupport.ExecuteScalerQuery(
                                           dataTransfer.TargetSourceConnection.ConnectionString,
                                           actualTransferQuery.ToString()
                                       );

                            dataTransfer.DataTransferStatus = 2;

                            _logger.LogDebug("Dump data from MySQL Temp Table to Actual Table Success");

                        }
                        catch (Exception e)
                        {
                            IsTempTableDataInsertSuccess = false;
                            dataTransfer.DataTransferStatus = 3;
                            _logger.LogDebug("Dump data from MySQL Temp Table to Actual Table Failed with exception : {0}", e.ToString());
                            _logger.LogError("Dump data from MySQL Temp Table to Actual Table Failed with exception : {0}", e.ToString());
                        }

                    }

                    #endregion

                    _logger.LogDebug("Ended Dump Data From Temporary Table to Actual Table");

                }

                #endregion

                #region Delete Temporary Table

                _logger.LogDebug("Started Delete Temporary Table");

                var deleteTempTableQuery = new StringBuilder();
                foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
                {
                    if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                    {

                        var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;

                        var guidColumns = BLMsSQLConnectionSupport.GetGUIDColumn(dataTransfer.TargetSourceConnection.ConnectionString, bLDataTransferTableDetail.TargetTable);
                        if (guidColumns != null && guidColumns.Rows.Count > 0)
                            foreach (DataRow guidColumn in guidColumns.Rows)
                            {
                                var constraintName = "df_" + guidColumn[0] + "_" + bLDataTransferTableDetail.DataTransferTableDetailId;

                                deleteTempTableQuery.Append("IF EXISTS ( SELECT * " +
                                    "FROM sys.foreign_keys " +
                                    "WHERE object_id = OBJECT_ID(N'" + constraintName + "') " +
                                    "AND parent_object_id = OBJECT_ID(N'dbo." + tempTableName + "') " +
                                    ") " +
                                    "ALTER TABLE[dbo].[" + tempTableName + "] DROP CONSTRAINT[" + constraintName + "]; ");
                            }

                        deleteTempTableQuery.Append("DROP TABLE IF EXISTS " + tempTableName + " ;");
                    }
                    else if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                    {
                        var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;
                        deleteTempTableQuery.Append("DROP TABLE IF EXISTS " + tempTableName + " ;");
                    }
                }
                _logger.LogDebug("Delete Temporary Table query : {0}", deleteTempTableQuery.ToString());

                try
                {
                    if (deleteTempTableQuery.Length > 0)
                        if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                            BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, deleteTempTableQuery.ToString());
                        else if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                            BLMySQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, deleteTempTableQuery.ToString());
                    _logger.LogDebug("Delete Temporary Table Success");
                }
                catch (Exception e)
                {
                    _logger.LogDebug("Delete Temporary Failed with exception : {0}", e.ToString());
                    _logger.LogError("Delete Temporary Failed with exception : {0}", e.ToString());
                }

                #endregion
            }

            #endregion
        }

        private void PerformOneToManyDataTransfer(DataTransfer dataTransfer, ref DataSet originSourceData, ref bool? IsDataReadSuceess)
        {
            _logger.LogDebug("Data Transfer Type : One to Many");

            #region Read Data From origin Source

            var bLDataTransferTableDetails = dataTransfer.BLDataTransferTableDetails
                                   .OrderBy(o => o.ExecutionOrder);

            foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
            {
                var columns = dataTransfer.BLDataTransferColumnDetails
                    .Where(o => o.DataTransferId.Equals(dataTransfer.BLDatatransfer.DataTransferId)
                        && o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId))
                    .Select(o => new
                    {
                        SourceColumn = o.SourceColumn,
                        IsUniqueColumn = o.IsUniqueColumn.GetValueOrDefault(false)
                    });

                var filters = dataTransfer.BLDataTransferFilterDetails
                    .Where(o => o.DataTransferId.Equals(dataTransfer.BLDatatransfer.DataTransferId)
                        && o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId)
                        && o.FilterOperator > 0)
                    .Select(o => new
                    {
                        o.FilterColumn,
                        FilterOperator = o.FilterOperator.GetValueOrDefault(0),
                        o.FilterValue
                    });

                try
                {
                    if (columns.Count() > 0)
                    {
                        DataTable dataTable = null;
                        var query = new StringBuilder();
                        var dependentColumnList = new List<string>();

                        #region MSSQL

                        if (dataTransfer.OriginSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                        {
                            _logger.LogDebug("Data Read Started for MsSQL Origin Source");

                            query.Append("SELECT ");

                            foreach (var column in columns)
                            {
                                // Dependent Column Starts with #2
                                // For e.g #2County.CountryId
                                // Select Only columns of actual table and not dependent columns
                                if (!column.SourceColumn.StartsWith("#2"))
                                {
                                    query.Append(" " + column.SourceColumn + " AS " + column.SourceColumn + ",");
                                }
                                else
                                {
                                    // Add Dependent Column of Other Target table into list
                                    // This Columns will be filled up at run time
                                    dependentColumnList.Add(column.SourceColumn.Replace("#2", ""));
                                }
                            }

                            query.Remove(query.Length - 1, 1);
                            query.Append(" FROM");
                            query.Append(" [" + bLDataTransferTableDetail.SourceTable + "] ");

                            if (filters.Count() > 0)
                            {
                                query.Append(" WHERE");
                                foreach (var filter in filters)
                                {
                                    query.Append(" " + GetFilterdColumn(
                                        "[" + filter.FilterColumn + "]",
                                        filter.FilterOperator,
                                        filter.FilterValue));
                                    query.Append(" AND");
                                }
                                query.Remove(query.Length - 4, 4);
                            }

                            _logger.LogDebug("Data Read for MsSQL Origin Source Query : {0}", query.ToString());

                            var data = BLMsSQLConnectionSupport.ExecuteQuery(dataTransfer.OriginSourceConnection.ConnectionString
                                , query.ToString());

                            dataTable = data.Copy();

                            _logger.LogDebug("Data Read for MsSQL Response : {0}", JsonConvert.SerializeObject(dataTable));
                            _logger.LogDebug("Data Read Ended for MsSQL Origin Source");
                            data.Dispose();
                        }

                        #endregion

                        #region MySQL

                        else if (dataTransfer.OriginSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                        {
                            _logger.LogDebug("Data Read Started for MySQL Origin Source");

                            query.Append("SELECT ");

                            foreach (var column in columns)
                            {
                                // Dependent Column Starts with #2
                                // For e.g #2County.CountryId
                                // Select Only columns of actual table and not dependent columns
                                if (!column.SourceColumn.StartsWith("#2"))
                                {
                                    query.Append(" `" + column.SourceColumn + "` AS " + column.SourceColumn + ",");
                                }
                                else
                                {
                                    // Add Dependent Column of Other Target table into list
                                    // This Columns will be filled up at run time
                                    dependentColumnList.Add(column.SourceColumn.Replace("#2", "")); // Dependent Column of Other Target table
                                }
                            }

                            query.Remove(query.Length - 1, 1);
                            query.Append(" FROM");
                            query.Append(" `" + bLDataTransferTableDetail.SourceTable + "` ");

                            if (filters.Count() > 0)
                            {
                                query.Append(" WHERE");
                                foreach (var filter in filters)
                                {
                                    query.Append(" " + GetFilterdColumn(
                                        "`" + filter.FilterColumn + "`",
                                        filter.FilterOperator,
                                        filter.FilterValue));
                                    query.Append(" AND");
                                }
                                query.Remove(query.Length - 4, 4);
                            }

                            _logger.LogDebug("Data Read for MySQL Origin Source Query : {0}", query.ToString());

                            var data = BLMySQLConnectionSupport.ExecuteQuery(dataTransfer.OriginSourceConnection.ConnectionString
                                , query.ToString());

                            dataTable = data.Copy();

                            _logger.LogDebug("Data Read for MySQL Response : {0}", JsonConvert.SerializeObject(dataTable));
                            _logger.LogDebug("Data Read Ended for MySQL Origin Source");
                            data.Dispose();

                        }

                        #endregion

                        #region Excel

                        else if (dataTransfer.OriginSourceConnection.DataSourceId.Equals(AppConstant.DataSource_Excel))
                        {
                            _logger.LogDebug("Data Read Started for Excel Origin Source");

                            query.Append("SELECT ");

                            foreach (var column in columns)
                            {
                                // Dependent Column Starts with #2
                                // For e.g #2County.CountryId
                                // Select Only columns of actual table and not dependent columns
                                if (!column.SourceColumn.StartsWith("#2"))
                                {
                                    query.Append(" [" + column.SourceColumn + "] ,");
                                }
                                else
                                {
                                    // Add Dependent Column of Other Target table into list
                                    // This Columns will be filled up at run time
                                    dependentColumnList.Add(column.SourceColumn.Replace("#2", ""));
                                }
                            }

                            query.Remove(query.Length - 1, 1);
                            query.Append(" FROM");
                            query.Append(" [" + bLDataTransferTableDetail.SourceTable + "] ");

                            if (filters.Count() > 0)
                            {
                                query.Append(" WHERE");
                                foreach (var filter in filters)
                                {
                                    query.Append(" " + GetFilterdColumn(
                                        "[" + filter.FilterColumn + "]",
                                        filter.FilterOperator,
                                        filter.FilterValue));
                                    query.Append(" AND");
                                }
                                query.Remove(query.Length - 4, 4);
                            }

                            query.Replace("Column_", "F");

                            _logger.LogDebug("Data Read for Excel Origin Source Query : {0}", query.ToString());

                            var data = BLOLEDBConnectionSupport.ExecuteQuery(dataTransfer.OriginSourceConnection.ConnectionString
                                , query.ToString());

                            dataTable = data.Copy();

                            _logger.LogDebug("Data Read for Excel Response : {0}", JsonConvert.SerializeObject(dataTable));
                            _logger.LogDebug("Data Read Ended for Excel Origin Source");
                            data.Dispose();
                        }

                        #endregion

                        _logger.LogDebug("Dependent Columns List : {0}", JsonConvert.SerializeObject(dependentColumnList));

                        // Add Dependent Columns in Original Data Source (Data Table) with nullable Type
                        foreach (string hasColumnItem in dependentColumnList)
                        {
                            DataColumn preTableColumn = dataTable.Columns.Add(hasColumnItem, typeof(Int32));
                            preTableColumn.AllowDBNull = true;
                        }


                        dataTable.TableName = bLDataTransferTableDetail.TargetTable;

                        originSourceData.Tables.Add(dataTable);

                        IsDataReadSuceess = true;
                    }
                }
                catch (Exception e)
                {
                    dataTransfer.ErrorMessage = e.ToString();
                    _logger.LogError(e, e.Message);
                }
            }

            if (IsDataReadSuceess.Equals(true))
                _logger.LogDebug("Data Read Successful");
            else
                _logger.LogDebug("Data Read Unsuccessful");

            #endregion

            #region Execute Transfer

            if (IsDataReadSuceess.GetValueOrDefault(false))
            {
                var IsTempTableDataInsertSuccess = true;

                #region Create Temporary Tables and Dump Data

                _logger.LogDebug("Started Create Temporary Table");

                foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
                {
                    var columns = dataTransfer.BLDataTransferColumnDetails
                        .Where(o => o.DataTransferId.Equals(dataTransfer.BLDatatransfer.DataTransferId)
                            && o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId))
                        .Select(o => Tuple.Create(o.SourceColumn.Replace("Column_", "F").Replace("#2", ""), o.TargetColumn));

                    #region MsSQL

                    if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                    {
                        try
                        {
                            // Create new Temp table with same Structure as Origin table
                            var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;
                            var createTempTableQuery = "DROP TABLE IF EXISTS " + tempTableName +
                               " ;" +
                               " SELECT * INTO" +
                               " " + tempTableName +
                               " FROM " +
                               " " + bLDataTransferTableDetail.TargetTable + " ; ";

                            _logger.LogDebug("Create Temporary Table MsSQL Query : {0}", createTempTableQuery);

                            BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, createTempTableQuery);

                            // Add GUID constraints into temp table
                            var guidColumns = BLMsSQLConnectionSupport.GetGUIDColumn(dataTransfer.TargetSourceConnection.ConnectionString, bLDataTransferTableDetail.TargetTable);
                            _logger.LogDebug("GUID Columns Fetched from Database : {0}", JsonConvert.SerializeObject(guidColumns));
                            if (guidColumns != null && guidColumns.Rows.Count > 0)
                            {
                                createTempTableQuery = string.Empty;
                                foreach (DataRow guidColumn in guidColumns.Rows)
                                {
                                    var constraintName = "df_" + guidColumn[0] + "_" + bLDataTransferTableDetail.DataTransferTableDetailId;

                                    createTempTableQuery += "IF EXISTS(SELECT * " +
                                        "FROM sys.foreign_keys " +
                                        "WHERE object_id = OBJECT_ID(N'" + constraintName + "') " +
                                        "AND parent_object_id = OBJECT_ID(N'dbo." + tempTableName + "') " +
                                        ") " +
                                        "ALTER TABLE[dbo].[" + tempTableName + "] DROP CONSTRAINT[" + constraintName + "]; ";

                                    createTempTableQuery += " ALTER TABLE " + tempTableName +
                                    " ADD CONSTRAINT " + constraintName +
                                    " DEFAULT NEWID() FOR " + guidColumn[0] + " ;";
                                }
                                _logger.LogDebug("Create GUID Columns MsSQL Query : {0}", createTempTableQuery);

                                BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, createTempTableQuery);
                            }

                            // Add unique constraints into temp table
                            var uniqueColumns = BLMsSQLConnectionSupport.GetUniqueColumn(dataTransfer.TargetSourceConnection.ConnectionString, bLDataTransferTableDetail.TargetTable);
                            _logger.LogDebug("Unique Columns Fetched from Database : {0}", JsonConvert.SerializeObject(guidColumns));
                            if (uniqueColumns != null && uniqueColumns.Rows.Count > 0)
                            {
                                createTempTableQuery = string.Empty;
                                foreach (DataRow uniqueColumn in uniqueColumns.Rows)
                                {
                                    var constraintName = "ak_" + uniqueColumn[0] + "_" + bLDataTransferTableDetail.DataTransferTableDetailId;

                                    createTempTableQuery += "IF EXISTS(SELECT * " +
                                        "FROM sys.foreign_keys " +
                                        "WHERE object_id = OBJECT_ID(N'" + constraintName + "') " +
                                        "AND parent_object_id = OBJECT_ID(N'dbo." + tempTableName + "') " +
                                        ") " +
                                        "ALTER TABLE[dbo].[" + tempTableName + "] DROP CONSTRAINT[" + constraintName + "]; ";

                                    createTempTableQuery += " ALTER TABLE " + tempTableName +
                                    " ADD CONSTRAINT " + constraintName +
                                    " UNIQUE ( " + uniqueColumn[0]
                                    + " ) ;";
                                }
                                _logger.LogDebug("Create Unique Columns MsSQL Query : {0}", createTempTableQuery);

                                BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, createTempTableQuery);
                            }

                            // Get Identity Column Name of Table
                            var drIdentityColumn = BLMsSQLConnectionSupport.GetIdentityColumn(dataTransfer.TargetSourceConnection.ConnectionString,
                                bLDataTransferTableDetail.TargetTable).Rows[0][0];
                            _logger.LogDebug("Identity Column Name: {0}", JsonConvert.SerializeObject(drIdentityColumn));

                            // Get Last Auto Increment Value
                            var autoIncrementValue = Convert.ToInt32(BLMsSQLConnectionSupport.GetAutoIncrementValue(dataTransfer.TargetSourceConnection.ConnectionString,
                                bLDataTransferTableDetail.TargetTable).Rows[0][0]);
                            _logger.LogDebug("Auto Increment Value: {0}", autoIncrementValue);

                            // Set Temp Table Auto Increment Value to Actual Table Auto Increment Value
                            // Will be useful in case of any records are deleted in between of Actual Table
                            if (autoIncrementValue > 0)
                            {
                                var setAutoIncrementValueQuery = "DBCC CHECKIDENT(" + tempTableName + ", RESEED, " + autoIncrementValue + ")";
                                _logger.LogDebug("Set Auto Increment Value query : {0}", setAutoIncrementValueQuery);
                                BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                    setAutoIncrementValueQuery);
                            }

                            // Current Target Table name with identity column e.g. : Country.CountryId
                            string strCurrentTargetTableName = bLDataTransferTableDetail.TargetTable + "." + drIdentityColumn;
                            string strDependentTableName = string.Empty;

                            // <TableName, ColumnName>
                            // Dependent Table and Column name
                            var DependentTableAndTableColumn = new Dictionary<string, string>();

                            // Iterate over all the Origin source to check dependency column of this table
                            foreach (DataTable TableItem in originSourceData.Tables)
                            {
                                DataColumnCollection CurrentTableColumns = TableItem.Columns;
                                strDependentTableName = CurrentTableColumns.Contains(strCurrentTargetTableName) ? TableItem.TableName : "";

                                // If current target table identity column found in some other table then add it into list
                                if (!string.IsNullOrEmpty(strDependentTableName))
                                    DependentTableAndTableColumn.Add(TableItem.TableName, strCurrentTargetTableName);

                            }
                            _logger.LogDebug("Dependent Table and Columns to Update Values : {0}", JsonConvert.SerializeObject(DependentTableAndTableColumn));

                            // Copy all the Source rows into temp table for duplication and Validation purpose
                            // Update Dependent columns in the Data Source by current table column identity value
                            var isOperationSuccess = BLMsSQLConnectionSupport.InsertBulkDataIntoTempTableOneToMany(
                                    dataTransfer.TargetSourceConnection.ConnectionString,
                                    originSourceData.Tables[bLDataTransferTableDetail.TargetTable],
                                    tempTableName,
                                    columns,
                                    DependentTableAndTableColumn,
                                    ref originSourceData,
                                    drIdentityColumn.ToString(),
                                    _logger
                                );
                            IsTempTableDataInsertSuccess = isOperationSuccess;

                            // Delete all the existing rows from temp table
                            if (autoIncrementValue > 0)
                            {
                                var deleteExistingRowsFromTempTableQuery = "DELETE FROM [" + tempTableName + "] WHERE [" + Convert.ToString(drIdentityColumn) + "] < " + autoIncrementValue;
                                _logger.LogDebug("Delete all the existing rows from Temp table query : {0}", deleteExistingRowsFromTempTableQuery);
                                BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                    deleteExistingRowsFromTempTableQuery);
                            }
                            _logger.LogDebug("Create Temp Table MsSQL Success");
                        }
                        catch (Exception e)
                        {
                            IsTempTableDataInsertSuccess = false;
                            _logger.LogDebug("Create Temp Table MsSQL Failed with error : {0}", e.ToString());
                            _logger.LogError(e, e.Message);
                        }
                    }

                    #endregion

                    #region MySQL

                    else if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                    {
                        try
                        {

                            //Create new Temp table with same Structure as Origin table
                            var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;
                            var createTempTableQuery = "DROP TABLE IF EXISTS " + tempTableName +
                               " ;" +
                               " CREATE TABLE " +
                               " " + tempTableName +
                               " LIKE " +
                               " " + bLDataTransferTableDetail.TargetTable + " ; " +
                               "INSERT INTO " + tempTableName + " SELECT * FROM " + bLDataTransferTableDetail.TargetTable + "; ";

                            _logger.LogDebug("Create Temporary Table MySQL Query : {0}", createTempTableQuery);

                            BLMySQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, createTempTableQuery);

                            //Get Identity Column Name of Table
                            var drIdentityColumn = BLMySQLConnectionSupport.GetIdentityColumn(dataTransfer.TargetSourceConnection.ConnectionString,
                                bLDataTransferTableDetail.TargetTable).Rows[0][0];
                            _logger.LogDebug("Identity Column Name: {0}", JsonConvert.SerializeObject(drIdentityColumn));

                            //Get Last Auto Increment Value
                            var autoIncrementValue = Convert.ToInt32(BLMySQLConnectionSupport.GetAutoIncrementValue(dataTransfer.TargetSourceConnection.ConnectionString,
                                bLDataTransferTableDetail.TargetTable).Rows[0][0]);
                            _logger.LogDebug("Auto Increment Value: {0}", autoIncrementValue);

                            // Set Temp Table Auto Increment Value to Actual Table Auto Increment Value
                            // Will be useful in case of any records are deleted in between of Actual Table
                            if (autoIncrementValue > 0)
                            {
                                var setAutoIncrementValueQuery = "ALTER TABLE `" + tempTableName + "` AUTO_INCREMENT = " + autoIncrementValue;
                                _logger.LogDebug("Set Auto Increment Value query : {0}", setAutoIncrementValueQuery);
                                BLMySQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                    setAutoIncrementValueQuery);
                            }

                            // Current Target Table name with identity column e.g. : Country.CountryId
                            string strCurrentTargetTableName = bLDataTransferTableDetail.TargetTable + "." + drIdentityColumn;
                            string strDependentTableName = string.Empty;

                            // <Table, ColumnName>
                            // Dependent Table and Column name
                            var DependentTableAndTableColumn = new Dictionary<string, string>();

                            // Iterate over all the Origin source to check dependency column of this table
                            foreach (DataTable TableItem in originSourceData.Tables)
                            {
                                strDependentTableName = TableItem.Columns.Contains(strCurrentTargetTableName)
                                    ? TableItem.TableName : "";

                                // If current target table identity column found in some other table then add it into list
                                if (!string.IsNullOrEmpty(strDependentTableName))
                                    DependentTableAndTableColumn.Add(TableItem.TableName, strCurrentTargetTableName);

                            }

                            // Copy all the Source rows into temp table for duplication and Validation purpose
                            // Update Dependent columns in the Data Source by current table column identity value
                            var isOperationSuccess = BLMySQLConnectionSupport.InsertBulkDataIntoTempTableOneToMany(
                                    dataTransfer.TargetSourceConnection.ConnectionString,
                                    originSourceData.Tables[bLDataTransferTableDetail.TargetTable],
                                    tempTableName,
                                    columns,
                                    DependentTableAndTableColumn,
                                    ref originSourceData,
                                    drIdentityColumn.ToString(),
                                    _logger
                                );

                            IsTempTableDataInsertSuccess = isOperationSuccess;
                            //Delete all the existing rows from temp table
                            if (autoIncrementValue > 0)
                            {
                                var deleteExistingRowsFromTempTableQuery = "DELETE FROM `" + tempTableName + "` WHERE `" + Convert.ToString(drIdentityColumn) + "` < " + autoIncrementValue;
                                _logger.LogDebug("Delete all the existing rows from Temp table query : {0}", deleteExistingRowsFromTempTableQuery);
                                BLMySQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                    deleteExistingRowsFromTempTableQuery);
                            }
                            _logger.LogDebug("Create Temp Table MySQL Success");
                        }
                        catch (Exception e)
                        {
                            IsTempTableDataInsertSuccess = false;
                            _logger.LogDebug("Create Temp Table MySQL Failed with error : {0}", e.ToString());
                            _logger.LogError(e, e.Message);
                        }
                    }

                    #endregion
                }

                _logger.LogDebug("Ended Create Temporary Table");

                #endregion

                #region Dump Data From Temporary Table to Actual Table

                if (IsTempTableDataInsertSuccess)
                {
                    _logger.LogDebug("Started Dump Data From Temporary Table to Actual Table");

                    #region MsSQL

                    if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                    {

                        try
                        {
                            var tables = dataTransfer.BLDataTransferTableDetails
                                .Select(o => Tuple.Create(o.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId, o.TargetTable));

                            var actualTransferQuery = new StringBuilder();

                            foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
                            {
                                var columns = dataTransfer.BLDataTransferColumnDetails
                                    .Where(o => o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId))
                                    .Select(o => o.TargetColumn);

                                var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;

                                actualTransferQuery.Append("INSERT INTO " + bLDataTransferTableDetail.TargetTable + " (");

                                foreach (var column in columns)
                                    actualTransferQuery.Append("[" + column + "] ,");
                                actualTransferQuery.Remove(actualTransferQuery.Length - 1, 1);
                                actualTransferQuery.Append(") ");

                                actualTransferQuery.Append("SELECT ");
                                foreach (var column in columns)
                                    actualTransferQuery.Append("[" + column + "] ,");
                                actualTransferQuery.Remove(actualTransferQuery.Length - 1, 1);
                                actualTransferQuery.Append(" FROM " + tempTableName + "; ");

                            }

                            _logger.LogDebug("Dump data from MsSQL Temp Table to Actual Table query : {0}", actualTransferQuery.ToString());

                            BLMsSQLConnectionSupport.ExecuteScalerQuery(
                                           dataTransfer.TargetSourceConnection.ConnectionString,
                                           actualTransferQuery.ToString()
                                       );

                            dataTransfer.DataTransferStatus = 2;

                            _logger.LogDebug("Dump data from MsSQL Temp Table to Actual Table Success");

                        }
                        catch (Exception e)
                        {
                            IsTempTableDataInsertSuccess = false;
                            dataTransfer.DataTransferStatus = 3;
                            _logger.LogDebug("Dump data from MsSQL Temp Table to Actual Table Failed with exception : {0}", e.ToString());
                            _logger.LogError("Dump data from MsSQL Temp Table to Actual Table Failed with exception : {0}", e.ToString());
                        }
                    }

                    #endregion

                    #region MySQL

                    if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                    {
                        try
                        {
                            var tables = dataTransfer.BLDataTransferTableDetails
                                .Select(o => Tuple.Create(o.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId, o.TargetTable));

                            var actualTransferQuery = new StringBuilder();

                            foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
                            {
                                var columns = dataTransfer.BLDataTransferColumnDetails
                                    .Where(o => o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId))
                                    .Select(o => o.TargetColumn);

                                var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;

                                actualTransferQuery.Append("INSERT INTO " + bLDataTransferTableDetail.TargetTable + " (");

                                foreach (var column in columns)
                                    actualTransferQuery.Append("`" + column + "` ,");
                                actualTransferQuery.Remove(actualTransferQuery.Length - 1, 1);
                                actualTransferQuery.Append(") ");

                                actualTransferQuery.Append("SELECT ");
                                foreach (var column in columns)
                                    actualTransferQuery.Append("`" + column + "` ,");
                                actualTransferQuery.Remove(actualTransferQuery.Length - 1, 1);
                                actualTransferQuery.Append(" FROM " + tempTableName + "; ");

                            }

                            _logger.LogDebug("Dump data from MySQL Temp Table to Actual Table query : {0}", actualTransferQuery.ToString());

                            BLMySQLConnectionSupport.ExecuteScalerQuery(
                                           dataTransfer.TargetSourceConnection.ConnectionString,
                                           actualTransferQuery.ToString()
                                       );

                            dataTransfer.DataTransferStatus = 2;

                            _logger.LogDebug("Dump data from MySQL Temp Table to Actual Table Success");
                        }
                        catch (Exception e)
                        {
                            IsTempTableDataInsertSuccess = false;
                            dataTransfer.DataTransferStatus = 3;
                            _logger.LogDebug("Dump data from MySQL Temp Table to Actual Table Failed with exception : {0}", e.ToString());
                            _logger.LogError("Dump data from MySQL Temp Table to Actual Table Failed with exception : {0}", e.ToString());
                        }
                    }

                    #endregion

                    _logger.LogDebug("Ended Dump Data From Temporary Table to Actual Table");

                }
                else
                {
                    dataTransfer.DataTransferStatus = 3;
                }

                #endregion

                #region Delete Temporary Table

                _logger.LogDebug("Started Delete Temporary Table");

                var deleteTempTableQuery = new StringBuilder();
                foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
                {
                    if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                    {

                        var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;

                        var guidColumns = BLMsSQLConnectionSupport.GetGUIDColumn(dataTransfer.TargetSourceConnection.ConnectionString, bLDataTransferTableDetail.TargetTable);
                        if (guidColumns != null && guidColumns.Rows.Count > 0)
                            foreach (DataRow guidColumn in guidColumns.Rows)
                            {
                                var constraintName = "df_" + guidColumn[0] + "_" + bLDataTransferTableDetail.DataTransferTableDetailId;

                                deleteTempTableQuery.Append("IF EXISTS ( SELECT * " +
                                    "FROM sys.foreign_keys " +
                                    "WHERE object_id = OBJECT_ID(N'" + constraintName + "') " +
                                    "AND parent_object_id = OBJECT_ID(N'dbo." + tempTableName + "') " +
                                    ") " +
                                    "ALTER TABLE[dbo].[" + tempTableName + "] DROP CONSTRAINT[" + constraintName + "]; ");
                            }

                        deleteTempTableQuery.Append("DROP TABLE IF EXISTS " + tempTableName + " ;");
                    }
                    else if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                    {
                        var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;
                        deleteTempTableQuery.Append("DROP TABLE IF EXISTS " + tempTableName + " ;");
                    }
                }
                _logger.LogDebug("Delete Temporary Table query : {0}", deleteTempTableQuery.ToString());
                try
                {
                    if (deleteTempTableQuery.Length > 0)
                        if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                            BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, deleteTempTableQuery.ToString());
                        else if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                            BLMySQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, deleteTempTableQuery.ToString());
                    _logger.LogDebug("Delete Temporary Table Success");
                }
                catch (Exception e)
                {
                    _logger.LogDebug("Delete Temporary Failed with exception : {0}", e.ToString());
                    _logger.LogError("Delete Temporary Failed with exception : {0}", e.ToString());
                }

                _logger.LogDebug("Ended Delete Temporary Table");

                #endregion
            }

            #endregion
        }

        private bool? PerformOneToOneDataTransfer(DataTransfer dataTransfer, DataSet originSourceData, bool? IsDataReadSuceess)
        {
            _logger.LogDebug("Data Transfer Type : One to One");

            #region Read Data from origin Source

            var bLDataTransferTableDetails = dataTransfer.BLDataTransferTableDetails
                                    .OrderBy(o => o.ExecutionOrder);

            foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
            {
                var columns = dataTransfer.BLDataTransferColumnDetails
                    .Where(o => o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId))
                    .Select(o => new
                    {
                        SourceColumn = o.SourceColumn,
                        IsUniqueColumn = o.IsUniqueColumn.GetValueOrDefault(false)
                    });

                var filters = dataTransfer.BLDataTransferFilterDetails
                    .Where(o => o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId)
                        && o.FilterOperator > 0)
                    .Select(o => new
                    {
                        o.FilterColumn,
                        FilterOperator = o.FilterOperator.GetValueOrDefault(0),
                        o.FilterValue
                    });

                try
                {
                    if (columns.Count() > 0)
                    {
                        DataTable dataTable = null;
                        var query = new StringBuilder();

                        #region MSSQL

                        if (dataTransfer.OriginSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                        {
                            _logger.LogDebug("Data Read Started for MsSQL Origin Source");

                            query.Append("SELECT ");

                            foreach (var column in columns)
                                query.Append(" [" + column.SourceColumn + "] ,");

                            query.Remove(query.Length - 1, 1);
                            query.Append(" FROM");
                            query.Append(" [" + bLDataTransferTableDetail.SourceTable + "] ");

                            if (filters.Count() > 0)
                            {
                                query.Append(" WHERE");
                                foreach (var filter in filters)
                                {
                                    query.Append(" " + GetFilterdColumn(
                                        "[" + filter.FilterColumn + "]",
                                        filter.FilterOperator,
                                        filter.FilterValue));
                                    query.Append(" AND");
                                }
                                query.Remove(query.Length - 4, 4);
                            }

                            _logger.LogDebug("Data Read for MsSQL Origin Source Query : {0}", query.ToString());

                            var data = BLMsSQLConnectionSupport.ExecuteQuery(dataTransfer.OriginSourceConnection.ConnectionString
                                , query.ToString());

                            dataTable = data.Copy();

                            _logger.LogDebug("Data Read for MsSQL Response : {0}", JsonConvert.SerializeObject(dataTable));
                            _logger.LogDebug("Data Read Ended for MsSQL Origin Source");
                            data.Dispose();
                        }

                        #endregion

                        #region MySQL

                        else if (dataTransfer.OriginSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                        {
                            _logger.LogDebug("Data Read Started for MySQL Origin Source");

                            query.Append("SELECT ");

                            foreach (var column in columns)
                                query.Append(" `" + column.SourceColumn + "` AS " + column.SourceColumn + ",");

                            query.Remove(query.Length - 1, 1);
                            query.Append(" FROM");
                            query.Append(" `" + bLDataTransferTableDetail.SourceTable + "` ");

                            if (filters.Count() > 0)
                            {
                                query.Append(" WHERE");
                                foreach (var filter in filters)
                                {
                                    query.Append(" " + GetFilterdColumn(
                                        "`" + filter.FilterColumn + "`",
                                        filter.FilterOperator,
                                        filter.FilterValue));
                                    query.Append(" AND");
                                }
                                query.Remove(query.Length - 4, 4);
                            }


                            _logger.LogDebug("Data Read for MySQL Origin Source Query : {0}", query.ToString());

                            var data = BLMySQLConnectionSupport.ExecuteQuery(dataTransfer.OriginSourceConnection.ConnectionString
                                , query.ToString());

                            dataTable = data.Copy();

                            _logger.LogDebug("Data Read for MySQL Response : {0}", JsonConvert.SerializeObject(dataTable));
                            _logger.LogDebug("Data Read Ended for MySQL Origin Source");
                            data.Dispose();
                        }

                        #endregion

                        #region Excel

                        else if (dataTransfer.OriginSourceConnection.DataSourceId.Equals(AppConstant.DataSource_Excel))
                        {
                            _logger.LogDebug("Data Read Started for Excel Origin Source");

                            query.Append("SELECT ");

                            foreach (var column in columns)
                                query.Append(" [" + column.SourceColumn + "] ,");

                            query.Remove(query.Length - 1, 1);
                            query.Append(" FROM");
                            query.Append(" [" + bLDataTransferTableDetail.SourceTable + "] ");

                            if (filters.Count() > 0)
                            {
                                query.Append(" WHERE");
                                foreach (var filter in filters)
                                {
                                    query.Append(" " + GetFilterdColumn(
                                        "[" + filter.FilterColumn + "]",
                                        filter.FilterOperator,
                                        filter.FilterValue));
                                    query.Append(" AND");
                                }
                                query.Remove(query.Length - 4, 4);
                            }

                            query.Replace("Column_", "F");

                            _logger.LogDebug("Data Read for Excel Origin Source Query : {0}", query.ToString());

                            var data = BLOLEDBConnectionSupport.ExecuteQuery(dataTransfer.OriginSourceConnection.ConnectionString
                                , query.ToString());

                            dataTable = data.Copy();

                            _logger.LogDebug("Data Read for Excel Response : {0}", JsonConvert.SerializeObject(dataTable));
                            _logger.LogDebug("Data Read Ended for Excel Origin Source");
                            data.Dispose();
                        }

                        #endregion

                        #region API

                        if (dataTransfer.BLDatatransfer.OriginSourceAPITemplateId.GetValueOrDefault(0) > 0)
                        {
                            _logger.LogDebug("Data Read Started for API Origin Source");

                            var apiGUID = new BLApi(dataTransfer.BLDatatransfer.OriginSourceAPITemplateId.Value).Apiguid;

                            _logger.LogDebug("API GUID to fetch master data : {0}", apiGUID);

                            var apiExecutionResponse = new APIManager(_logger).ExecuteAPI(apiGUID);

                            if (apiExecutionResponse.Status.Equals(1))
                            {
                                dataTable = AppCommon.GetDataTableFromJSON(apiExecutionResponse.APIResultData?.Trim());
                            }
                            else
                            {
                                _logger.LogDebug("API Execution Failed!");
                                throw new Exception("API Execution Failed!");
                            }

                            _logger.LogDebug("Data Read for API Response : {0}", JsonConvert.SerializeObject(dataTable));
                            _logger.LogDebug("Data Read Ended for API Origin Source");
                        }

                        #endregion

                        // Remove Duplicate Values if Is Duplication is true
                        if (bLDataTransferTableDetail.IsDeduplicateData.GetValueOrDefault(false))
                        {

                            _logger.LogDebug("Data De-duplication Enabled");

                            var uniqueColumns = columns.Where(o => o.IsUniqueColumn.Equals(true)).Select(o => o.SourceColumn);
                            foreach (var column in uniqueColumns)
                                dataTable = RemoveDuplicateRows(dataTable, column).Copy();

                            _logger.LogDebug("De-Duplicated Data : {0}", JsonConvert.SerializeObject(dataTable));
                        }

                        dataTable.TableName = bLDataTransferTableDetail.TargetTable;

                        originSourceData.Tables.Add(dataTable);

                        IsDataReadSuceess = true;
                    }
                }
                catch (Exception e)
                {
                    dataTransfer.ErrorMessage = e.ToString();
                    _logger.LogError(e, e.Message);
                }
            }

            if (IsDataReadSuceess.Equals(true))
                _logger.LogDebug("Data Read Successful");
            else
                _logger.LogDebug("Data Read Unsuccessful");

            #endregion

            #region Execute Transfer

            if (IsDataReadSuceess.GetValueOrDefault(false))
            {
                var IsTempTableDataInsertSuccess = true;

                #region Create Temporary Tables and Dump Data

                _logger.LogDebug("Started Create Temporary Table");

                foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
                {
                    var columns = dataTransfer.BLDataTransferColumnDetails
                        .Where(o => o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId))
                        .Select(o => Tuple.Create(o.SourceColumn.Replace("Column_", "F"), o.TargetColumn)); // Column_ is Column name created for User Interface for Excel Source

                    #region MsSQL

                    if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                    {
                        try
                        {
                            //Create new Temp table with same Structure as Origin table
                            var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;
                            var createTempTableQuery = "DROP TABLE IF EXISTS " + tempTableName +
                               " ;" +
                               " SELECT * INTO" +
                               " " + tempTableName +
                               " FROM " +
                               " " + bLDataTransferTableDetail.TargetTable + " ; ";

                            _logger.LogDebug("Create Temporary Table MsSQL Query : {0}", createTempTableQuery);

                            BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, createTempTableQuery);

                            // Add GUID constraints into temp table
                            var guidColumns = BLMsSQLConnectionSupport.GetGUIDColumn(dataTransfer.TargetSourceConnection.ConnectionString, bLDataTransferTableDetail.TargetTable);
                            _logger.LogDebug("GUID Columns Fetched from Database : {0}", JsonConvert.SerializeObject(guidColumns));
                            if (guidColumns != null && guidColumns.Rows.Count > 0)
                            {
                                createTempTableQuery = string.Empty;
                                foreach (DataRow guidColumn in guidColumns.Rows)
                                {
                                    var constraintName = "df_" + guidColumn[0] + "_" + bLDataTransferTableDetail.DataTransferTableDetailId;

                                    createTempTableQuery += "IF EXISTS(SELECT * " +
                                        "FROM sys.foreign_keys " +
                                        "WHERE object_id = OBJECT_ID(N'" + constraintName + "') " +
                                        "AND parent_object_id = OBJECT_ID(N'dbo." + tempTableName + "') " +
                                        ") " +
                                        "ALTER TABLE[dbo].[" + tempTableName + "] DROP CONSTRAINT[" + constraintName + "]; ";

                                    createTempTableQuery += " ALTER TABLE " + tempTableName +
                                    " ADD CONSTRAINT " + constraintName +
                                    " DEFAULT NEWID() FOR " + guidColumn[0] + " ;";
                                }

                                _logger.LogDebug("Create GUID Columns MsSQL Query : {0}", createTempTableQuery);

                                BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, createTempTableQuery);
                            }

                            // Add unique constraints into temp table
                            var uniqueColumns = BLMsSQLConnectionSupport.GetUniqueColumn(dataTransfer.TargetSourceConnection.ConnectionString, bLDataTransferTableDetail.TargetTable);
                            _logger.LogDebug("Unique Columns Fetched from Database : {0}", JsonConvert.SerializeObject(guidColumns));
                            if (uniqueColumns != null && uniqueColumns.Rows.Count > 0)
                            {
                                createTempTableQuery = string.Empty;
                                foreach (DataRow uniqueColumn in uniqueColumns.Rows)
                                {
                                    var constraintName = "ak_" + uniqueColumn[0] + "_" + bLDataTransferTableDetail.DataTransferTableDetailId;

                                    createTempTableQuery += "IF EXISTS(SELECT * " +
                                        "FROM sys.foreign_keys " +
                                        "WHERE object_id = OBJECT_ID(N'" + constraintName + "') " +
                                        "AND parent_object_id = OBJECT_ID(N'dbo." + tempTableName + "') " +
                                        ") " +
                                        "ALTER TABLE[dbo].[" + tempTableName + "] DROP CONSTRAINT[" + constraintName + "]; ";

                                    createTempTableQuery += " ALTER TABLE " + tempTableName +
                                    " ADD CONSTRAINT " + constraintName +
                                    " UNIQUE ( " + uniqueColumn[0] + " ) ;";
                                }
                                _logger.LogDebug("Create Unique Columns MsSQL Query : {0}", createTempTableQuery);

                                BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, createTempTableQuery);
                            }

                            //Get Identity Column Name of Table
                            var drIdentityColumn = BLMsSQLConnectionSupport.GetIdentityColumn(dataTransfer.TargetSourceConnection.ConnectionString,
                                bLDataTransferTableDetail.TargetTable).Rows;
                            _logger.LogDebug("Identity Column Name: {0}", JsonConvert.SerializeObject(drIdentityColumn));

                            // Get Last Auto Increment Value
                            var autoIncrementValue = 0;
                            if (drIdentityColumn.Count > 0)
                                autoIncrementValue = Convert.ToInt32(BLMsSQLConnectionSupport.GetAutoIncrementValue(dataTransfer.TargetSourceConnection.ConnectionString,
                                    bLDataTransferTableDetail.TargetTable).Rows[0][0]);
                            _logger.LogDebug("Auto Increment Value: {0}", autoIncrementValue);

                            // Set Temp Table Auto Increment Value to Actual Table Auto Increment Value
                            // Will be useful in case of any records are deleted in between of Actual Table
                            if (autoIncrementValue > 0)
                            {
                                var setAutoIncrementValueQuery = "DBCC CHECKIDENT(" + tempTableName + ", RESEED, " + autoIncrementValue + ")";
                                _logger.LogDebug("Set Auto Increment Value query : {0}", setAutoIncrementValueQuery);
                                BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                    setAutoIncrementValueQuery);
                            }

                            //Copy all the Source rows into temp table for duplication and Validation purpose
                            var isOperationSuccess = BLMsSQLConnectionSupport.InsertBulkDataIntoTempTable(
                                    dataTransfer.TargetSourceConnection.ConnectionString,
                                    originSourceData.Tables[bLDataTransferTableDetail.TargetTable],
                                    tempTableName,
                                    columns,
                                    _logger
                                );

                            IsTempTableDataInsertSuccess = isOperationSuccess;
                            //Delete all the existing rows from temp table
                            if (autoIncrementValue > 0)
                            {
                                var deleteExistingRowsFromTempTableQuery = "DELETE FROM [" + tempTableName + "] WHERE [" + Convert.ToString(drIdentityColumn[0][0]) + "] < " + autoIncrementValue;
                                _logger.LogDebug("Delete all the existing rows from Temp table query : {0}", deleteExistingRowsFromTempTableQuery);
                                BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                    deleteExistingRowsFromTempTableQuery);
                            }
                            _logger.LogDebug("Create Temp Table MsSQL Success");
                        }
                        catch (Exception e)
                        {
                            IsTempTableDataInsertSuccess = false;
                            _logger.LogDebug("Create Temp Table MsSQL Failed with error : {0}", e.ToString());
                            _logger.LogError(e, e.Message);
                        }
                    }

                    #endregion

                    #region MySQL

                    else if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                    {
                        try
                        {

                            //Create new Temp table with same Structure as Origin table
                            var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;
                            var createTempTableQuery = "DROP TABLE IF EXISTS " + tempTableName +
                               " ;" +
                               " CREATE TABLE " +
                               " " + tempTableName +
                               " LIKE " +
                               " " + bLDataTransferTableDetail.TargetTable + " ; " +
                               "INSERT INTO " + tempTableName + " SELECT * FROM " + bLDataTransferTableDetail.TargetTable + "; ";

                            _logger.LogDebug("Create Temporary Table MySQL Query : {0}", createTempTableQuery);

                            BLMySQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, createTempTableQuery);

                            //Get Identity Column Name of Table
                            var drIdentityColumn = BLMySQLConnectionSupport.GetIdentityColumn(dataTransfer.TargetSourceConnection.ConnectionString,
                                bLDataTransferTableDetail.TargetTable).Rows;
                            _logger.LogDebug("Identity Column Name: {0}", JsonConvert.SerializeObject(drIdentityColumn));

                            //Get Last Auto Increment Value
                            var autoIncrementValue = 0;
                            if (drIdentityColumn.Count > 0)
                                autoIncrementValue = Convert.ToInt32(BLMySQLConnectionSupport.GetAutoIncrementValue(dataTransfer.TargetSourceConnection.ConnectionString,
                                    bLDataTransferTableDetail.TargetTable).Rows[0][0]);
                            _logger.LogDebug("Auto Increment Value: {0}", autoIncrementValue);

                            // Set Temp Table Auto Increment Value to Actual Table Auto Increment Value
                            // Will be useful in case of any records are deleted in between of Actual Table
                            if (autoIncrementValue > 0)
                            {
                                var setAutoIncrementValueQuery = "ALTER TABLE `" + tempTableName + "` AUTO_INCREMENT = " + autoIncrementValue;
                                _logger.LogDebug("Set Auto Increment Value query : {0}", setAutoIncrementValueQuery);
                                BLMySQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                    setAutoIncrementValueQuery);
                            }

                            //Copy all the Source rows into temp table for duplication and Validation purpose
                            bool isOperationSuccess = BLMySQLConnectionSupport.InsertBulkDataIntoTempTable(
                                    dataTransfer.TargetSourceConnection.ConnectionString,
                                    originSourceData.Tables[bLDataTransferTableDetail.TargetTable],
                                    tempTableName,
                                    columns,
                                    _logger
                                );
                            IsTempTableDataInsertSuccess = isOperationSuccess;
                            //Delete all the existing rows from temp table
                            if (autoIncrementValue > 0)
                            {
                                var deleteExistingRowsFromTempTableQuery = "DELETE FROM `" + tempTableName + "` WHERE `" + Convert.ToString(drIdentityColumn[0][0]) + "` < " + autoIncrementValue;
                                _logger.LogDebug("Delete all the existing rows from Temp table query : {0}", deleteExistingRowsFromTempTableQuery);
                                BLMySQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString,
                                    deleteExistingRowsFromTempTableQuery);
                            }

                            _logger.LogDebug("Create Temp Table MySQL Success");
                        }
                        catch (Exception e)
                        {
                            IsTempTableDataInsertSuccess = false;
                            _logger.LogDebug("Create Temp Table MySQL Failed with error : {0}", e.ToString());
                            _logger.LogError(e, e.Message);
                        }
                    }

                    #endregion
                }

                _logger.LogDebug("Ended Create Temporary Table");

                #endregion

                #region Dump Data From Temporary Table to Actual Table

                if (IsTempTableDataInsertSuccess)
                {
                    _logger.LogDebug("Started Dump Data From Temporary Table to Actual Table");

                    #region MsSQL

                    if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                    {
                        try
                        {
                            var tables = dataTransfer.BLDataTransferTableDetails
                                .Select(o => Tuple.Create(o.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId, o.TargetTable));

                            var actualTransferQuery = new StringBuilder();

                            foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
                            {
                                var columns = dataTransfer.BLDataTransferColumnDetails
                                    .Where(o => o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId))
                                    .Select(o => o.TargetColumn);

                                var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;

                                actualTransferQuery.Append("INSERT INTO " + bLDataTransferTableDetail.TargetTable + " (");

                                foreach (var column in columns)
                                    actualTransferQuery.Append("[" + column + "] ,");
                                actualTransferQuery.Remove(actualTransferQuery.Length - 1, 1);
                                actualTransferQuery.Append(") ");

                                actualTransferQuery.Append("SELECT ");
                                foreach (var column in columns)
                                    actualTransferQuery.Append("[" + column + "] ,");
                                actualTransferQuery.Remove(actualTransferQuery.Length - 1, 1);
                                actualTransferQuery.Append(" FROM " + tempTableName + "; ");

                            }

                            _logger.LogDebug("Dump data from MsSQL Temp Table to Actual Table query : {0}", actualTransferQuery.ToString());

                            BLMsSQLConnectionSupport.ExecuteScalerQuery(
                                           dataTransfer.TargetSourceConnection.ConnectionString,
                                           actualTransferQuery.ToString()
                                       );

                            dataTransfer.DataTransferStatus = 2;

                            _logger.LogDebug("Dump data from MsSQL Temp Table to Actual Table Success");
                        }
                        catch (Exception e)
                        {
                            IsTempTableDataInsertSuccess = false;
                            dataTransfer.DataTransferStatus = 3;
                            _logger.LogDebug("Dump data from MsSQL Temp Table to Actual Table Failed with exception : {0}", e.ToString());
                            _logger.LogError("Dump data from MsSQL Temp Table to Actual Table Failed with exception : {0}", e.ToString());
                        }
                    }

                    #endregion

                    #region MySQL

                    if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                    {
                        try
                        {
                            var tables = dataTransfer.BLDataTransferTableDetails
                                .Select(o => Tuple.Create(o.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId, o.TargetTable));

                            var actualTransferQuery = new StringBuilder();

                            foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
                            {
                                var columns = dataTransfer.BLDataTransferColumnDetails
                                    .Where(o => o.DataTransferTableDetailId.Equals(bLDataTransferTableDetail.DataTransferTableDetailId))
                                    .Select(o => o.TargetColumn);

                                var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;

                                actualTransferQuery.Append("INSERT INTO " + bLDataTransferTableDetail.TargetTable + " (");

                                foreach (var column in columns)
                                    actualTransferQuery.Append("`" + column + "` ,");
                                actualTransferQuery.Remove(actualTransferQuery.Length - 1, 1);
                                actualTransferQuery.Append(") ");

                                actualTransferQuery.Append("SELECT ");
                                foreach (var column in columns)
                                    actualTransferQuery.Append("`" + column + "` ,");
                                actualTransferQuery.Remove(actualTransferQuery.Length - 1, 1);
                                actualTransferQuery.Append(" FROM " + tempTableName + "; ");

                            }
                            _logger.LogDebug("Dump data from MySQL Temp Table to Actual Table query : {0}", actualTransferQuery.ToString());

                            BLMySQLConnectionSupport.ExecuteScalerQuery(
                                           dataTransfer.TargetSourceConnection.ConnectionString,
                                           actualTransferQuery.ToString()
                                       );

                            dataTransfer.DataTransferStatus = 2;

                            _logger.LogDebug("Dump data from MySQL Temp Table to Actual Table Success");

                        }
                        catch (Exception e)
                        {
                            IsTempTableDataInsertSuccess = false;
                            dataTransfer.DataTransferStatus = 3;
                            _logger.LogDebug("Dump data from MySQL Temp Table to Actual Table Failed with exception : {0}", e.ToString());
                            _logger.LogError("Dump data from MySQL Temp Table to Actual Table Failed with exception : {0}", e.ToString());
                        }
                    }

                    #endregion

                    _logger.LogDebug("Ended Dump Data From Temporary Table to Actual Table");

                }
                else
                {
                    dataTransfer.DataTransferStatus = 3;
                }

                #endregion

                #region Delete Temporary Table

                _logger.LogDebug("Started Delete Temporary Table");

                var deleteTempTableQuery = new StringBuilder();
                foreach (var bLDataTransferTableDetail in bLDataTransferTableDetails)
                {
                    if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                    {

                        var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;

                        var guidColumns = BLMsSQLConnectionSupport.GetGUIDColumn(dataTransfer.TargetSourceConnection.ConnectionString, bLDataTransferTableDetail.TargetTable);
                        if (guidColumns != null && guidColumns.Rows.Count > 0)
                            foreach (DataRow guidColumn in guidColumns.Rows)
                            {
                                var constraintName = "df_" + guidColumn[0] + "_" + bLDataTransferTableDetail.DataTransferTableDetailId;

                                deleteTempTableQuery.Append("IF EXISTS ( SELECT * " +
                                    "FROM sys.foreign_keys " +
                                    "WHERE object_id = OBJECT_ID(N'" + constraintName + "') " +
                                    "AND parent_object_id = OBJECT_ID(N'dbo." + tempTableName + "') " +
                                    ") " +
                                    "ALTER TABLE[dbo].[" + tempTableName + "] DROP CONSTRAINT[" + constraintName + "]; ");
                            }

                        deleteTempTableQuery.Append("DROP TABLE IF EXISTS " + tempTableName + " ;");
                    }
                    else if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                    {
                        var tempTableName = bLDataTransferTableDetail.TargetTable + "_" + dataTransfer.BLDatatransfer.DataTransferId;
                        deleteTempTableQuery.Append("DROP TABLE IF EXISTS " + tempTableName + " ;");
                    }
                }
                _logger.LogDebug("Delete Temporary Table query : {0}", deleteTempTableQuery.ToString());
                try
                {
                    if (deleteTempTableQuery.Length > 0)
                        if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                            BLMsSQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, deleteTempTableQuery.ToString());
                        else if (dataTransfer.TargetSourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                            BLMySQLConnectionSupport.ExecuteScalerQuery(dataTransfer.TargetSourceConnection.ConnectionString, deleteTempTableQuery.ToString());
                    _logger.LogDebug("Delete Temporary Table Success");
                }
                catch (Exception e)
                {
                    _logger.LogDebug("Delete Temporary Failed with exception : {0}", e.ToString());
                    _logger.LogError("Delete Temporary Failed with exception : {0}", e.ToString());
                }

                _logger.LogDebug("Ended Delete Temporary Table");
                #endregion
            }

            #endregion
            return IsDataReadSuceess;
        }

        /// <summary>
        /// Save Data Transfer in Database based on Template GUID
        /// </summary>
        /// <param name="templateGUID">Template GUID to get data from</param>
        /// <returns>DataTransfer obj containing Data Transfer data along with Input and Output Parameters</returns>
        private DataTransfer SaveDataTransfer(Guid templateGUID)
        {
            _logger.LogDebug("Entered SaveDataTransfer for Template GUID {0}", templateGUID);
            var dataTransfer = new DataTransfer()
            {
                BLDatatransfer = new BLDataTransfer(),
                BLDataTransferColumnDetails = new List<BLDataTransferColumnDetail>(),
                BLDataTransferFilterDetails = new List<BLDataTransferFilterDetail>(),
                BLDataTransferTableDetails = new List<BLDataTransferTableDetail>()
            };
            try
            {
                var ds = BLDataTransferManager.SaveDataTransferFromTemplateGUID(templateGUID);
                var drTemplate = ds.Tables[0].Rows[0];
                var drDataTransfer = ds.Tables[1].Rows[0];

                if (drTemplate != null && drDataTransfer != null)
                {
                    dataTransfer.TemplateType = Convert.ToInt32(drTemplate["TemplateType"]);

                    var bLDataTransfer = new BLDataTransfer();
                    bLDataTransfer.TemplateId = toNullableInt(drDataTransfer["TemplateId"]);
                    bLDataTransfer.OriginSourceTypeId = toNullableInt(drDataTransfer["OriginSourceTypeId"]);
                    bLDataTransfer.OriginSourceAPITemplateId = toNullableInt(drDataTransfer["OriginSourceAPITemplateId"]);
                    bLDataTransfer.OriginSourceFileTypeId = toNullableInt(drDataTransfer["OriginSourceFileTypeId"]);
                    bLDataTransfer.OriginSourceServer = toStr(drDataTransfer["OriginSourceServer"]);
                    bLDataTransfer.OriginSourcePort = toStr(drDataTransfer["OriginSourcePort"]);
                    bLDataTransfer.OriginSourceUsername = toStr(drDataTransfer["OriginSourceUsername"]);
                    bLDataTransfer.OriginSourcePassword = toStr(drDataTransfer["OriginSourcePassword"]);
                    bLDataTransfer.OriginSourceDatabase = toStr(drDataTransfer["OriginSourceDatabase"]);
                    bLDataTransfer.OriginSourceFilePath = toStr(drDataTransfer["OriginSourceFilePath"]);
                    bLDataTransfer.OriginSourceFileName = toStr(drDataTransfer["OriginSourceFileName"]);
                    bLDataTransfer.IsFirstColumnContainHeader = Convert.ToBoolean(Convert.ToInt32(drDataTransfer["IsFirstColumnContainHeader"]));
                    bLDataTransfer.TargetSourceTypeId = toNullableInt(drDataTransfer["TargetSourceTypeId"]);
                    bLDataTransfer.TargetSourceAPITemplateId = toNullableInt(drDataTransfer["TargetSourceAPITemplateId"]);
                    bLDataTransfer.TargetSourceServer = toStr(drDataTransfer["TargetSourceServer"]);
                    bLDataTransfer.TargetSourcePort = toStr(drDataTransfer["TargetSourcePort"]);
                    bLDataTransfer.TargetSourceUsername = toStr(drDataTransfer["TargetSourceUsername"]);
                    bLDataTransfer.TargetSourcePassword = toStr(drDataTransfer["TargetSourcePassword"]);
                    bLDataTransfer.TargetSourceDatabase = toStr(drDataTransfer["TargetSourceDatabase"]);

                    bLDataTransfer.TransferStatus = toNullableInt(drDataTransfer["TransferStatus"]);
                    bLDataTransfer.TransferDate = GetDateTime();

                    bLDataTransfer.DataTransferGUID = toStr(drDataTransfer["DataTransferGUID"]);
                    bLDataTransfer.IsActive = true;
                    bLDataTransfer.IsDelete = false;
                    bLDataTransfer.EnteredDate = GetDateTime();
                    bLDataTransfer.EnteredBy = Convert.ToInt32(drDataTransfer["EnteredBy"]);
                    bLDataTransfer.UpdatedDate = GetDateTime();

                    bLDataTransfer.DataTransferId = Convert.ToInt32(drDataTransfer["DataTransferId"]);

                    dataTransfer.BLDatatransfer = bLDataTransfer;

                    #region Table Details

                    var dtDataTransferTableDetails = ds.Tables[2].Rows;

                    foreach (DataRow drTemplateTableDetail in dtDataTransferTableDetails)
                    {

                        var bLDataTransferTableDetail = new BLDataTransferTableDetail();
                        bLDataTransferTableDetail.DataTransferId = bLDataTransfer.DataTransferId;
                        bLDataTransferTableDetail.TemplateTableDetailId = Convert.ToInt32(drTemplateTableDetail["TemplateTableDetailId"]);

                        bLDataTransferTableDetail.SourceTable = Convert.ToString(drTemplateTableDetail["SourceTable"]);
                        bLDataTransferTableDetail.IsDeduplicateData = Convert.ToBoolean(Convert.ToInt32(drTemplateTableDetail["IsDeduplicateData"]));
                        bLDataTransferTableDetail.TargetTable = Convert.ToString(drTemplateTableDetail["TargetTable"]);
                        bLDataTransferTableDetail.ExecutionOrder = Convert.ToInt32(drTemplateTableDetail["ExecutionOrder"]);
                        bLDataTransferTableDetail.TotalRecords = 0;
                        bLDataTransferTableDetail.SuccessRecords = 0;
                        bLDataTransferTableDetail.ErrorRecords = 0;

                        bLDataTransferTableDetail.DataTransferTableDetailIdGUID = Convert.ToString(drTemplateTableDetail["DataTransferTableDetailIdGUID"]);
                        bLDataTransferTableDetail.IsActive = true;
                        bLDataTransferTableDetail.IsDelete = false;
                        bLDataTransferTableDetail.EnteredDate = GetDateTime();
                        bLDataTransferTableDetail.EnteredBy = Convert.ToInt32(drTemplateTableDetail["EnteredBy"]);

                        bLDataTransferTableDetail.DataTransferTableDetailId = Convert.ToInt32(drTemplateTableDetail["DataTransferTableDetailId"]);

                        dataTransfer.BLDataTransferTableDetails.Add(bLDataTransferTableDetail);

                    }
                    #endregion

                    #region Filter Details

                    var dtDataTransferFilterDetails = ds.Tables[3].Rows;

                    foreach (DataRow drDataTransferFilterDetail in dtDataTransferFilterDetails)
                    {
                        var bLDataTransferFilterDetail = new BLDataTransferFilterDetail();
                        bLDataTransferFilterDetail.DataTransferId = bLDataTransfer.DataTransferId;
                        bLDataTransferFilterDetail.DataTransferTableDetailId = Convert.ToInt32(drDataTransferFilterDetail["DataTransferTableDetailId"]);

                        bLDataTransferFilterDetail.TemplateTableDetailId = 0;

                        bLDataTransferFilterDetail.FilterColumn = Convert.ToString(drDataTransferFilterDetail["FilterColumn"]);
                        bLDataTransferFilterDetail.FilterOperator = Convert.ToInt32(drDataTransferFilterDetail["FilterOperator"]);
                        bLDataTransferFilterDetail.FilterValue = Convert.ToString(drDataTransferFilterDetail["FilterValue"]);

                        bLDataTransferFilterDetail.DataTransferFilterDetailGUID = Convert.ToString(drDataTransferFilterDetail["DataTransferFilterDetailGUID"]);
                        bLDataTransferFilterDetail.IsActive = true;
                        bLDataTransferFilterDetail.IsDelete = false;
                        bLDataTransferFilterDetail.EnteredDate = GetDateTime();
                        bLDataTransferFilterDetail.EnteredBy = Convert.ToInt32(drDataTransferFilterDetail["EnteredBy"]);

                        bLDataTransferFilterDetail.DataTransferFilterDetailId = Convert.ToInt32(drDataTransferFilterDetail["DataTransferFilterDetailId"]);

                        dataTransfer.BLDataTransferFilterDetails.Add(bLDataTransferFilterDetail);
                    }

                    #endregion

                    #region Column Details

                    var dtDataTransferColumnDetails = ds.Tables[4].Rows;

                    foreach (DataRow drDataTransferColumnDetail in dtDataTransferColumnDetails)
                    {
                        var bLDataTransferColumnDetail = new BLDataTransferColumnDetail();
                        bLDataTransferColumnDetail.DataTransferId = bLDataTransfer.DataTransferId;
                        bLDataTransferColumnDetail.DataTransferTableDetailId = Convert.ToInt32(drDataTransferColumnDetail["DataTransferTableDetailId"]);

                        bLDataTransferColumnDetail.TemplateColumnDetailId = 0;

                        bLDataTransferColumnDetail.SourceColumn = Convert.ToString(drDataTransferColumnDetail["SourceColumn"]);
                        bLDataTransferColumnDetail.SourceDependentColumn = Convert.ToString(drDataTransferColumnDetail["SourceDependentColumn"]);
                        bLDataTransferColumnDetail.IsUniqueColumn = Convert.ToBoolean(Convert.ToInt32(drDataTransferColumnDetail["IsUniqueColumn"]));
                        bLDataTransferColumnDetail.TargetColumn = Convert.ToString(drDataTransferColumnDetail["TargetColumn"]);

                        bLDataTransferColumnDetail.DataTransferColumnDetailGUID = Convert.ToString(drDataTransferColumnDetail["DataTransferColumnDetailGUID"]);
                        bLDataTransferColumnDetail.IsActive = true;
                        bLDataTransferColumnDetail.IsDelete = false;
                        bLDataTransferColumnDetail.EnteredDate = GetDateTime();
                        bLDataTransferColumnDetail.EnteredBy = Convert.ToInt32(drDataTransferColumnDetail["EnteredBy"]);

                        bLDataTransferColumnDetail.DataTransferColumnDetailId = Convert.ToInt32(drDataTransferColumnDetail["DataTransferColumnDetailId"]);

                        dataTransfer.BLDataTransferColumnDetails.Add(bLDataTransferColumnDetail);
                    }

                    #endregion

                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogDebug("Exited SaveDataTransfer with response {0}", JsonConvert.SerializeObject(dataTransfer));
            return dataTransfer;
        }

        /// <summary>
        /// Check if connections are valid or not for given credentials
        /// </summary>
        /// <param name="dataTransfer">DataTransfer Object containing Credentials</param>
        /// <returns>Returns DataTransfer object along with Connection status</returns>
        private DataTransfer GetConnectionStatus(DataTransfer dataTransfer)
        {
            _logger.LogDebug("Entered DataTransferController-GetConnectionStatus");
            try
            {
                var bLDataTransfer = dataTransfer.BLDatatransfer;
                if (bLDataTransfer != null)
                {
                    var originSourceConnection = new Connection
                    {
                        DataSourceId = bLDataTransfer.OriginSourceTypeId.GetValueOrDefault(0),
                        Server = bLDataTransfer.OriginSourceServer,
                        Username = bLDataTransfer.OriginSourceUsername,
                        Password = bLDataTransfer.OriginSourcePassword,
                        Port = bLDataTransfer.OriginSourcePort,
                        Database = bLDataTransfer.OriginSourceDatabase,
                        OriginSourceFilePath = bLDataTransfer.OriginSourceFilePath,
                        OriginSourceFileName = bLDataTransfer.OriginSourceFileName,
                        OriginSourceFileTypeId = bLDataTransfer.OriginSourceFileTypeId.GetValueOrDefault(0),
                    };
                    if (originSourceConnection.DataSourceId.Equals(AppConstant.DataSource_Excel))
                    {
                        originSourceConnection = new AppCommon(_logger).GetFileStatus(originSourceConnection,
                            bLDataTransfer.TemplateId.GetValueOrDefault(0),
                            bLDataTransfer.IsFirstColumnContainHeader.GetValueOrDefault(true));
                    }
                    else
                    {
                        originSourceConnection = new AppCommon(_logger).GetConnectionStatus(originSourceConnection);
                    }

                    dataTransfer.OriginSourceConnection = originSourceConnection;

                    if (originSourceConnection.IsConnectionSuccessfull)
                    {
                        _logger.LogDebug("Origin Source Connection Successful");
                        var targetSourceConnection = new Connection
                        {
                            DataSourceId = bLDataTransfer.TargetSourceTypeId.GetValueOrDefault(0),
                            Server = bLDataTransfer.TargetSourceServer,
                            Username = bLDataTransfer.TargetSourceUsername,
                            Password = bLDataTransfer.TargetSourcePassword,
                            Port = bLDataTransfer.TargetSourcePort,
                            Database = bLDataTransfer.TargetSourceDatabase,
                        };

                        targetSourceConnection = new AppCommon(_logger).GetConnectionStatus(targetSourceConnection);

                        dataTransfer.TargetSourceConnection = targetSourceConnection;

                        if (targetSourceConnection.IsConnectionSuccessfull)
                        {
                            _logger.LogDebug("Target Source Connection Successful");
                            dataTransfer.IsValidConnection = true;
                        }
                        else
                        {
                            _logger.LogDebug("Target Source Connection Unsuccessful");
                            dataTransfer.ErrorMessage = "Target Source Connection could not be established!";
                        }
                    }
                    else
                    {
                        _logger.LogDebug("Origin Source Connection Unsuccessful");
                        dataTransfer.ErrorMessage = "Origin Source Connection could not be established!";
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            _logger.LogDebug("Exited DataTransferController-GetConnectionStatus");
            return dataTransfer;
        }

        /// <summary>
        /// Get Conditional expression based on filter column, operator and filter value
        /// </summary>
        /// <param name="filterColumn"></param>
        /// <param name="filterOperator"></param>
        /// <param name="filterValue"></param>
        /// <returns>Returns Expression like "IsDelete = false"</returns>
        private string GetFilterdColumn(string filterColumn, int filterOperator, string filterValue)
        {
            var filterString = new StringBuilder();

            if (!(filterOperator.Equals(9) || filterOperator.Equals(10)) && !string.IsNullOrEmpty(filterValue))
            {
                filterString.Append(filterColumn);
                filterString.Append(" ");
                switch (filterOperator)
                {
                    case 1:
                        filterString.Append("= '" + filterValue + "'");
                        break;
                    case 2:
                        filterString.Append("<> '" + filterValue + "'");
                        break;
                    case 3:
                        filterString.Append("< '" + filterValue + "'");
                        break;
                    case 4:
                        filterString.Append("<= '" + filterValue + "'");
                        break;
                    case 5:
                        filterString.Append("> '" + filterValue + "'");
                        break;
                    case 6:
                        filterString.Append(">= '" + filterValue + "'");
                        break;
                    case 7:
                        {
                            filterString.Append("IN (");

                            var filterValues = filterValue.Split(',',
                                StringSplitOptions.RemoveEmptyEntries);

                            foreach (var item in filterValues)
                                filterString.Append("'" + item + "'");

                            filterString.Append(")");
                        }
                        break;
                    case 8:
                        {
                            filterString.Append("NOT IN (");

                            var filterValues = filterValue.Split(',',
                                StringSplitOptions.RemoveEmptyEntries);

                            foreach (var item in filterValues)
                                filterString.Append("'" + item + "'");

                            filterString.Append(")");
                        }
                        break;
                    case 11:
                        filterString.Append("LIKE '%" + filterValue + "%'");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                filterString.Append(filterColumn);
                filterString.Append(" ");
                switch (filterOperator)
                {
                    case 9:
                        filterString.Append("IS NULL ");
                        break;
                    case 10:
                        filterString.Append("IS NOT NULL ");
                        break;
                    default:
                        break;
                }
            }

            return filterString.ToString();
        }

        /// <summary>
        /// Remove Duplicate rows from DataTable
        /// </summary>
        /// <param name="dTable">Data Table object from which data should be removed</param>
        /// <param name="colName">Column named which should be checked for duplication</param>
        /// <returns>Returns DataTable with unique rows</returns>
        private DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
            //And add duplicate item value in arraylist.
            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            //Removing a list of duplicate items from datatable.
            foreach (DataRow dRow in duplicateList)
                dTable.Rows.Remove(dRow);

            //Datatable which contains unique records will be return as output.
            return dTable;
        }

        /// <summary>
        /// Check if All the Tables are mapped in Template
        /// </summary>
        /// <param name="templateGuid">Template GUID</param>
        /// <returns>Return if DataTransfer can be done further or not</returns>
        private bool ValidateColumnMappingForDataTransfer(Guid templateGuid)
        {
            _logger.LogDebug("Entered ValidateColumnMappingForDataTransfer with templateGuid : {0}", templateGuid);
            var isValid = false;

            var ds = BLDataTransferManager.GetColumnMappingForDataTransfer(templateGuid);
            using (ds)
            {
                isValid = Convert.ToInt32(ds.Tables[0].Rows[0][0]) == Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                _logger.LogDebug("GetColumnMappingForDataTransfer response : {0}", JsonConvert.SerializeObject(ds));
            }
            _logger.LogDebug("Exited ValidateColumnMappingForDataTransfer with response : {0}", isValid);
            return isValid;
        }

    }
}
