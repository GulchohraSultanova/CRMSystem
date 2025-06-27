using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using CRMSystem.Application.Absrtacts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CRMSystem.Persistence.Contexts;

namespace CRMSystem.Persistence.Concreters.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : class, new()
    {
        private readonly CRMSystemDbContext _CRMSystemDbContext;

        public ReadRepository(CRMSystemDbContext CRMSystemDbContext)
        {
            _CRMSystemDbContext = CRMSystemDbContext;
        }

        private DbSet<T> Table { get => _CRMSystemDbContext.Set<T>(); }




        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? func = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool EnableTraking = false)
        {
            IQueryable<T> query = Table;
            if (!EnableTraking) query = query.AsNoTracking();
            if (include != null) query = include(query); // Include kullanımı
            if (func != null) query = query.Where(func);
            if (orderBy != null)
                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id, bool enableTracking = false)
        {
            if (!Guid.TryParse(id, out Guid parsedId))
            {
                throw new KeyNotFoundException($"Yanlış ID formatı: {id}");
            }

            IQueryable<T> query = Table;

            if (!enableTracking)
            {
                query = query.AsNoTracking();
            }

            var entity = await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == parsedId); // ✅ Düzgün GUID müqayisəsi

            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            return entity;
        }

        public async Task<T> GetAsync(
           Expression<Func<T, bool>> predicate,
           Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
           Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
           bool enableTracking = false)
        {
            IQueryable<T> query = Table;

            if (!enableTracking)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            return await query.FirstOrDefaultAsync();
        }


        public Task<int> GetCountAsync(Expression<Func<T, bool>>? func = null)
        {
            Table.AsNoTracking();
            return Table.Where(func).CountAsync();
        }
        public IQueryable<T> GetQueryable()
        {
            return Table.AsQueryable();
        }



    }
}
