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
            return "SELECT * FROM Invoices WHERE InvoiceNum = " + sInvoiceId;
        }

        /// <summary>
        /// This SQL retrieves all inventory items to populate the combo boxes.
        /// </summary>
        /// <returns>SQL string to select all items.</returns>
        public string GetInventoryItems()
        {
            return "SELECT ItemCode, ItemDesc, Cost FROM ItemDesc";
        }

        /// <summary>
        /// This SQL inserts a brand new invoice header record.
        /// </summary>
        public string InsertInvoice(string sDate, string sTotalCost)
        {
            return $"INSERT INTO Invoices (InvoiceDate, TotalCost) VALUES ('{sDate}', {sTotalCost})";
        }

        /// <summary>
        /// This SQL appends line items bound to a specific invoice number.
        /// </summary>
        public string InsertLineItem(string sInvoiceId, string sLineItemNum, string sItemCode)
        {
            return $"INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) VALUES ({sInvoiceId}, {sLineItemNum}, '{sItemCode}')";
        }

        /// <summary>
        /// This SQL updates the running total cost of an existing invoice.
        /// </summary>
        public string UpdateTotalCost(string sInvoiceId, string sTotalCost)
        {
            return  $"UPDATE Invoices SET TotalCost = {sTotalCost} WHERE InvoiceNum = {sInvoiceId}";
        }

        /// <summary>
        /// This SQL deletes all line items linked to an invoice before updating or deleting.
        /// </summary>
        public string DeleteLineItems(string sInvoiceId)
        {
            return "DELETE FROM LineItems WHERE InvoiceNum = " + sInvoiceId;
        }


        /// <summary>
        /// Retrieves all line items for a given invoice, including item descriptions and cost.
        /// </summary>
        /// <remarks>
        /// This joins ItemDesc so Main Window can display ItemDesc + Cost in the DataGrid.
        /// </remarks>
        public string GetLineItemsForInvoice(string sInvoiceId)
        {
            return $@"
                SELECT 
                    LineItems.LineItemNum,
                    LineItems.ItemCode,
                    ItemDesc.ItemDesc,
                    ItemDesc.Cost
                FROM LineItems
                INNER JOIN ItemDesc ON LineItems.ItemCode = ItemDesc.ItemCode
                WHERE LineItems.InvoiceNum = {sInvoiceId}
                ORDER BY LineItems.LineItemNum";
        }

        /// <summary>
        /// Retrieves the highest InvoiceNum in the database.
        /// Used after inserting a new invoice to determine the new ID.
        /// </summary>
        public string GetMaxInvoiceNum()
        {
            return "SELECT MAX(InvoiceNum) FROM Invoices";
        }


        /// <summary>
        /// Deletes a single line item from an invoice.
        /// </summary>
        /// <remarks>
        /// This is optional but useful if you want to delete individual items
        /// without wiping the entire invoice.
        /// </remarks>
        public string DeleteSingleLineItem(string sInvoiceId, string sLineItemNum)
        {
            return $"DELETE FROM LineItems WHERE InvoiceNum = {sInvoiceId} AND LineItemNum = {sLineItemNum}";
        }
    }
}
