/*************************************************************
** Class generated by CodeTrigger, Version 6.3.0.5
** This class was generated on 27/08/2021 16:39:34
** Changes to this file may cause incorrect behaviour and will be lost if the code is regenerated
**************************************************************/
using System;
using System.Collections.Generic;
using SSXImport.Data;
using SSXImport.Data.Interfaces;
using SSXImport.Business.Interfaces;

namespace SSXImport.Business
{
	///<Summary>
	///Class definition
	///This is the definition of the class BLAPIOutputParameter.
	///</Summary>
	public partial class BLAPIOutputParameter : SSXImport_BaseBusiness, IQueryableCollection
	{
		#region member variables
		protected Int32? _outputParameterId;
		protected string _outputParameterGUID;
		protected Int32? _aPIId;
		protected string _keyColumn;
		protected string _valueColumn;
		protected Int32? _type;
		protected bool? _isActive;
		protected bool? _isDelete;
		protected Int32? _enteredBy;
		protected DateTime? _enteredDate;
		protected Int32? _updatedBy;
		protected DateTime? _updatedDate;
		protected bool _isDirty = false;
		/*collection member objects*******************/
		/*********************************************/
		#endregion

		#region class methods
		///<Summary>
		///Constructor
		///This is the default constructor
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///
		///</parameters>
		public BLAPIOutputParameter()
		{
		}

		///<Summary>
		///Constructor
		///Constructor using primary key(s)
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///Int32 outputParameterId
		///</parameters>
		public BLAPIOutputParameter(Int32 outputParameterId)
		{
			try
			{
				DLAPIOutputParameter dlAPIOutputParameter = DLAPIOutputParameter.SelectOne(outputParameterId);
				_outputParameterId = dlAPIOutputParameter.OutputParameterId;
				_outputParameterGUID = dlAPIOutputParameter.OutputParameterGUID;
				_aPIId = dlAPIOutputParameter.APIId;
				_keyColumn = dlAPIOutputParameter.KeyColumn;
				_valueColumn = dlAPIOutputParameter.ValueColumn;
				_type = dlAPIOutputParameter.Type;
				_isActive = dlAPIOutputParameter.IsActive;
				_isDelete = dlAPIOutputParameter.IsDelete;
				_enteredBy = dlAPIOutputParameter.EnteredBy;
				_enteredDate = dlAPIOutputParameter.EnteredDate;
				_updatedBy = dlAPIOutputParameter.UpdatedBy;
				_updatedDate = dlAPIOutputParameter.UpdatedDate;
			}
			catch
			{
				throw;
			}
		}

		///<Summary>
		///Constructor
		///Constructor using unique field(s)
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///OutputParameterGUID
		///</parameters>
		public BLAPIOutputParameter(string outputParameterGUID)
		{
			try
			{
				DLAPIOutputParameter dlAPIOutputParameter = DLAPIOutputParameter.SelectOneByOutputParameterGUID(outputParameterGUID);
				_outputParameterId = dlAPIOutputParameter.OutputParameterId;
				_outputParameterGUID = dlAPIOutputParameter.OutputParameterGUID;
				_aPIId = dlAPIOutputParameter.APIId;
				_keyColumn = dlAPIOutputParameter.KeyColumn;
				_valueColumn = dlAPIOutputParameter.ValueColumn;
				_type = dlAPIOutputParameter.Type;
				_isActive = dlAPIOutputParameter.IsActive;
				_isDelete = dlAPIOutputParameter.IsDelete;
				_enteredBy = dlAPIOutputParameter.EnteredBy;
				_enteredDate = dlAPIOutputParameter.EnteredDate;
				_updatedBy = dlAPIOutputParameter.UpdatedBy;
				_updatedDate = dlAPIOutputParameter.UpdatedDate;
			}
			catch
			{
				throw;
			}
		}

		///<Summary>
		///Constructor
		///This constructor initializes the business object from its respective data object
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///DLAPIOutputParameter
		///</parameters>
		protected internal BLAPIOutputParameter(DLAPIOutputParameter dlAPIOutputParameter)
		{
			try
			{
				_outputParameterId = dlAPIOutputParameter.OutputParameterId;
				_outputParameterGUID = dlAPIOutputParameter.OutputParameterGUID;
				_aPIId = dlAPIOutputParameter.APIId;
				_keyColumn = dlAPIOutputParameter.KeyColumn;
				_valueColumn = dlAPIOutputParameter.ValueColumn;
				_type = dlAPIOutputParameter.Type;
				_isActive = dlAPIOutputParameter.IsActive;
				_isDelete = dlAPIOutputParameter.IsDelete;
				_enteredBy = dlAPIOutputParameter.EnteredBy;
				_enteredDate = dlAPIOutputParameter.EnteredDate;
				_updatedBy = dlAPIOutputParameter.UpdatedBy;
				_updatedDate = dlAPIOutputParameter.UpdatedDate;
			}
			catch
			{
				throw;
			}
		}

		///<Summary>
		///SaveNew
		///This method persists a new APIOutputParameter record to the store
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///
		///</parameters>
		public virtual void SaveNew()
		{
			DLAPIOutputParameter dlAPIOutputParameter = new DLAPIOutputParameter();
			RegisterDataObject(dlAPIOutputParameter);
			BeginTransaction("savenewBLAPIOutputParameter");
			try
			{
				dlAPIOutputParameter.OutputParameterGUID = _outputParameterGUID;
				dlAPIOutputParameter.APIId = _aPIId;
				dlAPIOutputParameter.KeyColumn = _keyColumn;
				dlAPIOutputParameter.ValueColumn = _valueColumn;
				dlAPIOutputParameter.Type = _type;
				dlAPIOutputParameter.IsActive = _isActive;
				dlAPIOutputParameter.IsDelete = _isDelete;
				dlAPIOutputParameter.EnteredBy = _enteredBy;
				dlAPIOutputParameter.EnteredDate = _enteredDate;
				dlAPIOutputParameter.UpdatedBy = _updatedBy;
				dlAPIOutputParameter.UpdatedDate = _updatedDate;
				dlAPIOutputParameter.Insert();
				CommitTransaction();
				
				_outputParameterId = dlAPIOutputParameter.OutputParameterId;
				_outputParameterGUID = dlAPIOutputParameter.OutputParameterGUID;
				_aPIId = dlAPIOutputParameter.APIId;
				_keyColumn = dlAPIOutputParameter.KeyColumn;
				_valueColumn = dlAPIOutputParameter.ValueColumn;
				_type = dlAPIOutputParameter.Type;
				_isActive = dlAPIOutputParameter.IsActive;
				_isDelete = dlAPIOutputParameter.IsDelete;
				_enteredBy = dlAPIOutputParameter.EnteredBy;
				_enteredDate = dlAPIOutputParameter.EnteredDate;
				_updatedBy = dlAPIOutputParameter.UpdatedBy;
				_updatedDate = dlAPIOutputParameter.UpdatedDate;
				_isDirty = false;
			}
			catch
			{
				RollbackTransaction("savenewBLAPIOutputParameter");
				throw;
			}
		}
		
		///<Summary>
		///Update
		///This method updates one APIOutputParameter record in the store
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///BLAPIOutputParameter
		///</parameters>
		public virtual void Update()
		{
			DLAPIOutputParameter dlAPIOutputParameter = new DLAPIOutputParameter();
			RegisterDataObject(dlAPIOutputParameter);
			BeginTransaction("updateBLAPIOutputParameter");
			try
			{
				dlAPIOutputParameter.OutputParameterId = _outputParameterId;
				dlAPIOutputParameter.OutputParameterGUID = _outputParameterGUID;
				dlAPIOutputParameter.APIId = _aPIId;
				dlAPIOutputParameter.KeyColumn = _keyColumn;
				dlAPIOutputParameter.ValueColumn = _valueColumn;
				dlAPIOutputParameter.Type = _type;
				dlAPIOutputParameter.IsActive = _isActive;
				dlAPIOutputParameter.IsDelete = _isDelete;
				dlAPIOutputParameter.EnteredBy = _enteredBy;
				dlAPIOutputParameter.EnteredDate = _enteredDate;
				dlAPIOutputParameter.UpdatedBy = _updatedBy;
				dlAPIOutputParameter.UpdatedDate = _updatedDate;
				dlAPIOutputParameter.Update();
				CommitTransaction();
				
				_outputParameterId = dlAPIOutputParameter.OutputParameterId;
				_outputParameterGUID = dlAPIOutputParameter.OutputParameterGUID;
				_aPIId = dlAPIOutputParameter.APIId;
				_keyColumn = dlAPIOutputParameter.KeyColumn;
				_valueColumn = dlAPIOutputParameter.ValueColumn;
				_type = dlAPIOutputParameter.Type;
				_isActive = dlAPIOutputParameter.IsActive;
				_isDelete = dlAPIOutputParameter.IsDelete;
				_enteredBy = dlAPIOutputParameter.EnteredBy;
				_enteredDate = dlAPIOutputParameter.EnteredDate;
				_updatedBy = dlAPIOutputParameter.UpdatedBy;
				_updatedDate = dlAPIOutputParameter.UpdatedDate;
				_isDirty = false;
			}
			catch
			{
				RollbackTransaction("updateBLAPIOutputParameter");
				throw;
			}
		}
		///<Summary>
		///Delete
		///This method deletes one APIOutputParameter record from the store
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///
		///</parameters>
		public virtual void Delete()
		{
			DLAPIOutputParameter dlAPIOutputParameter = new DLAPIOutputParameter();
			RegisterDataObject(dlAPIOutputParameter);
			BeginTransaction("deleteBLAPIOutputParameter");
			try
			{
				dlAPIOutputParameter.OutputParameterId = _outputParameterId;
				dlAPIOutputParameter.Delete();
				CommitTransaction();
			}
			catch
			{
				RollbackTransaction("deleteBLAPIOutputParameter");
				throw;
			}
		}
		
		///<Summary>
		///APIOutputParameterCollection
		///This method returns the collection of BLAPIOutputParameter objects
		///</Summary>
		///<returns>
		///List[BLAPIOutputParameter]
		///</returns>
		///<parameters>
		///
		///</parameters>
		public static IList<BLAPIOutputParameter> APIOutputParameterCollection()
		{
			try
			{
				IList<BLAPIOutputParameter> blAPIOutputParameterCollection = new List<BLAPIOutputParameter>();
				IList<DLAPIOutputParameter> dlAPIOutputParameterCollection = DLAPIOutputParameter.SelectAll();
			
				foreach(DLAPIOutputParameter dlAPIOutputParameter in dlAPIOutputParameterCollection)
					blAPIOutputParameterCollection.Add(new BLAPIOutputParameter(dlAPIOutputParameter));
			
				return blAPIOutputParameterCollection;
			}
			catch
			{
				throw;
			}
		}
		
		
		///<Summary>
		///APIOutputParameterCollectionCount
		///This method returns the collection count of BLAPIOutputParameter objects
		///</Summary>
		///<returns>
		///Int32
		///</returns>
		///<parameters>
		///
		///</parameters>
		public static Int32 APIOutputParameterCollectionCount()
		{
			try
			{
				Int32 objCount = DLAPIOutputParameter.SelectAllCount();
				return objCount;
			}
			catch
			{
				throw;
			}
		}
		
		
		///<Summary>
		///Projections
		///This method returns the collection of projections, ordered and filtered by optional criteria
		///</Summary>
		///<returns>
		///List<BLAPIOutputParameter>
		///</returns>
		///<parameters>
		///ICriteria icriteria
		///</parameters>
		public virtual IDictionary<string, IList<object>> Projections(object o)
		{
			try
			{
				ICriteria icriteria = (ICriteria)o;
				IList<IDataProjection> lstDataProjection = (icriteria == null) ? null : icriteria.ListDataProjection();
				IList<IDataCriterion> lstDataCriteria = (icriteria == null) ? null : icriteria.ListDataCriteria();
				IList<IDataOrderBy> lstDataOrder = (icriteria == null) ? null : icriteria.ListDataOrder();
				IDataTake dataTake = (icriteria == null) ? null : icriteria.DataTake();
				IDataSkip dataSkip = (icriteria == null) ? null : icriteria.DataSkip();
				IDictionary<string, IList<object>> retObj = DLAPIOutputParameter.SelectAllByCriteriaProjection(lstDataProjection, lstDataCriteria, lstDataOrder, dataSkip, dataTake);
				return retObj;
			}
			catch
			{
				throw;
			}
		}
		
		
		///<Summary>
		///APIOutputParameterCollection<T>
		///This method implements the IQueryable Collection<T> method, returning a collection of BLAPIOutputParameter objects, filtered by optional criteria
		///</Summary>
		///<returns>
		///IList<T>
		///</returns>
		///<parameters>
		///object o
		///</parameters>
		public virtual IList<T> Collection<T>(object o)
		{
			try
			{
				ICriteria icriteria = (ICriteria)o;
				IList<T> blAPIOutputParameterCollection = new List<T>();
				IList<IDataCriterion> lstDataCriteria = (icriteria == null) ? null : icriteria.ListDataCriteria();
				IList<IDataOrderBy> lstDataOrder = (icriteria == null) ? null : icriteria.ListDataOrder();
				IDataTake dataTake = (icriteria == null) ? null : icriteria.DataTake();
				IDataSkip dataSkip = (icriteria == null) ? null : icriteria.DataSkip();
				IList<DLAPIOutputParameter> dlAPIOutputParameterCollection = DLAPIOutputParameter.SelectAllByCriteria(lstDataCriteria, lstDataOrder, dataSkip, dataTake);
			
				foreach(DLAPIOutputParameter resdlAPIOutputParameter in dlAPIOutputParameterCollection)
					blAPIOutputParameterCollection.Add((T)(object)new BLAPIOutputParameter(resdlAPIOutputParameter));
			
				return blAPIOutputParameterCollection;
			}
			catch
			{
				throw;
			}
		}
		
		
		///<Summary>
		///APIOutputParameterCollectionCount
		///This method implements the IQueryable CollectionCount method, returning a collection count of BLAPIOutputParameter objects, filtered by optional criteria
		///</Summary>
		///<returns>
		///Int32
		///</returns>
		///<parameters>
		///object o
		///</parameters>
		public virtual Int32 CollectionCount(object o)
		{
			try
			{
				ICriteria icriteria = (ICriteria)o;
				IList<IDataCriterion> lstDataCriteria = (icriteria == null) ? null : icriteria.ListDataCriteria();
				Int32 objCount = DLAPIOutputParameter.SelectAllByCriteriaCount(lstDataCriteria);
				return objCount;
			}
			catch
			{
				throw;
			}
		}
		
		#endregion

		#region member properties
		
		public virtual Int32? OutputParameterId
		{
			get
			{
				 return _outputParameterId;
			}
			set
			{
				_outputParameterId = value;
				_isDirty = true;
			}
		}
		
		public virtual string OutputParameterGUID
		{
			get
			{
				 return _outputParameterGUID;
			}
			set
			{
				_outputParameterGUID = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? APIId
		{
			get
			{
				 return _aPIId;
			}
			set
			{
				_aPIId = value;
				_isDirty = true;
			}
		}
		
		public virtual string KeyColumn
		{
			get
			{
				 return _keyColumn;
			}
			set
			{
				_keyColumn = value;
				_isDirty = true;
			}
		}
		
		public virtual string ValueColumn
		{
			get
			{
				 return _valueColumn;
			}
			set
			{
				_valueColumn = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? Type
		{
			get
			{
				 return _type;
			}
			set
			{
				_type = value;
				_isDirty = true;
			}
		}
		
		public virtual bool? IsActive
		{
			get
			{
				 return _isActive;
			}
			set
			{
				_isActive = value;
				_isDirty = true;
			}
		}
		
		public virtual bool? IsDelete
		{
			get
			{
				 return _isDelete;
			}
			set
			{
				_isDelete = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? EnteredBy
		{
			get
			{
				 return _enteredBy;
			}
			set
			{
				_enteredBy = value;
				_isDirty = true;
			}
		}
		
		public virtual DateTime? EnteredDate
		{
			get
			{
				 return _enteredDate;
			}
			set
			{
				_enteredDate = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? UpdatedBy
		{
			get
			{
				 return _updatedBy;
			}
			set
			{
				_updatedBy = value;
				_isDirty = true;
			}
		}
		
		public virtual DateTime? UpdatedDate
		{
			get
			{
				 return _updatedDate;
			}
			set
			{
				_updatedDate = value;
				_isDirty = true;
			}
		}
		
		public virtual object Repository
		{
			get {	return null;	}
			set	{	}
		}
		
		public virtual bool IsDirty
		{
			get
			{
				 return _isDirty;
			}
			set
			{
				_isDirty = value;
			}
		}
		#endregion
	}
}