using System;
using System.Collections.Generic;
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
    /// Interaction logic for DBMapping.xaml
    /// </summary>
    public partial class DBMapping : Window
    {
        public DBMapping()
        {
            InitializeComponent();
            grdMapping.ItemsSource = GetSourceDataMappings();
            grdMapping.AutoGenerateColumns = false;
            grdMapping.CanUserAddRows = false;
            grdMapping.CanUserDeleteRows = false;
            grdMapping.CanUserReorderColumns = false;
            grdMapping.CanUserResizeColumns = false;
            grdMapping.CanUserSortColumns = false;

            cboBox1.ItemsSource = AppCommon.GetDataSourceItems();
            cboBox1.SelectedValuePath = "Tag";
            var binding = new Binding("Tag");
            binding.Mode = BindingMode.Default;
            cboBox1.SelectedValueBinding = binding;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var sourceColumns = grdMapping.Columns[2];
            foreach (var dr in grdMapping.ItemsSource)
            {
                //MessageBox.Show(dr.Header.ToString());
            }
            e.Handled = true;
        }
        List<DataMapping> GetSourceDataMappings()
        {
            var list = new List<DataMapping>();
            list.Add(new DataMapping
            {
                SourceColumn = "",
                TargetColumn = "FirstName",
            });
            list.Add(new DataMapping
            {
                SourceColumn = "",
                TargetColumn = "LastName"
            });
            list.Add(new DataMapping
            {
                SourceColumn = "",
                TargetColumn = "Email"
            });
            return list;
        }

        private void cboSourceColumn_SelectionChanged(object sender, DataGridCellEditEndingEventArgs e)
        {
            Console.WriteLine(e);
        }

        private void cboSourceColumn_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBoxItems = AppCommon.GetDataSourceItems();


        }

        private void SomeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedItem = this.grdMapping.CurrentItem;

        }
    }
}
