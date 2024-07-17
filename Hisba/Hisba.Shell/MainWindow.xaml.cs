using DevExpress.Xpf.Core;
using Hisba.Shell.Views;
using Hisba.Shell.Views.GoodsReceipt;
using Hisba.Shell.Views.Product;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hisba.Shell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void GoodsReceiptAccordionItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            LoadingDecorator.IsSplashScreenShown = true;

            var GoodsReceipt = DockLayoutManager.DockController.AddDocumentPanel(DocumentGroup);
            GoodsReceipt.FloatOnDoubleClick = GoodsReceipt.AllowFloat = false;
            GoodsReceipt.Caption = "Goods receipt";
            GoodsReceipt.Content = new GoodsReceiptUc();

            DockLayoutManager.Activate(GoodsReceipt);

            LoadingDecorator.IsSplashScreenShown = false;

        }

        private void PurchaseReturnOrderAccordionItem_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void DeliveryNoteAccordionItem_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void SaleReturnOrder_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void ProductsAccordionItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            LoadingDecorator.IsSplashScreenShown = true;

            var GoodsReceipt = DockLayoutManager.DockController.AddDocumentPanel(DocumentGroup);
            GoodsReceipt.FloatOnDoubleClick = GoodsReceipt.AllowFloat = false;
            GoodsReceipt.Caption = "Products";
            GoodsReceipt.Content = new ProductUc();

            DockLayoutManager.Activate(GoodsReceipt);

            LoadingDecorator.IsSplashScreenShown = false;
        }

        private void CashInAccordionItem_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void CashOutAccordionItem_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void biLogOut_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }
    }
}
