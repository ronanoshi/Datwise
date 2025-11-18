using Microsoft.EntityFrameworkCore;
using Datwise.Models;

namespace Datwise.Data
{
    public class DatwiseDbContext : DbContext
    {
        public DatwiseDbContext(DbContextOptions<DatwiseDbContext> options) : base(options) { }

        public DbSet<ExampleModel> Examples { get; set; } = null!;

        // Add more DbSets for your tables here
    }
}
