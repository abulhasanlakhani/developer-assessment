using Microsoft.EntityFrameworkCore;
using TodoList.Infrastructure.Data.Models;

namespace TodoList.Infrastructure.Data
{
    public class TodoContext : DbContext
    {
        //public TodoContext(DbContextOptions<TodoContext> options)
        //    : base(options)
        //{
        //}

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TodoItemsDB");
        }
    }
}
