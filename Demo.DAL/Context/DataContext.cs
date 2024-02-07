using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Context
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
            :base(options)
        {
            
        }
 

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

     


    } 
}
