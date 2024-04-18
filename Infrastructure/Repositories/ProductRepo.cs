using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly StoreContext _Context;
        public ProductRepo(StoreContext Context)
        {
            _Context = Context;
        }
        public async Task<IReadOnlyList<Product>> GetAllProduct()
        {
            return await _Context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _Context.Products.FindAsync(id);
        }
    }
}
