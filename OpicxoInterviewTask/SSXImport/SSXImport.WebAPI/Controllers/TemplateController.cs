using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SSXImport.Business;
using SSXImport.Business.ImplementationManager;
using SSXImport.WebAPI.Common;
using SSXImport.WebAPI.Models;
using SSXImport.WebAPI.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SSXImport.WebAPI.Controllers
{
    /// <summary>
    /// Used for Template Related Operations
    /// </summary>
    [Route("api/template")]
    [ApiController]
    public class TemplateController : BaseController
    {
        private readonly ILogger<TemplateController> _logger;

        /// <summary>
        /// Initialize ILogger for logging purpose
        /// </summary>
        /// <param name="logger"></param>
        public TemplateController(ILogger<TemplateController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get Template List bases of provided parameters
        /// </summary>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageFrom"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        [ApiVersion("1.0")]
        public virtual Response GetTemplateList(int sortCol = 0, string sortDir = "desc", int pageSize = int.MaxValue, int pageFrom = 0, string keyword = null, string applicationGuid = null)
        {

            _logger.LogInformation("Entered GetTemplateList");
            _logger.LogDebug("Entered GetTemplateList with Parameters sortCol: {0},sortDir: {1},pageSize: {2},pageFrom: {3},keyword: {4}",
                sortCol, sortDir, pageSize, pageFrom, keyword);
            try
            {
                var data = BLTemplateManager.GetTemplateList(
                    sortCol,
                    sortDir,
                    pageFrom,
                    pageSize,
                    keyword,
                    applicationGuid
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

            _logger.LogDebug("Exited GetTemplateList with response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetTemplateLit");

            return response;
        }

        /// <summary>
        /// Insert / Update / Delete Template
        /// </summary>
        /// <param name="form">Template object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("manage")]
        [ApiVersion("1.0")]
        public virtual Response ManageTemplate(Template form)
        {
            _logger.LogInformation("Entered ManageTemplate");
            _logger.LogDebug("Entered ManageTemplate with Request : {0}", JsonConvert.SerializeObject(form));
            try
            {
                bool isEdit = false;
                BLTemplate data = null;
                var oldFTPFileName = string.Empty;

                #region Check Record Existence
                if (!string.IsNullOrEmpty(form.TemplateGUID))
                {
                    data = new BLTemplate(form.TemplateGUID);

                    isEdit = true;
                    oldFTPFileName = data.OriginSourceFileName != null ? data.OriginSourceFileName : string.Empty;
                    _logger.LogDebug("Existing Record to update");
                }
                else
                {
                    data = new BLTemplate();
                    isEdit = false;
                    _logger.LogDebug("New Record to Insert");
                }

                if (isEdit && data == null)
                {
                    _logger.LogError("Template Record not found in Database for given Template GUID : {0}", form.TemplateGUID);
                    _logger.LogDebug("Exited ManageTemplate with Template Record not found in Database");
                    _logger.LogInformation("Exited ManageTemplate");
                    return GetNoRecordFound();
                }
                #endregion

                #region Delete Operation
                if (form.IsDelete == true)
                {
                    _logger.LogDebug("Existing Record to Delete");

                    BLTemplateManager.DeleteTemplate(Guid.Parse(form.TemplateGUID), form.UpdatedBy.Value);
                    response = GetSuccessResponse(data.TemplateGUID, Message: AppConstant.MessageDelete);

                    _logger.LogDebug("Record Deleted");
                }
                #endregion

                #region Main Operation
                else
                {
                    #region Insert / Update Operation

                    if (form.IsDelete == false)
                    {

                        #region Check Already Exist
                        var IsDuplicate = false;

                        var criteria = new Criteria<BLTemplate>();
                        criteria.Add(Expression.Eq("TemplateName", toStr(form.TemplateName)));
                        criteria.Add(Expression.Eq("IsDelete", false));
                        if (!string.IsNullOrEmpty(form.TemplateGUID))
                            criteria.Add(Expression.NotEq("TemplateGUID", form.TemplateGUID));

                        var count = new BLTemplate().CollectionCount(criteria);

                        if (count > 0)
                            IsDuplicate = true;

                        if (IsDuplicate == true)
                            return GetNoRecordFound("Template Name is already exist.");
                        #endregion

                        data.TemplateName = toStr(form.TemplateName);
                        data.OriginSourceTypeId = form.OriginSourceTypeId;
                        data.OriginSourceFileTypeId = form.OriginSourceFileTypeId;
                        data.OriginSourceServer = form.OriginSourceServer;
                        data.OriginSourcePort = form.OriginSourcePort;
                        data.OriginSourceUsername = form.OriginSourceUsername;
                        data.OriginSourcePassword = form.OriginSourcePassword;
                        data.OriginSourceDatabase = form.OriginSourceDatabase;
                        data.OriginSourceAPITemplateId = form.OriginSourceAPITemplateId;
                        data.TemplateType = form.TemplateType;

                        if (!(data.OriginSourceTypeId.Equals(AppConstant.DataSource_Excel)
                            && data.OriginSourceFileTypeId.Equals(1))// TODO add OS
                        )
                        {
                            data.OriginSourceFilePath = form.OriginSourceFilePath;
                            data.OriginSourceFileName = form.OriginSourceFileName;
                        }

                        data.IsFirstColumnContainHeader = form.IsFirstColumnContainHeader;
                        data.TargetSourceTypeId = form.TargetSourceTypeId;
                        data.TargetSourceAPITemplateId = form.TargetSourceAPITemplateId;
                        data.TargetSourceServer = form.TargetSourceServer;
                        data.TargetSourcePort = form.TargetSourcePort;
                        data.TargetSourceUsername = form.TargetSourceUsername;
                        data.TargetSourcePassword = form.TargetSourcePassword;
                        data.TargetSourceDatabase = form.TargetSourceDatabase;
                        data.IsScheduleEnabled = form.IsScheduleEnabled;
                        data.ScheduleType = form.ScheduleType;
                        data.FrequencyType = form.FrequencyType;
                        data.FrequencyRecurrsDailyEveryDay = form.FrequencyRecurrsDailyEveryDay;
                        data.FrequencyRecurrsWeeklyEveryWeek = form.FrequencyRecurrsWeeklyEveryWeek;
                        data.IsFrequencyRecurrsWeeklyOnMonday = form.IsFrequencyRecurrsWeeklyOnMonday;
                        data.IsFrequencyRecurrsWeeklyOnTuesday = form.IsFrequencyRecurrsWeeklyOnTuesday;
                        data.IsFrequencyRecurrsWeeklyOnWednesday = form.IsFrequencyRecurrsWeeklyOnWednesday;
                        data.IsFrequencyRecurrsWeeklyOnThursday = form.IsFrequencyRecurrsWeeklyOnThursday;
                        data.IsFrequencyRecurrsWeeklyOnFriday = form.IsFrequencyRecurrsWeeklyOnFriday;
                        data.IsFrequencyRecurrsWeeklyOnSaturday = form.IsFrequencyRecurrsWeeklyOnSaturday;
                        data.IsFrequencyRecurrsWeeklyOnSunday = form.IsFrequencyRecurrsWeeklyOnSunday;
                        data.FrequencyRecurrsMonthlyType = form.FrequencyRecurrsMonthlyType;
                        data.FrequencyRecurrsMonthtlyEveryMonth = form.FrequencyRecurrsMonthtlyEveryMonth;
                        data.FrequencyRecurrsMonthtlyDayOfMonth = form.FrequencyRecurrsMonthtlyDayOfMonth;
                        data.FrequencyRecurrsMonthtlyDayOfWeekOccurance = form.FrequencyRecurrsMonthtlyDayOfWeekOccurance;
                        data.FrequencyRecurrsMonthtlyDayOfWeek = form.FrequencyRecurrsMonthtlyDayOfWeek;
                        data.DailyFrequencyType = form.DailyFrequencyType;
                        data.DailyFrequencyTime = ConvertTime(form.DailyFrequencyTime);

                        data.DailyFrequencyOccuranceType = form.DailyFrequencyOccuranceType;
                        data.DailyFrequencyOccuranceEvery = form.DailyFrequencyOccuranceEvery;
                        data.DailyFrequencyOccuranceStartTime = ConvertTime(form.DailyFrequencyOccuranceStartTime);
                        data.DailyFrequencyOccuranceEndTime = ConvertTime(form.DailyFrequencyOccuranceEndTime);
                        data.DurationStartDate = ConvertDate(form.DurationStartDate);
                        data.IsDurationEndDateSpecified = form.IsDurationEndDateSpecified;
                        data.DurationEndDate = ConvertDate(form.DurationEndDate);
                        data.IsActive = form.IsActive;
                    }
                    data.IsDelete = form.IsDelete;
                    data.UpdatedBy = form.UpdatedBy;
                    data.UpdatedDate = GetDateTime();

                    if (isEdit)
                    {
                        data.Update();
                        _logger.LogDebug("Record Updated");
                    }
                    else
                    {
                        data.TemplateGUID = GetGUID();
                        data.EnteredDate = GetDateTime();
                        data.EnteredBy = form.EnteredBy;
                        data.SaveNew();
                        _logger.LogDebug("Record Inserted with Id :{0}", data.TemplateId);
                    }
                    #endregion

                    #region Download file in case of FTP

                    if (data.OriginSourceTypeId.Equals(AppConstant.DataSource_Excel)
                        && data.OriginSourceFileTypeId.Equals(2)
                        && !string.IsNullOrEmpty(data.OriginSourceFilePath)
                        && !string.IsNullOrEmpty(data.OriginSourceFileName)
                        //&& data.OriginSourceFileName != oldFTPFileName
                        )
                    {
                        var sessionOptions = new WinSCP.SessionOptions
                        {
                            Protocol = WinSCP.Protocol.Ftp,
                            HostName = data.OriginSourceServer,
                            PortNumber = Convert.ToInt32(data.OriginSourcePort),
                            UserName = data.OriginSourceUsername,
                            Password = data.OriginSourcePassword,
                        };

                        using (var session = new WinSCP.Session())
                        {
                            session.Open(sessionOptions);

                            // Download files
                            WinSCP.TransferOptions transferOptions = new WinSCP.TransferOptions();
                            transferOptions.TransferMode = WinSCP.TransferMode.Binary;
                            transferOptions.OverwriteMode = WinSCP.OverwriteMode.Overwrite;

                            var downloadPath = AppCommon.Environment.WebRootPath
                                + "\\Document\\Template\\"
                                + data.TemplateId.GetValueOrDefault(0)
                                + "\\";

                            CreateFolderIfNotExists(downloadPath);

                            downloadPath += data.OriginSourceFileName;

                            WinSCP.TransferOperationResult transferResult;
                            transferResult =
                                session.GetFiles(data.OriginSourceFilePath + "/" + data.OriginSourceFileName,
                                downloadPath,
                                false,
                                transferOptions);

                            // Throw on any error
                            transferResult.Check();
                        }
                    }

                    #endregion

                    #region Generate Response
                    response = GetSuccessResponse(data.TemplateGUID,
                        Message: isEdit
                            ? AppConstant.MessageUpdate
                            : AppConstant.MessageInsert);
                    #endregion
                }
                #endregion
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited ManageTemplate with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited ManageTemplate");
            return response;
        }

        /// <summary>
        /// Get Template Details By ID
        /// </summary>
        /// <param name="templateGuid">Template GUID based on which data will be fetched</param>
        /// <returns>Returns Template Details</returns>
        [HttpGet]
        [Route("edit")]
        [ApiVersion("1.0")]
        public virtual Response GetTemplateDetails(Guid templateGuid)
        {
            _logger.LogInformation("Entered GetTemplateDetails");
            _logger.LogDebug("Entered GetTemplateDetails with GUID : {0}", templateGuid);
            try
            {
                var dbDetails = new BLTemplate(templateGuid.ToString());

                if (dbDetails != null)
                {
                    var details = dbDetails.Cast<Template>();

                    details.IsValidOriginConnection = 0;
                    details.IsValidTargetConnection = 0;
                    details.OriginConnectionString = string.Empty;
                    details.TargetConnectionString = string.Empty;
                    details.IsScheduleEnabled = details.IsScheduleEnabled.GetValueOrDefault(false);
                    details.IsFrequencyRecurrsWeeklyOnMonday = details.IsFrequencyRecurrsWeeklyOnMonday.GetValueOrDefault(false);
                    details.IsFrequencyRecurrsWeeklyOnTuesday = details.IsFrequencyRecurrsWeeklyOnTuesday.GetValueOrDefault(false);
                    details.IsFrequencyRecurrsWeeklyOnWednesday = details.IsFrequencyRecurrsWeeklyOnWednesday.GetValueOrDefault(false);
                    details.IsFrequencyRecurrsWeeklyOnThursday = details.IsFrequencyRecurrsWeeklyOnThursday.GetValueOrDefault(false);
                    details.IsFrequencyRecurrsWeeklyOnFriday = details.IsFrequencyRecurrsWeeklyOnFriday.GetValueOrDefault(false);
                    details.IsFrequencyRecurrsWeeklyOnSaturday = details.IsFrequencyRecurrsWeeklyOnSaturday.GetValueOrDefault(false);
                    details.IsFrequencyRecurrsWeeklyOnSunday = details.IsFrequencyRecurrsWeeklyOnSunday.GetValueOrDefault(false);
                    details.IsDurationEndDateSpecified = details.IsDurationEndDateSpecified.GetValueOrDefault(false);
                    details.IsFirstColumnContainHeader = details.IsFirstColumnContainHeader.GetValueOrDefault(false);

                    if (dbDetails.DailyFrequencyTime != null)
                        details.DailyFrequencyTime = ConvertTime(dbDetails.DailyFrequencyTime);
                    else
                        details.DailyFrequencyTime = string.Empty;

                    if (dbDetails.DailyFrequencyOccuranceStartTime != null)
                        details.DailyFrequencyOccuranceStartTime = ConvertTime(dbDetails.DailyFrequencyOccuranceStartTime);
                    else
                        details.DailyFrequencyOccuranceStartTime = string.Empty;

                    if (dbDetails.DailyFrequencyOccuranceEndTime != null)
                        details.DailyFrequencyOccuranceEndTime = ConvertTime(dbDetails.DailyFrequencyOccuranceEndTime);
                    else
                        details.DailyFrequencyOccuranceEndTime = string.Empty;

                    if (dbDetails.DurationStartDate != null)
                        details.DurationStartDate = ConvertDate(dbDetails.DurationStartDate);
                    else
                        details.DurationStartDate = string.Empty;

                    if (dbDetails.DurationEndDate != null)
                        details.DurationEndDate = ConvertDate(dbDetails.DurationEndDate);
                    else
                        details.DurationEndDate = string.Empty;

                    #region Origin

                    var sourceConnectionRequest = new Connection
                    {
                        Server = details.OriginSourceServer,
                        Username = details.OriginSourceUsername,
                        Password = details.OriginSourcePassword,
                        Port = details.OriginSourcePort,
                        Database = details.OriginSourceDatabase,
                        DataSourceId = details.OriginSourceTypeId.GetValueOrDefault(0),
                        OriginSourceFileName = details.OriginSourceFileName,
                        OriginSourceFilePath = details.OriginSourceFilePath,
                        OriginSourceFileTypeId = details.OriginSourceFileTypeId.GetValueOrDefault(2),
                    };

                    var sourceConnection = new AppCommon(_logger).GetConnectionStatus(sourceConnectionRequest);
                    if (!sourceConnection.IsConnectionSuccessfull)
                        details.IsValidOriginConnection = 2;
                    else
                    {
                        details.IsValidOriginConnection = 1;
                        details.OriginConnectionString = sourceConnection.ConnectionString;

                        if (details.OriginSourceTypeId.Equals(1))
                        {
                            if (details.OriginSourceFileTypeId.Equals(2))
                            {
                                details.OriginSourceFilePathList = new AppCommon(_logger).GetFTPDirectory(sourceConnection);
                                details.OriginSourceFileNameList = new AppCommon(_logger).GetFTPFiles(sourceConnection).OrderBy(o => o.Name).ToList();
                            }
                        }

                        else if (details.OriginSourceTypeId.Equals(AppConstant.DataSource_MsSQL)
                            || details.OriginSourceTypeId.Equals(AppConstant.DataSource_MySQL))
                        {
                            DataTable dt = new DataTable();
                            if (sourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                                dt = BLMsSQLConnectionSupport.GetAllDatabase(sourceConnection.ConnectionString);
                            else if (sourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                                dt = BLMySQLConnectionSupport.GetAllDatabase(sourceConnection.ConnectionString);

                            if (dt.Rows.Count > 0)
                            {
                                var list = new List<MasterItem>();
                                foreach (DataRow dataRow in dt.Rows)
                                {
                                    list.Add(new MasterItem
                                    {
                                        Name = Convert.ToString(dataRow["Name"]),
                                        Id = Convert.ToString(dataRow["Name"])
                                    });
                                }
                                details.OriginSourceDatabaseList = list;

                                if (!string.IsNullOrEmpty(details.OriginSourceDatabase))
                                {
                                    var dtTables = new DataTable();
                                    if (sourceConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                                        dtTables = BLMsSQLConnectionSupport.GetAllTables(sourceConnection.ConnectionString);
                                    else if (sourceConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                                        dtTables = BLMySQLConnectionSupport.GetAllTables(sourceConnection.ConnectionString, sourceConnection.Database);

                                    if (dtTables.Rows.Count > 0)
                                    {
                                        list = new List<MasterItem>();
                                        foreach (DataRow dataRow in dtTables.Rows)
                                        {
                                            list.Add(new MasterItem
                                            {
                                                Name = Convert.ToString(dataRow["Name"]),
                                                Id = Convert.ToString(dataRow["Name"])
                                            });
                                        }
                                        details.OriginSourceTableList = list;
                                    }
                                }
                            }
                        }
                    }

                    if (details.OriginSourceTypeId.Equals(AppConstant.DataSource_API))
                    {
                        var list = new List<MasterItem>();
                        list.Add(new MasterItem
                        {
                            Name = Convert.ToString("API Response"),
                            Id = Convert.ToString("API Response")
                        });
                        details.OriginSourceTableList = list;
                    }

                    #endregion

                    #region Target 

                    var targetConnectionRequest = new Connection
                    {
                        Server = details.TargetSourceServer,
                        Username = details.TargetSourceUsername,
                        Password = details.TargetSourcePassword,
                        Port = details.TargetSourcePort,
                        Database = details.TargetSourceDatabase,
                        DataSourceId = details.TargetSourceTypeId.GetValueOrDefault(0),
                    };

                    var targetConnection = new AppCommon(_logger).GetConnectionStatus(targetConnectionRequest);
                    if (!targetConnection.IsConnectionSuccessfull)
                        details.IsValidTargetConnection = 2;
                    else
                    {
                        details.IsValidTargetConnection = 1;
                        details.TargetConnectionString = targetConnection.ConnectionString;
                        if (details.TargetSourceTypeId.Equals(AppConstant.DataSource_MsSQL)
                        || details.TargetSourceTypeId.Equals(AppConstant.DataSource_MySQL))
                        {
                            DataTable dt = new DataTable();
                            if (targetConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                                dt = BLMsSQLConnectionSupport.GetAllDatabase(targetConnection.ConnectionString);
                            else if (targetConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                                dt = BLMySQLConnectionSupport.GetAllDatabase(targetConnection.ConnectionString);

                            if (dt.Rows.Count > 0)
                            {
                                var list = new List<MasterItem>();
                                foreach (DataRow dataRow in dt.Rows)
                                {
                                    list.Add(new MasterItem
                                    {
                                        Name = Convert.ToString(dataRow["Name"])
                                    });
                                }
                                details.TargetSourceDatabaseList = list;

                                if (!string.IsNullOrEmpty(details.TargetSourceDatabase))
                                {
                                    var dtTables = new DataTable();
                                    if (targetConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                                        dtTables = BLMsSQLConnectionSupport.GetAllTables(targetConnection.ConnectionString);
                                    else if (targetConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                                        dtTables = BLMySQLConnectionSupport.GetAllTables(targetConnection.ConnectionString, targetConnection.Database);

                                    if (dtTables.Rows.Count > 0)
                                    {
                                        list = new List<MasterItem>();
                                        foreach (DataRow dataRow in dtTables.Rows)
                                        {
                                            list.Add(new MasterItem
                                            {
                                                Name = Convert.ToString(dataRow["Name"])
                                            });
                                        }
                                        details.TargetSourceTableList = list;
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    response = GetSuccessResponse(details);
                }
                else
                {
                    _logger.LogDebug("Template Details not found in database for GUID : {0}", templateGuid);
                    _logger.LogError("Template Details not found in database for GUID : {0}", templateGuid);
                    response = GetNoRecordFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited GetTemplateDetails with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetTemplateDetails");
            return response;
        }

        /// <summary>
        /// Insert / Update / Delete Template Table Details
        /// </summary>
        /// <param name="form">TemplateTableDetails object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("table/manage")]
        [ApiVersion("1.0")]
        public virtual Response ManageTemplateTableDetail(TemplateTableDetail form)
        {
            _logger.LogInformation("Entered ManageTemplateTableDetail");
            _logger.LogDebug("Entered ManageTemplateTableDetail with Request : {0}", JsonConvert.SerializeObject(form));
            try
            {
                bool isEdit = false;
                BLTemplateTableDetail data = null;

                #region Check Record Existence
                if (!string.IsNullOrEmpty(form.TemplateTableDetailGUID))
                {
                    data = new BLTemplateTableDetail(form.TemplateTableDetailGUID);
                    isEdit = true;
                    _logger.LogDebug("Existing Record to update");

                }
                else
                {
                    data = new BLTemplateTableDetail();
                    isEdit = false;
                    _logger.LogDebug("New Record to Insert");
                }

                if (isEdit && data == null)
                {
                    _logger.LogError("Record not found in Database for given GUID : {0}", form.TemplateTableDetailGUID);
                    _logger.LogDebug("Exited ManageTemplateTableDetail with Record not found in Database");
                    _logger.LogInformation("Exited ManageTemplateTableDetail");
                    return GetNoRecordFound();
                }
                #endregion

                #region Insert / Update / Delete operation
                if (form.IsDelete.Equals(false))
                {
                    #region Fetch TemplateId from TemplateGUID

                    var objTemplate = new BLTemplate(form.TemplateGUID);

                    form.TemplateId = objTemplate != null ? objTemplate.TemplateId.GetValueOrDefault(0) : 0;

                    int intTemplateType = objTemplate != null ? objTemplate.TemplateType.GetValueOrDefault(0) : 0;

                    var objTemplateTableDetail = new BLTemplateTableDetail().Collection<BLTemplateTableDetail>(
                      new Criteria<BLTemplateTableDetail>()
                          .Add(Expression.Eq("TemplateId", form.TemplateId))
                          .Add(Expression.Eq("IsDelete", false))
                          ).FirstOrDefault();

                    string sourceTableName = objTemplateTableDetail != null ? objTemplateTableDetail.SourceTable : string.Empty;

                    #endregion

                    #region Check Source Table Already Exist For One To Many

                    var IsDiffrentSourceTableOTM = false;

                    if (!string.IsNullOrEmpty(sourceTableName))
                    {
                        if (form.SourceTable != sourceTableName)
                        {
                            IsDiffrentSourceTableOTM = true;
                        }
                    }

                    #endregion

                    #region Check Already Exist

                    var count = new BLTemplateTableDetail().CollectionCount(
                          new Criteria<BLTemplateTableDetail>()
                              .Add(Expression.Eq("TargetTable", toStr(form.TargetTable)))
                              .Add(Expression.Eq("IsDelete", false))
                              .Add(Expression.Eq("TemplateId", form.TemplateId))
                              );

                    if (intTemplateType == 1)
                    {
                        if (count > 0)
                            return GetNoRecordFound("Target Source Table("
                                + toStr(form.TargetTable)
                                + ") Table is already exist for One To One (Normal) Template Type.");
                    }
                    else if (intTemplateType == 2)
                    {
                        if (IsDiffrentSourceTableOTM)
                        {
                            return GetNoRecordFound("Source Table("
                                + toStr(form.SourceTable)
                                + ") Table can't be different for One To Many Template Type, Please select "
                                + sourceTableName
                                + " as Source Table");
                        }
                    }
                    else if (intTemplateType == 3)
                    {

                    }

                    #endregion

                    data.TemplateId = form.TemplateId;
                    data.SourceTable = toStr(form.SourceTable);
                    data.IsDeduplicateData = form.IsDeduplicateData;
                    data.TargetTable = toStr(form.TargetTable);
                    data.IsActive = form.IsActive;

                    #region Check Count for Order

                    var existingCount = new BLTemplateTableDetail().CollectionCount(
                      new Criteria<BLTemplateTableDetail>()
                          .Add(Expression.Eq("TemplateId", form.TemplateId))
                          .Add(Expression.Eq("IsDelete", false))
                          );

                    data.ExecutionOrder = existingCount + 1;

                    #endregion

                    data.IsDelete = form.IsDelete;
                    data.UpdatedBy = form.UpdatedBy;
                    data.UpdatedDate = GetDateTime();

                    if (isEdit)
                    {
                        data.Update();
                        _logger.LogDebug("Record Updated");
                    }
                    else
                    {
                        data.TemplateTableDetailGUID = GetGUID();
                        data.EnteredDate = GetDateTime();
                        data.EnteredBy = form.EnteredBy;
                        data.SaveNew();
                        _logger.LogDebug("Record Inserted with Id :{0}", data.TemplateTableDetailId);
                    }
                }
                else
                {
                    _logger.LogDebug("Existing Record to Delete");

                    BLTemplateManager.DeleteTemplateTableDetail(data.TemplateTableDetailId.Value, form.UpdatedBy.Value);

                    #region Update Execution Order

                    var sb = new StringBuilder();

                    sb.Append("UPDATE TemplateTableDetail ");
                    sb.Append("SET ");
                    sb.Append("ExecutionOrder = ExecutionOrder - 1 ");
                    sb.Append(", UpdatedDate = NOW() ");
                    sb.Append("WHERE IsDelete = 0 ");
                    sb.Append("AND TemplateId = " + data.TemplateId + " ");
                    sb.Append("AND ExecutionOrder > " + data.ExecutionOrder.GetValueOrDefault(0));

                    _logger.LogDebug("ManageTemplateTableDetail Delete query to execute: {0}", sb.ToString());

                    BLTemplateManager.ExecuteScalarQuery(sb.ToString());

                    #endregion

                    _logger.LogDebug("Record Deleted");

                }

                #endregion

                #region Generate Response
                if (data.IsDelete == true)
                    response = GetSuccessResponse(data.TemplateTableDetailGUID, Message: AppConstant.MessageDelete);
                else
                    response = GetSuccessResponse(data.TemplateTableDetailGUID, Message: isEdit
                        ? AppConstant.MessageUpdate
                        : AppConstant.MessageInsert);
                #endregion

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited ManageTemplateTableDetail with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited ManageTemplateTableDetail");
            return response;
        }

        /// <summary>
        /// Get Template Table List based on provided parameters
        /// </summary>
        /// <param name="templateGUID"></param>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageFrom"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("table/list")]
        [ApiVersion("1.0")]
        public virtual Response GetTemplateTableDetailList([Required] string templateGUID, int sortCol = 0, string sortDir = "desc",
            int pageSize = int.MaxValue, int pageFrom = 0, string keyword = null)
        {
            _logger.LogInformation("Entered GetTemplateTableDetailList");
            _logger.LogDebug("Entered GetTemplateTableDetailList with Parameters templateGUID:{5}, sortCol: {0},sortDir: {1},pageSize: {2},pageFrom: {3},keyword: {4}",
                sortCol, sortDir, pageSize, pageFrom, keyword, templateGUID);
            try
            {
                var dt = BLTemplateManager.GetTemplateTableDetailsList(templateGUID);

                response = GetDataTableResponse(dt);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited GetTemplateTableDetailList with response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetTemplateTableDetailList");
            return response;
        }

        /// <summary>
        /// Update Table Row Order details for execution
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("table/updateorder")]
        [ApiVersion("1.0")]
        public virtual Response UpdateTemplateTableRowOrder(List<TemplateReOrderTableDetail> request)
        {
            _logger.LogInformation("Entered UpdateTemplateTableRowOrder");
            _logger.LogDebug("Entered UpdateTemplateTableRowOrder with request : {0}", JsonConvert.SerializeObject(request));
            try
            {
                BLTemplateManager.DeleteTemplateColumnDetail(Guid.Parse(request.FirstOrDefault().TemplateTableDetailGUID), 1);

                var sb = new StringBuilder();

                foreach (var templateReOrderTableDetail in request)
                {
                    sb.Append("UPDATE TemplateTableDetail ");
                    sb.Append("SET ");
                    sb.Append("ExecutionOrder = " + templateReOrderTableDetail.NewPosition + " ");
                    sb.Append(", UpdatedDate = NOW() ");
                    sb.Append("WHERE TemplateTableDetailGUID = ");
                    sb.Append("'" + templateReOrderTableDetail.TemplateTableDetailGUID + "'; ");
                }

                _logger.LogDebug("UpdateTemplateTableRowOrder query to execute: {0}", sb.ToString());

                BLTemplateManager.ExecuteScalarQuery(sb.ToString());

                return GetSuccessResponse("1", 1, "", "");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited UpdateTemplateTableRowOrder with response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited UpdateTemplateTableRowOrder");
            return response;
        }

        /// <summary>
        /// Insert / Update / Delete Template Column Details
        /// </summary>
        /// <param name="form">TemplateColumnDetail Object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("column/manage")]
        [ApiVersion("1.0")]
        public virtual Response ManageTemplateColumnDetail(TemplateColumnDetail form)
        {
            _logger.LogInformation("Entered ManageTemplateColumnDetail");
            _logger.LogDebug("Entered ManageTemplateColumnDetail with Request : {0}", JsonConvert.SerializeObject(form));

            try
            {
                #region Fetch TemplateId from TemplateGUID

                var _TemplateId = new BLTemplate(form.TemplateGUID).TemplateId.GetValueOrDefault(0);

                #endregion

                #region Fetch TemplateTableDetailId from TemplateTableDetailGUID

                var _TemplateTableDetailId = new BLTemplateTableDetail(form.TemplateTableDetailGUID).TemplateTableDetailId.GetValueOrDefault(0);

                #endregion

                #region Get Old Details
                var bLTemplateColumnDetails = new List<BLTemplateColumnDetail>();

                if (_TemplateTableDetailId > 0)
                {
                    try
                    {
                        bLTemplateColumnDetails = new BLTemplateColumnDetail().Collection<BLTemplateColumnDetail>(
                         new Criteria<BLTemplateColumnDetail>()
                             .Add(Expression.Eq("TemplateId", _TemplateId))
                             .Add(Expression.Eq("TemplateTableDetailId", _TemplateTableDetailId))
                             .Add(Expression.Eq("IsDelete", false))
                             ).ToList();
                    }
                    catch (Exception e)
                    {
                        GetException(e);
                    }
                }
                #endregion Get Old Details

                var listChildDetails = new List<TemplateColumnDetail>();
                var listDeleteIdsDetails = new List<string>();

                if (IsStr(form.Details_Insert))
                    listChildDetails = JsonConvert.DeserializeObject<List<TemplateColumnDetail>>(form.Details_Insert);

                if (IsStr(form.Details_Delete))
                {
                    listDeleteIdsDetails = (toStr(form.Details_Delete)).
                        Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).
                        Select(x => x.ToString()).ToList();
                }

                #region Delete Details

                if (listDeleteIdsDetails.Count > 0)
                {
                    foreach (var item in listDeleteIdsDetails)
                    {
                        try
                        {
                            using (var templateColumnDetail = bLTemplateColumnDetails.FirstOrDefault(o => o.TemplateColumnDetailGUID.ToString() == item))
                            {
                                templateColumnDetail.IsDelete = true;
                                templateColumnDetail.UpdatedBy = form.UpdatedBy;
                                templateColumnDetail.UpdatedDate = GetDateTime();

                                templateColumnDetail.Update();
                            }
                        }
                        catch (Exception e) { GetException(e); }
                    }
                }

                #endregion Delete Details

                #region Update Details

                var listUpdateDetails = listChildDetails.Where(o => o.TemplateColumnDetailGUID != null).ToList();
                foreach (var item in listUpdateDetails)
                {
                    try
                    {
                        var templateColumnDetail = bLTemplateColumnDetails.FirstOrDefault(o => o.TemplateColumnDetailGUID == item.TemplateColumnDetailGUID.ToString());
                        templateColumnDetail.TemplateId = _TemplateId;
                        templateColumnDetail.TemplateTableDetailId = _TemplateTableDetailId;

                        templateColumnDetail.SourceColumn = toStr(item.SourceColumn);
                        templateColumnDetail.SourceDependentColumn = toStr(item.SourceDependentColumn);
                        templateColumnDetail.IsUniqueColumn = item.IsUniqueColumn;
                        templateColumnDetail.TargetColumn = toStr(item.TargetColumn);

                        templateColumnDetail.UpdatedDate = GetDateTime();
                        templateColumnDetail.UpdatedBy = form.UpdatedBy;

                        templateColumnDetail.Update();
                    }
                    catch (Exception e) { GetException(e); }
                }

                #endregion Update Details

                #region Insert Details

                var listInsertDetails = listChildDetails.Where(o => o.TemplateColumnDetailGUID == null).ToList();
                foreach (var item in listInsertDetails)
                {
                    try
                    {
                        var templateColumnDetail = new BLTemplateColumnDetail();
                        templateColumnDetail.TemplateId = _TemplateId;
                        templateColumnDetail.TemplateTableDetailId = _TemplateTableDetailId;

                        templateColumnDetail.SourceColumn = toStr(item.SourceColumn);
                        templateColumnDetail.SourceDependentColumn = toStr(item.SourceDependentColumn);
                        templateColumnDetail.IsUniqueColumn = item.IsUniqueColumn;
                        templateColumnDetail.TargetColumn = toStr(item.TargetColumn);

                        templateColumnDetail.TemplateColumnDetailGUID = GetGUID();
                        templateColumnDetail.EnteredDate = GetDateTime();
                        templateColumnDetail.EnteredBy = form.EnteredBy;
                        templateColumnDetail.IsDelete = false;
                        templateColumnDetail.IsActive = true;

                        templateColumnDetail.SaveNew();
                    }
                    catch (Exception e) { GetException(e); }
                }

                #endregion Insert Details

                #region Generate Response
                response = GetSuccessResponse(1, Message: bLTemplateColumnDetails.Count > 0
                    ? AppConstant.MessageUpdate
                    : AppConstant.MessageInsert);
                #endregion

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }

            _logger.LogDebug("Exited ManageTemplateColumnDetail with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited ManageTemplateColumnDetail");
            return response;
        }

        /// <summary>
        /// Get List of Template Column details based on provided parameters
        /// </summary>
        /// <param name="templateGUID"></param>
        /// <param name="templateTableDetailGUID"></param>
        /// <returns>Returns List of Template Column Details</returns>
        [HttpGet]
        [Route("column/list")]
        [ApiVersion("1.0")]
        public virtual Response GetTemplateColumnDetailList(Guid templateGUID, Guid templateTableDetailGUID)
        {
            _logger.LogInformation("Entered GetTemplateColumnDetailList");
            _logger.LogDebug("Entered GetTemplateColumnDetailList with templateGUID : {0}, templateTableDetailGUID : {1}",
                templateGUID,
                templateTableDetailGUID);

            var originSourceColumnDetails = new List<TemplateColumnDetail>();
            var originSourceDependentColumnDetails = new List<TemplateColumnDetail>();
            var targetSourceColumnDetails = new List<TemplateColumnDetail>();

            try
            {
                var ds = BLTemplateManager.GetTemplateColumnDetailList(templateGUID, templateTableDetailGUID);

                var drTemplate = ds.Tables[0].Rows[0];

                if (drTemplate != null)
                {
                    var drTemplateTableDetail = ds.Tables[1].Select("TemplateTableDetailGUID = '" + templateTableDetailGUID.ToString() + "'").FirstOrDefault();

                    if (drTemplateTableDetail != null)
                    {
                        #region Target

                        if (Convert.ToInt32(drTemplate["TargetSourceTypeId"]).Equals(AppConstant.DataSource_MsSQL) ||
                                      Convert.ToInt32(drTemplate["TargetSourceTypeId"]).Equals(AppConstant.DataSource_MySQL))
                        {
                            var targetConnection = new Connection();
                            targetConnection.DataSourceId = Convert.ToInt32(drTemplate["TargetSourceTypeId"]);
                            targetConnection.Server = Convert.ToString(drTemplate["TargetSourceServer"]);
                            targetConnection.Username = Convert.ToString(drTemplate["TargetSourceUsername"]);
                            targetConnection.Password = Convert.ToString(drTemplate["TargetSourcePassword"]);
                            targetConnection.Port = Convert.ToString(drTemplate["TargetSourcePort"]);
                            targetConnection.Database = Convert.ToString(drTemplate["TargetSourceDatabase"]);

                            targetConnection = new AppCommon(_logger).GetConnectionStatus(targetConnection);

                            if (targetConnection.IsConnectionSuccessfull)
                            {
                                var dtDependentTemplateTableDetail = ds.Tables[1].Select("TemplateTableDetailGUID <> "
                                    + "'" + templateTableDetailGUID.ToString() + "'"
                                    + " AND ExecutionOrder < "
                                    + Convert.ToInt32(drTemplateTableDetail["ExecutionOrder"]));

                                if (dtDependentTemplateTableDetail != null && dtDependentTemplateTableDetail.Count() > 0)
                                {
                                    foreach (DataRow drDependentTemplateTableDetail in dtDependentTemplateTableDetail)
                                    {
                                        var dtDependentDataTable = new DataTable();

                                        string currentTableName = Convert.ToString(drDependentTemplateTableDetail["TargetTable"]);

                                        if (targetConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                                            dtDependentDataTable = BLMsSQLConnectionSupport.GetIdentityColumn(targetConnection.ConnectionString, currentTableName);
                                        else if (targetConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                                            dtDependentDataTable = BLMySQLConnectionSupport.GetIdentityColumn(targetConnection.ConnectionString, currentTableName);

                                        if (dtDependentDataTable.Rows.Count > 0)
                                        {
                                            foreach (DataRow row in dtDependentDataTable.Rows)
                                            {
                                                string strTableIentityColumnName = currentTableName + "." + Convert.ToString(row["Name"]);

                                                if (Convert.ToInt32(drTemplate["TemplateType"]).Equals(2))
                                                    originSourceColumnDetails.Add(new TemplateColumnDetail
                                                    {
                                                        SourceColumn = toStr(strTableIentityColumnName)
                                                    });
                                                else if (Convert.ToInt32(drTemplate["TemplateType"]).Equals(3))
                                                    originSourceDependentColumnDetails.Add(new TemplateColumnDetail
                                                    {
                                                        SourceDependentColumn = toStr(strTableIentityColumnName)
                                                    });
                                            }
                                        }

                                    }
                                }

                                targetConnection.Table = Convert.ToString(drTemplateTableDetail["TargetTable"]);

                                var actualTargetSourceColumnDetails = new List<TemplateColumnDetail>();

                                var dtTargetSourceColumn = new DataTable();
                                if (targetConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                                    dtTargetSourceColumn = BLMsSQLConnectionSupport.GetAllColumns(targetConnection.ConnectionString, targetConnection.Table, false);
                                else if (targetConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                                    dtTargetSourceColumn = BLMySQLConnectionSupport.GetAllColumns(targetConnection.ConnectionString, targetConnection.Table, false);

                                if (dtTargetSourceColumn.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtTargetSourceColumn.Rows)
                                    {
                                        actualTargetSourceColumnDetails.Add(new TemplateColumnDetail
                                        {
                                            TargetColumn = toStr(Convert.ToString(row["Name"])),
                                            IsUniqueColumn = false
                                        });
                                    }
                                }
                                else
                                    response = GetNoRecordFound();

                                var dtTemplateColumnDetails = ds.Tables[2];

                                foreach (var item in actualTargetSourceColumnDetails)
                                {
                                    var drSourceColumnDetail = dtTemplateColumnDetails.AsEnumerable().FirstOrDefault(o => Convert.ToString(o["TargetColumn"]) == item.TargetColumn);
                                    if (drSourceColumnDetail != null)
                                    {
                                        targetSourceColumnDetails.Add(new TemplateColumnDetail
                                        {
                                            TemplateColumnDetailId = Convert.ToInt32(drSourceColumnDetail["TemplateColumnDetailId"]),
                                            TemplateColumnDetailGUID = Convert.ToString(drSourceColumnDetail["TemplateColumnDetailGUID"]),
                                            TemplateTableDetailId = Convert.ToInt32(drSourceColumnDetail["TemplateTableDetailId"]),
                                            SourceColumn = Convert.ToString(drSourceColumnDetail["SourceColumn"]),
                                            SourceDependentColumn = Convert.ToString(drSourceColumnDetail["SourceDependentColumn"]),
                                            SourceColumnDataType = Convert.ToString(drSourceColumnDetail["SourceColumnDataType"]),
                                            IsUniqueColumn = Convert.ToBoolean(Convert.ToInt32(drSourceColumnDetail["IsUniqueColumn"])),
                                            TargetColumn = Convert.ToString(drSourceColumnDetail["TargetColumn"]),
                                            TargetColumnDataType = Convert.ToString(drSourceColumnDetail["TargetColumnDataType"]),
                                        });
                                    }
                                    else
                                    {
                                        targetSourceColumnDetails.Add(item);
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Source

                        if (Convert.ToInt32(drTemplate["OriginSourceTypeId"]).Equals(AppConstant.DataSource_Excel))
                        {
                            if (Path.GetExtension(Convert.ToString(drTemplate["OriginSourceFileName"])).Equals(".xls")
                                || Path.GetExtension(Convert.ToString(drTemplate["OriginSourceFileName"])).Equals(".xlsx")
                                || Path.GetExtension(Convert.ToString(drTemplate["OriginSourceFileName"])).Equals(".csv")
                                )
                            {
                                var list = AppCommon.GetAllWorkSheetColumns(
                                            AppCommon.Environment.WebRootPath
                                                + "\\Document\\Template\\"
                                                + Convert.ToInt32(drTemplate["TemplateId"])
                                                + "\\" + Convert.ToString(drTemplate["OriginSourceFileName"])
                                           , Convert.ToBoolean(drTemplate["IsFirstColumnContainHeader"])
                                           , Convert.ToString(drTemplateTableDetail["SourceTable"]));

                                foreach (var item in list)
                                    originSourceColumnDetails.Add(new TemplateColumnDetail
                                    {
                                        SourceColumn = toStr(item.Name)
                                    });
                            }
                        }
                        else if (Convert.ToInt32(drTemplate["OriginSourceTypeId"]).Equals(AppConstant.DataSource_MsSQL)
                            || Convert.ToInt32(drTemplate["OriginSourceTypeId"]).Equals(AppConstant.DataSource_MySQL))
                        {
                            var originConnection = new Connection();
                            originConnection.DataSourceId = Convert.ToInt32(drTemplate["OriginSourceTypeId"]);
                            originConnection.Server = Convert.ToString(drTemplate["OriginSourceServer"]);
                            originConnection.Username = Convert.ToString(drTemplate["OriginSourceUsername"]);
                            originConnection.Password = Convert.ToString(drTemplate["OriginSourcePassword"]);
                            originConnection.Port = Convert.ToString(drTemplate["OriginSourcePort"]);
                            originConnection.Database = Convert.ToString(drTemplate["OriginSourceDatabase"]);

                            originConnection = new AppCommon(_logger).GetConnectionStatus(originConnection);

                            if (originConnection.IsConnectionSuccessfull)
                            {
                                originConnection.Table = Convert.ToString(drTemplateTableDetail["SourceTable"]);

                                var dtTargetSourceColumn = new DataTable();
                                if (originConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                                    dtTargetSourceColumn = BLMsSQLConnectionSupport.GetAllColumns(originConnection.ConnectionString, originConnection.Table);
                                else if (originConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                                    dtTargetSourceColumn = BLMySQLConnectionSupport.GetAllColumns(originConnection.ConnectionString, originConnection.Table);

                                if (dtTargetSourceColumn.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtTargetSourceColumn.Rows)
                                    {
                                        originSourceColumnDetails.Add(new TemplateColumnDetail
                                        {
                                            SourceColumn = toStr(Convert.ToString(row["Name"]))
                                        });
                                    }


                                    if ((originSourceDependentColumnDetails != null && originSourceDependentColumnDetails.Count > 0) && Convert.ToInt32(drTemplate["TemplateType"]) != 3)
                                    {
                                        foreach (var oriSrcItem in originSourceDependentColumnDetails)
                                        {
                                            originSourceColumnDetails.Add(new TemplateColumnDetail()
                                            {
                                                SourceColumn = toStr(oriSrcItem.SourceDependentColumn)
                                            });
                                        }
                                    }
                                }
                                dtTargetSourceColumn.Dispose();
                            }
                        }
                        else if (Convert.ToInt32(drTemplate["OriginSourceTypeId"]).Equals(AppConstant.DataSource_API))
                        {
                            var dtAPIOutPutParameters = ds.Tables[3];

                            foreach (DataRow item in dtAPIOutPutParameters.Rows)
                                originSourceColumnDetails.Add(new TemplateColumnDetail
                                {
                                    SourceColumn = Convert.ToString(item["KeyColumn"])
                                });
                        }

                        #endregion
                    }
                }

                return GetSuccessResponse(Tuple.Create(originSourceColumnDetails, originSourceDependentColumnDetails, targetSourceColumnDetails));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }

            _logger.LogDebug("Exited GetTemplateColumnDetailList with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetTemplateColumnDetailList");

            return response;
        }

        /// <summary>
        /// Remove All Column mapping using templateGUID
        /// </summary>
        /// <param name="templateGUID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("column/removemapping")]
        [ApiVersion("1.0")]
        public virtual Response RemoveColumnMapping(Guid templateGUID)
        {
            _logger.LogInformation("Entered RemoveColumnMapping");
            _logger.LogDebug("Entered RemoveColumnMapping with templateGUID : {0}", templateGUID);
            try
            {
                BLTemplateManager.DeleteTemplateColumnDetailFromTemplateGUID(templateGUID, 1);

                response = GetSuccessResponse("1", 1, "Template table column mapping is removed", "success");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited RemoveColumnMapping with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited RemoveColumnMapping");

            return response;
        }

        /// <summary>
        /// Insert / Update / Delete Template Filter Details
        /// </summary>
        /// <param name="form">TemplateFilterDetail object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("filter/manage")]
        [ApiVersion("1.0")]
        public virtual Response ManageTemplateFilterDetail(TemplateFilterDetail form)
        {
            _logger.LogInformation("Entered ManageTemplateFilterDetail");
            _logger.LogDebug("Entered ManageTemplateFilterDetail with Request : {0}", JsonConvert.SerializeObject(form));

            try
            {
                #region Fetch TemplateId from TemplateGUID

                var _TemplateId = new BLTemplate(form.TemplateGUID).TemplateId.GetValueOrDefault(0);

                #endregion

                #region Fetch TemplateTableDetailId from TemplateTableDetailGUID

                var _TemplateTableDetailId = new BLTemplateTableDetail(form.TemplateTableDetailGUID)
                    .TemplateTableDetailId.GetValueOrDefault(0);

                #endregion

                #region Get Old Details
                var bLTemplateFilterDetails = new List<BLTemplateFilterDetail>();

                if (_TemplateTableDetailId > 0)
                {
                    try
                    {
                        bLTemplateFilterDetails = new BLTemplateFilterDetail().Collection<BLTemplateFilterDetail>(
                         new Criteria<BLTemplateFilterDetail>()
                             .Add(Expression.Eq("TemplateId", _TemplateId))
                             .Add(Expression.Eq("TemplateTableDetailId", _TemplateTableDetailId))
                             .Add(Expression.Eq("IsDelete", false))
                             ).ToList();
                    }
                    catch (Exception e)
                    {
                        GetException(e);
                    }
                }
                #endregion Get Old Details

                var listChildDetails = new List<TemplateFilterDetail>();
                var listDeleteIdsDetails = new List<string>();

                if (IsStr(form.Details_Insert))
                    listChildDetails = JsonConvert.DeserializeObject<List<TemplateFilterDetail>>(form.Details_Insert);

                if (IsStr(form.Details_Delete))
                {
                    listDeleteIdsDetails = (toStr(form.Details_Delete)).
                        Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).
                        Select(x => x.ToString()).ToList();
                }

                #region Delete Details

                if (listDeleteIdsDetails.Count > 0)
                {
                    foreach (var item in listDeleteIdsDetails)
                    {
                        try
                        {
                            using (var templateFilterDetail = bLTemplateFilterDetails.FirstOrDefault(o => o.TemplateFilterDetailGUID.ToString() == item))
                            {
                                templateFilterDetail.IsDelete = true;
                                templateFilterDetail.UpdatedBy = form.UpdatedBy;
                                templateFilterDetail.UpdatedDate = GetDateTime();

                                templateFilterDetail.Update();
                            }
                        }
                        catch (Exception e) { GetException(e); }
                    }
                }

                #endregion Delete Details

                #region Update Details

                var listUpdateDetails = listChildDetails.Where(o => o.TemplateFilterDetailGUID != null).ToList();
                foreach (var item in listUpdateDetails)
                {
                    try
                    {
                        var templateFilterDetail = bLTemplateFilterDetails.FirstOrDefault(o => o.TemplateFilterDetailGUID == item.TemplateFilterDetailGUID.ToString());
                        templateFilterDetail.TemplateId = _TemplateId;
                        templateFilterDetail.TemplateTableDetailId = _TemplateTableDetailId;

                        templateFilterDetail.FilterColumn = toStr(item.FilterColumn);
                        templateFilterDetail.FilterOperator = item.FilterOperator;
                        templateFilterDetail.FilterValue = toStr(item.FilterValue);

                        templateFilterDetail.UpdatedDate = GetDateTime();
                        templateFilterDetail.UpdatedBy = form.UpdatedBy;

                        templateFilterDetail.Update();
                    }
                    catch (Exception e) { GetException(e); }
                }

                #endregion Update Details

                #region Insert Details

                var listInsertDetails = listChildDetails.Where(o => o.TemplateFilterDetailGUID == null).ToList();
                foreach (var item in listInsertDetails)
                {
                    try
                    {
                        var templateFilterDetail = new BLTemplateFilterDetail();

                        templateFilterDetail.TemplateId = _TemplateId;
                        templateFilterDetail.TemplateTableDetailId = _TemplateTableDetailId;

                        templateFilterDetail.FilterColumn = toStr(item.FilterColumn);
                        templateFilterDetail.FilterOperator = item.FilterOperator;
                        templateFilterDetail.FilterValue = toStr(item.FilterValue);

                        templateFilterDetail.TemplateFilterDetailGUID = GetGUID();
                        templateFilterDetail.EnteredDate = GetDateTime();
                        templateFilterDetail.EnteredBy = form.EnteredBy;
                        templateFilterDetail.IsDelete = false;
                        templateFilterDetail.IsActive = true;

                        templateFilterDetail.SaveNew();
                    }
                    catch (Exception e) { GetException(e); }
                }

                #endregion Insert Details

                #region Generate Response
                response = GetSuccessResponse(1, Message: bLTemplateFilterDetails.Count > 0
                    ? AppConstant.MessageUpdate
                    : AppConstant.MessageInsert);
                #endregion

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }

            _logger.LogDebug("Exited ManageTemplateFilterDetail with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited ManageTemplateFilterDetail");
            return response;
        }

        /// <summary>
        /// Get List of Template Filter details based on provided parameters
        /// </summary>
        /// <param name="templateGUID"></param>
        /// <param name="templateTableDetailGUID"></param>
        /// <returns>Returns List of Template filter details</returns>
        [HttpGet]
        [Route("filter/list")]
        [ApiVersion("1.0")]
        public virtual Response GetTemplateFilterDetailList(Guid templateGUID, Guid templateTableDetailGUID)
        {
            _logger.LogInformation("Entered GetTemplateFilterDetailList");
            _logger.LogDebug("Entered GetTemplateFilterDetailList with templateGUID : {0}, templateTableDetailGUID : {1}",
                templateGUID,
                templateTableDetailGUID);

            var originSourceColumnDetails = new List<TemplateFilterDetail>();
            try
            {
                var bLTemplate = new BLTemplate(templateGUID.ToString());
                if (bLTemplate != null && bLTemplate.IsDelete.Equals(false))
                {
                    using (bLTemplate)
                    {
                        #region Origin

                        if (bLTemplate.OriginSourceTypeId.GetValueOrDefault(0).Equals(AppConstant.DataSource_Excel) ||
                                          bLTemplate.OriginSourceTypeId.GetValueOrDefault(0).Equals(AppConstant.DataSource_MsSQL) ||
                                          bLTemplate.OriginSourceTypeId.GetValueOrDefault(0).Equals(AppConstant.DataSource_MySQL))
                        {
                            var OriginConnection = new Connection();
                            OriginConnection.DataSourceId = bLTemplate.OriginSourceTypeId.GetValueOrDefault(0);
                            OriginConnection.Server = bLTemplate.OriginSourceServer;
                            OriginConnection.Username = bLTemplate.OriginSourceUsername;
                            OriginConnection.Password = bLTemplate.OriginSourcePassword;
                            OriginConnection.Port = bLTemplate.OriginSourcePort;
                            OriginConnection.Database = bLTemplate.OriginSourceDatabase;

                            OriginConnection = new AppCommon(_logger).GetConnectionStatus(OriginConnection);

                            if (OriginConnection.IsConnectionSuccessfull)
                            {
                                var bLTemplateTableDetail = new BLTemplateTableDetail(templateTableDetailGUID.ToString());

                                if (bLTemplateTableDetail != null)
                                {
                                    using (bLTemplateTableDetail)
                                        OriginConnection.Table = bLTemplateTableDetail.SourceTable;

                                    var actualOriginSourceColumnDetails = new List<TemplateFilterDetail>();

                                    var dtOriginSourceColumn = new DataTable();

                                    if (OriginConnection.DataSourceId.Equals(AppConstant.DataSource_Excel))
                                    {
                                        var list = AppCommon.GetAllWorkSheetColumns(
                                           AppCommon.Environment.WebRootPath
                                               + "\\Document\\Template\\"
                                               + bLTemplate.TemplateId
                                               + "\\" + Convert.ToString(bLTemplate.OriginSourceFileName)
                                          , Convert.ToBoolean(bLTemplate.IsFirstColumnContainHeader)
                                          , Convert.ToString(bLTemplateTableDetail.SourceTable));

                                        dtOriginSourceColumn.Columns.Add("Name", typeof(string));

                                        foreach (var item in list)
                                            dtOriginSourceColumn.Rows.Add(item.Name.Trim());

                                    }
                                    else if (OriginConnection.DataSourceId.Equals(AppConstant.DataSource_MsSQL))
                                        dtOriginSourceColumn = BLMsSQLConnectionSupport.GetAllColumns(OriginConnection.ConnectionString, OriginConnection.Table);
                                    else if (OriginConnection.DataSourceId.Equals(AppConstant.DataSource_MySQL))
                                        dtOriginSourceColumn = BLMySQLConnectionSupport.GetAllColumns(OriginConnection.ConnectionString, OriginConnection.Table);

                                    if (dtOriginSourceColumn.Rows.Count > 0)
                                    {
                                        foreach (DataRow row in dtOriginSourceColumn.Rows)
                                        {
                                            actualOriginSourceColumnDetails.Add(new TemplateFilterDetail
                                            {
                                                FilterColumn = Convert.ToString(row["Name"]),
                                                FilterValue = string.Empty
                                            });
                                        }
                                    }
                                    else
                                        response = GetNoRecordFound();

                                    var bLTemplateFilterDetails = new BLTemplateFilterDetail().Collection<BLTemplateFilterDetail>(
                                        new Criteria<BLTemplateFilterDetail>()
                                        .Add(Expression.Eq("TemplateTableDetailId", bLTemplateTableDetail.TemplateTableDetailId))
                                        .Add(Expression.Eq("TemplateId", bLTemplate.TemplateId))
                                        .Add(Expression.Eq("IsDelete", false))
                                        );

                                    foreach (var item in actualOriginSourceColumnDetails)
                                    {
                                        var dbTemplateFilterDetail = bLTemplateFilterDetails.FirstOrDefault(o => o.FilterColumn.Equals(item.FilterColumn));
                                        if (dbTemplateFilterDetail != null)
                                            originSourceColumnDetails.Add(dbTemplateFilterDetail.Cast<TemplateFilterDetail>());
                                        else
                                            originSourceColumnDetails.Add(item);
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                }
                return GetSuccessResponse(Tuple.Create(originSourceColumnDetails, GetOperators()));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }
            _logger.LogDebug("Exited GetTemplateFilterDetailList with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetTemplateFilterDetailList");
            return response;
        }

        /// <summary>
        /// Get All Operator to apply in filter functionality
        /// </summary>
        /// <returns></returns>
        private List<MasterItem> GetOperators()
        {
            var operators = new List<MasterItem>();
            operators.Add(new MasterItem
            {
                Id = "1",
                Name = "Equals"
            });
            operators.Add(new MasterItem
            {
                Id = "2",
                Name = "Not Equals"
            });
            operators.Add(new MasterItem
            {
                Id = "3",
                Name = "Less Then"
            });
            operators.Add(new MasterItem
            {
                Id = "4",
                Name = "Less Then or Equals"
            });
            operators.Add(new MasterItem
            {
                Id = "5",
                Name = "Greater Then"
            });
            operators.Add(new MasterItem
            {
                Id = "6",
                Name = "Greater Then or Equals"
            });
            operators.Add(new MasterItem
            {
                Id = "7",
                Name = "In"
            });
            operators.Add(new MasterItem
            {
                Id = "8",
                Name = "Not In"
            });
            operators.Add(new MasterItem
            {
                Id = "9",
                Name = "Null"
            });
            operators.Add(new MasterItem
            {
                Id = "10",
                Name = "Not Null"
            });
            operators.Add(new MasterItem
            {
                Id = "11",
                Name = "Like"
            });

            return operators;
        }

        /// <summary>
        /// Get List of All Sheets in excel file for specified Template 
        /// </summary>
        /// <param name="templateGUID">Template GUID from where to fetch the file name</param>
        /// <returns>Returns List of Sheets</returns>
        [HttpGet]
        [Route("excel/list/sheet")]
        [ApiVersion("1.0")]
        public virtual Response GetAllExcelSheets([Required] Guid templateGUID)
        {
            _logger.LogInformation("Entered GetAllExcelSheets");
            _logger.LogDebug("Entered GetAllExcelSheets with templateGUID : {0}",
                templateGUID);

            var originSourceColumnDetails = new List<MasterItem>();
            try
            {
                var bLTemplate = new BLTemplate(templateGUID.ToString());
                if (bLTemplate != null && bLTemplate.IsDelete.Equals(false))
                {
                    using (bLTemplate)
                    {
                        if (!string.IsNullOrEmpty(bLTemplate.OriginSourceFileName))
                        {
                            var fileName = AppCommon.Environment.WebRootPath
                                + "\\Document\\Template\\"
                                + bLTemplate.TemplateId
                                + "\\"
                                + bLTemplate.OriginSourceFileName;
                            if (System.IO.File.Exists(fileName))
                            {
                                originSourceColumnDetails = AppCommon.GetAllWorksheets(
                                     fileName
                                    , bLTemplate.IsFirstColumnContainHeader.GetValueOrDefault(false));
                            }
                            else
                                return GetNoRecordFound("File Not Found!", "danger");
                        }
                    }
                }
                return GetSuccessResponse(originSourceColumnDetails);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response = GetException(e);
            }

            _logger.LogDebug("Exited GetAllExcelSheets with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited GetAllExcelSheets");
            return response;
        }

        /// <summary>
        /// Upload excel file (will be called for multiple times for each file chunk)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("excel/upload")]
        [ApiVersion("1.0")]
        public virtual Response UploadExcelFile(TemplateFileUpload request)
        {
            _logger.LogInformation("Entered UploadExcelFile");
            _logger.LogDebug("Entered UploadExcelFile with request : {0}",
                JsonConvert.SerializeObject(request));

            var IsSuccess = 0;
            try
            {
                var bLTemplate = new BLTemplate(request.TemplateGUID.ToString());

                if (bLTemplate != null && !string.IsNullOrEmpty(request.FileContent))
                {
                    var downloadPath = AppCommon.Environment.WebRootPath
                                    + "\\Document\\Template\\"
                                    + bLTemplate.TemplateId.GetValueOrDefault(0)
                                    + "\\";

                    CreateFolderIfNotExists(downloadPath);

                    downloadPath += request.FileName;

                    var array = Convert.FromBase64String(request.FileContent);

                    System.IO.File.WriteAllBytes(downloadPath, array);

                    IsSuccess = new FileUtils().MergeFile(downloadPath) ? 1 : 0;

                    if (IsSuccess.Equals(1))
                    {
                        bLTemplate.OriginSourceFileName = toStr(request.FileName.Substring(0, request.FileName.IndexOf(".part_")));

                        bLTemplate.UpdatedDate = GetDateTime();

                        bLTemplate.Update();
                    }
                }
                response = GetSuccessResponse(IsSuccess);
            }
            catch (Exception ex)
            {
                response = GetException(ex);
            }
            _logger.LogDebug("Exited UploadExcelFile with Response : {0}", JsonConvert.SerializeObject(response));
            _logger.LogInformation("Exited UploadExcelFile");
            return response;
        }
    }
}
