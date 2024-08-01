using DevExpress.Xpf.Core;
using DevExpress.Xpf.Layout.Core;
using DevExpress.XtraPrinting.Native;
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

namespace Hisba.Shell.Views.GoodsReceipt
{
    /// <summary>
    /// Interaction logic for GoodsReceiptUc.xaml
    /// </summary>
    public partial class GoodsReceiptUc : UserControl, INotifyPropertyChanged
    {


        private ObservableCollection<OrderItemInfos> _items;

        public ObservableCollection<OrderItemInfos> OrderItems 
        {
            get => _items;
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged();
                }
            }
        }

        private OrderItemInfos _item;

        public OrderItemInfos SelectedOrderItem
        {
            get => _item;
            set
            {
                if (value != _item)
                {
                    _item = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<TierInfo> _providers;

        public ObservableCollection<TierInfo> Providers
        {
            get =>_providers;
            set
            {
                if(_providers != value)
                {
                    _providers = value;
                    OnPropertyChanged("Providers");
                }
            }
        }

        private Order _Order;

        private TierInfo _provider;

        public TierInfo SelectedProvider
        {
            get => _provider;
            set
            {
                //if (_provider != value)
                //{
                //    _provider = value;
                //    OnPropertyChanged("Providers");
                //}

                _provider = value;
                OnPropertyChanged("Providers");
            }
        }


        public GoodsReceiptUc()
        {
            InitializeComponent();

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private async void _LoadProviders()
        {
            var providers = await TierBll.GetAllProviders();
            Providers = new ObservableCollection<TierInfo>(providers);
        }

        private async void _LoadOrderItems()
        {
            var Items = await OrderItemBll.GetAllOrderItems();
            OrderItems = new ObservableCollection<OrderItemInfos>(Items);
        }






        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadOrderItems();
            _LoadProviders();
            ProviderLookUpEdit.ShowPopup();
            DateDateEdit.DateTime = DateTime.Now;
        }

        private void OrderIsBack(Order order)
        {
            _Order = order;
            //_LoadOrderItems();
        }

        private void AddReceipt_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {           
            try
            {
                var addItem = new GoodsReceiptManageWindow(-1, "") { Owner = Window.GetWindow(this) };

                if (SelectedProvider != null)
                    addItem = new GoodsReceiptManageWindow(-1, SelectedProvider.Reference) { Owner = Window.GetWindow(this) };


                addItem.GetOrderBack += OrderIsBack;

                addItem.ShowDialog();

                _LoadOrderItems();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditReceipt_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                if(TableView.GetSelectedRows().Count > 0)
                {
                    var EditItem = new GoodsReceiptManageWindow(SelectedOrderItem.Code, "") { Owner = Window.GetWindow(this) };

                    if (SelectedProvider != null)
                        EditItem = new GoodsReceiptManageWindow(SelectedOrderItem.Code, SelectedProvider.Reference) { Owner = Window.GetWindow(this) };

                    EditItem.ShowDialog();

                    _LoadOrderItems();
                }
                else
                    DXMessageBox.Show("Grid is empty. Add new goods receipt first", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteReceipt_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                if (TableView.GetSelectedRows().Count > 0)
                {
                    var answer = DXMessageBox.Show($"Are you sure you want to delete goods receipt: \n[{SelectedOrderItem.TierName}] \n[{SelectedOrderItem.ProductName}] ?",
                                                "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes ? true : false;
                    if (answer == true)
                    {
                        var isDeleted = await OrderItemBll.Delete(SelectedOrderItem.Code);

                        if (isDeleted == true)
                        {
                            DXMessageBox.Show("Goods receipt was deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            _LoadOrderItems();
                        }
                        else
                            DXMessageBox.Show("Failed. Goods receipt was not deleted", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    _LoadOrderItems();
                }
                else
                    DXMessageBox.Show("Grid is empty. Add new Goods receipt first", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ValidateReceipt_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                if (_Order == null)
                {
                    DXMessageBox.Show("Goods receipt cannot be found!", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_Order.TierId <= 0)
                {
                    DXMessageBox.Show("Please select a provider", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    ProviderLookUpEdit.ShowPopup();
                    return;
                }

                _Order.Status = true;

                var isUpdated = await OrderBll.Update(_Order);
                if(isUpdated == true)
                    DXMessageBox.Show("Goods reciept validated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshReceipt_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

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

        private void ProviderLookUpEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if(ProviderLookUpEdit != null)
                _Order.TierId = SelectedProvider.Id;
        }

    }
}
