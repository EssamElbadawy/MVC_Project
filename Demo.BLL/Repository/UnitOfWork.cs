using System.Threading.Tasks;
using Demo.BLL.Interfaces;
using Demo.DAL.Context;

namespace Demo.BLL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dbContext;
        public IDepartmentRepository DepartmentRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }

        public UnitOfWork(DataContext dbContext)
        {
            _dbContext = dbContext;
            DepartmentRepository = new DepartmentRepository(dbContext);
            EmployeeRepository = new EmployeeRepository(dbContext);
        }
        public async Task<int> Complete()
            => await _dbContext.SaveChangesAsync();



        public void Dispose()
        => _dbContext.Dispose();
        
    }
}
