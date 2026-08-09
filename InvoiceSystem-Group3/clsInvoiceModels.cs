using System;

namespace InvoiceSystem_Group3
{
    /// <summary>
    /// Global model tracking shared invoice header details across windows.
    /// </summary>
    public class InvoiceHeader
    {
        public int InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalCost { get; set; }
    }

    /// <summary>
    /// Global model tracking shared line item elements across windows.
    /// </summary>
    public class InvoiceLineItem
    {
        public int LineItemNum { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public decimal Cost { get; set; }
    }
}