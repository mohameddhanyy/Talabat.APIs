﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public Expression<Func<T, object>>? OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get ; set; }
        public bool IsEnabledPaginaiton { get; set; }

        public BaseSpecifications()
        {
        }
        public BaseSpecifications(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }
        public void AddOrderByDesc(Expression<Func<T, object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }

        public void EnablePaginaiton(int skip , int take)
        {
            IsEnabledPaginaiton = true;
            Skip = skip;
            Take = take;
        }
    }
}
