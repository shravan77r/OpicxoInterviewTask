using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSXImport.MsSQLSupport
{
    public partial class SQL_BaseData : IDisposable
    {
        #region members
        protected SQL_TxConnectionProvider _connectionProvider;
        static string _staticConnectionString;
        bool _isDisposed = false;
        #endregion

        #region initialisation
        public SQL_BaseData()
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
            if (!_isDisposed)
            {
                if (isDisposing)
                {
                    if (_connectionProvider != null)
                    {
                        ((IDisposable)_connectionProvider).Dispose();
                        _connectionProvider = null;
                    }
                }
            }
            _isDisposed = true;
        }
        #endregion

        #region connection support
        public static SqlConnection StaticSqlConnection
        {
            get
            {
                SqlConnection staticConnection = new SqlConnection();
                staticConnection.ConnectionString = StaticConnectionString;
                return staticConnection;
            }
        }

        public virtual SQL_TxConnectionProvider ConnectionProvider
        {
            set
            {
                if (value == null)
                    throw new Exception("Connection provider cannot be null");

                _connectionProvider = value;
            }
        }

        public static string StaticConnectionString
        {
            set { _staticConnectionString = value; }
            get
            {
                if (!string.IsNullOrEmpty(_staticConnectionString))
                    return _staticConnectionString;

                var cons = System.Configuration.ConfigurationSettings.AppSettings["connection"].ToString().Split(';')[3].Remove(0, 9);
                var decon = cons;
                cons = System.Configuration.ConfigurationSettings.AppSettings["connection"].ToString().Replace(cons, decon);
                return cons;
            }
        }
        #endregion

    }
}
