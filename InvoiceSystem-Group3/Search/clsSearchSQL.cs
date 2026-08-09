using System;

namespace InvoiceSystem_Group3.Search
{
    /// <summary>
    /// Contains all SQL statements utilized by the Search Window.
    /// This keeps all raw database command text isolated to a single, static class.
    /// </summary>
    public static class clsSearchSQL
    {
        /// <summary>
        /// Returns a query to get unique, distinct invoice numbers sorted from lowest to highest.
        /// </summary>
        /// <returns>SQL string for unique invoice numbers.</returns>
        public static string GetDistinctInvoiceNums()
        {
            return "SELECT DISTINCT InvoiceNum FROM Invoices ORDER BY InvoiceNum ASC";
        }

        /// <summary>
        /// Returns a query to get unique, distinct invoice dates sorted chronologically.
        /// </summary>
        /// <returns>SQL string for unique invoice dates.</returns>
        public static string GetDistinctInvoiceDates()
        {
            return "SELECT DISTINCT InvoiceDate FROM Invoices ORDER BY InvoiceDate ASC";
        }

        /// <summary>
        /// Returns a query to get unique, distinct total costs sorted from smallest to largest.
        /// </summary>
        /// <returns>SQL string for unique total costs.</returns>
        public static string GetDistinctTotalCosts()
        {
            return "SELECT DISTINCT TotalCost FROM Invoices ORDER BY TotalCost ASC";
        }

        /// <summary>
        /// Dynamically builds a combined query allowing multiple filter fields to be active simultaneously.
        /// </summary>
        /// <param name="sInvoiceNum">The selected invoice number filter value (can be null/empty).</param>
        /// <param name="sDate">The selected date filter value (can be null/empty).</param>
        /// <param name="sTotalCost">The selected total cost filter value (can be null/empty).</param>
        /// <returns>A dynamically built SQL statement containing all active criteria selections.</returns>
        public static string FilterInvoices(string sInvoiceNum, string sDate, string sTotalCost)
        {
            string sQuery = "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices WHERE 1=1";

            if (!string.IsNullOrEmpty(sInvoiceNum))
            {
                sQuery += " AND InvoiceNum = " + sInvoiceNum;
            }

            if (!string.IsNullOrEmpty(sDate))
            {
                sQuery += " AND InvoiceDate = #" + sDate + "#";
            }

            if (!string.IsNullOrEmpty(sTotalCost))
            {
                sQuery += " AND TotalCost = " + sTotalCost;
            }

            sQuery += " ORDER BY InvoiceNum ASC";
            return sQuery;
        }
    }
}
