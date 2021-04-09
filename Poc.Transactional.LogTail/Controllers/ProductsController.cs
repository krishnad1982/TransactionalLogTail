using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Poc.LogTail.Core.Services.Contracts;
using Poc.LogTail.Core.ViewModels.RequestModels;

namespace Poc.Transactional.LogTail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductRequestModel product)
        {
            await _productService.CreateProductAsync(product);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProductRequestModel product)
        {
            await _productService.UpdateProductAsync(product);
            return Ok();
        }
    }
}
