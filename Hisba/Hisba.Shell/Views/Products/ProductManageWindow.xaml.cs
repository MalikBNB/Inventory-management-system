using DevExpress.Office.Utils;
using DevExpress.Xpf.Core;
using Hisba.Data.Bll.Entities;
using Hisba.Data.Layers.Entities;
using Hisba.Data.Layers.EntitiesInfo;
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

        private Product _Product;
        public Product Product
        {
            get => _Product;
            set
            {
                _Product = value;
                OnPropertyChanged();
            }
        }

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

        //private ProductCategory _Category;

        private CategoryInfo _SelectedCategory;
        public CategoryInfo SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                _SelectedCategory = value;
                OnPropertyChanged("SelectedCategory");
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






        public ProductManageWindow()
        {
            InitializeComponent();

            DataContext = this;
        }




        private async void _LoadCategories()
        {
            var _Categories = await ProductCategoryBll.GetAllProductCategories();

            Categories = new ObservableCollection<CategoryInfo>(_Categories);
        }

        private void _ResetWindow()
        {
            _LoadCategories();

            CodeTextEdit.Text = string.Empty;
            ReferenceTextEdit.Text = string.Empty;
            LabelTextEdit.Text = string.Empty;
            LookupCategory.AllowDrop = true;
            PurchasePriceHTSpinEdit.NullText = "0";
            SalePriceHTSpinEdit.NullText = "0";

            TVAcbxEdit.SelectedItem = 0.0M;
            MarginSpinEdit.NullText = "0";
            SalePriceTTCSpinEdit.NullText = "0";
            QtyInStockSpinEdit.NullText = "0";

            DescriptionTextEdit.Text = string.Empty;
        }

        private void _CreateProduct()
        {
            _Product = new Product();

            _Product.Code = int.Parse(CodeTextEdit.Text);
            _Product.Reference = ReferenceTextEdit.Text;
            _Product.Name = LabelTextEdit.Text;
            _Product.Description = DescriptionTextEdit.Text;
            _Product.CategoryId = SelectedCategory.Id;
            _Product.PurchasePrice = PurchasePriceHTSpinEdit.Value;
            _Product.MarginPercentage = MarginSpinEdit.Value;
            _Product.TVAPercentage = SelectedTVA;
            _Product.QuantityInStock = QtyInStockSpinEdit.Value;
            _Product.CreatorId = 1; // LogedInUserInfo.CurrentUser.Id;
            _Product.Created = DateTime.Now;
            _Product.ModifierId = 1; // LogedInUserInfo.CurrentUser.Id;
            _Product.Modified = DateTime.Now;
        }

        private bool _SaveProduct()
        {

            try
            {
                _CreateProduct();
                ProductBll.Add(_Product);

                //using (var context = new AppDbContext())
                //{
                //}

                return true;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ResetWindow();
        }

        private void SaveBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (_SaveProduct() == true)
                DXMessageBox.Show("Product saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                DXMessageBox.Show("Product saved successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveAndCloseBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (_SaveProduct() == true)
            {
                DXMessageBox.Show("Product saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            else
                DXMessageBox.Show("Product saved successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveAndNewBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (_SaveProduct() == true)
            {
                DXMessageBox.Show("Product saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Error);
                _ResetWindow();
            }
            else
                DXMessageBox.Show("Product saved successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EmptyFieldsBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var Answer = DXMessageBox.Show("Are you sure you want to empty fields ?", "Confirm empty fields", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes ? true : false;

            if (Answer == true)
                _ResetWindow();

        }



        private void LookupCategory_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            //_Category = new ProductCategory();
            //_Category = await ProductCategoryBll.GetProductCategoryByName(SelectedCategory.Name);
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

        private void CodeTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            //if (LookupCategory.EditValue == null)
            //{
            //    DXMessageBox.Show("You have to choose category first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    CodeTextEdit.Clear();
            //    LookupCategory.AllowDrop = true;
            //}

            //if (CodeTextEdit != null)
            //    CodeTextEdit.DoValidate();

        }
        static bool Check(int value)
        {
            return ProductBll.IsCodeExist(value);
        }
        private async void CodeTextEdit_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (!string.IsNullOrEmpty((string)e.Value))
            {
                if (e.Value == null) return;

                List<int> values = new List<int>();
                values.Add(Convert.ToInt32(e.Value));
                //Task<bool> response = Task.Run(() => ProductBll.IsCodeExist(Convert.ToInt32(e.Value)));
                //var b = await response;
                //if (b == true) return;

                bool r = true;
                Parallel.ForEach(values, value =>
                {
                    r = Check(value);
                });

                if (r == true) return;

                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning;
                e.IsValid = false;
                e.ErrorContent = "Code already exist. Enter another one.";

            }
        }
    }
}
