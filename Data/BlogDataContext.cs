using Blog.Data.Mappings;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog.Data
{
    public class BlogDataContext : DbContext
    {
        public IConfiguration _configuration;
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        // public DbSet<PostTag> PostTags { get; set; }
        // public DbSet<Role> Roles { get; set; }
        // public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        // public DbSet<UserRole> UserRoles { get; set; }

        public BlogDataContext(IConfiguration configuration) => _configuration = configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new PostMap());
        }
    }
}