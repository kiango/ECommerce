using ECommerce.Api.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Interfaces
{
    public interface IProductsProvider
    {
        Task<(bool IsSuccess, IEnumerable<Product> Products, string ErrorMeaasage)> GetProductsAsync();
        Task<(bool IsSuccess, Product Product, string ErrorMeaasage)> GetProductAsync(int id);
    }
}
