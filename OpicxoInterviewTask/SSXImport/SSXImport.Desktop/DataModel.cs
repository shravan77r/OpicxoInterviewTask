using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SSXImport
{
    public class DataModel
    {
        public string Username { get; set; }
        /// <Summary>
        /// DataSourceId 
        /// 1 : Excel (.csv, .xlsx)
        /// 2 : MsSQl
        /// 3 : MySQl
        /// </Summary>
        public int SourceDataSourceId { get; set; }
        public int TargetDataSourceId { get; set; }
        public string SourceFile { get; set; }
        public string SourceServer { get; set; }
        public string SourcePort { get; set; }
        public string SourceUsername { get; set; }
        public string SourcePassword { get; set; }
        public string SourceDatabase { get; set; }
        public string SourceTable { get; set; }
        public string TargetFile { get; set; }
        public string TargetServer { get; set; }
        public string TargetPort { get; set; }
        public string TargetUsername { get; set; }
        public string TargetPassword { get; set; }
        public string TargetDatabase { get; set; }
        public string TargetTable { get; set; }
        public List<ComboBoxItem> dataMappings { get; set; }
    }

    public class DataMapping
    {
        public string SourceColumn { get; set; }
        public string TargetColumn { get; set; }
    }
}
