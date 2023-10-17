using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SSXImport.Portal.Models
{
    public class APIModel
    {
        public class Request
        {
            public int? OrganizationId { get; set; }
            public int? SortCol { get; set; }
            public string SortDir { get; set; }
            public int? PageSize { get; set; }
            public int? PageFrom { get; set; }
            public string Keyword { get; set; }
            public string TemplateGUID { get; set; }
            public string TemplateTableDetailGUID { get; set; }
            public string Guid { get; set; }
            public int? MemberId { get; set; }
            public string FileName { get; set; }
            public string FileContent { get; set; }
        }

        public class Response<T>
        {
            public int? Status { get; set; }
            public string Message { get; set; }
            public string MessageType { get; set; }
            public int Count { get; set; }
            public T Data { get; set; }
        }
    }
}
