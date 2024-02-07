using System.Linq;
using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Models;

namespace Demo.BLL.Repository
{
    public class DepartmentRepository : GenericRepository<Department> , IDepartmentRepository
    {


        public DepartmentRepository(DataContext dbContext):base(dbContext)
        {
        }


        public IQueryable<Department> SearchDepartmentByName(string name)
        {
           return _dbContext.Departments.Where(d => d.Name.ToLower().Contains(name.ToLower()));
        }
    }
}
