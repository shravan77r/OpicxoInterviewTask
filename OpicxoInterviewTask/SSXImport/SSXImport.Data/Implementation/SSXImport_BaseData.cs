/*************************************************************
** Class generated by CodeTrigger, Version 6.3.0.5
** This class was generated on 28/05/2021 14:51:26
**************************************************************/

using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SSXImport.Data.Interfaces;

namespace SSXImport.Data
{
	public partial class SSXImport_BaseData 
	{
		#region members
		protected SSXImport_TxConnectionProvider _connectionProvider;
		static string _staticConnectionString;
		bool _isDisposed = false;
		#endregion

		#region initialisation
		public SSXImport_BaseData()
		{
			Init();
		}

		private void Init()
		{
		}
		#endregion

		#region disposable interface support
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool isDisposing)
		{
			if(!_isDisposed)
			{
				if(isDisposing)
				{
					if(_connectionProvider != null)
					{
						((IDisposable)_connectionProvider).Dispose();
						_connectionProvider = null;
					}
				}
			}
			_isDisposed = true;
		}
		#endregion

		#region exception mapping
		protected static Exception MapException(Exception ex)
		{
			if (ex.Message.Contains("RAISERROR_UPDATEVERSION"))
				return new Exception("There has been a data concurrency issue updating this record, possibly stale data", ex);
			
			else if (ex.Message.Contains("RAISERROR_RECORDNOTFOUND"))
				return new Exception("There has been a data concurrency issue updating this record, record not available (not found)", ex);
			
			else
				return null;
		}
		#endregion

		#region connection support
		public virtual SSXImport_TxConnectionProvider ConnectionProvider
		{
			set
			{
				if(value == null)
					throw new Exception("Connection provider cannot be null");
				
				_connectionProvider = value;
			}
		}

		public static MySqlConnection StaticSqlConnection
		{
			get
			{
				MySqlConnection staticConnection = new MySqlConnection();
				staticConnection.ConnectionString = StaticConnectionString;
				return staticConnection;
			}
		}
		
		public static string StaticConnectionString
		{
			set { _staticConnectionString = value; }
			get
			{
				if (!string.IsNullOrEmpty(_staticConnectionString))
					return _staticConnectionString;
				
				return SSXImport_ConfigWrapper.GetConnectionString("DBConnectionString");
			}
		}
		#endregion

		#region criteria api support
		protected static string GetProjections(IList<IDataProjection> dataProjection)
		{
			string ret = "";
			if (dataProjection != null)
			{
				foreach (var projection in dataProjection)
				{
					if (ret != "") ret += ", ";
						ret += projection.ToSql();
				}
			}
			return ret;
		}

		public static string GetSelectionCriteria(IList<IDataCriterion> dataCriteria)
		{
			string ret = "";
			if(dataCriteria != null)
			{
				foreach (var criterion in dataCriteria)
				{
					if (ret != "") ret += " AND ";
					ret += criterion.ToSql();
				}
			}
			return ret;
		}

		public static string GetSelectionOrder(IList<IDataOrderBy> dataOrder)
		{
			string retorder = "";
			if(dataOrder != null)
			{
				foreach (var order in dataOrder)
				{
					if (retorder != "") retorder += ", ";
					retorder += order.ToSql();
				}
			}
			return retorder;
		}

		public static string GetSelectionSkip(IDataSkip dataSkip)
		{
			string retskip = "";
			if (dataSkip != null)
				retskip = " " + dataSkip.ToSql();
			return retskip;
		}

		public static string GetSelectionTake(IDataTake dataTake)
		{
			string rettake = "";
			if (dataTake != null)
				rettake = " " + dataTake.ToSql();
			return rettake;
		}

		protected static string UpdateFieldProjections(string query, string projections)
		{
			string fieldsStartMarker = "##STARTFIELDS##";
			string fieldsEndMarker = "##ENDFIELDS##";
			int fPosStart = query.IndexOf(fieldsStartMarker);
			int fPosEnd = query.IndexOf(fieldsEndMarker);
			if ((fPosStart < 0) || (fPosEnd < 0))
				return query;
			else if(string.IsNullOrEmpty(projections))
			{
				query = query.Replace(fieldsStartMarker, "");
				query = query.Replace(fieldsEndMarker, "");
			}
			else
			{
				string fieldsSubString = query.Substring(fPosStart, (fPosEnd - fPosStart + 13));
				query = query.Replace(fieldsSubString, projections);
			}
		return query;
		}

		public static string GetSelectionCriteria(string query, IList<IDataProjection> dataProjection, IList<IDataCriterion> dataCriteria, IList<IDataOrderBy> dataOrder)
		{
			return GetSelectionCriteria(query, dataProjection, dataCriteria, dataOrder, null, null);
		}
			
		public static string GetSelectionCriteria(string query, IList<IDataProjection> dataProjection, IList<IDataCriterion> dataCriteria, IList<IDataOrderBy> dataOrder, IDataSkip dataSkip, IDataTake dataTake)
		{
			string projections = "";
			projections = GetProjections(dataProjection);
			query = UpdateFieldProjections(query, projections);

			string ret = GetSelectionCriteria(dataCriteria);
			ret = (ret != "") ? " WHERE " + ret : "";
			
			string retorder = GetSelectionOrder(dataOrder);
			retorder = (retorder != "") ? " ORDER BY " + retorder : "";
			
			string retskip = GetSelectionSkip(dataSkip);
			string rettake = GetSelectionTake(dataTake);
			if ((retorder == "") && ((rettake != "") || (retskip != "")))
				throw new Exception("Invalid query: Using 'Take' or 'Skip' requires an OrderBy clause");
			
			retorder += (rettake + retskip);
			ret = ret + retorder;
			query = query.Replace("##CRITERIA##", ret);
			return query;
		}
		#endregion
	}
}
