using System.Linq;
using Demo.DAL.Models;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IQueryable<Employee> SearchEmployeesByName(string name);
    }
}
