using DevExpress.Xpf.Core;
using Hisba.Data.Bll.Entities;
using Hisba.Data.Layers.Entities;
using Hisba.Data.Layers.EntitiesInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hisba.Shell.Views.Products
{
    /// <summary>
    /// Interaction logic for ProductUc.xaml
    /// </summary>
    public partial class ProductUc : INotifyPropertyChanged
    {
        private ObservableCollection<ProductInfos> _productInfos;

        public ObservableCollection<ProductInfos> ProductInfos
        {
            get => _productInfos;
            set
            {
                _productInfos = value;
                OnPropertyChanged();
            }
        }

        public ProductUc()
        {
            InitializeComponent();

            DataContext = this;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private async void _LoadProducts()
        {
            var productInfos = await ProductBll.GetAllProducts();
            ProductInfos = new ObservableCollection<ProductInfos>(productInfos);
        }




        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadProducts();
            OnPropertyChanged(nameof(ProductInfos));
        }

        private void AddProduct_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                var AddProduct = new ProductManageWindow { Owner = Window.GetWindow(this) };
                AddProduct.ShowDialog();

                _LoadProducts();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditProduct_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void DeleteProduct_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void RefreshProduct_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            _LoadProducts();
        }

        private void ManageGridControl_ItemsSourceChanged(object sender, DevExpress.Xpf.Grid.ItemsSourceChangedEventArgs e)
        {

        }

        private void TableView_RowDoubleClick(object sender, DevExpress.Xpf.Grid.RowDoubleClickEventArgs e)
        {

        }

        private void TableView_Loaded(object sender, RoutedEventArgs e)
        {

        }


    }
}
