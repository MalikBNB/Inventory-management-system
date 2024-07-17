using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Xpf.Bars;
using Hisba.Data.Layers;

namespace Hisba.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ApplicationThemeHelper.ApplicationThemeName = "Win11Light";
            base.OnStartup(e);
            ThemedWindow.RoundCorners = true;

            using (var db = new AppDbContext())
            {

            }
        }
    }

    public class Helper
    {
        #region GridControl

        #region TotalSummaries

        public static readonly DependencyProperty TotalSummariesProperty = DependencyProperty.RegisterAttached("TotalSummaries", typeof(GridSummaryItems), typeof(Helper), new FrameworkPropertyMetadata(TotalSummariesPropertyChanged));

        public static void SetTotalSummaries(UIElement element, GridSummaryItems value)
        {
            element.SetValue(TotalSummariesProperty, value);
        }

        public static GridSummaryItems GetTotalSummaries(UIElement element)
        {
            return (GridSummaryItems)element.GetValue(TotalSummariesProperty);
        }

        private static void TotalSummariesPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (!(source is GridControl grid) || !(e.NewValue is GridSummaryItems summaries)) return;
            foreach (var item in summaries)
            {
                if (grid.TotalSummary.Cast<GridSummaryItem>().Any(o => o.Tag?.ToString() == item.Tag.ToString())) continue;
                grid.TotalSummary.Add(item);
            }
        }

        #endregion TotalSummaries

        #region GroupSummaries

        public static readonly DependencyProperty GroupSummariesProperty = DependencyProperty.RegisterAttached("GroupSummaries", typeof(GridSummaryItems), typeof(Helper), new FrameworkPropertyMetadata(GroupSummariesPropertyChanged));

        public static void SetGroupSummaries(UIElement element, GridSummaryItems value)
        {
            element.SetValue(GroupSummariesProperty, value);
        }

        public static GridSummaryItems GetGroupSummaries(UIElement element)
        {
            return (GridSummaryItems)element.GetValue(GroupSummariesProperty);
        }

        private static void GroupSummariesPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (!(source is GridControl grid) || !(e.NewValue is GridSummaryItems summaries)) return;
            foreach (var item in summaries)
            {
                if (grid.GroupSummary.Cast<GridSummaryItem>().Any(o => o.Tag?.ToString() == item.Tag.ToString())) continue;
                grid.GroupSummary.Add(item);
            }
        }

        #endregion GroupSummaries

        #endregion GridControl

        #region TableView

        #region ColumnMenuCustomizations

        public static readonly DependencyProperty ColumnMenuCustomizationsProperty = DependencyProperty.RegisterAttached("ColumnMenuCustomizations", typeof(ColumnMenuCustomizationsItems), typeof(Helper), new FrameworkPropertyMetadata(ColumnMenuCustomizationsPropertyChanged));

        public static void SetColumnMenuCustomizations(UIElement element, ColumnMenuCustomizationsItems value)
        {
            element.SetValue(ColumnMenuCustomizationsProperty, value);
        }

        public static ColumnMenuCustomizationsItems GetColumnMenuCustomizations(UIElement element)
        {
            return (ColumnMenuCustomizationsItems)element.GetValue(ColumnMenuCustomizationsProperty);
        }

        private static void ColumnMenuCustomizationsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (!(source is TableView view) || !(e.NewValue is ColumnMenuCustomizationsItems columnMenuCustomizationsItems)) return;
            foreach (var item in columnMenuCustomizationsItems)
            {
                view.ColumnMenuCustomizations.Add(new RemoveBarItemAndLinkAction { ItemName = item.ItemName });
            }
        }

        #endregion ColumnMenuCustomizations

        #endregion TableView
    }

    public class GridSummaryItems : ObservableCollection<GridSummaryItem> { }

    public class ColumnMenuCustomizationsItems : ObservableCollection<RemoveBarItemAndLinkAction> { }
}

