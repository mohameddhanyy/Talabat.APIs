﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecsAsync(ISpecifications<T> specs)
        {
            return await ApplySpecification(specs).ToListAsync();
        }

        public async Task<T?> GetWithSpecsAsync(ISpecifications<T> specs)
        {
            return await ApplySpecification(specs).FirstOrDefaultAsync();

        }

        private IQueryable<T> ApplySpecification(ISpecifications<T> specs)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), specs); 
        }
    }
}
