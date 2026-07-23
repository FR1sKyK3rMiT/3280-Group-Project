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

namespace InvoiceSystem_Group3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            wndSearch searchWindow = new wndSearch();
            searchWindow.Show();
        }

        private void ManageItemsBtn_Click(object sender, RoutedEventArgs e)
        {
            wndItems itemsWidnow = new wndItems();
            itemsWidnow.Show();
        }


    }
}
