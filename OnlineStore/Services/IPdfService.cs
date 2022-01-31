using DAL.Data.Entities;
using System.Collections.Generic;
using System.IO;

namespace OnlineStore.Services
{
    public interface IPdfService
    {
        /// <summary>
        /// Print orders to PDF
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="orders">Orders</param>
        /// <param name="languageId">Language identifier; 0 to use a language used when placing an order</param>
        /// <param name="vendorId">Vendor identifier to limit products; 0 to print all products. If specified, then totals won't be printed</param>
        //void PrintOrdersToPdf(Stream stream, IList<Order> orders, int languageId = 0, int vendorId = 0);
        void PrintOrdersToPdf(Stream stream, Order order, int languageId = 0, int vendorId = 0);
    }
}