using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSXImport.Portal.Models
{
    public class WorkflowList
    {
        public string id { get; set; }
        public string name { get; set; }
        public string active { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }

    public class WorkflowExecution
    {
        public int count { get; set; }
        public List<WorkflowExecutionList> results { get; set; }
    }


    public class WorkflowExecutionList
    {
        public string id { get; set; }
        public string finished { get; set; }
        public string mode { get; set; }
        public string startedAt { get; set; }
        public string stoppedAt { get; set; }
        public string workflowId { get; set; }
        public string workflowName { get; set; }
    }
}
