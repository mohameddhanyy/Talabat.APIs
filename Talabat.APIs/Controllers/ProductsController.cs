using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;

        public ProductsController(IGenericRepository<Product> productsRepo)
        {
            _productsRepo = productsRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var specs = new ProductWithBrandAndCategorySpecifications();
            var result = await _productsRepo.GetAllWithSpecsAsync(specs);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(id);
            var resutl = await _productsRepo.GetWithSpecsAsync(specs);

            if (resutl == null) 
                return NotFound();
            return Ok(resutl);
        }
    }
}
