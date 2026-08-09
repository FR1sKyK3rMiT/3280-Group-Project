using System.Globalization;

namespace InvoiceSystem_Group3.Items
{
    /// <summary>
    /// Builds the SQL statements used by the Items portion of the application.
    /// Values passed to this class must already be escaped when appropriate.
    /// </summary>
    public class clsItemsSQL
    {
        public string GetAllItems()
        {
            return "SELECT ItemCode, ItemDesc, Cost " +
                   "FROM ItemDesc ORDER BY ItemCode";
        }

        public string GetItem(string itemCode)
        {
            return "SELECT ItemCode FROM ItemDesc " +
                   $"WHERE ItemCode = '{itemCode}'";
        }

        public string InsertNewItem(
            string itemCode,
            string description,
            decimal cost)
        {
            string databaseCost = cost.ToString(CultureInfo.InvariantCulture);

            return "INSERT INTO ItemDesc (ItemCode, ItemDesc, Cost) " +
                   $"VALUES ('{itemCode}', '{description}', {databaseCost})";
        }

        public string UpdateItemDetails(
            string itemCode,
            string description,
            decimal cost)
        {
            string databaseCost = cost.ToString(CultureInfo.InvariantCulture);

            return "UPDATE ItemDesc SET " +
                   $"ItemDesc = '{description}', Cost = {databaseCost} " +
                   $"WHERE ItemCode = '{itemCode}'";
        }

        public string DeleteItemDefinition(string itemCode)
        {
            return "DELETE FROM ItemDesc " +
                   $"WHERE ItemCode = '{itemCode}'";
        }

        public string CheckItemUsage(string itemCode)
        {
            return "SELECT DISTINCT InvoiceNum FROM LineItems " +
                   $"WHERE ItemCode = '{itemCode}'";
        }
    }
}
