using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> baseQuery, ISpecifications<TEntity> specs)
        {
            var query = baseQuery;
            if (specs.Criteria != null)
                query = query.Where(specs.Criteria);

            query = specs.Includes.Aggregate(query, (currentquery, queryexpression) => currentquery.Include(queryexpression));

            return query;
        }
    }
}
