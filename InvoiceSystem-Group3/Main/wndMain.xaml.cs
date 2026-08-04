using System;
using System.Collections.Generic;
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
using System.Data;
using System.Linq;

using InvoiceSystem_Group3.Search;
using InvoiceSystem_Group3.Items;
using InvoiceSystem_Group3.Main;
using InvoiceSystem_Group3;
using System.Runtime.Remoting.Activation;
//using InvoiceSystem_Group3.clsDataAccess;

namespace InvoiceSystem_Group3.Main
{
    public partial class wndMain : Window
    {

        private clsMainLogic logic = new clsMainLogic();
        private clsMainSQL sql = new clsMainSQL();
        private clsDataAccess db = new clsDataAccess();

        public wndMain()
        {
            InitializeComponent();
        }

        // ============================================================
        //                     WINDOW LOADED
        // ============================================================
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                db.TestConnection;
                LoadItemsIntoComboBox();
                SwitchToReadOnlyMode();
                txtInvoiceNum.Text = ""; //No invoice loaded yet
                txtTotal.Text = "0.00";
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error loading main window: " + ex.Message);
            }
        }

        // ============================================================
        //                     LOAD ITEMS INTO COMBOBOX
        // ============================================================
        private void LoadItemsIntoComboBox()
        {
            try
            {
                int iRet = 0;
                DataSet ds = db.ExecuteSQLStatement(sql.GetInventoryItems(), ref iRet);

                cbItems.ItemsSource = ds.Tables[0].DefaultView;
                cbItems.DisplayMemberPath = "ItemDesc";
                cbItems.SelectedValuePath = "ItemCode";
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error loading items: " + ex.Message);
            }
        }

        // ============================================================
        //                     SEARCH WINDOW
        // ============================================================
        private void SearchInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wndSearch searchWindow = new wndSearch();
                searchWindow.ShowDialog(); 

                // After seach window closes, main window checks Selected Invoce ID
                if (searchWindow.SelectedInvoiceId != null)
                {
                    LoadInvoice(searchWindow.SelectedInvoiceId);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error opening search window: " + ex.Message);
            }
        }

        private void LoadInvoice(string invoiceId)
        {
            try
            {
                InvoiceHeader header = logic.LoadInvoiceFromSearch(invoiceId);

                txtInvoiceNum.Text = header.InvoiceNum.ToString();
                dpInvoiceDate.SelectedDate = header.InvoiceDate;
                txtTotal.Text = header.TotalCost.ToString("F2");

                dgLineItems.ItemsSource = null;
                dgLineItems.ItemsSource = logic.LineItems;

                SwitchToReadOnlyMode();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error loading invoice: " + ex.Message0);
            }
        }

        // ============================================================
        //                     ITEMS WINDOW
        // ============================================================

        private void ManageItemsBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Items.wndItems itemsWindow = new Items.wndItems();
                itemsWindow.ShowDialog();

                // Items window sets InventoryChanged = true if user modified items
                if (itemsWindow.InventoryChanged)
                {
                    RefreshItemsAfterEdit();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error opening items window: " + ex.Message);
            }
        }

        public void RefreshItemsAfterEdit()
        {
            try
            {
                DataSet ds = logic.RefreshItemsAfterEdit();

                cbItems.ItemsSource = ds.Tables[0].DefaultView;
                cbItems.DisplayMemberPath = "ItemDesc";
                cbItems.SelectedValuePath = "ItemCode";

                //If item description changed, update DataGrid display
                dgLineItems.ItemsSource = null;
                dgLineItems.ItemsSource = logic.LineItems;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error refreshing items: " + ex.Message);
            }

        }

        // ============================================================
        //                     NEW INVOICE
        // ============================================================
        private void NewInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                logic.StartNewInvoice();

                txtInvoiceNum.Text = "TBD";
                dpInvoiceDate.SelectedDate = null;
                txtTotal.Text = "0.00";

                dgLineItems.ItemsSource = null;
                dgLineItems.ItemsSource = logic.LineItems;

                SwitchToEditMode();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error starting new invoice: " + ex.Message);
            }
        }

        // ============================================================
        //                     ADD ITEM
        // ============================================================
        private void cbItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(cbItems.SelectedItem == null)
                {
                    return;
                }

                DataRowView row = cbItems.SelectedItem as DataRowView;
                txtItemCost.Text = row["Cost"].ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error selecting item: " + ex.Message);
            }
        }

        private void AddItemBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(cbItems.SelectedItem == null)
                {
                    MessageBox.Show("Please select an item. ");
                    return;
                }

                DataRowView row = cbItems.SelectedItem as DataRowView;

                string code = row["ItemCode"].ToString();
                string desc = row["ItemDesc"].ToString();
                decimal cost = Convert.ToDecimal(row["Cost"]);


                logic.AddItem(code, desc, cost);

                dgLineItems.ItemsSource = null;
                dgLineItems.ItemsSource = logic.LineItems;

                txtTotal.Text = logic.RunningTotal.ToString("F2");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error adding item: " + ex.Message);
            }
        }

        // ============================================================
        //                     DELETE ITEM
        // ============================================================
        private void DeleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(dgLineItems.SelectedItem == null)
                {
                    MessageBox.Show("Select an item to delete.");
                    return;
                }

                InvoiceLineItem selected = dgLineItems.SelectedItem as InvoiceLineItem;

                logic.DeleteItem(selected.LineItemNum);

                dgLineitems.ItemsSource = null;
                dgLineItems.ItemsSource = logic.LineItems;

                txtTotal.Text = logic.RunningTotal.ToString("F2");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error deleting item: " + ex.Message);
            }
        }

        // ============================================================
        //                     SAVE INVOICE
        // ============================================================
        private void SaveInvoiceBtn_Click(object sender, RoutedEventArgs e) 
        {
            try
            {
                if(dpInvoiceDate.SelectedDate == null)
                {
                    MessageBox.SelectedDate("Invoice date is required. ");
                    return;
                }

                int newId = logic.SaveInvoice(dpInvoiceDate.SelectedDate.Value);

                txtInvoiceNum.Text = newId.ToString();
                txtTotal.Text = logic.RunningTotal.ToString("F2");

                SwitchToReadOnlyMode();

                MessageBox.Show("Invoice saved successfully.");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error saving invoice: " + ex.Message);
            }
        }


        // ============================================================
        //                     EDIT MODE
        // ============================================================
        private void EditInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                logic.SwitchToEditMode();
                SwitchToEditMode();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error switching to edit mode: " + ex.Message);
            }
        }

        private void SwitchToEditMode()
        {
            dpInvoiceDate.IsEnabled = true;
            cbItems.IsEnabled = true;
            txtItemCost.IsEnabled = true;
            dgLineItems.IsEnabled = true;

            //buttons
            SaveInvoiceBtn.IsEnabled = true;
            AddItemBtn.IsEnabled = true;
            DeleteItemBtn.IsEnabled = true;
        }

        private void SwitchToReadOnlyMode()
        {
            dpInvoiceDate.IsEnabled = false;
            cbItems.IsEnabled = false;
            txtItemCost.IsEnabled = false;
            dgLineItems.IsEnabled = false;

            //buttons
            SaveInvoiceBtn.IsEnabled = false;
            AddItemBtn.IsEnabled = false;
            DeleteItemBtn.IsEnabled = false;
        }




    }
}
