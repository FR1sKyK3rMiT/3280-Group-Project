using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem_Group3.Main
{
    public class clsMainSQL
    {
        /// <summary>
        /// This SQL gets all data on an invoice for a given InvoiceNum.
        /// </summary>
        /// <param name="sInvoiceId">The Invoice number to retrieve.</param>
        /// <returns>SQL string to select invoice information.</returns>
        public string SelectInvoiceData(string sInvoiceId)
        {
            string sSQL = "SELECT * FROM Invoices WHERE InvoiceNum = " + sInvoiceId;
            return sSQL;
        }

        /// <summary>
        /// This SQL retrieves all inventory items to populate the combo boxes.
        /// </summary>
        /// <returns>SQL string to select all items.</returns>
        public string GetInventoryItems()
        {
            string sSQL = "SELECT ItemCode, ItemDesc, Cost FROM ItemDesc";
            return sSQL;
        }

        /// <summary>
        /// This SQL inserts a brand new invoice header record.
        /// </summary>
        public string InsertInvoice(string sDate, string sTotalCost)
        {
            string sSQL = $"INSERT INTO Invoices (InvoiceDate, TotalCost) VALUES ('{sDate}', {sTotalCost})";
            return sSQL;
        }

        /// <summary>
        /// This SQL appends line items bound to a specific invoice number.
        /// </summary>
        public string InsertLineItem(string sInvoiceId, string sLineItemNum, string sItemCode)
        {
            string sSQL = $"INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) VALUES ({sInvoiceId}, {sLineItemNum}, '{sItemCode}')";
            return sSQL;
        }

        /// <summary>
        /// This SQL updates the running total cost of an existing invoice.
        /// </summary>
        public string UpdateTotalCost(string sInvoiceId, string sTotalCost)
        {
            string sSQL = $"UPDATE Invoices SET TotalCost = {sTotalCost} WHERE InvoiceNum = {sInvoiceId}";
            return sSQL;
        }

        /// <summary>
        /// This SQL deletes all line items linked to an invoice before updating or deleting.
        /// </summary>
        public string DeleteLineItems(string sInvoiceId)
        {
            string sSQL = "DELETE FROM LineItems WHERE InvoiceNum = " + sInvoiceId;
            return sSQL;
        }
    }
}
