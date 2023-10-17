﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXImport.MsSQLSupport
{
    public partial class SQL_TxConnectionProvider : IDisposable
    {
        protected bool _isDisposed;
        protected SqlConnection _txConnection;
        protected SqlTransaction _currTransaction;
        static string _connectionString;

        public SQL_TxConnectionProvider()
        {
            Init();
        }

        private void Init()
        {
            _txConnection = new SqlConnection();
            _txConnection.ConnectionString = ConnectionString;
            _currTransaction = null;
            _isDisposed = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                if (isDisposing)
                {
                    if (_currTransaction != null)
                    {
                        _currTransaction.Dispose();
                        _currTransaction = null;
                    }
                    if (_txConnection != null)
                    {
                        /*this will also rollback any pending transactions on this connection*/
                        _txConnection.Close();
                        _txConnection.Dispose();
                        _txConnection = null;
                    }
                }
            }
            _isDisposed = true;
        }

        public virtual void OpenConnection()
        {
            try
            {
                if (_txConnection.State == ConnectionState.Open)
                    throw new Exception("Connection is already open");

                _txConnection.Open();
                _currTransaction = null;
                _isDisposed = false;
            }
            catch
            {
                throw;
            }
        }

        public virtual void CloseConnection(bool commit)
        {
            if ((_txConnection == null) || (_txConnection.State != ConnectionState.Open))
                return;
            try
            {
                if ((_currTransaction != null) && commit)
                    _currTransaction.Commit();

                else if (_currTransaction != null)
                    _currTransaction.Rollback();

                if (_currTransaction != null)
                    _currTransaction.Dispose();

                _currTransaction = null;
                _txConnection.Close();
            }
            catch
            {
                throw;
            }
        }

        public virtual void BeginTransaction(string trans)
        {
            if (_currTransaction != null)
                throw new Exception("Transaction nesting not allowed");

            if ((_txConnection == null) || (_txConnection.State != ConnectionState.Open))
                throw new Exception("Connection not open");

            try
            {
                _currTransaction = _txConnection.BeginTransaction(IsolationLevel.ReadCommitted, trans);
            }
            catch
            {
                throw;
            }
        }

        public virtual void CommitTransaction()
        {
            if (_currTransaction == null)
                throw new Exception("No Transaction to commit");

            if ((_txConnection == null) || (_txConnection.State != ConnectionState.Open))
                throw new Exception("Connection not open");

            try
            {
                _currTransaction.Commit();
                _currTransaction.Dispose();
                _currTransaction = null;
            }
            catch
            {
                throw;
            }
        }

        public virtual void RollbackTransaction(string trans)
        {
            if (_currTransaction == null)
                throw new Exception("No Transaction to rollback");

            if ((_txConnection == null) || (_txConnection.State != ConnectionState.Open))
                throw new Exception("Connection not open");

            try
            {
                _currTransaction.Rollback(trans);
                _currTransaction.Dispose();
                _currTransaction = null;
            }
            catch
            {
                throw;
            }
        }

        public virtual SqlTransaction CurrentTransaction
        {
            get
            {
                return _currTransaction;
            }
        }

        public virtual SqlConnection Connection
        {
            get
            {
                return _txConnection;
            }
        }

        public static string ConnectionString
        {
            set { _connectionString = value; }
            get
            {
                if (!string.IsNullOrEmpty(_connectionString))
                    return _connectionString;

                var cons = System.Configuration.ConfigurationSettings.AppSettings["connection"].ToString().Split(';')[3].Remove(0, 9);
                var decon = cons;
                cons = System.Configuration.ConfigurationSettings.AppSettings["connection"].ToString().Replace(cons, decon);
                return cons;
            }
        }
    }
}
