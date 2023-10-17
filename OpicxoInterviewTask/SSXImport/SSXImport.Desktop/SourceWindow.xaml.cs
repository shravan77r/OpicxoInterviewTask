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
    /// Interaction logic for SourceWindow.xaml
    /// </summary>
    public partial class SourceWindow : Window
    {
        DataModel dataModel = new DataModel();

        public SourceWindow()
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

        public SourceWindow(DataModel dataModel)
        {
            InitializeComponent();

            var comboBoxItems = AppCommon.GetDataSourceItems();

            cboDataSource.ItemsSource = comboBoxItems;
            cboDataSource.SelectedValuePath = "Tag";

            foreach (var item in cboDataSource.Items)
            {
                var comboItem = (ComboBoxItem)item;
                if (comboItem.Tag.Equals(dataModel.SourceDataSourceId))
                {
                    cboDataSource.SelectedItem = comboItem;
                    break;
                }
            }

            this.dataModel = dataModel;

            txtServer.Text = dataModel.SourceServer;
            txtUsername.Text = dataModel.SourceUsername;
            txtPassword.Text = dataModel.SourcePassword;
            txtPort.Text = dataModel.SourcePort;

            BindDatabaseComboBox(dataModel, false);

            foreach (var item in cboDatabase.Items)
            {
                var comboItem = (DataRowView)item;
                if (comboItem.Row["Name"].Equals(dataModel.SourceDatabase.ToString()))
                {
                    cboDatabase.SelectedItem = comboItem;
                    break;
                }
            }
            foreach (var item in cboTables.Items)
            {
                var comboItem = (DataRowView)item;
                if (comboItem.Row["Name"].Equals(dataModel.SourceTable.ToString()))
                {
                    cboTables.SelectedItem = comboItem;
                    break;
                }
            }

        }

        private void cboDataSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataSourceId = (sender as ComboBox).SelectedValue as int?;
            if (dataSourceId.GetValueOrDefault(0) > 0)
            {
                dataModel.SourceDataSourceId = Convert.ToInt32(dataSourceId);
                if (dataModel.SourceDataSourceId.Equals(AppConstant.DataSource_Excel))
                {
                    grd_Excel.Visibility = Visibility.Visible;
                    grd_Database.Visibility = Visibility.Hidden;
                }
                else if (dataModel.SourceDataSourceId.Equals(AppConstant.DataSource_MsSQL))
                {
                    grd_Excel.Visibility = Visibility.Hidden;
                    grd_Database.Visibility = Visibility.Visible;
                    txtPort.Text = AppConstant.MsSQL_DefaultPort;
                }
                else if (dataModel.SourceDataSourceId.Equals(AppConstant.DataSource_MySQL))
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
            dataModel.SourceServer = txtServer.Text;
            if (!string.IsNullOrEmpty(txtPort.Text))
                dataModel.SourcePort = txtPort.Text;
            else
            {
                if (cboDataSource.SelectedValue.Equals(AppConstant.DataSource_MsSQL))
                    dataModel.SourcePort = AppConstant.MsSQL_DefaultPort;
                else
                    dataModel.SourcePort = AppConstant.MySQL_DefaultPort;
            }
            dataModel.SourceUsername = txtUsername.Text;
            dataModel.SourcePassword = txtPassword.Text;
            if (string.IsNullOrEmpty(dataModel.SourceServer))
            {
                MessageBox.Show(this, "Please Enter Server");
                txtServer.Focus();
            }
            else if (string.IsNullOrEmpty(dataModel.SourceUsername))
            {
                MessageBox.Show(this, "Please Enter Username");
                txtUsername.Focus();
            }
            else if (string.IsNullOrEmpty(dataModel.SourcePassword))
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
                dataModel.SourcePort = txtPort.Text;
            else
            {
                if (cboDataSource.SelectedValue.Equals(AppConstant.DataSource_MsSQL))
                    dataModel.SourcePort = AppConstant.MsSQL_DefaultPort;
                else
                    dataModel.SourcePort = AppConstant.MySQL_DefaultPort;
            }
            dataModel.SourceServer = txtServer.Text;
            dataModel.SourceUsername = txtUsername.Text;
            dataModel.SourcePassword = txtPassword.Text;
            dataModel.SourceDatabase = (sender as ComboBox).SelectedValue as string;
            if (!string.IsNullOrEmpty(dataModel.SourceDatabase))
                BindTableComboBox(dataModel);
        }

        private void cboTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataModel.SourceTable = (sender as ComboBox).SelectedValue as string;
            if (!string.IsNullOrEmpty(dataModel.SourceTable))
                btnNext.IsEnabled = true;
            else
                btnNext.IsEnabled = false;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ClearSourceForm();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            dataModel.TargetDataSourceId = dataModel.TargetDataSourceId > 0
                ? dataModel.TargetDataSourceId
                : AppConstant.DataSource_MsSQL;
            new TargetWindow(dataModel).Show();
            this.Close();
        }

        private void BindDatabaseComboBox(DataModel dataModel, bool displayMessage = true)
        {
            cboDatabase.SelectedIndex = -1;
            cboTables.ItemsSource = null;
            var connection = AppCommon.GetConnectionStatus(dataModel, 1);
            if (connection.Item1)
            {
                var databases = new DataTable();

                if (dataModel.SourceDataSourceId.Equals(AppConstant.DataSource_MsSQL))
                    databases = MsSQLConnectionSupport.GetAllDatabase(connection.Item2);
                if (dataModel.SourceDataSourceId.Equals(AppConstant.DataSource_MySQL))
                    databases = MySQLConnectionSupport.GetAllDatabase(connection.Item2);

                cboDatabase.ItemsSource = databases.DefaultView;
                cboDatabase.DisplayMemberPath = databases.Columns["Name"].ToString();
                cboDatabase.SelectedValuePath = databases.Columns["Name"].ToString();

                if (displayMessage)
                    MessageBox.Show(this, "Connection Succesfull!");
            }
            else
            {
                if (displayMessage)
                    MessageBox.Show(this, "Connection Could not be established!");
            }
        }

        private void BindTableComboBox(DataModel dataModel, bool displayMessage = true)
        {
            var connection = AppCommon.GetConnectionStatus(dataModel, 1);

            if (connection.Item1)
            {
                var databases = new DataTable();

                if (dataModel.SourceDataSourceId.Equals(AppConstant.DataSource_MsSQL))
                    databases = MsSQLConnectionSupport.GetAllTables(connection.Item2);
                else if (dataModel.SourceDataSourceId.Equals(AppConstant.DataSource_MySQL))
                    databases = MySQLConnectionSupport.GetAllTables(connection.Item2);

                cboTables.ItemsSource = databases.DefaultView;
                cboTables.DisplayMemberPath = databases.Columns["Name"].ToString();
                cboTables.SelectedValuePath = databases.Columns["Name"].ToString();
            }
            else
            {
                cboDatabase.SelectedIndex = -1;
                if (displayMessage)
                    MessageBox.Show(this, "Database is not accessible throught given credentials!");
            }
        }

        private void ClearSourceForm()
        {
            foreach (var item in cboDataSource.Items)
            {
                var comboItem = (ComboBoxItem)item;
                if (comboItem.Tag.Equals(AppConstant.DataSource_MsSQL))
                {
                    cboDataSource.SelectedItem = comboItem;
                    break;
                }
            }

            txtServer.Text = string.Empty;
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtPort.Text = AppConstant.MsSQL_DefaultPort;
            cboDatabase.ItemsSource = null;
            cboTables.ItemsSource = null;
            btnNext.IsEnabled = false;
        }
    }
}
