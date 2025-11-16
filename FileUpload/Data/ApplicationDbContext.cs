using Microsoft.EntityFrameworkCore;
using FileUpload.Data.Entities;

namespace FileUpload.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
        public DbSet<Painting> Paintings{ get; set; } //Painting is the entity and Paintings is the table in the database
    }
}
