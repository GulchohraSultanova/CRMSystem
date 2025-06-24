using Microsoft.EntityFrameworkCore;
using CRMSystem.Application.Absrtacts.Repositories;
using CRMSystem.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMSystem.Persistence.Concreters.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : class, new()
    {
        private readonly CRMSystemDbContext _CRMSystemDbContext;

        public WriteRepository(CRMSystemDbContext CRMSystemDbContext)
        {
            _CRMSystemDbContext = CRMSystemDbContext;
        }

        private DbSet<T> Table { get => _CRMSystemDbContext.Set<T>(); }
        public async Task AddAsync(T entity)
        {
            await Table.AddAsync(entity);
        }

        public async Task HardDeleteAsync(T entity)
        {
            await Task.Run(() => Table.Remove(entity));

        }
        public async Task SoftDeleteAsync(T entity)
        {
            // Stub nesneyi attach et
            Table.Attach(entity);

            // Sadece IsDeleted alanını güncelle
            var entry = Table.Entry(entity);
            entry.Property("IsDeleted").CurrentValue = true;
            entry.Property("IsDeleted").IsModified = true;

            await Task.CompletedTask;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            await Task.Run(() => Table.Update(entity));
            return entity;
        }

        public async Task<int> CommitAsync()
        {
            return await _CRMSystemDbContext.SaveChangesAsync();
        }
    }
}
