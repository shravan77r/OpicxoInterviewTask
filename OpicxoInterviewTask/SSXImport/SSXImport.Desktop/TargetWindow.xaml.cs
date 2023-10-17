using SSXImport.MsSQLSupport;
using SSXImport.MySQLSupport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SSXImport
{
    /// <summary>
    /// Interaction logic for Target.xaml
    /// </summary>
    public partial class TargetWindow : Window
    {
        DataModel dataModel = new DataModel();

        public TargetWindow()
        {
            InitializeComponent();

            var comboBoxItems = AppCommon.GetDataSourceItems();

            cboDataSource.ItemsSource = comboBoxItems;
            cboDataSource.SelectedValuePath = "Tag";

            foreach (var item in cboDataSource.Items)
            {
                var comboItem = (ComboBoxItem)item;
                if (comboItem.Tag.Equals(AppConstant.DataSource_MsSQL))
                    cboDataSource.SelectedItem = comboItem;
            }
        }

        public TargetWindow(DataModel dataModel)
        {
            this.dataModel = dataModel;

            InitializeComponent();

            var comboBoxItems = AppCommon.GetDataSourceItems();

            cboDataSource.ItemsSource = comboBoxItems;
            cboDataSource.SelectedValuePath = "Tag";

            if (dataModel.TargetDataSourceId > 0)
                foreach (var item in cboDataSource.Items)
                {
                    var comboItem = (ComboBoxItem)item;
                    if (comboItem.Tag.Equals(dataModel.TargetDataSourceId))
                    {
                        cboDataSource.SelectedItem = comboItem;
                        break;
                    }
                }

            if (dataModel.TargetDataSourceId.Equals(AppConstant.DataSource_Excel))
            {
                txtFileName.Text = dataModel.TargetFile;
            }
            else if (dataModel.TargetDataSourceId.Equals(AppConstant.DataSource_MsSQL)
                || dataModel.TargetDataSourceId.Equals(AppConstant.DataSource_MySQL))
            {
                txtServer.Text = dataModel.TargetServer;
                txtUsername.Text = dataModel.TargetUsername;
                txtPassword.Text = dataModel.TargetPassword;
                if (string.IsNullOrEmpty(dataModel.TargetPort))
                {
                    if (dataModel.TargetDataSourceId.Equals(AppConstant.DataSource_MsSQL))
                        txtPort.Text = AppConstant.MsSQL_DefaultPort;
                    else if (dataModel.TargetDataSourceId.Equals(AppConstant.DataSource_MySQL))
                        txtPort.Text = AppConstant.MySQL_DefaultPort;
                }
                else
                    txtPort.Text = dataModel.TargetPort;

                BindDatabaseComboBox(dataModel, false);
                if (!string.IsNullOrEmpty(dataModel.TargetDatabase))
                    foreach (var item in cboDatabase.Items)
                    {
                        var comboItem = (DataRowView)item;
                        if (comboItem.Row["Name"].Equals(dataModel.TargetDatabase.ToString()))
                        {
                            cboDatabase.SelectedItem = comboItem;
                            break;
                        }
                    }
                if (!string.IsNullOrEmpty(dataModel.TargetTable))
                    foreach (var item in cboTables.Items)
                    {
                        var comboItem = (DataRowView)item;
                        if (comboItem.Row["Name"].Equals(dataModel.TargetTable.ToString()))
                        {
                            cboTables.SelectedItem = comboItem;
                            break;
                        }
                    }
            }
        }

        private void cboDataSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataSourceId = (sender as ComboBox).SelectedValue as int?;
            if (dataSourceId.GetValueOrDefault(0) > 0)
            {
                dataModel.TargetDataSourceId = Convert.ToInt32(dataSourceId);
                if (dataSourceId.Equals(AppConstant.DataSource_Excel))
                {
                    grd_Excel.Visibility = Visibility.Visible;
                    grd_Database.Visibility = Visibility.Hidden;
                }
                else if (dataSourceId.Equals(AppConstant.DataSource_MsSQL))
                {
                    grd_Excel.Visibility = Visibility.Hidden;
                    grd_Database.Visibility = Visibility.Visible;
                    txtPort.Text = AppConstant.MsSQL_DefaultPort;
                }
                else if (dataSourceId.Equals(AppConstant.DataSource_MySQL))
                {
                    grd_Excel.Visibility = Visibility.Hidden;
                    grd_Database.Visibility = Visibility.Visible;
                    txtPort.Text = AppConstant.MySQL_DefaultPort;
                }
                else
                {
                    grd_Excel.Visibility = Visibility.Hidden;
                    grd_Database.Visibility = Visibility.Hidden;
                }
            }
        }

        private void btnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            dataModel.TargetServer = txtServer.Text;
            if (!string.IsNullOrEmpty(txtPort.Text))
                dataModel.TargetPort = txtPort.Text;
            else
            {
                if (cboDataSource.SelectedValue.Equals(AppConstant.DataSource_MsSQL))
                    dataModel.TargetPort = AppConstant.MsSQL_DefaultPort;
                else
                    dataModel.TargetPort = AppConstant.MySQL_DefaultPort;
            }

            dataModel.TargetUsername = txtUsername.Text;
            dataModel.TargetPassword = txtPassword.Text;
            if (string.IsNullOrEmpty(dataModel.TargetServer))
            {
                MessageBox.Show(this, "Please Enter Server");
                txtServer.Focus();
            }
            else if (string.IsNullOrEmpty(dataModel.TargetUsername))
            {
                MessageBox.Show(this, "Please Enter Username");
                txtUsername.Focus();
            }
            else if (string.IsNullOrEmpty(dataModel.TargetPassword))
            {
                MessageBox.Show(this, "Please Enter Password");
                txtPassword.Focus();
            }
            else
                BindDatabaseComboBox(dataModel);
        }

        private void cboDatabase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPort.Text))
                dataModel.TargetPort = txtPort.Text;
            else
            {
                if (cboDataSource.SelectedValue.Equals(AppConstant.DataSource_MsSQL))
                    dataModel.TargetPort = AppConstant.MsSQL_DefaultPort;
                else
                    dataModel.TargetPort = AppConstant.MySQL_DefaultPort;
            }

            dataModel.TargetServer = txtServer.Text;
            dataModel.TargetUsername = txtUsername.Text;
            dataModel.TargetPassword = txtPassword.Text;
            dataModel.TargetDatabase = (sender as ComboBox).SelectedValue as string;
            if (!string.IsNullOrEmpty(dataModel.TargetDatabase))
                BindTableComboBox(dataModel);
        }

        private void cboTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataModel.TargetTable = (sender as ComboBox).SelectedValue as string;
            if (!string.IsNullOrEmpty(dataModel.TargetTable))
                btnNext.IsEnabled = true;
            else
                btnNext.IsEnabled = false;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            new SourceWindow(dataModel).Show();
            this.Close();
        }

        private void BindDatabaseComboBox(DataModel dataModel, bool IsDisplayMessage = true)
        {
            var connection = AppCommon.GetConnectionStatus(dataModel, 2);
            if (connection.Item1)
            {
                var databases = new DataTable();

                if (dataModel.TargetDataSourceId.Equals(AppConstant.DataSource_MsSQL))
                    databases = MsSQLConnectionSupport.GetAllDatabase(connection.Item2);
                if (dataModel.TargetDataSourceId.Equals(AppConstant.DataSource_MySQL))
                    databases = MySQLConnectionSupport.GetAllDatabase(connection.Item2);

                cboDatabase.ItemsSource = databases.DefaultView;
                cboDatabase.DisplayMemberPath = databases.Columns["Name"].ToString();
                cboDatabase.SelectedValuePath = databases.Columns["Name"].ToString();

                if (IsDisplayMessage)
                    MessageBox.Show(this, "Connection Succesfull!");
            }
            else
            {
                if (IsDisplayMessage)
                    MessageBox.Show(this, "Connection Could not be established!");
            }
            cboDatabase.SelectedIndex = -1;
            cboTables.ItemsSource = null;
        }

        private void BindTableComboBox(DataModel dataModel, bool IsDisplayMessage = true)
        {
            var connection = AppCommon.GetConnectionStatus(dataModel, 2);

            if (connection.Item1)
            {
                var databases = new DataTable();

                if (dataModel.TargetDataSourceId.Equals(AppConstant.DataSource_MsSQL))
                    databases = MsSQLConnectionSupport.GetAllTables(connection.Item2);
                else if (dataModel.TargetDataSourceId.Equals(AppConstant.DataSource_MySQL))
                    databases = MySQLConnectionSupport.GetAllTables(connection.Item2);

                cboTables.ItemsSource = databases.DefaultView;
                cboTables.DisplayMemberPath = databases.Columns["Name"].ToString();
                cboTables.SelectedValuePath = databases.Columns["Name"].ToString();
            }
            else
            {
                if (IsDisplayMessage)
                {
                    cboDatabase.SelectedIndex = -1;
                    MessageBox.Show(this, "Database is not accessible throught given credentials!");
                }
            }
        }
    }
}
