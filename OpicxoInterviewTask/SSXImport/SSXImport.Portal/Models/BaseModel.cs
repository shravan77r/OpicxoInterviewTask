using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSXImport.Portal.Models
{
    public class BaseModel
    {
        public int EnteredBy { get; set; } = 1;
        public int? UpdatedBy { get; set; } = 1;
        public bool IsDelete { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }

    public class MasterItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
