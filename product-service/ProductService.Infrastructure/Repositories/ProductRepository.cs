using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using Dapper;

namespace ProductService.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _connection;

        public ProductRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Guid> CreateProductAsync(Product product)
        {
            var sql = @"INSERT INTO Products (Id, Name, Price, CreatedAt)
                    VALUES (@Id, @Name, @Price, @CreatedAt)";
            product.Id = Guid.NewGuid();
            product.CreatedAt = DateTime.UtcNow;

            await _connection.ExecuteAsync(sql, product);
            return product.Id;
        }

        public Task<Product?> GetProductByIdAsync(Guid id)
        {
            var sql = @"SELECT * FROM Products WHERE Id = @Id";
            var product = _connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var sql = @"SELECT * FROM Products";
            var products = await _connection.QueryAsync<Product>(sql);
            return products;
        }
    }
}
