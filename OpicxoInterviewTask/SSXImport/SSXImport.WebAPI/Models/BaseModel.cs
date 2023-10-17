using SSXImport.WebAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSXImport.WebAPI.Models
{
    public class BaseModel
    {
        public bool? IsActive { get; set; } = true;
        public bool? IsDelete { get; set; } = false;
        public Int32? EnteredBy { get; set; } = 1;
        public Int32? UpdatedBy { get; set; } = 1;
        public DateTime? EnteredDate { get; set; } = AppCommon.GetDateTime();
        public DateTime? UpdatedDate { get; set; } = AppCommon.GetDateTime();
    }
}
