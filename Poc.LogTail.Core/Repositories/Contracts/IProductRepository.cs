using System.Threading.Tasks;
using Poc.LogTail.Core.DataAccess.Mongo.Documents;

namespace Poc.LogTail.Core.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<bool> CreateAsync(Product products);
        Task<bool> UpdateAsync(Product products);
        Task<bool> UpdateStatusAsync(string productId, bool publishStatus, string resumeToken);
        Task<bool> DeleteAsync(string id);
        Task<Product> FindAsync(string id);
        Task<Product> GetPreFailureResumeToken();
    }
}
