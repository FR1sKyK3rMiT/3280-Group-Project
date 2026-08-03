using System;

namespace InvoiceSystem_Group3.Main
{
    public class clsMainLogic
    {
        public void LoadInvoiceFromSearch(string sInvoiceID)
        {
            //TODO
        }

        public void RefreshItemsAFterEdit()
        {
            //TODO
        }

        public void StartNewInvoice()
        {
            //TODO
        }

        public decimal CalculateTotal(decimal currentTotal, decimal itemCost, bool adding)
        {
            if(adding)
                return currentTotal + itemCost;
            else
                return currentTotal - itemCost;
        }

        public void SwitchToEditMode()
        {
            //TODO
        }


    }
}


