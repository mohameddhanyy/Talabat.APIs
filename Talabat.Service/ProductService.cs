using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductSpecifications;

namespace Talabat.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecsParams specParams)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(specParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecsAsync(specs);
            return products;
        }

        public async Task<Product?> GetProductAsync(int productId)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(productId);
            var product = await _unitOfWork.Repository<Product>().GetWithSpecsAsync(specs);
            return product;
        }

        public async Task<int> GetCountAsync(ProductSpecsParams specsParams)
        {
            var countSpecs = new ProductWithFilterationForCountSpecification(specsParams);
            var count = await _unitOfWork.Repository<Product>().GetPaginationCount(countSpecs);
            return count;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()=> 
            await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

        public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync() =>
            await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
       
    }
}
