using Microsoft.AspNetCore.Http;

namespace SSXImport.WebAPI.Models.General
{
	public class Request
	{
		public int? SortCol { get; set; }
		public string SortDir { get; set; }
		public int? PageSize { get; set; }
		public int? PageFrom { get; set; }
		public string Keyword { get; set; }
		public string Guid { get; set; }
		public int? MemberId { get; set; }
		public string TemplateGUID { get; set; }
		public string TemplateTableDetailGUID { get; set; }
		public string Key { get; set; }
		public string Token { get; set; }
		public string FileName { get; set; }
		public string UpdatedTemplateTableRowsDetails { get; set; }
		public string FileContent { get; set; }
	}

	public class Response
	{
		public int? Status { get; set; }
		public string Message { get; set; }
		public string MessageType { get; set; }
		public int Count { get; set; }
		public object Data { get; set; }
	}
}
