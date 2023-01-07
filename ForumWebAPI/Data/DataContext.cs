using Microsoft.EntityFrameworkCore;

namespace ForumWebAPI
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DataContext() { }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<User>()
                .HasKey(u => u.Email);
            modelBuilder.Entity<Post>()
                .HasKey(p => p.PostId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder oB){
            string connectionString = "Data Source=../Database/DBsql.db";
            oB.UseSqlite(connectionString);
        }
    }
}