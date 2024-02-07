using Demo.BLL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.BLL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private protected readonly DataContext _dbContext;

        protected GenericRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(Employee))
                return (IEnumerable<T>)await _dbContext.Set<Employee>()
                    .Include(e => e.Department)
                    .ToListAsync();
            return await _dbContext.Set<T>().ToListAsync();
        }


        public async Task<T> GetById(int id)
        {
            if (typeof(T) == typeof(Employee))
                return await _dbContext.Set<Employee>()
                    .Include(e => e.Department)
                    .FirstOrDefaultAsync(e => e.Id == id) as T;
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task Add(T item)
        {
            await _dbContext.Set<T>().AddAsync(item);
        }

        public void Update(T item)
        {
            _dbContext.Set<T>().Update(item);
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }
    }
}
