﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Talabat.APIs.Dtos;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IGenericRepository<ProductCategory> _categoriesRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo
            ,IGenericRepository<ProductBrand> brandsRepo
            ,IGenericRepository<ProductCategory> categoriesRepo
            , IMapper mapper )
        {
            _productsRepo = productsRepo;
            _brandsRepo = brandsRepo;
            _categoriesRepo = categoriesRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecsParams specParams)
         {
            var specs = new ProductWithBrandAndCategorySpecifications(specParams);
            var result = await _productsRepo.GetAllWithSpecsAsync(specs);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(result);
            var countSpecs = new ProductWithFilterationForCountSpecification(specParams);
            var count = await _productsRepo.GetPaginationCount(countSpecs);
            return Ok(new Pagination<ProductToReturnDto>(specParams.PageSize , specParams.PageIndex,data ,count));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(id);
            var resutl = await _productsRepo.GetWithSpecsAsync(specs);

            if (resutl == null) 
                return NotFound();
            return Ok(_mapper.Map<Product,ProductToReturnDto>(resutl));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandsRepo.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _categoriesRepo.GetAllAsync();
            return Ok(categories);
        }
    }
}
