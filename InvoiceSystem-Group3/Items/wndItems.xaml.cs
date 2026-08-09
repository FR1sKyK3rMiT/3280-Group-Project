using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace InvoiceSystem_Group3.Items
{
    /// <summary>
    /// Interaction logic for wndItems.xaml.
    /// </summary>
    public partial class wndItems : Window
    {
        private readonly clsItemsLogic itemsLogic;

        public bool InventoryChanged { get; private set; }

        public bool ItemsChanged
        {
            get { return InventoryChanged; }
        }

        public wndItems()
        {
            InitializeComponent();

            itemsLogic = new clsItemsLogic();
            InventoryChanged = false;

            LoadItems();
            ClearForm();
        }

        private void LoadItems()
        {
            try
            {
                dgInventoryItems.ItemsSource =
                    itemsLogic.GetAllItems().DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "The items could not be loaded.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void dgInventoryItems_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            DataRowView selectedRow =
                dgInventoryItems.SelectedItem as DataRowView;

            if (selectedRow == null)
            {
                return;
            }

            txtItemCode.Text =
                selectedRow["ItemCode"].ToString();

            txtItemDesc.Text =
                selectedRow["ItemDesc"].ToString();

            txtItemCost.Text =
                Convert.ToDecimal(selectedRow["Cost"])
                    .ToString("0.00");

            txtItemCode.IsReadOnly = true;
        }

        private void btnAddItem_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                decimal cost;
                string errorMessage;

                if (!itemsLogic.ValidateItem(
                        txtItemCode.Text,
                        txtItemDesc.Text,
                        txtItemCost.Text,
                        out cost,
                        out errorMessage))
                {
                    ShowValidationError(errorMessage);
                    return;
                }

                itemsLogic.AddItem(
                    txtItemCode.Text,
                    txtItemDesc.Text,
                    cost);

                InventoryChanged = true;

                LoadItems();
                ClearForm();

                MessageBox.Show(
                    "The item was added successfully.",
                    "Item Added",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Item Cannot Be Added",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "The item could not be added.\n\n" + ex.Message,
                    "Add Item Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void btnUpdateItem_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                if (dgInventoryItems.SelectedItem == null)
                {
                    ShowValidationError(
                        "Select an item to update.");

                    return;
                }

                decimal cost;
                string errorMessage;

                if (!itemsLogic.ValidateItem(
                        txtItemCode.Text,
                        txtItemDesc.Text,
                        txtItemCost.Text,
                        out cost,
                        out errorMessage))
                {
                    ShowValidationError(errorMessage);
                    return;
                }

                itemsLogic.UpdateItem(
                    txtItemCode.Text,
                    txtItemDesc.Text,
                    cost);

                InventoryChanged = true;

                LoadItems();
                ClearForm();

                MessageBox.Show(
                    "The item was updated successfully.",
                    "Item Updated",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "The item could not be updated.\n\n" + ex.Message,
                    "Update Item Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void btnDeleteItem_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                if (dgInventoryItems.SelectedItem == null)
                {
                    ShowValidationError(
                        "Select an item to delete.");

                    return;
                }

                string itemCode = txtItemCode.Text.Trim();

                MessageBoxResult answer = MessageBox.Show(
                    "Are you sure you want to delete item " +
                    itemCode + "?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (answer != MessageBoxResult.Yes)
                {
                    return;
                }

                itemsLogic.DeleteItem(itemCode);

                InventoryChanged = true;

                LoadItems();
                ClearForm();

                MessageBox.Show(
                    "The item was deleted successfully.",
                    "Item Deleted",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Item Cannot Be Deleted",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "The item could not be deleted.\n\n" + ex.Message,
                    "Delete Item Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void btnClear_Click(
            object sender,
            RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            dgInventoryItems.SelectedItem = null;

            txtItemCode.Clear();
            txtItemDesc.Clear();
            txtItemCost.Clear();

            txtItemCode.IsReadOnly = false;
            txtItemCode.Focus();
        }

        private void ShowValidationError(string message)
        {
            MessageBox.Show(
                message,
                "Item Information Required",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }
}
