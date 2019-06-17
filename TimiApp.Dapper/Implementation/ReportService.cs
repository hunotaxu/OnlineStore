using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;
using TimiApp.Dapper.Interfaces;
using TimiApp.Dapper.ViewModels;
using Utilities.DTOs;

namespace TimiApp.Dapper.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IConfiguration _configuration;
        public ReportService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<BestSellerOfCategoryViewModel>> GetBestSellerOfCategory()
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("OnlineStoreContextConnection")))
            {
                await sqlConnection.OpenAsync();
                try
                {
                    return await sqlConnection.QueryAsync<BestSellerOfCategoryViewModel>("GetBestSellerOfCategory", commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<PagedResult<BestSellerProductViewModel>> GetBestSellerProductsAsync(string fromDate, string toDate, int categoryId, string productName, int pageIndex, int pageSize)
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("OnlineStoreContextConnection")))
            {
                await sqlConnection.OpenAsync();
                var dynamicParams = new DynamicParameters();
                fromDate = string.IsNullOrEmpty(fromDate) == true ?
                    DateTime.MinValue.ToString("MM/dd/yyyy") :
                    DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                toDate = string.IsNullOrEmpty(toDate) == true ?
                    DateTime.Now.ToString("MM/dd/yyyy") :
                    DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                dynamicParams.Add("@fromDate", fromDate);
                dynamicParams.Add("@toDate", toDate);
                dynamicParams.Add("@productName", productName);
                dynamicParams.Add("@categoryId", categoryId);
                dynamicParams.Add("@pageIndex", pageIndex);
                dynamicParams.Add("@pageSize", pageSize);
                try
                {
                    IEnumerable<BestSellerProductViewModel> result = await sqlConnection.QueryAsync<BestSellerProductViewModel>("ListBestSellerProducts", dynamicParams, commandType: CommandType.StoredProcedure);
                    var paginationSet = new PagedResult<BestSellerProductViewModel>
                    {
                        Results = result?.Any() == true ? result.ToList() : new List<BestSellerProductViewModel>(),
                        CurrentPage = pageIndex,
                        RowCount = result?.Any() == true ? result.First().RowsCount : 0,
                        PageSize = pageSize
                    };

                    return paginationSet;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public async Task<PagedResult<ProductsHasNotBeenPurchasedViewModel>> GetListProductsHasNotBeenPurchased(string fromDate, string toDate, int categoryId, string productName, int pageIndex, int pageSize)
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("OnlineStoreContextConnection")))
            {
                await sqlConnection.OpenAsync();
                var dynamicParams = new DynamicParameters();
                fromDate = string.IsNullOrEmpty(fromDate) == true ?
                    DateTime.MinValue.ToString("MM/dd/yyyy") :
                    DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                toDate = string.IsNullOrEmpty(toDate) == true ?
                    DateTime.Now.ToString("MM/dd/yyyy") :
                    DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                dynamicParams.Add("@fromDate", fromDate);
                dynamicParams.Add("@toDate", toDate);
                dynamicParams.Add("@productName", productName);
                dynamicParams.Add("@categoryId", categoryId);
                dynamicParams.Add("@pageIndex", pageIndex);
                dynamicParams.Add("@pageSize", pageSize);
                try
                {
                    IEnumerable<ProductsHasNotBeenPurchasedViewModel> result = await sqlConnection.QueryAsync<ProductsHasNotBeenPurchasedViewModel>("ListProductsHasNotBeenPurchased", dynamicParams, commandType: CommandType.StoredProcedure);
                    var paginationSet = new PagedResult<ProductsHasNotBeenPurchasedViewModel>
                    {
                        Results = result?.Any() == true ? result.ToList() : new List<ProductsHasNotBeenPurchasedViewModel>(),
                        CurrentPage = pageIndex,
                        RowCount = result?.Any() == true ? result.First().RowsCount : 0,
                        PageSize = pageSize
                    };

                    return paginationSet;
                    //return await sqlConnection.QueryAsync<ProductsHasNotBeenPurchasedViewModel>("ListProductsHasNotBeenPurchased", dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<RevenueReportViewModel>> GetRevenueReportAsync(string fromDate, string toDate)
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("OnlineStoreContextConnection")))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                var now = DateTime.Now;

                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                fromDate = !string.IsNullOrEmpty(fromDate) == true ?
                    DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                    : firstDayOfMonth.ToString("MM/dd/yyyy");
                toDate = !string.IsNullOrEmpty(toDate) == true ?
                    DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                    : lastDayOfMonth.ToString("MM/dd/yyyy");

                dynamicParameters.Add("@fromDate", fromDate);
                dynamicParameters.Add("@toDate", toDate);

                try
                {
                    return await sqlConnection.QueryAsync<RevenueReportViewModel>(
                        "GetRevenueDaily", dynamicParameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<MostReceivingMethodViewModel>> GetTopMostOfCategoryAsync()
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("OnlineStoreContextConnection")))
            {
                await sqlConnection.OpenAsync();
                try
                {
                    IEnumerable<MostReceivingMethodViewModel> a = await sqlConnection.QueryAsync<MostReceivingMethodViewModel>("GetBestDeliveryMethod", commandType: CommandType.StoredProcedure);
                    return await sqlConnection.QueryAsync<MostReceivingMethodViewModel>("GetBestDeliveryMethod", commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
