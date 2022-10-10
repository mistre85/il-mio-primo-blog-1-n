using Microsoft.EntityFrameworkCore;
using NetCore.Models;
using NetCore_01.Models;

namespace NetCore.Data
{
    public class BlogContext : DbContext
    {

        public DbSet<Post> Posts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=il_mio_primo_blog_ef;Integrated Security=True");
        }

    }
}
