/*************************************************************
** Class generated by CodeTrigger, Version 6.3.0.5
** This class was generated on 27/08/2021 16:39:33
** Changes to this file may cause incorrect behaviour and will be lost if the code is regenerated
**************************************************************/
using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using SSXImport.Data.Interfaces;

namespace SSXImport.Data
{
	public partial class DLDataTransferErrorLogs : SSXImport_BaseData
	{
		#region member variables
		protected Int32? _dataTransferLogsId;
		protected Int32? _dataTransferId;
		protected Int32? _dataTransferTableDetailId;
		protected DateTime? _logDate;
		protected string _recordUniqueId;
		protected string _errorMessage;
		protected bool? _isActive;
		protected bool? _isDelete;
		protected Int32? _enteredBy;
		protected DateTime? _enteredDate;
		protected Int32? _updatedBy;
		protected DateTime? _updatedDate;
		#endregion

		#region class methods
		public DLDataTransferErrorLogs()
		{
		}
		///<Summary>
		///Select one row by primary key(s)
		///This method returns one row from the table DataTransferErrorLogs based on the primary key(s)
		///</Summary>
		///<returns>
		///DLDataTransferErrorLogs
		///</returns>
		///<parameters>
		///Int32? dataTransferLogsId
		///</parameters>
		public static DLDataTransferErrorLogs SelectOne(Int32? dataTransferLogsId)
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "datatransfererrorlogs_getone";
			command.CommandType = CommandType.StoredProcedure;
			MySqlConnection staticConnection = StaticSqlConnection;
			command.Connection = staticConnection;

			DataTable dt = new DataTable("DataTransferErrorLogs");
			MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(command);
			try
			{
				command.Parameters.Add(new MySqlParameter("?P_DATATRANSFERLOGSID", MySqlDbType.Int32, 0, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, (object)dataTransferLogsId?? (object)DBNull.Value));

				staticConnection.Open();
				sqlAdapter.Fill(dt);


				DLDataTransferErrorLogs retObj = null;
				if(dt.Rows.Count > 0)
				{
					retObj = new DLDataTransferErrorLogs();
					retObj._dataTransferLogsId					 = Convert.IsDBNull(dt.Rows[0]["DataTransferLogsId"]) ? (Int32?)null : (Int32?)dt.Rows[0]["DataTransferLogsId"];
					retObj._dataTransferId					 = Convert.IsDBNull(dt.Rows[0]["DataTransferId"]) ? (Int32?)null : (Int32?)dt.Rows[0]["DataTransferId"];
					retObj._dataTransferTableDetailId					 = Convert.IsDBNull(dt.Rows[0]["DataTransferTableDetailId"]) ? (Int32?)null : (Int32?)dt.Rows[0]["DataTransferTableDetailId"];
					retObj._logDate					 = Convert.IsDBNull(dt.Rows[0]["LogDate"]) ? (DateTime?)null : (DateTime?)dt.Rows[0]["LogDate"];
					retObj._recordUniqueId					 = Convert.IsDBNull(dt.Rows[0]["RecordUniqueId"]) ? null : (string)dt.Rows[0]["RecordUniqueId"];
					retObj._errorMessage					 = Convert.IsDBNull(dt.Rows[0]["ErrorMessage"]) ? null : (string)dt.Rows[0]["ErrorMessage"];
					retObj._isActive					 = Convert.IsDBNull(dt.Rows[0]["IsActive"]) ? (bool?)null : (bool?)Convert.ToBoolean(dt.Rows[0]["IsActive"]);
					retObj._isDelete					 = Convert.IsDBNull(dt.Rows[0]["IsDelete"]) ? (bool?)null : (bool?)Convert.ToBoolean(dt.Rows[0]["IsDelete"]);
					retObj._enteredBy					 = Convert.IsDBNull(dt.Rows[0]["EnteredBy"]) ? (Int32?)null : (Int32?)dt.Rows[0]["EnteredBy"];
					retObj._enteredDate					 = Convert.IsDBNull(dt.Rows[0]["EnteredDate"]) ? (DateTime?)null : (DateTime?)dt.Rows[0]["EnteredDate"];
					retObj._updatedBy					 = Convert.IsDBNull(dt.Rows[0]["UpdatedBy"]) ? (Int32?)null : (Int32?)dt.Rows[0]["UpdatedBy"];
					retObj._updatedDate					 = Convert.IsDBNull(dt.Rows[0]["UpdatedDate"]) ? (DateTime?)null : (DateTime?)dt.Rows[0]["UpdatedDate"];
				}
				return retObj;
			}
			catch(Exception ex)
			{
				Exception map = MapException(ex);
				if(map != null) throw map; 
				else throw;
			}
			finally
			{
				staticConnection.Close();
				command.Dispose();
			}
		}

		///<Summary>
		///Delete one row by primary key(s)
		///this method allows the object to delete itself from the table DataTransferErrorLogs based on its primary key
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///
		///</parameters>
		public virtual void Delete()
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "datatransfererrorlogs_deleteone";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = _connectionProvider.Connection;

			try
			{
				command.Parameters.Add(new MySqlParameter("?P_DATATRANSFERLOGSID", MySqlDbType.Int32, 0, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, (object)_dataTransferLogsId?? (object)DBNull.Value));

				command.ExecuteNonQuery();


			}
			catch(Exception ex)
			{
				Exception map = MapException(ex);
				if(map != null) throw map; 
				else throw;
			}
			finally
			{
				command.Dispose();
			}
		}

		///<Summary>
		///Insert a new row
		///This method saves a new object to the table DataTransferErrorLogs
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///
		///</parameters>
		public virtual void Insert()
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "datatransfererrorlogs_insertone";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = _connectionProvider.Connection;

			try
			{
				command.Parameters.Add(new MySqlParameter("?P_DATATRANSFERLOGSID", MySqlDbType.Int32, 0, ParameterDirection.Output, true, 10, 0, "", DataRowVersion.Proposed, _dataTransferLogsId));
				command.Parameters.Add(new MySqlParameter("?P_DATATRANSFERID", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _dataTransferId));
				command.Parameters.Add(new MySqlParameter("?P_DATATRANSFERTABLEDETAILID", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _dataTransferTableDetailId));
				command.Parameters.Add(new MySqlParameter("?P_LOGDATE", MySqlDbType.DateTime, 0, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _logDate));
				command.Parameters.Add(new MySqlParameter("?P_RECORDUNIQUEID", MySqlDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _recordUniqueId));
				command.Parameters.Add(new MySqlParameter("?P_ERRORMESSAGE", MySqlDbType.VarChar, 12000, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _errorMessage));
				command.Parameters.Add(new MySqlParameter("?P_ISACTIVE", MySqlDbType.Bit, 0, ParameterDirection.InputOutput, true, 1, 0, "", DataRowVersion.Proposed, _isActive));
				command.Parameters.Add(new MySqlParameter("?P_ISDELETE", MySqlDbType.Bit, 0, ParameterDirection.InputOutput, true, 1, 0, "", DataRowVersion.Proposed, _isDelete));
				command.Parameters.Add(new MySqlParameter("?P_ENTEREDBY", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _enteredBy));
				command.Parameters.Add(new MySqlParameter("?P_ENTEREDDATE", MySqlDbType.DateTime, 0, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _enteredDate));
				command.Parameters.Add(new MySqlParameter("?P_UPDATEDBY", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _updatedBy));
				command.Parameters.Add(new MySqlParameter("?P_UPDATEDDATE", MySqlDbType.DateTime, 0, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _updatedDate));

				command.ExecuteNonQuery();

				_dataTransferLogsId					 = Convert.IsDBNull(command.Parameters["?P_DATATRANSFERLOGSID"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_DATATRANSFERLOGSID"].Value;
				_dataTransferId					 = Convert.IsDBNull(command.Parameters["?P_DATATRANSFERID"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_DATATRANSFERID"].Value;
				_dataTransferTableDetailId					 = Convert.IsDBNull(command.Parameters["?P_DATATRANSFERTABLEDETAILID"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_DATATRANSFERTABLEDETAILID"].Value;
				_logDate					 = Convert.IsDBNull(command.Parameters["?P_LOGDATE"].Value) ? (DateTime?)null : (DateTime?)command.Parameters["?P_LOGDATE"].Value;
				_recordUniqueId					 = Convert.IsDBNull(command.Parameters["?P_RECORDUNIQUEID"].Value) ? null : (string)command.Parameters["?P_RECORDUNIQUEID"].Value;
				_errorMessage					 = Convert.IsDBNull(command.Parameters["?P_ERRORMESSAGE"].Value) ? null : (string)command.Parameters["?P_ERRORMESSAGE"].Value;
				_isActive					 = Convert.IsDBNull(command.Parameters["?P_ISACTIVE"].Value) ? (bool?)null : (bool?)Convert.ToBoolean(command.Parameters["?P_ISACTIVE"].Value);
				_isDelete					 = Convert.IsDBNull(command.Parameters["?P_ISDELETE"].Value) ? (bool?)null : (bool?)Convert.ToBoolean(command.Parameters["?P_ISDELETE"].Value);
				_enteredBy					 = Convert.IsDBNull(command.Parameters["?P_ENTEREDBY"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_ENTEREDBY"].Value;
				_enteredDate					 = Convert.IsDBNull(command.Parameters["?P_ENTEREDDATE"].Value) ? (DateTime?)null : (DateTime?)command.Parameters["?P_ENTEREDDATE"].Value;
				_updatedBy					 = Convert.IsDBNull(command.Parameters["?P_UPDATEDBY"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_UPDATEDBY"].Value;
				_updatedDate					 = Convert.IsDBNull(command.Parameters["?P_UPDATEDDATE"].Value) ? (DateTime?)null : (DateTime?)command.Parameters["?P_UPDATEDDATE"].Value;

			}
			catch(Exception ex)
			{
				Exception map = MapException(ex);
				if(map != null) throw map; 
				else throw;
			}
			finally
			{
				command.Dispose();
			}
		}

		///<Summary>
		///Select all rows
		///This method returns all data rows in the table DataTransferErrorLogs
		///</Summary>
		///<returns>
		///List-DLDataTransferErrorLogs.
		///</returns>
		///<parameters>
		///
		///</parameters>
		public static List<DLDataTransferErrorLogs> SelectAll()
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "datatransfererrorlogs_getall";
			command.CommandType = CommandType.StoredProcedure;
			MySqlConnection staticConnection = StaticSqlConnection;
			command.Connection = staticConnection;

			DataTable dt = new DataTable("DataTransferErrorLogs");
			MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(command);
			try
			{

				staticConnection.Open();
				sqlAdapter.Fill(dt);


				List<DLDataTransferErrorLogs> objList = new List<DLDataTransferErrorLogs>();
				if(dt.Rows.Count > 0)
				{
					foreach(DataRow row in dt.Rows)
					{
						DLDataTransferErrorLogs retObj = new DLDataTransferErrorLogs();
						retObj._dataTransferLogsId					 = Convert.IsDBNull(row["DataTransferLogsId"]) ? (Int32?)null : (Int32?)row["DataTransferLogsId"];
						retObj._dataTransferId					 = Convert.IsDBNull(row["DataTransferId"]) ? (Int32?)null : (Int32?)row["DataTransferId"];
						retObj._dataTransferTableDetailId					 = Convert.IsDBNull(row["DataTransferTableDetailId"]) ? (Int32?)null : (Int32?)row["DataTransferTableDetailId"];
						retObj._logDate					 = Convert.IsDBNull(row["LogDate"]) ? (DateTime?)null : (DateTime?)row["LogDate"];
						retObj._recordUniqueId					 = Convert.IsDBNull(row["RecordUniqueId"]) ? null : (string)row["RecordUniqueId"];
						retObj._errorMessage					 = Convert.IsDBNull(row["ErrorMessage"]) ? null : (string)row["ErrorMessage"];
						retObj._isActive					 = Convert.IsDBNull(row["IsActive"]) ? (bool?)null : (bool?)Convert.ToBoolean(row["IsActive"]);
						retObj._isDelete					 = Convert.IsDBNull(row["IsDelete"]) ? (bool?)null : (bool?)Convert.ToBoolean(row["IsDelete"]);
						retObj._enteredBy					 = Convert.IsDBNull(row["EnteredBy"]) ? (Int32?)null : (Int32?)row["EnteredBy"];
						retObj._enteredDate					 = Convert.IsDBNull(row["EnteredDate"]) ? (DateTime?)null : (DateTime?)row["EnteredDate"];
						retObj._updatedBy					 = Convert.IsDBNull(row["UpdatedBy"]) ? (Int32?)null : (Int32?)row["UpdatedBy"];
						retObj._updatedDate					 = Convert.IsDBNull(row["UpdatedDate"]) ? (DateTime?)null : (DateTime?)row["UpdatedDate"];
						objList.Add(retObj);
					}
				}
				return objList;
			}
			catch(Exception ex)
			{
				Exception map = MapException(ex);
				if(map != null) throw map; 
				else throw;
			}
			finally
			{
				staticConnection.Close();
				command.Dispose();
			}
		}

		///<Summary>
		///</Summary>
		///<returns>
		///Int32
		///</returns>
		///<parameters>
		///
		///</parameters>
		public static Int32 SelectAllCount()
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "datatransfererrorlogs_getallcount";
			command.CommandType = CommandType.StoredProcedure;
			MySqlConnection staticConnection = StaticSqlConnection;
			command.Connection = staticConnection;

			try
			{

				staticConnection.Open();
				Int32 retCount = (Int32)(Int64) command.ExecuteScalar();

				return retCount;
			}
			catch(Exception ex)
			{
				Exception map = MapException(ex);
				if(map != null) throw map; 
				else throw;
			}
			finally
			{
				staticConnection.Close();
				command.Dispose();
			}
		}

		///<Summary>
		///Select specific fields of all rows using criteriaquery api
		///This method returns specific fields of all data rows in the table using criteriaquery apiDataTransferErrorLogs
		///</Summary>
		///<returns>
		///IDictionary-string, IList-object..
		///</returns>
		///<parameters>
		///IList<IDataProjection> listProjection, IList<IDataCriterion> listCriterion, IList<IDataOrderBy> listOrder, IDataSkip dataSkip, IDataTake dataTake
		///</parameters>
		public static IDictionary<string, IList<object>> SelectAllByCriteriaProjection(IList<IDataProjection> listProjection, IList<IDataCriterion> listCriterion, IList<IDataOrderBy> listOrder, IDataSkip dataSkip, IDataTake dataTake)
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "datatransfererrorlogs_getbyprojection";
			command.CommandType = CommandType.StoredProcedure;
			MySqlConnection staticConnection = StaticSqlConnection;
			command.Connection = staticConnection;

			DataTable dt = new DataTable("DataTransferErrorLogs");
			MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(command);
			try
			{
				string fieldsField = GetProjections(listProjection);
				string whereClause = GetSelectionCriteria(listCriterion);
				string orderClause = GetSelectionOrder(listOrder);
				string skipClause = GetSelectionSkip(dataSkip);
				string takeClause = GetSelectionTake(dataTake);
				command.Parameters.Add(new MySqlParameter("?P_FIELDSFIELD", MySqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, (object)fieldsField?? (object)DBNull.Value));
				command.Parameters.Add(new MySqlParameter("?P_WHERECLAUSE", MySqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, (object)whereClause?? (object)DBNull.Value));
				command.Parameters.Add(new MySqlParameter("?P_ORDERCLAUSE", MySqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, (object)orderClause?? (object)DBNull.Value));
				command.Parameters.Add(new MySqlParameter("?P_SKIPCLAUSE", MySqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, (object)skipClause?? (object)DBNull.Value));
				command.Parameters.Add(new MySqlParameter("?P_TAKECLAUSE", MySqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, (object)takeClause?? (object)DBNull.Value));

				staticConnection.Open();
				sqlAdapter.Fill(dt);


				IDictionary<string, IList<object>> dict = new Dictionary<string, IList<object>>();
				foreach (IDataProjection projection in listProjection)
				{
					IList<object> lst = new List<object>();
					dict.Add(projection.Member, lst);
					foreach (DataRow row in dt.Rows)
					{
						if (string.Compare(projection.Member, "DataTransferLogsId", true) == 0) lst.Add(Convert.IsDBNull(row["DataTransferLogsId"]) ? (Int32?)null : (Int32?)row["DataTransferLogsId"]);
						if (string.Compare(projection.Member, "DataTransferId", true) == 0) lst.Add(Convert.IsDBNull(row["DataTransferId"]) ? (Int32?)null : (Int32?)row["DataTransferId"]);
						if (string.Compare(projection.Member, "DataTransferTableDetailId", true) == 0) lst.Add(Convert.IsDBNull(row["DataTransferTableDetailId"]) ? (Int32?)null : (Int32?)row["DataTransferTableDetailId"]);
						if (string.Compare(projection.Member, "LogDate", true) == 0) lst.Add(Convert.IsDBNull(row["LogDate"]) ? (DateTime?)null : (DateTime?)row["LogDate"]);
						if (string.Compare(projection.Member, "RecordUniqueId", true) == 0) lst.Add(Convert.IsDBNull(row["RecordUniqueId"]) ? null : (string)row["RecordUniqueId"]);
						if (string.Compare(projection.Member, "ErrorMessage", true) == 0) lst.Add(Convert.IsDBNull(row["ErrorMessage"]) ? null : (string)row["ErrorMessage"]);
						if (string.Compare(projection.Member, "IsActive", true) == 0) lst.Add(Convert.IsDBNull(row["IsActive"]) ? (bool?)null : (bool?)Convert.ToBoolean(row["IsActive"]));
						if (string.Compare(projection.Member, "IsDelete", true) == 0) lst.Add(Convert.IsDBNull(row["IsDelete"]) ? (bool?)null : (bool?)Convert.ToBoolean(row["IsDelete"]));
						if (string.Compare(projection.Member, "EnteredBy", true) == 0) lst.Add(Convert.IsDBNull(row["EnteredBy"]) ? (Int32?)null : (Int32?)row["EnteredBy"]);
						if (string.Compare(projection.Member, "EnteredDate", true) == 0) lst.Add(Convert.IsDBNull(row["EnteredDate"]) ? (DateTime?)null : (DateTime?)row["EnteredDate"]);
						if (string.Compare(projection.Member, "UpdatedBy", true) == 0) lst.Add(Convert.IsDBNull(row["UpdatedBy"]) ? (Int32?)null : (Int32?)row["UpdatedBy"]);
						if (string.Compare(projection.Member, "UpdatedDate", true) == 0) lst.Add(Convert.IsDBNull(row["UpdatedDate"]) ? (DateTime?)null : (DateTime?)row["UpdatedDate"]);
					}
				}
				return dict;
			}
			catch(Exception ex)
			{
				Exception map = MapException(ex);
				if(map != null) throw map; 
				else throw;
			}
			finally
			{
				staticConnection.Close();
				command.Dispose();
			}
		}

		///<Summary>
		///Select all rows by filter criteria
		///This method returns all data rows in the table using criteriaquery api DataTransferErrorLogs
		///</Summary>
		///<returns>
		///List-DLDataTransferErrorLogs.
		///</returns>
		///<parameters>
		///IList<IDataCriterion> listCriterion, IList<IDataOrderBy> listOrder, IDataSkip dataSkip, IDataTake dataTake
		///</parameters>
		public static List<DLDataTransferErrorLogs> SelectAllByCriteria(IList<IDataCriterion> listCriterion, IList<IDataOrderBy> listOrder, IDataSkip dataSkip, IDataTake dataTake)
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "datatransfererrorlogs_getbycriteria";
			command.CommandType = CommandType.StoredProcedure;
			MySqlConnection staticConnection = StaticSqlConnection;
			command.Connection = staticConnection;

			DataTable dt = new DataTable("DataTransferErrorLogs");
			MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(command);
			try
			{
				string whereClause = GetSelectionCriteria(listCriterion);
				string orderClause = GetSelectionOrder(listOrder);
				string skipClause = GetSelectionSkip(dataSkip);
				string takeClause = GetSelectionTake(dataTake);
				command.Parameters.Add(new MySqlParameter("?P_WHERECLAUSE", MySqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, (object)whereClause?? (object)DBNull.Value));
				command.Parameters.Add(new MySqlParameter("?P_ORDERCLAUSE", MySqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, (object)orderClause?? (object)DBNull.Value));
				command.Parameters.Add(new MySqlParameter("?P_SKIPCLAUSE", MySqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, (object)skipClause?? (object)DBNull.Value));
				command.Parameters.Add(new MySqlParameter("?P_TAKECLAUSE", MySqlDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, (object)takeClause?? (object)DBNull.Value));

				staticConnection.Open();
				sqlAdapter.Fill(dt);


				List<DLDataTransferErrorLogs> objList = new List<DLDataTransferErrorLogs>();
				if(dt.Rows.Count > 0)
				{
					foreach(DataRow row in dt.Rows)
					{
						DLDataTransferErrorLogs retObj = new DLDataTransferErrorLogs();
						retObj._dataTransferLogsId					 = Convert.IsDBNull(row["DataTransferLogsId"]) ? (Int32?)null : (Int32?)row["DataTransferLogsId"];
						retObj._dataTransferId					 = Convert.IsDBNull(row["DataTransferId"]) ? (Int32?)null : (Int32?)row["DataTransferId"];
						retObj._dataTransferTableDetailId					 = Convert.IsDBNull(row["DataTransferTableDetailId"]) ? (Int32?)null : (Int32?)row["DataTransferTableDetailId"];
						retObj._logDate					 = Convert.IsDBNull(row["LogDate"]) ? (DateTime?)null : (DateTime?)row["LogDate"];
						retObj._recordUniqueId					 = Convert.IsDBNull(row["RecordUniqueId"]) ? null : (string)row["RecordUniqueId"];
						retObj._errorMessage					 = Convert.IsDBNull(row["ErrorMessage"]) ? null : (string)row["ErrorMessage"];
						retObj._isActive					 = Convert.IsDBNull(row["IsActive"]) ? (bool?)null : (bool?)Convert.ToBoolean(row["IsActive"]);
						retObj._isDelete					 = Convert.IsDBNull(row["IsDelete"]) ? (bool?)null : (bool?)Convert.ToBoolean(row["IsDelete"]);
						retObj._enteredBy					 = Convert.IsDBNull(row["EnteredBy"]) ? (Int32?)null : (Int32?)row["EnteredBy"];
						retObj._enteredDate					 = Convert.IsDBNull(row["EnteredDate"]) ? (DateTime?)null : (DateTime?)row["EnteredDate"];
						retObj._updatedBy					 = Convert.IsDBNull(row["UpdatedBy"]) ? (Int32?)null : (Int32?)row["UpdatedBy"];
						retObj._updatedDate					 = Convert.IsDBNull(row["UpdatedDate"]) ? (DateTime?)null : (DateTime?)row["UpdatedDate"];
						objList.Add(retObj);
					}
				}
				return objList;
			}
			catch(Exception ex)
			{
				Exception map = MapException(ex);
				if(map != null) throw map; 
				else throw;
			}
			finally
			{
				staticConnection.Close();
				command.Dispose();
			}
		}

		///<Summary>
		///Select count of all rows using criteriaquery api
		///This method returns all data rows in the table using criteriaquery api DataTransferErrorLogs
		///</Summary>
		///<returns>
		///Int32
		///</returns>
		///<parameters>
		///IList<IDataCriterion> listCriterion
		///</parameters>
		public static Int32 SelectAllByCriteriaCount(IList<IDataCriterion> listCriterion)
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "datatransfererrorlogs_getbycriteriacount";
			command.CommandType = CommandType.StoredProcedure;
			MySqlConnection staticConnection = StaticSqlConnection;
			command.Connection = staticConnection;

			try
			{
				string whereClause = GetSelectionCriteria(listCriterion);
				command.Parameters.Add(new MySqlParameter("?P_WHERECLAUSE", MySqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, (object)whereClause?? (object)DBNull.Value));

				staticConnection.Open();
				Int32 retCount = (Int32)(Int64) command.ExecuteScalar();

				return retCount;
			}
			catch(Exception ex)
			{
				Exception map = MapException(ex);
				if(map != null) throw map; 
				else throw;
			}
			finally
			{
				staticConnection.Close();
				command.Dispose();
			}
		}

		///<Summary>
		///Update one row by primary key(s)
		///This method allows the object to update itself in the table DataTransferErrorLogs based on its primary key(s)
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///
		///</parameters>
		public virtual void Update()
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "datatransfererrorlogs_updateone";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = _connectionProvider.Connection;

			try
			{
				command.Parameters.Add(new MySqlParameter("?P_DATATRANSFERLOGSID", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _dataTransferLogsId));
				command.Parameters.Add(new MySqlParameter("?P_DATATRANSFERID", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _dataTransferId));
				command.Parameters.Add(new MySqlParameter("?P_DATATRANSFERTABLEDETAILID", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _dataTransferTableDetailId));
				command.Parameters.Add(new MySqlParameter("?P_LOGDATE", MySqlDbType.DateTime, 0, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _logDate));
				command.Parameters.Add(new MySqlParameter("?P_RECORDUNIQUEID", MySqlDbType.VarChar, 300, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _recordUniqueId));
				command.Parameters.Add(new MySqlParameter("?P_ERRORMESSAGE", MySqlDbType.VarChar, 12000, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _errorMessage));
				command.Parameters.Add(new MySqlParameter("?P_ISACTIVE", MySqlDbType.Bit, 0, ParameterDirection.InputOutput, true, 1, 0, "", DataRowVersion.Proposed, _isActive));
				command.Parameters.Add(new MySqlParameter("?P_ISDELETE", MySqlDbType.Bit, 0, ParameterDirection.InputOutput, true, 1, 0, "", DataRowVersion.Proposed, _isDelete));
				command.Parameters.Add(new MySqlParameter("?P_ENTEREDBY", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _enteredBy));
				command.Parameters.Add(new MySqlParameter("?P_ENTEREDDATE", MySqlDbType.DateTime, 0, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _enteredDate));
				command.Parameters.Add(new MySqlParameter("?P_UPDATEDBY", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _updatedBy));
				command.Parameters.Add(new MySqlParameter("?P_UPDATEDDATE", MySqlDbType.DateTime, 0, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _updatedDate));

				command.ExecuteNonQuery();

				_dataTransferLogsId					 = Convert.IsDBNull(command.Parameters["?P_DATATRANSFERLOGSID"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_DATATRANSFERLOGSID"].Value;
				_dataTransferId					 = Convert.IsDBNull(command.Parameters["?P_DATATRANSFERID"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_DATATRANSFERID"].Value;
				_dataTransferTableDetailId					 = Convert.IsDBNull(command.Parameters["?P_DATATRANSFERTABLEDETAILID"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_DATATRANSFERTABLEDETAILID"].Value;
				_logDate					 = Convert.IsDBNull(command.Parameters["?P_LOGDATE"].Value) ? (DateTime?)null : (DateTime?)command.Parameters["?P_LOGDATE"].Value;
				_recordUniqueId					 = Convert.IsDBNull(command.Parameters["?P_RECORDUNIQUEID"].Value) ? null : (string)command.Parameters["?P_RECORDUNIQUEID"].Value;
				_errorMessage					 = Convert.IsDBNull(command.Parameters["?P_ERRORMESSAGE"].Value) ? null : (string)command.Parameters["?P_ERRORMESSAGE"].Value;
				_isActive					 = Convert.IsDBNull(command.Parameters["?P_ISACTIVE"].Value) ? (bool?)null : (bool?)Convert.ToBoolean(command.Parameters["?P_ISACTIVE"].Value);
				_isDelete					 = Convert.IsDBNull(command.Parameters["?P_ISDELETE"].Value) ? (bool?)null : (bool?)Convert.ToBoolean(command.Parameters["?P_ISDELETE"].Value);
				_enteredBy					 = Convert.IsDBNull(command.Parameters["?P_ENTEREDBY"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_ENTEREDBY"].Value;
				_enteredDate					 = Convert.IsDBNull(command.Parameters["?P_ENTEREDDATE"].Value) ? (DateTime?)null : (DateTime?)command.Parameters["?P_ENTEREDDATE"].Value;
				_updatedBy					 = Convert.IsDBNull(command.Parameters["?P_UPDATEDBY"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_UPDATEDBY"].Value;
				_updatedDate					 = Convert.IsDBNull(command.Parameters["?P_UPDATEDDATE"].Value) ? (DateTime?)null : (DateTime?)command.Parameters["?P_UPDATEDDATE"].Value;

			}
			catch(Exception ex)
			{
				Exception map = MapException(ex);
				if(map != null) throw map; 
				else throw;
			}
			finally
			{
				command.Dispose();
			}
		}

		#endregion

		#region member properties
		public Int32? DataTransferLogsId
		{
			get
			{
				return _dataTransferLogsId;
			}
			set
			{
				_dataTransferLogsId = value;
			}
		}
		public Int32? DataTransferId
		{
			get
			{
				return _dataTransferId;
			}
			set
			{
				_dataTransferId = value;
			}
		}
		public Int32? DataTransferTableDetailId
		{
			get
			{
				return _dataTransferTableDetailId;
			}
			set
			{
				_dataTransferTableDetailId = value;
			}
		}
		public DateTime? LogDate
		{
			get
			{
				return _logDate;
			}
			set
			{
				_logDate = value;
			}
		}
		public string RecordUniqueId
		{
			get
			{
				return _recordUniqueId;
			}
			set
			{
				_recordUniqueId = value;
			}
		}
		public string ErrorMessage
		{
			get
			{
				return _errorMessage;
			}
			set
			{
				_errorMessage = value;
			}
		}
		public bool? IsActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				_isActive = value;
			}
		}
		public bool? IsDelete
		{
			get
			{
				return _isDelete;
			}
			set
			{
				_isDelete = value;
			}
		}
		public Int32? EnteredBy
		{
			get
			{
				return _enteredBy;
			}
			set
			{
				_enteredBy = value;
			}
		}
		public DateTime? EnteredDate
		{
			get
			{
				return _enteredDate;
			}
			set
			{
				_enteredDate = value;
			}
		}
		public Int32? UpdatedBy
		{
			get
			{
				return _updatedBy;
			}
			set
			{
				_updatedBy = value;
			}
		}
		public DateTime? UpdatedDate
		{
			get
			{
				return _updatedDate;
			}
			set
			{
				_updatedDate = value;
			}
		}
		#endregion
	}
}