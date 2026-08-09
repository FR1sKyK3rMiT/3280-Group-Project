using System;
using System.Collections.Generic;
using System.Data;
using InvoiceSystem_Group3.Search;
using InvoiceSystem_Group3.Items;
using InvoiceSystem_Group3.Main;
using InvoiceSystem_Group3;

namespace InvoiceSystem_Group3.Main
{
    /// <summary>
    /// Provides the core logic for managing invoices, including creating, loading, editing, and saving invoices, as
    /// well as managing associated line items and inventory data.
    /// </summary>
    /// <remarks>This class serves as the main entry point for invoice-related operations. It maintains the
    /// state of the current invoice, including its line items, running total, and edit mode status. The class interacts
    /// with a database to retrieve and persist invoice and inventory data.   Typical usage involves calling methods
    /// such as <see cref="StartNewInvoice"/> to initialize a new invoice, <see cref="LoadInvoiceFromSearch(string)"/>
    /// to load an existing invoice, and <see cref="SaveInvoice(DateTime)"/> to save changes. Line items can be added,
    /// deleted, or modified using methods like <see cref="AddItem(string, string, decimal)"/> and <see
    /// cref="DeleteItem(int)"/>.</remarks>
    public class clsMainLogic
    {
        private clsMainSQL sql = new clsMainSQL();
        private clsDataAccess db = new clsDataAccess();

        //Internal working invoice data
        public List<InvoiceLineItem> LineItems { get; private set; } = new List<InvoiceLineItem>();
        public decimal RunningTotal { get; private set; } = 0;
        public bool IsEditMode { get; private set; } = false;

        /// <summary>
        /// Initializes a new invoice by clearing all existing line items, resetting the running total,  and enabling
        /// edit mode.
        /// </summary>
        /// <remarks>This method prepares the system for creating a new invoice by ensuring that no
        /// previous data  remains and that the invoice is ready for modifications. The invoice starts in edit mode  to
        /// allow adding new line items.</remarks>
        /// <exception cref="Exception">Thrown if an error occurs while initializing the new invoice.</exception>
        public void StartNewInvoice()
        {
            try
            {
                LineItems.Clear();
                RunningTotal = 0;
                IsEditMode = true; //New invoice starts in edit mode
            }
            catch (Exception ex)
            {
                throw new Exception("Error starting new invoice: " + ex.Message);
            }
        }


        /// <summary>
        /// Loads an invoice and its associated line items based on the specified invoice ID.
        /// </summary>
        /// <remarks>This method retrieves the invoice header and its associated line items from the
        /// database. The line items are added to the <c>LineItems</c> collection, and the total cost is updated. The
        /// method sets the <c>IsEditMode</c> property to <c>false</c> after loading the invoice.</remarks>
        /// <param name="sInvoiceID">The unique identifier of the invoice to load. This value cannot be null or empty.</param>
        /// <returns>An <see cref="InvoiceHeader"/> object containing the invoice details, including the invoice number, date,
        /// and total cost.</returns>
        /// <exception cref="Exception">Thrown if the invoice cannot be found or if an error occurs while loading the invoice.</exception>
        public InvoiceHeader LoadInvoiceFromSearch(string sInvoiceID)
        {
            try
            {
                //this method is called by main window after serch window
                //the main window passes the SelectedInvoiceId from wndSearch
                int iRet = 0;

                //Load invoice header
                DataSet dsHeader = db.ExecuteSQLStatement(sql.SelectInvoiceData(sInvoiceID), ref iRet);

                if (iRet == 0)
                    throw new Exception("Invoice not found.");
                

                InvoiceHeader header = new InvoiceHeader
                {
                    InvoiceNum = Convert.ToInt32(dsHeader.Tables[0].Rows[0]["InvoiceNum"]),
                    InvoiceDate = ConvertToDateTime(dsHeader.Tables[0].Rows[0]["InvoiceDate"]),
                    TotalCost = Convert.ToDecimal(dsHeader.Tables[0].Rows[0]["TotalCost"])
                };
                

                //Load line items
                DataSet dsLines = db.ExecuteSQLStatement(sql.GetLineItemsForInvoice(sInvoiceID), ref iRet);

                LineItems.Clear();
                RunningTotal = header.TotalCost;

                foreach(DataRow row in dsLines.Tables[0].Rows)
                {
                    LineItems.Add(new InvoiceLineItem
                    {
                        LineItemNum = Convert.ToInt32(row["LineItemNum"]),
                        ItemCode = row["ItemCode"].ToString(),
                        ItemDesc = row["ItemDesc"].ToString(),
                        Cost = Convert.ToDecimal(row["Cost"])
                    });
                }

                IsEditMode = false;

                return header;
            }
            catch(Exception ex)
            {
                throw new Exception("Error loading invoice: " + ex.Message);
            }
        }


        /// <summary>
        /// Adds a new item to the invoice
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="itemDesc"></param>
        /// <param name="cost"></param>
        /// <exception cref="Exception"></exception>
        public void AddItem(string itemCode, string itemDesc, decimal cost)
        {
            try
            {
                int nextLineNum = LineItems.Count + 1;

                LineItems.Add(new InvoiceLineItem
                {
                    LineItemNum = nextLineNum,
                    ItemCode = itemCode,
                    ItemDesc = itemDesc,
                    Cost = cost
                });

                RunningTotal += cost;
            }
            catch(Exception ex)
            {
                throw new Exception("Error adding item: " + ex.Message);
            }
        }


        /// <summary>
        /// Deletes a line item from the invoice based on the specified line item number.
        /// </summary>
        /// <remarks>After the specified line item is removed, the remaining line items are renumbered
        /// sequentially starting from 1.</remarks>
        /// <param name="lineItemNum">The unique number of the line item to delete. Must correspond to an existing line item.</param>
        /// <exception cref="Exception">Thrown if an error occurs during the deletion process.</exception>
        public void DeleteItem(int lineItemNum)
        {
            try
            {
                InvoiceLineItem item = LineItems.Find(x => x.LineItemNum == lineItemNum);

                if (item == null)
                    throw new Exception("Line item not found. ");

                RunningTotal -= item.Cost;
                LineItems.Remove(item);

                //Re-Number remaining items
                int counter = 1;
                foreach(var li in LineItems)
                {
                    li.LineItemNum = counter++;
                }

            }
            catch(Exception ex)
            {
                throw new Exception("Error deleting item: " + ex.Message);
            }
        }

        /// <summary>
        /// Saves a new invoice or updates an existing one
        /// </summary>
        /// <param name="invoiceDate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int SaveInvoice(DateTime invoiceDate)
        {
            try
            {
                if (invoiceDate == DateTime.MinValue)
                    throw new Exception("Invoice date is required. ");

                //Insert invoice header
                db.ExecuteNonQuery(sql.InsertInvoice(invoiceDate.ToShortDateString(), RunningTotal.ToString()));

                //Retrieve new invoice umber
                string newId = db.ExecuteScalarSQL(sql.GetMaxInvoiceNum());

                //Insert line items
                foreach (var li in LineItems)
                {
                    db.ExecuteNonQuery(sql.InsertLineItem(newId, li.LineItemNum.ToString(), li.ItemCode));
                }

                //After saving swithc to read only mode
                IsEditMode = false;

                return Convert.ToInt32(newId);
            }
            catch(Exception ex)
            {
                throw new Exception("Error saving invoice: " + ex.Message);
            }
        }

        /// <summary>
        /// Called when the user clicks "Edit Invoice"
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void SwitchToEditMode()
        {
            try
            {
                IsEditMode = true;
            }
            catch(Exception ex)
            {
                throw new Exception("Error switchig to edit mode: " + ex.Message);
            }
        }


        /// <summary>
        /// Refreshes the inventory items by retrieving the latest data from the database.
        /// </summary>
        /// <remarks>This method is typically called after the Items Window is closed, and the inventory
        /// has been modified (e.g., items were added, edited, or deleted). It ensures that the inventory data is
        /// up-to-date.</remarks>
        /// <returns>A <see cref="DataSet"/> containing the updated inventory items. The <see cref="DataSet"/> will include the
        /// latest changes made to the inventory.</returns>
        /// <exception cref="Exception">Thrown if an error occurs while retrieving the inventory data from the database.</exception>
        public DataSet RefreshItemsAfterEdit()
        {
            try
            {
                //Main window calls this after Items Window closes
                //Items Window sets InventoryChanged = true if user added/edited/deleted items

                int iRet = 0;
                return db.ExecuteSQLStatement(sql.GetInventoryItems(), ref iRet);
            }
            catch(Exception ex)
            {
                throw new Exception("Error refreshing items: " + ex.Message);
            }
        }


        // ===================== SUPPORTING MODELS =====================

        public class InvoiceHeader
        {
            public int InvoiceNum { get; set; }
            public DateTime InvoiceDate { get; set; }
            public decimal TotalCost { get; set; }
        }

        public class InvoiceLineItem
        {
            public int LineItemNum { get; set; }
            public string ItemCode { get; set; }
            public string ItemDesc { get; set; }
            public decimal Cost { get; set; }
        }

        private DateTime ConvertToDateTime(object dbValue)
        {
            if (dbValue == null || dbValue == DBNull.Value)
                return DateTime.MinValue;

            if (dbValue is DateTime dt)
                return dt;

            if (DateTime.TryParse(dbValue.ToString(), out DateTime parsed))
                return parsed;

            return DateTime.MinValue;
        }




    }
}


