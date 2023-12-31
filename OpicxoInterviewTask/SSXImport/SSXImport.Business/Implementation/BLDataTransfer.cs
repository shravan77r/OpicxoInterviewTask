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
	///This is the definition of the class BLDataTransfer.
	///</Summary>
	public partial class BLDataTransfer : SSXImport_BaseBusiness, IQueryableCollection
	{
		#region member variables
		protected Int32? _dataTransferId;
		protected string _dataTransferGUID;
		protected Int32? _templateId;
		protected Int32? _originSourceTypeId;
		protected Int32? _originSourceAPITemplateId;
		protected Int32? _originSourceFileTypeId;
		protected string _originSourceServer;
		protected string _originSourcePort;
		protected string _originSourceUsername;
		protected string _originSourcePassword;
		protected string _originSourceDatabase;
		protected string _originSourceFilePath;
		protected string _originSourceFileName;
		protected bool? _isFirstColumnContainHeader;
		protected Int32? _targetSourceTypeId;
		protected Int32? _targetSourceAPITemplateId;
		protected string _targetSourceServer;
		protected string _targetSourcePort;
		protected string _targetSourceUsername;
		protected string _targetSourcePassword;
		protected string _targetSourceDatabase;
		protected DateTime? _transferDate;
		protected Int32? _transferStatus;
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
		public BLDataTransfer()
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
		///Int32 dataTransferId
		///</parameters>
		public BLDataTransfer(Int32 dataTransferId)
		{
			try
			{
				DLDataTransfer dlDataTransfer = DLDataTransfer.SelectOne(dataTransferId);
				_dataTransferId = dlDataTransfer.DataTransferId;
				_dataTransferGUID = dlDataTransfer.DataTransferGUID;
				_templateId = dlDataTransfer.TemplateId;
				_originSourceTypeId = dlDataTransfer.OriginSourceTypeId;
				_originSourceAPITemplateId = dlDataTransfer.OriginSourceAPITemplateId;
				_originSourceFileTypeId = dlDataTransfer.OriginSourceFileTypeId;
				_originSourceServer = dlDataTransfer.OriginSourceServer;
				_originSourcePort = dlDataTransfer.OriginSourcePort;
				_originSourceUsername = dlDataTransfer.OriginSourceUsername;
				_originSourcePassword = dlDataTransfer.OriginSourcePassword;
				_originSourceDatabase = dlDataTransfer.OriginSourceDatabase;
				_originSourceFilePath = dlDataTransfer.OriginSourceFilePath;
				_originSourceFileName = dlDataTransfer.OriginSourceFileName;
				_isFirstColumnContainHeader = dlDataTransfer.IsFirstColumnContainHeader;
				_targetSourceTypeId = dlDataTransfer.TargetSourceTypeId;
				_targetSourceAPITemplateId = dlDataTransfer.TargetSourceAPITemplateId;
				_targetSourceServer = dlDataTransfer.TargetSourceServer;
				_targetSourcePort = dlDataTransfer.TargetSourcePort;
				_targetSourceUsername = dlDataTransfer.TargetSourceUsername;
				_targetSourcePassword = dlDataTransfer.TargetSourcePassword;
				_targetSourceDatabase = dlDataTransfer.TargetSourceDatabase;
				_transferDate = dlDataTransfer.TransferDate;
				_transferStatus = dlDataTransfer.TransferStatus;
				_isActive = dlDataTransfer.IsActive;
				_isDelete = dlDataTransfer.IsDelete;
				_enteredBy = dlDataTransfer.EnteredBy;
				_enteredDate = dlDataTransfer.EnteredDate;
				_updatedBy = dlDataTransfer.UpdatedBy;
				_updatedDate = dlDataTransfer.UpdatedDate;
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
		///DataTransferGUID
		///</parameters>
		public BLDataTransfer(string dataTransferGUID)
		{
			try
			{
				DLDataTransfer dlDataTransfer = DLDataTransfer.SelectOneByDataTransferGUID(dataTransferGUID);
				_dataTransferId = dlDataTransfer.DataTransferId;
				_dataTransferGUID = dlDataTransfer.DataTransferGUID;
				_templateId = dlDataTransfer.TemplateId;
				_originSourceTypeId = dlDataTransfer.OriginSourceTypeId;
				_originSourceAPITemplateId = dlDataTransfer.OriginSourceAPITemplateId;
				_originSourceFileTypeId = dlDataTransfer.OriginSourceFileTypeId;
				_originSourceServer = dlDataTransfer.OriginSourceServer;
				_originSourcePort = dlDataTransfer.OriginSourcePort;
				_originSourceUsername = dlDataTransfer.OriginSourceUsername;
				_originSourcePassword = dlDataTransfer.OriginSourcePassword;
				_originSourceDatabase = dlDataTransfer.OriginSourceDatabase;
				_originSourceFilePath = dlDataTransfer.OriginSourceFilePath;
				_originSourceFileName = dlDataTransfer.OriginSourceFileName;
				_isFirstColumnContainHeader = dlDataTransfer.IsFirstColumnContainHeader;
				_targetSourceTypeId = dlDataTransfer.TargetSourceTypeId;
				_targetSourceAPITemplateId = dlDataTransfer.TargetSourceAPITemplateId;
				_targetSourceServer = dlDataTransfer.TargetSourceServer;
				_targetSourcePort = dlDataTransfer.TargetSourcePort;
				_targetSourceUsername = dlDataTransfer.TargetSourceUsername;
				_targetSourcePassword = dlDataTransfer.TargetSourcePassword;
				_targetSourceDatabase = dlDataTransfer.TargetSourceDatabase;
				_transferDate = dlDataTransfer.TransferDate;
				_transferStatus = dlDataTransfer.TransferStatus;
				_isActive = dlDataTransfer.IsActive;
				_isDelete = dlDataTransfer.IsDelete;
				_enteredBy = dlDataTransfer.EnteredBy;
				_enteredDate = dlDataTransfer.EnteredDate;
				_updatedBy = dlDataTransfer.UpdatedBy;
				_updatedDate = dlDataTransfer.UpdatedDate;
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
		///DLDataTransfer
		///</parameters>
		protected internal BLDataTransfer(DLDataTransfer dlDataTransfer)
		{
			try
			{
				_dataTransferId = dlDataTransfer.DataTransferId;
				_dataTransferGUID = dlDataTransfer.DataTransferGUID;
				_templateId = dlDataTransfer.TemplateId;
				_originSourceTypeId = dlDataTransfer.OriginSourceTypeId;
				_originSourceAPITemplateId = dlDataTransfer.OriginSourceAPITemplateId;
				_originSourceFileTypeId = dlDataTransfer.OriginSourceFileTypeId;
				_originSourceServer = dlDataTransfer.OriginSourceServer;
				_originSourcePort = dlDataTransfer.OriginSourcePort;
				_originSourceUsername = dlDataTransfer.OriginSourceUsername;
				_originSourcePassword = dlDataTransfer.OriginSourcePassword;
				_originSourceDatabase = dlDataTransfer.OriginSourceDatabase;
				_originSourceFilePath = dlDataTransfer.OriginSourceFilePath;
				_originSourceFileName = dlDataTransfer.OriginSourceFileName;
				_isFirstColumnContainHeader = dlDataTransfer.IsFirstColumnContainHeader;
				_targetSourceTypeId = dlDataTransfer.TargetSourceTypeId;
				_targetSourceAPITemplateId = dlDataTransfer.TargetSourceAPITemplateId;
				_targetSourceServer = dlDataTransfer.TargetSourceServer;
				_targetSourcePort = dlDataTransfer.TargetSourcePort;
				_targetSourceUsername = dlDataTransfer.TargetSourceUsername;
				_targetSourcePassword = dlDataTransfer.TargetSourcePassword;
				_targetSourceDatabase = dlDataTransfer.TargetSourceDatabase;
				_transferDate = dlDataTransfer.TransferDate;
				_transferStatus = dlDataTransfer.TransferStatus;
				_isActive = dlDataTransfer.IsActive;
				_isDelete = dlDataTransfer.IsDelete;
				_enteredBy = dlDataTransfer.EnteredBy;
				_enteredDate = dlDataTransfer.EnteredDate;
				_updatedBy = dlDataTransfer.UpdatedBy;
				_updatedDate = dlDataTransfer.UpdatedDate;
			}
			catch
			{
				throw;
			}
		}

		///<Summary>
		///SaveNew
		///This method persists a new DataTransfer record to the store
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///
		///</parameters>
		public virtual void SaveNew()
		{
			DLDataTransfer dlDataTransfer = new DLDataTransfer();
			RegisterDataObject(dlDataTransfer);
			BeginTransaction("savenewBLDataTransfer");
			try
			{
				dlDataTransfer.DataTransferGUID = _dataTransferGUID;
				dlDataTransfer.TemplateId = _templateId;
				dlDataTransfer.OriginSourceTypeId = _originSourceTypeId;
				dlDataTransfer.OriginSourceAPITemplateId = _originSourceAPITemplateId;
				dlDataTransfer.OriginSourceFileTypeId = _originSourceFileTypeId;
				dlDataTransfer.OriginSourceServer = _originSourceServer;
				dlDataTransfer.OriginSourcePort = _originSourcePort;
				dlDataTransfer.OriginSourceUsername = _originSourceUsername;
				dlDataTransfer.OriginSourcePassword = _originSourcePassword;
				dlDataTransfer.OriginSourceDatabase = _originSourceDatabase;
				dlDataTransfer.OriginSourceFilePath = _originSourceFilePath;
				dlDataTransfer.OriginSourceFileName = _originSourceFileName;
				dlDataTransfer.IsFirstColumnContainHeader = _isFirstColumnContainHeader;
				dlDataTransfer.TargetSourceTypeId = _targetSourceTypeId;
				dlDataTransfer.TargetSourceAPITemplateId = _targetSourceAPITemplateId;
				dlDataTransfer.TargetSourceServer = _targetSourceServer;
				dlDataTransfer.TargetSourcePort = _targetSourcePort;
				dlDataTransfer.TargetSourceUsername = _targetSourceUsername;
				dlDataTransfer.TargetSourcePassword = _targetSourcePassword;
				dlDataTransfer.TargetSourceDatabase = _targetSourceDatabase;
				dlDataTransfer.TransferDate = _transferDate;
				dlDataTransfer.TransferStatus = _transferStatus;
				dlDataTransfer.IsActive = _isActive;
				dlDataTransfer.IsDelete = _isDelete;
				dlDataTransfer.EnteredBy = _enteredBy;
				dlDataTransfer.EnteredDate = _enteredDate;
				dlDataTransfer.UpdatedBy = _updatedBy;
				dlDataTransfer.UpdatedDate = _updatedDate;
				dlDataTransfer.Insert();
				CommitTransaction();
				
				_dataTransferId = dlDataTransfer.DataTransferId;
				_dataTransferGUID = dlDataTransfer.DataTransferGUID;
				_templateId = dlDataTransfer.TemplateId;
				_originSourceTypeId = dlDataTransfer.OriginSourceTypeId;
				_originSourceAPITemplateId = dlDataTransfer.OriginSourceAPITemplateId;
				_originSourceFileTypeId = dlDataTransfer.OriginSourceFileTypeId;
				_originSourceServer = dlDataTransfer.OriginSourceServer;
				_originSourcePort = dlDataTransfer.OriginSourcePort;
				_originSourceUsername = dlDataTransfer.OriginSourceUsername;
				_originSourcePassword = dlDataTransfer.OriginSourcePassword;
				_originSourceDatabase = dlDataTransfer.OriginSourceDatabase;
				_originSourceFilePath = dlDataTransfer.OriginSourceFilePath;
				_originSourceFileName = dlDataTransfer.OriginSourceFileName;
				_isFirstColumnContainHeader = dlDataTransfer.IsFirstColumnContainHeader;
				_targetSourceTypeId = dlDataTransfer.TargetSourceTypeId;
				_targetSourceAPITemplateId = dlDataTransfer.TargetSourceAPITemplateId;
				_targetSourceServer = dlDataTransfer.TargetSourceServer;
				_targetSourcePort = dlDataTransfer.TargetSourcePort;
				_targetSourceUsername = dlDataTransfer.TargetSourceUsername;
				_targetSourcePassword = dlDataTransfer.TargetSourcePassword;
				_targetSourceDatabase = dlDataTransfer.TargetSourceDatabase;
				_transferDate = dlDataTransfer.TransferDate;
				_transferStatus = dlDataTransfer.TransferStatus;
				_isActive = dlDataTransfer.IsActive;
				_isDelete = dlDataTransfer.IsDelete;
				_enteredBy = dlDataTransfer.EnteredBy;
				_enteredDate = dlDataTransfer.EnteredDate;
				_updatedBy = dlDataTransfer.UpdatedBy;
				_updatedDate = dlDataTransfer.UpdatedDate;
				_isDirty = false;
			}
			catch
			{
				RollbackTransaction("savenewBLDataTransfer");
				throw;
			}
		}
		
		///<Summary>
		///Update
		///This method updates one DataTransfer record in the store
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///BLDataTransfer
		///</parameters>
		public virtual void Update()
		{
			DLDataTransfer dlDataTransfer = new DLDataTransfer();
			RegisterDataObject(dlDataTransfer);
			BeginTransaction("updateBLDataTransfer");
			try
			{
				dlDataTransfer.DataTransferId = _dataTransferId;
				dlDataTransfer.DataTransferGUID = _dataTransferGUID;
				dlDataTransfer.TemplateId = _templateId;
				dlDataTransfer.OriginSourceTypeId = _originSourceTypeId;
				dlDataTransfer.OriginSourceAPITemplateId = _originSourceAPITemplateId;
				dlDataTransfer.OriginSourceFileTypeId = _originSourceFileTypeId;
				dlDataTransfer.OriginSourceServer = _originSourceServer;
				dlDataTransfer.OriginSourcePort = _originSourcePort;
				dlDataTransfer.OriginSourceUsername = _originSourceUsername;
				dlDataTransfer.OriginSourcePassword = _originSourcePassword;
				dlDataTransfer.OriginSourceDatabase = _originSourceDatabase;
				dlDataTransfer.OriginSourceFilePath = _originSourceFilePath;
				dlDataTransfer.OriginSourceFileName = _originSourceFileName;
				dlDataTransfer.IsFirstColumnContainHeader = _isFirstColumnContainHeader;
				dlDataTransfer.TargetSourceTypeId = _targetSourceTypeId;
				dlDataTransfer.TargetSourceAPITemplateId = _targetSourceAPITemplateId;
				dlDataTransfer.TargetSourceServer = _targetSourceServer;
				dlDataTransfer.TargetSourcePort = _targetSourcePort;
				dlDataTransfer.TargetSourceUsername = _targetSourceUsername;
				dlDataTransfer.TargetSourcePassword = _targetSourcePassword;
				dlDataTransfer.TargetSourceDatabase = _targetSourceDatabase;
				dlDataTransfer.TransferDate = _transferDate;
				dlDataTransfer.TransferStatus = _transferStatus;
				dlDataTransfer.IsActive = _isActive;
				dlDataTransfer.IsDelete = _isDelete;
				dlDataTransfer.EnteredBy = _enteredBy;
				dlDataTransfer.EnteredDate = _enteredDate;
				dlDataTransfer.UpdatedBy = _updatedBy;
				dlDataTransfer.UpdatedDate = _updatedDate;
				dlDataTransfer.Update();
				CommitTransaction();
				
				_dataTransferId = dlDataTransfer.DataTransferId;
				_dataTransferGUID = dlDataTransfer.DataTransferGUID;
				_templateId = dlDataTransfer.TemplateId;
				_originSourceTypeId = dlDataTransfer.OriginSourceTypeId;
				_originSourceAPITemplateId = dlDataTransfer.OriginSourceAPITemplateId;
				_originSourceFileTypeId = dlDataTransfer.OriginSourceFileTypeId;
				_originSourceServer = dlDataTransfer.OriginSourceServer;
				_originSourcePort = dlDataTransfer.OriginSourcePort;
				_originSourceUsername = dlDataTransfer.OriginSourceUsername;
				_originSourcePassword = dlDataTransfer.OriginSourcePassword;
				_originSourceDatabase = dlDataTransfer.OriginSourceDatabase;
				_originSourceFilePath = dlDataTransfer.OriginSourceFilePath;
				_originSourceFileName = dlDataTransfer.OriginSourceFileName;
				_isFirstColumnContainHeader = dlDataTransfer.IsFirstColumnContainHeader;
				_targetSourceTypeId = dlDataTransfer.TargetSourceTypeId;
				_targetSourceAPITemplateId = dlDataTransfer.TargetSourceAPITemplateId;
				_targetSourceServer = dlDataTransfer.TargetSourceServer;
				_targetSourcePort = dlDataTransfer.TargetSourcePort;
				_targetSourceUsername = dlDataTransfer.TargetSourceUsername;
				_targetSourcePassword = dlDataTransfer.TargetSourcePassword;
				_targetSourceDatabase = dlDataTransfer.TargetSourceDatabase;
				_transferDate = dlDataTransfer.TransferDate;
				_transferStatus = dlDataTransfer.TransferStatus;
				_isActive = dlDataTransfer.IsActive;
				_isDelete = dlDataTransfer.IsDelete;
				_enteredBy = dlDataTransfer.EnteredBy;
				_enteredDate = dlDataTransfer.EnteredDate;
				_updatedBy = dlDataTransfer.UpdatedBy;
				_updatedDate = dlDataTransfer.UpdatedDate;
				_isDirty = false;
			}
			catch
			{
				RollbackTransaction("updateBLDataTransfer");
				throw;
			}
		}
		///<Summary>
		///Delete
		///This method deletes one DataTransfer record from the store
		///</Summary>
		///<returns>
		///void
		///</returns>
		///<parameters>
		///
		///</parameters>
		public virtual void Delete()
		{
			DLDataTransfer dlDataTransfer = new DLDataTransfer();
			RegisterDataObject(dlDataTransfer);
			BeginTransaction("deleteBLDataTransfer");
			try
			{
				dlDataTransfer.DataTransferId = _dataTransferId;
				dlDataTransfer.Delete();
				CommitTransaction();
			}
			catch
			{
				RollbackTransaction("deleteBLDataTransfer");
				throw;
			}
		}
		
		///<Summary>
		///DataTransferCollection
		///This method returns the collection of BLDataTransfer objects
		///</Summary>
		///<returns>
		///List[BLDataTransfer]
		///</returns>
		///<parameters>
		///
		///</parameters>
		public static IList<BLDataTransfer> DataTransferCollection()
		{
			try
			{
				IList<BLDataTransfer> blDataTransferCollection = new List<BLDataTransfer>();
				IList<DLDataTransfer> dlDataTransferCollection = DLDataTransfer.SelectAll();
			
				foreach(DLDataTransfer dlDataTransfer in dlDataTransferCollection)
					blDataTransferCollection.Add(new BLDataTransfer(dlDataTransfer));
			
				return blDataTransferCollection;
			}
			catch
			{
				throw;
			}
		}
		
		
		///<Summary>
		///DataTransferCollectionCount
		///This method returns the collection count of BLDataTransfer objects
		///</Summary>
		///<returns>
		///Int32
		///</returns>
		///<parameters>
		///
		///</parameters>
		public static Int32 DataTransferCollectionCount()
		{
			try
			{
				Int32 objCount = DLDataTransfer.SelectAllCount();
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
		///List<BLDataTransfer>
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
				IDictionary<string, IList<object>> retObj = DLDataTransfer.SelectAllByCriteriaProjection(lstDataProjection, lstDataCriteria, lstDataOrder, dataSkip, dataTake);
				return retObj;
			}
			catch
			{
				throw;
			}
		}
		
		
		///<Summary>
		///DataTransferCollection<T>
		///This method implements the IQueryable Collection<T> method, returning a collection of BLDataTransfer objects, filtered by optional criteria
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
				IList<T> blDataTransferCollection = new List<T>();
				IList<IDataCriterion> lstDataCriteria = (icriteria == null) ? null : icriteria.ListDataCriteria();
				IList<IDataOrderBy> lstDataOrder = (icriteria == null) ? null : icriteria.ListDataOrder();
				IDataTake dataTake = (icriteria == null) ? null : icriteria.DataTake();
				IDataSkip dataSkip = (icriteria == null) ? null : icriteria.DataSkip();
				IList<DLDataTransfer> dlDataTransferCollection = DLDataTransfer.SelectAllByCriteria(lstDataCriteria, lstDataOrder, dataSkip, dataTake);
			
				foreach(DLDataTransfer resdlDataTransfer in dlDataTransferCollection)
					blDataTransferCollection.Add((T)(object)new BLDataTransfer(resdlDataTransfer));
			
				return blDataTransferCollection;
			}
			catch
			{
				throw;
			}
		}
		
		
		///<Summary>
		///DataTransferCollectionCount
		///This method implements the IQueryable CollectionCount method, returning a collection count of BLDataTransfer objects, filtered by optional criteria
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
				Int32 objCount = DLDataTransfer.SelectAllByCriteriaCount(lstDataCriteria);
				return objCount;
			}
			catch
			{
				throw;
			}
		}
		
		#endregion

		#region member properties
		
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
		
		public virtual string DataTransferGUID
		{
			get
			{
				 return _dataTransferGUID;
			}
			set
			{
				_dataTransferGUID = value;
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
		
		public virtual Int32? OriginSourceTypeId
		{
			get
			{
				 return _originSourceTypeId;
			}
			set
			{
				_originSourceTypeId = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? OriginSourceAPITemplateId
		{
			get
			{
				 return _originSourceAPITemplateId;
			}
			set
			{
				_originSourceAPITemplateId = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? OriginSourceFileTypeId
		{
			get
			{
				 return _originSourceFileTypeId;
			}
			set
			{
				_originSourceFileTypeId = value;
				_isDirty = true;
			}
		}
		
		public virtual string OriginSourceServer
		{
			get
			{
				 return _originSourceServer;
			}
			set
			{
				_originSourceServer = value;
				_isDirty = true;
			}
		}
		
		public virtual string OriginSourcePort
		{
			get
			{
				 return _originSourcePort;
			}
			set
			{
				_originSourcePort = value;
				_isDirty = true;
			}
		}
		
		public virtual string OriginSourceUsername
		{
			get
			{
				 return _originSourceUsername;
			}
			set
			{
				_originSourceUsername = value;
				_isDirty = true;
			}
		}
		
		public virtual string OriginSourcePassword
		{
			get
			{
				 return _originSourcePassword;
			}
			set
			{
				_originSourcePassword = value;
				_isDirty = true;
			}
		}
		
		public virtual string OriginSourceDatabase
		{
			get
			{
				 return _originSourceDatabase;
			}
			set
			{
				_originSourceDatabase = value;
				_isDirty = true;
			}
		}
		
		public virtual string OriginSourceFilePath
		{
			get
			{
				 return _originSourceFilePath;
			}
			set
			{
				_originSourceFilePath = value;
				_isDirty = true;
			}
		}
		
		public virtual string OriginSourceFileName
		{
			get
			{
				 return _originSourceFileName;
			}
			set
			{
				_originSourceFileName = value;
				_isDirty = true;
			}
		}
		
		public virtual bool? IsFirstColumnContainHeader
		{
			get
			{
				 return _isFirstColumnContainHeader;
			}
			set
			{
				_isFirstColumnContainHeader = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? TargetSourceTypeId
		{
			get
			{
				 return _targetSourceTypeId;
			}
			set
			{
				_targetSourceTypeId = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? TargetSourceAPITemplateId
		{
			get
			{
				 return _targetSourceAPITemplateId;
			}
			set
			{
				_targetSourceAPITemplateId = value;
				_isDirty = true;
			}
		}
		
		public virtual string TargetSourceServer
		{
			get
			{
				 return _targetSourceServer;
			}
			set
			{
				_targetSourceServer = value;
				_isDirty = true;
			}
		}
		
		public virtual string TargetSourcePort
		{
			get
			{
				 return _targetSourcePort;
			}
			set
			{
				_targetSourcePort = value;
				_isDirty = true;
			}
		}
		
		public virtual string TargetSourceUsername
		{
			get
			{
				 return _targetSourceUsername;
			}
			set
			{
				_targetSourceUsername = value;
				_isDirty = true;
			}
		}
		
		public virtual string TargetSourcePassword
		{
			get
			{
				 return _targetSourcePassword;
			}
			set
			{
				_targetSourcePassword = value;
				_isDirty = true;
			}
		}
		
		public virtual string TargetSourceDatabase
		{
			get
			{
				 return _targetSourceDatabase;
			}
			set
			{
				_targetSourceDatabase = value;
				_isDirty = true;
			}
		}
		
		public virtual DateTime? TransferDate
		{
			get
			{
				 return _transferDate;
			}
			set
			{
				_transferDate = value;
				_isDirty = true;
			}
		}
		
		public virtual Int32? TransferStatus
		{
			get
			{
				 return _transferStatus;
			}
			set
			{
				_transferStatus = value;
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
