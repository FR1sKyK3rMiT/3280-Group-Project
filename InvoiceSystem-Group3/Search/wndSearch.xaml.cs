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
using System.Windows.Shapes;

namespace InvoiceSystem_Group3.Search
{
    public partial class wndSearch : Window
    {
        // COMMENT: This property will temporarily hold the selected Invoice ID.
        // The Main Window will read this value after this window closes.
        public string SelectedInvoiceId { get; private set; }

        public wndSearch()
        {
            InitializeComponent();
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            // COMMENT: Grab the selected ID from the DataGrid or dropdown
            // SelectedInvoiceId = selectedRow.InvoiceId;
            this.Close(); // Return focus to Main Window
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectedInvoiceId = null;
            this.Close();
        }
    }
}