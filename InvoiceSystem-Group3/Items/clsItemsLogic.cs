uusing System;
using System.Data;
using System.Globalization;

namespace InvoiceSystem_Group3.Items
{
    /// <summary>
    /// Contains validation and database operations for the Items window.
    /// </summary>
    public class clsItemsLogic
    {
        private readonly clsDataAccess dataAccess;
        private readonly clsItemsSQL itemsSQL;

        public clsItemsLogic()
        {
            dataAccess = new clsDataAccess();
            itemsSQL = new clsItemsSQL();
        }

        public DataTable GetAllItems()
        {
            int rowCount = 0;

            DataSet dataSet = dataAccess.ExecuteSQLStatement(
                itemsSQL.GetAllItems(),
                ref rowCount);

            if (dataSet.Tables.Count == 0)
            {
                return new DataTable();
            }

            return dataSet.Tables[0];
        }

        public bool ValidateItem(
            string itemCode,
            string description,
            string costText,
            out decimal cost,
            out string errorMessage)
        {
            cost = 0;
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(itemCode))
            {
                errorMessage = "Enter an item code.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                errorMessage = "Enter an item description.";
                return false;
            }

            if (!decimal.TryParse(
                    costText,
                    NumberStyles.Currency,
                    CultureInfo.CurrentCulture,
                    out cost))
            {
                errorMessage = "Enter a valid numeric cost.";
                return false;
            }

            if (cost < 0)
            {
                errorMessage = "The item cost cannot be negative.";
                return false;
            }

            return true;
        }

        public bool ItemCodeExists(string itemCode)
        {
            int rowCount = 0;

            DataSet dataSet = dataAccess.ExecuteSQLStatement(
                itemsSQL.GetItem(EscapeSql(itemCode.Trim())),
                ref rowCount);

            return dataSet.Tables.Count > 0 &&
                   dataSet.Tables[0].Rows.Count > 0;
        }

        public void AddItem(
            string itemCode,
            string description,
            decimal cost)
        {
            itemCode = itemCode.Trim();
            description = description.Trim();

            if (ItemCodeExists(itemCode))
            {
                throw new InvalidOperationException(
                    "An item with that item code already exists.");
            }

            int rowsAffected = dataAccess.ExecuteNonQuery(
                itemsSQL.InsertNewItem(
                    EscapeSql(itemCode),
                    EscapeSql(description),
                    cost));

            if (rowsAffected != 1)
            {
                throw new Exception("The item could not be added.");
            }
        }

        public void UpdateItem(
            string itemCode,
            string description,
            decimal cost)
        {
            int rowsAffected = dataAccess.ExecuteNonQuery(
                itemsSQL.UpdateItemDetails(
                    EscapeSql(itemCode.Trim()),
                    EscapeSql(description.Trim()),
                    cost));

            if (rowsAffected != 1)
            {
                throw new Exception(
                    "The selected item could not be updated.");
            }
        }

        public bool CanDeleteItem(
            string itemCode,
            out string invoiceNumbers)
        {
            int rowCount = 0;

            DataSet dataSet = dataAccess.ExecuteSQLStatement(
                itemsSQL.CheckItemUsage(
                    EscapeSql(itemCode.Trim())),
                ref rowCount);

            invoiceNumbers = string.Empty;

            if (dataSet.Tables.Count == 0 ||
                dataSet.Tables[0].Rows.Count == 0)
            {
                return true;
            }

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                if (invoiceNumbers.Length > 0)
                {
                    invoiceNumbers += ", ";
                }

                invoiceNumbers +=
                    row["InvoiceNum"].ToString();
            }

            return false;
        }

        public void DeleteItem(string itemCode)
        {
            string invoiceNumbers;

            if (!CanDeleteItem(itemCode, out invoiceNumbers))
            {
                throw new InvalidOperationException(
                    "This item cannot be deleted because it is used by " +
                    "invoice(s): " + invoiceNumbers);
            }

            int rowsAffected = dataAccess.ExecuteNonQuery(
                itemsSQL.DeleteItemDefinition(
                    EscapeSql(itemCode.Trim())));

            if (rowsAffected != 1)
            {
                throw new Exception(
                    "The selected item could not be deleted.");
            }
        }

        private string EscapeSql(string value)
        {
            return value.Replace("'", "''");
        }
    }
}