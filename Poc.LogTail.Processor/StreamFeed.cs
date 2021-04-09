using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Poc.LogTail.Core.Contracts;
using Poc.LogTail.Core.DataAccess.Mongo;
using Poc.LogTail.Core.DataAccess.Mongo.Documents;
using Poc.LogTail.Core.Models;
using Poc.LogTail.Core.Services.Contracts;

namespace Poc.LogTail.Processor
{
    public class StreamFeed : IStreamFeed
    {
        private readonly IMongoCollection<Product> _collection;
        private readonly IProductService _productService;

        public StreamFeed(IDbContext<IMongoDatabase> mongoDbContext, IProductService productService)
        {
            _collection = mongoDbContext.GetDatabase().GetCollection<Product>(nameof(Product));
            _productService = productService;
        }

        public async Task StartFeed()
        {
            try
            {
                var watchPipeline = BuildPipelineDefinition();
                var watchResumeToken = await GetPreFailureResumeToken();
                var watchChangeStreamOptions = BuildChangeStreamOptions(watchResumeToken);

                using var cursor = await _collection.WatchAsync(watchPipeline, watchChangeStreamOptions);
                await cursor.ForEachAsync(async change =>
                {
                    var resumetoken = change.ResumeToken["_data"];
                    var product = change.FullDocument;
                    // Publish to Azure service bus
                    await PublishToAzureServiceBus(product.ProductId, resumetoken.AsString);

                    Console.WriteLine(
                        $"Product {product.ProductId} - {product.ProductName} with status {product.PublishStatus} and resume token {resumetoken} has been sent to Azure Service Bus");
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static PipelineDefinition<ChangeStreamDocument<Product>, ChangeStreamDocument<Product>> BuildPipelineDefinition()
        {
            var pipeline =
                new EmptyPipelineDefinition<ChangeStreamDocument<Product>>()
                    .Match(x => x.FullDocument.PublishStatus == false &&
                                (x.OperationType == ChangeStreamOperationType.Insert ||
                                 x.OperationType == ChangeStreamOperationType.Update));
            return pipeline;
        }

        private static ChangeStreamOptions BuildChangeStreamOptions(BsonDocument resumeToken)
        {
            var changeStreamOptions = new ChangeStreamOptions
            {
                FullDocument =
                    ChangeStreamFullDocumentOption
                        .UpdateLookup // Confgure to get full document whilst getting the update operation [by default update does not include full document]
            };
            if (!resumeToken.GetElement("_data").Value.IsBsonNull)
            {
                changeStreamOptions.ResumeAfter = resumeToken; // Confugre the resume token here, if any
            }

            return changeStreamOptions;
        }

        private async Task<BsonDocument> GetPreFailureResumeToken()
        {
            var tokens = new Tokens {Data = await _productService.GetPreFailureResumeToken()};
            var jsonToken = JsonConvert.SerializeObject(tokens);

            BsonDocument.TryParse(jsonToken,
                out var resumeTokenBson); // Get the resumetoken to start from where we left off
            return resumeTokenBson;
        }


        private async Task PublishToAzureServiceBus(string productId, string resumeToken)
        {
            try
            {
                Console.WriteLine($"Publishing to service bus {productId}");
                await _productService.UpdateStatusAsync(productId, true, resumeToken);
            }
            catch (Exception ex)
            {
               //polly retry
            }
        }
    }
}
