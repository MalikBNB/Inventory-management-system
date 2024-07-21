using DevExpress.Xpf.Controls;
using DevExpress.Xpf.Core;
using Hisba.Data.Bll.Entities;
using Hisba.Data.Layers;
using Hisba.Data.Layers.Entities;
using Hisba.Data.Layers.EntitiesInfo;
using Hisba.Shell.GlobalClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        private ObservableCollection<CategoryInfo> _Cotegories;
        public ObservableCollection<CategoryInfo> Cotegories
        {
            get { return _Cotegories; }
            set
            {
                _Cotegories = value;
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
                OnPropertyChanged();
            }
        }


        private ObservableCollection<decimal> _tVAs = new ObservableCollection<decimal> { 0, 9, 19 };
        public ObservableCollection<decimal> TVAs
        {
            get => _tVAs;
            set
            {
                _tVAs = value;
                OnPropertyChanged();
            }
        }

        private decimal _Selected;
        public decimal SelectedTVA
        {
            get => _Selected;
            set
            {
                _Selected = value;
                OnPropertyChanged();
            }
        }

        




        public ProductManageWindow()
        {
            InitializeComponent();

            DataContext = this;
        }




        private async void _LoadCategories()
        {
            var _Cotegories = await ProductCategoryBll.GetAllProductCategories();

            Cotegories = new ObservableCollection<CategoryInfo>(_Cotegories);
        }

        private void _ResetWindow()
        {
            _LoadCategories();

            CodeTextEdit.Text = string.Empty;
            ReferenceTextEdit.Text = string.Empty;            
            LabelTextEdit.Text = string.Empty;

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






        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ResetWindow();
        }

        private void SaveBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if(_SaveProduct() == true)
                DXMessageBox.Show("Product saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                DXMessageBox.Show("Product saved successfully", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveAndCloseBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void SaveAndNewBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void EmptyFieldsBarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            _ResetWindow();
        }



        private void LookupCategory_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            //_Category = new ProductCategory();
            //_Category = await ProductCategoryBll.GetProductCategoryByName(SelectedCategory.Name);

            DXMessageBox.Show(SelectedCategory.Name);
        }

        private void PurchasePriceHTSpinEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if(PurchasePriceHTSpinEdit.Value != 0)
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        
    }
}
