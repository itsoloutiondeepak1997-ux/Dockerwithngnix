using DockerWithNgnix.Models;
using Microsoft.EntityFrameworkCore;

namespace DockerWithNgnix.DataLayer
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
                
        }

        public DbSet<Product> Products { get; set; }
    }
}
