
using ECommerce.Api.Customers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Interfaces
{
    public interface ICustomerProvider
    {
        // return the whole customer catalog
        Task<(bool IsSuccess, IEnumerable<Customer> Customers, string ErrorMessage)> GetCustomersAsync();

        //return single customer
        Task<(bool IsSuccess, Customer Customer, string ErrorMessage)> GetCustomerAsync(int id);




    }
}
