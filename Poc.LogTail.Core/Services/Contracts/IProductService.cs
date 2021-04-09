using System.Threading.Tasks;
using Poc.LogTail.Core.DataAccess.Mongo.Documents;
using Poc.LogTail.Core.ViewModels.RequestModels;

namespace Poc.LogTail.Core.Services.Contracts
{
    public interface IProductService
    {
        Task<bool> CreateProductAsync(ProductRequestModel products);
        Task<bool> UpdateProductAsync(ProductRequestModel products);
        Task<bool> UpdateStatusAsync(string productId, bool publishStatus, string resumeToken);
        Task<bool> DeleteProductAsync(string id);
        Task<Product> FindProductAsync(string id);
        Task<string> GetPreFailureResumeToken();
    }
}
