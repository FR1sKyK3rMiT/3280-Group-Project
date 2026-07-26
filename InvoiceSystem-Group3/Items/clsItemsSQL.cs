using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem_Group3.Items
{
    public class clsItemsSQL
    {
        /// <summary>
        /// Retrieves the entire baseline inventory table for the items editor data grid.
        /// </summary>
        public string GetAllItems()
        {
            string sSQL = "SELECT ItemCode, ItemDesc, Cost FROM ItemDesc";
            return sSQL;
        }

        /// <summary>
        /// Saves a newly defined product into the central definition lookup table.
        /// </summary>
        public string InsertNewItem(string sCode, string sDesc, string sCost)
        {
            string sSQL = $"INSERT INTO ItemDesc (ItemCode, ItemDesc, Cost) VALUES ('{sCode}', '{sDesc}', {sCost})";
            return sSQL;
        }

        /// <summary>
        /// Edits pricing or descriptions of products in the central lookup table.
        /// </summary>
        public string UpdateItemDetails(string sCode, string sDesc, string sCost)
        {
            string sSQL = $"UPDATE ItemDesc SET ItemDesc = '{sDesc}', Cost = {sCost} WHERE ItemCode = '{sCode}'";
            return sSQL;
        }

        /// <summary>
        /// Drops a product configuration option entirely from inventory.
        /// </summary>
        public string DeleteItemDefinition(string sCode)
        {
            string sSQL = $"DELETE FROM ItemDesc WHERE ItemCode = '{sCode}'";
            return sSQL;
        }

        /// <summary>
        /// Safe validation check to make sure an item isn't actively tied to a real invoice before deleting.
        /// </summary>
        public string CheckItemUsage(string sCode)
        {
            string sSQL = $"SELECT Distinct(InvoiceNum) FROM LineItems WHERE ItemCode = '{sCode}'";
            return sSQL;
        }
    }
}
