using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;
        public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomersService customersService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var ordersResult = await ordersService.GetOrdersAsync(customerId);
            var productsResult = await productsService.GetproductsAsync();
            var customersResult = await customersService.GetCustomerAsync(customerId);

            if (ordersResult.IsSuccess)
            {
                foreach (var orders in ordersResult.Orders)
                {
                    foreach (var item in orders.Items)
                    {
                        item.ProductName = productsResult.IsSuccess? // search only if we have some results otherwise send: product not available!
                            productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name : "Product info is not available!"; // extract only the name of product
                    }
                }
                var result = new
                {
                    Customer = customersResult.IsSuccess ? customersResult.Customer : new { Name = "Customer info not available!" },
                    Orders = ordersResult.Orders
                };
                return (true, result);
            }
            // for testing:
            // await Task.Delay(1);
            // return (true, new { Message = "Hello"});
            return (false, null);
        }
    }
}
