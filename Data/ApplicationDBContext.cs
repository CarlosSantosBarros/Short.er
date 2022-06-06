using Microsoft.EntityFrameworkCore;
using Short.er.Models;

namespace Short.er.Data
{
    public class ApplicationDBContext :DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) 
        {

        }
        public DbSet<ShortenedUrl> Urls { get; set; }
    }
}
