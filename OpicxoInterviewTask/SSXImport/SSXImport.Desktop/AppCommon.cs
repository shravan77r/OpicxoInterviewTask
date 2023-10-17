using MySql.Data.MySqlClient;
using SSXImport.MsSQLSupport;
using SSXImport.MySQLSupport;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SSXImport
{
    public class AppCommon
    {
        public static Tuple<bool, string> GetConnectionStatus(DataModel dataModel, int Type = 1)
        {
            bool IsConnectionSuccessful = false;
            var connectionString = string.Empty;
            try
            {
                var dataSourceId = Type == 1 ? dataModel.SourceDataSourceId : dataModel.TargetDataSourceId;
                var database = Type == 1 ? dataModel.SourceDatabase : dataModel.TargetDatabase;
                var server = Type == 1 ? dataModel.SourceServer : dataModel.TargetServer;
                var port = Type == 1 ? dataModel.SourcePort : dataModel.TargetPort;
                var username = Type == 1 ? dataModel.SourceUsername : dataModel.TargetUsername;
                var password = Type == 1 ? dataModel.SourcePassword : dataModel.TargetPassword;

                if (!string.IsNullOrEmpty(server)
                    && !string.IsNullOrEmpty(port)
                    && !string.IsNullOrEmpty(username)
                    && !string.IsNullOrEmpty(password))

                {
                    if (dataSourceId.Equals(2))
                    {
                        var msSqlConnectionStringBuilder = new SqlConnectionStringBuilder();
                        msSqlConnectionStringBuilder.DataSource = string.Format("{0},{1}",
                            server, port);

                        msSqlConnectionStringBuilder.UserID = username;
                        msSqlConnectionStringBuilder.Password = password;
                        if (!string.IsNullOrEmpty(database))
                            msSqlConnectionStringBuilder.InitialCatalog = database;

                        connectionString = msSqlConnectionStringBuilder.ConnectionString;

                        IsConnectionSuccessful = MsSQLConnectionSupport.CheckCredentials(connectionString);

                    }
                    if (dataSourceId.Equals(3))
                    {
                        var mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder();
                        mySqlConnectionStringBuilder.Server = server;
                        mySqlConnectionStringBuilder.Port = Convert.ToUInt32(port);
                        mySqlConnectionStringBuilder.UserID = username;
                        mySqlConnectionStringBuilder.Password = password;
                        if (!string.IsNullOrEmpty(database))
                            mySqlConnectionStringBuilder.Database = database;

                        connectionString = mySqlConnectionStringBuilder.GetConnectionString(true);

                        IsConnectionSuccessful = MySQLConnectionSupport.CheckCredentials(connectionString);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return Tuple.Create(IsConnectionSuccessful, connectionString);
        }

        public static List<ComboBoxItem> GetDataSourceItems()
        {
            var comboBoxItems = new List<ComboBoxItem>();
            comboBoxItems.Add(new ComboBoxItem
            {
                Tag = AppConstant.DataSource_Excel,
                Content = AppConstant.DataSource_Excel_Name,
            });
            comboBoxItems.Add(new ComboBoxItem
            {
                Tag = AppConstant.DataSource_MsSQL,
                Content = AppConstant.DataSource_MsSQL_Name,
            });
            comboBoxItems.Add(new ComboBoxItem
            {
                Tag = AppConstant.DataSource_MySQL,
                Content = AppConstant.DataSource_MySQL_Name,
            });
            comboBoxItems.Add(new ComboBoxItem
            {
                Tag = AppConstant.DataSource_API,
                Content = AppConstant.DataSource_API_Name,
                IsEnabled = false
            });
            return comboBoxItems;
        }
    }
}
