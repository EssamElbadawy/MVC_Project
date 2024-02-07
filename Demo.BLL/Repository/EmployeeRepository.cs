using System.Linq;
using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;

namespace Demo.BLL.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>,IEmployeeRepository
    {
        public EmployeeRepository(DataContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Employee> SearchEmployeesByName(string name)
        {
            return _dbContext.Employees.Where(e => e.Name.ToLower().Contains(name.ToLower()));
        }
    }
}
