using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecifications
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications(ProductSpecsParams specParams) :
            base(P =>
                    (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value) &&
                    (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)
            )
        {
            AddIncludes();
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;

                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;

                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else AddOrderBy(P => P.Name);

            EnablePaginaiton((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }


        public ProductWithBrandAndCategorySpecifications(int id) : base(P => P.Id == id )
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }

    }
}
