using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem_Group3.Search
{
    public class clsSearchSQL
    {
        /// <summary>
        /// This SQL selects all columns to bind to the primary data grid.
        /// </summary>
        public string SelectAllInvoices()
        {
            string sSQL = "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices";
            return sSQL;
        }

        /// <summary>
        /// This SQL populates the unique Invoice Number lookup combo box.
        /// </summary>
        public string GetUniqueInvoiceNumbers()
        {
            string sSQL = "SELECT Distinct(InvoiceNum) FROM Invoices";
            return sSQL;
        }

        /// <summary>
        /// This SQL populates the unique Invoice Date filter combo box.
        /// </summary>
        public string GetUniqueInvoiceDates()
        {
            string sSQL = "SELECT Distinct(InvoiceDate) FROM Invoices";
            return sSQL;
        }

        /// <summary>
        /// This SQL populates the unique Total Cost filter combo box.
        /// </summary>
        public string GetUniqueTotalCosts()
        {
            string sSQL = "SELECT Distinct(TotalCost) FROM Invoices";
            return sSQL;
        }

        /// <summary>
        /// Dynamic multi-attribute filtering SQL based on selected criteria.
        /// </summary>
        public string FilterInvoices(string sInvoiceId, string sDate, string sTotalCost)
        {
            string sSQL = "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices WHERE 1=1";

            if (!string.IsNullOrEmpty(sInvoiceId)) sSQL += " AND InvoiceNum = " + sInvoiceId;
            if (!string.IsNullOrEmpty(sDate)) sSQL += $" AND InvoiceDate = '{sDate}'";
            if (!string.IsNullOrEmpty(sTotalCost)) sSQL += " AND TotalCost = " + sTotalCost;

            return sSQL;
        }
    }
}

