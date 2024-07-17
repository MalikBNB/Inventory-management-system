using Hisba.Data.Bll.Entities;
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
                _items = value;
                OnPropertyChanged();
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


        private async void _LoadOrderItems()
        {
            var Items = await OrderItemBll.GetAllOrderItems();
            OrderItems = new ObservableCollection<OrderItemInfos>(Items);
        }








        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _LoadOrderItems();
            OnPropertyChanged(nameof(OrderItems));
        }

        private void AddReceipt_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void EditReceipt_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void DeleteReceipt_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

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
    }
}
