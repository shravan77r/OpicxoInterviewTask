using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSXImport.Portal.Models
{
    public class Template : BaseModel
    {
        public int? TemplateId { get; set; }
        public string TemplateGUID { get; set; }
        public string TemplateName { get; set; }
        public int? TemplateType { get; set; }
        public string TemplateTypeName { get; set; }
        public int? OriginSourceTypeId { get; set; }
        public int? OriginSourceAPITemplateId { get; set; }
        public int? OriginSourceFileTypeId { get; set; }
        public string OriginSourceServer { get; set; }
        public string OriginSourcePort { get; set; }
        public string OriginSourceUsername { get; set; }
        public string OriginSourcePassword { get; set; }
        public string OriginSourceDatabase { get; set; }
        public string OriginSourceFilePath { get; set; }
        public string OriginSourceFileName { get; set; }
        public bool IsFirstColumnContainHeader { get; set; }
        public int? TargetSourceTypeId { get; set; }
        public int? TargetSourceAPITemplateId { get; set; }
        public string TargetSourceServer { get; set; }
        public string TargetSourcePort { get; set; }
        public string TargetSourceUsername { get; set; }
        public string TargetSourcePassword { get; set; }
        public string TargetSourceDatabase { get; set; }
        public bool IsScheduleEnabled { get; set; }
        public int? ScheduleType { get; set; }
        public int? FrequencyType { get; set; }
        public int? FrequencyRecurrsDailyEveryDay { get; set; }
        public int? FrequencyRecurrsWeeklyEveryWeek { get; set; }
        public bool IsFrequencyRecurrsWeeklyOnMonday { get; set; }
        public bool IsFrequencyRecurrsWeeklyOnTuesday { get; set; }
        public bool IsFrequencyRecurrsWeeklyOnWednesday { get; set; }
        public bool IsFrequencyRecurrsWeeklyOnThursday { get; set; }
        public bool IsFrequencyRecurrsWeeklyOnFriday { get; set; }
        public bool IsFrequencyRecurrsWeeklyOnSaturday { get; set; }
        public bool IsFrequencyRecurrsWeeklyOnSunday { get; set; }
        public int? FrequencyRecurrsMonthlyType { get; set; }
        public int? FrequencyRecurrsMonthtlyEveryMonth { get; set; }
        public int? FrequencyRecurrsMonthtlyDayOfMonth { get; set; }
        public int? FrequencyRecurrsMonthtlyDayOfWeekOccurance { get; set; }
        public int? FrequencyRecurrsMonthtlyDayOfWeek { get; set; }
        public int? DailyFrequencyType { get; set; }
        public string DailyFrequencyTime { get; set; }
        public int? DailyFrequencyOccuranceType { get; set; }
        public int? DailyFrequencyOccuranceEvery { get; set; }
        public string DailyFrequencyOccuranceStartTime { get; set; }
        public string DailyFrequencyOccuranceEndTime { get; set; }
        public string DurationStartDate { get; set; }
        public bool IsDurationEndDateSpecified { get; set; }
        public string DurationEndDate { get; set; }

        public List<MasterItem> OriginSourceDatabaseList { get; set; }
        public List<MasterItem> OriginSourceFilePathList { get; set; }
        public List<MasterItem> OriginSourceFileNameList { get; set; }
        public List<MasterItem> TargetSourceDatabaseList { get; set; }
        public List<MasterItem> OriginSourceTableList { get; set; }
        public List<MasterItem> TargetSourceTableList { get; set; }
        public int IsValidOriginConnection { get; set; }
        public int IsValidTargetConnection { get; set; }
        public string OriginConnectionString { get; set; }
        public string TargetConnectionString { get; set; }
    }

    public class TemplateList
    {
        public string RowNo { get; set; }
        public string TemplateId { get; set; }
        public string TemplateGUID { get; set; }
        public string TemplateName { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
    }

    public class TemplateTableDetail : BaseModel
    {
        public int? TemplateTableDetailId { get; set; }
        public string TemplateTableDetailGUID { get; set; }
        public int? TemplateId { get; set; }
        public string TemplateGUID { get; set; }
        public string SourceTable { get; set; }
        public bool IsDeduplicateData { get; set; }
        public string TargetTable { get; set; }
    }

    public class TemplateTableDetailList
    {
        public string RowNo { get; set; }
        public string TemplateTableDetailGUID { get; set; }
        public string SourceTable { get; set; }
        public string IsDeduplicateData { get; set; }
        public string TargetTable { get; set; }
        public string IsColumnMapped { get; set; }
    }

    public class TemplateColumnDetail : BaseModel
    {
        public int? TemplateColumnDetailId { get; set; }
        public string TemplateColumnDetailGUID { get; set; }
        public int? TemplateId { get; set; }
        public string TemplateGUID { get; set; }
        public int? TemplateTableDetailId { get; set; }
        public string TemplateTableDetailGUID { get; set; }
        public string SourceColumn { get; set; }
        public string SourceDependentColumn { get; set; }
        public string SourceColumnDataType { get; set; }
        public bool IsUniqueColumn { get; set; }
        public string TargetColumn { get; set; }
        public string TargetColumnDataType { get; set; }
        public string Details_Insert { get; set; }
        public string Details_Delete { get; set; }
    }

    public class TemplateColumnDetailList
    {
        public string TemplateColumnDetailGUID { get; set; }
        public string SourceColumn { get; set; }
        public string SourceDependentColumn { get; set; }
        public string TargetColumn { get; set; }
        public string IsUniqueColumn { get; set; }
    }

    public class TemplateFilterDetail : BaseModel
    {
        public int? TemplateFilterDetailId { get; set; }
        public string TemplateFilterDetailGUID { get; set; }
        public int? TemplateId { get; set; }
        public string TemplateGUID { get; set; }
        public int? TemplateTableDetailId { get; set; }
        public string TemplateTableDetailGUID { get; set; }
        public string FilterColumn { get; set; }
        public int? FilterOperator { get; set; }
        public string FilterValue { get; set; }
        public string Details_Insert { get; set; }
        public string Details_Delete { get; set; }
    }

    public class TemplateFilterDetailList
    {
        public string TemplateFilterDetailGUID { get; set; }
        public string FilterColumn { get; set; }
        public string FilterOperator { get; set; }
        public string FilterValue { get; set; }
    }

    public class TemplateReOrderTableDetail
    {
        public string TemplateTableDetailGUID { get; set; }
        public int OldPosition { get; set; }
        public int NewPosition { get; set; }
    }

    public class DataTransferList
    {
        public string RowNo { get; set; }
        public string DataTransferId { get; set; }
        public string DataTransferGUID { get; set; }
        public string TransferDate { get; set; }
        public string TemplateName { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string TransferStatus { get; set; }
    }
}
