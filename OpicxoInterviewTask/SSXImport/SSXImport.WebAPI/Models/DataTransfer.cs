using SSXImport.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSXImport.WebAPI.Models
{
    public class DataTransfer
    {
        public BLDataTransfer BLDatatransfer { get; set; }
        public List<BLDataTransferTableDetail> BLDataTransferTableDetails { get; set; }
        public List<BLDataTransferFilterDetail> BLDataTransferFilterDetails { get; set; }
        public List<BLDataTransferColumnDetail> BLDataTransferColumnDetails { get; set; }
        public bool IsValidConnection { get; set; }
        public Connection OriginSourceConnection { get; set; } 
        public Connection TargetSourceConnection { get; set; } 
        public string ErrorMessage { get; set; }
        public int DataTransferStatus { get; set; } = 3;
        public int TemplateType { get; set; }
    }

    public class DataTransferRequest {
        public Guid TemplateGuid { get; set; }
    }
}
