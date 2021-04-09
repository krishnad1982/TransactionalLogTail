using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Poc.LogTail.Core.DataAccess.Mongo;
using Poc.LogTail.Core.DataAccess.Mongo.Documents;
using Poc.LogTail.Core.Repositories.Contracts;

namespace Poc.LogTail.Core.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _collection;

        public ProductRepository(IDbContext<IMongoDatabase> mongoDbContext)
        {
            _collection = mongoDbContext.GetDatabase().GetCollection<Product>(nameof(Product));
        }

        public async Task<bool> CreateAsync(Product products)
        {
            try
            {
                await _collection.InsertOneAsync(products);
                return true;
            }
            catch (Exception ex)
            {
                //log here
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Product products)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(sub => sub.ProductId, products.ProductId);
                var update = Builders<Product>.Update.Set(sub => sub.PublishStatus, products.PublishStatus)
                    .Set(sub => sub.ProductName, products.ProductName);
                await _collection.UpdateOneAsync(filter, update);
                return true;
            }
            catch (Exception ex)
            {
                //log here
                return false;
            }
        }

        public async Task<bool> UpdateStatusAsync(string productId, bool publishStatus, string resumeToken)
        {
            try
            {
                var filter = Builders<Product>.Filter.Eq(sub => sub.ProductId, productId);
                var update = Builders<Product>.Update.Set(sub => sub.PublishStatus, publishStatus)
                    .Set(sub => sub.ResumeToken, resumeToken);
                await _collection.UpdateOneAsync(filter, update);
                return true;
            }
            catch (Exception ex)
            {
                //log here
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                await _collection.DeleteOneAsync(sub => sub.ProductId == id);
                return true;
            }
            catch (Exception ex)
            {
                //log here
                return false;
            }
        }

        public async Task<Product> FindAsync(string id)
        {
            var result = await _collection.FindAsync(sub => sub.ProductId == id);
            return await result.SingleOrDefaultAsync();
        }

        public async Task<Product> GetPreFailureResumeToken()
        {
            var sort = Builders<Product>.Sort.Descending(x => x.CreatedDateTimeUtc);
            var result = await _collection.FindAsync(sub => sub.ResumeToken != null,
                new FindOptions<Product, Product> {Sort = sort});
            return await result.FirstOrDefaultAsync();
        }
    }
}
