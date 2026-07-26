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

using InvoiceSystem_Group3.Search;
using InvoiceSystem_Group3.Items;

namespace InvoiceSystem_Group3.Main
{
    public partial class wndMain : Window
    {
        public wndMain()
        {
            InitializeComponent();
        }

        private void SearchInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            wndSearch searchWindow = new wndSearch();
            searchWindow.ShowDialog(); // ShowDialog halts Main until Search closes

            // COMMENT: After Search Window closes, extract the chosen Invoice ID 
            if (searchWindow.SelectedInvoiceId != null)
            {
                string targetId = searchWindow.SelectedInvoiceId;
                // Code here to pass targetId to clsMainLogic to load invoice data
            }
        }

        private void ManageItemsBtn_Click(object sender, RoutedEventArgs e)
        {
            wndItems itemsWindow = new wndItems();
            itemsWindow.ShowDialog();

            // COMMENT: Check if items were modified while the window was open.
            // If true, call logic method to refresh the items ComboBox layout.
            if (itemsWindow.InventoryChanged)
            {
                // RefreshItemsComboBox();
            }
        }
    }
}
