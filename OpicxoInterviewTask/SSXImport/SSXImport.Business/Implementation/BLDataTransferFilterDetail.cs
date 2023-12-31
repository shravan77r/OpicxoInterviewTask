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
	///This is the definition of the class BLDataTransferFilterDetail.
	///</Summary>
	public partial class BLDataTransferFilterDetail : SSXImport_BaseBusiness, IQueryableCollection
	{
		#region member variables
		protected Int32? _dataTransferFilterDetailId;
		protected string _dataTransferFilterDetailGUID;
		protected Int32? _dataTransferId;
		protected Int32? _dataTransferTableDetailId;
		protected Int32? _templateTableDetailId;
		protected string _filterColumn;
		protected Int32? _filterOperator;
		protected string _filterValue;
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
		public BLDataTransferFilterDetail()
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
		///Int32 dataTransferFilterDetailId
		///</parameters>
		public BLDataTransferFilterDetail(Int32 dataTransferFilterDetailId)
		{
			try
			{
				DLDataTransferFilterDetail dlDataTransferFilterDetail = DLDataTransferFilterDetail.SelectOne(dataTransferFilterDetailId);
				_dataTransferFilterDetailId = dlDataTransferFilterDetail.DataTransferFilterDetailId;
				_dataTransferFilterDetailGUID = dlDataTransferFilterDetail.DataTransferFilterDetailGUID;
				_dataTransferId = dlDataTransferFilterDetail.DataTransferId;
				_dataTransferTableDetailId = dlDataTransferFilterDetail.DataTransferTableDetailId;
				_templateTableDetailId = dlDataTransferFilterDetail.TemplateTableDetailId;
				_filterColumn = dlDataTransferFilterDetail.FilterColumn;
				_filterOperator = dlDataTransferFilterDetail.FilterOperator;
				_filterValue = dlDataTransferFilterDetail.FilterValue;
				_isActive = dlDataTransferFilterDetail.IsActive;
				_isDelete = dlDataTransferFilterDetail.IsDelete;
				_enteredBy = dlDataTransferFilterDetail.EnteredBy;
				_enteredDate = dlDataTransferFilterDetail.EnteredDate;
				_updatedBy = dlDataTransferFilterDetail.UpdatedBy;
				_updatedDate = dlDataTransferFilterDetail.UpdatedDate;
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
		///DataTransferFilterDetailGUID
		///</parameters>
		public BLDataTransferFilterDetail(string dataTransferFilterDetailGUID)
		{
			try
			{
				DLDataTransferFilterDetail dlDataTransferFilterDetail = DLDataTransferFilterDetail.SelectOneByDataTransferFilterDetailGUID(dataTransferFilterDetailGUID);
				_dataTransferFilterDetailId = dlDataTransferFilterDetail.DataTransferFilterDetailId;
				_dataTransferFilterDetailGUID = dlDataTransferFilterDetail.DataTransferFilterDetailGUID;
				_dataTransferId = dlDataTransferFilterDetail.DataTransferId;
				_dataTransferTableDetailId = dlDataTransferFilterDetail.DataTransferTableDetailId;
				_templateTableDetailId = dlDataTransferFilterDetail.TemplateTableDetailId;
				_filterColumn = dlDataTransferFilterDetail.FilterColumn;
				_filterOperator = dlDataTransferFilterDetail.FilterOperator;
				_filterValue = dlDataTransferFilterDetail.FilterValue;
				_isActive = dlDataTransferFilterDetail.IsActive;
				_isDelete = dlDataTransferFilterDetail.IsDelete;
				_enteredBy = dlDataTransferFilterDetail.EnteredBy;
				_enteredDate = dlDataTransferFilterDetail.EnteredDate;
				_updatedBy = dlDataTransferFilterDetail.UpdatedBy;
				_updatedDate = dlDataTransferFilterDetail.UpdatedDate;
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
		///DLDataTransferFilterDetail
		///</parameters>
		protected internal BLDataTransferFilterDetail(DLDataTransferFilterDetail dlDataTransferFilterDetail)
		{
			try
			{
				_dataTransferFilterDetailId = dlDataTransferFilterDetail.DataTransferFilterDetailId;
				_dataTransferFilterDetailGUID = dlDataTransferFilterDetail.DataTransferFilterDetailGUID;
				_dataTransferId = dlDataTransferFilterDetail.DataTransferId;
				_dataTransferTableDetailId = dlDataTransferFilterDetail.DataTransferTableDetailId;
				_templateTableDetailId = dlDataTransferFilterDetail.TemplateTableDetailId;
				_filterColumn = dlDataTransferFilterDetail.FilterColumn;
				_filterOperator = dlDataTransferFilterDetail.FilterOperator;
				_filterValue = dlDataTransferFilterDetail.FilterValue;
				_isActive = dlDataTransferFilterDetail.IsActive;
				_isDelete = dlDataTransferFilterDetail.IsDelete;
				_enteredBy = dlDataTransferFilterDetail.EnteredBy;
				_enteredDate = dlDataTransferFilterDetail.EnteredDate;
				_updatedBy = dlDataTransferFilterDetail.UpdatedBy;
				_updatedDate = dlDataTransferFilterDetail.UpdatedDate;
			}
			catch
			{
				throw;
			}
		}

		///<Summary>
		///SaveNew
		///This method persists a new DataTransferFilterDetail record to the store
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///
		///</parameters>
		public virtual void SaveNew()
		{
			DLDataTransferFilterDetail dlDataTransferFilterDetail = new DLDataTransferFilterDetail();
			RegisterDataObject(dlDataTransferFilterDetail);
			BeginTransaction("savenewBLDataTransferFilterD7627");
			try
			{
				dlDataTransferFilterDetail.DataTransferFilterDetailGUID = _dataTransferFilterDetailGUID;
				dlDataTransferFilterDetail.DataTransferId = _dataTransferId;
				dlDataTransferFilterDetail.DataTransferTableDetailId = _dataTransferTableDetailId;
				dlDataTransferFilterDetail.TemplateTableDetailId = _templateTableDetailId;
				dlDataTransferFilterDetail.FilterColumn = _filterColumn;
				dlDataTransferFilterDetail.FilterOperator = _filterOperator;
				dlDataTransferFilterDetail.FilterValue = _filterValue;
				dlDataTransferFilterDetail.IsActive = _isActive;
				dlDataTransferFilterDetail.IsDelete = _isDelete;
				dlDataTransferFilterDetail.EnteredBy = _enteredBy;
				dlDataTransferFilterDetail.EnteredDate = _enteredDate;
				dlDataTransferFilterDetail.UpdatedBy = _updatedBy;
				dlDataTransferFilterDetail.UpdatedDate = _updatedDate;
				dlDataTransferFilterDetail.Insert();
				CommitTransaction();
				
				_dataTransferFilterDetailId = dlDataTransferFilterDetail.DataTransferFilterDetailId;
				_dataTransferFilterDetailGUID = dlDataTransferFilterDetail.DataTransferFilterDetailGUID;
				_dataTransferId = dlDataTransferFilterDetail.DataTransferId;
				_dataTransferTableDetailId = dlDataTransferFilterDetail.DataTransferTableDetailId;
				_templateTableDetailId = dlDataTransferFilterDetail.TemplateTableDetailId;
				_filterColumn = dlDataTransferFilterDetail.FilterColumn;
				_filterOperator = dlDataTransferFilterDetail.FilterOperator;
				_filterValue = dlDataTransferFilterDetail.FilterValue;
				_isActive = dlDataTransferFilterDetail.IsActive;
				_isDelete = dlDataTransferFilterDetail.IsDelete;
				_enteredBy = dlDataTransferFilterDetail.EnteredBy;
				_enteredDate = dlDataTransferFilterDetail.EnteredDate;
				_updatedBy = dlDataTransferFilterDetail.UpdatedBy;
				_updatedDate = dlDataTransferFilterDetail.UpdatedDate;
				_isDirty = false;
			}
			catch
			{
				RollbackTransaction("savenewBLDataTransferFilterD7627");
				throw;
			}
		}
		
		///<Summary>
		///Update
		///This method updates one DataTransferFilterDetail record in the store
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///BLDataTransferFilterDetail
		///</parameters>
		public virtual void Update()
		{
			DLDataTransferFilterDetail dlDataTransferFilterDetail = new DLDataTransferFilterDetail();
			RegisterDataObject(dlDataTransferFilterDetail);
			BeginTransaction("updateBLDataTransferFilterDetail");
			try
			{
				dlDataTransferFilterDetail.DataTransferFilterDetailId = _dataTransferFilterDetailId;
				dlDataTransferFilterDetail.DataTransferFilterDetailGUID = _dataTransferFilterDetailGUID;
				dlDataTransferFilterDetail.DataTransferId = _dataTransferId;
				dlDataTransferFilterDetail.DataTransferTableDetailId = _dataTransferTableDetailId;
				dlDataTransferFilterDetail.TemplateTableDetailId = _templateTableDetailId;
				dlDataTransferFilterDetail.FilterColumn = _filterColumn;
				dlDataTransferFilterDetail.FilterOperator = _filterOperator;
				dlDataTransferFilterDetail.FilterValue = _filterValue;
				dlDataTransferFilterDetail.IsActive = _isActive;
				dlDataTransferFilterDetail.IsDelete = _isDelete;
				dlDataTransferFilterDetail.EnteredBy = _enteredBy;
				dlDataTransferFilterDetail.EnteredDate = _enteredDate;
				dlDataTransferFilterDetail.UpdatedBy = _updatedBy;
				dlDataTransferFilterDetail.UpdatedDate = _updatedDate;
				dlDataTransferFilterDetail.Update();
				CommitTransaction();
				
				_dataTransferFilterDetailId = dlDataTransferFilterDetail.DataTransferFilterDetailId;
				_dataTransferFilterDetailGUID = dlDataTransferFilterDetail.DataTransferFilterDetailGUID;
				_dataTransferId = dlDataTransferFilterDetail.DataTransferId;
				_dataTransferTableDetailId = dlDataTransferFilterDetail.DataTransferTableDetailId;
				_templateTableDetailId = dlDataTransferFilterDetail.TemplateTableDetailId;
				_filterColumn = dlDataTransferFilterDetail.FilterColumn;
				_filterOperator = dlDataTransferFilterDetail.FilterOperator;
				_filterValue = dlDataTransferFilterDetail.FilterValue;
				_isActive = dlDataTransferFilterDetail.IsActive;
				_isDelete = dlDataTransferFilterDetail.IsDelete;
				_enteredBy = dlDataTransferFilterDetail.EnteredBy;
				_enteredDate = dlDataTransferFilterDetail.EnteredDate;
				_updatedBy = dlDataTransferFilterDetail.UpdatedBy;
				_updatedDate = dlDataTransferFilterDetail.UpdatedDate;
				_isDirty = false;
			}
			catch
			{
				RollbackTransaction("updateBLDataTransferFilterDetail");
				throw;
			}
		}
		///<Summary>
		///Delete
		///This method deletes one DataTransferFilterDetail record from the store
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///
		///</parameters>
		public virtual void Delete()
		{
			DLDataTransferFilterDetail dlDataTransferFilterDetail = new DLDataTransferFilterDetail();
			RegisterDataObject(dlDataTransferFilterDetail);
			BeginTransaction("deleteBLDataTransferFilterDetail");
			try
			{
				dlDataTransferFilterDetail.DataTransferFilterDetailId = _dataTransferFilterDetailId;
				dlDataTransferFilterDetail.Delete();
				CommitTransaction();
			}
			catch
			{
				RollbackTransaction("deleteBLDataTransferFilterDetail");
				throw;
			}
		}
		
		///<Summary>
		///DataTransferFilterDetailCollection
		///This method returns the collection of BLDataTransferFilterDetail objects
		///</Summary>
		///<returns>
		///List[BLDataTransferFilterDetail]
		///</returns>
		///<parameters>
		///
		///</parameters>
		public static IList<BLDataTransferFilterDetail> DataTransferFilterDetailCollection()
		{
			try
			{
				IList<BLDataTransferFilterDetail> blDataTransferFilterDetailCollection = new List<BLDataTransferFilterDetail>();
				IList<DLDataTransferFilterDetail> dlDataTransferFilterDetailCollection = DLDataTransferFilterDetail.SelectAll();
			
				foreach(DLDataTransferFilterDetail dlDataTransferFilterDetail in dlDataTransferFilterDetailCollection)
					blDataTransferFilterDetailCollection.Add(new BLDataTransferFilterDetail(dlDataTransferFilterDetail));
			
				return blDataTransferFilterDetailCollection;
			}
			catch
			{
				throw;
			}
		}
		
		
		///<Summary>
		///DataTransferFilterDetailCollectionCount
		///This method returns the collection count of BLDataTransferFilterDetail objects
		///</Summary>
		///<returns>
		///Int32
		///</returns>
		///<parameters>
		///
		///</parameters>
		public static Int32 DataTransferFilterDetailCollectionCount()
		{
			try
			{
				Int32 objCount = DLDataTransferFilterDetail.SelectAllCount();
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
		///List<BLDataTransferFilterDetail>
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
				IDictionary<string, IList<object>> retObj = DLDataTransferFilterDetail.SelectAllByCriteriaProjection(lstDataProjection, lstDataCriteria, lstDataOrder, dataSkip, dataTake);
				return retObj;
			}
			catch
			{
				throw;
			}
		}
		
		
		///<Summary>
		///DataTransferFilterDetailCollection<T>
		///This method implements the IQueryable Collection<T> method, returning a collection of BLDataTransferFilterDetail objects, filtered by optional criteria
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
				IList<T> blDataTransferFilterDetailCollection = new List<T>();
				IList<IDataCriterion> lstDataCriteria = (icriteria == null) ? null : icriteria.ListDataCriteria();
				IList<IDataOrderBy> lstDataOrder = (icriteria == null) ? null : icriteria.ListDataOrder();
				IDataTake dataTake = (icriteria == null) ? null : icriteria.DataTake();
				IDataSkip dataSkip = (icriteria == null) ? null : icriteria.DataSkip();
				IList<DLDataTransferFilterDetail> dlDataTransferFilterDetailCollection = DLDataTransferFilterDetail.SelectAllByCriteria(lstDataCriteria, lstDataOrder, dataSkip, dataTake);
			
				foreach(DLDataTransferFilterDetail resdlDataTransferFilterDetail in dlDataTransferFilterDetailCollection)
					blDataTransferFilterDetailCollection.Add((T)(object)new BLDataTransferFilterDetail(resdlDataTransferFilterDetail));
			
				return blDataTransferFilterDetailCollection;
			}
			catch
			{
				throw;
			}
		}
		
		
		///<Summary>
		///DataTransferFilterDetailCollectionCount
		///This method implements the IQueryable CollectionCount method, returning a collection count of BLDataTransferFilterDetail objects, filtered by optional criteria
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
				Int32 objCount = DLDataTransferFilterDetail.SelectAllByCriteriaCount(lstDataCriteria);
				return objCount;
			}
			catch
			{
				throw;
			}
		}
		
		#endregion

		#region member properties
		
		public virtual Int32? DataTransferFilterDetailId
		{
			get
			{
				 return _dataTransferFilterDetailId;
			}
			set
			{
				_dataTransferFilterDetailId = value;
				_isDirty = true;
			}
		}
		
		public virtual string DataTransferFilterDetailGUID
		{
			get
			{
				 return _dataTransferFilterDetailGUID;
			}
			set
			{
				_dataTransferFilterDetailGUID = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? DataTransferId
		{
			get
			{
				 return _dataTransferId;
			}
			set
			{
				_dataTransferId = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? DataTransferTableDetailId
		{
			get
			{
				 return _dataTransferTableDetailId;
			}
			set
			{
				_dataTransferTableDetailId = value;
				_isDirty = true;
			}
		}
		
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
		
		public virtual string FilterColumn
		{
			get
			{
				 return _filterColumn;
			}
			set
			{
				_filterColumn = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? FilterOperator
		{
			get
			{
				 return _filterOperator;
			}
			set
			{
				_filterOperator = value;
				_isDirty = true;
			}
		}
		
		public virtual string FilterValue
		{
			get
			{
				 return _filterValue;
			}
			set
			{
				_filterValue = value;
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
