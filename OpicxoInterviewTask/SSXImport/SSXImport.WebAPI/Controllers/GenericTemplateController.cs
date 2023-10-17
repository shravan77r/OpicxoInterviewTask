using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SSXImport.Business;
using SSXImport.Business.ImplementationManager;
using SSXImport.WebAPI.Common;
using SSXImport.WebAPI.Models;
using SSXImport.WebAPI.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SSXImport.WebAPI.Controllers
{
	/// <summary>
	/// Used for Generic Template Related Operations
	/// </summary>
	[Route("api/generictemplate")]
	[ApiController]
	public class GenericTemplateController : BaseController
	{
		private readonly ILogger<GenericTemplateController> _logger;

		/// <summary>
		/// Initialize ILogger for logging purpose
		/// </summary>
		/// <param name="logger"></param>
		public GenericTemplateController(ILogger<GenericTemplateController> logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// Get Generic Template List bases of provided parameters
		/// </summary>
		/// <param name="sortCol"></param>
		/// <param name="sortDir"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageFrom"></param>
		/// <param name="keyword"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("list")]
		[ApiVersion("1.0")]
		public virtual Response GetGenericTemplateList(int sortCol = 0, string sortDir = "desc", int pageSize = int.MaxValue, int pageFrom = 0, string keyword = null)
		{
			_logger.LogInformation("Entered GetGenericTemplateList");
			_logger.LogDebug("Entered GetGenericTemplateList with Parameters sortCol: {0},sortDir: {1},pageSize: {2},pageFrom: {3},keyword: {4}",
			sortCol, sortDir, pageSize, pageFrom, keyword);
			try
			{
				var data = BLEmailTemplateManager.GetTemplateList(
				sortCol,
				sortDir,
				pageFrom,
				pageSize,
				keyword
				);

				var dt = data.Item2;

				response = GetDataTableResponse(dt);

				response.Count = data.Item1;
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				response = GetException(e);
			}

			_logger.LogDebug("Exited GetGenericTemplateList with response : {0}", JsonConvert.SerializeObject(response));
			_logger.LogInformation("Exited GetGenericTemplateLit");

			return response;
		}

		/// <summary>
		/// Insert / Update / Delete Generic Template
		/// </summary>
		/// <param name="form">Template object</param>
		/// <returns></returns>
		[HttpPost]
		[Route("manage")]
		[ApiVersion("1.0")]
		public virtual Response ManageGenericTemplate(GenericTemplate form)
		{
			_logger.LogInformation("Entered ManageGenericTemplate");
			_logger.LogDebug("Entered ManageGenericTemplate with Request : {0}", JsonConvert.SerializeObject(form));
			try
			{
				bool isEdit = false;
				var oldFTPFileName = string.Empty;
				Int32? _emailTemplateId = 0;
				#region Main Operation
				if (form.IsDelete == true)
				{
					BLEmailTemplateManager.DeleteTemplate(form.EmailTemplateGUID, 1);
					response = GetSuccessResponse(_emailTemplateId);
				}
				else
				{
					#region Insert / Update Operation

					_emailTemplateId = form.EmailTemplateId;

					if (_emailTemplateId > 0)
					{
						_emailTemplateId = BLEmailTemplateManager.UpdateTemplate(form.EmailTemplateName, _emailTemplateId + ".html", Convert.ToInt32(_emailTemplateId), 1, form.ObjectType, form.Object);
						_logger.LogDebug("Record Updated with Id :{0}", _emailTemplateId);
					}
					else
					{
						if (form.IsDelete == false)
						{
							_emailTemplateId = BLEmailTemplateManager.InsertTemplate(form.EmailTemplateName, form.EmailTemplateFile, 1, form.ObjectType, form.Object);
							_emailTemplateId = BLEmailTemplateManager.UpdateTemplate(form.EmailTemplateName, _emailTemplateId + ".html", Convert.ToInt32(_emailTemplateId), 1, form.ObjectType, form.Object);

						}
						_logger.LogDebug("Record Inserted with Id :{0}", _emailTemplateId);
					}
					#endregion

					response = GetSuccessResponse(_emailTemplateId);

				}
				#endregion
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				response = GetException(e);
			}
			_logger.LogDebug("Exited ManageGenericTemplate with Response : {0}", JsonConvert.SerializeObject(response));
			_logger.LogInformation("Exited ManageGenericTemplate");
			return response;
		}

		/// <summary>
		/// Get Generic Template Details By ID
		/// </summary>
		/// <param name="emailtemplateId">Template GUID based on which data will be fetched</param>
		/// <returns>Returns Template Details</returns>
		[HttpGet]
		[Route("edit")]
		[ApiVersion("1.0")]
		public virtual Response GetGenericTemplateDetails(string emailtemplateId)
		{
			_logger.LogInformation("Entered GetGenericTemplateDetails");
			_logger.LogDebug("Entered GetGenericTemplateDetails with ID : {0}", emailtemplateId);
			try
			{
				var dbDetails = BLEmailTemplateManager.GetEmailTemplateByID(emailtemplateId);
				if (dbDetails != null && dbDetails.Rows.Count > 0)
				{
					response = GetDataTableResponse(dbDetails);
				}
				else
				{
					_logger.LogDebug("Generic Template Details not found in database for GUID : {0}", emailtemplateId);
					_logger.LogError("Generic Template Details not found in database for GUID : {0}", emailtemplateId);
					response = GetNoRecordFound();
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				response = GetException(e);
			}
			_logger.LogDebug("Exited GetGenericTemplateDetails with Response : {0}", JsonConvert.SerializeObject(response));
			_logger.LogInformation("Exited GetGenericTemplateDetails");
			return response;
		}

		/// <summary>
		/// Get List Of tables of selected database
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("db/tables")]
		[ApiVersion("1.0")]
		public virtual Response GetTables(int key)
		{
			_logger.LogInformation("Entered GetTables");
			_logger.LogDebug("Entered GetTables with Request : {0}", key.ToString());

			try
			{
				var data = BLEmailTemplateManager.GetTables();
				response = GetSuccessResponse(data);
				response.Count = data.Count;
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				response = GetException(e);
			}

			_logger.LogDebug("Exited GetTables with response : {0}", JsonConvert.SerializeObject(response));
			_logger.LogInformation("Exited GetTables");

			return response;
		}


		/// <summary>
		/// Get List Of Views of selected database
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("db/views")]
		[ApiVersion("1.0")]
		public virtual Response GetViews(int key)
		{
			_logger.LogInformation("Entered GetViews");
			_logger.LogDebug("Entered GetViews with Request : {0}", key.ToString());

			try
			{
				var data = BLEmailTemplateManager.GetViews();
				response = GetSuccessResponse(data);
				response.Count = data.Count;
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				response = GetException(e);
			}

			_logger.LogDebug("Exited GetViews with response : {0}", JsonConvert.SerializeObject(response));
			_logger.LogInformation("Exited GetViews");
			return response;
		}

		/// <summary>
		/// Get List of Fields by Table or View Name
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("db/tables/fields")]
		[ApiVersion("1.0")]
		public virtual Response GetFieldsByTableName(string key)
		{
			_logger.LogInformation("Entered GetFieldsByTableName");
			_logger.LogDebug("Entered GetFieldsByTableName with Request : {0}", key);
			var request = new List<Tuple<string, object>>();
			try
			{
				var data = BLEmailTemplateManager.GetFieldsByTableName(key);
				response = GetDataTableResponse(data);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				response = GetException(e);
			}

			_logger.LogDebug("Exited GetFieldsByTableName with response : {0}", JsonConvert.SerializeObject(response));
			_logger.LogInformation("Exited GetFieldsByTableName");

			return response;
		}

		/// <summary>
		/// Get List of data by Table or View name
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("db/tables/data")]
		[ApiVersion("1.0")]
		public virtual Response GetDataByTableName(string key)
		{
			_logger.LogInformation("Entered GetDataByTableName");
			_logger.LogDebug("Entered GetDataByTableName with Request : {0}", key);
			var request = new List<Tuple<string, object>>();
			try
			{ 
				var data = BLEmailTemplateManager.GetDataByTableName(key);
				response = GetDataTableResponse(data);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				response = GetException(e);
			}

			_logger.LogDebug("Exited GetDataByTableName with response : {0}", JsonConvert.SerializeObject(response));
			_logger.LogInformation("Exited GetDataByTableName");

			return response;
		}


	}
}
