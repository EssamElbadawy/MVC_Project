using System.Linq;
using Demo.DAL.Models;

namespace Demo.BLL.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        IQueryable<Department> SearchDepartmentByName(string name);
    }
}
