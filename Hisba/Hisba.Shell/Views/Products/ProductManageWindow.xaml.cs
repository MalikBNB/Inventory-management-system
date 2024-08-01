using DevExpress.Office.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using Hisba.Data.Bll;
using Hisba.Data.Bll.Entities;
using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using Hisba.Data.Layers.EntitiesInfo;
using Hisba.Shell.GlobalClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
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

        private Product _Product = new Product();

        private Product _oldProduct;

        private string _reference;

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


        public ProductManageWindow(string Reference = "")
        {
            InitializeComponent();

            DataContext = this;

            if (!string.IsNullOrEmpty(Reference))
            {
                _mode = Mode.Update;
                _reference = Reference;
            }
            else
            {
                _mode = Mode.AddNew;
            }

        }



        #region My Functions
        private async void _LoadCategories()
        {
            var _Categories = await ProductCategoryBll.GetAllProductCategories();

            Categories = new ObservableCollection<CategoryInfo>(_Categories);
        }

        private void _ResetWindow()
        {
            try
            {
                _LoadCategories();

                if (_mode == Mode.AddNew)
                {
                    this.Title = Properties.Resources.Title_AddProduct;

                    CodeTextEdit.Text = string.Empty;
                    ReferenceTextEdit.Text = string.Empty;
                    LabelTextEdit.Text = string.Empty;
                    LabelTextEdit.Focus();
                    PurchasePriceHTSpinEdit.NullText = "0";
                    SalePriceHTSpinEdit.NullText = "0";

                    TVAcbxEdit.SelectedItem = 0.0M;
                    MarginSpinEdit.NullText = "0";
                    SalePriceTTCSpinEdit.NullText = "0";
                    QtyInStockSpinEdit.NullText = "0";

                    DescriptionTextEdit.Text = string.Empty;

                    return;
                }

                this.Title = Properties.Resources.Title_EditProduct;
                

                //_Product = new Product();
                //_Product.Code = _oldProduct.Code;
                //_Product.Reference = _oldProduct.ProductReference;
                //_Product.CategoryId = _oldProduct.CategoryId;
                //_Product.Name = _oldProduct.ProductName;
                //_Product.PurchasePrice = _oldProduct.PurchasePrice;
                //_Product.MarginPercentage = _oldProduct.MarginPercentage;
                //_Product.TVAPercentage = _oldProduct.TVAPercentage;
                //_Product.Description = _oldProduct.Description;
                //_Product.QuantityInStock = _oldProduct.QuantityInStock;
                //_Product.CreatorId = _oldProduct.CreatorId;
                //_Product.Created = _oldProduct.Created;
                //_Product.ModifierId = _oldProduct.ModifierId;
                //_Product.Modified = _oldProduct.Modified;
                //LookupCategory.Text = _oldProduct.Category;


            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, Properties.Resources.Exception, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private async void _LoadProductInfo()
        {
            try
            {
                _oldProduct = await ProductBll.GetProductByReference(_reference);

                if (_oldProduct != null)
                {
                    CodeTextEdit.Text = _oldProduct.Code.ToString();
                    ReferenceTextEdit.Text = _oldProduct.Reference;
                    LabelTextEdit.Text = _oldProduct.Name;
                    PurchasePriceHTSpinEdit.Text = _oldProduct.PurchasePrice.ToString();
                    SalePriceHTSpinEdit.Text = _oldProduct.SalePrice.ToString();
                    LookupCategory.Text = _oldProduct.Category.Name;
                    SelectedTVA = _oldProduct.TVAPercentage;
                    MarginSpinEdit.Text = _oldProduct.MarginPercentage.ToString();
                    SalePriceTTCSpinEdit.Text = _oldProduct.SalePriceTTC.ToString();
                    QtyInStockSpinEdit.Text = _oldProduct.QuantityInStock.ToString();
                    DescriptionTextEdit.Text = _oldProduct.Description;

                    LabelTextEdit.Focus();

                    _oldCode = _oldProduct.Code;
                    var category = await ProductCategoryBll.GetProductCategoryById(_oldProduct.Category.Id);
                    SelectedCategory = category;
                }
                else
                {
                    DXMessageBox.Show($"Cannot find product with reference {_reference}. Try again", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
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

                    _Product.Reference = ReferenceTextEdit.Text.Trim();
                    _Product.Name = LabelTextEdit.Text.Trim();
                    _Product.Description = DescriptionTextEdit.Text.Trim();
                    _Product.CategoryId = SelectedCategory.Id;
                    _Product.PurchasePrice = PurchasePriceHTSpinEdit.Value;
                    _Product.MarginPercentage = MarginSpinEdit.Value;
                    _Product.TVAPercentage = SelectedTVA;
                    _Product.QuantityInStock = QtyInStockSpinEdit.Value;
                    _Product.ModifierId = 1; //LogedInUserInfo.CurrentUser.Id;
                    _Product.Modified = DateTime.Now;

                    if (_mode == Mode.AddNew)
                    {
                        _Product.Code = string.IsNullOrEmpty(CodeTextEdit.Text.Trim()) ?
                                        await ProductBll.GenerateNewCode(SelectedCategory) : int.Parse(SelectedCategory.Code.ToString() + CodeTextEdit.Text.Trim());
                        _Product.CreatorId = 1; // LogedInUserInfo.CurrentUser.Id;
                        _Product.Created = DateTime.Now;

                        ProductBll.Add(_Product);
                    }
                    else
                    {
                        if (_newCode == _oldCode)
                            _Product.Code = _oldCode;
                        else
                            _Product.Code = string.IsNullOrEmpty(CodeTextEdit.Text.Trim()) ?
                                            await ProductBll.GenerateNewCode(SelectedCategory) : int.Parse(SelectedCategory.Code.ToString() + _newCode.ToString());

                        _Product.CreatorId = _oldProduct.CreatorId;
                        _Product.Created = _oldProduct.Created;

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
                var isCreated = await _CreateProduct();
                return isCreated;
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

            if(_mode == Mode.Update)
                _LoadProductInfo();
        }

        private async void SaveBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var isSaved = await _SaveProduct();

            if (isSaved == true)
                DXMessageBox.Show("Product saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                DXMessageBox.Show("Product was not saved", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void SaveAndCloseBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var isSaved = await _SaveProduct();

            if (isSaved == true)
            {
                DXMessageBox.Show("Product saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
                DXMessageBox.Show("Product was not saved", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void SaveAndNewBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var isSaved = await _SaveProduct();

            if (isSaved == true)
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
            //CodeTextEdit.Clear();

            if (LookupCategory != null)
            {
                SelectedCategory = (CategoryInfo)LookupCategory.SelectedItem;
            }
        }

        private void PurchasePriceHTSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (PurchasePriceHTSpinEdit.Value != 0)
                MarginSpinEdit.Value = ((SalePriceHTSpinEdit.Value / PurchasePriceHTSpinEdit.Value) - 1) * 100;
        }

        private void SalePriceHTSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (PurchasePriceHTSpinEdit.Value != 0)
                MarginSpinEdit.Value = ((SalePriceHTSpinEdit.Value / PurchasePriceHTSpinEdit.Value) - 1) * 100;

            SalePriceTTCSpinEdit.Value = SalePriceHTSpinEdit.Value * (1 + SelectedTVA / 100);
        }

        private void MarginSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (MarginSpinEdit.IsFocused)
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
