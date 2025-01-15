using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.ProductSpecifications
{
    public class ProductSpecsParams
    {
        private const int MaxSize = 10; 
        public string? Sort { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }

        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxSize ? MaxSize : pageSize; }
        }

        public int PageIndex { get; set; } = 1;
    }
}