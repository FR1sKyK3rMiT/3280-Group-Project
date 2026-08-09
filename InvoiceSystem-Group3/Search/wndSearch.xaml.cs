using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace InvoiceSystem_Group3.Search
{
    /// <summary>
    /// Interaction logic for wndSearch.xaml. Handles UI event mapping for invoice lookups.
    /// </summary>
    public partial class wndSearch : Window
    {
        /// <summary>
        /// Instance object tracking our business logic rules layer.
        /// </summary>
        private readonly clsSearchLogic logicLayer;

        /// <summary>
        /// Holds the selected Invoice ID to be read by the Main Window after closing.
        /// </summary>
        public string SelectedInvoiceId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Search Window.
        /// </summary>
        public wndSearch()
        {
            try
            {
                InitializeComponent();
                logicLayer = new clsSearchLogic();

                // Load all initial database records and populate dropdowns when window opens
                RefreshSearchFormState();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing search window controls: " + ex.Message, "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Resets and repopulates the entire form state using fresh database calls.
        /// </summary>
        private void RefreshSearchFormState()
        {
            try
            {
                // Unbind selection events temporarily to prevent premature firing during loading
                cbInvoiceNum.SelectionChanged -= Filter_SelectionChanged;
                cbInvoiceDate.SelectionChanged -= Filter_SelectionChanged;
                cbTotalCost.SelectionChanged -= Filter_SelectionChanged;

                // Load your unique sorted item sources from your business logic class
                cbInvoiceNum.ItemsSource = logicLayer.GetInvoiceIdFilterItems();
                cbInvoiceDate.ItemsSource = logicLayer.GetInvoiceDateFilterItems();
                cbTotalCost.ItemsSource = logicLayer.GetInvoiceCostFilterItems();

                // Clear chosen dropdown selections
                cbInvoiceNum.SelectedIndex = -1;
                cbInvoiceDate.SelectedIndex = -1;
                cbTotalCost.SelectedIndex = -1;

                // Bind events back to controls securely
                cbInvoiceNum.SelectionChanged += Filter_SelectionChanged;
                cbInvoiceDate.SelectionChanged += Filter_SelectionChanged;
                cbTotalCost.SelectionChanged += Filter_SelectionChanged;

                // Load all starting data rows into the data table display component
                UpdateInvoicesGridDisplay();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed refreshing system form caches: " + ex.Message);
            }
        }

        /// <summary>
        /// Gathers selected filter strings and updates the DataGrid dataset concurrently.
        /// </summary>
        private void UpdateInvoicesGridDisplay()
        {
            try
            {
                string sId = cbInvoiceNum.SelectedItem != null ? cbInvoiceNum.SelectedItem.ToString() : string.Empty;
                string sDate = cbInvoiceDate.SelectedItem != null ? cbInvoiceDate.SelectedItem.ToString() : string.Empty;
                string sCost = cbTotalCost.SelectedItem != null ? cbTotalCost.SelectedItem.ToString() : string.Empty;

                // Call unified combined filter execution
                DataTable dtResults = logicLayer.ExecuteFilteredSearch(sId, sDate, sCost);
                dgInvoices.ItemsSource = dtResults.DefaultView;
            }
            catch (Exception ex)
            {
                throw new Exception("Error compiling filtered grid view matrices: " + ex.Message);
            }
        }

        /// <summary>
        /// Fires whenever any combo box selection changes to refine matching results.
        /// </summary>
        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UpdateInvoicesGridDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error compiling database filter results: " + ex.Message, "Filter Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Fires when the user selects a specific row item from the active grid container.
        /// </summary>
        private void DgInvoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView drvSelected = dgInvoices.SelectedItem as DataRowView;
                logicLayer.ConfirmSelection(drvSelected);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing row choice mapping: " + ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles resetting filters back to their default structural states.
        /// </summary>
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshSearchFormState();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error resetting application filters: " + ex.Message, "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Passes the final chosen tracking key back to the application parent screen scope.
        /// </summary>
        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (logicLayer.SelectedInvoiceId != -1)
                {
                    SelectedInvoiceId = logicLayer.SelectedInvoiceId.ToString();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please select an active invoice from the grid view listing first.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error confirming invoice tracking key selections: " + ex.Message, "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Safely aborts search context loops returning focus to parent UI view.
        /// </summary>
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedInvoiceId = null;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing exit action request: " + ex.Message, "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}