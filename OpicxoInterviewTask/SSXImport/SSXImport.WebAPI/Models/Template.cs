using SSXImport.WebAPI.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSXImport.WebAPI.Models
{
    public class Template : BaseModel
    {
        public Int32? TemplateId { get; set; }
        public string TemplateGUID { get; set; }
        public Int32? TemplateType { get; set; }
        public string TemplateName { get; set; }
        public Int32? OriginSourceTypeId { get; set; }
        public Int32? OriginSourceFileTypeId { get; set; }
        public string OriginSourceServer { get; set; }
        public string OriginSourcePort { get; set; }
        public string OriginSourceUsername { get; set; }
        public string OriginSourcePassword { get; set; }
        public string OriginSourceDatabase { get; set; }
        public string OriginSourceFilePath { get; set; }
        public string OriginSourceFileName { get; set; }
        public bool? IsFirstColumnContainHeader { get; set; }
        public Int32? TargetSourceTypeId { get; set; }
        public string TargetSourceServer { get; set; }
        public string TargetSourcePort { get; set; }
        public string TargetSourceUsername { get; set; }
        public string TargetSourcePassword { get; set; }
        public string TargetSourceDatabase { get; set; }
        public bool? IsScheduleEnabled { get; set; }
        public Int32? ScheduleType { get; set; }
        public Int32? FrequencyType { get; set; }
        public Int32? FrequencyRecurrsDailyEveryDay { get; set; }
        public Int32? FrequencyRecurrsWeeklyEveryWeek { get; set; }
        public bool? IsFrequencyRecurrsWeeklyOnMonday { get; set; }
        public bool? IsFrequencyRecurrsWeeklyOnTuesday { get; set; }
        public bool? IsFrequencyRecurrsWeeklyOnWednesday { get; set; }
        public bool? IsFrequencyRecurrsWeeklyOnThursday { get; set; }
        public bool? IsFrequencyRecurrsWeeklyOnFriday { get; set; }
        public bool? IsFrequencyRecurrsWeeklyOnSaturday { get; set; }
        public bool? IsFrequencyRecurrsWeeklyOnSunday { get; set; }
        public Int32? FrequencyRecurrsMonthlyType { get; set; }
        public Int32? FrequencyRecurrsMonthtlyEveryMonth { get; set; }
        public Int32? FrequencyRecurrsMonthtlyDayOfMonth { get; set; }
        public Int32? FrequencyRecurrsMonthtlyDayOfWeekOccurance { get; set; }
        public Int32? FrequencyRecurrsMonthtlyDayOfWeek { get; set; }
        public Int32? DailyFrequencyType { get; set; }
        public string DailyFrequencyTime { get; set; }
        public Int32? DailyFrequencyOccuranceType { get; set; }
        public Int32? DailyFrequencyOccuranceEvery { get; set; }
        public string DailyFrequencyOccuranceStartTime { get; set; }
        public string DailyFrequencyOccuranceEndTime { get; set; }
        public string DurationStartDate { get; set; }
        public bool? IsDurationEndDateSpecified { get; set; }
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
        public int? OriginSourceAPITemplateId { get; set; }
        public int? TargetSourceAPITemplateId { get; set; }
    }

    public class TemplateColumnDetail : BaseModel
    {
        public Int32? TemplateColumnDetailId { get; set; }
        public string TemplateColumnDetailGUID { get; set; }
        public Int32? TemplateId { get; set; }
        public string TemplateGUID { get; set; }
        public Int32? TemplateTableDetailId { get; set; }
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

    public class TemplateFilterDetail : BaseModel
    {
        public Int32? TemplateFilterDetailId { get; set; }
        public string TemplateFilterDetailGUID { get; set; }
        public Int32? TemplateId { get; set; }
        public string TemplateGUID { get; set; }
        public Int32? TemplateTableDetailId { get; set; }
        public string TemplateTableDetailGUID { get; set; }
        public string FilterColumn { get; set; }
        public Int32? FilterOperator { get; set; }
        public string FilterValue { get; set; }
        public string Details_Insert { get; set; }
        public string Details_Delete { get; set; }
    }

    public class TemplateTableDetail : BaseModel
    {
        public Int32? TemplateTableDetailId { get; set; }
        public string TemplateTableDetailGUID { get; set; }
        public Int32? TemplateId { get; set; }
        public string TemplateGUID { get; set; }
        public bool IsDeduplicateData { get; set; }
        public string SourceTable { get; set; }
        public string TargetTable { get; set; }
    }

    public class TemplateReOrderTableDetail
    {
        public string TemplateTableDetailGUID { get; set; }
        public int OldPosition { get; set; }
        public int NewPosition { get; set; }
    }

    public class TemplateFileUpload {
        public Guid TemplateGUID { get; set; }
        public string FileContent { get; set; }
        public string FileName { get; set; }
    }
}
