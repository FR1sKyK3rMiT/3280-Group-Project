using System;
using System.Collections.Generic;
using System.Data;

namespace InvoiceSystem_Group3.Search
{
    /// <summary>
    /// Handles all processing, sorting, and extraction logic for the Search Window.
    /// This layer acts as the intermediary between the UI and the Data Access layer.
    /// </summary>
    public class clsSearchLogic
    {
        #region Attributes and Properties

        /// <summary>
        /// Instantiated object mapping back to the shared application database connector layer.
        /// </summary>
        private readonly clsDataAccess dbAccess;

        /// <summary>
        /// Stores the primary tracking identifier of the final user selection.
        /// </summary>
        private int iSelectedInvoiceId;

        /// <summary>
        /// Gets the primary tracking identifier of the final user selection.
        /// </summary>
        public int SelectedInvoiceId
        {
            get { return iSelectedInvoiceId; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the search business processing logic layer.
        /// </summary>
        public clsSearchLogic()
        {
            try
            {
                dbAccess = new clsDataAccess();
                iSelectedInvoiceId = -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Error initializing search logic layer: " + ex.Message);
            }
        }

        #endregion

        #region Business Processing Methods

        /// <summary>
        /// Populates and returns a collection of unique, sorted item objects for the Invoice ID Dropdown.
        /// </summary>
        /// <returns>A generic string list containing the individual dataset results.</returns>
        public List<string> GetInvoiceIdFilterItems()
        {
            try
            {
                List<string> lstIds = new List<string>();
                int iRows = 0;

                string sQuery = clsSearchSQL.GetDistinctInvoiceNums();
                DataSet dsResult = dbAccess.ExecuteSQLStatement(sQuery, ref iRows);

                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        lstIds.Add(row["InvoiceNum"].ToString());
                    }
                }
                return lstIds;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve unique invoice tracking keys: " + ex.Message);
            }
        }

        /// <summary>
        /// Populates and returns unique, sorted string formats representing specific entry dates.
        /// </summary>
        /// <returns>A standard list array matching distinct column occurrences.</returns>
        public List<string> GetInvoiceDateFilterItems()
        {
            try
            {
                List<string> lstDates = new List<string>();
                int iRows = 0;

                string sQuery = clsSearchSQL.GetDistinctInvoiceDates();
                DataSet dsResult = dbAccess.ExecuteSQLStatement(sQuery, ref iRows);

                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        if (row["InvoiceDate"] != DBNull.Value)
                        {
                            DateTime dtValue = Convert.ToDateTime(row["InvoiceDate"]);
                            lstDates.Add(dtValue.ToShortDateString());
                        }
                    }
                }
                return lstDates;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve unique invoice timestamps: " + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves and builds a unique, sorted collection tracking distinct cost value records.
        /// </summary>
        /// <returns>A string list representing sequential financial entries.</returns>
        public List<string> GetInvoiceCostFilterItems()
        {
            try
            {
                List<string> lstCosts = new List<string>();
                int iRows = 0;

                string sQuery = clsSearchSQL.GetDistinctTotalCosts();
                DataSet dsResult = dbAccess.ExecuteSQLStatement(sQuery, ref iRows);

                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        if (row["TotalCost"] != DBNull.Value)
                        {
                            decimal dCost = Convert.ToDecimal(row["TotalCost"]);
                            lstCosts.Add(dCost.ToString("F2"));
                        }
                    }
                }
                return lstCosts;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve unique charge records: " + ex.Message);
            }
        }

        /// <summary>
        /// Processes active form control objects to evaluate a multi-tier database query execution.
        /// </summary>
        /// <param name="sNum">Selected item tracking string parameter.</param>
        /// <param name="sDate">Selected timestamp property assignment.</param>
        /// <param name="sCost">Selected monetary string verification element.</param>
        /// <returns>A completed DataTable collection used to update the primary DataGrid control container.</returns>
        public DataTable ExecuteFilteredSearch(string sNum, string sDate, string sCost)
        {
            try
            {
                int iRows = 0;
                string sFilteredQuery = clsSearchSQL.FilterInvoices(sNum, sDate, sCost);
                DataSet ds = dbAccess.ExecuteSQLStatement(sFilteredQuery, ref iRows);

                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }

                return new DataTable();
            }
            catch (Exception ex)
            {
                throw new Exception("Error rendering integrated application filtering matrices: " + ex.Message);
            }
        }

        /// <summary>
        /// Persists the selected visual item to provide reference keys to the parent processing scope.
        /// </summary>
        /// <param name="selectedRow">The selected element from the active window dataset.</param>
        public void ConfirmSelection(DataRowView selectedRow)
        {
            try
            {
                if (selectedRow != null)
                {
                    iSelectedInvoiceId = Convert.ToInt32(selectedRow["InvoiceNum"]);
                }
                else
                {
                    iSelectedInvoiceId = -1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error resolving final invoice index selections: " + ex.Message);
            }
        }

        #endregion
    }
}
