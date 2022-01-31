using Microsoft.EntityFrameworkCore;
using TodoList.Infrastructure.Data.Models;

namespace TodoList.Infrastructure.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();

        // Use Seed.TodoList or Seed.BlankTodoList or comment out OnModelCreating method
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().HasData(Seed.TodoList);

            base.OnModelCreating(modelBuilder);
        }
    }
}
