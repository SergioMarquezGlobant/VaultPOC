using Microsoft.EntityFrameworkCore;
using VaultPOC.Data.Models;

namespace VaultPOC.Data
{
    public class TodoItemsDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public TodoItemsDbContext(DbContextOptions<TodoItemsDbContext> options) : base(options)
        {
        }
    }
}
