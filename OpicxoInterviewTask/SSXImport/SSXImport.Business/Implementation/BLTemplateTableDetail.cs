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
	///This is the definition of the class BLTemplateTableDetail.
	///</Summary>
	public partial class BLTemplateTableDetail : SSXImport_BaseBusiness, IQueryableCollection
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
		public BLTemplateTableDetail()
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
		///Int32 templateTableDetailId
		///</parameters>
		public BLTemplateTableDetail(Int32 templateTableDetailId)
		{
			try
			{
				DLTemplateTableDetail dlTemplateTableDetail = DLTemplateTableDetail.SelectOne(templateTableDetailId);
				_templateTableDetailId = dlTemplateTableDetail.TemplateTableDetailId;
				_templateTableDetailGUID = dlTemplateTableDetail.TemplateTableDetailGUID;
				_templateId = dlTemplateTableDetail.TemplateId;
				_sourceTable = dlTemplateTableDetail.SourceTable;
				_targetTable = dlTemplateTableDetail.TargetTable;
				_isDeduplicateData = dlTemplateTableDetail.IsDeduplicateData;
				_executionOrder = dlTemplateTableDetail.ExecutionOrder;
				_isActive = dlTemplateTableDetail.IsActive;
				_isDelete = dlTemplateTableDetail.IsDelete;
				_enteredBy = dlTemplateTableDetail.EnteredBy;
				_enteredDate = dlTemplateTableDetail.EnteredDate;
				_updatedBy = dlTemplateTableDetail.UpdatedBy;
				_updatedDate = dlTemplateTableDetail.UpdatedDate;
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
		///TemplateTableDetailGUID
		///</parameters>
		public BLTemplateTableDetail(string templateTableDetailGUID)
		{
			try
			{
				DLTemplateTableDetail dlTemplateTableDetail = DLTemplateTableDetail.SelectOneByTemplateTableDetailGUID(templateTableDetailGUID);
				_templateTableDetailId = dlTemplateTableDetail.TemplateTableDetailId;
				_templateTableDetailGUID = dlTemplateTableDetail.TemplateTableDetailGUID;
				_templateId = dlTemplateTableDetail.TemplateId;
				_sourceTable = dlTemplateTableDetail.SourceTable;
				_targetTable = dlTemplateTableDetail.TargetTable;
				_isDeduplicateData = dlTemplateTableDetail.IsDeduplicateData;
				_executionOrder = dlTemplateTableDetail.ExecutionOrder;
				_isActive = dlTemplateTableDetail.IsActive;
				_isDelete = dlTemplateTableDetail.IsDelete;
				_enteredBy = dlTemplateTableDetail.EnteredBy;
				_enteredDate = dlTemplateTableDetail.EnteredDate;
				_updatedBy = dlTemplateTableDetail.UpdatedBy;
				_updatedDate = dlTemplateTableDetail.UpdatedDate;
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
		///DLTemplateTableDetail
		///</parameters>
		protected internal BLTemplateTableDetail(DLTemplateTableDetail dlTemplateTableDetail)
		{
			try
			{
				_templateTableDetailId = dlTemplateTableDetail.TemplateTableDetailId;
				_templateTableDetailGUID = dlTemplateTableDetail.TemplateTableDetailGUID;
				_templateId = dlTemplateTableDetail.TemplateId;
				_sourceTable = dlTemplateTableDetail.SourceTable;
				_targetTable = dlTemplateTableDetail.TargetTable;
				_isDeduplicateData = dlTemplateTableDetail.IsDeduplicateData;
				_executionOrder = dlTemplateTableDetail.ExecutionOrder;
				_isActive = dlTemplateTableDetail.IsActive;
				_isDelete = dlTemplateTableDetail.IsDelete;
				_enteredBy = dlTemplateTableDetail.EnteredBy;
				_enteredDate = dlTemplateTableDetail.EnteredDate;
				_updatedBy = dlTemplateTableDetail.UpdatedBy;
				_updatedDate = dlTemplateTableDetail.UpdatedDate;
			}
			catch
			{
				throw;
			}
		}

		///<Summary>
		///SaveNew
		///This method persists a new TemplateTableDetail record to the store
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///
		///</parameters>
		public virtual void SaveNew()
		{
			DLTemplateTableDetail dlTemplateTableDetail = new DLTemplateTableDetail();
			RegisterDataObject(dlTemplateTableDetail);
			BeginTransaction("savenewBLTemplateTableDetail");
			try
			{
				dlTemplateTableDetail.TemplateTableDetailGUID = _templateTableDetailGUID;
				dlTemplateTableDetail.TemplateId = _templateId;
				dlTemplateTableDetail.SourceTable = _sourceTable;
				dlTemplateTableDetail.TargetTable = _targetTable;
				dlTemplateTableDetail.IsDeduplicateData = _isDeduplicateData;
				dlTemplateTableDetail.ExecutionOrder = _executionOrder;
				dlTemplateTableDetail.IsActive = _isActive;
				dlTemplateTableDetail.IsDelete = _isDelete;
				dlTemplateTableDetail.EnteredBy = _enteredBy;
				dlTemplateTableDetail.EnteredDate = _enteredDate;
				dlTemplateTableDetail.UpdatedBy = _updatedBy;
				dlTemplateTableDetail.UpdatedDate = _updatedDate;
				dlTemplateTableDetail.Insert();
				CommitTransaction();
				
				_templateTableDetailId = dlTemplateTableDetail.TemplateTableDetailId;
				_templateTableDetailGUID = dlTemplateTableDetail.TemplateTableDetailGUID;
				_templateId = dlTemplateTableDetail.TemplateId;
				_sourceTable = dlTemplateTableDetail.SourceTable;
				_targetTable = dlTemplateTableDetail.TargetTable;
				_isDeduplicateData = dlTemplateTableDetail.IsDeduplicateData;
				_executionOrder = dlTemplateTableDetail.ExecutionOrder;
				_isActive = dlTemplateTableDetail.IsActive;
				_isDelete = dlTemplateTableDetail.IsDelete;
				_enteredBy = dlTemplateTableDetail.EnteredBy;
				_enteredDate = dlTemplateTableDetail.EnteredDate;
				_updatedBy = dlTemplateTableDetail.UpdatedBy;
				_updatedDate = dlTemplateTableDetail.UpdatedDate;
				_isDirty = false;
			}
			catch
			{
				RollbackTransaction("savenewBLTemplateTableDetail");
				throw;
			}
		}
		
		///<Summary>
		///Update
		///This method updates one TemplateTableDetail record in the store
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///BLTemplateTableDetail
		///</parameters>
		public virtual void Update()
		{
			DLTemplateTableDetail dlTemplateTableDetail = new DLTemplateTableDetail();
			RegisterDataObject(dlTemplateTableDetail);
			BeginTransaction("updateBLTemplateTableDetail");
			try
			{
				dlTemplateTableDetail.TemplateTableDetailId = _templateTableDetailId;
				dlTemplateTableDetail.TemplateTableDetailGUID = _templateTableDetailGUID;
				dlTemplateTableDetail.TemplateId = _templateId;
				dlTemplateTableDetail.SourceTable = _sourceTable;
				dlTemplateTableDetail.TargetTable = _targetTable;
				dlTemplateTableDetail.IsDeduplicateData = _isDeduplicateData;
				dlTemplateTableDetail.ExecutionOrder = _executionOrder;
				dlTemplateTableDetail.IsActive = _isActive;
				dlTemplateTableDetail.IsDelete = _isDelete;
				dlTemplateTableDetail.EnteredBy = _enteredBy;
				dlTemplateTableDetail.EnteredDate = _enteredDate;
				dlTemplateTableDetail.UpdatedBy = _updatedBy;
				dlTemplateTableDetail.UpdatedDate = _updatedDate;
				dlTemplateTableDetail.Update();
				CommitTransaction();
				
				_templateTableDetailId = dlTemplateTableDetail.TemplateTableDetailId;
				_templateTableDetailGUID = dlTemplateTableDetail.TemplateTableDetailGUID;
				_templateId = dlTemplateTableDetail.TemplateId;
				_sourceTable = dlTemplateTableDetail.SourceTable;
				_targetTable = dlTemplateTableDetail.TargetTable;
				_isDeduplicateData = dlTemplateTableDetail.IsDeduplicateData;
				_executionOrder = dlTemplateTableDetail.ExecutionOrder;
				_isActive = dlTemplateTableDetail.IsActive;
				_isDelete = dlTemplateTableDetail.IsDelete;
				_enteredBy = dlTemplateTableDetail.EnteredBy;
				_enteredDate = dlTemplateTableDetail.EnteredDate;
				_updatedBy = dlTemplateTableDetail.UpdatedBy;
				_updatedDate = dlTemplateTableDetail.UpdatedDate;
				_isDirty = false;
			}
			catch
			{
				RollbackTransaction("updateBLTemplateTableDetail");
				throw;
			}
		}
		///<Summary>
		///Delete
		///This method deletes one TemplateTableDetail record from the store
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///
		///</parameters>
		public virtual void Delete()
		{
			DLTemplateTableDetail dlTemplateTableDetail = new DLTemplateTableDetail();
			RegisterDataObject(dlTemplateTableDetail);
			BeginTransaction("deleteBLTemplateTableDetail");
			try
			{
				dlTemplateTableDetail.TemplateTableDetailId = _templateTableDetailId;
				dlTemplateTableDetail.Delete();
				CommitTransaction();
			}
			catch
			{
				RollbackTransaction("deleteBLTemplateTableDetail");
				throw;
			}
		}
		
		///<Summary>
		///TemplateTableDetailCollection
		///This method returns the collection of BLTemplateTableDetail objects
		///</Summary>
		///<returns>
		///List[BLTemplateTableDetail]
		///</returns>
		///<parameters>
		///
		///</parameters>
		public static IList<BLTemplateTableDetail> TemplateTableDetailCollection()
		{
			try
			{
				IList<BLTemplateTableDetail> blTemplateTableDetailCollection = new List<BLTemplateTableDetail>();
				IList<DLTemplateTableDetail> dlTemplateTableDetailCollection = DLTemplateTableDetail.SelectAll();
			
				foreach(DLTemplateTableDetail dlTemplateTableDetail in dlTemplateTableDetailCollection)
					blTemplateTableDetailCollection.Add(new BLTemplateTableDetail(dlTemplateTableDetail));
			
				return blTemplateTableDetailCollection;
			}
			catch
			{
				throw;
			}
		}
		
		
		///<Summary>
		///TemplateTableDetailCollectionCount
		///This method returns the collection count of BLTemplateTableDetail objects
		///</Summary>
		///<returns>
		///Int32
		///</returns>
		///<parameters>
		///
		///</parameters>
		public static Int32 TemplateTableDetailCollectionCount()
		{
			try
			{
				Int32 objCount = DLTemplateTableDetail.SelectAllCount();
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
		///List<BLTemplateTableDetail>
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
				IDictionary<string, IList<object>> retObj = DLTemplateTableDetail.SelectAllByCriteriaProjection(lstDataProjection, lstDataCriteria, lstDataOrder, dataSkip, dataTake);
				return retObj;
			}
			catch
			{
				throw;
			}
		}
		
		
		///<Summary>
		///TemplateTableDetailCollection<T>
		///This method implements the IQueryable Collection<T> method, returning a collection of BLTemplateTableDetail objects, filtered by optional criteria
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
				IList<T> blTemplateTableDetailCollection = new List<T>();
				IList<IDataCriterion> lstDataCriteria = (icriteria == null) ? null : icriteria.ListDataCriteria();
				IList<IDataOrderBy> lstDataOrder = (icriteria == null) ? null : icriteria.ListDataOrder();
				IDataTake dataTake = (icriteria == null) ? null : icriteria.DataTake();
				IDataSkip dataSkip = (icriteria == null) ? null : icriteria.DataSkip();
				IList<DLTemplateTableDetail> dlTemplateTableDetailCollection = DLTemplateTableDetail.SelectAllByCriteria(lstDataCriteria, lstDataOrder, dataSkip, dataTake);
			
				foreach(DLTemplateTableDetail resdlTemplateTableDetail in dlTemplateTableDetailCollection)
					blTemplateTableDetailCollection.Add((T)(object)new BLTemplateTableDetail(resdlTemplateTableDetail));
			
				return blTemplateTableDetailCollection;
			}
			catch
			{
				throw;
			}
		}
		
		
		///<Summary>
		///TemplateTableDetailCollectionCount
		///This method implements the IQueryable CollectionCount method, returning a collection count of BLTemplateTableDetail objects, filtered by optional criteria
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
				Int32 objCount = DLTemplateTableDetail.SelectAllByCriteriaCount(lstDataCriteria);
				return objCount;
			}
			catch
			{
				throw;
			}
		}
		
		#endregion

		#region member properties
		
		public virtual Int32? TemplateTableDetailId
		{
			get
			{
				 return _templateTableDetailId;
			}
			set
			{
				_templateTableDetailId = value;
				_isDirty = true;
			}
		}
		
		public virtual string TemplateTableDetailGUID
		{
			get
			{
				 return _templateTableDetailGUID;
			}
			set
			{
				_templateTableDetailGUID = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? TemplateId
		{
			get
			{
				 return _templateId;
			}
			set
			{
				_templateId = value;
				_isDirty = true;
			}
		}
		
		public virtual string SourceTable
		{
			get
			{
				 return _sourceTable;
			}
			set
			{
				_sourceTable = value;
				_isDirty = true;
			}
		}
		
		public virtual string TargetTable
		{
			get
			{
				 return _targetTable;
			}
			set
			{
				_targetTable = value;
				_isDirty = true;
			}
		}
		
		public virtual bool? IsDeduplicateData
		{
			get
			{
				 return _isDeduplicateData;
			}
			set
			{
				_isDeduplicateData = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? ExecutionOrder
		{
			get
			{
				 return _executionOrder;
			}
			set
			{
				_executionOrder = value;
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
