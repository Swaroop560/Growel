using GMSAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace GMSAPI.DataContext
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {
            
        }

        public DbSet<EmployeeRoot> EmployeeRoots { get; set; }
        public DbSet<Department> Departments { get; set; }

    }
}

