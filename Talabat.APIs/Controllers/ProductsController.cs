using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Talabat.APIs.Dtos;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(
            IProductService productService,
            IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecsParams specParams)
        {
            var products = await _productService.GetProductsAsync(specParams);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var count = await _productService.GetCountAsync(specParams);
            return Ok(new Pagination<ProductToReturnDto>(specParams.PageSize, specParams.PageIndex, data, count));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var resutl = await _productService.GetProductAsync(id);

            if (resutl == null)
                return NotFound();
            return Ok(_mapper.Map<Product, ProductToReturnDto>(resutl));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _productService.GetCategoriesAsync();
            return Ok(categories);
        }
    }
}
