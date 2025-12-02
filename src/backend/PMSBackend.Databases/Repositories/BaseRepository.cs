using Microsoft.EntityFrameworkCore;
using PMSBackend.Databases.Data;
using PMSBackend.Domain.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace PMSBackend.Databases.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly PMSDbContext _context;
        private DbSet<T> table = null;
        public BaseRepository(PMSDbContext context)
        {
            try
            {
                _context = context;
                table = _context.Set<T>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Insert
        public async Task<T> AddAsync(T entity)
        {
            try
            {
                await table.AddAsync(entity);
                await _context.SaveChangesAsync();
                _context.Entry(entity).Reload();
                return entity;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Update
        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _context.Entry(entity).Reload();
                return entity;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Query       
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await table
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<T> GetDetailsByIdAsync(long id)
        {
            try
            {
                var noTrackingEntity = await GetByIdAsNoTrackingAsync(id);
                if (noTrackingEntity != null)
                {
                    return noTrackingEntity;
                }

                var result = await table.FindAsync(id);
                return result!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<T?> GetByIdAsNoTrackingAsync(long id)
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            if (entityType == null)
            {
                return null;
            }

            var primaryKey = entityType.FindPrimaryKey();
            if (primaryKey == null || primaryKey.Properties.Count != 1)
            {
                return null;
            }

            var keyProperty = primaryKey.Properties.First();
            var parameter = Expression.Parameter(typeof(T), "entity");
            var propertyAccess = Expression.Property(parameter, keyProperty.Name);

            object? keyValue;
            try
            {
                keyValue = Convert.ChangeType(id, keyProperty.ClrType);
            }
            catch
            {
                return null;
            }

            var equalsExpression = Expression.Equal(
                propertyAccess,
                Expression.Constant(keyValue, keyProperty.ClrType));

            var lambda = Expression.Lambda<Func<T, bool>>(equalsExpression, parameter);

            return await table
                .AsNoTracking()
                .FirstOrDefaultAsync(lambda);
        }

        // Delete
        public async Task DeleteAsync(long id)
        {
            try
            {
                var entity = await table.FindAsync(id);
                if (entity != null)
                {
                    table.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                _context.Dispose();
            }
            catch (Exception)
            {

                throw;
            }

        }


    }
}