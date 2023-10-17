using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSXImport.WebAPI.Models
{
    public class Connection
    {
        public int DataSourceId { get; set; }
        public int OriginSourceFileTypeId { get; set; }
        public string OriginSourceFilePath { get; set; }
        public string OriginSourceFileName { get; set; }
        public string Server { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string Table { get; set; }
        public bool IsConnectionSuccessfull { get; set; }
        public string ConnectionString { get; set; }
    }
}
