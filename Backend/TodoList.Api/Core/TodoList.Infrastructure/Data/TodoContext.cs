using Microsoft.EntityFrameworkCore;
using TodoList.Infrastructure.Data.Models;

namespace TodoList.Infrastructure.Data
{
    public interface ITodoContext
    {
        DbSet<TodoItem> TodoItems { get; }
    }
    
    public class TodoContext : DbContext, ITodoContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().HasData(Seed.Todos);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
