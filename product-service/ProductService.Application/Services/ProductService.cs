using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IRabbitMQPublisher _publisher;

        public ProductService(IProductRepository repo, IRabbitMQPublisher publisher)
        {
            _repo = repo;
            _publisher = publisher;
        }

        public async Task<Guid> CreateProductAsync(ProductCreateDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                CreatedAt = DateTime.UtcNow
            };

            var id = await _repo.CreateProductAsync(product);

            await _publisher.PublishAsync(new
            {
                ProductId = id,
                product.Name,
                product.Price,
                product.CreatedAt
            }, "product.exchange", "product.created");

            return id;
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _repo.GetProductByIdAsync(id);

            if (product == null) return null;

            return new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CreatedAt = product.CreatedAt
            };
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await _repo.GetProductsAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CreatedAt = p.CreatedAt
            });
        }
    }
}
