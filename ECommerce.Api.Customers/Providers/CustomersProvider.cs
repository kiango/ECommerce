using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Models;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomerProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }


        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            //throw new NotImplementedException();

            try
            {
                logger?.LogInformation("querying customers...");
                var customers = await dbContext.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    logger?.LogInformation($"(Customer.Count) customers found in dbContext ...");
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);
                }
                return (false, null, "customers not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                logger?.LogInformation("querying customer id ...");
                var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (customer != null)
                {
                    logger?.LogInformation("customer found by id ...");
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
                    return (true, result, null);
                }
                else
                {
                    return (false, null, "customernot found by id!");
                }
                
            }
            catch (Exception ex)
            {

                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }


        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "john doe", Address = "jay street 1"});
                dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "ib ibsen", Address = "ib street 2" });
                dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "al bundy", Address = "bundy street 3" });
                dbContext.SaveChanges();

            }
        }

    }
}
