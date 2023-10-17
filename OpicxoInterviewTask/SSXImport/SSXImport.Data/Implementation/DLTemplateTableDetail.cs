/*************************************************************
** Class generated by CodeTrigger, Version 6.3.0.5
** This class was generated on 27/08/2021 16:39:34
** Changes to this file may cause incorrect behaviour and will be lost if the code is regenerated
**************************************************************/
using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using SSXImport.Data.Interfaces;

namespace SSXImport.Data
{
	public partial class DLTemplateTableDetail : SSXImport_BaseData
	{
		#region member variables
		protected Int32? _templateTableDetailId;
		protected string _templateTableDetailGUID;
		protected Int32? _templateId;
		protected string _sourceTable;
		protected string _targetTable;
		protected bool? _isDeduplicateData;
		protected Int32? _executionOrder;
		protected bool? _isActive;
		protected bool? _isDelete;
		protected Int32? _enteredBy;
		protected DateTime? _enteredDate;
		protected Int32? _updatedBy;
		protected DateTime? _updatedDate;
		#endregion

		#region class methods
		public DLTemplateTableDetail()
		{
		}
		///<Summary>
		///Select one row by primary key(s)
		///This method returns one row from the table TemplateTableDetail based on the primary key(s)
		///</Summary>
		///<returns>
		///DLTemplateTableDetail
		///</returns>
		///<parameters>
		///Int32? templateTableDetailId
		///</parameters>
		public static DLTemplateTableDetail SelectOne(Int32? templateTableDetailId)
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "templatetabledetail_getone";
			command.CommandType = CommandType.StoredProcedure;
			MySqlConnection staticConnection = StaticSqlConnection;
			command.Connection = staticConnection;

			DataTable dt = new DataTable("TemplateTableDetail");
			MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(command);
			try
			{
				command.Parameters.Add(new MySqlParameter("?P_TEMPLATETABLEDETAILID", MySqlDbType.Int32, 0, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, (object)templateTableDetailId?? (object)DBNull.Value));

				staticConnection.Open();
				sqlAdapter.Fill(dt);


				DLTemplateTableDetail retObj = null;
				if(dt.Rows.Count > 0)
				{
					retObj = new DLTemplateTableDetail();
					retObj._templateTableDetailId					 = Convert.IsDBNull(dt.Rows[0]["TemplateTableDetailId"]) ? (Int32?)null : (Int32?)dt.Rows[0]["TemplateTableDetailId"];
					retObj._templateTableDetailGUID					 = Convert.IsDBNull(dt.Rows[0]["TemplateTableDetailGUID"]) ? null : (string)dt.Rows[0]["TemplateTableDetailGUID"];
					retObj._templateId					 = Convert.IsDBNull(dt.Rows[0]["TemplateId"]) ? (Int32?)null : (Int32?)dt.Rows[0]["TemplateId"];
					retObj._sourceTable					 = Convert.IsDBNull(dt.Rows[0]["SourceTable"]) ? null : (string)dt.Rows[0]["SourceTable"];
					retObj._targetTable					 = Convert.IsDBNull(dt.Rows[0]["TargetTable"]) ? null : (string)dt.Rows[0]["TargetTable"];
					retObj._isDeduplicateData					 = Convert.IsDBNull(dt.Rows[0]["IsDeduplicateData"]) ? (bool?)null : (bool?)Convert.ToBoolean(dt.Rows[0]["IsDeduplicateData"]);
					retObj._executionOrder					 = Convert.IsDBNull(dt.Rows[0]["ExecutionOrder"]) ? (Int32?)null : (Int32?)dt.Rows[0]["ExecutionOrder"];
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
		///this method allows the object to delete itself from the table TemplateTableDetail based on its primary key
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
			command.CommandText = "templatetabledetail_deleteone";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = _connectionProvider.Connection;

			try
			{
				command.Parameters.Add(new MySqlParameter("?P_TEMPLATETABLEDETAILID", MySqlDbType.Int32, 0, ParameterDirection.Input, false, 10, 0, "", DataRowVersion.Proposed, (object)_templateTableDetailId?? (object)DBNull.Value));

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
		///Select one row by unique constraint
		///This method returns one row from the table TemplateTableDetail based on a unique constraint
		///</Summary>
		///<returns>
		///DLTemplateTableDetail
		///</returns>
		///<parameters>
		///string templateTableDetailGUID
		///</parameters>
		public static DLTemplateTableDetail SelectOneByTemplateTableDetailGUID(string templateTableDetailGUID)
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "templatetabledetail_getonebytemplatetabledetailguid";
			command.CommandType = CommandType.StoredProcedure;
			MySqlConnection staticConnection = StaticSqlConnection;
			command.Connection = staticConnection;

			DataTable dt = new DataTable("TemplateTableDetail");
			MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(command);
			try
			{
				command.Parameters.Add(new MySqlParameter("?P_TEMPLATETABLEDETAILGUID", MySqlDbType.VarChar, 64, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, (object)templateTableDetailGUID?? (object)DBNull.Value));

				staticConnection.Open();
				sqlAdapter.Fill(dt);


				DLTemplateTableDetail retObj = null;
				if(dt.Rows.Count > 0)
				{
					retObj = new DLTemplateTableDetail();
					retObj._templateTableDetailId					 = Convert.IsDBNull(dt.Rows[0]["TemplateTableDetailId"]) ? (Int32?)null : (Int32?)dt.Rows[0]["TemplateTableDetailId"];
					retObj._templateTableDetailGUID					 = Convert.IsDBNull(dt.Rows[0]["TemplateTableDetailGUID"]) ? null : (string)dt.Rows[0]["TemplateTableDetailGUID"];
					retObj._templateId					 = Convert.IsDBNull(dt.Rows[0]["TemplateId"]) ? (Int32?)null : (Int32?)dt.Rows[0]["TemplateId"];
					retObj._sourceTable					 = Convert.IsDBNull(dt.Rows[0]["SourceTable"]) ? null : (string)dt.Rows[0]["SourceTable"];
					retObj._targetTable					 = Convert.IsDBNull(dt.Rows[0]["TargetTable"]) ? null : (string)dt.Rows[0]["TargetTable"];
					retObj._isDeduplicateData					 = Convert.IsDBNull(dt.Rows[0]["IsDeduplicateData"]) ? (bool?)null : (bool?)Convert.ToBoolean(dt.Rows[0]["IsDeduplicateData"]);
					retObj._executionOrder					 = Convert.IsDBNull(dt.Rows[0]["ExecutionOrder"]) ? (Int32?)null : (Int32?)dt.Rows[0]["ExecutionOrder"];
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
		///Delete one row by unique constraint
		///This method deletes one row from the table TemplateTableDetail based on a unique constraint
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///string templateTableDetailGUID
		///</parameters>
		public virtual void DeleteOneByTemplateTableDetailGUID(string templateTableDetailGUID)
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "templatetabledetail_deleteonebytemplatetabledetailguid";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = _connectionProvider.Connection;

			try
			{
				command.Parameters.Add(new MySqlParameter("?P_TEMPLATETABLEDETAILGUID", MySqlDbType.VarChar, 64, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, (object)_templateTableDetailGUID?? (object)DBNull.Value));

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
		///This method saves a new object to the table TemplateTableDetail
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
			command.CommandText = "templatetabledetail_insertone";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = _connectionProvider.Connection;

			try
			{
				command.Parameters.Add(new MySqlParameter("?P_TEMPLATETABLEDETAILID", MySqlDbType.Int32, 0, ParameterDirection.Output, true, 10, 0, "", DataRowVersion.Proposed, _templateTableDetailId));
				command.Parameters.Add(new MySqlParameter("?P_TEMPLATETABLEDETAILGUID", MySqlDbType.VarChar, 64, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _templateTableDetailGUID));
				command.Parameters.Add(new MySqlParameter("?P_TEMPLATEID", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _templateId));
				command.Parameters.Add(new MySqlParameter("?P_SOURCETABLE", MySqlDbType.VarChar, 255, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _sourceTable));
				command.Parameters.Add(new MySqlParameter("?P_TARGETTABLE", MySqlDbType.VarChar, 255, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _targetTable));
				command.Parameters.Add(new MySqlParameter("?P_ISDEDUPLICATEDATA", MySqlDbType.Bit, 0, ParameterDirection.InputOutput, true, 1, 0, "", DataRowVersion.Proposed, _isDeduplicateData));
				command.Parameters.Add(new MySqlParameter("?P_EXECUTIONORDER", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _executionOrder));
				command.Parameters.Add(new MySqlParameter("?P_ISACTIVE", MySqlDbType.Bit, 0, ParameterDirection.InputOutput, true, 1, 0, "", DataRowVersion.Proposed, _isActive));
				command.Parameters.Add(new MySqlParameter("?P_ISDELETE", MySqlDbType.Bit, 0, ParameterDirection.InputOutput, true, 1, 0, "", DataRowVersion.Proposed, _isDelete));
				command.Parameters.Add(new MySqlParameter("?P_ENTEREDBY", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _enteredBy));
				command.Parameters.Add(new MySqlParameter("?P_ENTEREDDATE", MySqlDbType.DateTime, 0, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _enteredDate));
				command.Parameters.Add(new MySqlParameter("?P_UPDATEDBY", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _updatedBy));
				command.Parameters.Add(new MySqlParameter("?P_UPDATEDDATE", MySqlDbType.DateTime, 0, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _updatedDate));

				command.ExecuteNonQuery();

				_templateTableDetailId					 = Convert.IsDBNull(command.Parameters["?P_TEMPLATETABLEDETAILID"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_TEMPLATETABLEDETAILID"].Value;
				_templateTableDetailGUID					 = Convert.IsDBNull(command.Parameters["?P_TEMPLATETABLEDETAILGUID"].Value) ? null : (string)command.Parameters["?P_TEMPLATETABLEDETAILGUID"].Value;
				_templateId					 = Convert.IsDBNull(command.Parameters["?P_TEMPLATEID"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_TEMPLATEID"].Value;
				_sourceTable					 = Convert.IsDBNull(command.Parameters["?P_SOURCETABLE"].Value) ? null : (string)command.Parameters["?P_SOURCETABLE"].Value;
				_targetTable					 = Convert.IsDBNull(command.Parameters["?P_TARGETTABLE"].Value) ? null : (string)command.Parameters["?P_TARGETTABLE"].Value;
				_isDeduplicateData					 = Convert.IsDBNull(command.Parameters["?P_ISDEDUPLICATEDATA"].Value) ? (bool?)null : (bool?)Convert.ToBoolean(command.Parameters["?P_ISDEDUPLICATEDATA"].Value);
				_executionOrder					 = Convert.IsDBNull(command.Parameters["?P_EXECUTIONORDER"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_EXECUTIONORDER"].Value;
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
		///This method returns all data rows in the table TemplateTableDetail
		///</Summary>
		///<returns>
		///List-DLTemplateTableDetail.
		///</returns>
		///<parameters>
		///
		///</parameters>
		public static List<DLTemplateTableDetail> SelectAll()
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "templatetabledetail_getall";
			command.CommandType = CommandType.StoredProcedure;
			MySqlConnection staticConnection = StaticSqlConnection;
			command.Connection = staticConnection;

			DataTable dt = new DataTable("TemplateTableDetail");
			MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(command);
			try
			{

				staticConnection.Open();
				sqlAdapter.Fill(dt);


				List<DLTemplateTableDetail> objList = new List<DLTemplateTableDetail>();
				if(dt.Rows.Count > 0)
				{
					foreach(DataRow row in dt.Rows)
					{
						DLTemplateTableDetail retObj = new DLTemplateTableDetail();
						retObj._templateTableDetailId					 = Convert.IsDBNull(row["TemplateTableDetailId"]) ? (Int32?)null : (Int32?)row["TemplateTableDetailId"];
						retObj._templateTableDetailGUID					 = Convert.IsDBNull(row["TemplateTableDetailGUID"]) ? null : (string)row["TemplateTableDetailGUID"];
						retObj._templateId					 = Convert.IsDBNull(row["TemplateId"]) ? (Int32?)null : (Int32?)row["TemplateId"];
						retObj._sourceTable					 = Convert.IsDBNull(row["SourceTable"]) ? null : (string)row["SourceTable"];
						retObj._targetTable					 = Convert.IsDBNull(row["TargetTable"]) ? null : (string)row["TargetTable"];
						retObj._isDeduplicateData					 = Convert.IsDBNull(row["IsDeduplicateData"]) ? (bool?)null : (bool?)Convert.ToBoolean(row["IsDeduplicateData"]);
						retObj._executionOrder					 = Convert.IsDBNull(row["ExecutionOrder"]) ? (Int32?)null : (Int32?)row["ExecutionOrder"];
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
			command.CommandText = "templatetabledetail_getallcount";
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
		///This method returns specific fields of all data rows in the table using criteriaquery apiTemplateTableDetail
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
			command.CommandText = "templatetabledetail_getbyprojection";
			command.CommandType = CommandType.StoredProcedure;
			MySqlConnection staticConnection = StaticSqlConnection;
			command.Connection = staticConnection;

			DataTable dt = new DataTable("TemplateTableDetail");
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
						if (string.Compare(projection.Member, "TemplateTableDetailId", true) == 0) lst.Add(Convert.IsDBNull(row["TemplateTableDetailId"]) ? (Int32?)null : (Int32?)row["TemplateTableDetailId"]);
						if (string.Compare(projection.Member, "TemplateTableDetailGUID", true) == 0) lst.Add(Convert.IsDBNull(row["TemplateTableDetailGUID"]) ? null : (string)row["TemplateTableDetailGUID"]);
						if (string.Compare(projection.Member, "TemplateId", true) == 0) lst.Add(Convert.IsDBNull(row["TemplateId"]) ? (Int32?)null : (Int32?)row["TemplateId"]);
						if (string.Compare(projection.Member, "SourceTable", true) == 0) lst.Add(Convert.IsDBNull(row["SourceTable"]) ? null : (string)row["SourceTable"]);
						if (string.Compare(projection.Member, "TargetTable", true) == 0) lst.Add(Convert.IsDBNull(row["TargetTable"]) ? null : (string)row["TargetTable"]);
						if (string.Compare(projection.Member, "IsDeduplicateData", true) == 0) lst.Add(Convert.IsDBNull(row["IsDeduplicateData"]) ? (bool?)null : (bool?)Convert.ToBoolean(row["IsDeduplicateData"]));
						if (string.Compare(projection.Member, "ExecutionOrder", true) == 0) lst.Add(Convert.IsDBNull(row["ExecutionOrder"]) ? (Int32?)null : (Int32?)row["ExecutionOrder"]);
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
		///This method returns all data rows in the table using criteriaquery api TemplateTableDetail
		///</Summary>
		///<returns>
		///List-DLTemplateTableDetail.
		///</returns>
		///<parameters>
		///IList<IDataCriterion> listCriterion, IList<IDataOrderBy> listOrder, IDataSkip dataSkip, IDataTake dataTake
		///</parameters>
		public static List<DLTemplateTableDetail> SelectAllByCriteria(IList<IDataCriterion> listCriterion, IList<IDataOrderBy> listOrder, IDataSkip dataSkip, IDataTake dataTake)
		{
			MySqlCommand	command = new MySqlCommand();
			command.CommandText = "templatetabledetail_getbycriteria";
			command.CommandType = CommandType.StoredProcedure;
			MySqlConnection staticConnection = StaticSqlConnection;
			command.Connection = staticConnection;

			DataTable dt = new DataTable("TemplateTableDetail");
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


				List<DLTemplateTableDetail> objList = new List<DLTemplateTableDetail>();
				if(dt.Rows.Count > 0)
				{
					foreach(DataRow row in dt.Rows)
					{
						DLTemplateTableDetail retObj = new DLTemplateTableDetail();
						retObj._templateTableDetailId					 = Convert.IsDBNull(row["TemplateTableDetailId"]) ? (Int32?)null : (Int32?)row["TemplateTableDetailId"];
						retObj._templateTableDetailGUID					 = Convert.IsDBNull(row["TemplateTableDetailGUID"]) ? null : (string)row["TemplateTableDetailGUID"];
						retObj._templateId					 = Convert.IsDBNull(row["TemplateId"]) ? (Int32?)null : (Int32?)row["TemplateId"];
						retObj._sourceTable					 = Convert.IsDBNull(row["SourceTable"]) ? null : (string)row["SourceTable"];
						retObj._targetTable					 = Convert.IsDBNull(row["TargetTable"]) ? null : (string)row["TargetTable"];
						retObj._isDeduplicateData					 = Convert.IsDBNull(row["IsDeduplicateData"]) ? (bool?)null : (bool?)Convert.ToBoolean(row["IsDeduplicateData"]);
						retObj._executionOrder					 = Convert.IsDBNull(row["ExecutionOrder"]) ? (Int32?)null : (Int32?)row["ExecutionOrder"];
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
		///This method returns all data rows in the table using criteriaquery api TemplateTableDetail
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
			command.CommandText = "templatetabledetail_getbycriteriacount";
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
		///This method allows the object to update itself in the table TemplateTableDetail based on its primary key(s)
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
			command.CommandText = "templatetabledetail_updateone";
			command.CommandType = CommandType.StoredProcedure;
			command.Connection = _connectionProvider.Connection;

			try
			{
				command.Parameters.Add(new MySqlParameter("?P_TEMPLATETABLEDETAILID", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _templateTableDetailId));
				command.Parameters.Add(new MySqlParameter("?P_TEMPLATETABLEDETAILGUID", MySqlDbType.VarChar, 64, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _templateTableDetailGUID));
				command.Parameters.Add(new MySqlParameter("?P_TEMPLATEID", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _templateId));
				command.Parameters.Add(new MySqlParameter("?P_SOURCETABLE", MySqlDbType.VarChar, 255, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _sourceTable));
				command.Parameters.Add(new MySqlParameter("?P_TARGETTABLE", MySqlDbType.VarChar, 255, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _targetTable));
				command.Parameters.Add(new MySqlParameter("?P_ISDEDUPLICATEDATA", MySqlDbType.Bit, 0, ParameterDirection.InputOutput, true, 1, 0, "", DataRowVersion.Proposed, _isDeduplicateData));
				command.Parameters.Add(new MySqlParameter("?P_EXECUTIONORDER", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _executionOrder));
				command.Parameters.Add(new MySqlParameter("?P_ISACTIVE", MySqlDbType.Bit, 0, ParameterDirection.InputOutput, true, 1, 0, "", DataRowVersion.Proposed, _isActive));
				command.Parameters.Add(new MySqlParameter("?P_ISDELETE", MySqlDbType.Bit, 0, ParameterDirection.InputOutput, true, 1, 0, "", DataRowVersion.Proposed, _isDelete));
				command.Parameters.Add(new MySqlParameter("?P_ENTEREDBY", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _enteredBy));
				command.Parameters.Add(new MySqlParameter("?P_ENTEREDDATE", MySqlDbType.DateTime, 0, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _enteredDate));
				command.Parameters.Add(new MySqlParameter("?P_UPDATEDBY", MySqlDbType.Int32, 0, ParameterDirection.InputOutput, true, 10, 0, "", DataRowVersion.Proposed, _updatedBy));
				command.Parameters.Add(new MySqlParameter("?P_UPDATEDDATE", MySqlDbType.DateTime, 0, ParameterDirection.InputOutput, true, 0, 0, "", DataRowVersion.Proposed, _updatedDate));

				command.ExecuteNonQuery();

				_templateTableDetailId					 = Convert.IsDBNull(command.Parameters["?P_TEMPLATETABLEDETAILID"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_TEMPLATETABLEDETAILID"].Value;
				_templateTableDetailGUID					 = Convert.IsDBNull(command.Parameters["?P_TEMPLATETABLEDETAILGUID"].Value) ? null : (string)command.Parameters["?P_TEMPLATETABLEDETAILGUID"].Value;
				_templateId					 = Convert.IsDBNull(command.Parameters["?P_TEMPLATEID"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_TEMPLATEID"].Value;
				_sourceTable					 = Convert.IsDBNull(command.Parameters["?P_SOURCETABLE"].Value) ? null : (string)command.Parameters["?P_SOURCETABLE"].Value;
				_targetTable					 = Convert.IsDBNull(command.Parameters["?P_TARGETTABLE"].Value) ? null : (string)command.Parameters["?P_TARGETTABLE"].Value;
				_isDeduplicateData					 = Convert.IsDBNull(command.Parameters["?P_ISDEDUPLICATEDATA"].Value) ? (bool?)null : (bool?)Convert.ToBoolean(command.Parameters["?P_ISDEDUPLICATEDATA"].Value);
				_executionOrder					 = Convert.IsDBNull(command.Parameters["?P_EXECUTIONORDER"].Value) ? (Int32?)null : (Int32?)command.Parameters["?P_EXECUTIONORDER"].Value;
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
		public Int32? TemplateTableDetailId
		{
			get
			{
				return _templateTableDetailId;
			}
			set
			{
				_templateTableDetailId = value;
			}
		}
		public string TemplateTableDetailGUID
		{
			get
			{
				return _templateTableDetailGUID;
			}
			set
			{
				_templateTableDetailGUID = value;
			}
		}
		public Int32? TemplateId
		{
			get
			{
				return _templateId;
			}
			set
			{
				_templateId = value;
			}
		}
		public string SourceTable
		{
			get
			{
				return _sourceTable;
			}
			set
			{
				_sourceTable = value;
			}
		}
		public string TargetTable
		{
			get
			{
				return _targetTable;
			}
			set
			{
				_targetTable = value;
			}
		}
		public bool? IsDeduplicateData
		{
			get
			{
				return _isDeduplicateData;
			}
			set
			{
				_isDeduplicateData = value;
			}
		}
		public Int32? ExecutionOrder
		{
			get
			{
				return _executionOrder;
			}
			set
			{
				_executionOrder = value;
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
