using System.Threading.Tasks;
using Poc.LogTail.Core.DataAccess.Mongo.Documents;
using Poc.LogTail.Core.Repositories.Contracts;
using Poc.LogTail.Core.Services.Contracts;
using Poc.LogTail.Core.ViewModels.RequestModels;

namespace Poc.LogTail.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> CreateProductAsync(ProductRequestModel products)
        {
            var newProducts = new Product
            {
                ProductId = products.ProductId,
                ProductName = products.ProductName,
                Price = products.Price,
                PublishStatus = products.PublishStatus
            };
            return await _productRepository.CreateAsync(newProducts);
        }

        public async Task<bool> UpdateProductAsync(ProductRequestModel products)
        {
            var newProducts = new Product
            {
                ProductId = products.ProductId,
                ProductName = products.ProductName,
                Price = products.Price,
                PublishStatus = products.PublishStatus
            };
            return await _productRepository.UpdateAsync(newProducts);
        }
        public async Task<bool> UpdateStatusAsync(string productId, bool publishStatus, string resumeToken)
        {
            return await _productRepository.UpdateStatusAsync(productId, publishStatus, resumeToken);
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<Product> FindProductAsync(string id)
        {
            return await _productRepository.FindAsync(id);
        }

        public async Task<string> GetPreFailureResumeToken()
        {
            var result = await _productRepository.GetPreFailureResumeToken();
            return result?.ResumeToken;
        }
    }
}
