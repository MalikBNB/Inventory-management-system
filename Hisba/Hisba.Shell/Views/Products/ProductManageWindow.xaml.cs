using DevExpress.Office.Utils;
using DevExpress.Xpf.Core;
using Hisba.Data.Bll.Entities;
using Hisba.Data.Layers.Entities;
using Hisba.Data.Layers.EntitiesInfo;
using Hisba.Shell.GlobalClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace Hisba.Shell.Views.Products
{
    /// <summary>
    /// Interaction logic for ProductManageWindow.xaml
    /// </summary>
    public partial class ProductManageWindow : INotifyPropertyChanged
    {

        #region Variables

        public enum Mode
        {
            AddNew = 1,
            Update = 2
        }

        private Mode _mode;

        private Product _Product;
        public Product Product
        {
            get => _Product;
            set
            {
                if (_Product != value)
                {
                    _Product = value;
                    OnPropertyChanged("Product");
                }              
            }
        }

        private ProductInfos _oldProduct;

        private ObservableCollection<CategoryInfo> _Categories;
        public ObservableCollection<CategoryInfo> Categories
        {
            get { return _Categories; }
            set
            {
                _Categories = value;
                OnPropertyChanged("Categories");
            }
        }

        private CategoryInfo _SelectedCategory;
        public CategoryInfo SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                if (value != _SelectedCategory)
                {
                    _SelectedCategory = value;
                    OnPropertyChanged("SelectedCategory");
                }

            }
        }


        private ObservableCollection<decimal> _tVAs = new ObservableCollection<decimal> { 0, 9, 19 };
        public ObservableCollection<decimal> TVAs
        {
            get => _tVAs;
            set
            {
                _tVAs = value;
                OnPropertyChanged("TVAs");
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

        private int _oldCode;
        private int _newCode;

        #endregion


        public ProductManageWindow(ProductInfos productInfos)
        {
            InitializeComponent();

            DataContext = this;

            if (productInfos != null)
            {
                _mode = Mode.Update;
                _oldProduct = productInfos;
            }
            else
            {
                _mode = Mode.AddNew;
                _Product = new Product();
            }

        }



        #region My Functions
        private async void _LoadCategories()
        {
            var _Categories = await ProductCategoryBll.GetAllProductCategories();

            Categories = new ObservableCollection<CategoryInfo>(_Categories);
        }

        //private async void _ResetWindow()
        //{
        //    try
        //    {
        //        _LoadCategories();

        //        if (_mode == Mode.AddNew)
        //        {
        //            CodeTextEdit.Text = string.Empty;
        //            ReferenceTextEdit.Text = string.Empty;
        //            LabelTextEdit.Text = string.Empty;
        //            LabelTextEdit.Focus();
        //            PurchasePriceHTSpinEdit.NullText = "0";
        //            SalePriceHTSpinEdit.NullText = "0";

        //            TVAcbxEdit.SelectedItem = 0.0M;
        //            MarginSpinEdit.NullText = "0";
        //            SalePriceTTCSpinEdit.NullText = "0";
        //            QtyInStockSpinEdit.NullText = "0";

        //            DescriptionTextEdit.Text = string.Empty;

        //            return;
        //        }

        //        _Product = new Product();

        //        _oldCode = _oldProduct.Code;

        //        _Product.Code = _oldProduct.Code;
        //        _Product.Reference = _oldProduct.ProductReference;
        //        _Product.CategoryId = _oldProduct.CategoryId;
        //        _Product.Name = _oldProduct.ProductName;
        //        _Product.PurchasePrice = _oldProduct.PurchasePrice;
        //        _Product.MarginPercentage = _oldProduct.MarginPercentage;
        //        _Product.TVAPercentage = _oldProduct.TVAPercentage;
        //        _Product.Description = _oldProduct.Description;
        //        _Product.QuantityInStock = _oldProduct.QuantityInStock;
        //        _Product.CreatorId = _oldProduct.CreatorId;
        //        _Product.Created = _oldProduct.Created;
        //        _Product.ModifierId = _oldProduct.ModifierId;
        //        _Product.Modified = _oldProduct.Modified;

        //        LookupCategory.Text = _oldProduct.Category;

        //        var category = await ProductCategoryBll.GetProductCategoryById((int)_Product.CategoryId);
        //        SelectedCategory = category;
        //    }
        //    catch (Exception ex)
        //    {
        //        DXMessageBox.Show(ex.Message, Properties.Resources.Exception, MessageBoxButton.OK, MessageBoxImage.Error);
        //    }

        //}


        private async void _ResetWindow()
        {
            try
            {
                _LoadCategories();

                if (_mode == Mode.AddNew)
                {
                    LabelTextEdit.Focus();
                    return;
                }

                _Product = new Product();

                _oldCode = _oldProduct.Code;

                _Product.Code = _oldProduct.Code;
                _Product.Reference = _oldProduct.ProductReference;
                _Product.CategoryId = _oldProduct.CategoryId;
                _Product.Name = _oldProduct.ProductName;
                _Product.PurchasePrice = _oldProduct.PurchasePrice;
                _Product.MarginPercentage = _oldProduct.MarginPercentage;
                _Product.TVAPercentage = _oldProduct.TVAPercentage;
                _Product.Description = _oldProduct.Description;
                _Product.QuantityInStock = _oldProduct.QuantityInStock;
                _Product.CreatorId = _oldProduct.CreatorId;
                _Product.Created = _oldProduct.Created;
                _Product.ModifierId = _oldProduct.ModifierId;
                _Product.Modified = _oldProduct.Modified;

                LookupCategory.Text = _oldProduct.Category;

                var category = await ProductCategoryBll.GetProductCategoryById((int)_Product.CategoryId);
                SelectedCategory = category;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, Properties.Resources.Exception, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private bool _AllFieldsAreValid()
        {
            if (string.IsNullOrEmpty(LabelTextEdit.Text))
            {
                DXMessageBox.Show("Label cannot be empty.", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                LabelTextEdit.Focus();

                return false;
            }

            if (string.IsNullOrEmpty(ReferenceTextEdit.Text))
            {
                DXMessageBox.Show("Reference cannot be empty.", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                ReferenceTextEdit.Focus();

                return false;
            }

            if (string.IsNullOrEmpty(LookupCategory.Text))
            {
                DXMessageBox.Show("Category cannot be empty.", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                LookupCategory.Focus();

                return false;
            }

            //if (LogedInUserInfo.CurrentUser == null)
            //{
            //    DXMessageBox.Show("Cannot find user.", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);

            //    return false;
            //}

            return true;
        }

        private async Task<bool> _CreateProduct()
        {
            try
            {
                if (_AllFieldsAreValid())
                {

                    //_Product.Reference = ReferenceTextEdit.Text;
                    //_Product.Name = LabelTextEdit.Text;
                    //_Product.Description = DescriptionTextEdit.Text;
                    //_Product.CategoryId = SelectedCategory.Id;
                    //_Product.PurchasePrice = PurchasePriceHTSpinEdit.Value;
                    //_Product.MarginPercentage = MarginSpinEdit.Value;
                    //_Product.TVAPercentage = SelectedTVA;
                    //_Product.QuantityInStock = QtyInStockSpinEdit.Value;
                    _Product.ModifierId = 1; //LogedInUserInfo.CurrentUser.Id;
                    _Product.Modified = DateTime.Now;

                    if (_mode == Mode.AddNew)
                    {
                        //_Product.Code = string.IsNullOrEmpty(CodeTextEdit.Text) || string.IsNullOrWhiteSpace(CodeTextEdit.Text) ? 
                        //                await ProductBll.GenerateNewCode(SelectedCategory) : int.Parse(SelectedCategory.Code.ToString() + CodeTextEdit.Text);
                        _Product.CreatorId = 1; // LogedInUserInfo.CurrentUser.Id;
                        _Product.Created = DateTime.Now;

                        ProductBll.Add(_Product);
                    }
                    else
                    {
                        //if (_newCode == _oldCode)
                        //    _Product.Code = _oldCode;
                        //else
                        //    _Product.Code = string.IsNullOrEmpty(CodeTextEdit.Text) || string.IsNullOrWhiteSpace(CodeTextEdit.Text) ?
                        //        await ProductBll.GenerateNewCode(SelectedCategory) : int.Parse(SelectedCategory.Code.ToString() + _newCode.ToString());

                        ProductBll.Update(_Product);
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, Properties.Resources.Exception, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }

        private async Task<bool> _SaveProduct()
        {
            try
            {                            
                return  await _CreateProduct();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, Properties.Resources.Exception, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ResetWindow();
        }

        private async void SaveBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (await _SaveProduct() == true)
                DXMessageBox.Show("Product saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                DXMessageBox.Show("Product was not saved", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void SaveAndCloseBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (await _SaveProduct() == true)
            {
                DXMessageBox.Show("Product saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
                DXMessageBox.Show("Product was not saved", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void SaveAndNewBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (await _SaveProduct() == true)
            {
                DXMessageBox.Show("Product saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _ResetWindow();
            }
            else
                DXMessageBox.Show("Product was not saved", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void EmptyFieldsBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var Answer = DXMessageBox.Show("Are you sure you want to empty fields ?", "Confirm empty fields", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes ? true : false;

            if (Answer == true)
                _ResetWindow();
        }



        private void CodeTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (_mode == Mode.Update)
                _newCode = int.Parse(CodeTextEdit.Text);
        }

        private void LookupCategory_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            CodeTextEdit.Clear();

            if (LookupCategory != null)
            {
                SelectedCategory = (CategoryInfo)LookupCategory.SelectedItem;
            }
        }

        private void PurchasePriceHTSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (PurchasePriceHTSpinEdit.Value != 0)
                MarginSpinEdit.Text = (((SalePriceHTSpinEdit.Value / PurchasePriceHTSpinEdit.Value) - 1) * 100).ToString();
        }

        private void SalePriceHTSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (PurchasePriceHTSpinEdit.Value != 0)
            {
                MarginSpinEdit.Text = (((SalePriceHTSpinEdit.Value / PurchasePriceHTSpinEdit.Value) - 1) * 100).ToString();
            }

            SalePriceTTCSpinEdit.Value = SalePriceHTSpinEdit.Value * (1 + (SelectedTVA / 100));

        }

        private void MarginSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if(MarginSpinEdit.IsFocused)
                SalePriceHTSpinEdit.Value = PurchasePriceHTSpinEdit.Value * (1 + (MarginSpinEdit.Value / 100));

        }

        private void TVAcbxEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            SalePriceTTCSpinEdit.Value = SalePriceHTSpinEdit.Value * (1 + (SelectedTVA / 100));
        }

        private void SalePriceTTCSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            SalePriceHTSpinEdit.Value = SalePriceTTCSpinEdit.Value / (1 + (SelectedTVA / 100));
        }

        private void QtyInStockSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

        }

        private void bBold_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void bItalic_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void bUnderline_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void bLeft_CheckedChanged(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void bCenter_CheckedChanged(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void bRight_CheckedChanged(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void SalepriceHTSmplButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MarginSmplButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SalepriceTTCSmplButton_Click(object sender, RoutedEventArgs e)
        {

        }

        #region Validation
        private void CodeTextEdit_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (!string.IsNullOrEmpty((string)e.Value) && SelectedCategory != null)
            {
                try
                {
                    if (e.Value == null) return;

                    if (int.TryParse(SelectedCategory.Code + (string)e.Value, out int value))
                    {
                        //Task<bool> task = Task.Run(() => ProductBll.IsCodeExist(value));
                        //if (task.Result == true) return;

                        var task = ProductBll.IsCodeExist(value);
                        if (task == true) return;


                        e.IsValid = false;
                        e.ErrorContent = "Code already exist. Enter another one.";
                        CodeTextEdit.Focus();

                        return;
                    }

                    e.IsValid = false;
                    e.ErrorContent = "Code only accept numbers.";
                }
                catch (Exception ex)
                {
                    DXMessageBox.Show(ex.Message, Properties.Resources.Exception, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ReferenceTextEdit_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (!string.IsNullOrEmpty((string)e.Value))
            {
                try
                {
                    if (e.Value == null) return;

                    Task<bool> task = Task.Run(() => ProductBll.IsReferenceExist((string) e.Value));
                    if (task.Result == true) return;


                    e.IsValid = false;
                    e.ErrorContent = "Reference already exist. Enter another one.";
                }
                catch (Exception ex)
                {
                    DXMessageBox.Show(ex.Message, Properties.Resources.Exception, MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        #endregion
    }
}
