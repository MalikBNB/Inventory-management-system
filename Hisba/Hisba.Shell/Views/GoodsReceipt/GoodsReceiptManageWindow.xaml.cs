using DevExpress.Data.Browsing.Design;
using DevExpress.Pdf.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Helpers;
using Hisba.Data.Bll;
using Hisba.Data.Bll.Entities;
using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using Hisba.Data.Layers.EntitiesInfo;
using Hisba.Shell.GlobalClasses;
using Hisba.Shell.Views.Products;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hisba.Shell.Views.GoodsReceipt
{
    /// <summary>
    /// Interaction logic for GoodsReceiptManageWindow.xaml
    /// </summary>
    public partial class GoodsReceiptManageWindow : INotifyPropertyChanged
    {

        #region Variables
        public enum Mode
        {
            AddNew = 1, Update = 2,
        }

        private Mode _mode;

        AppDbContext _context = new AppDbContext();


        private ObservableCollection<Product> _ProductsList;
        public ObservableCollection<Product> ProductsList
        {
            get => _ProductsList;
            set
            {
                if (_ProductsList != value)
                {
                    _ProductsList = value;
                    OnPropertyChanged("ProductsList");
                }
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                //if (_selectedProduct != value)
                //{
                    _selectedProduct = value;
                    OnPropertyChanged("SelectedProduct");
                //}
            }
        }

        private OrderItemInfos _orderItemInfos;
        public OrderItemInfos OrderItemInfos
        {
            get => _orderItemInfos;
            set
            {
                if (_orderItemInfos != value)
                {
                    _orderItemInfos = value;
                    OnPropertyChanged("OrderItem");
                }
            }
        }

        private ObservableCollection<decimal> _TVAs = new ObservableCollection<decimal> { 0, 9, 19};
        public ObservableCollection<decimal> TVAs
        {
            get => _TVAs;
            set
            {
                if (value != _TVAs)
                {
                    _TVAs = value;
                    OnPropertyChanged("TVAs");
                }
            }
        }

        private decimal _SelectedTVA;
        public decimal SelectedTVA
        {
            get => _SelectedTVA;
            set
            {
                _SelectedTVA = value;
                OnPropertyChanged("SelectedTVA");
            }
        }

        private OrderItem _OrderItem;

        private int _itemCode;

        private TierInfo _provider;

        private string _providerRef = "";

        private bool _MenuIsClosed;

        private Order _order;

        private OrderType _orderType;


        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string Property)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }

  

        #region Constructor
        public GoodsReceiptManageWindow(int Code, string providerRef)
        {
            InitializeComponent();

            DataContext = this;

            if(Code > 0)
            {
                _itemCode = Code;
                _mode = Mode.Update;
            }
            else
            {
                _mode = Mode.AddNew;
                _OrderItem = new OrderItem();
            }

            if (!string.IsNullOrEmpty(providerRef))
                _providerRef = providerRef;

        }
        #endregion


        #region Events
        public delegate void GetOrderBackEventHandler(Order order);
        public event GetOrderBackEventHandler GetOrderBack;
        #endregion

        #region My Functions

        // Fill the Product LookUpEdit
        private async void _LoadProducts()
        {
            var productsList = await ProductBll.GetProductsList();
            ProductsList = new ObservableCollection<Product>(productsList);
        }

        // Load Product data
        private void _LoadProductData()
        {
            if (SelectedProduct != null)
            {
                _OrderItem.Product = SelectedProduct;

                LabelTextEdit.Text = SelectedProduct.Name;
                DescriptionTextEdit.Text = SelectedProduct.Description;
                TVACbx.SelectedItem = SelectedProduct.TVAPercentage;
                UPurchasePriceHTSpinEdit.Value = SelectedProduct.PurchasePrice / (decimal)(1 + SelectedProduct.TVA);
                UPurchasePriceTTCSpinEdit.Value = SelectedProduct.PurchasePrice;
                QtySpinEdit.Value = 0;
                QtyInStokSpinEdit.Value = SelectedProduct.QuantityInStock;
                DiscountSpinEdit.Value = 0.00M;
                AfterDiscountSpinEdit.Value = 0.00M;
            }
            else
                DXMessageBox.Show($"Cannot find product with reference {SelectedProduct.Reference}.", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            
        }

        private void _ResetWindow()
        {
            _LoadProducts();

            _MenuIsClosed = true;

            if (_mode == Mode.AddNew)
            {
                this.Title = Properties.Resources.Title_Add;

                ProductLookUpEdit.Focus();
                ProductLookUpEdit.Text = string.Empty;                
                LabelTextEdit.Text = string.Empty;
                DescriptionTextEdit.Text = string.Empty;

                UPurchasePriceHTSpinEdit.NullText = "0.00";                
                UPurchasePriceTTCSpinEdit.NullText = "0.00";
                TVACbx.NullText = "0.00 %";                
                QtySpinEdit.NullText = "0.00";
                DiscountSpinEdit.NullText = "0.00";
                AfterDiscountSpinEdit.NullText = "0.00";
                QtyInStokSpinEdit.NullText = "0.00";

                return;
            }

            this.Title = Properties.Resources.Title_Edit;
        }

        // Load Order Item data to edit
        private async void _LoadItemInfo()
        {
            try
            {
                _orderItemInfos = await OrderItemBll.GetOrderItemByCode(_itemCode);

                if(_orderItemInfos != null)
                {
                    ProductLookUpEdit.Text = _orderItemInfos.ProductReference;
                    LabelTextEdit.Text = _orderItemInfos.ProductName;
                    DescriptionTextEdit.Text = _orderItemInfos.Description;
                    UPurchasePriceHTSpinEdit.Value = _orderItemInfos.Price;
                    UPurchasePriceTTCSpinEdit.Value = _orderItemInfos.PriceTTC;
                    SelectedTVA = _orderItemInfos.TVAPercentage;
                    QtySpinEdit.Value = _orderItemInfos.Quantity;
                    DiscountSpinEdit.Value = (decimal)(_orderItemInfos?.Discount); 
                    AfterDiscountSpinEdit.Value = _orderItemInfos.NetAmountTTC;
                }
                else
                {
                    DXMessageBox.Show($"Cannot find order item with code {_itemCode}. Try again", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private async Task<int> _CreateOrder()
        {
            _order = new Order();

            try
            {

                _provider = await TierBll.GetProvider(_providerRef);
                _order.TierId = _provider.Id;

                _order.TotalHT += _OrderItem.NetAmountHT;
                _order.TotalTVA += _OrderItem.TVAPercentage;
                _order.TotalTTC += _OrderItem.NetAmountTTC;

                _order.ModifierId = 1; //LogedInUserInfo.CurrentUser.Id;
                _order.Modified = DateTime.Now;

                if (_mode == Mode.AddNew)
                {
                    _orderType = await OrderTypeBll.GetOrderType(Data.Layers.Entities.OrderTypeCode.GoodsReceipt);

                    _order.Reference = await OrderBll.GenerateNewReference();
                    _order.OrderTypeId = _orderType.Id;
                    _order.Date = DateTime.Now;
                    _order.Status = false;

                    _order.CreatorId = 1; //LogedInUserInfo.CurrentUser.Id;
                    _order.Created = DateTime.Now;

                    var orderId = await OrderBll.Add(_order);

                    if (_order != null)
                        GetOrderBack?.Invoke(_order);

                    return orderId;

                }
                else
                {
                    await OrderBll.Update(_order);

                    return _order.Id;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, Properties.Resources.Exception, MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
        }

        private async void _CreateOrderItem()
        {
            try
            {
                
                //_OrderItem.ProductId = SelectedProduct.Id;
                //_OrderItem.Quantity = QtySpinEdit.Value;
                //_OrderItem.TVAPercentage = SelectedTVA;
                //_OrderItem.DiscountPercentage = DiscountSpinEdit.Value;

                _OrderItem.Created = DateTime.Now;
                _OrderItem.Modified = DateTime.Now;   
                
                if(_mode == Mode.AddNew)
                {
                    this.Title = Properties.Resources.Title_Add;

                    _OrderItem.Code = await OrderItemBll.GenerateNewCode();
                    _OrderItem.CreatorId = 1; //LogedInUserInfo.CurrentUser.Id;
                    _OrderItem.ModifierId = 1; //LogedInUserInfo.CurrentUser.Id;
                }
                else
                    this.Title = Properties.Resources.Title_Edit;


                var orderId = await _CreateOrder();

                if (orderId > 0)
                {
                    _OrderItem.OrderId = orderId;

                    if (_mode == Mode.AddNew)
                    {
                        var isAdded = await OrderItemBll.Add(_OrderItem);
                        if (isAdded == true)
                        {
                            //_order.Status = true;

                            ProductBll.Update(_OrderItem.Product);
                            //OrderBll.Update(_order);
                        }
                        else
                            DXMessageBox.Show("Goods receipt was not added", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (await OrderItemBll.Update(_OrderItem) == true)
                        {
                            //_order.Status = true;

                            ProductBll.Update(_OrderItem.Product);
                            //OrderBll.Update(_order);
                        }
                        else
                            DXMessageBox.Show("Goods receipt was not updated", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                DXMessageBox.Show("Cannot create new goods receipt", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, Properties.Resources.Exception, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion


        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _ResetWindow();

            if (_mode == Mode.Update)
                _LoadItemInfo();
        }

        private void AddBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            _CreateOrderItem();
        }

        private void CancelBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private async void ProductLookUpEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            _LoadProductData();

            if (ProductLookUpEdit != null)
            {
                _OrderItem.Product = await ProductBll.GetProductById(SelectedProduct.Id);
                QtySpinEdit.Value = 0;
            }
        }

        private void NewProductButton_Click(object sender, RoutedEventArgs e)
        {
            if(_MenuIsClosed == true)
            {
                NewProductPopupMenu.ShowPopup(NewProductButton);
                _MenuIsClosed = false;

                return;
            }

            NewProductPopupMenu.ClosePopup();
            _MenuIsClosed = true;
        }

        //private void UPriceHTSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        //{
        //    if (SelectedProduct != null)
        //    {
        //        _OrderItem.Product.MarginPercentage = ((UPriceHTSpinEdit.Value / _OrderItem.Product.PurchasePrice) - 1) * 100;
        //        //_OrderItem.Product = SelectedProduct; 
        //        UpriceTTCSpinEdit.Value = _OrderItem.Product.SalePriceTTC;
        //        AfterDiscountSpinEdit.Value = _OrderItem.NetAmountTTC;
        //    }
        //}

        private void UPurchasePriceHTSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                //_OrderItem.Product.PurchasePrice = UPurchsePriceHTSpinEdit.Value;
                //_OrderItem.Product = SelectedProduct; 
                UPurchasePriceTTCSpinEdit.Value = UPurchasePriceHTSpinEdit.Value * (1 + SelectedTVA / 100);
                AfterDiscountSpinEdit.Value = _OrderItem.NetAmountTTC;
            }
        }

        private void UPurchasepriceTTCSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                UPurchasePriceHTSpinEdit.Value = UPurchasePriceTTCSpinEdit.Value / (1 + SelectedTVA / 100);
                SelectedProduct.PurchasePrice = UPurchasePriceTTCSpinEdit.Value;
                _OrderItem.Product = SelectedProduct;
                AfterDiscountSpinEdit.Value = _OrderItem.NetAmountTTC;
            }

        }

        private void TVACbx_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                SelectedProduct.TVAPercentage = SelectedTVA;
                _OrderItem.Product = SelectedProduct;
                UPurchasePriceTTCSpinEdit.Value = UPurchasePriceHTSpinEdit.Value * (1 + SelectedTVA / 100);
                AfterDiscountSpinEdit.Value = _OrderItem.NetAmountTTC;
            }
        }

        private void DiscountSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                _OrderItem.DiscountPercentage = DiscountSpinEdit.Value;
                AfterDiscountSpinEdit.Value = _OrderItem.NetAmountTTC;
            }
        }

        private void QtySpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                if (e.OldValue != null)
                    _OrderItem.Product.QuantityInStock -= (decimal)e.OldValue;

                if (e.NewValue != null)
                    _OrderItem.Product.QuantityInStock += (decimal)e.NewValue;


                _OrderItem.Quantity = QtySpinEdit.Value;
                QtyInStokSpinEdit.Value = _OrderItem.Product.QuantityInStock;
                AfterDiscountSpinEdit.Value = _OrderItem.NetAmountTTC;
            }
        }

        private void AfterDiscountSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (SelectedProduct != null && (decimal)e.OldValue != 0)
            {
                if (QtySpinEdit.Value == 0) return;

                DiscountSpinEdit.Value = (1 - (AfterDiscountSpinEdit.Value / (_OrderItem.PriceTTC * _OrderItem.Quantity))) * 100;
                _OrderItem.DiscountPercentage = DiscountSpinEdit.Value;
            }
        }

        private void AddProduct_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                var AddProduct = new ProductManageWindow() { Owner = Window.GetWindow(this) };
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
            try
            {
                var EditProduct = new ProductManageWindow(SelectedProduct.Reference) { Owner = Window.GetWindow(this) };
                EditProduct.ShowDialog();

                _LoadProducts();
            }
            catch (Exception ex)
            {

            }
        }

        private async void DeleteProduct_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {

                var answer = DXMessageBox.Show($"Are you sure you want to delete Product: \n[{SelectedProduct.Reference}] [{SelectedProduct.Name}] ?",
                                                "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes ? true : false;
                if (answer == true)
                {
                    var isDeleted = await ProductBll.Delete(SelectedProduct.Reference);

                    if (isDeleted == true)
                    {
                        _LoadProducts();
                    }
                    else
                        DXMessageBox.Show("Product was not deleted successfully", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, Properties.Resources.Exception, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void QtyInStokSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

        }

    }
}
