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

namespace InvoiceSystem_Group3.Items
{
    public partial class wndItems : Window
    {
        // This fulfills the required Items Window interface comment assignment
        public bool InventoryChanged { get; set; } = false;

        public wndItems()
        {
            InitializeComponent();
        }
    }
}